using DAL.ContasBancarias;
using DAL.Financeiro.Procedures;

namespace System {

	public static class ContaBancariaExtensions {

	    //
	    public static string exibirDescricaoConta(this ContaBancaria OContaBancaria) {
	        
            var descricao = "";

            if (OContaBancaria == null || OContaBancaria.id == 0) {
                return descricao;
            }

            return OContaBancaria.descricao;

	    }

		//
		public static string exibirColorSaldo(this SpContaBancariaSaldoAtual OSaldo) {
			string color = "";

			if (OSaldo.saldoAtual >= 0) {
                color = "text-green";
			} else {
                color = "text-red";
			}

			return color;
		}

    }

}