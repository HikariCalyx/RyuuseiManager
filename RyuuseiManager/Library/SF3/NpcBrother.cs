using System;
using System.Collections.Generic;
using System.Text;

namespace RyuuseiManager.Library.SF3
{
    public class NpcBrother
    {
        public NpcBrother()
        {
            Npc = Npc.Unknown;
            MugshotID = 248;
        }

        public Npc Npc { get; set; }
        public int MugshotID { get; set; }

        public static readonly NpcBrother ShiroganeLuna = new NpcBrother()
        {
            Npc = Npc.ShiroganeLuna,
            MugshotID = 240
        };
        public static readonly NpcBrother UshijimaGonta = new NpcBrother()
        {
            Npc = Npc.UshijimaGonta,
            MugshotID = 7
        };
        public static readonly NpcBrother SaishoinKizamaro = new NpcBrother()
        {
            Npc = Npc.SaishoinKizamaro,
            MugshotID = 8
        };
        public static readonly NpcBrother HibikiMisora = new NpcBrother()
        {
            Npc = Npc.HibikiMisora,
            MugshotID = 241
        };
        public static readonly NpcBrother AkatsukiShido = new NpcBrother()
        {
            Npc = Npc.AkatsukiShido,
            MugshotID = 247
        };
        public static readonly NpcBrother KinoManabu = new NpcBrother()
        {
            Npc = Npc.KinoManabu,
            MugshotID = 257
        };
        public static readonly NpcBrother UrakataMamoro = new NpcBrother()
        {
            Npc = Npc.UrakataMamoro,
            MugshotID = 258
        };
        public static readonly NpcBrother DrYoiri = new NpcBrother()
        {
            Npc = Npc.DrYoiri,
            MugshotID = 277
        };
        public static readonly NpcBrother FutagoyamaUou = new NpcBrother()
        {
            Npc = Npc.FutagoyamaUou,
            MugshotID = 25
        };
        public static readonly NpcBrother TetsukugiUtsuzou = new NpcBrother()
        {
            Npc = Npc.TetsukugiUtsuzou,
            MugshotID = 22
        };
        public static readonly NpcBrother NambaKazuko = new NpcBrother()
        {
            Npc = Npc.NambaKazuko,
            MugshotID = 263
        };
        public static readonly NpcBrother OikakeMasumi = new NpcBrother()
        {
            Npc = Npc.OikakeMasumi,
            MugshotID = 18
        };
        public static readonly NpcBrother AtsumeTaiyou = new NpcBrother()
        {
            Npc = Npc.AtsumeTaiyou,
            MugshotID = 17
        };
        public static readonly NpcBrother Hiroki = new NpcBrother()
        {
            Npc = Npc.Hiroki,
            MugshotID = 262
        };
        public static readonly NpcBrother KagayakiTerumi = new NpcBrother()
        {
            Npc = Npc.KagayakiTerumi,
            MugshotID = 261
        };
    }
}
