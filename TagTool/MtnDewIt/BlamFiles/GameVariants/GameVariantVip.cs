using System;
using TagTool.Tags;

namespace TagTool.MtnDewIt.BlamFiles
{
    [Flags]
    public enum GameVariantVipFlags : short
    {
        SingleVip = 0,
        DestinationZonesEnabled,
        EndRoundOnVipDeath,
    }

    public enum VipSelectionSettings : byte 
    {
        Random = 0,
        NextRespawn,
        NextKill,
        Unchanged,
    }

    public enum VipZoneMovementSettings : byte
    {
        Off,
        Seconds_10,
        Seconds_15,
        Seconds_30,
        Minutes_1,
        Minutes_2,
        Minutes_3,
        Minutes_4,
        Minutes_5,
        OnArrival,
        OnSwitch,
    }

    public enum VipZoneOrderSettings : byte
    {
        Random,
        Sequence,
    }

    [TagStructure(Size = 0x238)]
    public class GameVariantVip : GameVariantBase
    {
        public short ScoreToWinRound;
        public short ScoreUnknown;
        public GameVariantVipFlags VariantFlags;
        public sbyte KillPoints;
        public sbyte TakedownPoints;
        public sbyte KillAsVipPoints;
        public sbyte VipDeathPoints;
        public sbyte DestinationArrivalPoints;
        public sbyte SuicidePoints;
        public sbyte BetrayalPoints;
        public sbyte VipSuicidePoints;
        public VipSelectionSettings VipSelection;
        public VipZoneMovementSettings ZoneMovement;
        public VipZoneOrderSettings ZoneOrder;

        [TagField(Flags = TagFieldFlags.Padding, Length = 1)]
        public byte[] Padding1 = new byte[1];

        public short InfluenceRadius;
        public PlayerTraits VipTeamTraits;
        public PlayerTraits VipInfluenceTraits;
        public PlayerTraits VipTraits;
    }
}