using System.Web.Mvc;

namespace WEB.Areas.AssociadosCarteirinha.Helpers{

	/**
	 * 
	 */
    public class AssociadoCarteirinhaHelper {

        // Tipos de Envio
		public static readonly string CORREIOS = "C";
		public static readonly string PESSOALMENTE = "P";

        // Tipos de Emissão
        public static readonly string EMISSAO = "EM";
        public static readonly string RENOVACAO = "RN";
        public static readonly string SEGUNDA_VIA = "SV";

        //
        public static SelectList selectListTipoEnvio(string selected){

            var list = new[] { 
                    new { value = CORREIOS, text = "Correios" },
                    new { value = PESSOALMENTE, text = "Pessoalmente" },
            };

            return new SelectList(list, "value", "text", selected);

        }

        //
        public static string getTipoEnvio(string flagTipoEnvio) {

            if (flagTipoEnvio.Equals(CORREIOS)) {
                return "Correios";
            }

            if (flagTipoEnvio.Equals(PESSOALMENTE)) {
                return "Pessoalmente";
            }

            return "";
            
        }

        //
        public static SelectList selectListTipoEmissao(string selected){

            var list = new[] { 
                    new { value = EMISSAO, text = "Emissão" },
                    new { value = RENOVACAO, text = "Renovação" },
                    new { value = SEGUNDA_VIA, text = "2ª Via" },
            };

            return new SelectList(list, "value", "text", selected);

        }

        //
        public static string getTipoEmissao(string flagTipoEmissao) {

            if (flagTipoEmissao.Equals(EMISSAO)) {
                return "Emissão";
            }

            if (flagTipoEmissao.Equals(RENOVACAO)) {
                return "Renovação";
            }

            if (flagTipoEmissao.Equals(SEGUNDA_VIA)) {
                return "2ª Via";
            }

            return "";
            
        }

    }
}