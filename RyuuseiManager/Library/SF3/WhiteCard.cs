namespace RyuuseiManager.Library.SF3
{
    public class WhiteCard
    {
        public WhiteCard()
        {
            Card1 = BattleCard.Cannon;
            Card2 = BattleCard.Cannon;
            Card3 = BattleCard.Cannon;
            Card4 = BattleCard.Cannon;
        }

        public BattleCard Card1 { get; set; }
        public BattleCard Card2 { get; set; }
        public BattleCard Card3 { get; set; }
        public BattleCard Card4 { get; set; }

        public Dictionary<BattleCard, int> ToDict()
        {
            Dictionary<BattleCard, int> dict = new Dictionary<BattleCard, int>();
            foreach (var i in new[] { Card1, Card2, Card3, Card4 })
            {
                if (dict.ContainsKey(i))
                {
                    dict[i] += 1;
                }
                else
                {
                    dict[i] = 1;
                }
            }
            return dict;
        }
    }
}
