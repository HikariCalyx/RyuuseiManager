using RyuuseiManager.Classes;
using System.Buffers.Binary;
using System.Text;

namespace RyuuseiManager.BinaryMagic
{
    public class Processor
    {
        private const byte BoundaryByte = 0x08;
        private const int CardDataSize = 60;
        private const int MinFolderNameLength = 0x01;
        private const int MaxFolderNameLength = 0x08;
        private const int SF3FolderBlobSize = 188;

        public static bool TryGetNextByte(ReadOnlySpan<byte> source, ReadOnlySpan<byte> header, out byte nextByte)
        {
            nextByte = default;
            if (!source.StartsWith(header) || source.Length <= header.Length)
                return false;

            nextByte = source[header.Length];
            return true;
        }

        public static byte[] RepopulateFooter(ReadOnlySpan<byte> blob, int targetGameID)
        {
            int footerOffset = blob.IndexOf(FooterMagic.SaveFooterMagic) + FooterMagic.SaveFooterMagic.Length;
            byte[] footerData = GetFooterByGameID(targetGameID);

            if (footerData == null)
                return blob.ToArray();

            var result = new byte[footerOffset + footerData.Length];
            blob.Slice(0, footerOffset).CopyTo(result);
            footerData.AsSpan().CopyTo(result.AsSpan(footerOffset));

            return result;
        }

        private static byte[] GetFooterByGameID(int gameID) => gameID switch
        {
            10 => FooterMagic.SF1.Pegasus.zh_CN,
            11 => FooterMagic.SF1.Leo.zh_CN,
            12 => FooterMagic.SF1.Dragon.zh_CN,
            20 => FooterMagic.SF2.Ninja.zh_CN,
            21 => FooterMagic.SF2.Saurian.zh_CN,
            22 => FooterMagic.SF2.ZerkerNinja.zh_CN,
            23 => FooterMagic.SF2.ZerkerSaurian.zh_CN,
            30 => FooterMagic.SF3.BlackAce1.zh_CN,
            31 => FooterMagic.SF3.BlackAce2.zh_CN,
            32 => FooterMagic.SF3.RedJoker1.zh_CN,
            33 => FooterMagic.SF3.RedJoker2.zh_CN,
            _ => null
        };

        public static byte[] StripSwitchSave(ReadOnlySpan<byte> blob)
        {
            // Remove Switch header if present
            if (blob.StartsWith(HeaderMagic.Switch))
                blob = blob.Slice(HeaderMagic.Switch.Length);

            // Find and trim to EOF footer
            blob = TrimToEofFooter(blob);

            int gameID = blob[^4] / 10;
            byte[] pattern = GetPlatformPattern(gameID);

            if (pattern == null)
                return blob.ToArray();

            // Find and remove platform magic prefix
            return RemovePlatformMagicPrefix(blob, pattern);
        }

        private static ReadOnlySpan<byte> TrimToEofFooter(ReadOnlySpan<byte> blob)
        {
            int footerLength = FooterMagic.Eof.Length;

            for (int i = 0; i <= blob.Length - footerLength; i++)
            {
                if (MatchesEofFooter(blob.Slice(i, footerLength)))
                {
                    return blob.Slice(0, i + footerLength);
                }
            }
            return blob;
        }

        private static bool MatchesEofFooter(ReadOnlySpan<byte> slice)
        {
            for (int j = 0; j < slice.Length; j++)
            {
                if (j != 12 && slice[j] != FooterMagic.Eof[j])
                    return false;
            }
            return true;
        }

        private static byte[] GetPlatformPattern(int gameID) => gameID switch
        {
            1 => PlatformMagic.SF1,
            2 => PlatformMagic.SF2,
            3 => PlatformMagic.SF3,
            _ => null
        };

        private static byte[] RemovePlatformMagicPrefix(ReadOnlySpan<byte> blob, ReadOnlySpan<byte> pattern)
        {
            byte[] source = blob.ToArray();

            for (int i = 0; i <= blob.Length - pattern.Length; i++)
            {
                if (blob.Slice(i, pattern.Length).SequenceEqual(pattern) && i >= 8)
                {
                    if (IsPlatformMagicPrefix(blob, i))
                    {
                        // Remove the 4-byte prefix
                        var result = new byte[source.Length - 4];
                        Buffer.BlockCopy(source, 0, result, 0, i - 4);
                        Buffer.BlockCopy(source, i, result, i - 4, source.Length - i);
                        return result;
                    }
                }
            }
            return source;
        }

