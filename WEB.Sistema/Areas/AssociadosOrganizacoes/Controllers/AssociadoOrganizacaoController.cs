using System.Web.Mvc;
using DAL.Permissao.Security.Extensions;
using WEB.Areas.AssociadosOrganizacoes.ViewModels;

namespace WEB.Areas.AssociadosOrganizacoes.Controllers {

    public class AssociadoOrganizacaoController : Controller {
        
        //
        [ActionName("partial-informacoes-adicionais")]
        public PartialViewResult partialInformacoesAdicionais(int id) {

            var ViewModel = new AssociadoOrganizacaoVM();

            if (User.idOrganizacao() == 0) {
                return PartialView(ViewModel);    
            }

            ViewModel.idAssociado = id;
            
            ViewModel.carregarInformacoes();

            return PartialView(ViewModel);

        }

    }
    
}
