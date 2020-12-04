using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using BLL.Caches;
using BLL.Financeiro;
using BLL.Services;
using DAL.Financeiro;
using Newtonsoft.Json;

namespace WEB.Areas.Financeiro.Helpers {

    public class MacroContaHelper{

        //Atributos
        private static MacroContaHelper _instance;
        private IMacroContaBL _MacroContaBL;

        //Propriedades
        public static MacroContaHelper getInstance => _instance = _instance ?? new MacroContaHelper();
        private IMacroContaBL OMacroContaBL => _MacroContaBL = _MacroContaBL ?? new MacroContaBL();

        
        //
        public SelectList selectList(int idCentroCusto, int? selected, bool flagCache = true) {

            var lista = JsonConvert.DeserializeObject<List<MacroConta>>(this.getList(idCentroCusto, flagCache));

            return new SelectList(lista, "id", "descricao", selected);
        }
        
        //
		public SelectList selectList(int? selected, bool flagCache = true) {

		    var lista = JsonConvert.DeserializeObject<List<MacroConta>>(this.getList(0, flagCache));

		    return new SelectList(lista, "id", "descricao", selected);
        }

        //
        public MultiSelectList selectMultList(List<int?> selected, bool flagCache = true) {

            var lista = JsonConvert.DeserializeObject<List<MacroConta>>(this.getList(0, flagCache));

            return new MultiSelectList(lista, "id", "descricao", selected);
        }

        public SelectList selectListPorTipo(int? selected, string flagReceitaDespesa, bool flagCache = true){

            var lista = JsonConvert.DeserializeObject<List<MacroConta>>(this.getList(0, flagCache, flagReceitaDespesa));

            return new SelectList(lista, "id", "descricao", selected);
        }

        /// <summary>
        /// 
        /// </summary>
        private string getList(int idCentroCusto, bool flagCache = true, string flagReceitaDespesa = "") {
           
            if (!flagCache){

                var query = OMacroContaBL.listar("", true, idCentroCusto)
                                        .Select(x => new {
                                            x.id, 
                                            x.flagReceitaDespesa, 
                                            x.codigoFiscal, 
                                            x.descricao
                                        }).ToListJsonObject<MacroConta>();

                if (!flagReceitaDespesa.isEmpty()) {
                    query = query.Where(x => x.flagReceitaDespesa == flagReceitaDespesa || x.flagReceitaDespesa == "A").ToList();
                }
                
                query = query.OrderBy(x => UtilString.onlyNumber(x.codigoFiscal).toInt()).ThenBy(x => x.descricao).ToList();

                var list = query.Select(x => new { x.id, descricao = x.descricaoMacroConta() }).ToList();

                return JsonConvert.SerializeObject(list);
            }

            var OCacheService = CacheService.getInstance;
            
            var keyCache = MacroContaBL.keyCache + (flagReceitaDespesa.isEmpty() ? "" : "_" + flagReceitaDespesa);

            object jsonData = OCacheService.carregar(keyCache);

            if (jsonData != null) {

                return jsonData.ToString();
            }

            var queryCampos = OMacroContaBL.listar("", true).ToList();

            if (!flagReceitaDespesa.isEmpty()) {
                queryCampos = queryCampos.Where(x => x.flagReceitaDespesa == flagReceitaDespesa || x.flagReceitaDespesa == "A").ToList();
            }

            var listaCampos = queryCampos.Select(x => new {x.id, descricao = x.descricaoMacroConta(), x.codigoFiscal}).OrderBy(x => UtilString.onlyNumber(x.codigoFiscal).toInt()).ThenBy(x => x.descricao);

            OCacheService.remover(keyCache);

            var json = JsonConvert.SerializeObject(listaCampos);

            OCacheService.adicionar(keyCache, json);

            return json;
        }

        public SelectList selectListReferencias(int? selected, int idMacroConta){
            
            var listaObjetos = OMacroContaBL.listarPorMacroConta(idMacroConta).OrderBy(x => x.nome);
            return new SelectList(listaObjetos, "id", "nome", selected);
        }

    	public string getReferencia(int idDespesa, int idMacroConta) {

            var OReferencia = OMacroContaBL.getReferenciaReceita(idMacroConta, idDespesa);
            return OReferencia.nome;
		}

        public SelectList selectListReceitaDespesa(string selected){
            
            var list = new[] { 
                    new{value = "A", text = "Ambos"},
                    new{value = "R", text = "Receita"},
                    new{value = "D", text = "Despesa"}
            };
            return new SelectList(list, "value", "text", selected);
        }
    }
}