        private static bool IsPlatformMagicPrefix(ReadOnlySpan<byte> blob, int index)
        {
            return blob[index - 8] == 0x08 &&
                   blob[index - 7] == 0x00 &&
                   blob[index - 6] == 0x00 &&
                   blob[index - 5] == 0x00;
        }

        public static int GetGameGen(ReadOnlySpan<byte> blob)
        {
            return blob[^4];
        }

        public static byte[] PopulateToSwitchSave(ReadOnlySpan<byte> blob, int gameID)
        {
            var config = GetSwitchSaveConfig(gameID);
            if (config == null)
                return blob.ToArray();

            byte[] source = blob.ToArray();
            int patternIndex = blob.IndexOf(config.Value.Pattern);
            if (patternIndex < 0)
                return source;

            var before = blob.Slice(0, patternIndex).ToArray();
            var after = blob.Slice(patternIndex).ToArray();

            byte[] switchSave = BuildSwitchSave(before, after, gameID, config.Value);
            byte[] murmurChecksum = BitConverter.GetBytes(MurmurHash3.Hash32(switchSave, 0xFFFFFFFF));

            var result = new byte[switchSave.Length + murmurChecksum.Length];
            Buffer.BlockCopy(switchSave, 0, result, 0, switchSave.Length);
            Buffer.BlockCopy(murmurChecksum, 0, result, switchSave.Length, murmurChecksum.Length);
            return result;
        }

        private static (byte[] Pattern, byte[] SwitchFooterMagic, byte[] Filler)? GetSwitchSaveConfig(int gameID) =>
            gameID switch
            {
                1 => (PlatformMagic.SF1, FooterMagic.SF1.SwitchFooterMagic, new byte[] { 0xBF, 0x75, 0xED, 0x75 }),
                2 => (PlatformMagic.SF2, FooterMagic.SF2.SwitchFooterMagic, new byte[] { 0x04, 0x00, 0x00, 0x00 }),
                3 => (PlatformMagic.SF3, FooterMagic.SF3.SwitchFooterMagic, new byte[] { 0x00, 0x00, 0x00, 0x00 }),
                _ => null
            };

        private static byte[] BuildSwitchSave(byte[] before, byte[] after, int gameID,
            (byte[] Pattern, byte[] SwitchFooterMagic, byte[] Filler) config)
        {
            var parts = new List<byte[]>
            {
                HeaderMagic.Switch,
                before,
                config.Filler,
                after,
                FooterMagic.SwitchSaveFooterMagic
            };

            if (gameID != 1)
                parts.Add(new byte[] { 0x04, 0x00, 0x00, 0x00 });

            parts.Add(config.SwitchFooterMagic);

            int totalLength = parts.Sum(p => p.Length);
            var result = new byte[totalLength];
            int offset = 0;

            foreach (var part in parts)
            {
                Buffer.BlockCopy(part, 0, result, offset, part.Length);
                offset += part.Length;
            }

            return result;
        }

        public static int GetMugshotID(ReadOnlySpan<byte> blob, int gameID)
        {
            return gameID switch
            {
                1 => (int)BitConverter.ToUInt16(blob.ToArray(), Offset.Absolute.SF1.Mugshot),
                2 => (int)BitConverter.ToUInt16(blob.ToArray(), Offset.Absolute.SF2.Mugshot),
                3 => GetSF3MugshotID(blob),
                _ => 0
            };
        }

        private static int GetSF3MugshotID(ReadOnlySpan<byte> blob)
        {
            int noiseForm = (int)BitConverter.ToUInt16(blob.ToArray(), Offset.Absolute.SF3.NoiseForm);
            return noiseForm switch
            {
                0 => 335,  // cygnus
                1 => 333,  // ophiuca
                2 => 345,  // burai
                3 => 343,  // wolf
                4 => 329,  // cancer
                5 => 331,  // gemini
                6 => 325,  // libra
                7 => 278,  // no form
                8 => 327,  // corvus
                9 => 339,  // virgo
                10 => 341, // crown
                11 => 337, // ox
                _ => 278
            };
        }

        public static string GetMessage(ReadOnlySpan<byte> blob, int gameID)
        {
            return ExtractTextByGameID(blob, gameID, isSecret: false);
        }

