using BLL.Services;
using DAL.Configuracoes;
using System;
using System.Linq;
using DAL.Permissao.Security.Extensions;

namespace BLL.Configuracoes {

    public class ConfiguracaoPerfilComissionavelBL : DefaultBL, IConfiguracaoPerfilComissionavelBL {


        //Carregamento de registro pelo ID
        public ConfiguracaoPerfilComissionavel carregar(int id) {
            
            return db.ConfiguracaoPerfilComissionavel.Find(id);
        }

        //Listagem de registros de acordo com filtros
        public IQueryable<ConfiguracaoPerfilComissionavel> listar(int idConfiguracaoComissao, int idPerfilAcesso) {
            
            var query = from P in db.ConfiguracaoPerfilComissionavel
                        where P.dtExclusao == null
                        select P;

            if (idConfiguracaoComissao > 0) {
                query = query.Where(x => x.idConfiguracaoComissao == idConfiguracaoComissao);
            }

            if (idPerfilAcesso > 0) {
                query = query.Where(x => x.idPerfilAcesso == idPerfilAcesso);
            }

            return query;
        }

        //Verificar se deve-se atualizar um registro existente ou criar um novo
        public bool salvar(ConfiguracaoPerfilComissionavel OConfiguracaoPerfilComissionavel) {

            if(OConfiguracaoPerfilComissionavel.id == 0) {
                return this.inserir(OConfiguracaoPerfilComissionavel);
            }

            return this.atualizar(OConfiguracaoPerfilComissionavel);
        }

        //Persistir o objecto e salvar na base de dados
        private bool inserir(ConfiguracaoPerfilComissionavel OConfiguracaoPerfilComissionavel) {
            
            OConfiguracaoPerfilComissionavel.setDefaultInsertValues();
            db.ConfiguracaoPerfilComissionavel.Add(OConfiguracaoPerfilComissionavel);
            db.SaveChanges();

            return (OConfiguracaoPerfilComissionavel.id > 0);
        }

        //Persistir o objecto e atualizar informações
        private bool atualizar(ConfiguracaoPerfilComissionavel OConfiguracaoPerfilComissionavel) {
            
            OConfiguracaoPerfilComissionavel.setDefaultUpdateValues();

            //Localizar existentes no ConfiguracaoPerfilComissionavel
            ConfiguracaoPerfilComissionavel dbConfiguracaoPerfilComissionavel = this.carregar(OConfiguracaoPerfilComissionavel.id);
            var TipoEntry = db.Entry(dbConfiguracaoPerfilComissionavel);
            TipoEntry.CurrentValues.SetValues(OConfiguracaoPerfilComissionavel);
            TipoEntry.ignoreFields();

            db.SaveChanges();
            return (OConfiguracaoPerfilComissionavel.id > 0);
        }


        //Remover um registro (exclusao lógica - nao remove-se fisicamente)
        public UtilRetorno excluir(int id) {
            
            var OConfiguracaoPerfilComissionavel = this.carregar(id);

		    if (OConfiguracaoPerfilComissionavel == null) {
		        return UtilRetorno.newInstance(true, "A configuração de perfil comissionável informado não foi localizado.");
		    }

            OConfiguracaoPerfilComissionavel.dtExclusao = DateTime.Now;

		    OConfiguracaoPerfilComissionavel.idUsuarioExclusao = User.id();

		    db.SaveChanges();

            return UtilRetorno.newInstance(false, "Registro removido com sucesso.");
            
        }
        
    }
}