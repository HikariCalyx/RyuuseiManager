namespace RyuuseiManager.Library.SF3
{
    public class BattleCardClass
    {
        public static readonly List<int> Standard = Enumerable.Range(1, 151).ToList(); // max 5ea
        public static readonly List<int> Mega = Enumerable.Range(151, 196).ToList();  // max 1ea, total 5 max in baseline
        public static readonly List<int> GigaBA = Enumerable.Range(196, 201).ToList(); // max 1ea, total 1 max in baseline, not allowed to equip in RJ
        public static readonly List<int> GigaRJ = Enumerable.Range(201, 206).ToList(); // max 1ea, total 1 max in baseline, not allowed to equip in BA
        public static readonly List<int> GigaEvent = Enumerable.Range(206, 208).ToList(); // max 1ea total 1 max in baseline, sum along with GigaBA/RJ/Illegal

        public static readonly List<int> StandardIllegal = Enumerable.Range(208, 324).ToList(); // max 5ea
        public static readonly List<int> MegaIllegal = Enumerable.Range(324, 387).ToList(); // max 1ea, total 5 max in baseline
        public static readonly List<int> GigaIllegal = Enumerable.Range(387, 398).ToList(); // max 1ea total 1 max in baseline, sum along with GigaBA/RJ/Event

        public static List<int> GetAllowedBattleCardList(int gameVersion)
        {
            List<int> allowedCardId = new List<int>();
            allowedCardId = allowedCardId.
                Concat(Standard).
                Concat(Mega).
                Concat(GigaEvent).
                Concat(StandardIllegal).
                Concat(MegaIllegal).
                Concat(GigaIllegal).
                ToList();
            switch (gameVersion)
            {
                case 0:
                    allowedCardId = allowedCardId.Concat(GigaBA).ToList();
                    break;
                case 1:
                    allowedCardId = allowedCardId.Concat(GigaRJ).ToList();
                    break;
            }
            allowedCardId.Sort();
            return allowedCardId;
        }
    }
}
