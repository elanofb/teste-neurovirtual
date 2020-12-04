using System;
using System.Linq;
using BLL.Services;
using DAL.Compras;
using EntityFramework.Extensions;

namespace BLL.Compras {

    public class CarrinhoItemExclusaoBL : DefaultBL, ICarrinhoItemExclusaoBL {

        //Atributos

        //Propriedades

        /// <summary>
        /// Remover um item do carrinho a partir do ID
        /// </summary>
        public UtilRetorno excluir(int idOrganizacaoParam, string idSessao, int id) {

            var OItem = db.CarrinhoItem.FirstOrDefault(x => x.id == id && x.idOrganizacao == idOrganizacaoParam && x.idSessao == idSessao && x.dtExclusao == null && x.flagComprado == false);

            if (OItem == null) {
                return UtilRetorno.newInstance(true, "O registro informado não pôde ser encontrado.");
            }

            OItem.dtExclusao = DateTime.Now;

            db.SaveChanges();

            return UtilRetorno.newInstance(false, "Item removido com sucesso!");

        }

        /// <summary>
        /// Limpar o carrinho de compras do usuário
        /// </summary>
        public void excluirTudo(int idOrganizacaoParam, int? idPessoa, string idSessao) {

            var queryItens = db.CarrinhoItem.Where(x => x.idOrganizacao == idOrganizacaoParam && x.idSessao == idSessao && x.dtExclusao == null && x.flagComprado == false);

            if (idPessoa > 0) {
                queryItens = queryItens.Where(x => x.idPessoa == idPessoa);
            }

            queryItens.Update(x => new CarrinhoItem { dtExclusao = DateTime.Now });

        }
    }
}
