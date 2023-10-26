using TagTool.Cache;
using TagTool.Cache.HaloOnline;
using TagTool.Common;
using TagTool.Tags.Definitions;
using System.IO;
using System.Collections.Generic;

namespace TagTool.Commands.MtnDewIt.ConvertCache 
{
    public class objects_weapons_rifle_covenant_carbine_covenant_carbine_v2_covenant_carbine_v2_weapon : TagFile
    {
        public objects_weapons_rifle_covenant_carbine_covenant_carbine_v2_covenant_carbine_v2_weapon(GameCache cache, GameCacheHaloOnline cacheContext, Stream stream) : base
        (
            cache,
            cacheContext,
            stream
        )
        {
            Cache = cache;
            CacheContext = cacheContext;
            Stream = stream;
            TagData();
        }

        public override void TagData()
        {
            var tag = GetCachedTag<Weapon>($@"objects\weapons\rifle\covenant_carbine\covenant_carbine_v2\covenant_carbine_v2");
            var weap = CacheContext.Deserialize<Weapon>(Stream, tag);
            weap.FirstPerson = new List<Weapon.FirstPersonBlock> 
            {
                new Weapon.FirstPersonBlock() 
                {
                    FirstPersonModel = GetCachedTag<RenderModel>($@"objects\weapons\rifle\covenant_carbine\covenant_carbine_v2\fp_covenant_carbine\fp_covenant_carbine_v2"),
                    FirstPersonAnimations = GetCachedTag<ModelAnimationGraph>($@"objects\characters\masterchief\fp\weapons\rifle\fp_covenant_carbine\fp_covenant_carbine"),
                },
                new Weapon.FirstPersonBlock()
                {
                    FirstPersonModel = GetCachedTag<RenderModel>($@"objects\weapons\rifle\covenant_carbine\covenant_carbine_v2\fp_covenant_carbine\fp_covenant_carbine_v2"),
                    FirstPersonAnimations = GetCachedTag<ModelAnimationGraph>($@"objects\characters\dervish\fp\weapons\rifle\fp_covenant_carbine\fp_covenant_carbine"),
                },
            };
            CacheContext.Serialize(Stream, tag, weap);
        }
    }
}
