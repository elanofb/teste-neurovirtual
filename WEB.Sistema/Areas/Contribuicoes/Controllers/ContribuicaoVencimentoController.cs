using System;
using System.Collections.Generic;
using System.Json;
using System.Linq;
using System.Web.Mvc;
using BLL.AssociadosContribuicoes;
using BLL.Contribuicoes;
using DAL.Contribuicoes;
using WEB.Areas.Contribuicoes.ViewModels;

namespace WEB.Areas.Contribuicoes.Controllers {

	public class ContribuicaoVencimentoController : Controller {

		//Constantes

		//Atributos
        private IPeriodoContribuicaoBL _PeriodoContribuicaoBL; 
		private IContribuicaoBL _ContribuicaoBL; 
        private IAssociadoContribuicaoVencimentoBL _AssociadoContribuicaoVencimentoBL; 

		//Propriedades
	    private IPeriodoContribuicaoBL OPeriodoContribuicaoBL => this._PeriodoContribuicaoBL = this._PeriodoContribuicaoBL ?? new PeriodoContribuicaoBL();
		private IContribuicaoBL OContribuicaoBL => this._ContribuicaoBL = this._ContribuicaoBL ?? new ContribuicaoPadraoBL();
		private IAssociadoContribuicaoVencimentoBL OAssociadoContribuicaoVencimentoBL => this._AssociadoContribuicaoVencimentoBL = this._AssociadoContribuicaoVencimentoBL ?? new AssociadoContribuicaoVencimentoBL();

	    // GET: Contribuicoes/Default/listar
		[HttpGet, ActionName("partial-box-vencimentos")]
		public PartialViewResult partialBoxVencimentos(int? idPeriodoContribuicao) {

		    var ViewModel = new ContribuicaoPadraoForm();

            ViewModel.Contribuicao.listaContribuicaoVencimento = new List<ContribuicaoVencimento>();

		    var OPeriodoContribuicao = this.OPeriodoContribuicaoBL.carregar( UtilNumber.toInt32(idPeriodoContribuicao) );

		    if (OPeriodoContribuicao == null) {

		        return PartialView(ViewModel);
		    }

		    int limite = 12/OPeriodoContribuicao.qtdeMeses;
            int mesProximo = 1;
		    int diaInicial = 1;

		    for (int i = 0; i < limite; i++) {

                var OContribuicao = new ContribuicaoVencimento();

		        OContribuicao.diaVencimento = (byte?) diaInicial;

                OContribuicao.mesVencimento = (byte?) mesProximo;

		        ViewModel.Contribuicao.listaContribuicaoVencimento.Add(OContribuicao);

		        mesProximo = mesProximo + OPeriodoContribuicao.qtdeMeses;
		    }

		    return PartialView(ViewModel);
		}



	    // POST: Contribuicoes/Default/listar
		[HttpPost, ActionName("buscar-vencimentos")]
		public ActionResult buscarVencimentos(int? idContribuicao, int? idAssociado) {

			Contribuicao OContribuicao = this.OContribuicaoBL.carregar(UtilNumber.toInt32(idContribuicao));

			if (OContribuicao == null) { 
				return Json(new JsonMessage{ error = true, message="Esse método precisa do código da contribuicao para calcular o valor."}, JsonRequestBehavior.AllowGet);
			}

		    var listaVencimentos = OContribuicao.listaContribuicaoVencimento.Where(x => x.dtExclusao == null)
                                                                            .ToList()
                                                                            .Select(x => string.Concat(x.diaVencimento.ToString().PadLeft(2, '0'), "/", x.mesVencimento.ToString().PadLeft(2, '0')))
                                                                            .ToList();

		    var OVencimento = this.OAssociadoContribuicaoVencimentoBL.retornarProximoVencimento(OContribuicao, UtilNumber.toInt32(idAssociado)) ?? new ContribuicaoVencimento();


			return Json(new {	error = false, 
                                listaVencimentos,
                                qtdeMeses = OContribuicao.PeriodoContribuicao?.qtdeMeses,
                                dtVencimento = OVencimento.dtVencimento.exibirData(),
                                flagVencimentoFixo = (listaVencimentos.Count > 0)
							}, 
							JsonRequestBehavior.AllowGet);
		}


	    // POST: Contribuicoes/ContribuicaoVencimento
		[HttpPost, ActionName("validar-vencimento")]
		public ActionResult validarVencimento(int? idContribuicao, DateTime? dtVencimento) {

			Contribuicao OContribuicao = this.OContribuicaoBL.carregar(UtilNumber.toInt32(idContribuicao));

			if (OContribuicao == null) { 
				return Json(new JsonMessage{ error = true, message="Não foi possível encontrar a cobrança."}, JsonRequestBehavior.AllowGet);
			}


			if (!dtVencimento.HasValue) { 
				return Json(new JsonMessage{ error = true, message="Data de Vencimento não informada."}, JsonRequestBehavior.AllowGet);
			}


		    if (OContribuicao.flagVencimentoVariado()) {		        
                return Json(new { error = false, dtVencimento = "", dtInicioVigencia = "", dtFimVigencia = "" }, JsonRequestBehavior.AllowGet);		            
		    }

		    var OVencimento = OContribuicao.retornarProximoVencimento(dtVencimento.Value);

			if (OVencimento == null) { 
				return Json(new JsonMessage{ error = true, message="A data de vencimento informada é inválida."}, JsonRequestBehavior.AllowGet);
			}

		    return Json(new {
                            error = false, 
                            dtVencimento = dtVencimento.exibirData().ToString(),
                            dtInicioVigencia = OVencimento.dtInicioVigencia.exibirData().ToString(),
                            dtFimVigencia = OVencimento.dtFimVigencia.exibirData().ToString()
							}, 
							JsonRequestBehavior.AllowGet);
		}
	}
}
