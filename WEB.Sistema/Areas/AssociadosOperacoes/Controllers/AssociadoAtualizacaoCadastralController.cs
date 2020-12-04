using System;
using System.Linq;
using System.Web.Mvc;
using BLL.Associados;
using BLL.Notificacoes.Interface;
using BLL.Services;
using DAL.Associados;
using DAL.Permissao.Security.Extensions;
using WEB.Areas.AssociadosOperacoes.ViewModels;

namespace WEB.Areas.AssociadosOperacoes.Controllers {

	public class AssociadoAtualizacaoCadastralController : Controller {

		//Atributos
        private IAssociadoBL _IAssociadoBL;
		
		//Propriedades
        private IAssociadoBL OAssociadoBL => _IAssociadoBL = _IAssociadoBL ?? new AssociadoBL();


		//Dependencias
		private IGeradorNotificacaoBL GeradorNotificacaoBL{ get; } 

		/// <summary>
		/// 
		/// </summary>
		public AssociadoAtualizacaoCadastralController(IGeradorNotificacaoBL _GeradorNotificacaoBL){

			GeradorNotificacaoBL = _GeradorNotificacaoBL;
		}

		
	    //
	    [HttpPost, ActionName("enviar-link-alteracao")]
	    public ActionResult enviarLinkAlteracao(AssociadoFiltroVM DadosConsulta) {		    	        
		    
	        var listaAssociados = DadosConsulta.montarQuery().Where(x => x.ativo == "S")
	                                                 .Select(x => new {
	                                                     x.id			        									 			        								     
	                                                 }).ToList();		    
            if (!listaAssociados.Any()) {

                return Json(new { error = true, message = "Nenhum associado ativo foi encontrado para envio do link de atualização cadastral." }, JsonRequestBehavior.AllowGet);

            }
			
		    var idsAssociados = listaAssociados.Select(x => x.id).ToList();
					    
		    var listaAssociadosNotificacao = this.OAssociadoBL.query(User.idOrganizacao())
			    .Select(x => new{
				    x.id, 
				    x.idOrganizacao,
				    x.idTipoAssociado,
				    TipoAssociado = new { x.TipoAssociado.descricao },
				    Pessoa = new { x.Pessoa.nome, listaEmails = x.Pessoa.listaEmails.Where(l => l.dtExclusao == null).Select(l => new { l.email }).ToList() }
			    })
			    .Where(x => idsAssociados.Contains(x.id))
			    .ToListJsonObject<Associado>();
			
		    ListaAssociadosDTO OListaAssociadosDTO = new ListaAssociadosDTO();
			
		    OListaAssociadosDTO.listaAssociados = listaAssociadosNotificacao;

		    UtilRetorno ORetorno = this.GeradorNotificacaoBL.gerarNotificacao(OListaAssociadosDTO);
				
		    if (ORetorno.flagError){
			    
			    return Json(new { error = true, message = string.Join("<br>", ORetorno.listaErros) }, JsonRequestBehavior.AllowGet);
			    
		    }
		    
		    return Json(new { error = false, message = "Notificação criada com sucesso!" }, JsonRequestBehavior.AllowGet);

	    }			    
        
	}

}
