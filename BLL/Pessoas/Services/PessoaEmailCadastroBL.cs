using System;
using DAL.Pessoas;

namespace BLL.Pessoas {

	public class PessoaEmailCadastroBL : PessoaEmailConsultaBL, IPessoaEmailCadastroBL {

		//
		public PessoaEmailCadastroBL() {
		}

        //Definir se é um insert ou update e enviar o registro para o banco de dados 
        public bool salvar(PessoaEmail OPessoaEmail) {

            var flagSucesso = false;

            if (OPessoaEmail.id > 0) {
                flagSucesso = this.atualizar(OPessoaEmail);
            }

            if (OPessoaEmail.id == 0) {
                flagSucesso = this.inserir(OPessoaEmail);
            }

            return flagSucesso;

        }

        //Persistir e inserir um novo registro 
        private bool inserir(PessoaEmail OPessoaEmail) {

	        OPessoaEmail.setDefaultInsertValues();
	        db.PessoaEmail.Add(OPessoaEmail);
	        db.SaveChanges();
	        
            return OPessoaEmail.id > 0;
        }

        //Persistir e atualizar um registro existente 
        private bool atualizar(PessoaEmail OPessoaEmail) {

            //Localizar existentes no banco
            PessoaEmail dbPessoaEmail = this.carregar(OPessoaEmail.id);

            //Configurar valores padrão
            OPessoaEmail.setDefaultUpdateValues();

            //Atualizacao do Email
            var PessoaEmailEntry = db.Entry(dbPessoaEmail);
            PessoaEmailEntry.CurrentValues.SetValues(OPessoaEmail);
            PessoaEmailEntry.ignoreFields(new [] { "idPessoa" });

            db.SaveChanges();

            return OPessoaEmail.id > 0;
        }

	}
}