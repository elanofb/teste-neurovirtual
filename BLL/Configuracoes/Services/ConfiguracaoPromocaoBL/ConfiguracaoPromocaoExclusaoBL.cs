using System;
using System.Json;
using System.Linq;

using BLL.Services;

using DAL.Configuracoes;
using DAL.Permissao.Security.Extensions;

using UTIL.Resources;

namespace BLL.Configuracoes.Interface.IConfiguracaoPromocaoBL {

    public class ConfiguracaoPromocaoExclusaoBL : DefaultBL, IConfiguracaoPromocaoExclusaoBL {
        public UtilRetorno excluir(int id) {
            var OldConf = db.ConfiguracaoPromocao.Find(id);

            if (OldConf == null) {
                return UtilRetorno.newInstance(true, NotificationMessages.invalid_register_id);
            }

            OldConf.idUsuarioExclusao = User.id();
            OldConf.dtExclusao        = DateTime.Now;

            db.SaveChanges();

            return UtilRetorno.newInstance(false, "Item removido com sucesso!");
        }

        public JsonMessageStatus alterarStatus(int id) {
            var Retorno = new JsonMessageStatus();

            var item = db.ConfiguracaoPromocao.Find(id);

            if (item == null) {
                Retorno.error   = true;
                Retorno.message = NotificationMessages.invalid_register_id;
            } else {
                item.ativo = !item.ativo;

                db.SaveChanges();

                Retorno.error   = false;
                Retorno.active  = item.ativo ? "S" : "N";
                Retorno.message = NotificationMessages.updateSuccess;
            }

            return Retorno;
        }
    }

}