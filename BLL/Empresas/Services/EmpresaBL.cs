using System;
using System.Linq;
using System.Data.Entity;
using DAL.Empresas;
using DAL.Pessoas;
using DAL.Documentos;
using DAL.Enderecos;
using DAL.Repository.Base;
using System.Data.Entity.Core.Objects;
using BLL.Services;
using DAL.Permissao.Security.Extensions;
using EntityFramework.Extensions;

namespace BLL.Empresas {

	public class EmpresaBL : DefaultBL, IEmpresaBL {

		//
		public EmpresaBL(){
		}

		//Carregar empresa pelo CNPJ
		public Empresa carregar(int id) {

			var query = (from Emp in 
							 db.Empresa 
							 .Include(x => x.Pessoa)
						 where 
							Emp.id == id && 
							Emp.flagExcluido == "N" 
						 select Emp
						 );

			return query.FirstOrDefault();
		}

		//Buscar empresa pelo CNPJ
		public Empresa carregar(string cnpj) {

			string cnpjLimpo = UtilString.onlyNumber(cnpj);

			var query = (from OEmpresa in 
							 db.Empresa 
							 .Include(x => x.Pessoa)
						 where 
							OEmpresa.Pessoa.nroDocumento == cnpjLimpo && 
							OEmpresa.flagExcluido == "N" 
						 select OEmpresa
						 );

			return query.FirstOrDefault();
		}

		
		//Listagem das empresas com possibilidade de busca pelos filtros
		public IQueryable<Empresa> listar(string valorBusca, string ativo) {

			var query = db.Empresa
							.Include(x => x.Pessoa)
							.Where(x => x.flagExcluido == "N");
            
			if (!String.IsNullOrEmpty(valorBusca)) {
				query = query.Where(x => x.Pessoa.nome.Contains(valorBusca) || x.Pessoa.razaoSocial.Contains(valorBusca));
			}

			if (!String.IsNullOrEmpty(ativo)) {
				query = query.Where(x => x.ativo == ativo);
			}

			return query;
		}

		//Salvar um novo registro ou atualizar um existente
		public bool salvar(Empresa OEmpresa) {

			OEmpresa.Pessoa.idTipoDocumento = TipoDocumentoConst.CNPJ;
			OEmpresa.Pessoa.nroDocumento = UtilString.onlyAlphaNumber(OEmpresa.Pessoa.nroDocumento);
			OEmpresa.Pessoa.inscricaoEstadual = UtilString.onlyNumber(OEmpresa.Pessoa.inscricaoEstadual);
			OEmpresa.Pessoa.inscricaoMunicipal = UtilString.onlyNumber(OEmpresa.Pessoa.inscricaoMunicipal);

			if(OEmpresa.id == 0){	
				return this.inserir(OEmpresa);
			}

			return this.atualizar(OEmpresa);
		}

		//Persistir e inserir um novo registro 
		//Inserir Empresa, Pessoa e lista de Endereços vinculados
		private bool inserir(Empresa OEmpresa) { 

			OEmpresa.setDefaultInsertValues<Empresa>();
			OEmpresa.Pessoa.setDefaultInsertValues<Pessoa>();
			OEmpresa.Pessoa.listaEnderecos.ToList().ForEach(Item =>{
				Item.cep = UtilString.onlyNumber(Item.cep);
				Item.idTipoEndereco = TipoEnderecoConst.PRINCIPAL;
				Item.setDefaultInsertValues<PessoaEndereco>();
			});
			
			db.Empresa.Add(OEmpresa);
			db.SaveChanges();

			return OEmpresa.id > 0;
		}

		//Persistir e atualizar um registro existente 
		//Atualizar dados da Empresa, Pessoa e lista de endereços
		private bool atualizar(Empresa OEmpresa) { 

			//Localizar existentes no banco
			Empresa dbEmpresa = this.carregar(OEmpresa.id);

            if (dbEmpresa == null) {
                return false;
            }

			Pessoa dbPessoa = db.Pessoa.FirstOrDefault(x => x.id == dbEmpresa.idPessoa);

			//Configurar valores padrão
			OEmpresa.setDefaultUpdateValues<Empresa>();
			OEmpresa.Pessoa.setDefaultUpdateValues<Pessoa>();
			OEmpresa.idPessoa= dbPessoa.id;
			OEmpresa.Pessoa.id = dbPessoa.id;

			//Atualizacao da Empresa
			var EmpresaEntry = db.Entry(dbEmpresa);
			EmpresaEntry.CurrentValues.SetValues(OEmpresa);
			EmpresaEntry.ignoreFields<Empresa>();

			//Atualizacao Dados Pessoa
			var PessoaEntry = db.Entry(dbPessoa);
			PessoaEntry.CurrentValues.SetValues(OEmpresa.Pessoa);
			PessoaEntry.ignoreFields<Pessoa>();
			
			//Atualizacao da lista de endereços enviados
			foreach (var ItemEndereco in OEmpresa.Pessoa.listaEnderecos) {
			    var dbEndereco = dbPessoa.listaEnderecos.FirstOrDefault(e => e.id == ItemEndereco.id);

				if (dbEndereco != null) {
					var EnderecoEntry = db.Entry(dbEndereco);

					ItemEndereco.setDefaultUpdateValues<PessoaEndereco>();
					EnderecoEntry.CurrentValues.SetValues(ItemEndereco);
					EnderecoEntry.ignoreFields<PessoaEndereco>(new string[]{"idTipoEndereco", "idPessoa"});
				} else { 
					ItemEndereco.idPessoa = OEmpresa.idPessoa;
					ItemEndereco.setDefaultInsertValues<PessoaEndereco>();
					db.PessoaEndereco.Add(ItemEndereco);
				}
			}

			db.SaveChanges();
			return OEmpresa.id > 0;
		}

		//Verificar existencia de dados para evitar duplicidades
		public bool existe(string cnpj, string email, int idDesconsiderado) {

			var query = from Emp in db.Empresa.AsNoTracking()
							.Include(x => x.Pessoa)
						where
							Emp.id != idDesconsiderado && 
							Emp.flagExcluido == "N"
						select 
							Emp;

			if (!String.IsNullOrEmpty(cnpj)) { 
				cnpj = UtilString.onlyAlphaNumber(cnpj);
				query = query.Where(x => x.Pessoa.nroDocumento == cnpj);
			}

			if (!String.IsNullOrEmpty(email)) { 
				query = query.Where(x => x.Pessoa.emailPrincipal == email || x.Pessoa.emailSecundario == email);
			}

			bool flagExiste = (query.Any());
			return flagExiste;
		}


		//Alteracao do status da empresa
		public bool alterarStatus(int id) {
			int idUsuarioLogado = User.id();

			db.Empresa.Where(x => x.id == id)
				.Update(x => new Empresa{ flagExcluido = "S", dtAlteracao = DateTime.Now, idUsuarioAlteracao = idUsuarioLogado});

			return true;
		}

		//Excluir uma empresa logicamente
		public bool excluir(int id) {
			int idUsuarioLogado = User.id();

			db.Empresa
				.Where(x => x.id == id)
				.Update(x => new Empresa { flagExcluido = "S", dtAlteracao = DateTime.Now, idUsuarioAlteracao = idUsuarioLogado });

			return true;
		}

	}
}