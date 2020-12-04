using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using BLL.Caches;
using BLL.Financeiro;
using BLL.Services;
using DAL.Financeiro;
using Newtonsoft.Json;

namespace WEB.Areas.Financeiro.Helpers{
    public class CategoriaTituloHelper{

        //Atributos
        private static CategoriaTituloHelper _instance;
        private ICategoriaTituloBL _CategoriaTituloBL;

        //Propriedades
        public static CategoriaTituloHelper getInstance => _instance = _instance ?? new CategoriaTituloHelper();
        private ICategoriaTituloBL OCategoriaTituloBL => (_CategoriaTituloBL = _CategoriaTituloBL ?? new CategoriaTituloBL());
        //Construtor

        /// <summary>
        /// Exibir combo somente com as subcontas Pai
        /// </summary>
        public SelectList selectListPais(int? selected, int? idMacroConta, List<int> idsExclude = null) {

            var listaItens = this.carregarLista()
                                 .Where(x => x.idCategoriaPai.toInt() == 0)
                                 .ToList();
            

            if (idsExclude != null) {
                
                listaItens = listaItens.Where(x => !idsExclude.Contains(x.id)).ToList();
            }


            return new SelectList(listaItens, "id", "descricao", selected);
        }
        
        /// <summary>
        /// Exibir combo com as subcontas
        /// </summary>
        public SelectList selectList(int? selected, int? idMacroConta, bool flagCache = true, List<int> idsExclude = null) {

            var lista = JsonConvert.DeserializeObject<List<CategoriaTitulo>>(this.listarFromCache(flagCache));

            if (idMacroConta > 0) {
                lista = lista.Where(x => x.idMacroConta == idMacroConta).ToList();
            }

            if (idsExclude != null) {
                lista = lista.Where(x => !idsExclude.Contains(x.id)).ToList();
            }

            return new SelectList(lista, "id", "descricao", selected);
        }

        //
        public MultiSelectList selectMultList(List<int?> selected, int? idMacroConta, bool flagCache = true) {

            var lista = JsonConvert.DeserializeObject<List<CategoriaTitulo>>(this.listarFromCache(flagCache));

            if (idMacroConta > 0) {
                lista = lista.Where(x => x.idMacroConta == idMacroConta).ToList();
            }

            return new MultiSelectList(lista, "id", "descricao", selected);
        }

        /// <summary>
        /// Carregar informacoes do cache (se existir)
        /// </summary>
        private string listarFromCache(bool flagCache = true) {
            
            List<CategoriaTitulo> listaItens;
            
            if (!flagCache) {
            
                listaItens = this.carregarLista();

                return JsonConvert.SerializeObject(listaItens);
            }

            var OCacheService = CacheService.getInstance;

            object jsonData = OCacheService.carregar(CategoriaTituloBL.keyCache);

            if (jsonData != null) {

                return jsonData.ToString();
            }

            listaItens = this.carregarLista();

            OCacheService.remover(CategoriaTituloBL.keyCache);

            var json = JsonConvert.SerializeObject(listaItens);

            OCacheService.adicionar(CategoriaTituloBL.keyCache, json);

            return json;
        }

        /// <summary>
        /// Carregar a lista no banco de dados e realizar os ajustes necessários para melhor exibição
        /// </summary>
        private List<CategoriaTitulo> carregarLista() {
            
            var listaCategorias = OCategoriaTituloBL.listar(0, "", "S")
                                                    .Select(x => new {
                                                        x.id, 
                                                        x.descricao, 
                                                        x.codigoFiscal,
                                                        x.idMacroConta,
                                                        MacroConta = new {
                                                            id = x.MacroConta == null ?  0 : x.MacroConta.id,
                                                            x.MacroConta.descricao, 
                                                            x.MacroConta.codigoFiscal
                                                        },
                                                        x.idCategoriaPai,
                                                        CategoriaPai = new {
                                                            id = x.CategoriaPai == null? 0: x.CategoriaPai.id,
                                                            descricao = x.CategoriaPai == null? "": x.CategoriaPai.descricao,
                                                            codigoFiscal = x.CategoriaPai == null? "": x.CategoriaPai.codigoFiscal
                                                        }
                                                    }).ToListJsonObject<CategoriaTitulo>();

            listaCategorias = this.ajustarParaExibicao(listaCategorias);
            
            return listaCategorias;
            
        }

        /// <summary>
        /// Ajustar os itens para exibição em combos 
        /// </summary>
        private List<CategoriaTitulo> ajustarParaExibicao(List<CategoriaTitulo> listaCategorias) {
            
            listaCategorias = listaCategorias.OrderBy(x => x.CategoriaPai.codigoFiscal)
                                            .ThenBy(x => x.codigoFiscal.toInt())
                                            .ThenBy(x => x.descricao)
                                            .ToList();
                                        
            
            listaCategorias.ForEach(item => {

                item.descricao = item.descricaoSubConta();

            });

            return listaCategorias;            
        }
    }
}