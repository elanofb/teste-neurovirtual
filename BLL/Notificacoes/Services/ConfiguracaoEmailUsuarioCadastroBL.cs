using System;
using System.Linq;
using BLL.Caches;
using BLL.Services;
using DAL.Permissao.Security.Extensions;
using EntityFramework.Extensions;
using DAL.Notificacoes;

namespace BLL.Notificacoes {

    public class ConfiguracaoEmailUsuarioCadastroBL : DefaultBL, IConfiguracaoEmailUsuarioCadastroBL {

        //Propriedades
        private string chaveCache = "configuracao_email_usuario";

        /// <summary>
        /// Salvar configurações de email usuario e remover os registros anteriores.
        /// </summary>
        public bool salvar(ConfiguracaoEmailUsuario OConfiguracoes) {

            OConfiguracoes.setDefaultInsertValues();

            OConfiguracoes.idUsuario = User.id();

            db.ConfiguracaoEmailUsuario.Add(OConfiguracoes);

            db.SaveChanges();

            bool flagSucesso = OConfiguracoes.id > 0;

            if (flagSucesso) {

                int? idOrganizacaoParam = OConfiguracoes.idOrganizacao;
                int? idUsuarioParam = OConfiguracoes.idUsuario;

                db.ConfiguracaoEmailUsuario.Where(x => x.dtExclusao == null && x.idOrganizacao == idOrganizacaoParam && x.idUsuario == idUsuarioParam && x.id != OConfiguracoes.id)
                                    .Update(x => new ConfiguracaoEmailUsuario { dtExclusao = DateTime.Now, idUsuarioExclusao = User.id()});

                CacheService.getInstance.remover(chaveCache, idOrganizacaoParam.toInt());
            }

            return flagSucesso;
        }
    }
}