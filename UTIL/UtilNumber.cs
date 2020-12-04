using System.Globalization;

namespace System{

    public static class UtilNumber{


        //
        public static bool isInt32(object value){
            int ret = 0;
            if (value != null) {
                return int.TryParse(value.ToString(), out ret);
            }
            return false;
        }

        //
        public static int toInt32(int? value){
            if (value == null) return 0;
            else return Convert.ToInt32(value.ToString());
        }


        //
        public static int toInt32(object value){
            int ret = 0;
            if (value != null) {
                int.TryParse(value.ToString(), out ret);
            }
            return ret;
        }


        //
        public static double toDouble(string str){
            double result = 0;
            Double.TryParse(str, out result);
            return result;
        }


        //
        public static decimal toDecimal(string str){
            decimal result = 0;
            Decimal.TryParse(str, out result);
            return result;
        }

          //
        public static decimal toDecimal(decimal? valor){
            
            decimal result = 0;
            
            if (valor == null) { 
                return result;
            }

            Decimal.TryParse(valor.ToString(), out result);
            return result;
        }

        //
        public static decimal valorPercentual(this decimal valor, decimal percent){

			if (percent == 0) { 
                return new decimal(0);
            }

			decimal fatorPercent = Decimal.Divide(percent, new decimal(100));
	        
			decimal valorCalculado = Decimal.Multiply(valor, fatorPercent);

			return valorCalculado;
        }

        //
        public static decimal toDecimalMod100(string str){
            decimal result = 0;

            str = str.Replace(",", "").Replace(".", "");

            int intValue = UtilNumber.toInt32(str);

            Decimal.TryParse( decimal.Divide(intValue,100).ToString(CultureInfo.InvariantCulture), out result);

            return result;
        }

        //Calcular percentual 
        public static decimal percentual(decimal total, decimal parcial){
			if (total == 0) { 
				return new Decimal(0);
			}

			decimal fator = Decimal.Divide(100, total);
			decimal percent = Decimal.Multiply(fator, parcial);

            return percent;
        }

        //Calcular percentual 
        public static int toCents(decimal? valorTotal){
			
			if (valorTotal == 0) { 
				return 0;
			}

			string valorCentavos = UtilString.onlyNumber(valorTotal.ToString());

            return UtilNumber.toInt32(valorCentavos);
        }

 
    }
}
