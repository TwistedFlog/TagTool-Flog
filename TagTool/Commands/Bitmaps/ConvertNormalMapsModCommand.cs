using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TagTool.Cache;
using TagTool.Cache.HaloOnline;
using TagTool.Bitmaps;
using TagTool.Bitmaps.DDS;
using TagTool.Bitmaps.Utils;
using TagTool.Commands.Common;
using TagTool.IO;
using TagTool.Tags;
using TagTool.Tags.Definitions;
using TagTool.Common;

namespace TagTool.Commands.Mod
{
    internal class ConvertNormalMapsModCommand : Command
    {
        private GameCache Cache { get; }

        public ConvertNormalMapsModCommand(GameCache cache)
            : base(true,
                   "ConvertNormalMaps",
                   "Re-encodes all normal-map bitmaps (suffix _zbump, _normal, _bump, _microbump, _n) from DXN to DXT1 Linear, including all mip levels, skipping broken mips and user-specified filters.",
                   "ConvertNormalMaps [skipSubstring1] [skipSubstring2] ...",
                   "Processes every bitmap tag in the current mod package whose filename ends with a normal-map suffix (unless its name contains one of the skip substrings provided) and re-encodes each mipmap chain from DXN (BC5) to DXT1 (BC1) with a linear curve, skipping any corrupted mip levels and tags which aren't DXN.")
        {
            Cache = cache;
        }

        public override object Execute(List<string> args)
        {
            // Suffixes to detect normal maps
            string[] suffixes = { "_zbump", "_normal", "_normals", "_bump", "_microbump", "_n" };
            // Substrings to skip (from args)
            var skipSubstrings = args.Select(a => a.ToLowerInvariant()).ToList();

            var tags = Cache.TagCache.TagTable
                .OfType<CachedTagHaloOnline>()
                .Where(t => !t.IsEmpty())
                .Where(t => t.Group.ToString().Equals("bitmap", StringComparison.OrdinalIgnoreCase))
                .ToList();

            foreach (var tag in tags)
            {
                string name = (tag.Name ?? tag.ToString()).ToLowerInvariant();

                // Skip if not a normal-map suffix
                if (!suffixes.Any(s => name.EndsWith(s)))
                    continue;
                // Skip if name contains any user-specified filter
                if (skipSubstrings.Any(filter => name.Contains(filter)))
                {
                    Console.WriteLine($"Skipping filtered bitmap: {tag.Name}");
                    continue;
                }

                // Load bitmap definition
                object def;
                using (var rs = Cache.OpenCacheRead())
                    def = Cache.Deserialize(rs, tag);
                var bitmap = def as Bitmap;
                if (bitmap == null)
                {
                    Console.WriteLine($"Skipping non-bitmap tag {tag.Name}");
                    continue;
                }

                // Only process tags whose images are stored as DXN
                if (!bitmap.Images.Any(img => img.Format == BitmapFormat.Dxn))
                {
                    Console.WriteLine($"Skipping {tag.Name}, not in DXN format");
                    continue;
                }

                Console.WriteLine($"Converting normal map: {tag.Name}");
                var newResources = new List<TagResourceReference>();

                // Process each image (mip chains) in the bitmap
                for (int i = 0; i < bitmap.Images.Count; i++)
                {
                    var image = bitmap.Images[i];
                    var hwRef = bitmap.HardwareTextures[i];
                    var resDef = Cache.ResourceCache.GetBitmapTextureInteropResource(hwRef);

                    int width = image.Width;
                    int height = image.Height;
                    int maxLevels = image.MipmapCount + 1;

                    byte[] primaryData = resDef.Texture.Definition.PrimaryResourceData.Data;
                    byte[] secondaryData = resDef.Texture.Definition.SecondaryResourceData.Data;
                    byte[] rawCombined = (primaryData != null && primaryData.Length > 0)
                        ? primaryData
                        : (secondaryData ?? Array.Empty<byte>());

                    int offset = 0;
                    var encodedMips = new List<byte[]>();

                    // Re-encode each mip level until break
                    for (int mip = 0; mip < maxLevels; mip++)
                    {
                        int w = Math.Max(1, width >> mip);
                        int h = Math.Max(1, height >> mip);
                        int blockWidth = (w + 3) / 4;
                        int blockHeight = (h + 3) / 4;
                        int dxnBlockSize = 16;
                        int dataSize = blockWidth * blockHeight * dxnBlockSize;

                        try
                        {
                            if (offset + dataSize > rawCombined.Length)
                                throw new Exception("data length mismatch");

                            var dxnData = new byte[dataSize];
                            Array.Copy(rawCombined, offset, dxnData, 0, dataSize);
                            offset += dataSize;

                            var raw = BitmapDecoder.DecodeBitmap(dxnData, BitmapFormat.Dxn, w, h);
                            var dxt1 = BitmapDecoder.EncodeBitmap(raw, BitmapFormat.Dxt1, w, h);
                            encodedMips.Add(dxt1);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Warning: skipping broken mip {mip} for {tag.Name}: {ex.Message}");
                            break;
                        }
                    }

                    // If nothing re-encoded, keep original
                    if (encodedMips.Count == 0)
                    {
                        Console.WriteLine($"No valid mips for {tag.Name}, keeping original resource.");
                        newResources.Add(hwRef);
                        continue;
                    }

                    // Concatenate re-encoded mips
                    int totalSize = encodedMips.Sum(m => m.Length);
                    var combined = new byte[totalSize];
                    int dst = 0;
                    foreach (var m in encodedMips)
                    {
                        Array.Copy(m, 0, combined, dst, m.Length);
                        dst += m.Length;
                    }

                    // Build and store new resource
                    var newBitmap = new BaseBitmap(image)
                    {
                        Format = BitmapFormat.Dxt1,
                        Data = combined,
                        MipMapCount = encodedMips.Count - 1
                    };
                    newBitmap.UpdateFormat(BitmapFormat.Dxt1);
                    BitmapUtils.SetBitmapImageData(newBitmap, image);
                    var newResDef = BitmapUtils.CreateBitmapTextureInteropResource(newBitmap);
                    var newRef = Cache.ResourceCache.CreateBitmapResource(newResDef);

                    newResources.Add(newRef);
                }

                // Replace and serialize
                bitmap.HardwareTextures = newResources;
                bitmap.InterleavedHardwareTextures.Clear();
                using (var ws = Cache.OpenCacheReadWrite())
                    Cache.Serialize(ws, tag, bitmap);
            }

            Console.WriteLine("Done converting all normal-map bitmaps.");
            return true;
        }
    }
}
