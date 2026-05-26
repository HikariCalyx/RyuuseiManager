using System;
using System.Collections.Generic;
using System.Text;

namespace RyuuseiManager.Library.SF3
{
    public static class BattleCardName
    {
        private static readonly Dictionary<BattleCard, string> en = new()
        {
            { BattleCard.Cannon, "Cannon" },
            { BattleCard.PlusCannon, "Plus Cannon" },
            { BattleCard.HeavyCannon, "Heavy Cannon" },
            { BattleCard.PlasmaGun, "Plasma Gun" },
            { BattleCard.AirSpread1, "Air Spread 1" },
            { BattleCard.EdoBlade1, "Bushido 1" },
            { BattleCard.EdoBlade2, "Bushido 2" },
            { BattleCard.EdoBlade3, "Bushido 3" },
            { BattleCard.SpadeMagnets, "Spade Magnes" },
            { BattleCard.DiaIcebahn, "Diamond Ice" },
            { BattleCard.OxFire, "Taurus Fire" },
        };

        private static readonly Dictionary<BattleCard, string> ja = new()
        {
            { BattleCard.EdoBlade1, "エドギリブレード1" },
            { BattleCard.EdoBlade2, "エドギリブレード2" },
            { BattleCard.EdoBlade3, "エドギリブレード3" },
        };

        private static readonly Dictionary<BattleCard, string> zh_cn = new()
        {
            { BattleCard.EdoBlade1, "浪客一刀斩1" },
            { BattleCard.EdoBlade2, "浪客一刀斩2" },
            { BattleCard.EdoBlade3, "浪客一刀斩3" },
        };

        private static readonly Dictionary<BattleCard, string> zh_tw = new()
        {

        };
    }
}
