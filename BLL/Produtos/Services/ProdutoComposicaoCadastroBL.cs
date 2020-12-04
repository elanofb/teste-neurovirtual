using System;
using System.Json;
using System.Linq;
using BLL.Services;
using DAL.Produtos;
using UTIL.Resources;

namespace BLL.Produtos {

    public class ProdutoComposicaoCadastroBL : DefaultBL, IProdutoComposicaoCadastroBL {
        
        //Verificar se deve-se atualizar um registro existente ou criar um novo
        public bool salvar(ProdutoComposicao OProdutoComposicao) {
            
            var flagSucesso = false;
            
            if (OProdutoComposicao.id > 0) {
                flagSucesso = this.atualizar(OProdutoComposicao);
            }

            if (OProdutoComposicao.id == 0) {
                flagSucesso = this.inserir(OProdutoComposicao);
            }

            return flagSucesso;
        }

        //Persistir o objecto e salvar na base de dados
        private bool inserir(ProdutoComposicao OProdutoComposicao) {
            
            OProdutoComposicao.setDefaultInsertValues();

            db.ProdutoComposicao.Add(OProdutoComposicao);

            db.SaveChanges();

            return (OProdutoComposicao.id > 0);
        }

        //Persistir o objecto e atualizar informações
        private bool atualizar(ProdutoComposicao OProdutoComposicao) {

            //Localizar existentes no banco
            var dbProdutoComposicao = db.ProdutoComposicao.condicoesSeguranca().FirstOrDefault(x => x.id == OProdutoComposicao.id);

            if (dbProdutoComposicao == null) {
                return false;
            }

            var dbEntry = db.Entry(dbProdutoComposicao);

            OProdutoComposicao.setDefaultUpdateValues();
            
            dbEntry.CurrentValues.SetValues(OProdutoComposicao);

            dbEntry.ignoreFields();

            db.SaveChanges();

            return (OProdutoComposicao.id > 0);

        }

        //Alteracao de status
        public JsonMessageStatus alterarStatus(int id) {
            var retorno = new JsonMessageStatus();

            var item = db.ProdutoComposicao.condicoesSeguranca().FirstOrDefault(x => x.id == id);

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