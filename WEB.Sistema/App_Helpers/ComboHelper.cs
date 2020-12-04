using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Linq;
using BLL.Produtos;

namespace WEB.Helpers {
    public class ComboHelper {

        //Select Lista para selecao de sexo
        public static SelectList selectListSexo(string selected) {
            var list = new[] {
                    new{value = "M", text = "Masculino"},
                    new{value = "F", text = "Feminino"}
            };
            return new SelectList(list, "value", "text", selected);
        }

        //Select da lista de situações do produto
        public static SelectList selectListSituacoes(string selected)
        {
            var listSits = new ProdutoSituacaoConsultaBL().query().Where(s => s.ativo);
            var list = new List<object>();
            foreach (var item in listSits)
            {
                list.Add(new { value = item.id, text = item.descricao });
            }
            return new SelectList(list, "value", "text", selected);
        }

        /*        //
                public static SelectList selectListTipoBusca(int selected) {
                    var list = new[] {
                            new{value = TipoBuscaConst.NUMERO_INSCRICAO, text = "Número da Inscrição"},
                            new{value = TipoBuscaConst.CPF_CNPJ, text = "CPF/CNPJ"}
                    };
                    return new SelectList(list, "value", "text", selected);
                }*/

        //
        public static SelectList selectListYesNo(string selected) {
            var list = new[] {
                    new{value = "N", text = "Não"},
                    new{value = "S", text = "Sim"}
            };
            return new SelectList(list, "value", "text", selected);
        }

        //
        public static SelectList selectListTrueFalse(bool? selected) {
            var list = new[] {
                    new{value = true, text = "Sim"},
                    new{value = false, text = "Não"}
            };
            return new SelectList(list, "value", "text", selected);
        }

        //
        public static SelectList selectListStatus(string selected) {
            var list = new[] {
                    new{value = "S", text = "Ativo"},
                    new{value = "N", text = "Desativado"},
            };

            return new SelectList(list, "value", "text", selected);
        }

        public static SelectList selectListStatus(bool? selected) {
            var list = new[] {
                    new{value = true, text = "Ativo"},
                    new{value = false, text = "Desativado"},
            };

            return new SelectList(list, "value", "text", selected);
        }

        //
        public static SelectList selectListFlagCampo(byte? selected) {
            var list = new[] {
                new{value = 0, text = "Não solicitar"},
                new{value = 1, text = "Sim, obrigatório."},
                new{value = 2, text = "Sim, opcional"}
            };
            return new SelectList(list, "value", "text", selected);
        }
        
        //
        public static SelectList selectListAno(string selected, int qtdAnosPassado = 3, int qtdAnosFuturo = 5) {

            int cont = 0;

            int limiteAno = DateTime.Now.Year + qtdAnosFuturo;

            int minimoAno = DateTime.Now.Year - qtdAnosPassado;

            var lista = new List<object>();

            for (int ano = minimoAno; ano <= limiteAno; ano++) {
                lista.Add(new { value = ano.ToString(), text = ano.ToString() });
                cont++;
            }

            return new SelectList(lista, "value", "text", selected);
        }

        //
        public static SelectList selectListHttpMetodos(string selected) {
            var list = new[] {
                    new{value = "GET", text = "GET"},
                    new{value = "POST", text = "POST"}
            };
            return new SelectList(list, "value", "text", selected);
        }

        //
        public static SelectList selectListAmbiente(string selected) {
            var list = new[] {
                    new{value = "D", text = "Desativado"},
                    new{value = "P", text = "Produção"},
                    new{value = "T", text = "Teste"}
            };
            return new SelectList(list, "value", "text", selected);
        }
        //
        public static SelectList selectListNroRegistros(string selected) {
            var list = new[] {
                    new{value = "20", text = "20 registros"},
                    new{value = "50", text = "50 registros"},
                    new{value = "100", text = "100 registros"},
                    new{value = "500", text = "500 registros"},
                    new{value = "1000", text = "1000 registros"}
            };
            return new SelectList(list, "value", "text", selected);
        }

        //
        public static SelectList selectListDiasMes(string selected) {
            var list = new[] {
                    new{value = "01", text = "01"},
                    new{value = "02", text = "02"},
                    new{value = "03", text = "03"},
                    new{value = "04", text = "04"},
                    new{value = "05", text = "05"},
                    new{value = "06", text = "06"},
                    new{value = "07", text = "07"},
                    new{value = "08", text = "08"},
                    new{value = "09", text = "09"},
                    new{value = "10", text = "10"},
                    new{value = "11", text = "11"},
                    new{value = "12", text = "12"},
                    new{value = "13", text = "13"},
                    new{value = "14", text = "14"},
                    new{value = "15", text = "15"},
                    new{value = "16", text = "16"},
                    new{value = "17", text = "17"},
                    new{value = "18", text = "18"},
                    new{value = "19", text = "19"},
                    new{value = "20", text = "20"},
                    new{value = "21", text = "21"},
                    new{value = "22", text = "22"},
                    new{value = "23", text = "23"},
                    new{value = "24", text = "24"},
                    new{value = "25", text = "25"},
                    new{value = "26", text = "26"},
                    new{value = "27", text = "27"},
                    new{value = "28", text = "28"},
                    new{value = "29", text = "29"},
                    new{value = "30", text = "30"},
                    new{value = "31", text = "31"}
            };

            selected = selected.PadLeft(2, '0');

            return new SelectList(list, "value", "text", selected);
        }

