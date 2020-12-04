using System;
using System.Data.Entity;
using System.Json;
using System.Linq;
using BLL.Services;
using DAL.Documentos;
using DAL.Enderecos;
using DAL.FinanceiroLancamentos;
using DAL.Pessoas;
using UTIL.Resources;

namespace BLL.FinanceiroLancamentos {

	public class DevedorBL : DefaultBL, IDevedorBL{

		//Carregar a partir do ID
		public Devedor carregar(int id) {

			var query = from For in db.Devedor
						where	
							For.flagExcluido == false && 
							For.id == id
						select
							For;

            query = query.condicoesSeguranca();

			return query.FirstOrDefault();
		}

		//Listagem de registros a partir dos parâmetros
		public IQueryable<Devedor> listar(string valorBusca, bool? ativo) {

			var query = from For in db.Devedor
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
		public object autocompletar(string term, int idDevedor) {

			var query = from For in db.Devedor
						where
							(For.Pessoa.nome.Contains(term)) &&
							(For.id == idDevedor || idDevedor == 0) &&
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
		public bool salvar(Devedor ODevedor) {

		    ODevedor.Pessoa.razaoSocial = ODevedor.Pessoa.razaoSocial ?? ODevedor.Pessoa.nome;
			ODevedor.Pessoa.idTipoDocumento = TipoDocumentoConst.CNPJ;
			ODevedor.Pessoa.nroDocumento = UtilString.onlyAlphaNumber(ODevedor.Pessoa.nroDocumento);
			ODevedor.Pessoa.inscricaoEstadual = UtilString.onlyNumber(ODevedor.Pessoa.inscricaoEstadual);
			ODevedor.Pessoa.inscricaoMunicipal = UtilString.onlyNumber(ODevedor.Pessoa.inscricaoMunicipal);

			if(ODevedor.id == 0){	
				return this.inserir(ODevedor);
			}

			return this.atualizar(ODevedor);
		}

		//Persistir e inserir um novo registro 
		//Inserir Devedor, Pessoa e lista de Endereços vinculados
		private bool inserir(Devedor ODevedor) { 

			ODevedor.setDefaultInsertValues();
			ODevedor.Pessoa.setDefaultInsertValues();
			ODevedor.Pessoa.listaEnderecos.ToList().ForEach(Item =>{
				Item.cep = UtilString.onlyNumber(Item.cep);
				Item.idTipoEndereco = TipoEnderecoConst.PRINCIPAL;
				Item.setDefaultInsertValues();
			});
			
			db.Devedor.Add(ODevedor);
			db.SaveChanges();

			return ODevedor.id > 0;
		}

		//Persistir e atualizar um registro existente 
		//Atualizar dados da Devedor, Pessoa e lista de endereços
		private bool atualizar(Devedor ODevedor) { 

			//Localizar existentes no banco
			Devedor dbDevedor = this.carregar(ODevedor.id);

            if (dbDevedor == null) {
                return false;
            }

			Pessoa dbPessoa = db.Pessoa.FirstOrDefault(x => x.id == dbDevedor.idPessoa);

			//Configurar valores padrão
			ODevedor.setDefaultUpdateValues();
			ODevedor.Pessoa.setDefaultUpdateValues();
			ODevedor.idPessoa = dbPessoa.id;
			ODevedor.Pessoa.id = dbPessoa.id;

			//Atualização do Devedor
			var DevedorEntry = db.Entry(dbDevedor);
			DevedorEntry.CurrentValues.SetValues(ODevedor);
			DevedorEntry.ignoreFields();

			//Atualização Dados Pessoa
			var PessoaEntry = db.Entry(dbPessoa);
			PessoaEntry.CurrentValues.SetValues(ODevedor.Pessoa);
			PessoaEntry.ignoreFields();
			
			//Atualização da lista de endereços enviados
			foreach (var ItemEndereco in ODevedor.Pessoa.listaEnderecos) {
				var dbEndereco = dbPessoa.listaEnderecos.FirstOrDefault(e => e.id == ItemEndereco.id);

				if (dbEndereco != null) {
					var EnderecoEntry = db.Entry(dbEndereco);

					ItemEndereco.setDefaultUpdateValues();
					EnderecoEntry.CurrentValues.SetValues(ItemEndereco);
					EnderecoEntry.ignoreFields(new[]{ "idTipoEndereco", "idPessoa" });
				} else { 
					ItemEndereco.idPessoa = ODevedor.idPessoa;
					ItemEndereco.setDefaultInsertValues();
					db.PessoaEndereco.Add(ItemEndereco);
				}
			}

			db.SaveChanges();

			return ODevedor.id > 0;
		}

		//Verifica a existência para evitar a duplicidade
		public bool existe(Devedor ODevedor) {

			string nroDocumento = UtilString.onlyNumber(ODevedor.Pessoa.nroDocumento);

			var query = from Forn in db.Devedor
						where 
							Forn.id != ODevedor.id && 
							Forn.flagExcluido == false
						select Forn;

            query = query.condicoesSeguranca();

			if (!String.IsNullOrEmpty(nroDocumento)) {
				query = query.Where(x => x.Pessoa.nroDocumento == nroDocumento);
			}

			if (!String.IsNullOrEmpty(ODevedor.Pessoa.emailPrincipal())) {
				query = query.Where(x => x.Pessoa.emailPrincipal() == ODevedor.Pessoa.emailPrincipal());
			}

			if (!String.IsNullOrEmpty(ODevedor.Pessoa.nome)) {
				query = query.Where(x => x.Pessoa.nome == ODevedor.Pessoa.nome);
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

			Devedor ODevedor = this.carregar(id);

			if (ODevedor == null) {
				return UtilRetorno.newInstance(true, "O registro informado não foi localizado.");
			}

			ODevedor.flagExcluido = true;

			ODevedor.dtAlteracao = DateTime.Now;

			db.SaveChanges();

			return UtilRetorno.newInstance(false, "O registro foi removido com sucesso.");
		}
	}
}