using System;
using System.Collections.Generic;
using System.Text;

namespace RyuuseiManager.Library.SF3
{
    public class WeaponName
    {
        public static readonly Dictionary<WeaponType, string> en = new()
        {
            { WeaponType.Unknown, "Unknown" },
            { WeaponType.SharpFang, "Pointed Fangs" },
            { WeaponType.TangaroaPierce, "Star Hoop" },
            { WeaponType.KageshibariNail, "Shadow Claw" },
            { WeaponType.QuickRing, "Quick Ring" },
            { WeaponType.WaterDragonFang, "Aqua Fang" },
            { WeaponType.ShippuuPierce, "Storm Hoop" },
            { WeaponType.ParalysisWing, "Paralyze Wing" },
            { WeaponType.MachineGunClaw, "R-F Claw" },
            { WeaponType.ZennyDecide, "Zenny Finder" },
            { WeaponType.TitanGlove, "Titan Glove" },
            { WeaponType.SenkoBracelet, "Flash Band" },
            { WeaponType.KaiserKnuckle, "K. Knuckle" },
            { WeaponType.BugRiseClaw, "Virus Claw" },
            { WeaponType.IllusionPupil, "Hypno Eye" },
            { WeaponType.ImpactBracelet, "Impact Roar" },
            { WeaponType.CollectorEye, "Card Finder" },
            { WeaponType.CosmicRave, "Cosmic Rave" },
            { WeaponType.NightSpirit, "Chivalry" },
            { WeaponType.WhiteFang, "White Fang" },
            { WeaponType.HeadwindNail, "Gale Claw" },
            { WeaponType.DangerRing, "Danger Ring" },
            { WeaponType.EXEBlasterEX, "BN Blaster EX" },
        };

        public static readonly Dictionary<WeaponType, string> ja = new()
        {
            { WeaponType.Unknown, "不明" },
            { WeaponType.SharpFang, "スルドイキバ" },
            { WeaponType.TangaroaPierce, "タンガロアピアス" },
            { WeaponType.KageshibariNail, "カゲシバリノツメ" },
            { WeaponType.QuickRing, "クイックリング" },
            { WeaponType.WaterDragonFang, "スイリュウノキバ" },
            { WeaponType.ShippuuPierce, "シップウノピアス" },
            { WeaponType.ParalysisWing, "パラライズウィング" },
            { WeaponType.MachineGunClaw, "マシンガンクロー" },
            { WeaponType.ZennyDecide, "ゼニーディサイド" },
            { WeaponType.TitanGlove, "タイタングローブ" },
            { WeaponType.SenkoBracelet, "センコウノウデワ" },
            { WeaponType.KaiserKnuckle, "カイザーナックル" },
            { WeaponType.BugRiseClaw, "バグライズクロー" },
            { WeaponType.IllusionPupil, "ゲンワクノヒトミ" },
            { WeaponType.ImpactBracelet, "インパクトブレス" },
            { WeaponType.CollectorEye, "コレクターアイ" },
            { WeaponType.CosmicRave, "コズミックレイブ" },
            { WeaponType.NightSpirit, "ナイトスピリット" },
            { WeaponType.WhiteFang, "ホワイトファング" },
            { WeaponType.HeadwindNail, "ギャクフウノツメ" },
            { WeaponType.DangerRing, "デンジャーリング" },
            { WeaponType.EXEBlasterEX, "エグゼブラスターＥＸ" },
        };

        public static readonly Dictionary<WeaponType, string> zh_cn = new()
        {
            { WeaponType.Unknown, "未知" },
            { WeaponType.SharpFang, "锐利之牙" },
            { WeaponType.TangaroaPierce, "海神耳环" },
            { WeaponType.KageshibariNail, "缚影爪" },
            { WeaponType.QuickRing, "迅捷戒指" },
            { WeaponType.WaterDragonFang, "水龙尖牙" },
            { WeaponType.ShippuuPierce, "疾风耳钉" },
            { WeaponType.ParalysisWing, "麻痹之翼" },
            { WeaponType.MachineGunClaw, "机关枪钩爪" },
            { WeaponType.ZennyDecide, "Zenny收集器" },
            { WeaponType.TitanGlove, "泰坦手套" },
            { WeaponType.SenkoBracelet, "闪光手镯" },
            { WeaponType.KaiserKnuckle, "王者铁拳" },
            { WeaponType.BugRiseClaw, "BUG钩爪" },
            { WeaponType.IllusionPupil, "幻惑之眼" },
            { WeaponType.ImpactBracelet, "撼地手环" },
            { WeaponType.CollectorEye, "收藏家之眼" },
            { WeaponType.CosmicRave, "宇宙狂潮" },
            { WeaponType.NightSpirit, "骑士真魂" },
            { WeaponType.WhiteFang, "苍白獠牙" },
            { WeaponType.HeadwindNail, "逆风爪" },
            { WeaponType.DangerRing, "凶戒" },
            { WeaponType.EXEBlasterEX, "EXE爆破枪EX" },
        };

        public static readonly Dictionary<WeaponType, string> zh_tw = new()
        {
            { WeaponType.Unknown, "未知" },
            { WeaponType.SharpFang, "銳利之牙" },
            { WeaponType.TangaroaPierce, "海神耳環" },
            { WeaponType.KageshibariNail, "影縛爪" },
            { WeaponType.QuickRing, "迅敏戒指" },
            { WeaponType.WaterDragonFang, "水龍之牙" },
            { WeaponType.ShippuuPierce, "疾風耳環" },
            { WeaponType.ParalysisWing, "麻痺之翼" },
            { WeaponType.MachineGunClaw, "機關砲爪" },
            { WeaponType.ZennyDecide, "Zenny收集器" },
            { WeaponType.TitanGlove, "泰坦手套" },
            { WeaponType.SenkoBracelet, "閃光手鐲" },
            { WeaponType.KaiserKnuckle, "凱薩鐵拳" },
            { WeaponType.BugRiseClaw, "Bug強化爪" },
            { WeaponType.IllusionPupil, "迷惑之瞳" },
            { WeaponType.ImpactBracelet, "衝擊手環" },
            { WeaponType.CollectorEye, "收藏家之眼" },
            { WeaponType.CosmicRave, "宇宙狂潮" },
            { WeaponType.NightSpirit, "騎士之魂" },
            { WeaponType.WhiteFang, "白色獠牙" },
            { WeaponType.HeadwindNail, "逆風之爪" },
            { WeaponType.DangerRing, "危險戒指" },
            { WeaponType.EXEBlasterEX, "EXE爆破槍EX" },
        };

        public static string GetWeaponName(WeaponType type, int language)
        {
            Dictionary<WeaponType, string> targetLanguage = new Dictionary<WeaponType, string>();
            switch (language)
            {
                case 0: targetLanguage = en; break;
                case 1: targetLanguage = ja; break;
                case 2: targetLanguage = zh_cn; break;
                case 3: targetLanguage = zh_tw; break;
            }
            if (targetLanguage.TryGetValue(type, out string? value))
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
