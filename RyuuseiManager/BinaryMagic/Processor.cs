using RyuuseiManager.Classes;
using System.Buffers.Binary;
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

        public static byte[] RepopulateFooter(ReadOnlySpan<byte> blob, int targetGameID)
        {
            int footerOffset = blob.IndexOf(FooterMagic.SaveFooterMagic) + FooterMagic.SaveFooterMagic.Length;
            byte[] result = blob.Slice(0, footerOffset).ToArray();
            switch (targetGameID)
            {
                case 10:
                    result = result.Concat(FooterMagic.SF1.Pegasus.zh_CN).ToArray(); break;
                case 11:
                    result = result.Concat(FooterMagic.SF1.Leo.zh_CN).ToArray(); break;
                case 12:
                    result = result.Concat(FooterMagic.SF1.Dragon.zh_CN).ToArray(); break;
                case 20:
                    result = result.Concat(FooterMagic.SF2.Ninja.zh_CN).ToArray(); break;
                case 21:
                    result = result.Concat(FooterMagic.SF2.Saurian.zh_CN).ToArray(); break;
                case 22:
                    result = result.Concat(FooterMagic.SF2.ZerkerNinja.zh_CN).ToArray(); break;
                case 23:
                    result = result.Concat(FooterMagic.SF2.ZerkerSaurian.zh_CN).ToArray(); break;
                case 30:
                    result = result.Concat(FooterMagic.SF3.BlackAce1.zh_CN).ToArray(); break;
                case 31:
                    result = result.Concat(FooterMagic.SF3.BlackAce2.zh_CN).ToArray(); break;
                case 32:
                    result = result.Concat(FooterMagic.SF3.RedJoker1.zh_CN).ToArray(); break;
                case 33:
                    result = result.Concat(FooterMagic.SF3.RedJoker2.zh_CN).ToArray(); break;
                default:
                    result = blob.ToArray(); break;
            }
            return result;
        }

        public static byte[] StripSwitchSave(ReadOnlySpan<byte> blob)
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
            int gameID = blob[^4] / 10;

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

        public static int GetGameGen(ReadOnlySpan<byte> blob)
        {
            return blob[^4];
        }

        public static byte[] PopulateToSwitchSave(ReadOnlySpan<byte> blob, int gameID)
        {
            byte[] pattern;
            byte[] footerMagic;
            byte[] filler;
            switch (gameID)
            {
                case 1:
                    pattern = PlatformMagic.SF1;
                    footerMagic = FooterMagic.SF1.SwitchFooterMagic;
                    filler = new byte[] { 0xBF, 0x75, 0xED, 0x75 }; break;
                case 2:
                    pattern = PlatformMagic.SF2;
                    footerMagic = FooterMagic.SF2.SwitchFooterMagic;
                    filler = new byte[] { 0x04, 0x00, 0x00, 0x00 }; break;
                case 3:
                    pattern = PlatformMagic.SF3;
                    footerMagic = FooterMagic.SF3.SwitchFooterMagic;
                    filler = new byte[] { 0x00, 0x00, 0x00, 0x00 }; break;
                default:
                    return blob.ToArray();
            }
            byte[] source = blob.ToArray();
            byte[] switchSave = Array.Empty<byte>();
            int patternIndex = blob.IndexOf(pattern);
            if (patternIndex < 0) return source;
            else
            {
                var before = blob.Slice(0, patternIndex).ToArray();
                var after = blob.Slice(patternIndex).ToArray();

                switchSave = HeaderMagic.Switch
                    .Concat(before)
                    .Concat(filler)
                    .Concat(after)
                    .Concat(FooterMagic.SwitchSaveFooterMagic)
                    .Concat(gameID == 1 ? Array.Empty<byte>() : new byte[] { 0x04, 0x00, 0x00, 0x00 })
                    .Concat(footerMagic)
                    .ToArray();
            }
            byte[] murmurChecksum = BitConverter.GetBytes(MurmurHash3.Hash32(switchSave, 0xFFFFFFFF));
            return switchSave.Concat(murmurChecksum).ToArray();
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

        public static int GetSF3TeamIconID(ReadOnlySpan<byte> blob)
        {
            int startIndex = -1;
            startIndex = blob.IndexOf(HeaderMagic.SF3.TeamIconHeaderMagic);
            if (startIndex < 0) return 0;

            int teamIconId = blob[startIndex + HeaderMagic.SF3.TeamIconHeaderMagic.Length] - 1;
            return teamIconId;
        }

        public static string GetSF3TeamPurpose(ReadOnlySpan<byte> blob)
        {
            int endIndex = -1;
            endIndex = blob.IndexOf(FooterMagic.SF3.TeamPurposeFooterMagic);
            if (endIndex < 0) return "";

            var beforeEnd = blob.Slice(0, endIndex);
            int startIndex = beforeEnd.LastIndexOf(HeaderMagic.SF3.TeamPurposeHeaderMagic) + HeaderMagic.SF3.TeamPurposeHeaderMagic.Length;

            var contentBytes = blob.Slice(startIndex, endIndex - startIndex).ToArray();
            contentBytes = new byte[] { 0xFF, 0xFE }.Concat(contentBytes).ToArray();
            return Encoding.Unicode.GetString(contentBytes);

        }

        public static List<Folder> GetFolders(ReadOnlySpan<byte> blob, int gameID)
        {
            List<Folder> resultFldr = new List<Folder>();
            switch (gameID)
            {
                case 3:
                    List<byte[]> fldrBlobs = GetSF3FolderBlob(blob, HeaderMagic.SF3.FolderHeaderMagic);
                    foreach (var subBlob in fldrBlobs)
                    {
                        int fldrNameLength = subBlob[0];
                        if (fldrNameLength < 0x01 || fldrNameLength > 0x08) continue;
                        Folder sf3Fldr = new Folder();
                        sf3Fldr.FolderName = Encoding.Unicode.GetString(subBlob.AsSpan(4, fldrNameLength * 2));
                        int cardDataFooterIndex = subBlob.IndexOf(FooterMagic.SF3.CardListFooterMagic);
                        if (cardDataFooterIndex > 60)
                        {
                            byte[] CardData = subBlob.AsSpan(cardDataFooterIndex - 60, 60).ToArray();
                            for (int i = 0; i < CardData.Length; i += 2)
                            {
                                sf3Fldr.Cards.Add(BitConverter.ToUInt16(CardData, i));
                            }
                        }
                        int regCardIndex = subBlob[subBlob.IndexOf(HeaderMagic.SF3.RegCardHeaderMagic) + HeaderMagic.SF3.RegCardHeaderMagic.Length];
                        if (regCardIndex < 30) sf3Fldr.RegularCardIndex = regCardIndex;
                        int tagCardIndex = subBlob.IndexOf(FooterMagic.SF3.TagCardIndexFootermagic) - 4;
                        if (tagCardIndex >= 0)
                        {
                            sf3Fldr.TagCards[0] = subBlob[tagCardIndex];
                            sf3Fldr.TagCards[1] = subBlob[tagCardIndex + 1];
                        }
                        resultFldr.Add(sf3Fldr);
                    }
                    break;
            }
            return resultFldr;
        }

        public static List<int> GetAbilities(ReadOnlySpan<byte> blob, int gameID)
        {
            List<int> abilities = new List<int>();
            switch (gameID)
            {
                case 3:
                    for (int pos = Offset.Absolute.SF3.Ability; pos + 1 < blob.Length; pos += 2)
                    {
                        int value = BinaryPrimitives.ReadUInt16LittleEndian(blob.Slice(pos, 2));
                        if (value == 0) break;

                        abilities.Add(value);
                    }
                    break;
            }
            return abilities;
        }

        public static int GetSF3SelfWhiteCard(ReadOnlySpan<byte> blob)
        {
            int result = 0xFF;
            int index = blob.IndexOf(HeaderMagic.SF3.WhiteCardHeaderMagic);
            if (index >= 0)
            {
                result = blob[index + HeaderMagic.SF3.WhiteCardHeaderMagic.Length];
            }
            return result;
        }

        public static int GetSF3EquippedFolder(ReadOnlySpan<byte> blob)
        {
            int result = 0;
            int index = blob.IndexOf(HeaderMagic.SF3.EquippedFolderHeaderMagic);
            if (index >= 0)
            {
                result = blob[index + HeaderMagic.SF3.EquippedFolderHeaderMagic.Length];
            }
            return result;
        }

        private static List<byte[]> GetSF3FolderBlob(ReadOnlySpan<byte> source, ReadOnlySpan<byte> pattern)
        {
            var results = new List<byte[]>();

            if (pattern.Length == 0 || source.Length < pattern.Length)
                return results;

            int offset = 0;

            while (true)
            {
                int index = source.Slice(offset).IndexOf(pattern);
                if (index < 0)
                    break;

                int foundAt = offset + index;

                int start = foundAt + pattern.Length;
                int length = Math.Min(188, source.Length - start);

                results.Add(source.Slice(start, length).ToArray());

                offset = foundAt + pattern.Length;
            }
            return results;
        }
    }
}
