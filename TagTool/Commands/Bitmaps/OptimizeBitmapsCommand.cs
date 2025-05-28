using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TagTool.Cache;
using TagTool.Cache.HaloOnline;
using TagTool.Bitmaps;
using TagTool.Bitmaps.Utils;
using TagTool.Commands.Common;
using TagTool.IO;
using TagTool.Tags;
using TagTool.Tags.Definitions;
using TagTool.Common;

namespace TagTool.Commands.Mod
{
    internal class OptimizeBitmapsCommand : Command
    {
        private GameCache Cache { get; }

        public OptimizeBitmapsCommand(GameCache cache)
            : base(true,
                   "OptimizeBitmaps",
                   "Trims, converts, and unlinks unused bitmap resources.",
                   "OptimizeBitmaps [skip...] [bitmaps] [normalmaps] [cutmips] [lightmaps]",
                   "- bitmaps: convert A8 formats to DXT5.\n" +
                   "- normalmaps: convert DXN normals to DXT1.\n" +
                   "- cutmips: truncate all chains to 3 mips.\n" +
                   "- lightmaps: for tags with 'lightmap', keep only base level.\n" +
                   "- skips: substrings of tags to ignore.\n" +
                   "- cubemap tags never processed.")
        {
            Cache = cache;
        }

        private static bool IsPowerOfTwo(int x) => x >= 2 && (x & (x - 1)) == 0;

