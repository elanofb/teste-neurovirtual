
using System.Globalization;

namespace System{

    public static class NumberExtensions{


        //Extensao para facilitar a conversao de int nullable
        public static short toShort(this int? value){

            if (value == null) return 0;

            return Convert.ToInt16(value.ToString());
        }
        
        //Extensao para facilitar a conversao de int nullable
        public static int toInt(this int? value){

            if (value == null) return 0;

            return Convert.ToInt32(value.ToString());
        }

        //
        public static decimal toDecimal(this decimal? value){

            if (value == null) return 0;

            return Convert.ToDecimal(value.ToString());
        }
        
        //
        public static decimal toDecimal(this byte? number){
            decimal result = new decimal(number ?? 0);

            return result;
        }
        
        //
        public static decimal toDecimal(this byte number){
            decimal result = new decimal(number);

            return result;
        }

        //
        public static decimal toDecimal(this int? number){
            decimal result = new decimal(number ?? 0);

            return result;
        }
        
        //
        public static decimal toDecimal(this int number){
            decimal result = new decimal(number);

            return result;
        }
        
        //
        public static decimal toDecimal(this object value){
            
            if (value == null) return 0;

            return Convert.ToDecimal(value.ToString());
        }
        
        //
        public static decimal toDecimalMod100(this int? value){

            if (value == null) return 0;

            decimal valorRetorno = decimal.Divide(new decimal(value.toInt()), new decimal(100));

            return valorRetorno;

        }

        //
        public static decimal toDecimalMod100(this int value){

            decimal valorRetorno = decimal.Divide(new decimal(value), new decimal(100));

            return valorRetorno;

        }

        //
        public static byte toByte(this byte? value){

            if (value == null) return 0;

            byte retorno;

            Byte.TryParse(value.ToString(), out retorno);

            return retorno;
        }

        //
        public static byte toByte(this int value){

            return Convert.ToByte(value.ToString());
        }
        
        //Extensao para facilitar a conversao de int nullable
        public static short toShort(this short? value){

            if (value == null) return 0;

            short retorno;

            Int16.TryParse(value.ToString(), out retorno);

            return retorno;
        }

        
        //Extensao para facilitar a conversao de int nullable
        public static int toInt(this string value){

            if (value.isEmpty()) return 0;


            return UtilNumber.toInt32(value);
        }
        
        //Extensao para facilitar a conversao de int nullable
        public static int toInt(this object value){

            if (value.isEmpty()) return 0;

            return UtilNumber.toInt32(value);
        }
        
        //
        public static decimal toPorcentagem(this decimal valor, decimal valorTotal){

            if (valorTotal == 0){
                return 0;
            }

            decimal porcentagem = (valor / valorTotal) * 100;

            return Math.Round(porcentagem, 2);
        }
        
        //
        public static decimal toPorcentagem(this int valor, int valorTotal){

            if (valorTotal == 0) {
                return 0;
            }

            decimal valorDecimal = new Decimal(valor);
            decimal valorTotalDecimal = new Decimal(valorTotal);
            
            decimal porcentagem = Decimal.Divide(valorDecimal, valorTotalDecimal) * 100;

            return Math.Round(porcentagem, 2);
        }
        
        //
        public static string toDecimalPoint(this object value, string qtdeCasas = "2"){
            
            if (value == null) return "0";
            
            return value.toDecimal().ToString($"F{qtdeCasas}", CultureInfo.InvariantCulture);
        }
                
        

    }
}
