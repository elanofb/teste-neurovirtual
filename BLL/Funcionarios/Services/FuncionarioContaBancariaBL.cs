using System;
using System.Data.Entity;
using System.Linq;
using DAL.Funcionarios;
using DAL.Repository.Base;
using EntityFramework.Extensions;
using BLL.Services;
using DAL.Permissao.Security.Extensions;

namespace BLL.Funcionarios {

	public class FuncionarioContaBancariaBL : DefaultBL, IFuncionarioContaBancariaBL {

		//Construtor
		public FuncionarioContaBancariaBL(){
		}


		//Carregamento de um registro específico
		public FuncionarioContaBancaria carregar(int id) { 

			var query = (	from PesCon in db.FuncionarioContaBancaria
											.Include(x => x.Funcionario)
							where 
								PesCon.id == id &&
								PesCon.flagExcluido == "N"
							select
								PesCon
						);

			return query.FirstOrDefault();

		}

		//Listagem de registros de acordo com parametros informados
		public IQueryable<FuncionarioContaBancaria> listar(int idFuncionario, string ativo) {

			var query = from Cont in db.FuncionarioContaBancaria
									.Include(x => x.Funcionario)
									.AsNoTracking()
						where Cont.flagExcluido == "N"
						select Cont;

			if (idFuncionario > 0) {
				query = query.Where(x => x.idFuncionario == idFuncionario);
			}

			if (!String.IsNullOrEmpty(ativo)) { 
				query = query.Where(x => x.ativo == ativo);
			}

			return query;
		}

		//Definir se é um insert ou update e enviar o registro para o banco de dados 
		public bool salvar(FuncionarioContaBancaria OContaBancaria) {
			
			if (OContaBancaria.id == 0) { 
				return this.inserir(OContaBancaria);
			} else { 
				return this.atualizar(OContaBancaria);
			}
		}

		//Persistir e inserir um novo registro 
		private bool inserir(FuncionarioContaBancaria OContaBancaria) { 

			OContaBancaria.setDefaultInsertValues<FuncionarioContaBancaria>();

			db.FuncionarioContaBancaria.Add(OContaBancaria);
			db.SaveChanges();
			return OContaBancaria.id > 0;
		}

		//Persistir e atualizar um registro existente 
		private bool atualizar(FuncionarioContaBancaria OContaBancaria) { 
			
			//Localizar existentes no banco
			FuncionarioContaBancaria dbContaBancaria = this.carregar(OContaBancaria.id);

			//Configurar valores padrão
			OContaBancaria.setDefaultUpdateValues<FuncionarioContaBancaria>();

			//Atualizacao da Empresa
			var ContaBancariaEntry = db.Entry(dbContaBancaria);
			ContaBancariaEntry.CurrentValues.SetValues(OContaBancaria);
			ContaBancariaEntry.ignoreFields<FuncionarioContaBancaria>(new string[]{"idFuncionario"});

			db.SaveChanges();

			return OContaBancaria.id > 0;
		}

		//Carregamento de um registro específico
		public bool existe(string codigoBanco, string nroAgencia, string nroConta, int idDesconsiderar) { 
			
			var query = (	from PesCon in db.FuncionarioContaBancaria.AsNoTracking()
							where 
								PesCon.id != idDesconsiderar &&
								PesCon.flagExcluido == "N"
							select
								PesCon
						);

			if (!String.IsNullOrEmpty(codigoBanco)) { 
				query = query.Where(x => x.codigoBanco == codigoBanco);
			}

			if (!String.IsNullOrEmpty(nroAgencia)) { 
				query = query.Where(x => x.nroAgencia == nroAgencia);
			}

			if (!String.IsNullOrEmpty(nroConta)) { 
				query = query.Where(x => x.nroConta == nroConta);
			}

			bool flagExiste = (query.Any());
			return flagExiste;

		}

		//Remover um registro logicamente
		public UtilRetorno excluir(int id) {

            int idUsuarioLogado = User.id();

			db.FuncionarioContaBancaria.Where(x => x.id == id)
							.Update(x => new FuncionarioContaBancaria{ flagExcluido = "S", idUsuarioAlteracao = idUsuarioLogado, dtAlteracao = DateTime.Now});
			
			UtilRetorno Retorno = UtilRetorno.getInstance();
			Retorno.flagError = false;
			return Retorno;
		}
	}
}