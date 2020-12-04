using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using BLL.Caches;
using BLL.FinanceiroLancamentos;
using DAL.FinanceiroLancamentos;
using Newtonsoft.Json;

namespace WEB.Areas.FinanceiroLancamentos.Helpers {

    public class CredorHelper {

        //Atributos
        private static CredorHelper _instance;
        private CredorVWBL _CredorVWBL;

        //Propriedades
        public static CredorHelper getInstance => _instance = _instance ?? new CredorHelper();
        private CredorVWBL OCredorVWBL => _CredorVWBL = _CredorVWBL ?? new CredorVWBL();

		//
		public SelectList selectList(string selected, bool flagCache = true) {

		    var lista = JsonConvert.DeserializeObject<List<CredorVW>>(this.getList(flagCache));

		    return new SelectList(lista, "id", "nome", selected);
        }

        public MultiSelectList multiSelectList(List<string> selecteds, bool flagCache = true) {

            var lista = JsonConvert.DeserializeObject<List<CredorVW>>(this.getList(flagCache));

            return new MultiSelectList(lista, "id", "nome", selecteds);
        }

        /// <summary>
        /// Criação de listagem customizada para selectlist
        /// </summary>
        private string getList(bool flagCache = true) {
            
            if (!flagCache) {

                var query = this.OCredorVWBL.listar("")
                                .Select(x => new {
                                    x.id, 
                                    x.idPessoa, 
                                    x.descricaoCategoriaPessoa, 
                                    x.nome, 
                                    x.nroDocumento
                                }).ToList();

                var list = query.Select(x => new {
                                    x.id,
                                    x.idPessoa,
                                    nome = (x.descricaoCategoriaPessoa.ToUpper() + " - " + x.nome.ToUpper() + (!UtilString.formatCPFCNPJ(x.nroDocumento).isEmpty() ? " (" + UtilString.formatCPFCNPJ(x.nroDocumento) + ")" : ""))
                                }).OrderBy(x => x.nome).ToList();

                return JsonConvert.SerializeObject(list);
            }

            var OCacheService = CacheService.getInstance;
            var keyCache = CredorVWBL.keyCache;

            object jsonData = OCacheService.carregar(keyCache);

            if (jsonData != null) {

                return jsonData.ToString();
            }

            var listaCampos = this.OCredorVWBL.listar("")
                                  .Select(x => new {
                                      x.id,
                                      x.idPessoa,
                                      x.descricaoCategoriaPessoa, 
                                      x.nome, 
                                      x.nroDocumento
                                  }).ToList()
                                    .Select(x => new {
                                        x.id,
                                        x.idPessoa,
                                        nome = (x.descricaoCategoriaPessoa.ToUpper() + " - " + x.nome.ToUpper() + (!UtilString.formatCPFCNPJ(x.nroDocumento).isEmpty() ? " (" + UtilString.formatCPFCNPJ(x.nroDocumento) + ")" : ""))
                                    }).OrderBy(x => x.nome).ToList();

            OCacheService.remover(keyCache);

            var json = JsonConvert.SerializeObject(listaCampos);

            OCacheService.adicionar(keyCache, json);

            return json;
        }
    }
}