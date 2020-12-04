using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using BLL.Caches;
using BLL.Services;
using DAL.ConfiguracoesAssociados;
using DAL.Permissao.Security.Extensions;
using BLL.Configuracoes.Services;
using EntityFramework.Extensions;
using Newtonsoft.Json;

namespace BLL.ConfiguracoesAssociados {

    public class ConfiguracaoAssociadoCampoBL : DefaultBL, IConfiguracaoAssociadoCampoBL {

        //Propriedades
        private readonly string chaveCache = "lista_campos_associado";

        /// <summary>
        /// Carregar as configurações de Associados PF da organização e cachear os dados se necessário
        /// </summary>
        public ConfiguracaoAssociadoCampo carregar(int id, int? idOrganizacaoParam = null) {

            if (idOrganizacao > 0 && idOrganizacaoParam == null) {
                idOrganizacaoParam = idOrganizacao;
            }

            var query = db.ConfiguracaoAssociadoCampo
                            .Include(x => x.Organizacao)
                            .Include(x => x.listaCampoOpcoes)
                            .Include(x => x.listaCampoPropriedades)
                            .Include(x => x.listaTipoAssociado)
                            .Where(x => x.id == id && x.dtExclusao == null);

            if (idOrganizacaoParam > 0) {
                query = query.Where(x => x.idOrganizacao == idOrganizacaoParam);
            }

            if (idOrganizacaoParam == 0) {
                query = query.Where(x => x.idOrganizacao == null);
            }

            var OConfiguracao = query.FirstOrDefault();

            return OConfiguracao;
        }

        /// <summary>
        /// Carregar lista de campos do associado configurado de acordo com associação informada ou logada
        /// </summary>
        public IQueryable<ConfiguracaoAssociadoCampo> listar(int idGrupo, bool? ativo, int? idOrganizacaoParam = null) {

            if (idOrganizacaoParam.toInt() == 0) {

                idOrganizacaoParam = idOrganizacao;
            }

            var query = db.ConfiguracaoAssociadoCampo
                            .Include(x => x.Organizacao)
                            .Include(x => x.TipoCampo)
                            .Include(x => x.AssociadoCampoGrupo)
                            .Include(x => x.UsuarioCadastro)
                            .Include(x => x.listaCampoOpcoes)
                            .Include(x => x.listaCampoPropriedades)
                            .Where(x => x.dtExclusao == null)
                            .AsNoTracking();

            if (idOrganizacaoParam > 0) {
                query = query.Where(x => x.idOrganizacao == idOrganizacaoParam);
            }

            if (idOrganizacaoParam == 0) {
                query = query.Where(x => x.idOrganizacao == null);
            }

            if (idGrupo > 0) {
                query = query.Where(x => x.idAssociadoCampoGrupo == idGrupo);
            }

            if (ativo.HasValue) {
                query = query.Where(x => x.ativo == ativo);
            }

            return query;
        }

