using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using BLL.Caches;
using BLL.Services;
using DAL.Configuracoes;
using DAL.Permissao.Security.Extensions;
using EntityFramework.Extensions;

namespace BLL.Configuracoes {

    public class ConfiguracaoComissaoBL : DefaultBL, IConfiguracaoComissaoBL {

        //Constantes
        private static ConfiguracaoComissaoBL _instance;

        //Propriedades
        public static ConfiguracaoComissaoBL getInstance => _instance = _instance ?? new ConfiguracaoComissaoBL();

        /// <summary>
        /// Carregar as configurações de sistema da transportadora e cachear os dados se necessário
        /// </summary>
        public ConfiguracaoComissao carregar(int idOrganizacaoInf = 0, bool flagCache = true) {

            if (idOrganizacao > 0) {
                idOrganizacaoInf = User.idOrganizacao();
            }

            if (flagCache) {
                var cacheData = CacheService.getInstance.carregar("configuracao_comissao");

                if (cacheData != null) {
                    return (ConfiguracaoComissao)cacheData;
                }
            }

            var query = db.ConfiguracaoComissao
                            .AsNoTracking().Where(x => x.dtExclusao == null).Include(x => x.listaPerfisComissionaveis);

            query = idOrganizacaoInf > 0 ? query.Where(x => x.idOrganizacao == idOrganizacaoInf) : query.Where(x => x.idOrganizacao == null);

            var OConfiguracao = query.OrderByDescending(x => x.id).FirstOrDefault() ?? new ConfiguracaoComissao();

            if (flagCache) {
                CacheService.getInstance.adicionar("configuracao_comissao", OConfiguracao);
            }
            return OConfiguracao;
        }

        //Configurações gerais
        public IQueryable<ConfiguracaoComissao> listar(int idOrganizacaoInf) {

            if (idOrganizacao > 0) {
                idOrganizacaoInf = User.idOrganizacao();
            }

            var query = db.ConfiguracaoComissao
                            .AsNoTracking().Where(x => x.dtExclusao == null).Include(x => x.Organizacao).Include(x => x.listaPerfisComissionaveis);

            if (idOrganizacaoInf > 0) {
                query = query.Where(x => x.idOrganizacao == idOrganizacaoInf);
            }
            return query;
        }

        /// <summary>
        /// Salvar configurações de serviços e remover  os registros anteriores.
        /// </summary>
        public bool salvar(ConfiguracaoComissao OConfiguracoes) {

            var idConfiguracaoBase = OConfiguracoes.id > 0 ? OConfiguracoes.id : 0;

            OConfiguracoes.id = 0;
            OConfiguracoes.setDefaultInsertValues();

            db.ConfiguracaoComissao.Add(OConfiguracoes);
            db.SaveChanges();

            bool flagSucesso = OConfiguracoes.id > 0;

            if (flagSucesso) {
                this.removeOldConfiguracao(OConfiguracoes, idConfiguracaoBase);

                CacheService.getInstance.remover("configuracao_comissao");
            }
            return flagSucesso;
        }

        private void removeOldConfiguracao(ConfiguracaoComissao OConfiguracoes, int idConfiguracaoBase = 0) {
            int idUsuarioLogado = User.id();
            
            using (var ctx = db) {

                ctx.Configuration.AutoDetectChangesEnabled = false;
                ctx.Configuration.ValidateOnSaveEnabled = false;

                var OConfiguracaoOld = ctx.ConfiguracaoComissao.FirstOrDefault(x => x.dtExclusao == null && x.idOrganizacao == OConfiguracoes.idOrganizacao && x.id != OConfiguracoes.id);

                if (OConfiguracaoOld != null && OConfiguracaoOld.idOrganizacao == OConfiguracoes.idOrganizacao) {
                    ctx.ConfiguracaoComissao.Where(x => x.id == OConfiguracaoOld.id).Update(x => new ConfiguracaoComissao {dtExclusao = DateTime.Now, idUsuarioExclusao = idUsuarioLogado});
                }

                idConfiguracaoBase = OConfiguracaoOld?.id > 0 && idConfiguracaoBase == 0 ? OConfiguracaoOld.id : idConfiguracaoBase;
                
                var listaPerfisComissionaveisOld = ctx.ConfiguracaoPerfilComissionavel.Where(x => x.dtExclusao == null && x.idConfiguracaoComissao == idConfiguracaoBase).ToList();
                if (!listaPerfisComissionaveisOld.Any()) {
                    return;
                }

                var listaPerfisComissionaveis = new List<ConfiguracaoPerfilComissionavel>();
                foreach (var Item in listaPerfisComissionaveisOld) {
                    var OPerfilComissionavel = new ConfiguracaoPerfilComissionavel();
                    OPerfilComissionavel.idConfiguracaoComissao = OConfiguracoes.id;
                    OPerfilComissionavel.idPerfilAcesso = Item.idPerfilAcesso;
                    OPerfilComissionavel.setDefaultInsertValues();
                    listaPerfisComissionaveis.Add(OPerfilComissionavel);
                }

                ctx.ConfiguracaoPerfilComissionavel.AddRange(listaPerfisComissionaveis);

                ctx.SaveChanges();
            }
        }
    }
}