using System;
using System.Linq;
using BLL.Services;
using DAL.PedidosTemp;
using EntityFramework.Extensions;

namespace BLL.PedidosTemp {

    public class PedidoProdutoTempBL : DefaultBL, IPedidoProdutoTempBL {

        //
        public IQueryable<PedidoProdutoTemp> query() {

            var query = from Obj in this.db.PedidoProdutoTemp
                        select Obj;

            return query;

        }

        //
        public PedidoProdutoTemp carregar(int id) {

            var query = this.query().Where(x => x.id == id);
            
            return query.FirstOrDefault();

        }

        //
        public PedidoProdutoTemp listar(string idSessao) {

            var query = this.query().Where(x => x.PedidoTemp.idSessao == idSessao);
            
            return query.FirstOrDefault();

        }

        //
        public bool salvar(PedidoProdutoTemp OPedidoProdutoTemp) {
			
            if (OPedidoProdutoTemp.id == 0) {
                return this.inserir(OPedidoProdutoTemp);
            }
            
            return this.atualizar(OPedidoProdutoTemp);
            
        }

        //Persistir o objecto e salvar na base de dados
        private bool inserir(PedidoProdutoTemp OPedidoProdutoTemp) {
            
            OPedidoProdutoTemp.setDefaultInsertValues();

            db.PedidoProdutoTemp.Add(OPedidoProdutoTemp);

            db.SaveChanges();

            return (OPedidoProdutoTemp.id > 0);
        }

        //Persistir o objecto e atualizar informações
        private bool atualizar(PedidoProdutoTemp OPedidoProdutoTemp) {

            //Localizar existentes no banco
            var dbPedidoProdutoTemp = this.carregar(OPedidoProdutoTemp.id);

            if (dbPedidoProdutoTemp == null) {
                return false;
            }

            var dbEntry = db.Entry(dbPedidoProdutoTemp);

            OPedidoProdutoTemp.setDefaultUpdateValues();

            dbEntry.CurrentValues.SetValues(OPedidoProdutoTemp);

            dbEntry.ignoreFields();

            db.SaveChanges();

            return (OPedidoProdutoTemp.id > 0);

        }

        //
        public void excluir(int id) {

            var query = this.query();

            query.Where(x => x.id == id).Delete();
            
        }

    }

}
