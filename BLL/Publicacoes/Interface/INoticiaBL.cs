using System.Linq;
using DAL.Pedidos;
using System.Json;
using DAL.Publicacoes;
using System.Web;
using DAL.Arquivos;
using System.Collections.Generic;

namespace BLL.Publicacoes {

	public interface INoticiaBL {

	    IQueryable<Noticia> query(int? idOrganizacaoParam = null);

		IQueryable<Noticia> listar(string valorBusca = "", string ativo = "S", int tipoNoticia = 0, bool? flagImagemAtiva = false, int? idPortal = 0);

		IQueryable<Noticia> listarComunicados(string valorBusca = "", string ativo = "S");

		Noticia carregar(int id);

		NoticiaDTO principalNoticia(int id = 0);

		IQueryable<NoticiaDTO> principalNoticiaFotos(int id);

		IQueryable<NoticiaDTO> listarPortal(int id = 0);

		IQueryable<NoticiaDTO> buscar(string valorBusca);

		bool salvar(Noticia ONoticia, HttpPostedFileBase OArquivo, HttpPostedFileBase OArquivoPDF = null);

        JsonMessageStatus alterarStatus(int id);

		bool excluir(int[] ids);

		bool existe(string descricao, int id);

		bool existeUrl(string titulo, int id, int idTipoNoticia);
	}
}
