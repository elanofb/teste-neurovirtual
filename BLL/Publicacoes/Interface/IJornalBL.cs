using System.Linq;
using DAL.Publicacoes;
using System.Web;
using DAL.Arquivos;
using System.Collections.Generic;
using System.Json;

namespace BLL.Publicacoes {
	public interface IJornalBL {

	    IQueryable<Jornal> query(int? idOrganizacaoParam = null);

		IQueryable<Jornal> listar(string valorBusca = "", string ativo = "S", int idTipoNoticia = 0, bool? flagImagemAtiva = false, int? idPortal = 0, int? idOrganizacaoParam = null);

		IQueryable<Jornal> listarComunicados(string valorBusca = "", string ativo = "S");

		Jornal carregar(int id);

		JornalDTO principalJornal(int id = 0);

		IQueryable<JornalDTO> principalJornalFotos(int id);

		IQueryable<JornalDTO> listarPortal(int id = 0);

		IQueryable<JornalDTO> buscar(string valorBusca);

		bool salvar(Jornal OJornal, HttpPostedFileBase[] arrayArquivos);

		bool excluir(int[] ids);

        JsonMessageStatus alterarStatus(int id);

		bool existe(string descricao, int id);

	}
}
