namespace RyuuseiManager.Library.SF3
{
    public class NoiseFormName
    {
        private static readonly Dictionary<NoiseForm, string> en = new()
        {
            { NoiseForm.None, "No Noise" },
            { NoiseForm.Ox, "Taurus Noise" },
            { NoiseForm.Cygnus, "Cygnus Noise" },
            { NoiseForm.Libra, "Libra Noise" },
            { NoiseForm.Ophiuchus, "Ophiuca Noise" },
            { NoiseForm.Gemini, "Gemini Noise" },
            { NoiseForm.Cancer, "Cancer Noise" },
            { NoiseForm.Wolf, "Wolf Noise" },
            { NoiseForm.Crown, "Crown Noise" },
            { NoiseForm.Virgo, "Virgo Noise" },
            { NoiseForm.Corvus, "Corvus Noise" },
            { NoiseForm.Burai, "Rogue Noise" },
        };

        private static readonly Dictionary<NoiseForm, string> ja = new()
        {
            { NoiseForm.None, "ノイズなし" },
            { NoiseForm.Ox, "オックスノイズ" },
            { NoiseForm.Cygnus, "キグナスノイズ" },
            { NoiseForm.Libra, "リブラノイズ" },
            { NoiseForm.Ophiuchus, "オヒュカスノイズ" },
            { NoiseForm.Gemini, "ジェミニノイズ" },
            { NoiseForm.Cancer, "キャンサーノイズ" },
            { NoiseForm.Wolf, "ウルフノイズ" },
            { NoiseForm.Crown, "クラウンノイズ" },
            { NoiseForm.Virgo, "ヴァルゴノイズ" },
            { NoiseForm.Corvus, "コーヴァスノイズ" },
            { NoiseForm.Burai, "ブライノイズ" },
        };

        private static readonly Dictionary<NoiseForm, string> zh_cn = new()
        {
            { NoiseForm.None, "无噪声变换" },
            { NoiseForm.Ox, "金牛噪声" },
            { NoiseForm.Cygnus, "天鹅噪声" },
            { NoiseForm.Libra, "天秤噪声" },
            { NoiseForm.Ophiuchus, "蛇夫噪声" },
            { NoiseForm.Gemini, "双子噪声" },
            { NoiseForm.Cancer, "巨蟹噪声" },
            { NoiseForm.Wolf, "豺狼噪声" },
            { NoiseForm.Crown, "北冕噪声" },
            { NoiseForm.Virgo, "室女噪声" },
            { NoiseForm.Corvus, "乌鸦噪声" },
            { NoiseForm.Burai, "孤胆噪声" },
        };

        private static readonly Dictionary<NoiseForm, string> zh_tw = new()
        {
            { NoiseForm.None, "無雜訊" },
            { NoiseForm.Ox, "奧克斯雜訊" },
            { NoiseForm.Cygnus, "席格納斯雜訊" },
            { NoiseForm.Libra, "利卜拉雜訊" },
            { NoiseForm.Ophiuchus, "奧修卡斯雜訊" },
            { NoiseForm.Gemini, "傑米尼雜訊" },
            { NoiseForm.Cancer, "坎薩雜訊" },
            { NoiseForm.Wolf, "沃爾伏雜訊" },
            { NoiseForm.Crown, "克朗雜訊" },
            { NoiseForm.Virgo, "瓦露歌雜訊" },
            { NoiseForm.Corvus, "柯爾瓦斯雜訊" },
            { NoiseForm.Burai, "無賴雜訊" },
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
