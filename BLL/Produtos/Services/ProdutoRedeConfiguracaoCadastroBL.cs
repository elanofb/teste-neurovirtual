using System;
using System.Json;
using System.Linq;
using BLL.Services;
using DAL.Produtos;
using UTIL.Resources;

namespace BLL.Produtos {

    public class ProdutoRedeConfiguracaoCadastroBL : DefaultBL, IProdutoRedeConfiguracaoCadastroBL {
        
        //Verificar se deve-se atualizar um registro existente ou criar um novo
        public bool salvar(ProdutoRedeConfiguracao OProdutoRede) {
            
            var flagSucesso = false;
            
            if (OProdutoRede.id > 0) {
                flagSucesso = this.atualizar(OProdutoRede);
            }

            if (OProdutoRede.id == 0) {
                flagSucesso = this.inserir(OProdutoRede);
            }

            return flagSucesso;
        }

        //Persistir o objecto e salvar na base de dados
        private bool inserir(ProdutoRedeConfiguracao OProdutoRede) {
            
            OProdutoRede.setDefaultInsertValues();

            db.ProdutoRedeConfiguracao.Add(OProdutoRede);

            db.SaveChanges();

            return (OProdutoRede.id > 0);
        }

        //Persistir o objecto e atualizar informações
        private bool atualizar(ProdutoRedeConfiguracao OProdutoRede) {

            //Localizar existentes no banco
            var dbProdutoComposicao = db.ProdutoRedeConfiguracao.condicoesSeguranca().FirstOrDefault(x => x.id == OProdutoRede.id);

            if (dbProdutoComposicao == null) {
                return false;
            }

            var dbEntry = db.Entry(dbProdutoComposicao);

            OProdutoRede.setDefaultUpdateValues();
            
            dbEntry.CurrentValues.SetValues(OProdutoRede);

            dbEntry.ignoreFields();

            db.SaveChanges();

            return (OProdutoRede.id > 0);

        }
        
    }
}