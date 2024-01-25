using System;
using TagTool.Tags;

namespace TagTool.MtnDewIt.BlamFiles
{
    public enum JuggernautInitialJuggernautSettings : byte
    {
        Random = 0,
        FirstKill,
        FirstDeath,
    }

    public enum JuggernautNextJuggernautSettings : byte
    {
        OnKillingJuggernaut = 0,
        OnKilledByJuggernaut,
        Unchanged,
        Random,
    }

    [Flags]
    public enum GameVariantJuggernautFlags : byte
    {
        AlliedAgainstJuggernaut = 0,
        RespawnOnLoneJuggernaut,
        DestinationZonesEnabled,
    }

    public enum JuggernautZoneMovementSettings : byte
    {
        Off = 0,
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

    public enum JuggernautZoneOrderSettings : byte
    {
        Random = 0,
        Sequence,
    }


    [TagStructure(Size = 0x200)]
    public class GameVariantJuggernaut : GameVariantBase
    {
        public short ScoreToWinRound;
        public short ScoreUnknown;

        [TagField(Flags = TagFieldFlags.Padding, Length = 2)]
        public byte[] Padding1 = new byte[2];

        public JuggernautInitialJuggernautSettings InitialJuggernaut;
        public JuggernautNextJuggernautSettings NextJuggernaut;
        public GameVariantJuggernautFlags VariantFlags;
        public JuggernautZoneMovementSettings ZoneMovement;
        public JuggernautZoneOrderSettings ZoneOrder;
        public sbyte KillPoints;
        public sbyte JuggernautKillPoints;
        public sbyte KillAsJuggernautPoints;
        public sbyte DestinationArrivalPoints;
        public sbyte SuicidePoints;
        public sbyte BetrayalPoints;
        public byte JuggernautDelay;
        public PlayerTraits JuggernautTraits;

        [TagField(Flags = TagFieldFlags.Padding, Length = 2)]
        public byte[] Padding2 = new byte[2];
    }
}