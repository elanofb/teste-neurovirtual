using System;
using System.Collections.Generic;
using System.Linq;
using BLL.Services;
using DAL.Permissao.Security.Extensions;
using DAL.Produtos;
using EntityFramework.Extensions;

namespace BLL.Produtos {

    public class ProdutoTipoAssociadoBL : DefaultBL, IProdutoTipoAssociadoBL {

        //Atributos

        //Propriedades

        //Construtor

        /// <summary>
        /// Montagem de query para consulta
        /// </summary>
        public IQueryable<ProdutoTipoAssociado> query(int idProduto) {

            var query = from Obj in db.ProdutoTipoAssociado
                        where Obj.dtExclusao == null
                        select Obj;


            if (idProduto > 0) {
                query = query.Where(x => x.idProduto == idProduto);
            }

            return query;

        }

        /// <summary>
        /// Carregamento a partir do ID 
        /// </summary>
        public ProdutoTipoAssociado carregar(int id) {

            var query = this.query(0).condicoesSeguranca();

            return query.FirstOrDefault(x => x.id == id);

        }

        /// <summary>
        /// Remover os vínculos existentes para o produto e cadastrar com os dados atualizados.
        /// </summary>
        public bool salvar(int idProduto, List<int> idsTipoAssociado) {

            this.query(idProduto)
                 .Where(x => x.dtExclusao == null && idProduto > 0)
                 .Update(x => new ProdutoTipoAssociado {
                     dtExclusao = DateTime.Now,
                     idUsuarioExclusao = User.id()
                 });

            foreach (var idTipoAssociado in idsTipoAssociado) {

                var Item = new ProdutoTipoAssociado();

                Item.idProduto = idProduto;

                Item.idTipoAssociado = idTipoAssociado;

                Item.setDefaultInsertValues();

                db.ProdutoTipoAssociado.Add(Item);
            }

            db.SaveChanges();

            return true;
        }


    }
}