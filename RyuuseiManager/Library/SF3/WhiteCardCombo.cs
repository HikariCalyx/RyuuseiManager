namespace RyuuseiManager.Library.SF3
{
    public static class WhiteCardCombo
    {
        private static readonly Dictionary<int, WhiteCard> whiteCards = new()
        {
            { 0x00, new()
                {
                    Card1 = BattleCard.Cannon,
                    Card2 = BattleCard.AirSpread1,
                    Card3 = BattleCard.Sword,
                    Card4 = BattleCard.Recovery30,
                }
            },
            { 0x01, new()
                {
                    Card1 = BattleCard.PlasmaGun,
                    Card2 = BattleCard.PlasmaGun,
                    Card3 = BattleCard.AirSpread1,
                    Card4 = BattleCard.BeastSwing1,
                }
            },
            { 0x02, new()
                {
                    Card1 = BattleCard.WideSword,
                    Card2 = BattleCard.Recovery30,
                    Card3 = BattleCard.GroundWave1,
                    Card4 = BattleCard.Barrier,
                }
            },
            { 0x03, new()
                {
                    Card1 = BattleCard.MadBalkan1,
                    Card2 = BattleCard.MiniGrenade,
                    Card3 = BattleCard.LongSword,
                    Card4 = BattleCard.Cannon,
                }
            },
            { 0x04, new()
                {
                    Card1 = BattleCard.PlasmaGun,
                    Card2 = BattleCard.Invisible,
                    Card3 = BattleCard.GroundWave1,
                    Card4 = BattleCard.BeastSwing1,
                }
            },
            { 0x05, new()
                {
                    Card1 = BattleCard.DivideLine,
                    Card2 = BattleCard.LongSword,
                    Card3 = BattleCard.MadBalkan1,
                    Card4 = BattleCard.Recovery30,
                }
            },
            { 0x06, new()
                {
                    Card1 = BattleCard.Invisible,
                    Card2 = BattleCard.GroundWave1,
                    Card3 = BattleCard.Sword,
                    Card4 = BattleCard.MadBalkan1,
                }
            },
            { 0x07, new()
                {
                    Card1 = BattleCard.BeastSwing1,
                    Card2 = BattleCard.Cannon,
                    Card3 = BattleCard.PlasmaGun,
                    Card4 = BattleCard.GravityPlus,
                }
            },
            { 0x08, new()
                {
                    Card1 = BattleCard.MiniGrenade,
                    Card2 = BattleCard.LongSword,
                    Card3 = BattleCard.LongSword,
                    Card4 = BattleCard.Recovery30,
                }
            },
            { 0x09, new()
                {
                    Card1 = BattleCard.PlasmaGun,
                    Card2 = BattleCard.WideSword,
                    Card3 = BattleCard.GroundWave1,
                    Card4 = BattleCard.GroundWave1,
                }
            },
            { 0x0A, new()
                {
                    Card1 = BattleCard.AirSpread1,
                    Card2 = BattleCard.MadBalkan1,
                    Card3 = BattleCard.BeastSwing1,
                    Card4 = BattleCard.AirSpread1,
                }
            },
            { 0x0B, new()
                {
                    Card1 = BattleCard.FlashSpire1,
                    Card2 = BattleCard.FlashSpire1,
                    Card3 = BattleCard.MadBalkan1,
                    Card4 = BattleCard.GravityPlus,
                }
            },
            { 0x0C, new()
                {
                    Card1 = BattleCard.BeastSwing1,
                    Card2 = BattleCard.BeastSwing1,
                    Card3 = BattleCard.AirSpread1,
                    Card4 = BattleCard.AttackPlus10,
                }
            },
            { 0x0D, new()
                {
                    Card1 = BattleCard.GroundWave1,
                    Card2 = BattleCard.MadBalkan1,
                    Card3 = BattleCard.AttackPlus10,
                    Card4 = BattleCard.BeastSwing1,
                }
            },
            { 0x0E, new()
                {
                    Card1 = BattleCard.AirSpread2,
                    Card2 = BattleCard.WideWave1,
                    Card3 = BattleCard.GravityPlus,
                    Card4 = BattleCard.DivideLine,
                }
            },
            { 0x0F, new()
                {
                    Card1 = BattleCard.DashAttack1,
                    Card2 = BattleCard.DashAttack1,
                    Card3 = BattleCard.GravityPlus,
                    Card4 = BattleCard.Recovery50,
                }
            },
            { 0x10, new()
                {
                    Card1 = BattleCard.DrillArm1,
                    Card2 = BattleCard.DrillArm1,
                    Card3 = BattleCard.AttackPlus10,
                    Card4 = BattleCard.DashAttack1,
                }
            },
            { 0x11, new()
                {
                    Card1 = BattleCard.WideWave1,
                    Card2 = BattleCard.SharkCutter1,
                    Card3 = BattleCard.SharkCutter1,
                    Card4 = BattleCard.WideWave1,
                }
            },
            { 0x12, new()
                {
                    Card1 = BattleCard.GroundWave2,
                    Card2 = BattleCard.GizaWheel1,
                    Card3 = BattleCard.GizaWheel1,
                    Card4 = BattleCard.PalsyPlus,
                }
            },
            { 0x13, new()
                {
                    Card1 = BattleCard.IceSpinning1,
                    Card2 = BattleCard.AttackPlus10,
                    Card3 = BattleCard.WideWave1,
                    Card4 = BattleCard.WideWave1,
                }
            },
            { 0x14, new()
                {
                    Card1 = BattleCard.SharkCutter1,
                    Card2 = BattleCard.PalsyPlus,
                    Card3 = BattleCard.GravityPlus,
                    Card4 = BattleCard.SharkCutter1,
                }
            },
            { 0x15, new()
                {
                    Card1 = BattleCard.AirSpread3,
                    Card2 = BattleCard.PlusCannon,
                    Card3 = BattleCard.GizaWheel2,
                    Card4 = BattleCard.GizaWheel2,
                }
            },
            { 0x16, new()
                {
                    Card1 = BattleCard.EdoBlade1,
                    Card2 = BattleCard.EdoBlade1,
                    Card3 = BattleCard.GizaWheel2,
                    Card4 = BattleCard.PalsyPlus,
                }
            },
            { 0x17, new()
                {
                    Card1 = BattleCard.ChainBubble1,
                    Card2 = BattleCard.WideWave2,
                    Card3 = BattleCard.PalsyPlus,
                    Card4 = BattleCard.ChainBubble1,
                }
            },
            { 0x18, new()
                {
                    Card1 = BattleCard.BeastSwing2,
                    Card2 = BattleCard.BeastSwing2,
                    Card3 = BattleCard.DrillArm1,
                    Card4 = BattleCard.PlusCannon,
                }
            },
            { 0x19, new()
                {
                    Card1 = BattleCard.AngerFire1,
                    Card2 = BattleCard.AngerFire1,
                    Card3 = BattleCard.HeatUpper2,
                    Card4 = BattleCard.HeatUpper2,
                }
            },
            { 0x1A, new()
                {
                    Card1 = BattleCard.CountBomb1,
                    Card2 = BattleCard.HeatUpper2,
                    Card3 = BattleCard.AngerFire1,
                    Card4 = BattleCard.AttackPlus10,
                }
            },
            { 0x1B, new()
                {
                    Card1 = BattleCard.PowderShoot1,
                    Card2 = BattleCard.Shurishuriken2,
                    Card3 = BattleCard.RollingNuts1,
                    Card4 = BattleCard.HexNet1,
                }
            },
            { 0x1C, new()
                {
                    Card1 = BattleCard.RollingNuts1,
                    Card2 = BattleCard.RollingNuts1,
                    Card3 = BattleCard.PowderShoot1,
                    Card4 = BattleCard.PowderShoot1,
                }
            },
            { 0x1D, new()
                {
                    Card1 = BattleCard.FlashSpire3,
                    Card2 = BattleCard.FlashSpire3,
                    Card3 = BattleCard.StealthLaser2,
                    Card4 = BattleCard.ThunderHead1,
                }
            },
            { 0x1E, new()
                {
                    Card1 = BattleCard.StealthLaser2,
                    Card2 = BattleCard.PalsyPlus,
                    Card3 = BattleCard.ThunderHead1,
                    Card4 = BattleCard.MummyHand1,
                }
            },
            { 0x1F, new()
                {
                    Card1 = BattleCard.DrillArm3,
                    Card2 = BattleCard.HammerWeapon1,
                    Card3 = BattleCard.HeavyCannon,
                    Card4 = BattleCard.TyphoonDance,
                }
            },
            { 0x20, new()
                {
                    Card1 = BattleCard.CountBomb2,
                    Card2 = BattleCard.MachineFlame1,
                    Card3 = BattleCard.HeatUpper3,
                    Card4 = BattleCard.AngerFire2,
                }
            },
            { 0x21, new() // NoiseWaveDispatchLog1
                {
                    Card1 = BattleCard.BeastSwing3,
                    Card2 = BattleCard.BeastSwing2,
                    Card3 = BattleCard.DoubleEater,
                    Card4 = BattleCard.GreatAxe,
                }
            },
            { 0x22, new() // NoiseWaveDispatchLog2
                {
                    Card1 = BattleCard.WideWave3,
                    Card2 = BattleCard.SharkCutter3,
                    Card3 = BattleCard.WideWave2,
                    Card4 = BattleCard.SharkCutter3,
                }
            },
            { 0x23, new() // NoiseWaveDispatchLog3
                {
                    Card1 = BattleCard.CountBomb2,
                    Card2 = BattleCard.HeatUpper3,
                    Card3 = BattleCard.AngerFire3,
                    Card4 = BattleCard.AttackPanel,
                }
            },
            { 0x24, new() // NoiseWaveDispatchLog4
                {
                    Card1 = BattleCard.PowderShoot3,
                    Card2 = BattleCard.Shurishuriken2,
                    Card3 = BattleCard.RollingNuts3,
                    Card4 = BattleCard.HexNet3,
                }
            },
            { 0x25, new() // OuterAstroDispatchLog1
                {
                    Card1 = BattleCard.FlashSpire3,
                    Card2 = BattleCard.ThunderHead2,
                    Card3 = BattleCard.FlashSpire3,
                    Card4 = BattleCard.StealthLaser2,
                }
            },
            { 0x26, new() // OuterAstroDispatchLog2
                {
                    Card1 = BattleCard.DrillArm3,
                    Card2 = BattleCard.DrillArm2,
                    Card3 = BattleCard.PalsyPlus,
                    Card4 = BattleCard.HammerWeapon3,
                }
            },
            { 0x27, new() // OuterAstroDispatchLog3
                {
                    Card1 = BattleCard.StealthLaser3,
                    Card2 = BattleCard.StealthLaser3,
                    Card3 = BattleCard.ThunderHead3,
                    Card4 = BattleCard.IllegalElecSlash,
                }
            },
            { 0x28, new() // OuterAstroDispatchLog4
                {
                    Card1 = BattleCard.MachineFlame2,
                    Card2 = BattleCard.HeatUpper3,
                    Card3 = BattleCard.CountBomb3,
                    Card4 = BattleCard.AngerFire3,
                }
            },
            { 0x29, new() // PlanetFMDispatchLog1
                {
                    Card1 = BattleCard.ChainBubble3,
                    Card2 = BattleCard.ChainBubble3,
                    Card3 = BattleCard.PalsyPlus,
                    Card4 = BattleCard.WideWave3,
                }
            },
            { 0x2A, new() // PlanetFMDispatchLog2
                {
                    Card1 = BattleCard.Shurishuriken3,
                    Card2 = BattleCard.StealthLaser3,
                    Card3 = BattleCard.ThunderHead3,
                    Card4 = BattleCard.IllegalWoodSlash,
                }
            },
            { 0x2B, new() // PlanetFMDispatchLog3
                {
                    Card1 = BattleCard.FlashSpire3,
                    Card2 = BattleCard.StealthLaser3,
                    Card3 = BattleCard.ThunderHead3,
                    Card4 = BattleCard.IllegalElecSlash,
                }
            },
            { 0x2C, new() // PlanetFMDispatchLog4
                {
                    Card1 = BattleCard.TornadoDance,
                    Card2 = BattleCard.WindyAttack3,
                    Card3 = BattleCard.DrillArm3,
                    Card4 = BattleCard.HammerWeapon3,
                }
            },
            { 0x2D, new() // White Card 1
                {
                    Card1 = BattleCard.PlasmaGun,
                    Card2 = BattleCard.StealthLaser2,
                    Card3 = BattleCard.MummyHand1,
                    Card4 = BattleCard.ThunderHead1,
                }
            },
            { 0x2E, new() // White Card 2
                {
                    Card1 = BattleCard.IllegalGreenInk,
                    Card2 = BattleCard.HexNet1,
                    Card3 = BattleCard.Shurishuriken1,
                    Card4 = BattleCard.PowderShoot2,
                }
            },
            { 0x2F, new() // White Card 3
                {
                    Card1 = BattleCard.Whistle,
                    Card2 = BattleCard.DrillArm1,
                    Card3 = BattleCard.DrillArm2,
                    Card4 = BattleCard.WindyAttack1,
                }
            },
            { 0x30, new() // White Card 4
                {
                    Card1 = BattleCard.ChainBubble1,
                    Card2 = BattleCard.ChainBubble1,
                    Card3 = BattleCard.FlashSpire2,
                    Card4 = BattleCard.ThunderHead2,
                }
            },
            { 0x31, new() // White Card 5
                {
                    Card1 = BattleCard.BlackInk,
                    Card2 = BattleCard.FlashSpire2,
                    Card3 = BattleCard.ChainBubble1,
                    Card4 = BattleCard.MummyHand2,
                }
            },
            { 0x32, new() // White Card 6
                {
                    Card1 = BattleCard.WindyAttack1,
                    Card2 = BattleCard.WindyAttack2,
                    Card3 = BattleCard.NoisedWizard1,
                    Card4 = BattleCard.NoisedWizard2,
                }
            },
            { 0x33, new() // White Card 7
                {
                    Card1 = BattleCard.GizaWheel2,
                    Card2 = BattleCard.GroundWave2,
                    Card3 = BattleCard.Shurishuriken1,
                    Card4 = BattleCard.IceSpinning1,
                }
            },
            { 0x34, new() // White Card 8
                {
                    Card1 = BattleCard.HeavyCannon,
                    Card2 = BattleCard.DrillArm2,
                    Card3 = BattleCard.HeatUpper2,
                    Card4 = BattleCard.HeavyDoon1,
                }
            },
            { 0x35, new() // White Card 9
                {
                    Card1 = BattleCard.HeelWizard1,
                    Card2 = BattleCard.ChainBubble1,
                    Card3 = BattleCard.IllegalElecSlash,
                    Card4 = BattleCard.HexNet1,
                }
            },
            { 0x36, new() // White Card 10
                {
                    Card1 = BattleCard.Barrier,
                    Card2 = BattleCard.Cannon,
                    Card3 = BattleCard.Cannon,
                    Card4 = BattleCard.Cannon,
                }
            },
            { 0x37, new() // White Card 11
                {
                    Card1 = BattleCard.DoubleEater,
                    Card2 = BattleCard.Whistle,
                    Card3 = BattleCard.BeastSwing3,
                    Card4 = BattleCard.GreatAxe,
                }
            },
            { 0x38, new() // White Card 12
                {
                    Card1 = BattleCard.RollingNuts2,
                    Card2 = BattleCard.RollingNuts1,
                    Card3 = BattleCard.MachineFlame1,
                    Card4 = BattleCard.CountBomb3,
                }
            },
            { 0x39, new() // White Card 13
                {
                    Card1 = BattleCard.IllegalIceGrenade,
                    Card2 = BattleCard.IllegalIceGrenade,
                    Card3 = BattleCard.SharkCutter2,
                    Card4 = BattleCard.WideWave2,
                }
            },
            { 0x3A, new() // White Card 14
                {
                    Card1 = BattleCard.EdoBlade3,
                    Card2 = BattleCard.EdoBlade3,
                    Card3 = BattleCard.EdoBlade2,
                    Card4 = BattleCard.EdoBlade1,
                }
            },
            { 0x3B, new() // White Card 15
                {
                    Card1 = BattleCard.SynchroHook1,
                    Card2 = BattleCard.IllegalFreezeKnuckle,
                    Card3 = BattleCard.HeatUpper3,
                    Card4 = BattleCard.IllegalStanKnuckle,
                }
            },
            { 0x3C, new() // White Card 16
                {
                    Card1 = BattleCard.IllegalFirePlus30,
                    Card2 = BattleCard.MachineFlame3,
                    Card3 = BattleCard.AngerFire3,
                    Card4 = BattleCard.OxFireEX,
                }
            },
            { 0x3D, new() // White Card 17
                {
                    Card1 = BattleCard.IllegalAquaPlus30,
                    Card2 = BattleCard.SharkCutter3,
                    Card3 = BattleCard.ChainBubble3,
                    Card4 = BattleCard.DiaIcebahnEX,
                }
            },
            { 0x3E, new() // White Card 18
                {
                    Card1 = BattleCard.Shurishuriken3,
                    Card2 = BattleCard.WideWave3,
                    Card3 = BattleCard.StealthLaser3,
                    Card4 = BattleCard.SmileCoin3,
                }
            },
            { 0x3F, new() // White Card 19
                {
                    Card1 = BattleCard.MachineFlame3,
                    Card2 = BattleCard.IllegalDabaFlame3,
                    Card3 = BattleCard.AngerFire3,
                    Card4 = BattleCard.HeatUpper3,
                }
            },
            { 0x40, new() // White Card 20
                {
                    Card1 = BattleCard.IceSpinning3,
                    Card2 = BattleCard.WideWave3,
                    Card3 = BattleCard.SharkCutter3,
                    Card4 = BattleCard.ChainBubble3,
                }
            },
            { 0x41, new() // White Card 21
                {
                    Card1 = BattleCard.SkullArrow3,
                    Card2 = BattleCard.SkullArrow3,
                    Card3 = BattleCard.DashAttack3,
                    Card4 = BattleCard.HammerWeapon3,
                }
            },
            { 0x42, new() // White Card 22
                {
                    Card1 = BattleCard.BeastSwing3,
                    Card2 = BattleCard.BeastSwing3,
                    Card3 = BattleCard.HeelWizard3,
                    Card4 = BattleCard.IllegalDeathScythe3,
                }
            },
            { 0x43, new() // White Card 23
                {
                    Card1 = BattleCard.TornadoDance,
                    Card2 = BattleCard.AirSpread3,
                    Card3 = BattleCard.IllegalVulcanSeed3,
                    Card4 = BattleCard.MadBalkan3,
                }
            },
            { 0x44, new() // White Card 24
                {
                    Card1 = BattleCard.PanicCloud,
                    Card2 = BattleCard.PanicCloud,
                    Card3 = BattleCard.FlashSpire3,
                    Card4 = BattleCard.PowderShoot3,
                }
            },
            { 0x45, new() // White Card 25
                {
                    Card1 = BattleCard.IllegalElecSlash,
                    Card2 = BattleCard.MummyHand3,
                    Card3 = BattleCard.HexNet3,
                    Card4 = BattleCard.ThunderOfEarth3,
                }
            },
            { 0x46, new() // White Card 26
                {
                    Card1 = BattleCard.MachineFlame3,
                    Card2 = BattleCard.WhiteMeteor,
                    Card3 = BattleCard.HeavyDoon3,
                    Card4 = BattleCard.CountBomb3,
                }
            },
            { 0x47, new() // White Card 27
                {
                    Card1 = BattleCard.MiniGrenade,
                    Card2 = BattleCard.Sword,
                    Card3 = BattleCard.LongSword,
                    Card4 = BattleCard.WideSword,
                }
            },
            { 0x48, new() // White Card 28
                {
                    Card1 = BattleCard.WindyAttack3,
                    Card2 = BattleCard.TornadoDance,
                    Card3 = BattleCard.IllegalKogarashi3,
                    Card4 = BattleCard.IllegalDeathScythe3,
                }
            },
            { 0x49, new() // White Card 29
                {
                    Card1 = BattleCard.SwordFighter3,
                    Card2 = BattleCard.IllegalElecSlash,
                    Card3 = BattleCard.IllegalWoodSlash,
                    Card4 = BattleCard.DancingBlade3,
                }
            },
            { 0x4A, new() // White Card 30
                {
                    Card1 = BattleCard.IllegalIceGrenade,
                    Card2 = BattleCard.WideWave3,
                    Card3 = BattleCard.SharkCutter3,
                    Card4 = BattleCard.FlameAxe,
                }
            },
            { 0x4B, new() // White Card 31
                {
                    Card1 = BattleCard.GreatAxe,
                    Card2 = BattleCard.HammerWeapon3,
                    Card3 = BattleCard.DrillArm3,
                    Card4 = BattleCard.IllegalBreakSabre,
                }
            },
            { 0x4C, new() // White Card 32
                {
                    Card1 = BattleCard.AngerFire3,
                    Card2 = BattleCard.CountBomb3,
                    Card3 = BattleCard.IllegalBobonBomb3,
                    Card4 = BattleCard.IllegalBobonBomb3,
                }
            },
            { 0x4D, new() // White Card 33
                {
                    Card1 = BattleCard.IllegalElecPlus30,
                    Card2 = BattleCard.FlashSpire3,
                    Card3 = BattleCard.ThunderHead3,
                    Card4 = BattleCard.SpadeMagnetsSP,
                }
            },
            { 0x4E, new() // White Card 34
                {
                    Card1 = BattleCard.IllegalWoodPlus30,
                    Card2 = BattleCard.IllegalKogarashi3,
                    Card3 = BattleCard.IllegalVulcanSeed3,
                    Card4 = BattleCard.ClubStrongSP,
                }
            },
            { 0x4F, new() // White Card - Acid Striker
                {
                    Card1 = BattleCard.IllegalBreakSabre,
                    Card2 = BattleCard.DrillArm3,
                    Card3 = BattleCard.SwordFighter3,
                    Card4 = BattleCard.AcidAceSP,
                }
            },
            { 0x50, new() // White Card - Fighter's Doctrine
                {
                    Card1 = BattleCard.IllegalStanKnuckle,
                    Card2 = BattleCard.IllegalFreezeKnuckle,
                    Card3 = BattleCard.IllegalPoisonKnuckle,
                    Card4 = BattleCard.IllegalDestroyUpper,
                }
            },
            { 0x51, new() // White Card - Force Element
                {
                    Card1 = BattleCard.FlashSpire2,
                    Card2 = BattleCard.Shurishuriken2,
                    Card3 = BattleCard.HeatUpper2,
                    Card4 = BattleCard.SharkCutter2,
                }
            },
            { 0x52, new() // White Card - Combo Master
                {
                    Card1 = BattleCard.IllegalWoodSlash,
                    Card2 = BattleCard.HexNet2,
                    Card3 = BattleCard.StealthLaser2,
                    Card4 = BattleCard.Shurishuriken2,
                }
            },
            { 0x53, new() // White Card - Nimble Black Ace
                {
                    Card1 = BattleCard.Meteoritebarrage,
                    Card2 = BattleCard.GravityPlus,
                    Card3 = BattleCard.WideWave1,
                    Card4 = BattleCard.BeastSwing1,
                }
            },
            { 0x54, new() // White Card - Mighty Red Joker
                {
                    Card1 = BattleCard.Atomicblazer,
                    Card2 = BattleCard.AttackPlus10,
                    Card3 = BattleCard.HeatUpper1,
                    Card4 = BattleCard.BeastSwing1,
                }
            },
            { 0x55, new() // White Card W1
                {
                    Card1 = BattleCard.BeastSwing1,
                    Card2 = BattleCard.GravityPlus,
                    Card3 = BattleCard.IceSpinning1,
                    Card4 = BattleCard.GizaWheel1,
                }
            },
            { 0x56, new() // White Card W2
                {
                    Card1 = BattleCard.HeatUpper2,
                    Card2 = BattleCard.AngerFire1,
                    Card3 = BattleCard.CountBomb2,
                    Card4 = BattleCard.IllegalDabaFlame1,
                }
            },
            { 0x57, new() // White Card W3
                {
                    Card1 = BattleCard.WideWave3,
                    Card2 = BattleCard.SharkCutter2,
                    Card3 = BattleCard.IllegalBlueInk,
                    Card4 = BattleCard.IllegalIceGrenade,
                }
            },
            { 0x58, new() // White Card W4
                {
                    Card1 = BattleCard.FlashSpire3,
                    Card2 = BattleCard.IllegalElecSlash,
                    Card3 = BattleCard.IllegalElecSlash,
                    Card4 = BattleCard.ThunderHead2,
                }
            },
            { 0x59, new() // White Card W5
                {
                    Card1 = BattleCard.PowderShoot3,
                    Card2 = BattleCard.IllegalVulcanSeed1,
                    Card3 = BattleCard.Shurishuriken3,
                    Card4 = BattleCard.IllegalVulcanSeed2,
                }
            },
            { 0x5A, new() // White Card W6
                {
                    Card1 = BattleCard.TyphoonDance,
                    Card2 = BattleCard.TornadoDance,
                    Card3 = BattleCard.WindyAttack3,
                    Card4 = BattleCard.IllegalDeathScythe2,
                }
            },
            { 0x5B, new() // White Card W7
                {
                    Card1 = BattleCard.HeatUpper3,
                    Card2 = BattleCard.HammerWeapon3,
                    Card3 = BattleCard.IllegalBreakSabre,
                    Card4 = BattleCard.HeavyDoon3,
                }
            },
            { 0x5C, new() // White Card W8
                {
                    Card1 = BattleCard.GroundWave3,
                    Card2 = BattleCard.SwordFighter1,
                    Card3 = BattleCard.NoisedWizard2,
                    Card4 = BattleCard.DrillArm3,
                }
            },
            { 0x5D, new() // White Card W9
                {
                    Card1 = BattleCard.DoubleEater,
                    Card2 = BattleCard.HeelWizard2,
                    Card3 = BattleCard.BeastSwing3,
                    Card4 = BattleCard.JackCorvus,
                }
            },
            { 0x5E, new() // White Card W10
                {
                    Card1 = BattleCard.DoubleEater,
                    Card2 = BattleCard.ChainBubble3,
                    Card3 = BattleCard.WideWave3,
                    Card4 = BattleCard.QueenVirgo,
                }
            },
            { 0x5F, new() // White Card W11
                {
                    Card1 = BattleCard.HammerWeapon3,
                    Card2 = BattleCard.HeatUpper3,
                    Card3 = BattleCard.GreatAxe,
                    Card4 = BattleCard.GraveJoker,
                }
            },
            { 0x60, new() // White Card W12
                {
                    Card1 = BattleCard.IllegalBreakSabre,
                    Card2 = BattleCard.StealthLaser3,
                    Card3 = BattleCard.GizaWheel3,
                    Card4 = BattleCard.AcidAce,
                }
            },
        };

        public static WhiteCard GetWhiteCard(int id)
        {
            if (whiteCards.ContainsKey(id))
            {
                return whiteCards[id];
            }
            else
            {
                return whiteCards[0x3A];
            }
        }
    }
}
