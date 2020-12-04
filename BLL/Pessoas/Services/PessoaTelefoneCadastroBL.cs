using System;
using DAL.Pessoas;

namespace BLL.Pessoas {

	public class PessoaTelefoneCadastroBL : PessoaTelefoneConsultaBL, IPessoaTelefoneCadastroBL {

		//
		public PessoaTelefoneCadastroBL() {
		}

        //Definir se é um insert ou update e enviar o registro para o banco de dados 
        public bool salvar(PessoaTelefone OPessoaTelefone) {

            var flagSucesso = false;

            if (OPessoaTelefone.id > 0) {
                flagSucesso = this.atualizar(OPessoaTelefone);
            }

            if (OPessoaTelefone.id == 0) {
                flagSucesso = this.inserir(OPessoaTelefone);
            }

            return flagSucesso;

        }

        //Persistir e inserir um novo registro 
        private bool inserir(PessoaTelefone OPessoaTelefone) {

	        OPessoaTelefone.setDefaultInsertValues();
	        db.PessoaTelefone.Add(OPessoaTelefone);
	        db.SaveChanges();
	        
            return OPessoaTelefone.id > 0;
        }

        //Persistir e atualizar um registro existente 
        private bool atualizar(PessoaTelefone OPessoaTelefone) {

            //Localizar existentes no banco
            PessoaTelefone dbPessoaTelefone = this.carregar(OPessoaTelefone.id);

            //Configurar valores padrão
            OPessoaTelefone.setDefaultUpdateValues();

            //Atualizacao do Telefone
            var PessoaTelefoneEntry = db.Entry(dbPessoaTelefone);
            PessoaTelefoneEntry.CurrentValues.SetValues(OPessoaTelefone);
            PessoaTelefoneEntry.ignoreFields(new [] { "idPessoa" });

            db.SaveChanges();

            return OPessoaTelefone.id > 0;
        }

	}
}