        /// <summary>
        /// Listagem de parcelamento
        /// </summary>
        public static SelectList selectListParcelamento(int selected, int limiteMaximo = 12, int limiteMinino = 1) {
            var list = new[] {
                    new{value = 1, text = "Somente à vista"},
                    new{value = 2, text = "2 Parcelas"},
                    new{value = 3, text = "3 Parcelas"},
                    new{value = 4, text = "4 Parcelas"},
                    new{value = 5, text = "5 Parcelas"},
                    new{value = 6, text = "6 Parcelas"},
                    new{value = 7, text = "7 Parcelas"},
                    new{value = 8, text = "8 Parcelas"},
                    new{value = 9, text = "9 Parcelas"},
                    new{value = 10, text = "10 Parcelas"},
                    new{value = 11, text = "11 Parcelas"},
                    new{value = 12, text = "12 Parcelas"}
            };

            list = list.Where(x => x.value <= limiteMaximo && x.value >= limiteMinino).ToArray();

            return new SelectList(list, "value", "text", selected);
        }

        //
        public static SelectList selectListMeses(string selected) {
            var list = new[] {
                    new{value = "1", text = "Janeiro"},
                    new{value = "2", text = "Fevereiro"},
                    new{value = "3", text = "Março"},
                    new{value = "4", text = "Abril"},
                    new{value = "5", text = "Maio"},
                    new{value = "6", text = "Junho"},
                    new{value = "7", text = "Julho"},
                    new{value = "8", text = "Agosto"},
                    new{value = "9", text = "Setembro"},
                    new{value = "10", text = "Outubro"},
                    new{value = "11", text = "Novembro"},
                    new{value = "12", text = "Dezembro"}
            };
            return new SelectList(list, "value", "text", selected);
        }
        
        //
        public static MultiSelectList multiSelectListMeses(List<int> selected) {
            var list = new[] {
                    new{value = 1, text = "Janeiro"},
                    new{value = 2, text = "Fevereiro"},
                    new{value = 3, text = "Março"},
                    new{value = 4, text = "Abril"},
                    new{value = 5, text = "Maio"},
                    new{value = 6, text = "Junho"},
                    new{value = 7, text = "Julho"},
                    new{value = 8, text = "Agosto"},
                    new{value = 9, text = "Setembro"},
                    new{value = 10, text = "Outubro"},
                    new{value = 11, text = "Novembro"},
                    new{value = 12, text = "Dezembro"}
            };
            return new MultiSelectList(list, "value", "text", selected);
        }

        //
        public static SelectList selectListMesesNumericos(string selected) {
            var list = new[] {
                    new{value = "1", text = "01"},
                    new{value = "2", text = "02"},
                    new{value = "3", text = "03"},
                    new{value = "4", text = "04"},
                    new{value = "5", text = "05"},
                    new{value = "6", text = "06"},
                    new{value = "7", text = "07"},
                    new{value = "8", text = "08"},
                    new{value = "9", text = "09"},
                    new{value = "10", text = "10"},
                    new{value = "11", text = "11"},
                    new{value = "12", text = "12"}
            };
            return new SelectList(list, "value", "text", selected);
        }

        public static SelectList tipoPessoa(string selected) {
            var list = new[] {
                    new{value = "F", text = "FÍSICO"},
                    new{value = "J", text = "JURÍDICO"},
            };

            return new SelectList(list, "value", "text", selected);
        }

        //
        public static SelectList selectTipoComissionamento(string selected) {
            var list = new[] {
                    new{value = "F", text = "Fixo"},
                    new{value = "P", text = "Percentual"}
            };
            return new SelectList(list, "value", "text", selected);
        }

        public static SelectList selectStatusProcessamento(string selected) {
            var list = new[] {
                    new{value = "A_PROCESSAR", text = "A Processar"},
                    new{value = "PROCESSANDO", text = "Processando"},
                    new{value = "PROCESSADO", text = "Processado"}
            };
            return new SelectList(list, "value", "text", selected);
        }

        public static SelectList selectStatusGeracao(string selected) {
            var list = new[] {
                    new{value = "A_GERAR", text = "A Gerar"},
                    new{value = "GERADO", text = "Gerado"},
                    new{value = "ERRO_GERAR", text = "Erro ao Gerar"}
            };
            return new SelectList(list, "value", "text", selected);
        }

        public static SelectList selectTipoOrdernacao(string selected) {
            var list = new[] {
                    new{value = "asc", text = "Crescente"},
                    new{value = "desc", text = "Decrescente"}
            };
            return new SelectList(list, "value", "text", selected);
        }
        
        public static SelectList selectListDesconto(decimal? selected){
            
            var list = new List<object>();
            
            for (decimal i = new decimal(5.00); i <= 90; i += 5){
                
                var item = new{value = i.ToString("F2"), text = String.Concat(i.ToString(), " %") };
                
                list.Add(item);
            }
            
            return new SelectList(list, "value", "text", selected);
            
        }
    }
}