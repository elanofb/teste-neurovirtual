using System;
using System.Json;
using System.Linq;
using DAL.Documentos;
using DAL.Enderecos;
using DAL.Fornecedores;
using DAL.Pessoas;
using UTIL.Resources;

namespace BLL.Fornecedores {

	public class FornecedorCadastroBL : FornecedorConsultaBL, IFornecedorCadastroBL{

		//Atributos

		//Propriedades

		//Construtor
		public FornecedorCadastroBL() {
		}

	    //Salvar um novo registro ou atualizar um existente
		public bool salvar(Fornecedor OFornecedor) {

			OFornecedor.Pessoa.idTipoDocumento = OFornecedor.Pessoa.flagTipoPessoa == "J" ? TipoDocumentoConst.CNPJ : TipoDocumentoConst.CPF;
			OFornecedor.Pessoa.nroDocumento = UtilString.onlyAlphaNumber(OFornecedor.Pessoa.nroDocumento);

			OFornecedor.Pessoa.nome = OFornecedor.Pessoa.nome.abreviar(100);
			OFornecedor.Pessoa.razaoSocial = OFornecedor.Pessoa.razaoSocial.abreviar(100);
			
			OFornecedor.Pessoa.nroTelPrincipal = UtilString.onlyAlphaNumber(OFornecedor.Pessoa.nroTelPrincipal);
			OFornecedor.Pessoa.nroTelSecundario = UtilString.onlyAlphaNumber(OFornecedor.Pessoa.nroTelSecundario);
			OFornecedor.Pessoa.nroTelTerciario = UtilString.onlyAlphaNumber(OFornecedor.Pessoa.nroTelTerciario);
			
			OFornecedor.Pessoa.inscricaoEstadual = UtilString.onlyNumber(OFornecedor.Pessoa.inscricaoEstadual).abreviar(50);
			OFornecedor.Pessoa.inscricaoMunicipal = UtilString.onlyNumber(OFornecedor.Pessoa.inscricaoMunicipal).abreviar(50);

			if(OFornecedor.id == 0){	
				return this.inserir(OFornecedor);
			}

			return this.atualizar(OFornecedor);
		}

		//Persistir e inserir um novo registro 
		//Inserir Fornecedor, Pessoa e lista de Endereços vinculados
		private bool inserir(Fornecedor OFornecedor) { 

			OFornecedor.setDefaultInsertValues();

			OFornecedor.Pessoa.setDefaultInsertValues();

			OFornecedor.Pessoa.listaEnderecos.ToList().ForEach(x =>{

				x.cep = UtilString.onlyNumber(x.cep);

				x.idTipoEndereco = TipoEnderecoConst.PRINCIPAL;

				x.setDefaultInsertValues();

			});
			
			db.Fornecedor.Add(OFornecedor);

			db.SaveChanges();

			return OFornecedor.id > 0;
		}

		//Persistir e atualizar um registro existente 
		//Atualizar dados da Fornecedor, Pessoa e lista de endereços
		private bool atualizar(Fornecedor OFornecedor) { 

			//Localizar existentes no banco
			var dbFornecedor = this.carregar(OFornecedor.id);

            if (dbFornecedor == null) {
                return false;
            }

			Pessoa dbPessoa = db.Pessoa.FirstOrDefault(x => x.id == dbFornecedor.idPessoa);

			//Configurar valores padrão
			OFornecedor.setDefaultUpdateValues();

			OFornecedor.Pessoa.setDefaultUpdateValues();

			OFornecedor.idPessoa = dbPessoa.id;

			OFornecedor.Pessoa.id = dbPessoa.id;

			//Atualizacao da Empresa
			var FornecedorEntry = db.Entry(dbFornecedor);

			FornecedorEntry.CurrentValues.SetValues(OFornecedor);

			FornecedorEntry.ignoreFields();

			//Atualizacao Dados Pessoa
			var PessoaEntry = db.Entry(dbPessoa);

			PessoaEntry.CurrentValues.SetValues(OFornecedor.Pessoa);

			PessoaEntry.ignoreFields();
			
			//Atualizacao da lista de endereços enviados
			foreach (var ItemEndereco in OFornecedor.Pessoa.listaEnderecos) {

				var dbEndereco = dbPessoa.listaEnderecos.FirstOrDefault(e => e.id == ItemEndereco.id);

				if (dbEndereco != null) {

					var EnderecoEntry = db.Entry(dbEndereco);

					ItemEndereco.setDefaultUpdateValues();

					EnderecoEntry.CurrentValues.SetValues(ItemEndereco);

					EnderecoEntry.ignoreFields(new [] { "idTipoEndereco", "idPessoa" });

				} else { 

					ItemEndereco.idPessoa = OFornecedor.idPessoa;

					ItemEndereco.setDefaultInsertValues();

					db.PessoaEndereco.Add(ItemEndereco);

				}
			}

			db.SaveChanges();

			return OFornecedor.id > 0;
		}

        //Alteracao de status
        public JsonMessageStatus alterarStatus(int id) {

			var retorno = new JsonMessageStatus();

			var item = this.carregar(id);

			if (item == null) {
				retorno.error = true;
				retorno.message = NotificationMessages.invalid_register_id;
			} else {
				item.ativo = item.ativo != true;
				db.SaveChanges();
				retorno.active = item.ativo == true ? "S" : "N";
				retorno.message = NotificationMessages.updateSuccess;
			}
			return retorno;
		}

	}
}