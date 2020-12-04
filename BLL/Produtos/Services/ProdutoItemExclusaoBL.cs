using System;
using System.Linq;
using BLL.Services;
using DAL.Permissao.Security.Extensions;

namespace BLL.Produtos {

    public class ProdutoItemExclusaoBL : DefaultBL, IProdutoItemExclusaoBL {
        
        //Remover o registro do sistema (exclusao lógico - não se remove fiscamente do banco de dados)
        public UtilRetorno excluir(int id) {

            var Objeto = db.ProdutoItem.condicoesSeguranca().FirstOrDefault(x => x.id == id);

            if (Objeto == null) {
                return UtilRetorno.newInstance(true, "O registro informado não foi localizado.");
            }

            Objeto.flagExcluido = true;

            Objeto.dtAlteracao = DateTime.Now;

            Objeto.idUsuarioAlteracao = User.id();

            db.SaveChanges();

            return UtilRetorno.newInstance(false, "Registro removido com sucesso.");

        }

    }
}