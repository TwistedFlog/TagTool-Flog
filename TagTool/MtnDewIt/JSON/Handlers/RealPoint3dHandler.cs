﻿using Newtonsoft.Json;
using System;
using TagTool.Cache.HaloOnline;
using TagTool.Cache;
using TagTool.Common;

namespace TagTool.MtnDewIt.JSON.Handlers
{
    public class RealPoint3dHandler : JsonConverter<RealPoint3d>
    {
        private GameCache Cache;
        private GameCacheHaloOnline CacheContext;

        public RealPoint3dHandler(GameCache cache, GameCacheHaloOnline cacheContext)
        {
            Cache = cache;
            CacheContext = cacheContext;
        }

        public override void WriteJson(JsonWriter writer, RealPoint3d value, JsonSerializer serializer)
        {
            writer.WriteValue($@"X: {value.X}, Y: {value.Y}, Z: {value.Z}");
        }

        public override RealPoint3d ReadJson(JsonReader reader, Type objectType, RealPoint3d existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            var value = reader.Value.ToString();
            var valueArray = value
            .Replace("X: ", "")
            .Replace("Y: ", "")
            .Replace("Z: ", "")
            .Split(',');

            var x = float.Parse(valueArray[0]);
            var y = float.Parse(valueArray[1]);
            var z = float.Parse(valueArray[2]);

            return new RealPoint3d(x, y, z);
        }
    }
}
