using System;
using System.Web.Mvc;
using BLL.RedeAfiliados.Services;
using WEB.App_Infrastructure;
using WEB.Areas.RedeAfiliados.Models.ViewModels;

namespace WEB.Areas.RedeAfiliados.Controllers {

    public class PontuacaoMembroController : BaseSistemaController {

        //Atributos
        private IRedePontuacaoConsultaBL _PontoConsulta;

        //Servicos
        private IRedePontuacaoConsultaBL PontoConsultaBL => _PontoConsulta = _PontoConsulta ?? new RedePontuacaoConsultaBL();

        public PontuacaoMembroController() {
            
        }
        
        // GET
        [ActionName("partial-pontuacao-membro")]
        public ActionResult partialPontuacaoMembro(int? idMembro) {

            var ViewModel = new PontuacaoMembroVM();

            if (idMembro.toInt() == 0) {
                
                return PartialView(ViewModel);
                
            }

            ViewModel.Pontuacao = PontoConsultaBL.carregarPorMembro(idMembro.toInt());
            
            return PartialView(ViewModel);
        }
    }

}
