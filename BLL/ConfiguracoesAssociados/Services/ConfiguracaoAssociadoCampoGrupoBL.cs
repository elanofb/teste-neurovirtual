using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using BLL.Caches;
using BLL.Configuracoes.Services;
using BLL.Services;
using DAL.ConfiguracoesAssociados;
using DAL.Permissao.Security.Extensions;
using EntityFramework.Extensions;

namespace BLL.ConfiguracoesAssociados {

    public class ConfiguracaoAssociadoCampoGrupoBL : DefaultBL, IConfiguracaoAssociadoCampoGrupoBL {

        //Propriedades
        private readonly string chaveCache = "lista_grupos_campos_associado";

        /// <summary>
        /// Carregar as configurações de Associados PF da organização e cachear os dados se necessário
        /// </summary>
        public ConfiguracaoAssociadoCampoGrupo carregar(int id, int? idOrganizacaoInf = null) {

            if (idOrganizacao > 0 && idOrganizacaoInf != 0) {
                idOrganizacaoInf = idOrganizacao;
            }

            var query = db.ConfiguracaoAssociadoCampoGrupo
                            .Include(x => x.Organizacao)
                            .Where(x => x.id == id && x.dtExclusao == null);

            if (idOrganizacaoInf > 0) {
                query = query.Where(x => x.idOrganizacao == idOrganizacaoInf);
            }

            if (idOrganizacaoInf == 0) {
                query = query.Where(x => x.idOrganizacao == null);
            }

            return query.FirstOrDefault();
        }

        //Configurações gerais
        public IQueryable<ConfiguracaoAssociadoCampoGrupo> listar(int? idOrganizacaoInf = null) {

            if (idOrganizacaoInf.toInt() == 0) {
                idOrganizacaoInf = idOrganizacao;
            }

            var query = db.ConfiguracaoAssociadoCampoGrupo
                            .Include(x => x.Organizacao)
                            .Include(x => x.UsuarioCadastro)
                            .Where(x => x.dtExclusao == null)
                            .AsNoTracking();

            if (idOrganizacaoInf > 0) {
                query = query.Where(x => x.idOrganizacao == idOrganizacaoInf);
            }

            if (idOrganizacaoInf == 0) {
                query = query.Where(x => x.idOrganizacao == null);
            }

            return query;
        }

        /// <summary>
        /// Carregar as configurações de campos por organização e colocar em cache (se houver)
        /// </summary>
        public List<ConfiguracaoAssociadoCampoGrupo> listarFromCache(int idOrganizacaoParam, bool flagCache = true) {

            List<ConfiguracaoAssociadoCampoGrupo> cacheData = CacheService.getInstance.carregar<List<ConfiguracaoAssociadoCampoGrupo>>(chaveCache, idOrganizacaoParam);

            if (cacheData != null && cacheData.Count > 0  && flagCache) {
                return cacheData;
            }

            var query = this.listar(idOrganizacaoParam);

            var listaGrupos = query.ToList();

            CacheService.getInstance.remover(chaveCache, idOrganizacaoParam);

            CacheService.getInstance.adicionar(chaveCache, listaGrupos, idOrganizacaoParam);

            return listaGrupos;
        }

        /// <summary>
        /// Carregar as configurações de campos por organização e colocar em cache (se houver)
        /// </summary>
        public List<ConfiguracaoAssociadoCampoGrupo> listarFromCacheOrDefault(int idOrganizacaoParam, bool flagCache = true) {

            var listaCache = this.listarFromCache(idOrganizacaoParam, flagCache);

            if (listaCache.Any()) {
                return listaCache;
            }

            var listaCampos = this.listarFromDefault();

            return listaCampos;
        }

