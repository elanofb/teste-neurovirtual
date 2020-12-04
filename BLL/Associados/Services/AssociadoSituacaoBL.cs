using System;
using BLL.Services;

namespace BLL.Associados {

	public class AssociadoSituacaoBL : DefaultBL, IAssociadoSituacaoBL {

		//Constantes
		public static readonly string flagSituacaoAdimplente = "AD";
		public static readonly string flagSituacaoInadimplente = "IN";
		public static readonly string flagSituacaoIsento = "IS";

		//Atributos

		//Propriedades


		public bool desativar(int idAssociado) {
			throw new NotImplementedException();
		}

		public bool admitir(int idAssociado) {
			throw new NotImplementedException();
		}
	}
}