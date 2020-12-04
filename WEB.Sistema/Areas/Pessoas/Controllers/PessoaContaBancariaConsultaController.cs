using System.Web.Mvc;
using WEB.App_Infrastructure;
using WEB.Areas.Pessoas.ViewModels;

// ReSharper disable Mvc.ViewNotResolved
// ReSharper disable Mvc.PartialViewNotResolved
namespace WEB.Areas.Pessoas.Controllers {
    
    public class PessoaContaBancariaConsultaController : BaseSistemaController {
      
        //        
        public PartialViewResult index(int idPessoa) {
            
            var ViewModel = new PessoaContaBancariaConsultaVM();
            
            ViewModel.carregarContas(idPessoa);                       
            
            return PartialView(ViewModel.RetornoAPI);           

        }
       

    }
}