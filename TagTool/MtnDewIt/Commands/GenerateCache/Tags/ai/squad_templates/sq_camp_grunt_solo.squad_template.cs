using TagTool.Cache;
using TagTool.Cache.HaloOnline;
using TagTool.Common;
using TagTool.Tags.Definitions;
using System.IO;
using System.Collections.Generic;

namespace TagTool.MtnDewIt.Commands.GenerateCache.Tags 
{
    public class ai_squad_templates_sq_camp_grunt_solo_squad_template : TagFile
    {
        public ai_squad_templates_sq_camp_grunt_solo_squad_template(GameCache cache, GameCacheHaloOnline cacheContext, Stream stream) : base
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
            var tag = GetCachedTag<SquadTemplate>($@"ai\squad_templates\sq_camp_grunt_solo");
            var sqtm = CacheContext.Deserialize<SquadTemplate>(Stream, tag);
            sqtm.Name = CacheContext.StringTable.GetOrAddString($@"sq_camp_grunt_solo");
            sqtm.CellTemplates = new List<SquadTemplate.CellTemplate>
            {
                new SquadTemplate.CellTemplate
                {
                    Name = CacheContext.StringTable.GetOrAddString($@"1_grunt"),
                    NormalDiffCount = 1,
                    Characters = new List<SquadTemplate.CellTemplate.ObjectBlock>
                    {
                        new SquadTemplate.CellTemplate.ObjectBlock
                        {
                            Probability = 1,
                        },
                    },
                    InitialWeapons = new List<SquadTemplate.CellTemplate.ObjectBlock>
                    {
                        new SquadTemplate.CellTemplate.ObjectBlock
                        {
                            Object = GetCachedTag<Weapon>($@"objects\weapons\pistol\plasma_pistol\plasma_pistol"),
                            Probability = 3,
                        },
                        new SquadTemplate.CellTemplate.ObjectBlock
                        {
                            Object = GetCachedTag<Weapon>($@"objects\weapons\pistol\needler\needler"),
                            Probability = 1,
                        },
                    },
                },
            };
            CacheContext.Serialize(Stream, tag, sqtm);
        }
    }
}