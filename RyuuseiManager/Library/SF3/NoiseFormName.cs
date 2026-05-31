namespace RyuuseiManager.Library.SF3
{
    public class NoiseFormName
    {
        private static readonly Dictionary<NoiseForm, string> en = new()
        {
            { NoiseForm.None, "No" },
            { NoiseForm.Ox, "Taurus" },
            { NoiseForm.Cygnus, "Cygnus" },
            { NoiseForm.Libra, "Libra" },
            { NoiseForm.Ophiuchus, "Ophiuca" },
            { NoiseForm.Gemini, "Gemini" },
            { NoiseForm.Cancer, "Cancer" },
            { NoiseForm.Wolf, "Wolf" },
            { NoiseForm.Crown, "Crown" },
            { NoiseForm.Virgo, "Virgo" },
            { NoiseForm.Corvus, "Corvus" },
            { NoiseForm.Burai, "Rogue" },
        };

        private static readonly Dictionary<NoiseForm, string> ja = new()
        {
            { NoiseForm.None, "なし" },
            { NoiseForm.Ox, "オックス" },
            { NoiseForm.Cygnus, "キグナス" },
            { NoiseForm.Libra, "リブラ" },
            { NoiseForm.Ophiuchus, "オヒュカス" },
            { NoiseForm.Gemini, "ジェミニ" },
            { NoiseForm.Cancer, "キャンサー" },
            { NoiseForm.Wolf, "ウルフ" },
            { NoiseForm.Crown, "クラウン" },
            { NoiseForm.Virgo, "ヴァルゴ" },
            { NoiseForm.Corvus, "コーヴァス" },
            { NoiseForm.Burai, "ブライ" },
        };

        private static readonly Dictionary<NoiseForm, string> zh_cn = new()
        {
            { NoiseForm.None, "无" },
            { NoiseForm.Ox, "金牛" },
            { NoiseForm.Cygnus, "天鹅" },
            { NoiseForm.Libra, "天秤" },
            { NoiseForm.Ophiuchus, "蛇夫" },
            { NoiseForm.Gemini, "双子" },
            { NoiseForm.Cancer, "巨蟹" },
            { NoiseForm.Wolf, "豺狼" },
            { NoiseForm.Crown, "北冕" },
            { NoiseForm.Virgo, "室女" },
            { NoiseForm.Corvus, "乌鸦" },
            { NoiseForm.Burai, "孤胆" },
        };

        private static readonly Dictionary<NoiseForm, string> zh_tw = new()
        {
            { NoiseForm.None, "無" },
            { NoiseForm.Ox, "奧克斯" },
            { NoiseForm.Cygnus, "席格納斯" },
            { NoiseForm.Libra, "利卜拉" },
            { NoiseForm.Ophiuchus, "奧修卡斯" },
            { NoiseForm.Gemini, "傑米尼" },
            { NoiseForm.Cancer, "坎薩" },
            { NoiseForm.Wolf, "沃爾伏" },
            { NoiseForm.Crown, "克朗" },
            { NoiseForm.Virgo, "瓦露歌" },
            { NoiseForm.Corvus, "柯爾瓦斯" },
            { NoiseForm.Burai, "無賴" },
        };

        public static string GetNoiseFormName(NoiseForm form, int language)
        {
            Dictionary<NoiseForm, string> targetLanguage = new Dictionary<NoiseForm, string>();
            switch (language)
            {
                case 0: targetLanguage = en; break;
                case 1: targetLanguage = ja; break;
                case 2: targetLanguage = zh_cn; break;
                case 3: targetLanguage = zh_tw; break;
            }
            if (targetLanguage.TryGetValue(form, out string? value))
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
