using System.Linq;
using TagTool.Cache;
using TagTool.Common;
using TagTool.Tags;

namespace TagTool.MtnDewIt.BlamFiles
{
    [TagStructure(Size = 0x3390, MinVersion = CacheVersion.HaloOnlineED, MaxVersion = CacheVersion.HaloOnline700123)]
    public class CacheFileHeaderDataHaloOnline : CacheFileHeaderData
    {
        public Tag HeaderSignature;

        public CacheFileVersion FileVersion;
        public int FileLength;
        public int FileCompressedLength;

        public uint TagTableHeaderOffset;

        public TagMemoryHeader TagMemoryHeader;

        [TagField(Length = 256)]
        public string SourceFile;

        [TagField(Length = 32)]
        public string Build;

        public CacheFileType CacheType;
        public CacheFileSharedType SharedCacheType;

        public bool Unknown1;
        public bool TrackedBuild;
        public bool HasInsertionPoints;
        public byte HeaderFlags;

        public LastModificationDate ModificationDate;

        [TagField(Length = 12)]
        public byte[] Unknown2;

        public StringIDHeader StringIdsHeader;

        public uint SharedFileFlags;

        public LastModificationDate CreationTime;

        [TagField(Length = 6, MinVersion = CacheVersion.HaloOnlineED, MaxVersion = CacheVersion.HaloOnline106708)]
        [TagField(Length = 8, MinVersion = CacheVersion.HaloOnline235640)]
        public LastModificationDate[] SharedFileTimes;

        [TagField(Length = 0x20)]
        public string Name;

        public GameLanguage GameLanguage;

        [TagField(Length = 256)]
        public string ScenarioPath;

        public int MinorVersion;

        public TagNameHeader TagNamesHeader;

        public SectionFileBounds Reports;

        [TagField(Length = 4)]
        public byte[] Unknown3;

        public FileAuthor Author;

        [TagField(Length = 0x10)]
        public byte[] Unknown4;

        public ulong Unknown5;

        public NetworkRequestHash Hash;

        public RSASignature RSASignature;

        [TagField(Length = 4)]
        public int[] SectionOffsets;

        [TagField(Length = 4)]
        public SectionFileBounds[] OriginalSectionBounds;

        public SharedResourceUsage SharedResourceUsage;

        public int InsertionPointResourceUsageCount;

        [TagField(Length = 9)]
        public InsertionPointResourceUsage[] InsertionPointResourceUsage;

        public int TagCacheOffsets;

        public int TagCount;

        public int MapId;

        public int ScenarioTagIndex;

        public int CacheFileResourceGestaltIndex;

        [TagField(Length = 0x594, MinVersion = CacheVersion.HaloOnlineED, MaxVersion = CacheVersion.HaloOnline106708)]
        [TagField(Length = 0x584, MinVersion = CacheVersion.HaloOnline235640)]
        public byte[] Unknown6;

        public Tag FooterSignature;

        public override Tag GetFootTag() => FooterSignature;
        public override Tag GetHeadTag() => HeaderSignature;
        public override ulong GetTagTableHeaderOffset() => TagTableHeaderOffset;
        public override string GetName() => Name;
        public override string GetBuild() => Build;
        public override string GetScenarioPath() => ScenarioPath;
        public override CacheFileType GetCacheType() => CacheType;
        public override CacheFileSharedType GetSharedCacheType() => SharedCacheType;
        public override StringIDHeader GetStringIDHeader() => StringIdsHeader;
        public override TagNameHeader GetTagNameHeader() => TagNamesHeader;
        public override TagMemoryHeader GetTagMemoryHeader() => TagMemoryHeader;
        public override int GetScenarioTagIndex() => ScenarioTagIndex;


        public static byte[] FileCreatorKey = new byte[64]
        {
            0x05, 0x11, 0x6A, 0xA3, 0xCA, 0xB5, 0x07, 0xDF, 0x50, 0xE7,
            0x5B, 0x75, 0x6B, 0x4A, 0xBB, 0xF4, 0xE8, 0x54, 0x8F, 0xC6,
            0xD6, 0xCC, 0x92, 0x15, 0x97, 0xDC, 0xF5, 0xEE, 0xB9, 0x3C,
            0x01, 0x3C, 0x95, 0xCF, 0xB8, 0x58, 0x5A, 0x6F, 0x2E, 0xB9,
            0x30, 0x6D, 0x89, 0x31, 0x2F, 0x83, 0x6F, 0xF0, 0x9F, 0xE8,
            0x37, 0x78, 0xE4, 0xC7, 0xE2, 0x2B, 0x19, 0x66, 0x11, 0x06,
            0x77, 0x24, 0x74, 0x66
        };

        public static string GetAuthor(byte[] author)
        {
            char[] creatorString = new char[32];

            author.CopyTo(creatorString, 0);

            for (int i = 0; i < 32; i++)
            {
                creatorString[i] ^= (char)FileCreatorKey[i];
            }

            return new string(creatorString.Where(c => c != 0).ToArray());
        }

        public static byte[] SetAuthor(string author)
        {
            char[] creatorArray = new char[32];

            author.ToCharArray().CopyTo(creatorArray, 0);

            for (int i = 0; i < 32; i++)
                creatorArray[i] ^= (char)FileCreatorKey[i];

            byte[] authorBytes = new byte[32];

            for (int i = 0; i < 32; i++)
                authorBytes[i] = (byte)creatorArray[i];

            return authorBytes;
        }
    }

    [TagStructure(Size = 0x8)]
    public class LastModificationDate
    {
        public uint Low;
        public uint High;
    }

    [TagStructure(Size = 0x8)]
    public class SectionFileBounds
    {
        public int Offset;
        public int Size;
    }

    [TagStructure(Size = 0x14)]
    public class NetworkRequestHash
    {
        [TagField(Length = 5)]
        public uint[] Data;
    }

    [TagStructure(Size = 0x100)]
    public class RSASignature
    {
        [TagField(Length = 32)]
        public ulong[] Data;
    }

    [TagStructure(Size = 0x2328)]
    public class SharedResourceUsage
    {
        [TagField(Length = 0x2328)]
        public byte[] Data;
    }

    [TagStructure(Size = 0xB4)]
    public class InsertionPointResourceUsage
    {
        [TagField(Length = 0xB4)]
        public byte[] Data;
    }

    [TagStructure(Size = 0x20)]
    public class FileAuthor
    {
        [TagField(Length = 0x20)]
        public byte[] Data;
    }
}