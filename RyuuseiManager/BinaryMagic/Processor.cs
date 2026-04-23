using System.Text;

namespace RyuuseiManager.BinaryMagic
{
    public class Processor
    {
        public static bool TryGetNextByte(ReadOnlySpan<byte> source, ReadOnlySpan<byte> header, out byte nextByte)
        {
            nextByte = default;
            if (!source.StartsWith(header))
                return false;
            if (source.Length <= header.Length)
                return false;
            nextByte = source[header.Length];
            return true;
        }

        public static byte[] StripSwitchSave(ReadOnlySpan<byte> blob, int gameID)
        {
            if (blob.StartsWith(HeaderMagic.Switch)) blob = blob.Slice(HeaderMagic.Switch.Length).ToArray();
            int footerLength = FooterMagic.Eof.Length;

            for (int i = 0; i <= blob.Length - footerLength; i++)
            {
                var slice = blob.Slice(i, footerLength);
                bool match = true;
                for (int j = 0; j < footerLength; j++)
                {
                    if (j == 12) continue;
                    if (slice[j] != FooterMagic.Eof[j])
                    {
                        match = false;
                        break;
                    }
                }
                if (match)
                {
                    blob = blob.Slice(0, i + footerLength).ToArray();
                }
            }

            byte[] pattern;
            switch (gameID)
            {
                case 1:
                    pattern = PlatformMagic.SF1; break;
                case 2:
                    pattern = PlatformMagic.SF2; break;
                case 3:
                    pattern = PlatformMagic.SF3; break;
                default:
                    return blob.ToArray();
            }
            byte[] source = blob.ToArray();
            for (int i = 0; i <= blob.Length - pattern.Length; i++)
            {
                if (blob.Slice(i, pattern.Length).SequenceEqual(pattern) && i >= 8)
                {
                    if (blob[i - 8] == 0x08 &&
                        blob[i - 7] == 0x00 &&
                        blob[i - 6] == 0x00 &&
                        blob[i - 5] == 0x00)
                    {
                        // Remove those 4 bytes
                        var result = new byte[source.Length - 4];
                        Buffer.BlockCopy(source, 0, result, 0, i - 4);
                        Buffer.BlockCopy(source, i, result, i - 4, source.Length - i);
                        return result;
                    }
                }
            }
            return source;
        }

        public static int GetMugshotID(ReadOnlySpan<byte> blob, int gameID)
        {
            int mugshotOffset = 0;
            switch (gameID)
            {
                case 1:
                    mugshotOffset = Offset.Absolute.SF1.Mugshot; break;
                case 2:
                    mugshotOffset = Offset.Absolute.SF2.Mugshot; break;
                case 3:
                    return 278;
                    mugshotOffset = Offset.Absolute.SF3.NoiseForm;
                    int noiseForm = (int)BitConverter.ToUInt16(blob.ToArray(), mugshotOffset);
                    switch (noiseForm)
                    {
                        default:
                            return 278;
                        case 0: // cygnus
                            return 335;
                        case 1: // ophiuca
                            return 333;
                        case 2: // burai
                            return 345;
                        case 3: // wolf
                            return 343;
                        case 4: // cancer
                            return 329;
                        case 5: // gemini
                            return 331;
                        case 6: // libra
                            return 325;
                        case 7: // no form
                            return 278;
                        case 8: // corvus
                            return 327;
                        case 9: // virgo
                            return 339;
                        case 10: // crown
                            return 341;
                        case 11: // ox
                            return 337;
                    }
            }
            return (int)BitConverter.ToUInt16(blob.ToArray(), mugshotOffset);
        }

        public static string GetMessage(ReadOnlySpan<byte> blob, int gameID)
        {
            int endIndex = -1;
            switch (gameID)
            {
                case 1:
                    endIndex = blob.IndexOf(FooterMagic.SF1.MessageFooterMagic);
                    break;
                case 2:
                    endIndex = blob.IndexOf(FooterMagic.SF2.MessageFooterMagic);
                    break;
                case 3:
                    endIndex = blob.IndexOf(FooterMagic.SF3.MessageFooterMagic);
                    break;
            }
            if (endIndex < 0) return "";

            var beforeEnd = blob.Slice(0, endIndex);
            int startIndex = 0;
            switch (gameID)
            {
                case 1:
                    startIndex = beforeEnd.LastIndexOf(HeaderMagic.SF1.HeaderMagic) + HeaderMagic.SF1.HeaderMagic.Length + 4;
                    break;
                case 2:
                    startIndex = beforeEnd.LastIndexOf(HeaderMagic.SF2.HeaderMagic) + HeaderMagic.SF2.HeaderMagic.Length + 4;
                    break;
                case 3:
                    startIndex = beforeEnd.LastIndexOf(HeaderMagic.SF3.HeaderMagic) + HeaderMagic.SF3.HeaderMagic.Length + 4;
                    break;

            }
            var contentBytes = blob.Slice(startIndex, endIndex - startIndex).ToArray();
            if (contentBytes.Length > 0 && contentBytes[0] < 0x80)
            {
                contentBytes = new byte[] { 0xFF, 0xFE }.Concat(contentBytes).ToArray();
            }
            return Encoding.Unicode.GetString(contentBytes);
        }

        public static string GetSecret(ReadOnlySpan<byte> blob, int gameID)
        {
            int endIndex = -1;
            switch (gameID)
            {
                case 1:
                    endIndex = blob.IndexOf(FooterMagic.SF1.SecretFooterMagic);
                    break;
                case 2:
                    endIndex = blob.IndexOf(FooterMagic.SF2.SecretFooterMagic);
                    break;
                case 3:
                    endIndex = blob.IndexOf(FooterMagic.SF3.TeamNameFooterMagic);
                    break;
            }
            if (endIndex < 0) return "";

            var beforeEnd = blob.Slice(0, endIndex);
            int startIndex = 0;
            switch (gameID)
            {
                case 1:
                    startIndex = beforeEnd.LastIndexOf(HeaderMagic.SF1.HeaderMagic) + HeaderMagic.SF1.HeaderMagic.Length + 4;
                    break;
                case 2:
                    startIndex = beforeEnd.LastIndexOf(HeaderMagic.SF2.HeaderMagic) + HeaderMagic.SF2.HeaderMagic.Length + 4;
                    break;
                case 3:
                    startIndex = beforeEnd.LastIndexOf(HeaderMagic.SF3.HeaderMagic) + HeaderMagic.SF3.HeaderMagic.Length + 4;
                    break;
            }
            var contentBytes = blob.Slice(startIndex, endIndex - startIndex).ToArray();
            if (contentBytes.Length > 0 && contentBytes[0] < 0x80)
            {
                contentBytes = new byte[] { 0xFF, 0xFE }.Concat(contentBytes).ToArray();
            }
            return Encoding.Unicode.GetString(contentBytes);
        }
    }
}
