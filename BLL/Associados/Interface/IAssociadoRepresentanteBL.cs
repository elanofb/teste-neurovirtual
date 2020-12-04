using System;
using System.Linq;
using DAL.Associados;

namespace BLL.Associados {

	public interface IAssociadoRepresentanteBL{
		AssociadoRepresentante carregar(int id);
		IQueryable<AssociadoRepresentante> listar(int idAssociado, string ativo);
		bool salvar(AssociadoRepresentante OAssociadoRepresentante);
		bool existe(AssociadoRepresentante OAssociadoRepresentante, int idDesconsiderar);
		UtilRetorno excluir(string cpf, int idAssociado);
	    UtilRetorno excluirPorId(int id);
	}
}