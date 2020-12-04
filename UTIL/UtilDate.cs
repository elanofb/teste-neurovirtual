using System.Globalization;

namespace System {

    public static class UtilDate {

        //

        //
        public static DateTime? toDateTime(string value) {
            DateTime date;
            return DateTime.TryParse(value, out date) ? date : (DateTime?)null;
        }

        //
        public static DateTime cast(string date) {
            DateTime result = DateTime.MinValue;

            if (String.IsNullOrEmpty(date)) {
                return result;
            }

            DateTime.TryParse(date.Trim(), out result);
            return result;
        }


        //
        public static DateTime? getDate(string strDate, string strFormat) {
            DateTime? result = null;
            string day;
            string month;
            string year;

            if (strFormat.ToLower() == "ddmmyy") {
                if (strDate.Length >= 6) {
                    day = strDate.Substring(0, 2);
                    month = strDate.Substring(2, 2);
                    year = String.Concat("20", strDate.Substring(4, 2));
                    result = new DateTime(UtilNumber.toInt32(year), UtilNumber.toInt32(month), UtilNumber.toInt32(day));
                }
            }

            return result;
        }

        //
        public static bool isValid(string date) {
            DateTime result;
            return DateTime.TryParse(date.Trim(), out result);
        }

        //
        public static bool isFutureDate(DateTime? date) {
            if (date == null)
                return false;
            return (date >= DateTime.Today);
        }

        //
        public static string toDisplay(string date) {
            if (String.IsNullOrEmpty(date))
                return "";
            DateTime result;
            DateTime.TryParse(date.Trim(), out result);
            return result.ToShortDateString();
        }

        //
        //Verificar a diferença de dias considerando apenas dias úteis
        public static int diffDaysWeekDays(DateTime initialDate, DateTime finalDate, bool saturdayUtil = false) {
            int days;
            int daysCount = 0;
            days = initialDate.Subtract(finalDate).Days;

            //Módulo 
            if (days < 0)
                days = days * -1;

            for (int i = 1; i <= days; i++) {
                initialDate = initialDate.AddDays(1);
                if (initialDate.DayOfWeek != DayOfWeek.Sunday) {
                    if (initialDate.DayOfWeek != DayOfWeek.Saturday || saturdayUtil) {
                        daysCount++;
                    }
                }
            }
            return daysCount;
        }


        //
        public static string retornarMesPorExtenso(int mes) {
            string mesConvertido;

            switch (mes) {
                case 1:
                    mesConvertido = "Janeiro";
                    break;
                case 2:
                    mesConvertido = "Fevereiro";
                    break;
                case 3:
                    mesConvertido = "Março";
                    break;
                case 4:
                    mesConvertido = "Abril";
                    break;
                case 5:
                    mesConvertido = "Maio";
                    break;
                case 6:
                    mesConvertido = "Junho";
                    break;
                case 7:
                    mesConvertido = "Julho";
                    break;
                case 8:
                    mesConvertido = "Agosto";
                    break;
                case 9:
                    mesConvertido = "Setembro";
                    break;
                case 10:
                    mesConvertido = "Outubro";
                    break;
                case 11:
                    mesConvertido = "Novembro";
                    break;
                case 12:
                    mesConvertido = "Dezembro";
                    break;
                default:
                    mesConvertido = "Janeiro";
                    break;

            }

            return mesConvertido;
        }


        //Exibir uma data por escrito por extenso
        public static string exibirExtenso(DateTime data, String idioma = "pt-BR") {
            //Mês INT
            int mes = data.Month;
            string mesExtenso;
            string diaExtenso;

            CultureInfo Cultura = new CultureInfo(idioma);

            //Mês abreviado em português também.
            Cultura.DateTimeFormat.GetAbbreviatedMonthName(mes);

            //Mês (int) por extenso com primeira letra maiúscula.
            string month = Cultura.DateTimeFormat.GetMonthName(mes);
            mesExtenso = char.ToUpper(month[0]) + month.Substring(1);

            //Dia da semana (int) por extenso em português (segunda-feira)
            diaExtenso = Cultura.DateTimeFormat.GetDayName(data.DayOfWeek);

            //Dia da semana abreviado (seg)
            Cultura.DateTimeFormat.GetAbbreviatedDayName(data.DayOfWeek);

            string retorno = String.Concat(diaExtenso, ", ", data.Day.ToString(), " de ", mesExtenso, " de ", data.Year.ToString());
            return retorno;
        }


        //Retorno o Datetime do próximo dia da semana (ex: próxima sexta, próximo sábado, etc)
        public static DateTime getNextDay(DateTime startDate, DayOfWeek day) {

            // The (... + 7) % 7 ensures we end up with a value in the range [0, 6]
            int daysUntil = ((int)day - (int)startDate.DayOfWeek + 7) % 7;

            DateTime nextDay = startDate.AddDays(daysUntil);

            return nextDay;
        }

        //Retorno o Datetime do próximo dia da semana (ex: próxima sexta, próximo sábado, etc)
        public static DateTime getNextDay(int dayOfWeek) {
            DateTime today = DateTime.Today;

            // The (... + 7) % 7 ensures we end up with a value in the range [0, 6]
            int daysUntil = (dayOfWeek - (int)today.DayOfWeek + 7) % 7;

            DateTime nextDay = today.AddDays(daysUntil);
            return nextDay;
        }

        //Retorno o Datetime do próximo dia da semana (ex: próxima sexta, próximo sábado, etc)
        public static DateTime getNextBusinessDay(bool flagIncludeSaturday = false, int qtdeDias = 1) {
            DateTime nextDay = DateTime.Today.AddDays(qtdeDias);

            if (nextDay.DayOfWeek == DayOfWeek.Sunday) {
                return nextDay.AddDays(1);
            }

            if ((nextDay.DayOfWeek == DayOfWeek.Saturday) && (!flagIncludeSaturday)) {
                return nextDay.AddDays(2);
            }

            return nextDay;
        }

        //Calcular a idade
        public static string calcularIdade(DateTime? dtNascimento) {

            DateTime dAtual = DateTime.Today;

            int idAnos = 0; // (id = idade)

            string ta = "-";

            if (!dtNascimento.HasValue || dAtual < dtNascimento) {
                return ta;
            }


            if (dAtual.Month < dtNascimento.Value.Month) {
                idAnos = -1;
            }

            idAnos = dAtual.Year - dtNascimento.Value.Year + idAnos;

            if (idAnos > 1) {

                ta = idAnos + " anos ";

            } else {

                ta = idAnos + "ano";
            }

            return ta;
        }

        
        /// <summary>
        /// Receber uma string de data e tentar converter para tipo DateTime
        /// </summary>
        public static DateTime? dateFromUTC(string strDate, string strFormat = "yyyy-MM-ddTHHmmsszzz", int fusoHorario = -3) {

            DateTimeOffset dtConvert;

            

            if (DateTimeOffset.TryParse(strDate, out dtConvert)) {

                return dtConvert.DateTime.AddHours(fusoHorario);

            }

            return null;
        }

    }
}
