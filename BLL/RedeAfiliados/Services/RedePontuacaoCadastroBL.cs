using System;
using BLL.Services;
using DAL.RedeAfiliados;

namespace BLL.RedeAfiliados.Services {

    public class RedePontuacaoCadastroBL : DefaultBL{

        //
        public RedePontuacaoCadastroBL(){

        }

        //Salvar um novo registro ou atualizar um existente
        public bool salvar(RedePontuacao ORedePontuacao) {

            bool flagSucesso = false;

            if(ORedePontuacao.id > 0) {
                flagSucesso = this.atualizar(ORedePontuacao);
            }

            if(ORedePontuacao.id == 0) {
                flagSucesso = this.inserir(ORedePontuacao);
            }
            
            return flagSucesso;

        }

        //Persistir e inserir um novo registro 
        //Inserir RedePontuacao, Pessoa e lista de Endereços vinculados
        private bool inserir(RedePontuacao ORedePontuacao) {

            ORedePontuacao.setDefaultInsertValues();

            db.RedePontuacao.Add(ORedePontuacao);

            db.SaveChanges();

            return ORedePontuacao.id > 0;
        }

        //Persistir e atualizar um registro existente 
        //Atualizar dados da RedePontuacao, Pessoa e lista de endereços
        private bool atualizar(RedePontuacao ORedePontuacao) {

            //Localizar existentes no banco
            RedePontuacao dbRedePontuacao = this.db.RedePontuacao.Find(ORedePontuacao.id);

            if (dbRedePontuacao == null) {
                return false;
            }

            //Configurar valores padrão
            ORedePontuacao.setDefaultUpdateValues();

            //Atualizacao da RedePontuacao
            var RedePontuacaoEntry = db.Entry(dbRedePontuacao);
            RedePontuacaoEntry.CurrentValues.SetValues(ORedePontuacao);
            RedePontuacaoEntry.ignoreFields();

            db.SaveChanges();
            
            return ORedePontuacao.id > 0;
        }


    }
}