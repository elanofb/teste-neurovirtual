using System;
using System.Json;
using System.Linq;
using System.Web.Mvc;
using BLL.Associados;
using BLL.AssociadosContribuicoes;
using BLL.Contribuicoes;
using DAL.Associados;
using DAL.Contribuicoes;
using WEB.App_Infrastructure;

namespace WEB.Areas.AssociadosContribuicoes.Controllers {

    public class AssociadoContribuicaoPrecoController : BaseSistemaController {

        //Constantes

        //Atributos
        private IAssociadoBL _AssociadoBL;
        private IContribuicaoBL _ContribuicaoBL;
        private IAssociadoContribuicaoBL _AssociadoContribuicaoBL;

        //Propriedades
        private IContribuicaoBL OContribuicaoBL => this._ContribuicaoBL = this._ContribuicaoBL ?? new ContribuicaoPadraoBL();
        private IAssociadoBL OAssociadoBL => this._AssociadoBL = this._AssociadoBL ?? new AssociadoBL();
        private IAssociadoContribuicaoBL OAssociadoContribuicaoBL => _AssociadoContribuicaoBL = _AssociadoContribuicaoBL ?? new AssociadoContribuicaoBL();

        // POST: Contribuicoes/Default/listar
        [HttpPost, ActionName("buscar-preco")]
        public ActionResult buscarPreco(int? idContribuicao, int idAssociado) {

            Associado OAssociado = this.OAssociadoBL.carregar(idAssociado);

            Contribuicao OContribuicao = this.OContribuicaoBL.carregar(UtilNumber.toInt32(idContribuicao));

            if (OAssociado == null) {
                return Json(new JsonMessage { error = true, message = "Esse método precisa do código do associado para calcular o valor da anuidade." }, JsonRequestBehavior.AllowGet);
            }

            if (OContribuicao == null) {
                return Json(new JsonMessage { error = true, message = "Informe a contribuição para calcular o valor." }, JsonRequestBehavior.AllowGet);
            }

            var OTabela = OContribuicao.retornarTabelaVigente();

            var OPreco = OTabela.retornarPreco(UtilNumber.toInt32(OAssociado.idTipoAssociado));

            if (OPreco.id == 0) {
                return Json(new JsonMessage { error = true, message = "Não há preço configurado para esse tipo de associado." }, JsonRequestBehavior.AllowGet);
            }

            var listaVencimentos = OContribuicao.listaContribuicaoVencimento.Where(x => x.dtExclusao == null).ToList();
            var OVencimento = new ContribuicaoVencimento();

            if (OContribuicao.idTipoVencimento == TipoVencimentoConst.FIXO_PELA_CONTRIBUICAO) {
                OVencimento = OContribuicao.retornarProximoVencimento();
            }

            if (OContribuicao.idTipoVencimento == TipoVencimentoConst.VENCIMENTO_PELA_ADMISSAO_ASSOCIADO) {
                var anoVigencia = DateTime.Today.Year;
                var dtCobranca = OAssociado.dtAdmissao ?? DateTime.Today;

                DateTime? dtVencimentoAdmissao = new DateTime(anoVigencia, dtCobranca.Month, dtCobranca.Day);
                OVencimento = OContribuicao.retornarProximoVencimento(dtVencimentoAdmissao);
            }


            if (OContribuicao.idTipoVencimento == TipoVencimentoConst.VENCIMENTO_PELO_ULTIMO_PAGAMENTO) {

                var UltimaContribuicao = OAssociadoContribuicaoBL.listar(OContribuicao.id, idAssociado, null, true, "")
                                                                .Select(x => new { x.dtPagamento, x.dtVencimentoOriginal})
                                                                .OrderByDescending(x => x.dtPagamento)
                                                                .FirstOrDefault();

                var dtUltimaContribuicao = UltimaContribuicao == null ?  DateTime.Today : (UltimaContribuicao.dtPagamento ?? UltimaContribuicao.dtVencimentoOriginal);

                DateTime? dtNovoVencimento = new DateTime(DateTime.Today.Year, dtUltimaContribuicao.Month, dtUltimaContribuicao.Day);

                OVencimento = OContribuicao.retornarProximoVencimento(dtNovoVencimento);
            }

            var dtVencimento = OVencimento.dtVencimento;

            var listaJson = listaVencimentos.Select(x => new {
                x.id,
                dtVencimento = string.Concat(x.diaVencimento.ToString().PadLeft(2, '0'), "/", x.mesVencimento.ToString().PadLeft(2, '0'))
            });

            return Json(new {
                error = false,
                valor = UtilNumber.toDecimal(OPreco.valorFinal).ToString("F"),
                dtVencimento = dtVencimento.exibirData(),
                listaVencimentos = listaJson,
                flagVencimentoFixo = (listaVencimentos.Count > 0),
                dtInicioVigencia = OVencimento.dtInicioVigencia.exibirData(),
                dtFimVigencia = OVencimento.dtFimVigencia.exibirData(),
                OPreco.flagIsento
            }, JsonRequestBehavior.AllowGet);
        }



    }
}
