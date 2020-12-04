
namespace DAL.AssociadosCarteirinha {

	public static class AssociadoCarteirinhaExtension {
        
        // Tipos de Envio
		public static readonly string CORREIOS = "C";
		public static readonly string PESSOALMENTE = "P";

        // Tipos de Emissão
        public static readonly string EMISSAO = "EM";
        public static readonly string RENOVACAO = "RN";
        public static readonly string SEGUNDA_VIA = "SV";

		public static string flagTipoEnvio(this AssociadoCarteirinha OAssociadoCarteirinha) {

			if (OAssociadoCarteirinha.flagTipoEnvio.Equals(CORREIOS)) {
                return "Correios";
            }

            if (OAssociadoCarteirinha.flagTipoEnvio.Equals(PESSOALMENTE)) {
                return "Pessoalmente";
            }

            return "";

		}

        public static string flagTipoEmissao(this AssociadoCarteirinha OAssociadoCarteirinha) {

			if (OAssociadoCarteirinha.flagTipoEmissao.Equals(EMISSAO)) {
                return "Emissão";
            }

            if (OAssociadoCarteirinha.flagTipoEmissao.Equals(RENOVACAO)) {
                return "Renovação";
            }

            if (OAssociadoCarteirinha.flagTipoEmissao.Equals(SEGUNDA_VIA)) {
                return "2ª Via";
            }

            return "";

		}

	}

}
