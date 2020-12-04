using System;
using BLL.Caches;
using DAL.Permissao.Security.Extensions;

namespace BLL.ConfiguracoesAssociados {

    public class ConfiguracaoAssociadoCampoExclusaoBL : ConfiguracaoAssociadoCampoBL, IConfiguracaoAssociadoCampoExclusaoBL {

        //Propriedades
        private readonly string chaveCache = "lista_campos_associado";

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

            CacheService.getInstance.remover(chaveCache);

            return UtilRetorno.newInstance(false, "O registro foi removido com sucesso.");
        }
    }
}