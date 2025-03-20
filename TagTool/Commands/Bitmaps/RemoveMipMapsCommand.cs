using System;
using System.Collections.Generic;
using System.IO;
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
    internal class RemoveMipMapsCommand : Command
    {
        private GameCache Cache { get; }
        private CachedTag Tag { get; }
        private Bitmap Bitmap { get; }

        public RemoveMipMapsCommand(GameCache cache, CachedTag tag, Bitmap bitmap)
            : base(false,
                  "RemoveMipMaps",
                  "Removes all mipmaps from every image in the current bitmap, leaving only the highest resolution image for each.",
                  "RemoveMipMaps",
                  "Reencodes each bitmap image using its original format with only mip level 0, reducing overall file size.")
        {
            Cache = cache;
            Tag = tag;
            Bitmap = bitmap;
        }

        public override object Execute(List<string> args)
        {
            // We'll update each image in the bitmap file.
            List<TagResourceReference> newHardwareTextures = new List<TagResourceReference>();

            // Process every image within the bitmap.
            for (int imageIndex = 0; imageIndex < Bitmap.Images.Count; imageIndex++)
            {
                var image = Bitmap.Images[imageIndex];

                // Get the hardware texture resource corresponding to this image.
                var resourceReference = Bitmap.HardwareTextures[imageIndex];
                var resourceDefinition = Cache.ResourceCache.GetBitmapTextureInteropResource(resourceReference);

                // Retrieve the primary and secondary data buffers.
                byte[] primaryData = resourceDefinition?.Texture.Definition.PrimaryResourceData.Data;
                byte[] secondaryData = resourceDefinition?.Texture.Definition.SecondaryResourceData.Data;
                BitmapFormat currentFormat = image.Format;
                int width = image.Width;
                int height = image.Height;

                // Extract only mip level 0 (highest resolution) for all layers.
                byte[] mip0Data;
                using (var result = new MemoryStream())
                {
                    int mipLevel = 0;
                    for (int layerIndex = 0; layerIndex < image.Depth; layerIndex++)
                    {
                        int pixelDataOffset = BitmapUtilsPC.GetTextureOffset(image, mipLevel);
                        int pixelDataSize = BitmapUtilsPC.GetMipmapPixelDataSize(image, mipLevel);
                        byte[] pixelData = new byte[pixelDataSize];

                        // Determine which resource data to use.
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
                    mip0Data = result.ToArray();
                }

                // Create a new bitmap image entry based on the existing one.
                BaseBitmap resultBitmap = new BaseBitmap(image);
                resultBitmap.UpdateFormat(currentFormat);
                resultBitmap.Data = mip0Data;
                resultBitmap.MipMapCount = 0; // Remove all mipmaps

                // Update compression flags.
                if (!BitmapUtils.IsCompressedFormat(resultBitmap.Format))
                    resultBitmap.Flags &= ~BitmapFlags.Compressed;
                else
                    resultBitmap.Flags |= BitmapFlags.Compressed;

                // Update the image's metadata.
                BitmapUtils.SetBitmapImageData(resultBitmap, image);

                // Create a new resource for the updated image.
                var newBitmapResourceDefinition = BitmapUtils.CreateBitmapTextureInteropResource(resultBitmap);
                var newResourceReference = Cache.ResourceCache.CreateBitmapResource(newBitmapResourceDefinition);

                newHardwareTextures.Add(newResourceReference);
            }

            // Replace the hardware textures with the new ones for all images.
            Bitmap.HardwareTextures = newHardwareTextures;
            Bitmap.InterleavedHardwareTextures = new List<TagResourceReference>();

            return true;
        }
    }
}
