using System;
using System.Data.Entity;
using System.Json;
using System.Linq;
using BLL.Services;
using DAL.Documentos;
using DAL.Enderecos;
using DAL.FinanceiroLancamentos;
using DAL.Permissao.Security.Extensions;
using DAL.Pessoas;
using UTIL.Resources;

namespace BLL.FinanceiroLancamentos {

	public class CredorBL : DefaultBL, ICredorBL{

		//Carregar a partir do ID
		public Credor carregar(int id) {

			var query = from For in db.Credor
						where	
							For.flagExcluido == false && 
							For.id == id
						select
							For;

            query = query.condicoesSeguranca();

			return query.FirstOrDefault();
		}

		//Listagem de registros a partir dos parâmetros
		public IQueryable<Credor> listar(string valorBusca, bool? ativo) {

			var query = from For in db.Credor
									  .Include(x => x.Pessoa)
						where	
							For.flagExcluido == false
						select
							For;

            query = query.condicoesSeguranca();

			if (!String.IsNullOrEmpty(valorBusca)) {
				query = query.Where(x => x.Pessoa.nome.Contains(valorBusca));
			}

			if (ativo.HasValue) {
				query = query.Where(x => x.ativo == ativo);
			}

			return query.AsNoTracking();
		}

		//Completar busca de autocomplete
		public object autocompletar(string term, int idCredor) {

			var query = from For in db.Credor
						where
							(For.Pessoa.nome.Contains(term)) &&
							(For.id == idCredor || idCredor == 0) &&
							(For.flagExcluido == false) && 
							(For.ativo == true)
						select new {
							value = For.Pessoa.nome,
							id = For.id,
                            idOrganizacao = For.idOrganizacao,
							label = For.Pessoa.nome,
							telPrincipal = For.Pessoa.dddTelPrincipal + For.Pessoa.nroTelPrincipal,
							telSecundario = For.Pessoa.dddTelSecundario + For.Pessoa.nroTelSecundario,
							cnpf = For.Pessoa.nroDocumento,
							emailPrincipal = For.Pessoa.emailPrincipal(),
							emailSecundario = For.Pessoa.emailSecundario()
						};

            query = query.condicoesSeguranca();

			return query.ToList();
		}

		//Salvar um novo registro ou atualizar um existente
		public bool salvar(Credor OCredor) {

		    OCredor.Pessoa.razaoSocial = OCredor.Pessoa.razaoSocial ?? OCredor.Pessoa.nome;
			OCredor.Pessoa.idTipoDocumento = TipoDocumentoConst.CNPJ;
			OCredor.Pessoa.nroDocumento = UtilString.onlyAlphaNumber(OCredor.Pessoa.nroDocumento);
			OCredor.Pessoa.inscricaoEstadual = UtilString.onlyNumber(OCredor.Pessoa.inscricaoEstadual);
			OCredor.Pessoa.inscricaoMunicipal = UtilString.onlyNumber(OCredor.Pessoa.inscricaoMunicipal);

			if(OCredor.id == 0){	
				return this.inserir(OCredor);
			}

			return this.atualizar(OCredor);
		}

		//Persistir e inserir um novo registro 
		//Inserir Credor, Pessoa e lista de Endereços vinculados
		private bool inserir(Credor OCredor) { 

			OCredor.setDefaultInsertValues();
			OCredor.Pessoa.setDefaultInsertValues();
			OCredor.Pessoa.listaEnderecos.ToList().ForEach(Item =>{
				Item.cep = UtilString.onlyNumber(Item.cep);
				Item.idTipoEndereco = TipoEnderecoConst.PRINCIPAL;
				Item.setDefaultInsertValues();
			});
			
			db.Credor.Add(OCredor);
			db.SaveChanges();

			return OCredor.id > 0;
		}

		//Persistir e atualizar um registro existente 
		//Atualizar dados da Credor, Pessoa e lista de endereços
		private bool atualizar(Credor OCredor) { 

			//Localizar existentes no banco
			Credor dbCredor = this.carregar(OCredor.id);

            if (dbCredor == null) {
                return false;
            }

			Pessoa dbPessoa = db.Pessoa.FirstOrDefault(x => x.id == dbCredor.idPessoa);

			//Configurar valores padrão
			OCredor.setDefaultUpdateValues();
			OCredor.Pessoa.setDefaultUpdateValues();
			OCredor.idPessoa = dbPessoa.id;
			OCredor.Pessoa.id = dbPessoa.id;

			//Atualização do Credor
			var CredorEntry = db.Entry(dbCredor);
			CredorEntry.CurrentValues.SetValues(OCredor);
			CredorEntry.ignoreFields();

			//Atualização Dados Pessoa
			var PessoaEntry = db.Entry(dbPessoa);
			PessoaEntry.CurrentValues.SetValues(OCredor.Pessoa);
			PessoaEntry.ignoreFields();
			
			//Atualização da lista de endereços enviados
			foreach (var ItemEndereco in OCredor.Pessoa.listaEnderecos) {
				var dbEndereco = dbPessoa.listaEnderecos.FirstOrDefault(e => e.id == ItemEndereco.id);

				if (dbEndereco != null) {
					var EnderecoEntry = db.Entry(dbEndereco);

					ItemEndereco.setDefaultUpdateValues();
					EnderecoEntry.CurrentValues.SetValues(ItemEndereco);
					EnderecoEntry.ignoreFields(new[]{ "idTipoEndereco", "idPessoa" });
				} else { 
					ItemEndereco.idPessoa = OCredor.idPessoa;
					ItemEndereco.setDefaultInsertValues();
					db.PessoaEndereco.Add(ItemEndereco);
				}
			}

			db.SaveChanges();

			return OCredor.id > 0;
		}

		//Verifica a existência para evitar a duplicidade
		public bool existe(Credor OCredor) {

			string nroDocumento = UtilString.onlyNumber(OCredor.Pessoa.nroDocumento);

			var query = from Forn in db.Credor
						where 
							Forn.id != OCredor.id && 
							Forn.flagExcluido == false
						select Forn;

            query = query.condicoesSeguranca();

			if (!String.IsNullOrEmpty(nroDocumento)) {
				query = query.Where(x => x.Pessoa.nroDocumento == nroDocumento);
			}

			if (!String.IsNullOrEmpty(OCredor.Pessoa.emailPrincipal())) {
				query = query.Where(x => x.Pessoa.emailPrincipal() == OCredor.Pessoa.emailPrincipal());
			}

			if (!String.IsNullOrEmpty(OCredor.Pessoa.nome)) {
				query = query.Where(x => x.Pessoa.nome == OCredor.Pessoa.nome);
			}

			return (query.Count() != 0);
		}

        //Alteração de status
        public JsonMessageStatus alterarStatus(int id) {
			var retorno = new JsonMessageStatus();

			var item = this.carregar(id);

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

		//Exclusao logica
		public UtilRetorno excluir(int id) {

			Credor OCredor = this.carregar(id);

			if (OCredor == null) {
				return UtilRetorno.newInstance(true, "O registro informado não foi localizado.");
			}

			OCredor.flagExcluido = true;

			OCredor.dtAlteracao = DateTime.Now;

            OCredor.idUsuarioAlteracao = User.id();

			db.SaveChanges();

			return UtilRetorno.newInstance(false, "O registro foi removido com sucesso.");
		}
	}
}