        /// <summary>
        /// Carregar as configuracoes do arquivo padrao
        /// </summary>
        public List<ConfiguracaoAssociadoCampoGrupo> listarFromDefault(){

            var listaCampos = ConfiguracaoJsonBL.getInstance.carregar<List<ConfiguracaoAssociadoCampoGrupo>>(ConfiguracaoJsonBL.CADASTRO_ASSOCIADO_CAMPOS);

            if (listaCampos == null) {
                listaCampos = new List<ConfiguracaoAssociadoCampoGrupo>();
            }

            return listaCampos;
        }

        /// <summary>
        /// Salvar um novo registro o atualizar
        /// </summary>
        public bool salvar(ConfiguracaoAssociadoCampoGrupo OConfiguracao) {

            CacheService.getInstance.remover(chaveCache);

            OConfiguracao.htmlAposBox = OConfiguracao.htmlAposBox.abreviar(255);

            if (OConfiguracao.id == 0) {
                return this.inserir(OConfiguracao);
            }

            return this.atualizar(OConfiguracao);
        }


        /// <summary>
        /// Inserir um novo registro
        /// </summary>
        private bool inserir(ConfiguracaoAssociadoCampoGrupo OConfiguracao) {

            OConfiguracao.setDefaultInsertValues();

            db.ConfiguracaoAssociadoCampoGrupo.Add(OConfiguracao);

            db.SaveChanges();

            return OConfiguracao.id > 0;
        }

        /// <summary>
        /// Atualizar os dados de um registro existente
        /// </summary>
        private bool atualizar(ConfiguracaoAssociadoCampoGrupo OConfiguracao) {

            ConfiguracaoAssociadoCampoGrupo dbRegistro = this.carregar(OConfiguracao.id);
            if (dbRegistro == null) {
                return false;
            }

            var TipoEntry = db.Entry(dbRegistro);

            OConfiguracao.setDefaultUpdateValues();

            TipoEntry.CurrentValues.SetValues(OConfiguracao);

            TipoEntry.State = EntityState.Modified;

            TipoEntry.ignoreFields();

            db.SaveChanges();

            return OConfiguracao.id > 0;
        }

        /// <summary>
        /// Excluir um registro
        /// </summary>
        public UtilRetorno excluir(int id) {

            var Registro = this.carregar(id);

            if (Registro == null) {
                return UtilRetorno.newInstance(true, "O registro informado não pode ser removido.");
            }

            Registro.dtExclusao = DateTime.Now;

            Registro.idUsuarioExclusao = User.id();

            this.db.SaveChanges();
            
            db.ConfiguracaoAssociadoCampo.Where(x => x.idAssociadoCampoGrupo == Registro.id)
                .Update(x => new ConfiguracaoAssociadoCampo() {dtExclusao = DateTime.Now, idUsuarioExclusao = Registro.idUsuarioExclusao});

            CacheService.getInstance.remover(chaveCache, Registro.idOrganizacao.toInt());
            CacheService.getInstance.remover("lista_campos_associado", Registro.idOrganizacao.toInt());

            return UtilRetorno.newInstance(false, "O registro foi removido com sucesso.");
        }

        /// <summary>
        /// Reordenar a lista de grupos de campos
        /// </summary>
        public void reordenarExibicao(int idGrupo, byte ordem, int idTipoCampoCadastro) {

            var OGrupo = this.carregar(idGrupo);

            if (OGrupo == null || (idOrganizacao > 0 && OGrupo.idOrganizacao != idOrganizacao)) {
                return;
            }

            byte cont = 1;

            var query = db.ConfiguracaoAssociadoCampoGrupo
                .Where(x => x.idOrganizacao == OGrupo.idOrganizacao && x.dtExclusao == null && x.idTipoCampoCadastro == idTipoCampoCadastro);

            var itens = query.OrderBy(x => x.ordemExibicao ?? 100000).ToList();

            itens.ForEach(x => {
                if (x.id == OGrupo.id) {
                    x.ordemExibicao = ordem;
                } else {
                    if (cont == ordem)
                        cont++;
                    x.ordemExibicao = cont;
                    cont++;
                }
            });

            db.SaveChanges();

            CacheService.getInstance.remover(chaveCache);
        }
    }
}