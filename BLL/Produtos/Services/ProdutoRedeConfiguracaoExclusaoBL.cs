using System;
using System.Linq;
using BLL.Services;
using DAL.Permissao.Security.Extensions;

namespace BLL.Produtos {

    public class ProdutoRedeConfiguracaoExclusaoBL : DefaultBL, IProdutoRedeConfiguracaoExclusaoBL {
        
        //Remover o registro do sistema (exclusao lógico - não se remove fiscamente do banco de dados)
        public UtilRetorno excluir(int id) {

            var Objeto = db.ProdutoRedeConfiguracao.condicoesSeguranca().FirstOrDefault(x => x.id == id);

            if (Objeto == null) {
                return UtilRetorno.newInstance(true, "O registro informado não foi localizado.");
            }

            Objeto.dtExclusao = DateTime.Now;

            Objeto.idUsuarioExclusao = User.id();

            db.SaveChanges();

            return UtilRetorno.newInstance(false, "Registro removido com sucesso.");

        }

    }
}