        public static string GetSecret(ReadOnlySpan<byte> blob, int gameID)
        {
            return ExtractTextByGameID(blob, gameID, isSecret: true);
        }

        private static string ExtractTextByGameID(ReadOnlySpan<byte> blob, int gameID, bool isSecret)
        {
            var footerMagic = GetTextFooterMagic(gameID, isSecret);
            var headerMagic = GetHeaderMagic(gameID);

            if (footerMagic == null || headerMagic == null)
                return "";

            int endIndex = blob.IndexOf(footerMagic);
            if (endIndex < 0)
                return "";

            var beforeEnd = blob.Slice(0, endIndex);
            int startIndex = beforeEnd.LastIndexOf(headerMagic) + headerMagic.Length + 4;

            var contentBytes = blob.Slice(startIndex, endIndex - startIndex).ToArray();
            return DecodeUnicodeWithBOM(contentBytes);
        }

        private static ReadOnlySpan<byte> GetHeaderMagic(int gameID) => gameID switch
        {
            1 => HeaderMagic.SF1.HeaderMagic,
            2 => HeaderMagic.SF2.HeaderMagic,
            3 => HeaderMagic.SF3.HeaderMagic,
            _ => null
        };

        private static ReadOnlySpan<byte> GetTextFooterMagic(int gameID, bool isSecret) =>
            (gameID, isSecret) switch
            {
                (1, false) => FooterMagic.SF1.MessageFooterMagic,
                (1, true) => FooterMagic.SF1.SecretFooterMagic,
                (2, false) => FooterMagic.SF2.MessageFooterMagic,
                (2, true) => FooterMagic.SF2.SecretFooterMagic,
                (3, false) => FooterMagic.SF3.MessageFooterMagic,
                (3, true) => FooterMagic.SF3.TeamNameFooterMagic,
                _ => null
            };

        private static string DecodeUnicodeWithBOM(byte[] contentBytes)
        {
            if (contentBytes.Length > 0 && contentBytes[0] < 0x80)
            {
                // Add UTF-16 LE BOM
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
            var resultFolders = new List<Folder>();

            if (gameID != 3)
                return resultFolders;

            var folderBlobs = GetSF3FolderBlob(blob, HeaderMagic.SF3.FolderHeaderMagic);
            foreach (var folderBlob in folderBlobs)
            {
                var folder = ParseSF3Folder(folderBlob);
                if (folder != null)
                    resultFolders.Add(folder);
            }

            return resultFolders;
        }

        private static Folder ParseSF3Folder(byte[] folderBlob)
        {
            int folderNameLength = folderBlob[0];
            if (folderNameLength < MinFolderNameLength || folderNameLength > MaxFolderNameLength)
                return null;

            var folder = new Folder
            {
                FolderName = Encoding.Unicode.GetString(folderBlob.AsSpan(4, folderNameLength * 2))
            };

            // Extract card data
            int cardDataFooterIndex = folderBlob.IndexOf(FooterMagic.SF3.CardListFooterMagic);
            if (cardDataFooterIndex > CardDataSize)
            {
                byte[] cardData = folderBlob.AsSpan(cardDataFooterIndex - CardDataSize, CardDataSize).ToArray();
                for (int i = 0; i < cardData.Length; i += 2)
                {
                    folder.Cards.Add(BitConverter.ToUInt16(cardData, i));
                }
            }

            // Extract regular card index
            int regCardIndex = folderBlob[folderBlob.IndexOf(HeaderMagic.SF3.RegCardHeaderMagic) +
                                         HeaderMagic.SF3.RegCardHeaderMagic.Length];
            if (regCardIndex < 30)
                folder.RegularCardIndex = regCardIndex;

            // Extract tag cards
            int tagCardIndex = folderBlob.IndexOf(FooterMagic.SF3.TagCardIndexFootermagic) - 4;
            if (tagCardIndex >= 0)
            {
                folder.TagCards[0] = folderBlob[tagCardIndex];
                folder.TagCards[1] = folderBlob[tagCardIndex + 1];
            }

            return folder;
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
                int length = Math.Min(SF3FolderBlobSize, source.Length - start);

                results.Add(source.Slice(start, length).ToArray());

                offset = foundAt + pattern.Length;
            }

            return results;
        }
    }
}

