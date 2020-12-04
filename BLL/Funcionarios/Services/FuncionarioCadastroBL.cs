using System;
using System.Linq;
using DAL.Funcionarios;
using DAL.Pessoas;
using DAL.Documentos;
using DAL.Enderecos;
using System.Json;
using UTIL.Resources;

namespace BLL.Funcionarios {

	public class FuncionarioCadastroBL : FuncionarioConsultaBL, IFuncionarioCadastroBL {

		//
		public FuncionarioCadastroBL() {
		}

		//Salvar um novo registro ou atualizar um existente
		public bool salvar(Funcionario OFuncionario) {

			OFuncionario.Pessoa.flagTipoPessoa = "F";
			OFuncionario.Pessoa.idTipoDocumento = TipoDocumentoConst.CPF;
			OFuncionario.Pessoa.nroDocumento = UtilString.onlyAlphaNumber(OFuncionario.Pessoa.nroDocumento);
			OFuncionario.Pessoa.nroTelPrincipal = UtilString.onlyNumber(OFuncionario.Pessoa.nroTelPrincipal).abreviar(20);
			OFuncionario.Pessoa.nroTelSecundario = UtilString.onlyAlphaNumber(OFuncionario.Pessoa.nroTelSecundario).abreviar(20);
			OFuncionario.Pessoa.rg = UtilString.onlyAlphaNumber(OFuncionario.Pessoa.rg).abreviar(50);
			OFuncionario.Pessoa.CidadeOrigem = null;

			OFuncionario.Pessoa.nome = OFuncionario.Pessoa.nome.abreviar(100);
			OFuncionario.Pessoa.nroCTPS = OFuncionario.Pessoa.nroCTPS.abreviar(20);
			OFuncionario.Pessoa.serieCTPS = OFuncionario.Pessoa.serieCTPS.abreviar(5);
			OFuncionario.Pessoa.nroPIS = OFuncionario.Pessoa.nroPIS.abreviar(20);
			OFuncionario.Pessoa.nroTituloEleitor = OFuncionario.Pessoa.nroTituloEleitor.abreviar(20);
			OFuncionario.Pessoa.zonaEleitoral = OFuncionario.Pessoa.zonaEleitoral.abreviar(5);
			OFuncionario.Pessoa.sessaoEleitoral = OFuncionario.Pessoa.sessaoEleitoral.abreviar(5);
			OFuncionario.Pessoa.nroReservista = OFuncionario.Pessoa.nroReservista.abreviar(15);
			OFuncionario.Pessoa.serieReservista = OFuncionario.Pessoa.serieReservista.abreviar(5);
			OFuncionario.Pessoa.nroCNH = OFuncionario.Pessoa.nroCNH.abreviar(20);
			OFuncionario.Pessoa.categoriaCNH = OFuncionario.Pessoa.categoriaCNH.abreviar(1);
			OFuncionario.Pessoa.nroRegistroOrgaoClasse = OFuncionario.Pessoa.nroRegistroOrgaoClasse.abreviar(20);

            if (String.IsNullOrEmpty(OFuncionario.Pessoa.listaEnderecos[0].cep)) {
                OFuncionario.Pessoa.listaEnderecos = null;
            }

			if(OFuncionario.id == 0){	
				return this.inserir(OFuncionario);
			}

			return this.atualizar(OFuncionario);
		}

		//Persistir e inserir um novo registro 
		//Inserir Funcionario, Pessoa e lista de Endereços vinculados
		private bool inserir(Funcionario OFuncionario) { 

			OFuncionario.setDefaultInsertValues<Funcionario>();
			OFuncionario.Pessoa.setDefaultInsertValues<Pessoa>();

		    OFuncionario.Pessoa.listaEnderecos?.ToList().ForEach(Item =>{
		        Item.cep = UtilString.onlyNumber(Item.cep);
		        Item.idTipoEndereco = TipoEnderecoConst.PRINCIPAL;
		        Item.setDefaultInsertValues<PessoaEndereco>();
		    });

		    db.Funcionario.Add(OFuncionario);
			db.SaveChanges();

			return OFuncionario.id > 0;
		}

		//Persistir e atualizar um registro existente 
		//Atualizar dados da Funcionario, Pessoa e lista de endereços
		private bool atualizar(Funcionario OFuncionario) { 

			//Localizar existentes no banco
			Funcionario dbFuncionario = this.carregar(OFuncionario.id);

            if (dbFuncionario == null) {
                return false;
            }

			Pessoa dbPessoa = db.Pessoa.FirstOrDefault(x => x.id == dbFuncionario.idPessoa);

			//Configurar valores padrão
			OFuncionario.setDefaultUpdateValues<Funcionario>();
			OFuncionario.Pessoa.setDefaultUpdateValues<Pessoa>();
			OFuncionario.idPessoa= dbPessoa.id;
			OFuncionario.Pessoa.id = dbPessoa.id;

			//Atualizacao da Funcionario
			var FuncionarioEntry = db.Entry(dbFuncionario);
			FuncionarioEntry.CurrentValues.SetValues(OFuncionario);
			FuncionarioEntry.ignoreFields<Funcionario>();

			//Atualizacao Dados Pessoa
			var PessoaEntry = db.Entry(dbPessoa);
			PessoaEntry.CurrentValues.SetValues(OFuncionario.Pessoa);
			PessoaEntry.ignoreFields<Pessoa>();

            if (OFuncionario.Pessoa.listaEnderecos != null)
            {
			    //Atualizacao da lista de endereços enviados
			    foreach (var ItemEndereco in OFuncionario.Pessoa.listaEnderecos) {
			        var dbEndereco = dbPessoa.listaEnderecos.FirstOrDefault(e => e.id == ItemEndereco.id);

				    if (dbEndereco != null) {
					    var EnderecoEntry = db.Entry(dbEndereco);

					    ItemEndereco.setDefaultUpdateValues<PessoaEndereco>();
					    EnderecoEntry.CurrentValues.SetValues(ItemEndereco);
					    EnderecoEntry.ignoreFields<PessoaEndereco>(new string[]{"idTipoEndereco", "idPessoa"});
				    } else { 
					    ItemEndereco.idPessoa = OFuncionario.idPessoa;
					    ItemEndereco.setDefaultInsertValues<PessoaEndereco>();
					    db.PessoaEndereco.Add(ItemEndereco);
				    }
			    }
			}

			db.SaveChanges();
			return OFuncionario.id > 0;
		}

		//Alteracao do status da Funcionario
        public JsonMessageStatus alterarStatus(int id) {

            var retorno = new JsonMessageStatus();

            Funcionario Objeto = this.carregar(id);
            if (Objeto == null) {
                retorno.error = true;
                retorno.message = NotificationMessages.invalid_register_id;
            } else {
                Objeto.ativo = (Objeto.ativo == "S" ? "N" : "S");
                db.SaveChanges();
                retorno.active = Objeto.ativo;
                retorno.message = "Os dados foram alterados com sucesso.";
            }
            return retorno;
        }

	}
}