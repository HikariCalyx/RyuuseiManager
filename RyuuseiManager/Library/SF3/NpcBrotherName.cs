using System;
using System.Collections.Generic;
using System.Text;

namespace RyuuseiManager.Library.SF3
{
    public class NpcBrotherName
    {
        public static readonly Dictionary<NpcBrother, string> en = new()
        {
            { NpcBrother.ShiroganeLuna, "Luna Platz" },
            { NpcBrother.UshijimaGonta, "Bud Bison" },
            { NpcBrother.SaishoinKizamaro, "Zack Temple" },
            { NpcBrother.HibikiMisora, "Sonia Strumm" },
            { NpcBrother.AkatsukiShido, "Arthur C. Eos" },
            { NpcBrother.KinoManabu, "Woodrow Boffin" },
            { NpcBrother.UrakataMamoro, "Lee Stern" },
            { NpcBrother.DrYoiri, "Dr. Goodall" },
            { NpcBrother.FutagoyamaUou, "Jax Dublez" },
            { NpcBrother.TetsukugiUtsuzou, "Hamer Nayl" },
            { NpcBrother.NambaKazuko, "Trish Trig" },
            { NpcBrother.OikakeMasumi, "Fany Chase" },
            { NpcBrother.AtsumeTaiyou, "Don McCard" },
            { NpcBrother.Hiroki, "Gian Luigi" },
            { NpcBrother.KagayakiTerumi, "Onda Sette" },
        };

        public static readonly Dictionary<NpcBrother, string> ja = new()
        {
            { NpcBrother.ShiroganeLuna, "白金 ルナ" },
            { NpcBrother.UshijimaGonta, "牛島 ゴン太" },
            { NpcBrother.SaishoinKizamaro, "最小院 キザマロ" },
            { NpcBrother.HibikiMisora, "響 ミソラ" },
            { NpcBrother.AkatsukiShido, "暁 シドウ" },
            { NpcBrother.KinoManabu, "木野 マナブ" },
            { NpcBrother.UrakataMamoro, "浦方 マモロウ" },
            { NpcBrother.DrYoiri, "Dr. ヨイリー" },
            { NpcBrother.FutagoyamaUou, "ふたごやま うおう" }, //35
            { NpcBrother.TetsukugiUtsuzou, "テツクギ ウツゾウ" }, //36
            { NpcBrother.NambaKazuko, "なんば かずこ" },//57
            { NpcBrother.OikakeMasumi, "おいかけ マスミ" },//58
            { NpcBrother.AtsumeTaiyou, "あつめ たいよう" },//64
            { NpcBrother.Hiroki, "ヒロキ" },//100
            { NpcBrother.KagayakiTerumi, "かがやき テルミ" },//122
        };

        public static readonly Dictionary<NpcBrother, string> zh_cn = new()
        {
            { NpcBrother.ShiroganeLuna, "白金 露娜" },
            { NpcBrother.UshijimaGonta, "牛岛 权太" },
            { NpcBrother.SaishoinKizamaro, "最小院 刻麻吕" },
            { NpcBrother.HibikiMisora, "响 美空" },
            { NpcBrother.AkatsukiShido, "晓 狮童" },
            { NpcBrother.KinoManabu, "木野 学" },
            { NpcBrother.UrakataMamoro, "浦方 护" },
            { NpcBrother.DrYoiri, "尤莉博士" },
            { NpcBrother.FutagoyamaUou, "双子山 弥夫" },
            { NpcBrother.TetsukugiUtsuzou, "木宫 丁蔵" },
            { NpcBrother.NambaKazuko, "难波 和子" },
            { NpcBrother.OikakeMasumi, "椎星 真澄" },
            { NpcBrother.AtsumeTaiyou, "守吉 太阳" },
            { NpcBrother.Hiroki, "广树" },
            { NpcBrother.KagayakiTerumi, "涉影 照美" },
        };

        public static readonly Dictionary<NpcBrother, string> zh_tw = new()
        {
            { NpcBrother.ShiroganeLuna, "白金露娜" },
            { NpcBrother.UshijimaGonta, "牛島權太" },
            { NpcBrother.SaishoinKizamaro, "最小院刻麻呂" },
            { NpcBrother.HibikiMisora, "響美空" },
            { NpcBrother.AkatsukiShido, "曉獅童" },
            { NpcBrother.KinoManabu, "木野學" },
            { NpcBrother.UrakataMamoro, "浦方護" },
            { NpcBrother.DrYoiri, "優莉博士" },
            { NpcBrother.FutagoyamaUou, "霜包苔祐旺" },
            { NpcBrother.TetsukugiUtsuzou, "鍬帖丁藏" },
            { NpcBrother.NambaKazuko, "難波和子" },
            { NpcBrother.OikakeMasumi, "椎星愛美" },
            { NpcBrother.AtsumeTaiyou, "守吉太陽" },
            { NpcBrother.Hiroki, "廣樹" },
            { NpcBrother.KagayakiTerumi, "旬照美井" },
        };

        public static string GetBrotherName(NpcBrother brother, int language)
        {
            Dictionary<NpcBrother, string> targetLanguage = new Dictionary<NpcBrother, string>();
            switch (language)
            {
                case 0: targetLanguage = en; break;
                case 1: targetLanguage = ja; break;
                case 2: targetLanguage = zh_cn; break;
                case 3: targetLanguage = zh_tw; break;
            }
            if (targetLanguage.TryGetValue(brother, out string? value))
            {
                return value;
            }
            else
            {
                return "(null)";
            }
        }
    }
}
