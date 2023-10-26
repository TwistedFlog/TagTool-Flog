using TagTool.Cache;
using TagTool.Cache.HaloOnline;
using TagTool.Common;
using TagTool.Tags.Definitions;
using System.IO;
using TagTool.Tags.Definitions.Common;

namespace TagTool.Commands.MtnDewIt.ConvertCache 
{
    public class ui_chud_multiplayer_intro_summary_assault_chud_definition : TagFile
    {
        public ui_chud_multiplayer_intro_summary_assault_chud_definition(GameCache cache, GameCacheHaloOnline cacheContext, Stream stream) : base
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
            var tag = GetCachedTag<ChudDefinition>($@"ui\chud\multiplayer_intro\summary_assault");
            var chdt = CacheContext.Deserialize<ChudDefinition>(Stream, tag);
            chdt.HudWidgets[0].PlacementData[0].Offset = new RealPoint2d(0f, 140f);
            chdt.HudWidgets[1].PlacementData[0].Offset = new RealPoint2d(0f, 140f);
            chdt.HudWidgets[2].PlacementData[0].Offset = new RealPoint2d(0f, 140f);
            chdt.HudWidgets[2].TextWidgets[0].PlacementData[0].Scale = new RealPoint2d(0.6f, 0.6f);
            chdt.HudWidgets[2].TextWidgets[0].Font = WidgetFontValue.LargeBodyText;
            chdt.HudWidgets[2].TextWidgets[1].PlacementData[0].Scale = new RealPoint2d(0.55f, 0.55f);
            chdt.HudWidgets[2].TextWidgets[1].Font = WidgetFontValue.LargeBodyText;
            chdt.HudWidgets[2].TextWidgets[2].PlacementData[0].Scale = new RealPoint2d(0.55f, 0.55f);
            chdt.HudWidgets[2].TextWidgets[2].Font = WidgetFontValue.LargeBodyText;
            chdt.HudWidgets[2].TextWidgets[3].PlacementData[0].Scale = new RealPoint2d(0.65f, 0.65f);
            chdt.HudWidgets[2].TextWidgets[3].Font = WidgetFontValue.LargeBodyText;
            chdt.HudWidgets[2].TextWidgets[4].PlacementData[0].Scale = new RealPoint2d(0.65f, 0.65f);
            chdt.HudWidgets[2].TextWidgets[4].Font = WidgetFontValue.LargeBodyText;
            chdt.HudWidgets[3].PlacementData[0].Offset = new RealPoint2d(0f, 140f);
            chdt.HudWidgets[3].TextWidgets[0].PlacementData[0].Scale = new RealPoint2d(0.6f, 0.6f);
            chdt.HudWidgets[3].TextWidgets[0].Font = WidgetFontValue.LargeBodyText;
            chdt.HudWidgets[3].TextWidgets[1].PlacementData[0].Scale = new RealPoint2d(0.55f, 0.55f);
            chdt.HudWidgets[3].TextWidgets[1].Font = WidgetFontValue.LargeBodyText;
            CacheContext.Serialize(Stream, tag, chdt);
        }
    }
}
