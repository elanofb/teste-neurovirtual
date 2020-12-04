using System;
using System.Json;
using System.Linq;
using BLL.Services;
using DAL.Produtos;
using UTIL.Resources;

namespace BLL.Produtos {

    public class ProdutoItemCadastroBL : DefaultBL, IProdutoItemCadastroBL {
        
        //Verificar se deve-se atualizar um registro existente ou criar um novo
        public bool salvar(ProdutoItem OProdutoItem) {
            
            var flagSucesso = false;

            OProdutoItem.UnidadeMedida = null;
            
            if (OProdutoItem.id > 0) {
                flagSucesso = this.atualizar(OProdutoItem);
            }

            if (OProdutoItem.id == 0) {
                flagSucesso = this.inserir(OProdutoItem);
            }

            return flagSucesso;
        }

        //Persistir o objecto e salvar na base de dados
        private bool inserir(ProdutoItem OProdutoItem) {
            
            OProdutoItem.setDefaultInsertValues();

            db.ProdutoItem.Add(OProdutoItem);

            db.SaveChanges();

            return (OProdutoItem.id > 0);
        }

        //Persistir o objecto e atualizar informações
        private bool atualizar(ProdutoItem OProdutoItem) {

            //Localizar existentes no banco
            var dbProdutoItem = db.ProdutoItem.condicoesSeguranca().FirstOrDefault(x => x.id == OProdutoItem.id);

            if (dbProdutoItem == null) {
                return false;
            }

            var dbEntry = db.Entry(dbProdutoItem);

            OProdutoItem.setDefaultUpdateValues();
            
            dbEntry.CurrentValues.SetValues(OProdutoItem);

            dbEntry.ignoreFields();

            db.SaveChanges();

            return (OProdutoItem.id > 0);

        }

        //Alteracao de status
        public JsonMessageStatus alterarStatus(int id) {
            var retorno = new JsonMessageStatus();

            var item = db.ProdutoItem.condicoesSeguranca().FirstOrDefault(x => x.id == id);

            if (item == null) {
                retorno.error = true;
                retorno.message = NotificationMessages.invalid_register_id;
            } else {
                item.ativo = (item.ativo != true);
                db.SaveChanges();
                retorno.active = item.ativo == true ? "S" : "N";
                retorno.message = NotificationMessages.updateSuccess;
            }
            return retorno;
        }
        
    }
}