        /// <summary>
        /// Carregar as configurações de campos por organização e colocar em cache (se houver)
        /// </summary>
        public List<ConfiguracaoAssociadoCampo> listarFromCache(int idOrganizacaoParam, bool flagCache = true) {

            object jsonData = CacheService.getInstance.carregar(chaveCache, idOrganizacaoParam);

            if (jsonData != null && flagCache) {

                var cacheData = JsonConvert.DeserializeObject<List<ConfiguracaoAssociadoCampo>>(jsonData.ToString());

                return cacheData;
            }

            var query = this.listar(0, true, idOrganizacaoParam);

            var listaDb = query.AsNoTracking();

            var listaCampos = listaDb.Select(cam => new {
                cam.id,
                cam.label,
                cam.idAssociadoCampoGrupo,
                cam.ativo,
                cam.cssClassBox,
                cam.cssClassCampo,
                cam.flagAreaAdm,
                cam.flagAreaAssociado,
                cam.flagAssociadoPodeEditar,
                cam.flagCadastro,
                cam.flagDependente,
                cam.flagEdicao,
                cam.flagExibir,
                cam.flagExibirOptionVazio,
                cam.flagMultiSelect,
                cam.flagObrigatorio,
                cam.idTipoCampoCadastro,
                cam.htmlAfterBox,
                cam.htmlAposCampo,
                cam.idDOM,
                cam.name,
                cam.nameDescription,
                cam.nameHelper,
                cam.methodHelper,
                cam.parametrosHelper,
                cam.valorPadrao,
                cam.valorFixo,
                cam.maxlength,
                cam.minlength,
                cam.mask,
                cam.textoInstrucoes,
                cam.idTipoCampo,
                cam.idFuncaoFiltro,
                cam.ordemExibicao,
                cam.dtExclusao,
                AssociadoCampoGrupo = new {
                    cam.AssociadoCampoGrupo.ordemExibicao,
                },
                TipoCampo = new {
                    cam.TipoCampo.id,
                    cam.TipoCampo.descricao,
                    cam.TipoCampo.flagOpcoes,
                    cam.TipoCampo.tipo
                },
                listaTipoAssociado = cam.listaTipoAssociado.Where(item => item.dtExclusao == null).Select(prop => new {
                    prop.id, 
                    prop.idTipoAssociado,
                    prop.idConfiguracaoAssociadoCampo
                }),
                listaCampoPropriedades = cam.listaCampoPropriedades.Where(item => item.dtExclusao == null).Select(prop => new {
                    prop.id,
                    prop.idConfiguracaoAssociadoCampo,
                    prop.nome,
                    prop.valor
                }),
                listaCampoOpcoes = cam.listaCampoOpcoes.Where(item => item.dtExclusao == null).Select(op => new {
                    op.id,
                    op.idConfiguracaoAssociadoCampo,
                    op.value,
                    op.texto
                })
            }).ToListJsonObject<ConfiguracaoAssociadoCampo>();

            if (flagCache) {

                CacheService.getInstance.remover(chaveCache, idOrganizacaoParam);

                var json = JsonConvert.SerializeObject(listaCampos);

                CacheService.getInstance.adicionar(chaveCache, json, idOrganizacaoParam);

            }

            return listaCampos;
        }

        /// <summary>
        /// Carregar as configuracoes de campos por organizacao e colocar em cache (se houver)
        /// </summary>
        public List<ConfiguracaoAssociadoCampo> listarFromCacheOrDefault(int idOrganizacaoParam, bool flagCache = true) {

            var listaCache = this.listarFromCache(idOrganizacaoParam, flagCache);

            if (listaCache.Any()) {

                return listaCache;

            }

            var listaGrupo = ConfiguracaoJsonBL.getInstance.carregar<List<ConfiguracaoAssociadoCampoGrupo>>(ConfiguracaoJsonBL.CADASTRO_ASSOCIADO_CAMPOS);

            if (listaGrupo == null) {

                return new List<ConfiguracaoAssociadoCampo>();

            }

            var listaCampos = this.listarFromDefault();

            return listaCampos;

        }

        /// <summary>
        /// Listagem de itens a partir do arquivo de configuração padrão
        /// </summary>
        public List<ConfiguracaoAssociadoCampo> listarFromDefault(){

            var listaGrupo = ConfiguracaoJsonBL.getInstance.carregar<List<ConfiguracaoAssociadoCampoGrupo>>(ConfiguracaoJsonBL.CADASTRO_ASSOCIADO_CAMPOS);

            if (listaGrupo == null) {

                return new List<ConfiguracaoAssociadoCampo>();

            }

            var listaCampos = listaGrupo.SelectMany(x => x.listaConfiguracaoAssociadoCampos).ToList();

            return listaCampos;
            
        }


