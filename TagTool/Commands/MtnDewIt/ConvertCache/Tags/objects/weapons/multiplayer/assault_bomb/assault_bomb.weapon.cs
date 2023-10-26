using TagTool.Cache;
using TagTool.Cache.HaloOnline;
using TagTool.Common;
using TagTool.Tags.Definitions;
using System.IO;
using System.Collections.Generic;

namespace TagTool.Commands.MtnDewIt.ConvertCache 
{
    public class objects_weapons_multiplayer_assault_bomb_assault_bomb_weapon : TagFile
    {
        public objects_weapons_multiplayer_assault_bomb_assault_bomb_weapon(GameCache cache, GameCacheHaloOnline cacheContext, Stream stream) : base
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
            var tag = GetCachedTag<Weapon>($@"objects\weapons\multiplayer\assault_bomb\assault_bomb");
            var weap = CacheContext.Deserialize<Weapon>(Stream, tag);
            weap.FirstPerson = new List<Weapon.FirstPersonBlock> 
            {
                new Weapon.FirstPersonBlock() 
                {
                    FirstPersonModel = GetCachedTag<RenderModel>($@"objects\weapons\multiplayer\assault_bomb\fp_assault_bomb\fp_assault_bomb"),
                    FirstPersonAnimations = GetCachedTag<ModelAnimationGraph>($@"objects\characters\masterchief\fp\weapons\multiplayer\fp_assault_bomb\fp_assault_bomb"),
                },
                new Weapon.FirstPersonBlock()
                {
                    FirstPersonModel = GetCachedTag<RenderModel>($@"objects\weapons\multiplayer\assault_bomb\fp_assault_bomb\fp_assault_bomb"),
                    FirstPersonAnimations = GetCachedTag<ModelAnimationGraph>($@"objects\characters\dervish\fp\weapons\multiplayer\fp_assault_bomb\fp_assault_bomb"),
                },
            };
            CacheContext.Serialize(Stream, tag, weap);
        }
    }
}
