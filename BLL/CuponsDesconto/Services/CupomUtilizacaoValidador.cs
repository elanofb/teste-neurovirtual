using System;
using System.Linq;
using BLL.Services;
using DAL.Financeiro;

namespace BLL.CuponsDesconto.Services {

    public class CupomUtilizacaoValidador : DefaultBL, ICupomUtilizacaoValidador {

        //Atributos

        //Propriedades

        /// <summary>
        /// Construtor
        /// </summary>
        public CupomUtilizacaoValidador() {

        }

        //Validar cupom para utilização
        public UtilRetorno validarUso(int idOrganizacaoParam, string codigoCupom, byte idTipoPagamento) {

            var OCupom = this.db.CupomDesconto.FirstOrDefault(x => x.codigo == codigoCupom && x.idOrganizacao == idOrganizacaoParam && x.flagExcluido == "N");

            if (OCupom == null) {

                return UtilRetorno.newInstance(true, "O código de cupom informado é inválido.");

            }

            if (OCupom.dtVencimento.HasValue && OCupom.dtVencimento < DateTime.Today) {

                return UtilRetorno.newInstance(true, "Desculpe, o cupom informado está vencido e não pode mais ser utilizado.");

            }

            if (OCupom.ativo != "S") {

                return UtilRetorno.newInstance(true, "Desculpe, o cupom informado não está ativo para uso.");
            }

            if (OCupom.valorDesconto <= 0) {

                return UtilRetorno.newInstance(true, "Desculpe, o cupom de desconto informado não possui valor válido.");
            }

            bool flagPedido = idTipoPagamento == TipoReceitaConst.PEDIDO;

            bool flagEvento = idTipoPagamento == TipoReceitaConst.INSCRICAO_EVENTO;

            bool flagContribuicao = idTipoPagamento == TipoReceitaConst.CONTRIBUICAO;


            if (flagPedido == true && OCupom.flagPedido != true) {
                return UtilRetorno.newInstance(true, "Desculpe, o cupom informado não pode ser utilizado para pagamento de um pedido.");
            }

            if (flagEvento == true && OCupom.flagEvento != true) {
                return UtilRetorno.newInstance(true, "Desculpe, o cupom informado não pode ser utilizado para pagamento de uma inscrição.");
            }

            if (flagContribuicao == true && OCupom.flagContribuicao != true) {
                return UtilRetorno.newInstance(true, "Desculpe, o cupom informado não pode ser utilizado para esse tipo de pagamento.");
            }

            if (OCupom.qtdeUsos.toInt() == 0){

                return UtilRetorno.newInstance(false, "Cupom válido!");
            }

            var qtdeUtilizados = db.TituloReceitaPagamento.Where(x => x.dtExclusao == null && x.idCupomDesconto == OCupom.id).Select(x => x.id).Count();

            if (qtdeUtilizados >= OCupom.qtdeUsos.toInt()){

                return UtilRetorno.newInstance(true, "Desculpe, o limite de utilização desse cupom de desconto já foi atingido.");

            }

            return UtilRetorno.newInstance(false, "Cupom válido!", OCupom);
        }
    }
}
