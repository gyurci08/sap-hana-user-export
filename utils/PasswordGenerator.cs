using System;
using System.Linq;
using System.Text;

namespace sap_hana_user_export.utils
{
    public class PasswordGenerator
    {
        internal string Generate(int CharLength, int SpecNum)
        {
            Random rnd = new Random();
            string nChars = string.Empty;
            string sChars = string.Empty;

            for (int i = 0; i <= CharLength && i < (CharLength - SpecNum); i++)
            {
                nChars += (char)(rnd.Next(2) == 0 ? rnd.Next(48, 57) :
                                 rnd.Next(2) == 0 ? rnd.Next(65, 90) : rnd.Next(97, 122));
            }

            for (int i = 0; i < SpecNum && i < SpecNum; i++)
            {
                sChars += (char)(rnd.Next(2) == 0 ? rnd.Next(33, 47) :
                                 rnd.Next(2) == 0 ? rnd.Next(58, 64) : rnd.Next(123, 126));
            }

            char[] ch = (nChars + sChars).ToCharArray();
            Random random = new Random();

            for (int i = ch.Length - 1; i > 0; i--)
            {
                int j = random.Next(0, i + 1);
                (ch[i], ch[j]) = (ch[j], ch[i]);
            }
            return new string(ch);
        }
    }
}
