using BLL.Associados;
using PagedList;
using System;
using System.Linq;
using System.Web.Mvc;
using WEB.Areas.AssociadosConsultas.ViewModels;

namespace WEB.Areas.AssociadosConsultas.Controllers
{
    public class PendenciaCadastralController : Controller
    {
        // Atributos
        private IPendenciaCadastralBL _PendenciaCadastralBL;

        // Propriedades
        private IPendenciaCadastralBL OPendenciaCadastralBL => _PendenciaCadastralBL = _PendenciaCadastralBL ?? new PendenciaCadastralBL();

        // Listagem dos associados do sistema
        public ActionResult index(PendenciaCadastralConsulta ViewModel) {

            string valorBusca = UtilRequest.getString("valorBusca");

            ViewModel.listaPendenciaCadastral = OPendenciaCadastralBL.listar(ViewModel.idsTipoAssociado, ViewModel.qtdEmails, ViewModel.qtdTel, ViewModel.qtdEnderecos, ViewModel.flagSituacaoContribuicao, ViewModel.ativo, valorBusca).OrderBy(x => x.nome).ToPagedList(UtilRequest.getNroPagina(), UtilRequest.getNroRegistros());

            return View("index", ViewModel);
        }
    }
}