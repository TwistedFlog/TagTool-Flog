﻿using System;
using System.Collections.Generic;

namespace TagTool.Tags
{
    [TagStructure(Size = 0x8, MaxVersion = Cache.CacheVersion.Halo2Vista)]
    [TagStructure(Size = 0xC, MinVersion = Cache.CacheVersion.Halo3Beta)]
    public class TagBlockBitVector : TagStructure
    {
        public List<BitField> BitFields;

        [TagStructure(Size = 0x4)]
        public class BitField : TagStructure
        {
            public BitVector Bits;

            [Flags]
            public enum BitVector : int
            {
                None = 0,
                Bit0 = 1 << 0,
                Bit1 = 1 << 1,
                Bit2 = 1 << 2,
                Bit3 = 1 << 3,
                Bit4 = 1 << 4,
                Bit5 = 1 << 5,
                Bit6 = 1 << 6,
                Bit7 = 1 << 7,
                Bit8 = 1 << 8,
                Bit9 = 1 << 9,
                Bit10 = 1 << 10,
                Bit11 = 1 << 11,
                Bit12 = 1 << 12,
                Bit13 = 1 << 13,
                Bit14 = 1 << 14,
                Bit15 = 1 << 15,
                Bit16 = 1 << 16,
                Bit17 = 1 << 17,
                Bit18 = 1 << 18,
                Bit19 = 1 << 19,
                Bit20 = 1 << 20,
                Bit21 = 1 << 21,
                Bit22 = 1 << 22,
                Bit23 = 1 << 23,
                Bit24 = 1 << 24,
                Bit25 = 1 << 25,
                Bit26 = 1 << 26,
                Bit27 = 1 << 27,
                Bit28 = 1 << 28,
                Bit29 = 1 << 29,
                Bit30 = 1 << 30,
                Bit31 = 1 << 31,
            }
        }
    }
}