using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Linq;
using BLL.Associados;
using BLL.Caches;
using BLL.Services;
using DAL.Associados;

namespace WEB.Areas.Associados.Helpers {

    public class TipoAssociadoHelper {

        private static TipoAssociadoHelper _instance;
        private ITipoAssociadoBL _TipoAssociadoBL;

        public static TipoAssociadoHelper getInstance => _instance = _instance ?? new TipoAssociadoHelper();
        private ITipoAssociadoBL OTipoAssociadoBL => _TipoAssociadoBL = _TipoAssociadoBL ?? new TipoAssociadoBL();


        //Carregar combo de seleção dos tipos de associados
        public SelectList selectList(int? selected, string flagTipoPessoa = "", bool? flagNaoAssociado = false, bool flagCache = true, int?[] idsRemove = null) {
            
            var listaItens = CacheService.getInstance.carregar<List<TipoAssociado>>(CacheService.TIPO_ASSOCIADO_DD_SIMPLES);
            
            if (listaItens == null || !flagCache){
                
                listaItens = OTipoAssociadoBL.listar("", null, "S").Select(x => new {
                    x.id, x.descricao, x.nomeDisplay, x.flagPessoaFisica, x.flagPessoaJuridica, x.flagNaoAssociado, x.idCategoria,
                    Categoria = new { x.Categoria.descricao }
                }).ToListJsonObject<TipoAssociado>();
                
                if (flagCache) {
                    CacheService.getInstance.remover(CacheService.TIPO_ASSOCIADO_DD_SIMPLES);

                    CacheService.getInstance.adicionar(CacheService.TIPO_ASSOCIADO_DD_SIMPLES, listaItens);
                }
            }
            
            if (flagTipoPessoa == "F") {
                listaItens = listaItens.Where(x => x.flagPessoaFisica).ToList();
            }
            
            if (flagTipoPessoa == "J") {
                listaItens = listaItens.Where(x => x.flagPessoaJuridica).ToList();
            }
            
            if (flagNaoAssociado.HasValue) {
                listaItens = listaItens.Where(x => x.flagNaoAssociado == flagNaoAssociado).ToList();
            }

            if (idsRemove != null && idsRemove.Length > 0) {
                listaItens = listaItens.Where(x => !idsRemove.Contains(x.id)).ToList();
            }
            
            var itens = listaItens.OrderBy(x => x.nomeDisplay)
                            .Select(x => new { x.id, descricao = x.Categoria?.descricao.isEmpty() == true ? x.descricao ?? x.nomeDisplay : x.descricao ?? x.nomeDisplay + " (" + x.Categoria?.descricao + ")"}).ToList();
            
            return new SelectList(itens, "id", "descricao", selected);
        }

        //Carregar combo de seleção dos tipos de associados
        public MultiSelectList multiSelectList(List<int> selected, string flagTipoPessoa = "", bool? flagNaoAssociado = false, bool flagCache = true, int?[] idsRemove = null, int? idOrganizacaoParam = null) {

            var listaItens = CacheService.getInstance.carregar<List<TipoAssociado>>(CacheService.TIPO_ASSOCIADO_DD_SIMPLES);

            if (listaItens == null || !flagCache){

                listaItens = OTipoAssociadoBL.listar("", null, "S", idOrganizacaoParam).Select(x => new {
                    x.id, x.descricao, x.nomeDisplay, x.flagPessoaFisica, x.flagPessoaJuridica, x.flagNaoAssociado, x.idCategoria,
                    Categoria = new { x.Categoria.descricao }
                }).ToListJsonObject<TipoAssociado>();

                if (flagCache) {
                    CacheService.getInstance.remover(CacheService.TIPO_ASSOCIADO_DD_SIMPLES);

                    CacheService.getInstance.adicionar(CacheService.TIPO_ASSOCIADO_DD_SIMPLES, listaItens);
                }
            }
            
            if (flagTipoPessoa == "F") {
                listaItens = listaItens.Where(x => x.flagPessoaFisica).ToList();
            }

            if (flagTipoPessoa == "J") {
                listaItens = listaItens.Where(x => x.flagPessoaJuridica).ToList();
            }

            if (flagNaoAssociado.HasValue) {
                listaItens = listaItens.Where(x => x.flagNaoAssociado == flagNaoAssociado).ToList();
            }

            if (idsRemove != null && idsRemove.Length > 0) {
                listaItens = listaItens.Where(x => !idsRemove.Contains(x.id)).ToList();
            }
            
            var itens = listaItens.OrderBy(x => x.nomeDisplay)
                            .Select(x => new { x.id, descricao = x.Categoria?.descricao.isEmpty() == true ? x.descricao ?? x.nomeDisplay : x.descricao ?? x.nomeDisplay + " (" + x.Categoria?.descricao + ")"}).ToList();
            
            return new MultiSelectList(itens, "id", "descricao", selected);
        }

        /// <summary>
        /// Carregar combo com tipos de associados físicos
        /// </summary>
        public SelectList selectListDependente(int? selected) {


            var query = OTipoAssociadoBL.listar("", null, "S").Where(x => x.flagDependente == true);

            var listaTipos = query.Select(x => new { x.id, x.nomeDisplay, x.descricao}).ToList()
                                .Select(x => new { value = x.id, text = x.nomeDisplay.isEmpty() ? x.descricao : x.nomeDisplay })
                                .OrderBy(x => x.text).ToList();

            return new SelectList(listaTipos, "value", "text", selected);
        }

        /// <summary>
        /// Carregar combo com tipos de associados físicos
        /// </summary>
        public MultiSelectList multiSelectListDependente(List<int> selected, int? idOrganizacaoParam = null) {
            
            var query = OTipoAssociadoBL.listar("", null, "S", idOrganizacaoParam).Where(x => x.flagDependente == true);

            var listaTipos = query.Select(x => new { x.id, x.nomeDisplay, x.descricao}).ToList()
                                .Select(x => new { value = x.id, text = x.nomeDisplay.isEmpty() ? x.descricao : x.nomeDisplay })
                                .OrderBy(x => x.text).ToList();

            return new MultiSelectList(listaTipos, "value", "text", selected);
        }


        /// <summary>
        /// Carregar combo com tipos de associados físicos
        /// </summary>
        public SelectList selectListEstudante(int? selected) {


            var query = OTipoAssociadoBL.listar("", null, "S").Where(x => x.flagEstudante == true  && x.flagDependente != true);

            var listaTipos = query.Select(x => new { x.id, x.nomeDisplay, x.descricao}).ToList()
                                .Select(x => new { value = x.id, text = x.nomeDisplay.isEmpty() ? x.descricao : x.nomeDisplay })
                                .OrderBy(x => x.text).ToList();

            return new SelectList(listaTipos, "value", "text", selected);
        }
    }
}