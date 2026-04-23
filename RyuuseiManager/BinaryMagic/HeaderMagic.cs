namespace RyuuseiManager.BinaryMagic
{
    public class HeaderMagic
    {
        // Header Magic
        // The header magic here would check if the save data is encrypted from Steam, or from Switch, or raw.
        public static readonly byte[] Steam = new byte[] { 0x44, 0x53, 0x53, 0x53, 0x02, 0x00, 0x00, 0x00, 0x10, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
        public static readonly byte[] Switch = new byte[] { 0x44, 0x53, 0x53, 0x53, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
        // Next byte after the raw header should be this:
        // 0x43: SF1
        // 0x45: SF2
        // 0x53: SF3
        public static readonly byte[] Raw = new byte[] { 0x99, 0xF1, 0xE3, 0xDB };

        public class SF1
        {
            public static readonly byte[] HeaderMagic = new byte[] { 0x0F, 0x00, 0x00, 0x00 };
        }

        public class SF2
        {
            public static readonly byte[] HeaderMagic = new byte[] { 0x0F, 0x00, 0x00, 0x00 };
        }

        public class SF3
        {
            public static readonly byte[] HeaderMagic = new byte[] { 0x0F, 0x00, 0x00, 0x00 };
        }

    }
}
