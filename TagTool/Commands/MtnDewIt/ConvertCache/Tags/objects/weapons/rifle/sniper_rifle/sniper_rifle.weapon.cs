using TagTool.Cache;
using TagTool.Cache.HaloOnline;
using TagTool.Common;
using TagTool.Tags.Definitions;
using System.IO;
using System.Collections.Generic;

namespace TagTool.Commands.MtnDewIt.ConvertCache 
{
    public class objects_weapons_rifle_sniper_rifle_sniper_rifle_weapon : TagFile
    {
        public objects_weapons_rifle_sniper_rifle_sniper_rifle_weapon(GameCache cache, GameCacheHaloOnline cacheContext, Stream stream) : base
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
            var tag = GetCachedTag<Weapon>($@"objects\weapons\rifle\sniper_rifle\sniper_rifle");
            var weap = CacheContext.Deserialize<Weapon>(Stream, tag);
            weap.FirstPerson = new List<Weapon.FirstPersonBlock> 
            {
                new Weapon.FirstPersonBlock() 
                {
                    FirstPersonModel = GetCachedTag<RenderModel>($@"objects\weapons\rifle\sniper_rifle\fp_sniper_rifle\fp_sniper_rifle"),
                    FirstPersonAnimations = GetCachedTag<ModelAnimationGraph>($@"objects\characters\masterchief\fp\weapons\rifle\fp_sniper_rifle\fp_sniper_rifle"),
                },
                new Weapon.FirstPersonBlock()
                {
                    FirstPersonModel = GetCachedTag<RenderModel>($@"objects\weapons\rifle\sniper_rifle\fp_sniper_rifle\fp_sniper_rifle"),
                    FirstPersonAnimations = GetCachedTag<ModelAnimationGraph>($@"objects\characters\dervish\fp\weapons\rifle\fp_sniper_rifle\fp_sniper_rifle"),
                },
            };
            CacheContext.Serialize(Stream, tag, weap);
        }
    }
}
