﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TagTool.Cache;
using TagTool.Cache.HaloOnline; // For CachedTagHaloOnline and its IsEmpty() method.
using TagTool.Bitmaps;
using TagTool.Bitmaps.DDS;
using TagTool.Bitmaps.Utils;
using TagTool.BlamFile;
using TagTool.Commands.Common;
using TagTool.IO;
using TagTool.Tags;
using TagTool.Tags.Definitions;
using TagTool.Common;

namespace TagTool.Commands.Mod
{
    internal class RemoveMipMapsModCommand : Command
    {
        private GameCache Cache { get; }

        public RemoveMipMapsModCommand(GameCache cache)
            : base(true,
                  "RemoveModMipMaps",
                  "Removes all mipmaps from every bitmap tag in the current mod package (except for blacklisted ones), leaving only the highest resolution images.",
                  "RemoveModMipMaps [blacklist_substring1] [blacklist_substring2] ...",
                  "Processes all bitmap tags in the mod package (excluding base cache tags and any tags whose names contain one of the specified blacklist substrings) and removes extra mipmap levels to reduce overall file sizes.")
        {
            Cache = cache;
        }

        public override object Execute(List<string> args)
        {
            // Convert each provided argument to lowercase and normalize backslashes to forward slashes.
            List<string> blacklistedSubstrings = args
                .Select(a => a.ToLowerInvariant().Replace('\\', '/'))
                .ToList();

            // Only process tags that are part of the mod package.
            var bitmapTags = new List<CachedTag>();
            foreach (var tag in Cache.TagCache.TagTable)
            {
                // Only consider mod package tags.
                if (tag is CachedTagHaloOnline haloTag)
                {
                    if (haloTag.IsEmpty())
                        continue;
                }
                else
                {
                    // Not a mod package tag.
                    continue;
                }

                // Only add bitmap tags (group identifier "bitmap").
                if (!IsBitmapTag(tag))
                    continue;

                // Normalize the tag's full name.
                string tagFullName = (tag.Name ?? tag.ToString()).Replace('\\', '/').ToLowerInvariant();

                // Check which blacklist substrings match.
                var matchingSubstrings = blacklistedSubstrings.Where(bl => tagFullName.Contains(bl)).ToList();
                if (matchingSubstrings.Any())
                {
                    Console.WriteLine($"Skipping blacklisted bitmap tag: {tagFullName} (matches: {string.Join(", ", matchingSubstrings)})");
                    continue;
                }

                bitmapTags.Add(tag);
            }

            if (bitmapTags.Count == 0)
            {
                Console.WriteLine("No bitmap tags found in the mod package.");
                return true;
            }

            foreach (var tag in bitmapTags)
            {
                // Deserialize the tag definition.
                object tagDefinition;
                using (var readStream = Cache.OpenCacheRead())
                {
                    tagDefinition = Cache.Deserialize(readStream, tag);
                }

                // Cast to Bitmap (ensure this matches your project’s type).
                var bitmap = tagDefinition as Bitmap;
                if (bitmap == null)
                {
                    Console.WriteLine($"Tag {tag.Name} is not a valid bitmap. Skipping.");
                    continue;
                }

                // Process each image in the bitmap.
                var newHardwareTextures = new List<TagResourceReference>();
                for (int imageIndex = 0; imageIndex < bitmap.Images.Count; imageIndex++)
                {
                    var image = bitmap.Images[imageIndex];
                    var resourceReference = bitmap.HardwareTextures[imageIndex];
                    var resourceDefinition = Cache.ResourceCache.GetBitmapTextureInteropResource(resourceReference);

                    byte[] primaryData = resourceDefinition?.Texture.Definition.PrimaryResourceData.Data;
                    byte[] secondaryData = resourceDefinition?.Texture.Definition.SecondaryResourceData.Data;
                    BitmapFormat currentFormat = image.Format;
                    int width = image.Width;
                    int height = image.Height;

                    // Extract only mip level 0 (highest resolution) for each layer.
                    byte[] mip0Data;
                    using (var ms = new MemoryStream())
                    {
                        int mipLevel = 0;
                        for (int layerIndex = 0; layerIndex < image.Depth; layerIndex++)
                        {
                            int pixelDataOffset = BitmapUtilsPC.GetTextureOffset(image, mipLevel);
                            int pixelDataSize = BitmapUtilsPC.GetMipmapPixelDataSize(image, mipLevel);
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
                            ms.Write(pixelData, 0, pixelData.Length);
                        }
                        mip0Data = ms.ToArray();
                    }

                    // In this version we directly use the mip0Data without full decode/encode to save time.
                    // (If you require a full re-encode, you can uncomment the following two lines:)
                    // byte[] rawData = BitmapDecoder.DecodeBitmap(mip0Data, currentFormat, width, height);
                    // mip0Data = BitmapDecoder.EncodeBitmap(rawData, currentFormat, width, height);

                    // Create an updated image entry.
                    BaseBitmap resultBitmap = new BaseBitmap(image);
                    resultBitmap.UpdateFormat(currentFormat);
                    resultBitmap.Data = mip0Data;
                    resultBitmap.MipMapCount = 0; // Remove extra mipmaps

                    if (!BitmapUtils.IsCompressedFormat(resultBitmap.Format))
                        resultBitmap.Flags &= ~BitmapFlags.Compressed;
                    else
                        resultBitmap.Flags |= BitmapFlags.Compressed;

                    BitmapUtils.SetBitmapImageData(resultBitmap, image);
                    var newBitmapResourceDefinition = BitmapUtils.CreateBitmapTextureInteropResource(resultBitmap);
                    var newResourceReference = Cache.ResourceCache.CreateBitmapResource(newBitmapResourceDefinition);

                    newHardwareTextures.Add(newResourceReference);
                }

                // Update the bitmap tag with the new resource data.
                bitmap.HardwareTextures = newHardwareTextures;
                bitmap.InterleavedHardwareTextures = new List<TagResourceReference>();

                // Save the updated bitmap tag back to the mod package.
                using (var writeStream = Cache.OpenCacheReadWrite())
                {
                    Cache.Serialize(writeStream, tag, bitmap);
                }
                Console.WriteLine($"Processed bitmap tag: {tag.Name}");
            }

            Console.WriteLine("Done processing all bitmap tags in the mod package.");
            return true;
        }

        /// <summary>
        /// Determines if the specified tag is a bitmap tag.
        /// Assumes that bitmap tags use the group identifier "bitmap".
        /// </summary>
        private bool IsBitmapTag(CachedTag tag)
        {
            return tag.Group.ToString().Equals("bitmap", StringComparison.OrdinalIgnoreCase);
        }
    }
}
