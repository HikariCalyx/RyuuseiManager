namespace RyuuseiManager.Library.SF3
{
    public class GaCombo
    {
        public static readonly Dictionary<BattleCard, List<BattleCard>> gaCombos = new()
        {
            // Standard
            { BattleCard.ImpactCannon, new List<BattleCard>() { BattleCard.Cannon, BattleCard.Cannon, BattleCard.Cannon } },
            { BattleCard.BigGrenade, new List<BattleCard>() { BattleCard.MiniGrenade, BattleCard.MiniGrenade, BattleCard.MiniGrenade } },
            { BattleCard.GiantAxe, new List<BattleCard>() { BattleCard.Sword, BattleCard.WideSword, BattleCard.LongSword } },
            { BattleCard.HurricaneDance, new List<BattleCard>() { BattleCard.Sword, BattleCard.Sword, BattleCard.DashAttack1 } },
            { BattleCard.PlatinumMeteor, new List<BattleCard>() { BattleCard.MadBalkan1, BattleCard.AttackPlus10, BattleCard.WhiteMeteor } },
            // Mega
            { BattleCard.SpadeMagnetsGA, new List<BattleCard> { BattleCard.FlashSpire1, BattleCard.FlashSpire2, BattleCard.FlashSpire3 } },
            { BattleCard.DiaIcebahnGA, new List<BattleCard> { BattleCard.IceSpinning1, BattleCard.IceSpinning2, BattleCard.IceSpinning3 } },
            { BattleCard.ClubStrongGA, new List<BattleCard> { BattleCard.PowderShoot1, BattleCard.PowderShoot2, BattleCard.PowderShoot3 } },
            { BattleCard.QueenVirgoGA, new List<BattleCard> { BattleCard.WideWave1, BattleCard.WideWave2, BattleCard.WideWave3 } },
            { BattleCard.JackCorvusGA, new List<BattleCard> { BattleCard.IllegalBobonBomb1, BattleCard.IllegalBobonBomb2, BattleCard.IllegalBobonBomb3 } },
            { BattleCard.GraveJokerGA, new List<BattleCard> { BattleCard.EdoBlade1, BattleCard.EdoBlade2, BattleCard.EdoBlade3 } },
            { BattleCard.AcidAceGA, new List<BattleCard> { BattleCard.MadBalkan1, BattleCard.MadBalkan2, BattleCard.MadBalkan3 } },
            { BattleCard.OxFireGA, new List<BattleCard> { BattleCard.AngerFire1, BattleCard.AngerFire2, BattleCard.AngerFire3 } },
            { BattleCard.CygnusWingGA, new List<BattleCard> { BattleCard.GroundWave1, BattleCard.GroundWave2, BattleCard.GroundWave3 } },
            { BattleCard.WolfForestGA, new List<BattleCard> { BattleCard.Shurishuriken1, BattleCard.Shurishuriken2, BattleCard.Shurishuriken3 } },
            { BattleCard.PhantomBlackGA, new List<BattleCard> { BattleCard.SmileCoin1, BattleCard.SmileCoin2, BattleCard.SmileCoin3 } },
            { BattleCard.BuraiGA, new List<BattleCard> { BattleCard.MuTechnology1, BattleCard.MuTechnology2, BattleCard.MuTechnology3 } },
            { BattleCard.MoonDisasterGA, new List<BattleCard> { BattleCard.GizaWheel1, BattleCard.GizaWheel2, BattleCard.GizaWheel3 } },
            { BattleCard.LibraBalanceGA, new List<BattleCard> { BattleCard.AngerFire2, BattleCard.WideWave2, BattleCard.HeavyDoon2 } },
            { BattleCard.OphiuchusQueenGA, new List<BattleCard> { BattleCard.HexNet3, BattleCard.HexNet3, BattleCard.HexNet3 } },
            { BattleCard.GeminiSparkGA, new List<BattleCard> { BattleCard.DoubleStone, BattleCard.IllegalStanKnuckle, BattleCard.IllegalElecSlash } },
            { BattleCard.CancerBubbleGA, new List<BattleCard> { BattleCard.WideWave1, BattleCard.WideWave1, BattleCard.ChainBubble1 } },
            { BattleCard.CrownThunderGA, new List<BattleCard> { BattleCard.PlasmaGun, BattleCard.PlasmaGun, BattleCard.DrillArm1 } },
            { BattleCard.YetiBlizzardGA, new List<BattleCard> { BattleCard.IllegalSnowBall3, BattleCard.IllegalSnowBall3, BattleCard.DoubleStone } },
            { BattleCard.BrachioWaveGA, new List<BattleCard> { BattleCard.SharkCutter1, BattleCard.SharkCutter1, BattleCard.SharkCutter1 } },
            { BattleCard.CondorGeographGA, new List<BattleCard> { BattleCard.PowderShoot2, BattleCard.PowderShoot2, BattleCard.TyphoonDance } },
            { BattleCard.OlgaGeneralGA, new List<BattleCard> { BattleCard.HeavyDoon3, BattleCard.HeavyDoon3, BattleCard.HeavyDoon3 } },
            { BattleCard.ApollonFrameGA, new List<BattleCard> { BattleCard.MachineFlame1, BattleCard.MachineFlame2, BattleCard.MachineFlame3 } },
            { BattleCard.SiriusGA, new List<BattleCard> { BattleCard.WhiteMeteor, BattleCard.SilverMeteor, BattleCard.IllegalBreakSabre } },
            { BattleCard.AxisJetGA, new List<BattleCard> { BattleCard.DrillArm1, BattleCard.DrillArm1, BattleCard.DrillArm1 } },
            { BattleCard.BIcehammerGA, new List<BattleCard> { BattleCard.IllegalSnowStorm, BattleCard.IllegalSnowStorm, BattleCard.HammerWeapon3 } },
            { BattleCard.StrongSwingGA, new List<BattleCard> { BattleCard.RollingNuts2, BattleCard.RollingNuts2, BattleCard.IllegalWoodSlash } },
            // Giga
            { BattleCard.DarknessHoleGA, new List<BattleCard> { BattleCard.BlackHole1, BattleCard.BlackHole1, BattleCard.BlackInk } },
            { BattleCard.DestroyMissileGA, new List<BattleCard> { BattleCard.DashAttack2, BattleCard.DashAttack2, BattleCard.BombRizer } },
            { BattleCard.BreakCountBombGA, new List<BattleCard> { BattleCard.CountBomb3, BattleCard.CountBomb3, BattleCard.CountBomb3 } },
            { BattleCard.OxTackleGA, new List<BattleCard> { BattleCard.HeatUpper1, BattleCard.HeatUpper1, BattleCard.DashAttack1 } },
            { BattleCard.GorgonEyeGA, new List<BattleCard> { BattleCard.KusamuraStage, BattleCard.MuTechnology3, BattleCard.MuTechnology3 } },
            { BattleCard.GeminiThunderGA, new List<BattleCard> { BattleCard.MummyHand3, BattleCard.MummyHand3, BattleCard.MummyHand3 } },
            { BattleCard.NadareDaikoGA, new List<BattleCard> { BattleCard.IllegalSnowBall3, BattleCard.IllegalSnowBall3, BattleCard.WideWave3 } },
            { BattleCard.GekiryuWaveGA, new List<BattleCard> { BattleCard.IllegalPiranhaKiss2, BattleCard.IllegalPiranhaKiss2, BattleCard.IllegalPiranhaKiss2 } },
            { BattleCard.FlyingImpactGA, new List<BattleCard> { BattleCard.DashAttack3, BattleCard.DashAttack3, BattleCard.IllegalJungleStorm } },
            { BattleCard.BuraiBreakGA, new List<BattleCard> { BattleCard.IllegalDeathScythe2, BattleCard.IllegalDeathScythe2, BattleCard.IllegalDeathScythe2 } },
            { BattleCard.LightOfSaintGA, new List<BattleCard> { BattleCard.IllegalKesalanPatharan3, BattleCard.IllegalKesalanPatharan3, BattleCard.TornadoDance } },
            { BattleCard.PainHellFlameGA, new List<BattleCard> { BattleCard.IllegalDabaFlame3, BattleCard.IllegalDabaFlame3, BattleCard.IllegalDabaFlame3 } },
        };

        public static Dictionary<BattleCard, int> GetPossibleCombos(Dictionary<BattleCard, int> inventory)
        {
            var result = new Dictionary<BattleCard, int>();

            foreach (var kvp in gaCombos)
            {
                BattleCard gaCard = kvp.Key;
                List<BattleCard> required = kvp.Value;

                // Count how many of each required card is needed
                var requiredCounts = required
                    .GroupBy(c => c)
                    .ToDictionary(g => g.Key, g => g.Count());

                // For each required card, compute how many sets we can make
                int maxSets = int.MaxValue;

                foreach (var req in requiredCounts)
                {
                    BattleCard card = req.Key;
                    int needed = req.Value;

                    // If inventory doesn't contain the card → cannot form combo
                    if (!inventory.TryGetValue(card, out int have))
                    {
                        maxSets = 0;
                        break;
                    }

                    // How many sets can be made from this card
                    int setsFromThisCard = have / needed;

                    if (setsFromThisCard < maxSets)
                        maxSets = setsFromThisCard;
                }

                if (maxSets > 0)
                    result[gaCard] = maxSets;
            }

            return result;
        }

    }
}
