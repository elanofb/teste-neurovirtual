using System;
using System.Data.Entity;
using System.Linq;
using BLL.Services;
using DAL.Funcionarios;
using DAL.Permissao.Security.Extensions;
using DAL.Repository.Base;
using EntityFramework.Extensions;

namespace BLL.Funcionarios {

	public class FuncionarioFeriasBL : DefaultBL, IFuncionarioFeriasBL {

		//Construtor
		public FuncionarioFeriasBL(){
		}

		//Carregamento de um registro específico
		public FuncionarioFerias carregar(int id) { 

			var query = (	from PesCon in db.FuncionarioFerias
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
		public IQueryable<FuncionarioFerias> listar(int idFuncionario, string ativo) {

			var query = from Cont in db.FuncionarioFerias
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
		public bool salvar(FuncionarioFerias OContaBancaria) {

			if (OContaBancaria.id == 0) { 
				return this.inserir(OContaBancaria);
			} else { 
				return this.atualizar(OContaBancaria);
			}
		}

		//Persistir e inserir um novo registro 
		private bool inserir(FuncionarioFerias OContaBancaria) { 

			OContaBancaria.setDefaultInsertValues<FuncionarioFerias>();

			db.FuncionarioFerias.Add(OContaBancaria);
			db.SaveChanges();
			return OContaBancaria.id > 0;
		}

		//Persistir e atualizar um registro existente 
		private bool atualizar(FuncionarioFerias OContaBancaria) { 

			//Localizar existentes no banco
			FuncionarioFerias dbContaBancaria = this.carregar(OContaBancaria.id);

			//Configurar valores padrão
			OContaBancaria.setDefaultUpdateValues<FuncionarioFerias>();

			//Atualizacao da Empresa
			var ContaBancariaEntry = db.Entry(dbContaBancaria);
			ContaBancariaEntry.CurrentValues.SetValues(OContaBancaria);
			ContaBancariaEntry.ignoreFields<FuncionarioFerias>(new string[]{"idFuncionario"});

			db.SaveChanges();

			return OContaBancaria.id > 0;
		}

		//Carregamento de um registro específico
		public bool existe(DateTime? dtInicioFerias, DateTime? dtFimFerias, int idDesconsiderado) { 
			
			var query = (	from PesCon in db.FuncionarioFerias.AsNoTracking()
							where 
								PesCon.id != idDesconsiderado &&
								PesCon.flagExcluido == "N"
							select
								PesCon
						);

			if (dtInicioFerias != null && dtFimFerias != null) { 
				query = query.Where(x => x.dtInicioFerias == dtInicioFerias && x.dtFimFerias == dtFimFerias);
			}

			bool flagExiste = (query.Any());
			return flagExiste;

		}

		//Remover um registro logicamente
		public UtilRetorno excluir(int id) {
			int idUsuarioLogado = User.id();

			db.FuncionarioFerias.Where(x => x.id == id)
							.Update(x => new FuncionarioFerias{ flagExcluido = "S", idUsuarioAlteracao = idUsuarioLogado, dtAlteracao = DateTime.Now});
			
			UtilRetorno Retorno = UtilRetorno.getInstance();
			Retorno.flagError = false;
			return Retorno;
		}
	}
}