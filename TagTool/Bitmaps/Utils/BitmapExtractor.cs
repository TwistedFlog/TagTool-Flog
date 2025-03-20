﻿using System;
using System.IO;
using TagTool.Cache;
using TagTool.IO;
using TagTool.Tags.Definitions;
using TagTool.Bitmaps.DDS;
using TagTool.Bitmaps.Utils;

namespace TagTool.Bitmaps
{
    public static class BitmapExtractor
    {
        public static byte[] ExtractBitmapData(GameCache cache, Bitmap bitmap, int imageIndex, ref Bitmap.Image image)
        {
            var resourceReference = bitmap.HardwareTextures[imageIndex];
            var resourceDefinition = cache.ResourceCache.GetBitmapTextureInteropResource(resourceReference);
            if (cache is GameCacheHaloOnlineBase)
            {
                if(resourceDefinition != null)
                {
                    image.Width = resourceDefinition.Texture.Definition.Bitmap.Width;
                    image.Height = resourceDefinition.Texture.Definition.Bitmap.Height;
                    return resourceDefinition.Texture.Definition.PrimaryResourceData.Data;
                }
                else
                {
                    Console.Error.WriteLine("No resource associated to this bitmap.");
                    return null;
                }
            }
            else if(cache.GetType() == typeof(GameCacheGen3))
            {
                if (resourceDefinition != null)
                {
                    var bitmapTextureInteropDefinition = resourceDefinition.Texture.Definition.Bitmap;

                    if(bitmapTextureInteropDefinition.HighResInSecondaryResource == 1)
                    {
                        var result = new byte[resourceDefinition.Texture.Definition.PrimaryResourceData.Data.Length + resourceDefinition.Texture.Definition.SecondaryResourceData.Data.Length];
                        Array.Copy(resourceDefinition.Texture.Definition.PrimaryResourceData.Data, 0, result, 0, resourceDefinition.Texture.Definition.PrimaryResourceData.Data.Length);
                        Array.Copy(resourceDefinition.Texture.Definition.SecondaryResourceData.Data, 0, result, resourceDefinition.Texture.Definition.PrimaryResourceData.Data.Length, resourceDefinition.Texture.Definition.SecondaryResourceData.Data.Length);
                        return result;
                    }
                    else
                    {
                        return resourceDefinition.Texture.Definition.PrimaryResourceData.Data;
                    }
                }
                else
                {
                    Console.Error.WriteLine("No resource associated to this bitmap.");
                    return null;
                }
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public static BaseBitmap ExtractBitmap(GameCache cache, Bitmap bitmap, int imageIndex, string tagName, bool forDDS = true)
        {
            if (cache is GameCacheHaloOnlineBase)
            {
                var image = bitmap.Images[imageIndex].DeepCloneV2();
                return new BaseBitmap(image, ExtractBitmapData(cache, bitmap, imageIndex, ref image));
            }
            else if (CacheVersionDetection.GetGeneration(cache.Version) ==  CacheGeneration.Third)
            {
                return BitmapConverter.ConvertGen3Bitmap(cache, bitmap, imageIndex, tagName, forDDS);
            }

            return null;
        }

        public static DDSFile ExtractBitmap(GameCache cache, Bitmap bitmap, int imageIndex, string tagName)
        {
            var baseBitmap = ExtractBitmap(cache, bitmap, imageIndex, tagName, true);
            if (baseBitmap == null)
                return null;

            if (cache.GetType() != typeof(GameCacheGen3) && bitmap.Images[imageIndex].Format == BitmapFormat.Dxn)
            {
                byte[] dxt5Data = ReencodeDXNToDXT5(cache, bitmap, imageIndex);
                baseBitmap.Data = dxt5Data;
                baseBitmap.UpdateFormat(BitmapFormat.Dxt5);
            }

            return new DDSFile(baseBitmap);
        }

        private static byte[] ReencodeDXNToDXT5(GameCache cache, Bitmap bitmap, int imageIndex)
        {
            var resourceReference = bitmap.HardwareTextures[imageIndex];
            var resourceDefinition = cache.ResourceCache.GetBitmapTextureInteropResource(resourceReference);
            byte[] primaryData = resourceDefinition?.Texture.Definition.PrimaryResourceData.Data;
            byte[] secondaryData = resourceDefinition?.Texture.Definition.SecondaryResourceData.Data;
            int mipLevel = 0;
            int width = bitmap.Images[imageIndex].Width;
            int height = bitmap.Images[imageIndex].Height;

            int pixelDataOffset = BitmapUtilsPC.GetTextureOffset(bitmap.Images[imageIndex], mipLevel);
            int pixelDataSize = BitmapUtilsPC.GetMipmapPixelDataSize(bitmap.Images[imageIndex], mipLevel);
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

            byte[] uncompressed = BitmapDecoder.DecodeBitmap(highestResData, BitmapFormat.Dxn, width, height);

            int pixelCount = width * height;
            for (int i = 0; i < pixelCount; i++)
            {
                int index = i * 4;
                float x = (((float)uncompressed[index + 2] / 255f) * 2f) - 1f;
                float y = (((float)uncompressed[index + 1] / 255f) * 2f) - 1f;
                float z = (float)Math.Sqrt(Math.Max(0f, Math.Min(1f, (1f - (x * x)) - (y * y))));
                uncompressed[index] = (byte)(((z + 1f) / 2f) * 255f);
            }

            float brightnessOffset = 0.01f * 255f;
            for (int i = 0; i < pixelCount; i++)
            {
                int index = i * 4;
                uncompressed[index] = (byte)Math.Clamp(uncompressed[index] + brightnessOffset, 0, 255);       // Blue
                uncompressed[index + 1] = (byte)Math.Clamp(uncompressed[index + 1] + brightnessOffset, 0, 255); // Green
                uncompressed[index + 2] = (byte)Math.Clamp(uncompressed[index + 2] + brightnessOffset, 0, 255); // Red
            }

            byte[] dxt5Data = BitmapDecoder.EncodeBitmap(uncompressed, BitmapFormat.Dxt5, width, height);
            return dxt5Data;
        }
    }
}