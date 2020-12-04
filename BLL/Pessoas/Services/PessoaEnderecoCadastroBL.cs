using System;
using DAL.Pessoas;

namespace BLL.Pessoas {

	public class PessoaEnderecoCadastroBL : PessoaEnderecoConsultaBL, IPessoaEnderecoCadastroBL {

		//
		public PessoaEnderecoCadastroBL() {
		}

        //Definir se é um insert ou update e enviar o registro para o banco de dados 
        public bool salvar(PessoaEndereco OPessoaEndereco) {

            OPessoaEndereco.cep = UtilString.onlyNumber(OPessoaEndereco.cep);
	        OPessoaEndereco.logradouro = OPessoaEndereco.logradouro.abreviar(100);

            var flagSucesso = false;

            if (OPessoaEndereco.id > 0) {
                flagSucesso = this.atualizar(OPessoaEndereco);   
            }

            if (OPessoaEndereco.id == 0) {
                flagSucesso = this.inserir(OPessoaEndereco);
            }

            return flagSucesso;

        }

        //Persistir e inserir um novo registro 
        private bool inserir(PessoaEndereco OPessoaEndereco) {

	        OPessoaEndereco.setDefaultInsertValues();
	        db.PessoaEndereco.Add(OPessoaEndereco);
	        db.SaveChanges();
	        
            return OPessoaEndereco.id > 0;
        }

        //Persistir e atualizar um registro existente 
        private bool atualizar(PessoaEndereco OPessoaEndereco) {

            //Localizar existentes no banco
            PessoaEndereco dbPessoaEndereco = this.carregar(OPessoaEndereco.id);

            //Configurar valores padrão
            OPessoaEndereco.setDefaultUpdateValues();

            //Atualizacao do Endereco
            var PessoaEnderecoEntry = db.Entry(dbPessoaEndereco);
            PessoaEnderecoEntry.CurrentValues.SetValues(OPessoaEndereco);
            PessoaEnderecoEntry.ignoreFields(new [] { "idPessoa" });

            db.SaveChanges();

            return OPessoaEndereco.id > 0;
        }

	}
}