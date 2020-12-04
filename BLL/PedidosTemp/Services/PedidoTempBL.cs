using System;
using System.Data.Entity;
using System.Linq;
using BLL.Services;
using DAL.PedidosTemp;
using EntityFramework.Extensions;

namespace BLL.PedidosTemp {

    public class PedidoTempBL : DefaultBL, IPedidoTempBL {

        //
        public IQueryable<PedidoTemp> query() {

            var query = from Obj in this.db.PedidoTemp
                        select Obj;

            return query;

        }

        //
        private PedidoTemp carregar(int id) {
            
            var query = this.query().Include(x => x.listaProdutos).Where(x => x.id == id);

            query = query.condicoesSeguranca();

            return query.OrderByDescending(x => x.id).FirstOrDefault();

        }

        //
        public PedidoTemp carregar(string idSessao) {
            
            var query = this.query().Include(x => x.listaProdutos).Where(x => x.idSessao == idSessao);

            query = query.condicoesSeguranca();

            return query.OrderByDescending(x => x.id).FirstOrDefault();

        }

        //
        public bool salvar(PedidoTemp OPedidoTemp) {
			
            OPedidoTemp.CupomDesconto = null;

            OPedidoTemp.Estado = null;

            OPedidoTemp.Cidade = null;
            
            if (OPedidoTemp.id == 0) {
                return this.inserir(OPedidoTemp);
            }
            
            return this.atualizar(OPedidoTemp);
            
        }

        //Persistir o objecto e salvar na base de dados
        private bool inserir(PedidoTemp OPedidoTemp) {
            
            OPedidoTemp.setDefaultInsertValues();

            db.PedidoTemp.Add(OPedidoTemp);

            db.SaveChanges();

            return (OPedidoTemp.id > 0);
        }

        //Persistir o objecto e atualizar informações
        private bool atualizar(PedidoTemp OPedidoTemp) {

            //Localizar existentes no banco
            var dbPedidoTemp = this.carregar(OPedidoTemp.id);

            if (dbPedidoTemp == null) {
                return false;
            }

            var dbEntry = db.Entry(dbPedidoTemp);

            OPedidoTemp.setDefaultUpdateValues();

            dbEntry.CurrentValues.SetValues(OPedidoTemp);

            dbEntry.ignoreFields();

            db.SaveChanges();

            return (OPedidoTemp.id > 0);

        }
        
    }

}
