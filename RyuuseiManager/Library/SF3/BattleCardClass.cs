namespace RyuuseiManager.Library.SF3
{
    public static class BattleCardClass
    {
        private static readonly (int Start, int End)[] StandardRanges =
        {
        (1, 150)
    };

        private static readonly (int Start, int End)[] MegaRanges =
        {
        (151, 195)
    };

        private static readonly (int Start, int End)[] GigaBARanges =
        {
        (196, 200)
    };

        private static readonly (int Start, int End)[] GigaRJRanges =
        {
        (201, 205)
    };

        private static readonly (int Start, int End)[] GigaEventRanges =
        {
        (206, 207)
    };

        private static readonly (int Start, int End)[] StandardIllegalRanges =
        {
        (208, 323)
    };

        private static readonly (int Start, int End)[] MegaIllegalRanges =
        {
        (324, 386)
    };

        private static readonly (int Start, int End)[] GigaIllegalRanges =
        {
        (387, 397)
    };

        private static List<int> ExpandRanges(params (int Start, int End)[] ranges)
            => ranges.SelectMany(r => Enumerable.Range(r.Start, r.End - r.Start + 1)).ToList();

        public static List<int> GetAllowedBattleCardList(int gameVersion)
        {
            var allowed = new List<int>();

            allowed.AddRange(ExpandRanges(StandardRanges));
            allowed.AddRange(ExpandRanges(MegaRanges));
            allowed.AddRange(ExpandRanges(GigaEventRanges));
            allowed.AddRange(ExpandRanges(StandardIllegalRanges));
            allowed.AddRange(ExpandRanges(MegaIllegalRanges));
            allowed.AddRange(ExpandRanges(GigaIllegalRanges));

            switch (gameVersion)
            {
                case 0:
                    allowed.AddRange(ExpandRanges(GigaBARanges));
                    break;
                case 1:
                    allowed.AddRange(ExpandRanges(GigaRJRanges));
                    break;
            }

            allowed.Sort();
            return allowed;
        }

        public static List<int> GetStandardCards()
        {
            var allowed = new List<int>();

            allowed.AddRange(ExpandRanges(StandardRanges));
            allowed.AddRange(ExpandRanges(StandardIllegalRanges));
            allowed.Sort();
            return allowed;
        }

        public static List<int> GetMegaCards()
        {
            var allowed = new List<int>();
            allowed.AddRange(ExpandRanges(MegaRanges));
            allowed.AddRange(ExpandRanges(MegaIllegalRanges));
            allowed.Sort();
            return allowed;
        }

        public static List<int> GetGigaCards(int gameVersion)
        {
            var allowed = new List<int>();
            allowed.AddRange(ExpandRanges(GigaEventRanges));
            allowed.AddRange(ExpandRanges(GigaIllegalRanges));
            switch (gameVersion)
            {
                case 0:
                    allowed.AddRange(ExpandRanges(GigaBARanges));
                    break;
                case 1:
                    allowed.AddRange(ExpandRanges(GigaRJRanges));
                    break;
            }
            allowed.Sort();
            return allowed;
        }
    }

}
