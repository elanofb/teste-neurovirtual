using System;
using System.Data.Entity;
using System.Linq;
using BLL.Caches;
using BLL.Services;
using DAL.Configuracoes;
using DAL.Permissao.Security.Extensions;
using EntityFramework.Extensions;

namespace BLL.Configuracoes {

    public class ConfiguracaoOperacaoCompraBL : DefaultBL, IConfiguracaoOperacaoCompraBL {

        //Constantes
        private static ConfiguracaoOperacaoCompraBL _instance;
        
        //Propriedades
        private string chaveCache = "configuracao_operaca_compra";
        
        public static ConfiguracaoOperacaoCompraBL getInstance => _instance = _instance ?? new ConfiguracaoOperacaoCompraBL();
                
        /// <summary>
        /// 
        /// </summary>
        public ConfiguracaoOperacaoCompra carregar(int idOrganizacaoParam = 0, bool flagCache = true) {

            if (idOrganizacaoParam == 0) {

                idOrganizacaoParam = User.idOrganizacao();
            }

            if (flagCache) {
                var cacheData = CacheService.getInstance.carregar<ConfiguracaoOperacaoCompra>(chaveCache, idOrganizacaoParam);

                if (cacheData != null) {
                    return cacheData;
                }
            }
            
            var query = db.ConfiguracaoOperacaoCompra.AsNoTracking().Where(x => x.dtExclusao == null);
            
            if (idOrganizacaoParam > 0) {

                query = query.Where(x => x.idOrganizacao == idOrganizacaoParam);

            } else {
                query = query.Where(x => x.idOrganizacao == null);

            }
            
            var OConfiguracao = query.Include(x => x.UsuarioSistema).OrderByDescending(x => x.id).FirstOrDefault() ?? new ConfiguracaoOperacaoCompra();

            if (flagCache) {

                CacheService.getInstance.remover(chaveCache, idOrganizacaoParam);

                CacheService.getInstance.adicionar(chaveCache, OConfiguracao, idOrganizacaoParam);
            }
            
            return OConfiguracao;
            
        }

        
        public IQueryable<ConfiguracaoOperacaoCompra> listar(int idOrganizacaoInfo) {

            var query = db.ConfiguracaoOperacaoCompra
                                                  .AsNoTracking()
                                                  .Where(x => x.dtExclusao == null);

            if (idOrganizacaoInfo > 0) {
            
                query = query.Where(x => x.idOrganizacao == idOrganizacaoInfo);

            }

            return query;
        }
    
        
        public bool salvar(ConfiguracaoOperacaoCompra OConfiguracoes) {

            OConfiguracoes.setDefaultInsertValues();

            db.ConfiguracaoOperacaoCompra.Add(OConfiguracoes);

            db.SaveChanges();

            bool flagSucesso = OConfiguracoes.id > 0;

            if (flagSucesso) {

                int? idOrganizacaoInfo = OConfiguracoes.idOrganizacao;
                
                db.ConfiguracaoOperacaoCompra
                    .Where(x => x.dtExclusao == null && x.idOrganizacao == idOrganizacaoInfo && x.id != OConfiguracoes.id)
                    .Update(x => new ConfiguracaoOperacaoCompra { dtExclusao = DateTime.Now, idUsuarioExclusao =  User.id() });

                CacheService.getInstance.remover(chaveCache, idOrganizacaoInfo.toInt());
            }
            return flagSucesso;
        }
    }
}