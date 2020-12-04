using System.Security.Cryptography;
using System.Text;

namespace System {

    public class UtilCrypt {

        public static readonly string saltSistema = "SINC_2013_GROUP_BLA*9!##XYZ";
        public static readonly long fatorNumerico = 10137;

        //
        public UtilCrypt() {

        }

        //Devolve uma string sem acentos
        public static string SHA512(string str) {

            string cryprStr = String.Concat(UtilCrypt.saltSistema, str);
            UnicodeEncoding UE = new UnicodeEncoding();
            byte[] HashValue, MessageBytes = UE.GetBytes(cryprStr);

            SHA512Managed SHhash = new SHA512Managed();
            HashValue = SHhash.ComputeHash(MessageBytes);

            string strHex = "";
            foreach (byte b in HashValue) {
                strHex += String.Format("{0:x2}", b);
            }
            return strHex;
        }

        //
        public static string toBase64Encode(int value) {
            long strToEncode = (value * UtilCrypt.fatorNumerico);
            byte[] toEncodeAsBytes = System.Text.Encoding.Unicode.GetBytes(strToEncode.ToString());
            string returnValue = System.Convert.ToBase64String(toEncodeAsBytes);
            return returnValue.Trim();
        }

        //
        public static string toBase64Decode(string encodedString) {

            if (encodedString.LengthNullable() % 4 != 0) {

                return string.Empty;
            }

            byte[] data = Convert.FromBase64String(encodedString);

            string strToDecode = Encoding.Unicode.GetString(data);

            long value = (UtilNumber.toInt32(strToDecode) / UtilCrypt.fatorNumerico);

            return value.ToString();
        }

        //
        public static string signRecipe(string prefix, string value) {
            string sign = String.Concat(prefix, "_", value.Replace("=", "$#"));
            return sign;
        }

    }
}
