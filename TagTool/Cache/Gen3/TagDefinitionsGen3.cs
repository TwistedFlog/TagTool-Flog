﻿using System;
using System.Collections.Frozen;
using System.Collections.Generic;
using TagTool.Cache.Resources;
using TagTool.Common;
using TagTool.Tags;
using TagTool.Tags.Definitions;

namespace TagTool.Cache.Gen3
{
    public class TagDefinitionsGen3 : TagDefinitions
    {
        public FrozenDictionary<TagGroup, Type> Gen3Types => Gen3Definitions.TagGroupToTypeLookup;
        private static readonly CachedDefinitions Gen3Definitions = GetCachedDefinitions(new Dictionary<TagGroup, Type>
        {
            { new TagGroupGen3("<fx>", "sound_effect_template"), typeof(SoundEffectTemplate) },
            { new TagGroupGen3("achi", "achievements"), typeof(Achievements) },
            { new TagGroupGen3("adlg", "ai_dialogue_globals"), typeof(AiDialogueGlobals) },
            { new TagGroupGen3("aigl", "ai_globals"), typeof(AiGlobals) },
            { new TagGroupGen3("ant!", "antenna"), typeof(Antenna) },
            { new TagGroupGen3("argd", "devi", "obje", "device_arg_device"), typeof(DeviceArgDevice) },
            { new TagGroupGen3("armr", "obje", "armor"), typeof(Armor) },
            { new TagGroupGen3("arms", "armor_sounds"), typeof(ArmorSounds) },
            { new TagGroupGen3("atgf", "atmosphere_globals"), typeof(AtmosphereGlobals) },
            { new TagGroupGen3("beam", "beam_system"), typeof(BeamSystem) },
            { new TagGroupGen3("bink", "bink"), typeof(Bink) },
            { new TagGroupGen3("bipd", "unit", "obje", "biped"), typeof(Biped) },
            { new TagGroupGen3("bitm", "bitmap"), typeof(Bitmap) },
            { new TagGroupGen3("bkey", "gui_button_key_definition"), typeof(GuiButtonKeyDefinition) },
            { new TagGroupGen3("bloc", "obje", "crate"), typeof(Crate) },
            { new TagGroupGen3("bmp3", "gui_bitmap_widget_definition"), typeof(GuiBitmapWidgetDefinition) },
            { new TagGroupGen3("bsdt", "breakable_surface"), typeof(BreakableSurface) },
            { new TagGroupGen3("cddf", "collision_damage"), typeof(CollisionDamage) },
            { new TagGroupGen3("cfgt", "cache_file_global_tags"), typeof(CacheFileGlobalTags) },
            { new TagGroupGen3("cfxs", "camera_fx_settings"), typeof(CameraFxSettings) },
            { new TagGroupGen3("chad", "chud_animation_definition"), typeof(ChudAnimationDefinition) },
            { new TagGroupGen3("char", "character"), typeof(Character) },
            { new TagGroupGen3("chdt", "chud_definition"), typeof(ChudDefinition) },
            { new TagGroupGen3("chgd", "chud_globals_definition"), typeof(ChudGlobalsDefinition) },
            { new TagGroupGen3("chmt", "chocolate_mountain_new"), typeof(ChocolateMountainNew) },
            { new TagGroupGen3("cine", "cinematic"), typeof(Cinematic) },
            { new TagGroupGen3("cisc", "cinematic_scene"), typeof(CinematicScene) },
            { new TagGroupGen3("clwd", "cloth"), typeof(Cloth) },
            { new TagGroupGen3("cmoe", "camo"), typeof(Camo) },
            { new TagGroupGen3("cmpu", "compute_shader"), typeof(ComputeShader) },
            { new TagGroupGen3("cntl", "contrail_system"), typeof(ContrailSystem) },
            { new TagGroupGen3("cobj", "obje", "custom_object"), typeof(CustomObject) },
            { new TagGroupGen3("coll", "collision_model"), typeof(CollisionModel) },
            { new TagGroupGen3("colo", "color_table"), typeof(ColorTable) },
            { new TagGroupGen3("cprl", "chud_widget_parallax_data"), typeof(ChudWidgetParallaxData) },
            { new TagGroupGen3("crea", "obje", "creature"), typeof(Creature) },
            { new TagGroupGen3("crte", "cortana_effect_definition"), typeof(CortanaEffectDefinition) },
            { new TagGroupGen3("csdt", "camera_shake"), typeof(CameraShake) },
            { new TagGroupGen3("ctrl", "devi", "obje", "device_control"), typeof(DeviceControl) },
            { new TagGroupGen3("dctr", "decorator_set"), typeof(DecoratorSet) },
            { new TagGroupGen3("decs", "decal_system"), typeof(DecalSystem) },
            { new TagGroupGen3("devi", "obje", "device"), typeof(Device) },
            { new TagGroupGen3("draw", "rasterizer_cache_file_globals"), typeof(RasterizerCacheFileGlobals) },
            { new TagGroupGen3("drdf", "damage_response_definition"), typeof(DamageResponseDefinition) },
            { new TagGroupGen3("dsrc", "gui_datasource_definition"), typeof(GuiDatasourceDefinition) },
            { new TagGroupGen3("effe", "effect"), typeof(Effect) },
            { new TagGroupGen3("effg", "effect_globals"), typeof(EffectGlobals) },
            { new TagGroupGen3("efsc", "obje", "effect_scenery"), typeof(EffectScenery) },
            { new TagGroupGen3("eqip", "item", "obje", "equipment"), typeof(Equipment) },
            { new TagGroupGen3("flck", "flock"), typeof(Flock) },
            { new TagGroupGen3("fogg", "atmosphere_fog"), typeof(AtmosphereFog) },
            { new TagGroupGen3("foot", "material_effects"), typeof(MaterialEffects) },
            { new TagGroupGen3("forg", "forge_globals_definition"), typeof(ForgeGlobalsDefinition) },
            { new TagGroupGen3("form", "formation"), typeof(Formation) },
            { new TagGroupGen3("fwtg", "user_interface_fourth_wall_timing_definition"), typeof(UserInterfaceFourthWallTimingDefinition) },
            { new TagGroupGen3("gfxt", "gfx_textures_list"), typeof(GfxTexturesList) },
            { new TagGroupGen3("gint", "unit", "obje", "giant"), typeof(Giant) },
            { new TagGroupGen3("gldf", "cheap_light"), typeof(CheapLight) },
            { new TagGroupGen3("glps", "global_pixel_shader"), typeof(GlobalPixelShader) },
            { new TagGroupGen3("glvs", "global_vertex_shader"), typeof(GlobalVertexShader) },
            { new TagGroupGen3("goof", "multiplayer_variant_settings_interface_definition"), typeof(MultiplayerVariantSettingsInterfaceDefinition) },
            { new TagGroupGen3("gpdt", "game_progression"), typeof(GameProgression) },
            { new TagGroupGen3("gpix", "global_cache_file_pixel_shaders"), typeof(GlobalCacheFilePixelShaders) },
            { new TagGroupGen3("grup", "gui_group_widget_definition"), typeof(GuiGroupWidgetDefinition) },
            { new TagGroupGen3("hf2p", "hf2p_globals"), typeof(Hf2pGlobals) },
            { new TagGroupGen3("hlmt", "model"), typeof(Model) },
            { new TagGroupGen3("hlsl", "hlsl_include"), typeof(HlslInclude) },
            { new TagGroupGen3("hsc*", "scenario_hs_source_file"), typeof(ScenarioHsSourceFile) },
            { new TagGroupGen3("inpg", "input_globals"), typeof(InputGlobals) },
            { new TagGroupGen3("item", "obje", "item"), typeof(Item) },
            { new TagGroupGen3("jmad", "model_animation_graph"), typeof(ModelAnimationGraph) },
            { new TagGroupGen3("jmrq", "sandbox_text_value_pair_definition"), typeof(SandboxTextValuePairDefinition) },
            { new TagGroupGen3("jpt!", "damage_effect"), typeof(DamageEffect) },
            { new TagGroupGen3("Lbsp", "scenario_lightmap_bsp_data"), typeof(ScenarioLightmapBspData) },
            { new TagGroupGen3("lens", "lens_flare"), typeof(LensFlare) },
            { new TagGroupGen3("ligh", "light"), typeof(Light) },
            { new TagGroupGen3("lsnd", "sound_looping"), typeof(SoundLooping) },
            { new TagGroupGen3("lst3", "gui_list_widget_definition"), typeof(GuiListWidgetDefinition) },
            { new TagGroupGen3("lswd", "leaf_system"), typeof(LeafSystem) },
            { new TagGroupGen3("ltvl", "light_volume_system"), typeof(LightVolumeSystem) },
            { new TagGroupGen3("mach", "devi", "obje", "device_machine"), typeof(DeviceMachine) },
            { new TagGroupGen3("matg", "globals"), typeof(Globals) },
            { new TagGroupGen3("mdl3", "gui_model_widget_definition"), typeof(GuiModelWidgetDefinition) },
            { new TagGroupGen3("mdlg", "ai_mission_dialogue"), typeof(AiMissionDialogue) },
            { new TagGroupGen3("mffn", "muffin"), typeof(Muffin) },
            { new TagGroupGen3("mlib", "emblem_library"), typeof(EmblemLibrary) },
            { new TagGroupGen3("mlst", "map_list"), typeof(MapList) },
            { new TagGroupGen3("mode", "render_model"), typeof(RenderModel) },
            { new TagGroupGen3("modg", "mod_globals"), typeof(ModGlobalsDefinition) },
            { new TagGroupGen3("mulg", "multiplayer_globals"), typeof(MultiplayerGlobals) },
            { new TagGroupGen3("nclt", "new_cinematic_lighting"), typeof(NewCinematicLighting) },
            { new TagGroupGen3("obje", "object"), typeof(GameObject) },
            { new TagGroupGen3("pact", "player_action_set"), typeof(PlayerActionSet) },
            { new TagGroupGen3("pecp", "particle_emitter_custom_points"), typeof(ParticleEmitterCustomPoints) },
            { new TagGroupGen3("pdm!", "podium_settings"), typeof(PodiumSettings) },
            { new TagGroupGen3("perf", "performance_throttles"), typeof(PerformanceThrottles) },
            { new TagGroupGen3("phmo", "physics_model"), typeof(PhysicsModel) },
            { new TagGroupGen3("pixl", "pixel_shader"), typeof(PixelShader) },
            { new TagGroupGen3("play", "cache_file_resource_layout_table"), typeof(ResourceLayoutTable) },
            { new TagGroupGen3("pmdf", "particle_model"), typeof(ParticleModel) },
            { new TagGroupGen3("pmov", "particle_physics"), typeof(ParticlePhysics) },
            { new TagGroupGen3("pphy", "point_physics"), typeof(PointPhysics) },
            { new TagGroupGen3("proj", "obje", "projectile"), typeof(Projectile) },
            { new TagGroupGen3("prt3", "particle"), typeof(Particle) },
            { new TagGroupGen3("rasg", "rasterizer_globals"), typeof(RasterizerGlobals) },
            { new TagGroupGen3("rm  ", "render_method"), typeof(RenderMethod) },
            { new TagGroupGen3("rmbk", "rm  ", "shader_black"), typeof(ShaderBlack) },
            { new TagGroupGen3("rmcs", "rm  ", "shader_custom"), typeof(ShaderCustom) },
            { new TagGroupGen3("rmct", "rm  ", "shader_cortana"), typeof(ShaderCortana) },
            { new TagGroupGen3("rmd ", "rm  ", "shader_decal"), typeof(ShaderDecal) },
            { new TagGroupGen3("rmdf", "render_method_definition"), typeof(RenderMethodDefinition) },
            { new TagGroupGen3("rmfl", "rm  ", "shader_foliage"), typeof(ShaderFoliage) },
            { new TagGroupGen3("rmgl", "rm  ", "shader_glass"), typeof(ShaderGlass) },
            { new TagGroupGen3("rmhg", "rm  ", "shader_halogram"), typeof(ShaderHalogram) },
            { new TagGroupGen3("rmop", "render_method_option"), typeof(RenderMethodOption) },
            { new TagGroupGen3("rmsh", "rm  ", "shader"), typeof(Shader) },
            { new TagGroupGen3("rmss", "rm  ", "shader_screen"), typeof(ShaderScreen) },
            { new TagGroupGen3("rmt2", "render_method_template"), typeof(RenderMethodTemplate) },
            { new TagGroupGen3("rmtr", "rm  ", "shader_terrain"), typeof(ShaderTerrain) },
            { new TagGroupGen3("rmw ", "rm  ", "shader_water"), typeof(ShaderWater) },
            { new TagGroupGen3("rmzo", "rm  ", "shader_zonly"), typeof(ShaderZonly) },
            { new TagGroupGen3("rmbl", "rumble"), typeof(Rumble) },
            { new TagGroupGen3("rwrd", "render_water_ripple"), typeof(RenderWaterRipple) },
            { new TagGroupGen3("sLdT", "scenario_lightmap"), typeof(ScenarioLightmap) },
            { new TagGroupGen3("sbsp", "scenario_structure_bsp"), typeof(ScenarioStructureBsp) },
            { new TagGroupGen3("scen", "obje", "scenery"), typeof(Scenery) },
            { new TagGroupGen3("scn3", "gui_screen_widget_definition"), typeof(GuiScreenWidgetDefinition) },
            { new TagGroupGen3("scnr", "scenario"), typeof(Scenario) },
            { new TagGroupGen3("sddt", "structure_design"), typeof(StructureDesign) },
            { new TagGroupGen3("sdzg", "scenario_required_resource"), typeof(ScenarioRequiredResource) },
            { new TagGroupGen3("sefc", "area_screen_effect"), typeof(AreaScreenEffect) },
            { new TagGroupGen3("sfx+", "sound_effect_collection"), typeof(SoundEffectCollection) },
            { new TagGroupGen3("sgp!", "sound_global_propagation"), typeof(SoundGlobalPropagation) },
            { new TagGroupGen3("shit", "shield_impact"), typeof(ShieldImpact) },
            { new TagGroupGen3("siin", "simulation_interpolation"), typeof(SimulationInterpolation) },
            { new TagGroupGen3("sily", "text_value_pair_definition"), typeof(TextValuePairDefinition) },
            { new TagGroupGen3("skn3", "gui_skin_definition"), typeof(GuiSkinDefinition) },
            { new TagGroupGen3("skya", "sky_atm_parameters"), typeof(SkyAtmParameters) },
            { new TagGroupGen3("smdt", "survival_mode_globals"), typeof(SurvivalModeGlobals) },
            { new TagGroupGen3("sncl", "sound_classes"), typeof(SoundClasses) },
            { new TagGroupGen3("snd!", "sound"), typeof(Sound) },
            { new TagGroupGen3("snde", "sound_environment"), typeof(SoundEnvironment) },
            { new TagGroupGen3("snmx", "sound_mix"), typeof(SoundMix) },
            { new TagGroupGen3("spda", "scenario_pda"), typeof(ScenarioPDA) },
            { new TagGroupGen3("spk!", "sound_dialogue_constants"), typeof(SoundDialogueConstants) },
            { new TagGroupGen3("sqtm", "squad_template"), typeof(SquadTemplate) },
            { new TagGroupGen3("ssce", "obje", "sound_scenery"), typeof(SoundScenery) },
            { new TagGroupGen3("styl", "style"), typeof(Style) },
            { new TagGroupGen3("sus!", "sound_ui_sounds"), typeof(SoundUiSounds) },
            { new TagGroupGen3("term", "devi", "obje", "device_terminal"), typeof(Terminal) },
            //{ new TagGroupGen3("test", "test_blah"), typeof(TestDefinition) },
            { new TagGroupGen3("trak", "camera_track"), typeof(CameraTrack) },
            { new TagGroupGen3("trdf", "texture_render_list"), typeof(TextureRenderList) },
            { new TagGroupGen3("txt3", "gui_text_widget_definition"), typeof(GuiTextWidgetDefinition) },
            { new TagGroupGen3("udlg", "dialogue"), typeof(Dialogue) },
            { new TagGroupGen3("ugh!", "sound_cache_file_gestalt"), typeof(SoundCacheFileGestalt) },
            { new TagGroupGen3("uise", "user_interface_sounds_definition"), typeof(UserInterfaceSoundsDefinition) },
            { new TagGroupGen3("unic", "multilingual_unicode_string_list"), typeof(MultilingualUnicodeStringList) },
            { new TagGroupGen3("unit", "obje", "unit"), typeof(Unit) },
            { new TagGroupGen3("vehi", "unit", "obje", "vehicle"), typeof(Vehicle) },
            { new TagGroupGen3("vfsl", "vfiles_list"), typeof(VFilesList) },
            { new TagGroupGen3("vmdx", "vision_mode"), typeof(VisionMode) },
            { new TagGroupGen3("vtsh", "vertex_shader"), typeof(VertexShader) },
            { new TagGroupGen3("wacd", "gui_widget_animation_collection_definition"), typeof(GuiWidgetAnimationCollectionDefinition) },
            { new TagGroupGen3("wave", "wave_template"), typeof(WaveTemplate) },
            { new TagGroupGen3("wclr", "gui_widget_color_animation_definition"), typeof(GuiWidgetColorAnimationDefinition) },
            { new TagGroupGen3("weap", "item", "obje", "weapon"), typeof(Weapon) },
            { new TagGroupGen3("wezr", "game_engine_settings_definition"), typeof(GameEngineSettingsDefinition) },
            { new TagGroupGen3("wgan", "gui_widget_animation_definition"), typeof(GuiWidgetAnimationDefinition) },
            { new TagGroupGen3("wgtz", "user_interface_globals_definition"), typeof(UserInterfaceGlobalsDefinition) },
            { new TagGroupGen3("wigl", "user_interface_shared_globals_definition"), typeof(UserInterfaceSharedGlobalsDefinition) },
            { new TagGroupGen3("wind", "wind"), typeof(Wind) },
            { new TagGroupGen3("wpos", "gui_widget_position_animation_definition"), typeof(GuiWidgetPositionAnimationDefinition) },
            { new TagGroupGen3("wrot", "gui_widget_rotation_animation_definition"), typeof(GuiWidgetRotationAnimationDefinition) },
            { new TagGroupGen3("wscl", "gui_widget_scale_animation_definition"), typeof(GuiWidgetScaleAnimationDefinition) },
            { new TagGroupGen3("wspr", "gui_widget_sprite_animation_definition"), typeof(GuiWidgetSpriteAnimationDefinition) },
            { new TagGroupGen3("wtuv", "gui_widget_texture_coordinate_animation_definition"), typeof(GuiWidgetTextureCoordinateAnimationDefinition) },
            { new TagGroupGen3("zone", "cache_file_resource_gestalt"), typeof(ResourceGestalt) }
        });
        public TagDefinitionsGen3() : base(Gen3Definitions) { }

        private static readonly FrozenDictionary<string, Tag> NameToTagLookup = NameToTagLookupValue();
        private static FrozenDictionary<string, Tag> NameToTagLookupValue()
        {
            var result = new Dictionary<string, Tag>();
            foreach (var (key, _) in Gen3Definitions.TagGroupToTypeLookup)
            {
                result.Add(((TagGroupGen3)key).Name, key.Tag);
            }
            return result.ToFrozenDictionary();
        }

        public bool TryGetTagFromName(string name, out Tag tag)
        {
            return NameToTagLookup.TryGetValue(name, out tag);
        }
    }
}
