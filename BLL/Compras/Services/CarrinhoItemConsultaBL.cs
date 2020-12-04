using System;
using System.Linq;
using BLL.Services;
using DAL.Compras;

namespace BLL.Compras {

    public class CarrinhoItemConsultaBL : DefaultBL, ICarrinhoItemConsultaBL {

        //Atributos

        //Propriedades

        //Carregar registro
        public IQueryable<CarrinhoItem> listar(int idOrganizacaoParam, int idPessoa, string idSessao, bool? flagComprado = false) {

            var query = from Item in db.CarrinhoItem
                        where
                            Item.dtExclusao == null &&
                            Item.idOrganizacao == idOrganizacaoParam
                        select Item;

            if (idPessoa > 0) {

                query = query.Where(x => x.idPessoa == idPessoa);

            }

            if (!idSessao.isEmpty()) {

                query = query.Where(x => x.idSessao == idSessao);

            }

            if (flagComprado == true) {

                query = query.Where(x => x.flagComprado == true);

            }

            if (flagComprado == false) {

                query = query.Where(x => x.flagComprado == false);
            }

            return query;
        }

        //Carregar registro
        public IQueryable<CarrinhoItemProdutoVW> listarResumo(int idOrganizacaoParam, int idPessoa, string idSessao, bool? flagComprado = false) {

            var query = from Item in db.CarrinhoItemProdutoVW
                        where
                            Item.idOrganizacao == idOrganizacaoParam
                        select Item;

            if (idPessoa > 0) {

                query = query.Where(x => x.idPessoa == idPessoa);

            }

            if (!idSessao.isEmpty()) {

                query = query.Where(x => x.idSessao == idSessao);

            }

            if (flagComprado == true) {

                query = query.Where(x => x.flagComprado == true);

            }

            if (flagComprado == false) {

                query = query.Where(x => x.flagComprado == false);
            }

            return query;
        }

    }
}
