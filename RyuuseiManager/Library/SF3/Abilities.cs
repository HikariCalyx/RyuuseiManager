using System;
using System.Collections.Generic;
using System.Text;

namespace RyuuseiManager.Library.SF3
{
    public static class Abilities
    {
        private static Dictionary<int, Ability> abilities = new()
        {
            { 0x3867, new()
                {
                    Type = AbilityType.HPPlus50,
                    Cost = 120,
                }
            },
            { 0x38A8, new()
                {
                    Type = AbilityType.HPPlus50,
                    Cost = 110,
                }
            },
            { 0x38BE, new()
                {
                    Type = AbilityType.HPPlus50,
                    Cost = 100,
                }
            },
            { 0x38D4, new()
                {
                    Type = AbilityType.HPPlus50,
                    Cost = 90,
                }
            },
            { 0x3868, new()
                {
                    Type = AbilityType.HPPlus100,
                    Cost = 220,
                }
            },
            { 0x38A9, new()
                {
                    Type = AbilityType.HPPlus100,
                    Cost = 205,
                }
            },
            { 0x38BF, new()
                {
                    Type = AbilityType.HPPlus100,
                    Cost = 190,
                }
            },
            { 0x38D5, new()
                {
                    Type = AbilityType.HPPlus100,
                    Cost = 170,
                }
            },
            { 0x386A, new()
                {
                    Type = AbilityType.HPPlus200,
                    Cost = 310,
                }
            },
            { 0x38AA, new()
                {
                    Type = AbilityType.HPPlus200,
                    Cost = 290,
                }
            },
            { 0x38C0, new()
                {
                    Type = AbilityType.HPPlus200,
                    Cost = 270,
                }
            },
            { 0x38D6, new()
                {
                    Type = AbilityType.HPPlus200,
                    Cost = 250,
                }
            },
            { 0x386C, new()
                {
                    Type = AbilityType.HPPlus300,
                    Cost = 420,
                }
            },
            { 0x38AB, new()
                {
                    Type = AbilityType.HPPlus300,
                    Cost = 390,
                }
            },
            { 0x38C1, new()
                {
                    Type = AbilityType.HPPlus300,
                    Cost = 370,
                }
            },
            { 0x38D7, new()
                {
                    Type = AbilityType.HPPlus300,
                    Cost = 340,
                }
            },
            { 0x386D, new()
                {
                    Type = AbilityType.HPPlus400,
                    Cost = 490,
                }
            },
            { 0x38AC, new()
                {
                    Type = AbilityType.HPPlus400,
                    Cost = 470,
                }
            },
            { 0x38C2, new()
                {
                    Type = AbilityType.HPPlus400,
                    Cost = 450,
                }
            },
            { 0x38D8, new()
                {
                    Type = AbilityType.HPPlus400,
                    Cost = 430,
                }
            },
            { 0x386E, new()
                {
                    Type = AbilityType.HPPlus500,
                    Cost = 610,
                }
            },
            { 0x38AD, new()
                {
                    Type = AbilityType.HPPlus500,
                    Cost = 570,
                }
            },
            { 0x38C3, new()
                {
                    Type = AbilityType.HPPlus500,
                    Cost = 540,
                }
            },
            { 0x38D9, new()
                {
                    Type = AbilityType.HPPlus500,
                    Cost = 450,
                }
            },
            { 0x38E5, new()
                {
                    Type = AbilityType.HPPlus500,
                    Cost = 350,
                }
            },
            { 0x3800, new()
                {
                    Type = AbilityType.Undershirt,
                    Cost = 60,
                }
            },
            { 0x3892, new()
                {
                    Type = AbilityType.Undershirt,
                    Cost = 50,
                }
            },
            { 0x38B0, new()
                {
                    Type = AbilityType.Undershirt,
                    Cost = 30,
                }
            },
            { 0x38C6, new()
                {
                    Type = AbilityType.Undershirt,
                    Cost = 5,
                }
            },
            { 0x3801, new()
                {
                    Type = AbilityType.AirShoes,
                    Cost = 450,
                }
            },
            { 0x3893, new()
                {
                    Type = AbilityType.AirShoes,
                    Cost = 400,
                }
            },
            { 0x38B1, new()
                {
                    Type = AbilityType.AirShoes,
                    Cost = 350,
                }
            },
            { 0x38C7, new()
                {
                    Type = AbilityType.AirShoes,
                    Cost = 260,
                }
            },
            { 0x38DC, new()
                {
                    Type = AbilityType.AirShoes,
                    Cost = 200,
                }
            },
            { 0x3802, new()
                {
                    Type = AbilityType.FloatShoes,
                    Cost = 300,
                }
            },
            { 0x3894, new()
                {
                    Type = AbilityType.FloatShoes,
                    Cost = 270,
                }
            },
            { 0x38B2, new()
                {
                    Type = AbilityType.FloatShoes,
                    Cost = 240,
                }
            },
            { 0x38C8, new()
                {
                    Type = AbilityType.FloatShoes,
                    Cost = 210,
                }
            },
            { 0x38DD, new()
                {
                    Type = AbilityType.FloatShoes,
                    Cost = 150,
                }
            },
            { 0x3803, new()
                {
                    Type = AbilityType.Reflect,
                    Cost = 260,
                }
            },
            { 0x3895, new()
                {
                    Type = AbilityType.Reflect,
                    Cost = 230,
                }
            },
            { 0x38B3, new()
                {
                    Type = AbilityType.Reflect,
                    Cost = 200,
                }
            },
            { 0x38C9, new()
                {
                    Type = AbilityType.Reflect,
                    Cost = 150,
                }
            },
            { 0x38DE, new()
                {
                    Type = AbilityType.Reflect,
                    Cost = 50,
                }
            },
            { 0x3804, new()
                {
                    Type = AbilityType.SuperArmor,
                    Cost = 900,
                }
            },
            { 0x3896, new()
                {
                    Type = AbilityType.SuperArmor,
                    Cost = 600,
                }
            },
            { 0x3805, new()
                {
                    Type = AbilityType.NoFlash,
                    Cost = 300,
                }
            },
            { 0x3897, new()
                {
                    Type = AbilityType.NoFlash,
                    Cost = 200,
                }
            },
            { 0x3806, new()
                {
                    Type = AbilityType.NoGravity,
                    Cost = 500,
                }
            },
            { 0x3898, new()
                {
                    Type = AbilityType.NoGravity,
                    Cost = 300,
                }
            },
            { 0x3807, new()
                {
                    Type = AbilityType.NoParalyze,
                    Cost = 800,
                }
            },
            { 0x3899, new()
                {
                    Type = AbilityType.NoParalyze,
                    Cost = 600,
                }
            },
            { 0x3808, new()
                {
                    Type = AbilityType.NoFreeze,
                    Cost = 800,
                }
            },
            { 0x389A, new()
                {
                    Type = AbilityType.NoFreeze,
                    Cost = 600,
                }
            },
            { 0x3809, new()
                {
                    Type = AbilityType.NoBubble,
                    Cost = 800,
                }
            },
            { 0x389B, new()
                {
                    Type = AbilityType.NoBubble,
                    Cost = 600,
                }
            },
            { 0x380B, new()
                {
                    Type = AbilityType.FirstBarrier,
                    Cost = 60,
                }
            },
            { 0x389C, new()
                {
                    Type = AbilityType.FirstBarrier,
                    Cost = 50,
                }
            },
            { 0x38B4, new()
                {
                    Type = AbilityType.FirstBarrier,
                    Cost = 40,
                }
            },
            { 0x38CA, new()
                {
                    Type = AbilityType.FirstBarrier,
                    Cost = 30,
                }
            },
            { 0x380C, new()
                {
                    Type = AbilityType.FirstAura,
                    Cost = 600,
                }
            },
            { 0x389D, new()
                {
                    Type = AbilityType.FirstAura,
                    Cost = 400,
                }
            },
            { 0x386F, new()
                {
                    Type = AbilityType.MegaClassPlus1,
                    Cost = 440,
                }
            },
            { 0x38AE, new()
                {
                    Type = AbilityType.MegaClassPlus1,
                    Cost = 400,
                }
            },
            { 0x38C4, new()
                {
                    Type = AbilityType.MegaClassPlus1,
                    Cost = 360,
                }
            },
            { 0x38DA, new()
                {
                    Type = AbilityType.MegaClassPlus1,
                    Cost = 270,
                }
            },
            { 0x38E6, new()
                {
                    Type = AbilityType.MegaClassPlus1,
                    Cost = 200,
                }
            },
            { 0x3871, new()
                {
                    Type = AbilityType.GigaClassPlus1,
                    Cost = 750,
                }
            },
            { 0x38AF, new()
                {
                    Type = AbilityType.GigaClassPlus1,
                    Cost = 700,
                }
            },
            { 0x38C5, new()
                {
                    Type = AbilityType.GigaClassPlus1,
                    Cost = 650,
                }
            },
            { 0x38DB, new()
                {
                    Type = AbilityType.GigaClassPlus1,
                    Cost = 400,
                }
            },
            { 0x380D, new()
                {
                    Type = AbilityType.PlusVShot,
                    Cost = 90,
                }
            },
            { 0x389E, new()
                {
                    Type = AbilityType.PlusVShot,
                    Cost = 80,
                }
            },
            { 0x38B5, new()
                {
                    Type = AbilityType.PlusVShot,
                    Cost = 70,
                }
            },
            { 0x38CB, new()
                {
                    Type = AbilityType.PlusVShot,
                    Cost = 50,
                }
            },
            { 0x380E, new()
                {
                    Type = AbilityType.PlusXShot,
                    Cost = 150,
                }
            },
            { 0x389F, new()
                {
                    Type = AbilityType.PlusXShot,
                    Cost = 130,
                }
            },
            { 0x38B6, new()
                {
                    Type = AbilityType.PlusXShot,
                    Cost = 110,
                }
            },
            { 0x38CC, new()
                {
                    Type = AbilityType.PlusXShot,
                    Cost = 90,
                }
            },
            { 0x38DF, new()
                {
                    Type = AbilityType.PlusXShot,
                    Cost = 45,
                }
            },
            { 0x380F, new()
                {
                    Type = AbilityType.PlusSpreadShot,
                    Cost = 200,
                }
            },
            { 0x38A0, new()
                {
                    Type = AbilityType.PlusSpreadShot,
                    Cost = 180,
                }
            },
            { 0x38B7, new()
                {
                    Type = AbilityType.PlusSpreadShot,
                    Cost = 160,
                }
            },
            { 0x38CD, new()
                {
                    Type = AbilityType.PlusSpreadShot,
                    Cost = 140,
                }
            },
            { 0x38E0, new()
                {
                    Type = AbilityType.PlusSpreadShot,
                    Cost = 100,
                }
            },
            { 0x3810, new()
                {
                    Type = AbilityType.PlusWoodPanel,
                    Cost = 220,
                }
            },
            { 0x38A1, new()
                {
                    Type = AbilityType.PlusWoodPanel,
                    Cost = 200,
                }
            },
            { 0x38B8, new()
                {
                    Type = AbilityType.PlusWoodPanel,
                    Cost = 170,
                }
            },
            { 0x38CE, new()
                {
                    Type = AbilityType.PlusWoodPanel,
                    Cost = 150,
                }
            },
            { 0x38E1, new()
                {
                    Type = AbilityType.PlusWoodPanel,
                    Cost = 50,
                }
            },
            { 0x3811, new()
                {
                    Type = AbilityType.PlusIcePanel,
                    Cost = 220,
                }
            },
            { 0x38A2, new()
                {
                    Type = AbilityType.PlusIcePanel,
                    Cost = 210,
                }
            },
            { 0x38B9, new()
                {
                    Type = AbilityType.PlusIcePanel,
                    Cost = 180,
                }
            },
            { 0x38CF, new()
                {
                    Type = AbilityType.PlusIcePanel,
                    Cost = 160,
                }
            },
            { 0x38E2, new()
                {
                    Type = AbilityType.PlusIcePanel,
                    Cost = 50,
                }
            },
            { 0x3812, new()
                {
                    Type = AbilityType.PlusParaPanel,
                    Cost = 250,
                }
            },
            { 0x38A3, new()
                {
                    Type = AbilityType.PlusParaPanel,
                    Cost = 230,
                }
            },
            { 0x38BA, new()
                {
                    Type = AbilityType.PlusParaPanel,
                    Cost = 210,
                }
            },
            { 0x38D0, new()
                {
                    Type = AbilityType.PlusParaPanel,
                    Cost = 180,
                }
            },
            { 0x38E3, new()
                {
                    Type = AbilityType.PlusParaPanel,
                    Cost = 70,
                }
            },
            { 0x3813, new()
                {
                    Type = AbilityType.PlusGNullPanel,
                    Cost = 200,
                }
            },
            { 0x38A4, new()
                {
                    Type = AbilityType.PlusGNullPanel,
                    Cost = 190,
                }
            },
            { 0x38BB, new()
                {
                    Type = AbilityType.PlusGNullPanel,
                    Cost = 170,
                }
            },
            { 0x38D1, new()
                {
                    Type = AbilityType.PlusGNullPanel,
                    Cost = 140,
                }
            },
            { 0x38E4, new()
                {
                    Type = AbilityType.PlusGNullPanel,
                    Cost = 50,
                }
            },
            { 0x3814, new()
                {
                    Type = AbilityType.PlusPoisonPanel,
                    Cost = 270,
                }
            },
            { 0x38A5, new()
                {
                    Type = AbilityType.PlusPoisonPanel,
                    Cost = 250,
                }
            },
            { 0x38BC, new()
                {
                    Type = AbilityType.PlusPoisonPanel,
                    Cost = 230,
                }
            },
            { 0x38D2, new()
                {
                    Type = AbilityType.PlusPoisonPanel,
                    Cost = 120,
                }
            },
            { 0x3825, new()
                {
                    Type = AbilityType.AutoLock,
                    Cost = 950,
                }
            },
            { 0x3826, new()
                {
                    Type = AbilityType.QuickGauge,
                    Cost = 500,
                }
            },
            { 0x38A6, new()
                {
                    Type = AbilityType.QuickGauge,
                    Cost = 300,
                }
            },
            { 0x3827, new()
                {
                    Type = AbilityType.AntiDamage,
                    Cost = 370,
                }
            },
            { 0x38A7, new()
                {
                    Type = AbilityType.AntiDamage,
                    Cost = 350,
                }
            },
            { 0x38BD, new()
                {
                    Type = AbilityType.AntiDamage,
                    Cost = 330,
                }
            },
            { 0x38D3, new()
                {
                    Type = AbilityType.AntiDamage,
                    Cost = 250,
                }
            },
            { 0x3828, new()
                {
                    Type = AbilityType.DiagLock,
                    Cost = 5,
                }
            },
            { 0x3829, new()
                {
                    Type = AbilityType.SideLock,
                    Cost = 5,
                }
            },
            { 0x382A, new()
                {
                    Type = AbilityType.AcePrgm,
                    Cost = 0,
                }
            },
            { 0x3831, new()
                {
                    Type = AbilityType.AreaEaterS,
                    Cost = 5,
                }
            },
            { 0x3832, new()
                {
                    Type = AbilityType.DblStoneS,
                    Cost = 5,
                }
            },
            { 0x3836, new()
                {
                    Type = AbilityType.PnlFormatS,
                    Cost = 5,
                }
            },
            { 0x382B, new()
                {
                    Type = AbilityType.HumorWord,
                    Cost = 1,
                }
            },
            { 0x382C, new()
                {
                    Type = AbilityType.HumorBstr,
                    Cost = 1,
                }
            },
            { 0x382D, new()
                {
                    Type = AbilityType.HumorBstr2,
                    Cost = 1,
                }
            },

        };

        public static Ability GetAbility(int id)
        {
            if (abilities.TryGetValue(id, out Ability? value))
            {
                return value;
            }
            else
            {
                return new Ability();
            }
        }

        public class Ability
        {
            public Ability()
            {
                Type = AbilityType.Unknown;
                Cost = 0;
            }

            public AbilityType Type { get; set; }
            public int Cost { get; set; }
        }
    }
}