        public override object Execute(List<string> args)
        {
            // Feature flags
            bool convA8 = args.Any(a => a.Equals("bitmaps", StringComparison.OrdinalIgnoreCase));
            bool convDxn = args.Any(a => a.Equals("normalmaps", StringComparison.OrdinalIgnoreCase));
            bool trim3 = args.Any(a => a.Equals("cutmips", StringComparison.OrdinalIgnoreCase));
            bool doLight = args.Any(a => a.Equals("lightmaps", StringComparison.OrdinalIgnoreCase));

            // Build skips excluding feature flags
            var flags = new[] { "bitmaps", "normalmaps", "cutmips", "lightmaps" };
            var skips = args
                .Where(a => !flags.Contains(a.ToLowerInvariant()))
                .Select(a => a.ToLowerInvariant())
                .ToList();

            var tags = Cache.TagCache.TagTable
                .OfType<CachedTagHaloOnline>()
                .Where(t => !t.IsEmpty() && t.Group.ToString().Equals("bitmap", StringComparison.OrdinalIgnoreCase))
                .ToList();

            foreach (var tag in tags)
            {
                var name = tag.Name ?? tag.ToString();
                var lower = name.ToLowerInvariant();

                // skip by user filters, always skip cubemaps
                if (skips.Any(s => lower.Contains(s)) || lower.Contains("cubemap") ||
                    (lower.Contains("lightmap") && !doLight))
                {
                    Console.WriteLine($"Skipping tag: {name}");
                    continue;
                }

                Console.WriteLine($"Processing tag: {name}");
                Bitmap bmp;
                using (var rs = Cache.OpenCacheRead())
                    bmp = Cache.Deserialize(rs, tag) as Bitmap;
                if (bmp == null)
                {
                    Console.WriteLine($"Skipping non-bitmap: {name}");
                    continue;
                }

                var original = bmp.HardwareTextures.ToList();
                var newList = new List<TagResourceReference>();

                bool isLightTag = lower.Contains("lightmap");

                for (int i = 0; i < bmp.Images.Count; i++)
                {
                    var img = bmp.Images[i];
                    var oldRef = bmp.HardwareTextures[i];
                    var res = Cache.ResourceCache.GetBitmapTextureInteropResource(oldRef);
                    var data = res.Texture.Definition.PrimaryResourceData.Data
                               ?? res.Texture.Definition.SecondaryResourceData.Data
                               ?? Array.Empty<byte>();

                    int levels = img.MipmapCount + 1;

                    // for lightmaps: keep only base
                    if (doLight && isLightTag)
                    {
                        Console.WriteLine($"Lightmap trim for {name}[{i}]");
                        levels = 1;
                    }
                    else if (trim3)
                    {
                        if (levels > 3)
                        {
                            Console.WriteLine($"Trimming to 3 mips: {name}[{i}]");
                            levels = 3;
                        }
                    }

                    bool a8 = img.Format == BitmapFormat.A8R8G8B8;
                    bool a8y = img.Format == BitmapFormat.A8Y8;
                    bool dxn = img.Format == BitmapFormat.Dxn;

                    bool doA8 = convA8 && (a8 || a8y) && IsPowerOfTwo(img.Width) && IsPowerOfTwo(img.Height);
                    bool doDXN = convDxn && dxn;

                    if (doA8 || doDXN)
                    {
                        var dst = doDXN ? BitmapFormat.Dxt1 : BitmapFormat.Dxt5;
                        Console.WriteLine($"Converting {name}[{i}] to {dst}, levels={levels}");
                        int offset = 0;
                        var outM = new List<byte[]>();

                        for (int lvl = 0; lvl < levels; lvl++)
                        {
                            int w = Math.Max(1, img.Width >> lvl);
                            int h = Math.Max(1, img.Height >> lvl);

                            if (dxn && doDXN)
                            {
                                int sz = ((w + 3) / 4) * ((h + 3) / 4) * 16;
                                if (offset + sz > data.Length) break;
                                var slice = new byte[sz];
                                Array.Copy(data, offset, slice, 0, sz);
                                offset += sz;
                                var raw = BitmapDecoder.DecodeBitmap(slice, BitmapFormat.Dxn, w, h);
                                outM.Add(BitmapDecoder.EncodeBitmap(raw, dst, w, h));
                            }
                            else if (a8 && doA8)
                            {
                                int sz = 4 * w * h;
                                if (offset + sz > data.Length) break;
                                var rawPixels = new byte[sz];
                                Array.Copy(data, offset, rawPixels, 0, sz);
                                offset += sz;
                                outM.Add(BitmapDecoder.EncodeBitmap(rawPixels, dst, w, h));
                            }
                            else if (a8y && doA8)
                            {
                                int sz = 2 * w * h;
                                if (offset + sz > data.Length) break;
                                var slice = new byte[sz];
                                Array.Copy(data, offset, slice, 0, sz);
                                offset += sz;
                                var raw = BitmapDecoder.DecodeBitmap(slice, BitmapFormat.A8Y8, w, h);
                                outM.Add(BitmapDecoder.EncodeBitmap(raw, dst, w, h));
                            }
                        }

                        if (outM.Count == 0)
                        {
                            Console.WriteLine($"No valid mips, keeping original: {name}[{i}]");
                            newList.Add(oldRef);
                        }
                        else
                        {
                            var all = outM.SelectMany(b => b).ToArray();
                            var nb = new BaseBitmap(img)
                            {
                                Format = dst,
                                Data = all,
                                MipMapCount = outM.Count - 1
                            };
                            nb.UpdateFormat(dst);
                            BitmapUtils.SetBitmapImageData(nb, img);
                            var nr = BitmapUtils.CreateBitmapTextureInteropResource(nb);
                            newList.Add(Cache.ResourceCache.CreateBitmapResource(nr));
                        }
                    }
                    else
                    {
                        // no conversion, but mips may have been trimmed above
                        if (levels != img.MipmapCount + 1)
                        {
                            Console.WriteLine($"Applying cut-only: {name}[{i}] levels={levels}");
                            int total = 0;
                            for (int l = 0; l < levels; l++)
                            {
                                int w = Math.Max(1, img.Width >> l);
                                int h = Math.Max(1, img.Height >> l);
                                total += (dxn ? 16 * ((w + 3) / 4) * ((h + 3) / 4) : ((a8y ? 2 : 4) * w * h));
                            }
                            var len = Math.Min(total, data.Length);
                            var cut = new byte[len];
                            Array.Copy(data, 0, cut, 0, len);
                            var cb = new BaseBitmap(img)
                            {
                                Format = img.Format,
                                Data = cut,
                                MipMapCount = levels - 1
                            };
                            cb.UpdateFormat(img.Format);
                            BitmapUtils.SetBitmapImageData(cb, img);
                            var cr = BitmapUtils.CreateBitmapTextureInteropResource(cb);
                            newList.Add(Cache.ResourceCache.CreateBitmapResource(cr));
                        }
                        else
                        {
                            newList.Add(oldRef);
                        }
                    }
                }

                bmp.HardwareTextures = newList;
                bmp.InterleavedHardwareTextures.Clear();
                using (var ws = Cache.OpenCacheReadWrite())
                    Cache.Serialize(ws, tag, bmp);

                // any original refs not in newList are now unreferenced for cleanup
            }

            Console.WriteLine("Done optimizing bitmaps.");
            return true;
        }
    }
}
