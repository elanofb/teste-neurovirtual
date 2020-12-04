using System.Collections.Generic;
using System.Web.Mvc;
using DAL.Financeiro;

namespace WEB.Areas.Financeiro.Helpers {

    public class FinanceiroHelper {

        public static SelectList getDtPesquisaFinanceiro(string selected) {
            var list = new[] {
                    new{value = "V", text = "Data Vencimento"},
                    new{value = "P", text = "Data Pagamento"}
            };

            return new SelectList(list, "value", "text", selected);
        }

        public static SelectList getDtPesquisaVencimentoCredito(string selected) {
            var list = new[] {
                new{value = "V", text = "Data Vencimento"},
                new{value = "C", text = "Data de Crédito"}
            };

            return new SelectList(list, "value", "text", selected);
        }

        public static SelectList getDtPesquisaFinanceiroExtrato(string selected) {
            var list = new[] {
                new{value = "V", text = "Data Vencimento"},
                new{value = "P", text = "Data Pagamento / Operação"},
                new{value = "C", text = "Data de Crédito"}
            };

            return new SelectList(list, "value", "text", selected);
        }

        public static SelectList getTipoAlteracao(string selected) {
            var list = new[] {
                    new{value = "UN", text = "Alterar apenas este registro"},
                    new{value = "EP", text = "Alterar este e os próximos registros"},
                    new{value = "TO", text = "Alterar todos os registros"}
            };

            return new SelectList(list, "value", "text", selected);
        }

        public static bool receitasBloqueadas(byte idTipoReceita) {
            var listaTipos = new List<byte>();
            listaTipos.Add(TipoReceitaConst.PEDIDO);
            listaTipos.Add(TipoReceitaConst.INSCRICAO_EVENTO);
            listaTipos.Add(TipoReceitaConst.CONTRIBUICAO);
            listaTipos.Add(TipoReceitaConst.PLANO);

            if(listaTipos.Contains(idTipoReceita)) return true;

            return false;
        }

        public static MultiSelectList getTipoMovimentacao(List<int> selected) {
            var list = new[] {
                    new{value = "", text = "Todas"},
                    new{value = "DESP", text = "Despesa"},
                    new{value = "RECE", text = "Receita"},
                    new{value = "TRANSD", text = "Transferência de Saída"},
                    new{value = "TRANSR", text = "Transferência Recebida"},
            };

            return new MultiSelectList(list, "value", "text", selected);
        }
    }
}