        /// <summary>
        /// Salvar um novo registro o atualizar
        /// </summary>
        public bool salvar(ConfiguracaoAssociadoCampo OConfiguracao) {

            CacheService.getInstance.remover(chaveCache);

            OConfiguracao.label = OConfiguracao.label.abreviar(50);

            OConfiguracao.cssClassBox = OConfiguracao.cssClassBox.abreviar(50);

            OConfiguracao.cssClassCampo = OConfiguracao.cssClassCampo.abreviar(50);

            OConfiguracao.mask = OConfiguracao.mask.abreviar(20);

            OConfiguracao.name = OConfiguracao.name.abreviar(100);

            OConfiguracao.idDOM = OConfiguracao.idDOM.abreviar(50);

            OConfiguracao.textoInstrucoes = OConfiguracao.textoInstrucoes.abreviar(100);

            OConfiguracao.htmlAfterBox = OConfiguracao.htmlAfterBox.abreviar(255);

            OConfiguracao.mensagemErro = OConfiguracao.mensagemErro.abreviar(100);

            OConfiguracao.nameHelper = OConfiguracao.nameHelper.abreviar(255);

            OConfiguracao.methodHelper = OConfiguracao.methodHelper.abreviar(100);

            OConfiguracao.parametrosHelper = OConfiguracao.parametrosHelper.abreviar(100);

            if (OConfiguracao.id == 0) {
                return this.inserir(OConfiguracao);
            }

            return this.atualizar(OConfiguracao);
        }

        /// <summary>
        /// Inserir um novo registro
        /// </summary>
        private bool inserir(ConfiguracaoAssociadoCampo OConfiguracao) {

            foreach (var idTipoAssociado in OConfiguracao.idsTipoAssociado) {
                OConfiguracao.listaTipoAssociado.Add(new ConfiguracaoAssociadoCampoTipoAssociado() {idTipoAssociado = idTipoAssociado});
            }

            OConfiguracao.setDefaultInsertValues();

            db.ConfiguracaoAssociadoCampo.Add(OConfiguracao);

            db.SaveChanges();

            CacheService.getInstance.remover(chaveCache);

            return OConfiguracao.id > 0;
        }

        /// <summary>
        /// Atualizar os dados de um registro existente
        /// </summary>
        private bool atualizar(ConfiguracaoAssociadoCampo OConfiguracao) {

            ConfiguracaoAssociadoCampo dbRegistro = this.carregar(OConfiguracao.id);
            if (dbRegistro == null) {
                return false;
            }

            var TipoEntry = db.Entry(dbRegistro);

            OConfiguracao.setDefaultUpdateValues();

            TipoEntry.CurrentValues.SetValues(OConfiguracao);

            TipoEntry.State = EntityState.Modified;

            TipoEntry.ignoreFields();

            db.SaveChanges();

            this.atualizarTipoAssociados(dbRegistro, OConfiguracao.idsTipoAssociado);

            CacheService.getInstance.remover(chaveCache);

            return OConfiguracao.id > 0;
        }

        /// <summary>
        /// Remover os relacionamentos e adicionar novamente os registros
        /// </summary>
        private void atualizarTipoAssociados(ConfiguracaoAssociadoCampo OConfiguracao, List<int> idsTipoAssociado) {

            var idsTipoAssociadoDb = OConfiguracao.listaTipoAssociado.Where(x => x.dtExclusao == null).Select(x => x.idTipoAssociado).ToList();

            var idsTipoAssociadoAdd = idsTipoAssociado.Where(x => !idsTipoAssociadoDb.Contains(x)).ToList();

            var idsTipoAssociadoDel = idsTipoAssociadoDb.Where(x => !idsTipoAssociado.Contains(x)).ToList();

            if (idsTipoAssociadoDel.Any()) {
                var idUser = User.id();

                db.ConfiguracaoAssociadoCampoTipoAssociado.Where(x => idsTipoAssociadoDel.Contains(x.idTipoAssociado) && x.idConfiguracaoAssociadoCampo == OConfiguracao.id).Update(x => new ConfiguracaoAssociadoCampoTipoAssociado() {
                    dtExclusao = DateTime.Now,
                    idUsuarioExclusao = idUser 
                    
                });
            }

            if (idsTipoAssociadoAdd.Any()) {

                var listaTipoAssociado = new List<ConfiguracaoAssociadoCampoTipoAssociado>();

                foreach (var idTipoAssociado in idsTipoAssociadoAdd){
                    listaTipoAssociado.Add(new ConfiguracaoAssociadoCampoTipoAssociado() { idTipoAssociado = idTipoAssociado, idConfiguracaoAssociadoCampo = OConfiguracao.id});
                }

                listaTipoAssociado.ForEach(Item => Item.setDefaultInsertValues());
                
                db.ConfiguracaoAssociadoCampoTipoAssociado.AddRange(listaTipoAssociado);
                db.SaveChanges();
            }
        }

    }
}