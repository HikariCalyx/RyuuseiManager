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
            AllowedLinkPower = [ 0 ];
        }

        public Npc Npc { get; set; }
        public int MugshotID { get; set; }
        public int[] AllowedLinkPower { get; set; }

        public static readonly NpcBrother ShiroganeLuna = new NpcBrother()
        {
            Npc = Npc.ShiroganeLuna,
            MugshotID = 240,
            AllowedLinkPower = [0, 60, 100]
        };
        public static readonly NpcBrother UshijimaGonta = new NpcBrother()
        {
            Npc = Npc.UshijimaGonta,
            MugshotID = 7,
            AllowedLinkPower = [0, 60, 100]
        };
        public static readonly NpcBrother SaishoinKizamaro = new NpcBrother()
        {
            Npc = Npc.SaishoinKizamaro,
            MugshotID = 8,
            AllowedLinkPower = [0, 60, 100]
        };
        public static readonly NpcBrother HibikiMisora = new NpcBrother()
        {
            Npc = Npc.HibikiMisora,
            MugshotID = 241,
            AllowedLinkPower = [0, 60, 100]
        };
        public static readonly NpcBrother AkatsukiShido = new NpcBrother()
        {
            Npc = Npc.AkatsukiShido,
            MugshotID = 247,
            AllowedLinkPower = [0, 100]
        };
        public static readonly NpcBrother KinoManabu = new NpcBrother()
        {
            Npc = Npc.KinoManabu,
            MugshotID = 257,
            AllowedLinkPower = [0, 40]
        };
        public static readonly NpcBrother UrakataMamoro = new NpcBrother()
        {
            Npc = Npc.UrakataMamoro,
            MugshotID = 258,
            AllowedLinkPower = [0, 40]
        };
        public static readonly NpcBrother DrYoiri = new NpcBrother()
        {
            Npc = Npc.DrYoiri,
            MugshotID = 277,
            AllowedLinkPower = [0, 40]
        };
        public static readonly NpcBrother FutagoyamaUou = new NpcBrother()
        {
            Npc = Npc.FutagoyamaUou,
            MugshotID = 25,
            AllowedLinkPower = [0, 40]
        };
        public static readonly NpcBrother TetsukugiUtsuzou = new NpcBrother()
        {
            Npc = Npc.TetsukugiUtsuzou,
            MugshotID = 22,
            AllowedLinkPower = [0, 40]
        };
        public static readonly NpcBrother NambaKazuko = new NpcBrother()
        {
            Npc = Npc.NambaKazuko,
            MugshotID = 263,
            AllowedLinkPower = [0, 40]
        };
        public static readonly NpcBrother OikakeMasumi = new NpcBrother()
        {
            Npc = Npc.OikakeMasumi,
            MugshotID = 18,
            AllowedLinkPower = [0, 40]
        };
        public static readonly NpcBrother AtsumeTaiyou = new NpcBrother()
        {
            Npc = Npc.AtsumeTaiyou,
            MugshotID = 17,
            AllowedLinkPower = [0, 40]
        };
        public static readonly NpcBrother Hiroki = new NpcBrother()
        {
            Npc = Npc.Hiroki,
            MugshotID = 262,
            AllowedLinkPower = [0, 40]
        };
        public static readonly NpcBrother KagayakiTerumi = new NpcBrother()
        {
            Npc = Npc.KagayakiTerumi,
            MugshotID = 261,
            AllowedLinkPower = [0, 40]
        };
    }
}
