using System;
using System.Linq;
using System.Web.Mvc;
using BLL.Associados;
using BLL.AssociadosContribuicoes;
using BLL.Contribuicoes;
using DAL.Associados;
using DAL.Contribuicoes;
using DAL.Permissao.Security.Extensions;
using MvcFlashMessages;
using WEB.App_Infrastructure;

namespace WEB.Areas.AssociadosContribuicoes.Controllers{

    public class AssociadoContribuicaoPadraoController : BaseSistemaController{

		//Atributos
        private IAssociadoBL _AssociadoBL;
        private IAssociadoAlteracaoBL _AssociadoAlteracaoBL;
        private IContribuicaoBL _ContribuicaoBL;

		//Propriedades
        private IAssociadoBL OAssociadoBL => _AssociadoBL = _AssociadoBL ?? new AssociadoBL();
        private IAssociadoAlteracaoBL OAssociadoAlteracaoBL => _AssociadoAlteracaoBL = _AssociadoAlteracaoBL ?? new AssociadoAlteracaoBL();
        private IContribuicaoBL OContribuicaoBL => _ContribuicaoBL = _ContribuicaoBL ?? new ContribuicaoPadraoBL();

		//Bloco Partial para listagem de anuidades de um associado
		[HttpGet, ActionName("modal-contribuicao-padrao")]
        public PartialViewResult modalContribuicaoPadrao(int idAssociado) {

		    var OAssociado = this.OAssociadoBL.carregar(idAssociado) ?? new Associado();

            return PartialView(OAssociado);
        }

		//Formulario submetido para novo cargo para o associado
		[HttpPost, ActionName("salvar-contribuicao-padrao")]
		public ActionResult salvarContribuicaoPadrao(int id, int? idContribuicaoPadrao) {

		    int idContribuicao = UtilNumber.toInt32(idContribuicaoPadrao);

		    if (idContribuicao > 0) {

                var OAssociado = this.OAssociadoBL.carregar(id) ?? new Associado();

		        var OContribuicao = this.OContribuicaoBL.carregar(idContribuicao);

		        var TabelaPreco = OContribuicao.retornarTabelaVigente();

		        var Preco = TabelaPreco.retornarPreco(UtilNumber.toInt32(OAssociado.idTipoAssociado));

		        if (Preco.id == 0) {
		            
                    this.Flash(UtilMessage.TYPE_MESSAGE_ERROR, "Não existe preço configurado para esse tipo de associado.");

                    return PartialView("modal-contribuicao-padrao", OAssociado);
		        }

		    }

		    var Retorno = this.OAssociadoAlteracaoBL.alterarDados(id, "idContribuicaoPadrao", idContribuicaoPadrao, User.id());

			return Json(new{ error = Retorno.flagError, message = Retorno.listaErros.FirstOrDefault() });
		}


    }
}
