﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagTool.Commands;
using TagTool.MtnDewIt.JSON;

namespace TagTool.MtnDewIt.Commands.GenerateCache
{
    partial class GenerateCacheCommand : Command 
    {
        private TagObjectParser TagParser;
        private MapObjectParser MapParser;
        private BlfObjectParser BlfParser;

        private List<string> TagObjectList;
        private List<string> MapObjectList;
        private List<string> BlfObjectList;

        // TODO: Add Tag Support :/
        // I will come back to you at some point, as I need to figure out how to handle resource data :/
        // I don't want to store any resource data in JSON, as that data is cache specific, and will not transfer over to a generated cache
        //public void UpdateTagData()
        //{
        //    TagParser = new TagObjectParser(Cache, CacheContext, CacheStream);
        //
        //    var jsonData = File.ReadAllText($@"{Program.TagToolDirectory}\Tools\JSON\commands\convertcache\tags.json");
        //    TagObjectList = JsonConvert.DeserializeObject<List<string>>(jsonData);
        //}

        public void UpdateMapData()
        {
            MapParser = new MapObjectParser(Cache, CacheContext, CacheStream);

            var jsonData = File.ReadAllText($@"{Program.TagToolDirectory}\Tools\JSON\commands\generatecache\maps.json");
            MapObjectList = JsonConvert.DeserializeObject<List<string>>(jsonData);

            foreach (var file in MapObjectList)
                MapParser.ParseFile($@"{Program.TagToolDirectory}\Tools\JSON\maps\{file}");
        }

        public void UpdateBlfData()
        {
            BlfParser = new BlfObjectParser(Cache, CacheContext, CacheStream);

            var jsonData = File.ReadAllText($@"{Program.TagToolDirectory}\Tools\JSON\commands\generatecache\blf.json");
            BlfObjectList = JsonConvert.DeserializeObject<List<string>>(jsonData);

            foreach (var file in BlfObjectList)
                BlfParser.ParseFile($@"{Program.TagToolDirectory}\Tools\JSON\{file}");
        }
    }
}