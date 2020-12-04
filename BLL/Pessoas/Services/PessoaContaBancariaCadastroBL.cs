using System;
using DAL.Pessoas;

namespace BLL.Pessoas {

	public class PessoaContaBancariaCadastroBL : PessoaContaBancariaConsultaBL, IPessoaContaBancariaCadastroBL {

		//
		public PessoaContaBancariaCadastroBL() {
		}

        //Definir se é um insert ou update e enviar o registro para o banco de dados 
        public bool salvar(PessoaContaBancaria OPessoaContaBancaria){
			
	        OPessoaContaBancaria.nroAgencia = OPessoaContaBancaria.nroAgencia.abreviar(20);
	        OPessoaContaBancaria.nroDigitoAgencia = OPessoaContaBancaria.nroDigitoAgencia.abreviar(1);
	        OPessoaContaBancaria.nroContaBancaria = OPessoaContaBancaria.nroContaBancaria.abreviar(20);
	        OPessoaContaBancaria.nroDocumentoTitular = UtilString.onlyNumber(OPessoaContaBancaria.nroDocumentoTitular);
	        OPessoaContaBancaria.nroDocumentoTitular = OPessoaContaBancaria.nroDocumentoTitular.abreviar(16);
	        OPessoaContaBancaria.nomeTitular = OPessoaContaBancaria.nomeTitular.abreviar(100);
			
            var flagSucesso = false;
			
            if (OPessoaContaBancaria.id > 0) {
                flagSucesso = this.atualizar(OPessoaContaBancaria);
            }
			
            if (OPessoaContaBancaria.id == 0) {
                flagSucesso = this.inserir(OPessoaContaBancaria);
            }
			
            return flagSucesso;

        }
		
        //Persistir e inserir um novo registro 
        private bool inserir(PessoaContaBancaria OPessoaContaBancaria) {

	        OPessoaContaBancaria.setDefaultInsertValues();
	        db.PessoaContaBancaria.Add(OPessoaContaBancaria);
	        db.SaveChanges();
	        
            return OPessoaContaBancaria.id > 0;
        }
		
        //Persistir e atualizar um registro existente 
        private bool atualizar(PessoaContaBancaria OPessoaContaBancaria) {

            //Localizar existentes no banco
            PessoaContaBancaria dbPessoaContaBancaria = this.carregar(OPessoaContaBancaria.id);

            //Configurar valores padrão
            OPessoaContaBancaria.setDefaultUpdateValues();

            //Atualizacao do Telefone
            var PessoaContaBancariaEntry = db.Entry(dbPessoaContaBancaria);
            PessoaContaBancariaEntry.CurrentValues.SetValues(OPessoaContaBancaria);
            PessoaContaBancariaEntry.ignoreFields(new [] { "idPessoa" });

            db.SaveChanges();

            return OPessoaContaBancaria.id > 0;
        }

	}
}