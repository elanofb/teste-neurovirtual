using System;
using System.Data.Entity;
using System.Linq;
using BLL.Caches;
using BLL.Services;
using DAL.Configuracoes;
using DAL.Permissao.Security.Extensions;
using EntityFramework.Extensions;

namespace BLL.Configuracoes {

    public class ConfiguracaoSaqueBL : DefaultBL, IConfiguracaoSaqueBL {

        //Constantes
        private static ConfiguracaoSaqueBL _instance;
        
        //Propriedades
        private string chaveCache = "configuracao_saque";
        
        public static ConfiguracaoSaqueBL getInstance => _instance = _instance ?? new ConfiguracaoSaqueBL();
                
        /// <summary>
        /// 
        /// </summary>
        public ConfiguracaoSaque carregar(int idOrganizacaoParam = 0, int idTipoCadastro = 0) {
            
            if (idOrganizacaoParam == 0) {

                idOrganizacaoParam = User.idOrganizacao();
            }
                        
            var query = db.ConfiguracaoSaque.AsNoTracking().Where(x => x.dtExclusao == null);
                    
            query = idOrganizacaoParam > 0 ? query.Where(x => x.idOrganizacao == idOrganizacaoParam) : query.Where(x => x.idOrganizacao == null);
            
            query = idTipoCadastro > 0 ? query.Where(x => x.idTipoCadastro == idTipoCadastro) : query.Where(x => x.idTipoCadastro == null);
            
            var OConfiguracao = query.Include(x => x.UsuarioSistema).OrderByDescending(x => x.id).FirstOrDefault() ?? new ConfiguracaoSaque();           
            
            return OConfiguracao;
            
        }
        
        public IQueryable<ConfiguracaoSaque> listar(int idOrganizacaoInfo) {
            
            var query = db.ConfiguracaoSaque
                            .AsNoTracking().Where(x => x.dtExclusao == null);
                
            if (idOrganizacaoInfo > 0) {
            
                query = query.Where(x => x.idOrganizacao == idOrganizacaoInfo);

            }

            return query;
        }
    
        
        public bool salvar(ConfiguracaoSaque OConfiguracoes) {
            
            OConfiguracoes.setDefaultInsertValues();

            db.ConfiguracaoSaque.Add(OConfiguracoes);

            db.SaveChanges();
            
            bool flagSucesso = OConfiguracoes.id > 0;

            if (flagSucesso) {

                int? idOrganizacaoInfo = OConfiguracoes.idOrganizacao;

                int? idTipoCadastro = OConfiguracoes.idTipoCadastro;
                
                db.ConfiguracaoSaque
                    .Where(x => x.dtExclusao == null && x.idOrganizacao == idOrganizacaoInfo && x.idTipoCadastro == idTipoCadastro && x.id != OConfiguracoes.id)
                    .Update(x => new ConfiguracaoSaque { dtExclusao = DateTime.Now, idUsuarioExclusao =  User.id() });                                
                
            }
            
            return flagSucesso;
        }
    }
}