using System.Web;
using System.Text.RegularExpressions;
using System.Collections;

namespace System {

    public class UtilValidation {

        //
        public UtilValidation() {

        }

        //
        public static bool isCPFCNPJ(string cpfcnpj, bool vazio = false) {

            if (string.IsNullOrEmpty(cpfcnpj)) {
                return vazio;
            } 

            return isCPF(cpfcnpj) || isCNPJ(cpfcnpj);

        }

        /// <summary>
        /// Verificar se o documento informado é um CNPJ
        /// </summary>
        public static bool isCNPJ(string cnpj) {

            int[] mt1 = new int[12] { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] mt2 = new int[13] { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            int soma;
            int resto;
            string digito;
            string TempCNPJ;

            cnpj = cnpj.Trim();
            cnpj = cnpj.onlyNumber();

            if (cnpj.Length != 14)
                return false;

            if (cnpj == "00000000000000" || cnpj == "11111111111111" || cnpj == "22222222222222" || cnpj == "33333333333333" || cnpj == "44444444444444" || cnpj == "55555555555555" ||
                cnpj == "66666666666666" || cnpj == "77777777777777" || cnpj == "88888888888888" || cnpj == "99999999999999")
                return false;

            TempCNPJ = cnpj.Substring(0, 12);
            soma = 0;

            for (int i = 0; i < 12; i++)
                soma += int.Parse(TempCNPJ[i].ToString()) * mt1[i];

            resto = (soma % 11);
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;

            digito = resto.ToString();

            TempCNPJ = TempCNPJ + digito;
            soma = 0;
            for (int i = 0; i < 13; i++)
                soma += int.Parse(TempCNPJ[i].ToString()) * mt2[i];

            resto = (soma % 11);
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;
            digito = digito + resto.ToString();

            return cnpj.EndsWith(digito);
        }

        /// <summary>
        /// Verificar se o documento informado é um CPF válido
        /// </summary>
        public static bool isCPF(string CPF) {

            if (CPF.stringOrEmpty().onlyNumber().Length != 11) {
                return false;
            }

            int[] mt1 = new int[9] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] mt2 = new int[10] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            string TempCPF;
            string Digito;
            int soma;
            int resto;

            CPF = CPF.Trim();
            CPF = CPF.onlyNumber();

            if (CPF.Length != 11)
                return false;

            if (CPF == "00000000000" || CPF == "11111111111" || CPF == "22222222222" || CPF == "33333333333" || CPF == "44444444444" || CPF == "55555555555" ||
                CPF == "66666666666" || CPF == "77777777777" || CPF == "88888888888" || CPF == "99999999999")
                return false;

            TempCPF = CPF.Substring(0, 9);
            soma = 0;
            for (int i = 0; i < 9; i++)
                soma += int.Parse(TempCPF[i].ToString()) * mt1[i];

            resto = soma % 11;
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;

            Digito = resto.ToString();
            TempCPF = TempCPF + Digito;
            soma = 0;

            for (int i = 0; i < 10; i++)
                soma += int.Parse(TempCPF[i].ToString()) * mt2[i];

            resto = soma % 11;
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;

            Digito = Digito + resto.ToString();

            return CPF.EndsWith(Digito);
        }

        /// <summary>
        /// Validar string para checar padroes de e-mail
        /// </summary>
        public static bool isEmail(string enderecoEmail) {

            if (enderecoEmail.isEmpty()) {
                return false;
            }

            bool isEmail = Regex.IsMatch(enderecoEmail, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);

            return isEmail;
        }


        /// <summary>
        /// Validar string para checar padroes de cep
        /// </summary>
        public static bool isCEP(string cep) {

            if (cep.stringOrEmpty().onlyNumber().Length < 8) {

                return false;
            }

            if (cep == "00000000" || cep == "11111111" || cep == "22222222" || cep == "33333333" || cep == "44444444" ||
                cep == "55555555" || cep == "66666666" || cep == "77777777" || cep == "88888888" || cep == "99999999") {

                return false;
            }

            return true;
        }

        /// <summary>
        /// Validar um número de cartao
        /// </summary>
        public static bool isCardNumber(string cardNumber) {

            try {
                // Array to contain individual numbers
                System.Collections.ArrayList CheckNumbers = new ArrayList();
                // So, get length of card
                int CardLength = cardNumber.Length;

                // Double the value of alternate digits, starting with the second digit
                // from the right, i.e. back to front.
                // Loop through starting at the end
                for (int i = CardLength - 2; i >= 0; i = i - 2) {
                    // Now read the contents at each index, this
                    // can then be stored as an array of integers

                    // Double the number returned
                    CheckNumbers.Add(Int32.Parse(cardNumber[i].ToString()) * 2);
                }

                int CheckSum = 0;    // Will hold the total sum of all checksum digits

                // Second stage, add separate digits of all products
                for (int iCount = 0; iCount <= CheckNumbers.Count - 1; iCount++) {
                    int _count = 0;    // will hold the sum of the digits

                    // determine if current number has more than one digit
                    if ((int)CheckNumbers[iCount] > 9) {
                        int _numLength = ((int)CheckNumbers[iCount]).ToString().Length;
                        // add count to each digit
                        for (int x = 0; x < _numLength; x++) {
                            _count = _count + Int32.Parse(
                                  ((int)CheckNumbers[iCount]).ToString()[x].ToString());
                        }
                    } else {
                        // single digit, just add it by itself
                        _count = (int)CheckNumbers[iCount];
                    }
                    CheckSum = CheckSum + _count;    // add sum to the total sum
                }
                // Stage 3, add the unaffected digits
                // Add all the digits that we didn't double still starting from the
                // right but this time we'll start from the rightmost number with 
                // alternating digits
                int OriginalSum = 0;
                for (int y = CardLength - 1; y >= 0; y = y - 2) {
                    OriginalSum = OriginalSum + Int32.Parse(cardNumber[y].ToString());
                }

                // Perform the final calculation, if the sum Mod 10 results in 0 then
                // it's valid, otherwise its false.
                return (((OriginalSum + CheckSum) % 10) == 0);
            } catch {
                return false;
            }
        }

    }
}
