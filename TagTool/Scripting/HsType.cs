using TagTool.Cache;
using TagTool.Tags;

namespace TagTool.Scripting
{
    [TagStructure(Size = 0x2)]
    public class HsType : TagStructure
	{
        [TagField(MinVersion = CacheVersion.Halo3Retail, MaxVersion = CacheVersion.Halo3Retail)]
        public Halo3RetailValue Halo3Retail;

        [TagField(MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.Halo3ODST)]
        public Halo3ODSTValue Halo3ODST;

        // I don't know if newer HO builds nedd thier own enums? (Will need to investigate)
        [TagField(MinVersion = CacheVersion.HaloOnlineED, MaxVersion = CacheVersion.HaloOnline700123)]
        public HaloOnlineValue HaloOnline;

        [TagField(MinVersion = CacheVersion.HaloReach)]
        public HaloReachValue HaloReach;

        public bool IsObject 
        {
            get 
            {
                switch (HaloOnline) 
                {
                    case HaloOnlineValue.Object:
                    case HaloOnlineValue.ObjectList:
                    case HaloOnlineValue.Device:
                    case HaloOnlineValue.EffectScenery:
                    case HaloOnlineValue.Scenery:
                    case HaloOnlineValue.Weapon:
                    case HaloOnlineValue.Unit:
                    case HaloOnlineValue.Vehicle:
                    	return true;
                    default:
                    	return false;
                }
            }
        }

        public enum Halo3RetailValue : ushort
        {
            Invalid = 47802,
            Unparsed = 0,
            SpecialForm,
            FunctionName,
            Passthrough,
            Void,
            Boolean,
            Real,
            Short,
            Long,
            String,
            Script,
            StringId,
            UnitSeatMapping,
            TriggerVolume,
            CutsceneFlag,
            CutsceneCameraPoint,
            CutsceneTitle,
            CutsceneRecording,
            DeviceGroup,
            Ai,
            AiCommandList,
            AiCommandScript,
            AiBehavior,
            AiOrders,
            AiLine,
            StartingProfile,
            Conversation,
            ZoneSet,
            DesignerZone,
            PointReference,
            Style,
            ObjectList,
            Folder,
            Sound,
            Effect,
            Damage,
            LoopingSound,
            AnimationGraph,
            DamageEffect,
            ObjectDefinition,
            Bitmap,
            Shader,
            RenderModel,
            StructureDefinition,
            LightmapDefinition,
            CinematicDefinition,
            CinematicSceneDefinition,
            BinkDefinition,
            AnyTag,
            AnyTagNotResolving,
            GameDifficulty,
            Team,
            MpTeam,
            Controller,
            ButtonPreset,
            JoystickPreset,
            PlayerColor,
            PlayerCharacterType,
            VoiceOutputSetting,
            VoiceMask,
            SubtitleSetting,
            ActorType,
            ModelState,
            Event,
            CharacterPhysics,
            Object,
            Unit,
            Vehicle,
            Weapon,
            Device,
            Scenery,
            EffectScenery,
            ObjectName,
            UnitName,
            VehicleName,
            WeaponName,
            DeviceName,
            SceneryName,
            EffectSceneryName,
            CinematicLightprobe,
            AnimationBudgetReference,
            LoopingSoundBudgetReference,
            SoundBudgetReference
        }

        public enum Halo3ODSTValue : ushort
        {
            Invalid = 47802,
            Unparsed = 0,
            SpecialForm,
            FunctionName,
            Passthrough,
            Void,
            Boolean,
            Real,
            Short,
            Long,
            String,
            Script,
            StringId,
            UnitSeatMapping,
            TriggerVolume,
            CutsceneFlag,
            CutsceneCameraPoint,
            CutsceneTitle,
            CutsceneRecording,
            DeviceGroup,
            Ai,
            AiCommandList,
            AiCommandScript,
            AiBehavior,
            AiOrders,
            AiLine,
            StartingProfile,
            Conversation,
            ZoneSet,
            DesignerZone,
            PointReference,
            Style,
            ObjectList,
            Folder,
            Sound,
            Effect,
            Damage,
            LoopingSound,
            AnimationGraph,
            DamageEffect,
            ObjectDefinition,
            Bitmap,
            Shader,
            RenderModel,
            StructureDefinition,
            LightmapDefinition,
            CinematicDefinition,
            CinematicSceneDefinition,
            BinkDefinition,
            AnyTag,
            AnyTagNotResolving,
            GameDifficulty,
            Team,
            MpTeam,
            Controller,
            ButtonPreset,
            JoystickPreset,
            PlayerColor,
            PlayerCharacterType,
            VoiceOutputSetting,
            VoiceMask,
            SubtitleSetting,
            ActorType,
            ModelState,
            Event,
            CharacterPhysics,
            PrimarySkull,
            SecondarySkull,
            Object,
            Unit,
            Vehicle,
            Weapon,
            Device,
            Scenery,
            EffectScenery,
            ObjectName,
            UnitName,
            VehicleName,
            WeaponName,
            DeviceName,
            SceneryName,
            EffectSceneryName,
            CinematicLightprobe,
            AnimationBudgetReference,
            LoopingSoundBudgetReference,
            SoundBudgetReference
        }

        public enum HaloOnlineValue : ushort
        {
            Invalid = 47802,
            Unparsed = 0,
            SpecialForm,
            FunctionName,
            Passthrough,
            Void,
            Boolean,
            Real,
            Short,
            Long,
            String,
            Script,
            StringId,
            UnitSeatMapping,
            TriggerVolume,
            CutsceneFlag,
            CutsceneCameraPoint,
            CutsceneTitle,
            CutsceneRecording,
            DeviceGroup,
            Ai,
            AiCommandList,
            AiCommandScript,
            AiBehavior,
            AiOrders,
            AiLine,
            StartingProfile,
            Conversation,
            ZoneSet,
            DesignerZone,
            PointReference,
            Style,
            ObjectList,
            Folder,
            Sound,
            Effect,
            Damage,
            LoopingSound,
            AnimationGraph,
            DamageEffect,
            ObjectDefinition,
            Bitmap,
            Shader,
            RenderModel,
            StructureDefinition,
            LightmapDefinition,
            CinematicDefinition,
            CinematicSceneDefinition,
            BinkDefinition,
            AnyTag,
            AnyTagNotResolving,
            GameDifficulty,
            Team,
            MpTeam,
            Controller,
            ButtonPreset,
            JoystickPreset,
            PlayerCharacterType,
            VoiceOutputSetting,
            VoiceMask,
            SubtitleSetting,
            ActorType,
            ModelState,
            Event,
            CharacterPhysics,
            PrimarySkull,
            SecondarySkull,
            Object,
            Unit,
            Vehicle,
            Weapon,
            Device,
            Scenery,
            EffectScenery,
            ObjectName,
            UnitName,
            VehicleName,
            WeaponName,
            DeviceName,
            SceneryName,
            EffectSceneryName,
            CinematicLightprobe,
            AnimationBudgetReference,
            LoopingSoundBudgetReference,
            SoundBudgetReference
        }

        public enum HaloReachValue : ushort
        {
            //empty for now
        }
    }
}
