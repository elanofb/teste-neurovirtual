using System;
using System.Linq;
using System.Web.Mvc;
using BLL.Pessoas;
using WEB.App_Infrastructure;
using System.Json;
using BLL.Arquivos;
using DAL.Entities;
using PagedList;
using WebGrease.Css.Extensions;

namespace WEB.Areas.Pessoas.Controllers{

    public class PessoaRelacionamentoController : BaseSistemaController{

	    //Atributos
	    private IPessoaRelacionamentoBL _PessoaRelacionamentoBL; 
	    private IArquivoUploadBL _ArquivoUploadBL; 

		//Propriedades
	    private IPessoaRelacionamentoBL OPessoaRelacionamentoBL => (this._PessoaRelacionamentoBL = this._PessoaRelacionamentoBL ?? new PessoaRelacionamentoBL());
	    private IArquivoUploadBL OArquivoUploadBL => (this._ArquivoUploadBL = this._ArquivoUploadBL ?? new ArquivoUploadBL());

		//Bloco Partial para histórico de relacionamentos
		[HttpGet, ActionName("partial-listar-relacionamentos")]
        public PartialViewResult partialListarRelacionamentos(int idPessoa) {

		    ViewBag.idPessoa = idPessoa;

			var listaOcorrencias = this.OPessoaRelacionamentoBL.listar(idPessoa, 0);

			var valorBusca = UtilRequest.getString("valorBusca");
			var flagTemArquivos = UtilRequest.getString("flagTemArquivos");
		    
			if (!valorBusca.isEmpty()) {
				listaOcorrencias = listaOcorrencias.Where(x => x.observacao.Contains(valorBusca) || x.OcorrenciaRelacionamento.descricao.Contains(valorBusca) || x.UsuarioCadastro.nome.Contains(valorBusca));
			}

			if (flagTemArquivos == "S") {
			    
				var ids = listaOcorrencias.Select(x => x.id).ToList();
				var idsComArquivo = OArquivoUploadBL.listarDocumentos(0, EntityTypes.PESSOADOCUMENTO_RELACIONAMENTO).Where(x => ids.Contains(x.idReferenciaEntidade)).Select(x => x.idReferenciaEntidade).ToList();

				listaOcorrencias = listaOcorrencias.Where(x => idsComArquivo.Contains(x.id));
			}

			var lista = listaOcorrencias.OrderByDescending(x => x.dtOcorrencia).ToPagedList(UtilRequest.getNroPagina(), UtilRequest.getNroRegistros());

			if (flagTemArquivos == "S"){
				
				lista.ForEach(item => { item.flagPossuiArquivo = true; });
				return PartialView(lista);
			}

			//Exponho as ocorrências que possuem arquivos.
			var idsOcorrencia = lista.Select(x => x.id).ToList();
			var idsOcorrenciaComArquivo = OArquivoUploadBL.listarDocumentos(0, EntityTypes.PESSOADOCUMENTO_RELACIONAMENTO).Where(x => idsOcorrencia.Contains(x.idReferenciaEntidade)).Select(x => x.idReferenciaEntidade).ToList();

			lista.ForEach(item => {
				item.flagPossuiArquivo = idsOcorrenciaComArquivo.Exists(x => x == item.id); 
			});

			return PartialView(lista);
        }

        //Excluir determinado registro
        [HttpPost]
        public ActionResult excluir(int[] id) {
			JsonMessage Retorno = new JsonMessage();
			Retorno.error = false;

			foreach (int idExclusao in id) { 
				UtilRetorno RetornoExclusao = this.OPessoaRelacionamentoBL.excluir(idExclusao);
				
				if (RetornoExclusao.flagError) { 
					Retorno.error = false;
				}
			}

            Retorno.message = "O(s) registro(s) foi removido com sucesso.";

            return Json(Retorno);
        }
    }
}
