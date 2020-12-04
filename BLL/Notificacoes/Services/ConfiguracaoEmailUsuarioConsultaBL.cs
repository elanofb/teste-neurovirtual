using System.Linq;
using BLL.Caches;
using BLL.Services;
using DAL.Permissao.Security.Extensions;
using DAL.Notificacoes;

namespace BLL.Notificacoes {

    public class ConfiguracaoEmailUsuarioConsultaBL : DefaultBL, IConfiguracaoEmailUsuarioConsultaBL {
        
        //Constantes
        private static ConfiguracaoEmailUsuarioConsultaBL _instance;

        //Propriedades
        private string chaveCache = "configuracao_email_usuario";

        public static ConfiguracaoEmailUsuarioConsultaBL getInstance => _instance = _instance ?? new ConfiguracaoEmailUsuarioConsultaBL();

        /// <summary>
        /// Carregar as configurações de email usuario e cachear os dados se necessário
        /// </summary>
        public ConfiguracaoEmailUsuario carregar(int idOrganizacaoParam = 0, int idUsuarioParam = 0, bool flagCache = true) {
    
            if (idOrganizacaoParam == 0) {

                idOrganizacaoParam = User.idOrganizacao();

            }
            
            if (idUsuarioParam == 0) {

                idUsuarioParam = User.id();

            }

            if (flagCache) {

                var cacheData = CacheService.getInstance.carregar(chaveCache, idOrganizacaoParam);

                if (cacheData != null) {
                    return (ConfiguracaoEmailUsuario)cacheData;
                }
                
            }

            var query = db.ConfiguracaoEmailUsuario.AsNoTracking().Where(x => x.dtExclusao == null);

            if (idOrganizacaoParam > 0) {

                query = query.Where(x => x.idOrganizacao == idOrganizacaoParam);

            } else {
                
                query = query.Where(x => x.idOrganizacao == null);

            }

            if (idUsuarioParam > 0) {

                query = query.Where(x => x.idUsuario == idUsuarioParam);

            } else {
                
                query = query.Where(x => x.idUsuario == null);

            }

            var OConfiguracao = query.OrderByDescending(x => x.id).FirstOrDefault() ?? new ConfiguracaoEmailUsuario();

            if (flagCache && OConfiguracao.id > 0) {

                CacheService.getInstance.remover(chaveCache, idOrganizacaoParam);

                CacheService.getInstance.adicionar(chaveCache, OConfiguracao, idOrganizacaoParam);
            }

            return OConfiguracao;
        }


        //Configurações email usuario
        public IQueryable<ConfiguracaoEmailUsuario> listar(int idOrganizacaoParam, int idUsuarioParam) {

            if (idOrganizacaoParam == 0) {

                idOrganizacaoParam = User.idOrganizacao();

            }
            
            if (idUsuarioParam == 0) {

                idUsuarioParam = User.id();

            }

            var query = db.ConfiguracaoEmailUsuario
                            .AsNoTracking().Where(x => x.dtExclusao == null);

            if (idOrganizacaoParam > 0) {

                query = query.Where(x => x.idOrganizacao == idOrganizacaoParam);
            }

            if (idUsuarioParam > 0) {

                query = query.Where(x => x.idUsuario == idUsuarioParam);
            }

            return query;
        }

    }
}