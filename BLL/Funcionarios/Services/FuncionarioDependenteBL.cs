using System;
using System.Data.Entity;
using System.Linq;
using BLL.Services;
using DAL.Funcionarios;
using DAL.Permissao.Security.Extensions;
using DAL.Repository.Base;
using EntityFramework.Extensions;

namespace BLL.Funcionarios {

	public class FuncionarioDependenteBL : DefaultBL, IFuncionarioDependenteBL {

		//Construtor
		public FuncionarioDependenteBL(){
		}


		//Carregamento de um registro específico
		public FuncionarioDependente carregar(int id) { 

			var query = (	from PesCon in db.FuncionarioDependente
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
		public IQueryable<FuncionarioDependente> listar(int idFuncionario, int idTipoDependente, string ativo) {

			var query = from Cont in db.FuncionarioDependente
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
		public bool salvar(FuncionarioDependente ODependente) {

			if (ODependente.id == 0) { 
				return this.inserir(ODependente);
			} else { 
				return this.atualizar(ODependente);
			}
		}

		//Persistir e inserir um novo registro 
		private bool inserir(FuncionarioDependente ODependente) { 

			ODependente.setDefaultInsertValues<FuncionarioDependente>();

			db.FuncionarioDependente.Add(ODependente);
			db.SaveChanges();
			return ODependente.id > 0;
		}

		//Persistir e atualizar um registro existente 
		private bool atualizar(FuncionarioDependente ODependente) {

			//Localizar existentes no banco
			FuncionarioDependente dbDependente = this.carregar(ODependente.id);

			//Configurar valores padrão
			ODependente.setDefaultUpdateValues<FuncionarioDependente>();

			//Atualizacao da Empresa
			var DependenteEntry = db.Entry(dbDependente);
			DependenteEntry.CurrentValues.SetValues(ODependente);
			DependenteEntry.ignoreFields<FuncionarioDependente>(new string[]{"idFuncionario"});

			db.SaveChanges();

			return ODependente.id > 0;
		}

		//Carregamento de um registro específico
		public bool existe(string nroDocumento, int idDesconsiderar) { 

			var query = (	from PesCon in db.FuncionarioDependente.AsNoTracking()
							where 
								PesCon.id != idDesconsiderar &&
								PesCon.flagExcluido == "N"
							select
								PesCon
						);

			if (!String.IsNullOrEmpty(nroDocumento)) { 
				query = query.Where(x => x.nroDocumento == nroDocumento);
			}

			bool flagExiste = (query.Any());
			return flagExiste;

		}

		//Remover um registro logicamente
		public UtilRetorno excluir(int id) {

		    int idUsuarioLogado = User.id();

			db.FuncionarioDependente.Where(x => x.id == id)
							.Update(x => new FuncionarioDependente{ flagExcluido = "S", idUsuarioAlteracao = idUsuarioLogado, dtAlteracao = DateTime.Now});
			
			UtilRetorno Retorno = UtilRetorno.getInstance();
			Retorno.flagError = false;
			return Retorno;
		}
	}
}