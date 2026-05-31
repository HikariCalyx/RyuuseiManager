using System.Text;

namespace RyuuseiManager.StringProcessor
{
    public static class JP
    {
        public static string Generalize(string input)
        {
            string s = input.Normalize(NormalizationForm.FormKC);
            var sb = new StringBuilder(s.Length);
            foreach (char c in s)
            {
                if (c >= 'ぁ' && c <= 'ゖ')
                    sb.Append((char)(c + 0x60));
                else
                    sb.Append(c);
            }
            return sb.ToString();
        }
    }
}
