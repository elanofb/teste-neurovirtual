using System;
using System.Linq;
using System.Web.Mvc;
using BLL.Associados;
using BLL.AssociadosContribuicoes;
using BLL.AssociadosDependentes;
using BLL.Contribuicoes;
using DAL.AssociadosContribuicoes;
using DAL.Contribuicoes;
using MvcFlashMessages;
using WEB.App_Infrastructure;
using WEB.Areas.AssociadosContribuicoes.ViewModels;

namespace WEB.Areas.AssociadosContribuicoes.Controllers {

    public class AssociadoContribuicaoController : BaseSistemaController {

        //Atributos
        private IAssociadoContribuicaoBL _AssociadoContribuicaoBL;
        private IContribuicaoBL _ContribuicaoBL;
        private IAssociadoBL _AssociadoBL;
        private IAssociadoDependenteBL _AssociadoDependenteBL;

        //Propriedades
        private IAssociadoContribuicaoBL OAssociadoContribuicaoBL => this._AssociadoContribuicaoBL = this._AssociadoContribuicaoBL ?? new AssociadoContribuicaoBL();
        private IContribuicaoBL OContribuicaoBL => this._ContribuicaoBL = this._ContribuicaoBL ?? new ContribuicaoPadraoBL();
        private IAssociadoBL OAssociadoBL => this._AssociadoBL = this._AssociadoBL ?? new AssociadoBL();
        private IAssociadoDependenteBL OAssociadoDependenteBL => this._AssociadoDependenteBL = this._AssociadoDependenteBL ?? new AssociadoDependenteBL();

        //Bloco Partial para listagem de anuidades de um associado
        [HttpGet, ActionName("partial-listar-cobrancas")]
        public PartialViewResult partialListarCobrancas(int idAssociado) {

            var OAssociado = this.OAssociadoBL.carregar(idAssociado);

            var ViewModel = new AssociadoContribuicaoPartialLista();

            ViewModel.carregarContribuicoes(OAssociado);

            ViewModel.carregarInscricoes(OAssociado);

            ViewModel.qtdeCobrancas = ViewModel.listaContribuicoes.Count;

            if (ViewModel.TaxaInscricao.id > 0) {

                ViewModel.qtdeCobrancas++;
            }

            return PartialView(ViewModel);
        }

        //Formulário Parcial para nova anuidade do associado
        [HttpGet, ActionName("partial-form-editar")]
        public PartialViewResult partialFormEditar(int? id, int? idAssociado) {

            var OAssociadoContribuicao = this.OAssociadoContribuicaoBL.carregar(UtilNumber.toInt32(id)) ?? new AssociadoContribuicao();

            OAssociadoContribuicao.idAssociado = (OAssociadoContribuicao.idAssociado > 0 ? OAssociadoContribuicao.idAssociado : UtilNumber.toInt32(idAssociado));

            AssociadoContribuicaoPartialForm ViewModel = new AssociadoContribuicaoPartialForm();

            ViewModel.AssociadoContribuicao = OAssociadoContribuicao;

            return PartialView(ViewModel);
        }


        //Formulario submetido para novo cargo para o associado
        [HttpPost]
        public ActionResult salvar(AssociadoContribuicaoPartialForm ViewModel) {

            DateTime? dtVencimentoAtual = UtilRequest.getDateTime("dtVencimentoAtual");

            if (!ModelState.IsValid) {

                return PartialView("partial-form-editar", ViewModel);
            }

            var OAssociado = this.OAssociadoBL.carregar(ViewModel.AssociadoContribuicao.idAssociado);

            var OContribuicao = this.OContribuicaoBL.carregar(ViewModel.AssociadoContribuicao.idContribuicao);

            var TabelaPreco = OContribuicao.retornarTabelaVigente();

            var Preco = TabelaPreco.retornarPreco(UtilNumber.toInt32(OAssociado.idTipoAssociado));

            if (TabelaPreco.id == 0 || Preco.id == 0) {

                this.Flash(UtilMessage.TYPE_MESSAGE_ERROR, "Essa cobrança não está configurada para esse tipo de associado.");

                return PartialView("partial-form-editar", ViewModel);
            }


            if (OContribuicao.dtValidade.HasValue && OContribuicao.dtValidade < ViewModel.AssociadoContribuicao.dtVencimentoOriginal) {

                this.Flash(UtilMessage.TYPE_MESSAGE_ERROR, $"Esse plano de cobrança é válido somente até {OContribuicao.dtValidade.exibirData()}.");

                return PartialView("partial-form-editar", ViewModel);

            }
            
            if (OContribuicao.idTipoVencimento == TipoVencimentoConst.VENCIMENTO_PELA_ADMISSAO_ASSOCIADO &&
                (OAssociado.dtAdmissao?.Day != ViewModel.AssociadoContribuicao.dtVencimentoOriginal.Day || 
                OAssociado.dtAdmissao?.Month != ViewModel.AssociadoContribuicao.dtVencimentoOriginal.Month)) {

                this.Flash(UtilMessage.TYPE_MESSAGE_ERROR, "A data de vencimento deve ter o dia e o mês iguais à data de admissão do associado.");

                return PartialView("partial-form-editar", ViewModel);

            }
            
            var dtVencimentoOriginal = ViewModel.AssociadoContribuicao.dtVencimentoOriginal;

            dtVencimentoAtual = dtVencimentoAtual ?? dtVencimentoOriginal;

            var OContribuicaoGeracaoBL = new AssociadoContribuicaoGeracaoBL();

            var Retorno = OContribuicaoGeracaoBL.gerarCobranca(OAssociado, OContribuicao, dtVencimentoOriginal, dtVencimentoAtual, false, ViewModel.AssociadoContribuicao.valorAtual);

            if (Retorno.flagError) {

                this.Flash(UtilMessage.TYPE_MESSAGE_ERROR, Retorno.listaErros.FirstOrDefault());

                return Json(new { flagSucesso = Retorno.flagError });

            }

            this.Flash(UtilMessage.TYPE_MESSAGE_SUCCESS, $"A cobrança do Associado {OAssociado.Pessoa.nome} foi salva com sucesso!");

            var listaDependentes = this.OAssociadoDependenteBL.listar(OAssociado.id, "", "S").ToList();

            if (!listaDependentes.Any()) {

                return Json(new { flagSucesso = Retorno.flagError });

            }

            var OCobrancaPrincipal = Retorno.info as AssociadoContribuicao;

            foreach (var ODependente in listaDependentes) {

                OContribuicaoGeracaoBL = new AssociadoContribuicaoGeracaoBL();

                OContribuicaoGeracaoBL.gerarCobrancaDependente(ODependente, OCobrancaPrincipal);

                this.Flash(UtilMessage.TYPE_MESSAGE_SUCCESS, $"A cobrança do dependente {ODependente.Pessoa.nome} foi salva com sucesso!");

            }

            return Json(new { flagSucesso = Retorno.flagError });
        }


    }
}
