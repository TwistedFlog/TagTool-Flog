using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TagTool.Cache;
using TagTool.Bitmaps;
using TagTool.Bitmaps.DDS;
using TagTool.Bitmaps.Utils;
using TagTool.BlamFile;
using TagTool.Commands.Common;
using TagTool.IO;
using TagTool.Tags;
using TagTool.Tags.Definitions;
using TagTool.Common;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace TagTool.Commands.Bitmaps
{
    internal class ReencodeBitmapCommand : Command
    {
        private GameCache Cache { get; }
        private CachedTag Tag { get; }
        private Bitmap Bitmap { get; }

        public ReencodeBitmapCommand(GameCache cache, CachedTag tag, Bitmap bitmap)
            : base(false,
                  "ReencodeBitmap",
                  "Re-encodes the current bitmap as the specified format.",
                  "ReencodeBitmap <format> [image index]",
                  $"Valid format types are: {string.Join(", ", Enum.GetNames(typeof(BitmapFormat)))}\n")
        {
            Cache = cache;
            Tag = tag;
            Bitmap = bitmap;
        }

        public override object Execute(List<string> args)
        {
            // Parse arguments.
            if (!Enum.TryParse<BitmapFormat>(args[0], out BitmapFormat destFormat))
                return new TagToolError(CommandError.ArgInvalid, $"\"{args[0]}\" is not a valid bitmap format");

            int imageIndex = 0;
            if (args.Count > 1 && (!int.TryParse(args[1], out imageIndex) || imageIndex >= Bitmap.Images.Count))
                return new TagToolError(CommandError.ArgInvalid, $"\"{args[1]}\" is not a valid image index");

            var resourceReference = Bitmap.HardwareTextures[imageIndex];
            var resourceDefinition = Cache.ResourceCache.GetBitmapTextureInteropResource(resourceReference);

            byte[] primaryData = resourceDefinition?.Texture.Definition.PrimaryResourceData.Data;
            byte[] secondaryData = resourceDefinition?.Texture.Definition.SecondaryResourceData.Data;
            BitmapFormat currentFormat = Bitmap.Images[imageIndex].Format;

            // --- DXN conversion branch ---
            if (destFormat == BitmapFormat.Dxn)
            {
                // Ensure the current image is in one of the supported DXT formats.
                switch (currentFormat)
                {
                    case BitmapFormat.Dxt1:
                    case BitmapFormat.Dxt3:
                    case BitmapFormat.Dxt5:
                        break;
                    default:
                        return new TagToolError(CommandError.OperationFailed, "DXN conversion requires a normal map in a DXT format");
                }

                // Extract only the highest-resolution mip level (mip level 0)
                int mipLevel = 0;
                int pixelDataOffset = BitmapUtilsPC.GetTextureOffset(Bitmap.Images[imageIndex], mipLevel);
                int pixelDataSize = BitmapUtilsPC.GetMipmapPixelDataSize(Bitmap.Images[imageIndex], mipLevel);
                byte[] highestResData = new byte[pixelDataSize];

                if ((mipLevel == 0 && resourceDefinition.Texture.Definition.Bitmap.HighResInSecondaryResource > 0) || primaryData == null)
                {
                    Array.Copy(secondaryData, pixelDataOffset, highestResData, 0, pixelDataSize);
                }
                else
                {
                    if (secondaryData != null)
                        pixelDataOffset -= secondaryData.Length;
                    Array.Copy(primaryData, pixelDataOffset, highestResData, 0, pixelDataSize);
                }

                // Decode the compressed data from mip level 0.
                byte[] uncompressed = BitmapDecoder.DecodeBitmap(highestResData, currentFormat, Bitmap.Images[imageIndex].Width, Bitmap.Images[imageIndex].Height);

                // Re-encode the uncompressed data as DXN (BC5)
                byte[] dxnData = BitmapDecoder.EncodeBitmap(uncompressed, BitmapFormat.Dxn, Bitmap.Images[imageIndex].Width, Bitmap.Images[imageIndex].Height);

                // Update the bitmap image and resource.
                BaseBitmap resultBitmap = new BaseBitmap(Bitmap.Images[imageIndex]);
                resultBitmap.UpdateFormat(BitmapFormat.Dxn);
                resultBitmap.Data = dxnData;
                resultBitmap.MipMapCount = 0;

                if (!BitmapUtils.IsCompressedFormat(resultBitmap.Format))
                    resultBitmap.Flags &= ~BitmapFlags.Compressed;
                else
                    resultBitmap.Flags |= BitmapFlags.Compressed;

                BitmapUtils.SetBitmapImageData(resultBitmap, Bitmap.Images[imageIndex]);
                var bitmapResourceDefinition = BitmapUtils.CreateBitmapTextureInteropResource(resultBitmap);
                var newResourceReference = Cache.ResourceCache.CreateBitmapResource(bitmapResourceDefinition);

                Bitmap.HardwareTextures.Clear();
                Bitmap.HardwareTextures.Add(newResourceReference);
                Bitmap.InterleavedHardwareTextures = new List<TagResourceReference>();

                return true;
            }
            // --- End DXN branch ---

            // Non-DXN branch (existing behavior)
            int mipCount = Bitmap.Images[imageIndex].MipmapCount;
            byte[] bitmapData;
            using (var result = new MemoryStream())
            {
                for (int mipLevel = 0; mipLevel < mipCount; mipLevel++)
                {
                    for (int layerIndex = 0; layerIndex < Bitmap.Images[imageIndex].Depth; layerIndex++)
                    {
                        int pixelDataOffset = BitmapUtilsPC.GetTextureOffset(Bitmap.Images[imageIndex], mipLevel);
                        int pixelDataSize = BitmapUtilsPC.GetMipmapPixelDataSize(Bitmap.Images[imageIndex], mipLevel);
                        byte[] pixelData = new byte[pixelDataSize];

                        if ((mipLevel == 0 && resourceDefinition.Texture.Definition.Bitmap.HighResInSecondaryResource > 0) || primaryData == null)
                        {
                            Array.Copy(secondaryData, pixelDataOffset, pixelData, 0, pixelData.Length);
                        }
                        else
                        {
                            if (secondaryData != null)
                                pixelDataOffset -= secondaryData.Length;
                            Array.Copy(primaryData, pixelDataOffset, pixelData, 0, pixelDataSize);
                        }
                        result.Write(pixelData, 0, pixelData.Length);
                    }
                }
                bitmapData = result.ToArray();
            }

            byte[] rawData = BitmapDecoder.DecodeBitmap(bitmapData, Bitmap.Images[imageIndex].Format, Bitmap.Images[imageIndex].Width, Bitmap.Images[imageIndex].Height);
            rawData = BitmapDecoder.EncodeBitmap(rawData, destFormat, Bitmap.Images[imageIndex].Width, Bitmap.Images[imageIndex].Height);

            BaseBitmap resultBitmapNonDXN = new BaseBitmap(Bitmap.Images[imageIndex]);
            resultBitmapNonDXN.UpdateFormat(destFormat);
            resultBitmapNonDXN.Data = rawData;

            if (!BitmapUtils.IsCompressedFormat(resultBitmapNonDXN.Format))
                resultBitmapNonDXN.Flags &= ~BitmapFlags.Compressed;
            else
                resultBitmapNonDXN.Flags |= BitmapFlags.Compressed;

            BitmapUtils.SetBitmapImageData(resultBitmapNonDXN, Bitmap.Images[imageIndex]);
            var bitmapResourceDefinitionNonDXN = BitmapUtils.CreateBitmapTextureInteropResource(resultBitmapNonDXN);
            var newResourceReferenceNonDXN = Cache.ResourceCache.CreateBitmapResource(bitmapResourceDefinitionNonDXN);

            Bitmap.HardwareTextures.Clear();
            Bitmap.HardwareTextures.Add(newResourceReferenceNonDXN);
            Bitmap.InterleavedHardwareTextures = new List<TagResourceReference>();

            return true;
        }
    }
}
