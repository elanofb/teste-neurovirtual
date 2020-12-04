using System;

namespace BLL.Fornecedores {

	public class FornecedorExclusaoBL : FornecedorConsultaBL, IFornecedorExclusaoBL{

		//Atributos

		//Propriedades

		//Construtor
		public FornecedorExclusaoBL() {
		}

		//Exclusao logica
		public UtilRetorno excluir(int id) {

			var OFornecedor = this.carregar(id);

			if (OFornecedor == null) {
				return UtilRetorno.newInstance(true, "O registro informado não foi localizado.");
			}

			OFornecedor.flagExcluido = true;

			OFornecedor.dtAlteracao = DateTime.Now;

			db.SaveChanges();

			return UtilRetorno.newInstance(false, "O registro foi removido com sucesso.");
		}
	}
}