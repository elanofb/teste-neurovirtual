using System;
using System.Json;
using System.Linq;
using BLL.Services;
using DAL.Produtos;
using UTIL.Resources;

namespace BLL.Produtos {

    public class ProdutoSituacaoCadastroBL : DefaultBL, IProdutoSituacaoCadastroBL {
        
        //Verificar se deve-se atualizar um registro existente ou criar um novo
        public bool salvar(ProdutoSituacao OProdutoSituacao) {
            
            var flagSucesso = false;

            //OProdutoSituacao.UnidadeMedida = null;
            
            if (OProdutoSituacao.id > 0) {
                flagSucesso = this.atualizar(OProdutoSituacao);
            }

            if (OProdutoSituacao.id == 0) {
                flagSucesso = this.inserir(OProdutoSituacao);
            }

            return flagSucesso;
        }

        //Persistir o objecto e salvar na base de dados
        private bool inserir(ProdutoSituacao OProdutoSituacao) {
            
            OProdutoSituacao.setDefaultInsertValues();

            db.ProdutoSituacao.Add(OProdutoSituacao);

            db.SaveChanges();

            return (OProdutoSituacao.id > 0);
        }

        //Persistir o objecto e atualizar informações
        private bool atualizar(ProdutoSituacao OProdutoSituacao) {

            //Localizar existentes no banco
            var dbProdutoSituacao = db.ProdutoSituacao.condicoesSeguranca().FirstOrDefault(x => x.id == OProdutoSituacao.id);

            if (dbProdutoSituacao == null) {
                return false;
            }

            var dbEntry = db.Entry(dbProdutoSituacao);

            OProdutoSituacao.setDefaultUpdateValues();
            
            dbEntry.CurrentValues.SetValues(OProdutoSituacao);

            dbEntry.ignoreFields();

            db.SaveChanges();

            return (OProdutoSituacao.id > 0);

        }

        //Alteracao de status
        public JsonMessageStatus alterarStatus(int id) {
            var retorno = new JsonMessageStatus();

            var item = db.ProdutoSituacao.condicoesSeguranca().FirstOrDefault(x => x.id == id);

            //if (item == null) {
            //    retorno.error = true;
            //    retorno.message = NotificationMessages.invalid_register_id;
            //} else {
            //    item.ativo = (item.ativo != true);
            //    db.SaveChanges();
            //    retorno.active = item.ativo == true ? "S" : "N";
            //    retorno.message = NotificationMessages.updateSuccess;
            //}
            return retorno;
        }
        
    }
}