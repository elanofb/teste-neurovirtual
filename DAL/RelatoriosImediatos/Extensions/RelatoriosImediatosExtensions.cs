using System;
using System.Linq;
using DAL.Arquivos;
using DAL.Produtos;
using System.IO;

namespace DAL.RelatoriosImediatos {

    public static class RelatoriosImediatosExtensions{
		
		//
        public static string displayNomeParametro(this ParametroInfo OParam) {

            if (OParam == null) {
                return "";

            }
            string nomeParam = OParam.nomeParametro;

            nomeParam = nomeParam.Replace("@", "");
            nomeParam = nomeParam.Replace("_", " ");
            nomeParam = nomeParam.Replace("dt", "Data ");
            nomeParam = nomeParam.Replace("Dt", "Data ");

			return nomeParam;
        }

		//
        public static string inputName(this ParametroInfo OParam) {

            if (OParam == null) {
                return "param";

            }
            string nomeParam = OParam.nomeParametro;

            nomeParam = nomeParam.Replace("@", "");

			return nomeParam;
        }

		//
        public static string inputType(this ParametroInfo OParam) {

            if (OParam == null) {
                return "text";

            }

            string tipoDado = OParam.tipoDado;

            if (tipoDado == "int" || tipoDado == "tinyint" || tipoDado == "smallint") {
                return "number";
            }

            if (tipoDado == "datetime" || tipoDado == "date" || tipoDado == "smalldatetime") {
                return "text";
            }

			return "text";
        }

		//
        public static bool isDate(this ParametroInfo OParam) {

            if (OParam == null) {
                return false;

            }

            string tipoDado = OParam.tipoDado;

            if (tipoDado == "datetime" || tipoDado == "date" || tipoDado == "smalldatetime") {
                return true;
            }

			return false;
        }

		//
        public static string cssSize(this ParametroInfo OParam) {

            if (OParam == null) {
                return "col-sm-4 col-md-3 col-lg-2";

            }

            string tipoDado = OParam.tipoDado;

            if (tipoDado == "datetime" || tipoDado == "date" || tipoDado == "smalldatetime") {
                return "col-xs-3 col-sm-3 col-md-2 col-lg-2";
            }

            if (tipoDado == "int" || tipoDado == "decimal" || OParam.tamanhoDado <= 20) {
                return "col-xs-3 col-sm-3 col-md-2 col-lg-2";
            }

			return "col-sm-4 col-md-3 col-lg-2";
        }

		//
        public static string inputMask(this ParametroInfo OParam) {

            if (OParam == null) {
                return "";

            }

            string tipoDado = OParam.tipoDado;

            if (tipoDado == "decimal" || tipoDado == "money" || tipoDado == "float") {
                return "decimal";
            }

            if (tipoDado == "datetime" || tipoDado == "date" || tipoDado == "smalldatetime") {
                return "date";
            }

			return "";
        }
	}
}
