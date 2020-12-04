using System;
using System.Web.Mvc;
using WEB.App_Infrastructure;
using BLL.SegmentosAtuacao;
using DAL.SegmentosAtuacao;
using MvcFlashMessages;
using WEB.Areas.SegmentosAtuacao.ViewModels;

namespace WEB.Areas.SegmentosAtuacao.Controllers {

    public class SegmentoAtuacaoCadastroController : BaseSistemaController {

        //Constantes

        //Atributos
        private ISegmentoAtuacaoCadastroBL _ISegmentoAtuacaoCadastroBL;
        private ISegmentoAtuacaoConsultaBL _ISegmentoAtuacaoConsultaBL;
        
        //Propriedades
        private ISegmentoAtuacaoCadastroBL OSegmentoAtuacaoCadastroBL => _ISegmentoAtuacaoCadastroBL = _ISegmentoAtuacaoCadastroBL ?? new SegmentoAtuacaoCadastroBL();
        private ISegmentoAtuacaoConsultaBL OSegmentoAtuacaoConsultaBL => _ISegmentoAtuacaoConsultaBL = _ISegmentoAtuacaoConsultaBL ?? new SegmentoAtuacaoConsultaBL();
        
        // GET: EventosPro/EventoCadastro
        [ActionName("modal-form-cadastro")]
        public ActionResult modalFormCadastro(int? id) {
			
            var ViewModel = new SegmentoAtuacaoForm();

            ViewModel.OSegmentoAtuacao = this.OSegmentoAtuacaoConsultaBL.carregar(id.toInt()) ?? new SegmentoAtuacao();
			
            return View(ViewModel);
        }
		
        //POST
        [HttpPost, ActionName("salvar-segmento")]
        public ActionResult salvarEvento(SegmentoAtuacaoForm ViewModel) {

            if(!ModelState.IsValid) {
                return PartialView("modal-form-cadastro", ViewModel);
            }
            
            bool flagSucesso = this.OSegmentoAtuacaoCadastroBL.salvar(ViewModel.OSegmentoAtuacao);

            if (flagSucesso) {

                //this.Flash(UtilMessage.TYPE_MESSAGE_SUCCESS, "O processo de avaliação foi salvo com sucesso.");

                return Json(new { error = false, message = "Segmento atuação cadastrado com sucesso", ViewModel.OSegmentoAtuacao.id, ViewModel.OSegmentoAtuacao.descricao });
            }

            this.Flash(UtilMessage.TYPE_MESSAGE_ERROR, "Houver algum problema ao tentar salvar os dados do segmento.");

            return PartialView("modal-form-cadastro", ViewModel);
        }

    }
}
