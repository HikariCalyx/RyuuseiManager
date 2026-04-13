namespace RyuuseiManager.BinaryMagic
{
    public class PlatformMagic
    {
        // Platform Specific Magic
        // By checking if there are 7 empty bytes (instead of 3) to know if the save data is Switch specific or Steam specific.
        // Switch specific: 7 empty bytes
        // Steam specific: 3 empty bytes
        public static readonly byte[] SF1 = new byte[] { 0x0A, 0xEB, 0x6E, 0xE5, 0x6A, 0x12 };
        public static readonly byte[] SF2 = new byte[] { 0x72, 0x2D, 0x6F, 0xE5, 0x6A, 0x12 };
        public static readonly byte[] SF3 = new byte[] { 0xF7, 0x37, 0xBD, 0xEA, 0x6A, 0x12 };
    }
}
