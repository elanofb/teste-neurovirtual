using System;
using System.Data.Entity;
using System.Linq;
using BLL.Services;
using DAL.Funcionarios;
using DAL.Permissao.Security.Extensions;
using DAL.Repository.Base;
using EntityFramework.Extensions;

namespace BLL.Funcionarios {

	public class FuncionarioBeneficioBL : DefaultBL, IFuncionarioBeneficioBL {

		//Construtor
		public FuncionarioBeneficioBL(){
		}

		//Carregamento de um registro específico
		public FuncionarioBeneficio carregar(int id) { 

			var query = (	from PesCon in db.FuncionarioBeneficio
											.Include(x => x.Funcionario)
											.Include(x => x.Beneficio)
							where 
								PesCon.id == id &&
								PesCon.flagExcluido == "N"
							select
								PesCon
						);

			return query.FirstOrDefault();

		}

		//Listagem de registros de acordo com parametros informados
		public IQueryable<FuncionarioBeneficio> listar(int idFuncionario, string ativo) {

			var query = from Cont in db.FuncionarioBeneficio
									.Include(x => x.Funcionario)
									.Include(x => x.Beneficio)
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
		public bool salvar(FuncionarioBeneficio OBeneficio) {

			if (OBeneficio.id == 0) { 
				return this.inserir(OBeneficio);
			} else { 
				return this.atualizar(OBeneficio);
			}
		}

		//Persistir e inserir um novo registro 
		private bool inserir(FuncionarioBeneficio OBeneficio) { 
			
			OBeneficio.setDefaultInsertValues<FuncionarioBeneficio>();

			db.FuncionarioBeneficio.Add(OBeneficio);
			db.SaveChanges();
			return OBeneficio.id > 0;
		}

		//Persistir e atualizar um registro existente 
		private bool atualizar(FuncionarioBeneficio OBeneficio) { 
			
			//Localizar existentes no banco
			FuncionarioBeneficio dbBeneficio = this.carregar(OBeneficio.id);

			//Configurar valores padrão
			OBeneficio.setDefaultUpdateValues<FuncionarioBeneficio>();

			//Atualizacao da Empresa
			var BeneficioEntry = db.Entry(dbBeneficio);
			BeneficioEntry.CurrentValues.SetValues(OBeneficio);
			BeneficioEntry.ignoreFields<FuncionarioBeneficio>(new string[]{"idFuncionario"});

			db.SaveChanges();

			return OBeneficio.id > 0;
		}

		//Carregamento de um registro específico
		public bool existe(int idBeneficio, int idDesconsiderado) { 
	
			var query = (	from PesCon in db.FuncionarioBeneficio.AsNoTracking()
							where 
								PesCon.id != idDesconsiderado &&
								PesCon.flagExcluido == "N"
							select
								PesCon
						);

			if (idBeneficio > 0) { 
				query = query.Where(x => x.idBeneficio == idBeneficio);
			}

			bool flagExiste = (query.Any());
			return flagExiste;

		}

		//Remover um registro logicamente
		public UtilRetorno excluir(int id) {
		    int idUsuarioLogado = User.id();
            
            db.FuncionarioBeneficio.Where(x => x.id == id)
							.Update(x => new FuncionarioBeneficio{ flagExcluido = "S", idUsuarioAlteracao = idUsuarioLogado, dtAlteracao = DateTime.Now});
			
			UtilRetorno Retorno = UtilRetorno.getInstance();
			Retorno.flagError = false;
			return Retorno;
		}
	}
}