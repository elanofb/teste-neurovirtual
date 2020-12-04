using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Linq;
using BLL.Contribuicoes;
using DAL.Contribuicoes;
using Newtonsoft.Json;
using UTIL.UtilClasses;

namespace WEB.Areas.Contribuicoes.Helpers {

    public class ContribuicaoHelper {

        //Constanctes
        private static ContribuicaoHelper _instance;

        //Atributos
        private IContribuicaoBL _ContribuicaoPadraoBL;

        //Propriedades
        public static ContribuicaoHelper getInstance => _instance = _instance ?? new ContribuicaoHelper();
        private IContribuicaoBL OContribuicaoBL => _ContribuicaoPadraoBL = _ContribuicaoPadraoBL ?? new ContribuicaoPadraoBL();


		//Combo com parcelamento de anuidade
		public SelectList selectListParcelamento(int? selected) {

		    int limite = /*Config.limiteParcelamentoContribuicao ??*/ 1;

		    var listaOpcoes = new List<OptionSelect>();

           listaOpcoes.Add(new OptionSelect { value = "1", text = "Á vista" });

		    for (int i = 2; i <= limite; i++) {
		        listaOpcoes.Add(new OptionSelect { value = i.ToString(), text = String.Concat(i, " parcelas") });
		    }


            return new SelectList(listaOpcoes, "value", "text", selected);
		}

        //Combo com relacao de contribuicoes
        public SelectList selectList(int? selected, int[] exclude = null) {

            var query = this.OContribuicaoBL.listar("", "S")
                                    .Where(x => x.idPeriodoContribuicao > 0 && (x.dtValidade == null || x.dtValidade > DateTime.Now))
                                    .Select(x => new { value = x.id, text = String.Concat(x.PeriodoContribuicao.descricao, " - ", x.descricao) });

            if (exclude != null && exclude.Length > 0) {

                query = query.Where(x => !exclude.Contains(x.value));

            }

            var lista = query.ToList().OrderBy(x => x.text).ToList();

            return new SelectList(lista, "value", "text", selected);
        }

        //Combo com relacao de contribuicoes
        public SelectList selectListQuemCobrar(bool? selected) {

            var list = new[] {
                    new{value = true, text = "Cobrar TODOS os associados com preço cadastrado."},
                    new{value = false, text = "Cobrar somente aqueles que NÃO optaram por outros planos de cobrança."}
            };

            return new SelectList(list, "value", "text", selected);
        }

        //Combo com relacao de contribuicoes
        public static SelectList selectListFormaPagamento(bool? selected) {

            var list = new[] {
                    new{value = false, text = "Deixar o associado escolher a forma de pagamento"},
                    new{value = true, text = "Gerar boleto automaticamente"}
            };

            return new SelectList(list, "value", "text", selected);
        }

		//Combo com parcelamento de anuidade
		public SelectList selectListSituacao(string selected) {

		    var listaOpcoes = new List<OptionSelect>();

           listaOpcoes.Add(new OptionSelect { value = "cobrados", text = "Somente já cobrados" });

            listaOpcoes.Add(new OptionSelect { value = "nao_cobrados", text = "Somente não cobrados" });

           listaOpcoes.Add(new OptionSelect { value = "nao_pagos", text = "Somente não pagos" });

           listaOpcoes.Add(new OptionSelect { value = "atrasados", text = "Somente vencidos" });

           listaOpcoes.Add(new OptionSelect { value = "isentos", text = "Somente isentos" });

            listaOpcoes.Add(new OptionSelect { value = "quitados", text = "Somente quitados" });


            return new SelectList(listaOpcoes, "value", "text", selected);
		}

        public MultiSelectList multiSelectList(List<int> selecteds) {

            var lista = JsonConvert.DeserializeObject<List<Contribuicao>>(this.getList());

            return new MultiSelectList(lista, "id", "descricao", selecteds);
        }

        /// <summary>
        /// Criação de listagem customizada para selectlist
        /// </summary>
        private string getList() {
            
            var query = this.OContribuicaoBL.listar("", "S").Select(x => new { x.id, x.descricao }).ToList();

            var list = query
                .Select(x => new { id = x.id, descricao = x.descricao })
                .OrderBy(x => x.descricao).ToList();

            return JsonConvert.SerializeObject(list);
                        
        }

    }
}