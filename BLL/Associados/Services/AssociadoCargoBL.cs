using System;
using System.Linq;
using BLL.Services;
using DAL.Associados;
using System.Data.Entity;
using DAL.Permissao.Security.Extensions;
using EntityFramework.Extensions;

namespace BLL.Associados {

	public class AssociadoCargoBL : DefaultBL, IAssociadoCargoBL {

		//Load de um registro a partir do ID
		public AssociadoCargo carregar(int id) {

			var query = from PesCar in db.AssociadoCargo
										.Include(x => x.Associado)
										.Include(x => x.Associado.Pessoa)
										.Include(x => x.Cargo)
						where PesCar.id == id && PesCar.flagExcluido == "N"
						select PesCar;
			AssociadoCargo OAssociadoCargo = query.FirstOrDefault();
			return OAssociadoCargo;
		}

		//Listagem de registros do banco a partir dos parâmetros informados
		public IQueryable<AssociadoCargo> listar(int idAssociado, string ativo) {

			var query = (from PesCar in db.AssociadoCargo
										.Include(x => x.Associado)
										.Include(x => x.Cargo)
						where PesCar.flagExcluido == "N"
						select PesCar).AsNoTracking();

			if (idAssociado > 0) {
				query = query.Where(x => x.idAssociado == idAssociado);
			}

			if (!String.IsNullOrEmpty(ativo)) {
				query = query.Where(x => x.ativo == ativo);
			}

			return query;
		}
		
		//Verificar se já existe registr dentro do mesmo período para evitar duplicidades
		public bool existe(AssociadoCargo OAssociadoCargo, int idDesconsiderado) {

			var query = (from PesCar in db.AssociadoCargo
						where PesCar.id != idDesconsiderado && PesCar.flagExcluido == "N"
						select PesCar
						).AsNoTracking();

			query = query.Where(x => x.idAssociado == OAssociadoCargo.idAssociado);
			query = query.Where(x => x.idCargo == OAssociadoCargo.idCargo);
			query = query.Where(x => x.inicioGestao == OAssociadoCargo.inicioGestao);
			query = query.Where(x => x.fimGestao == OAssociadoCargo.fimGestao);

			var Item = query.Take(1).FirstOrDefault();
			return (Item != null);
		}
		
		//Definir se é um insert ou update e enviar o registro para o banco de dados 
		public bool salvar(AssociadoCargo OAssociadoCargo) {

			if (OAssociadoCargo.id == 0) { 
				return this.inserir(OAssociadoCargo);
			} else { 
				return this.atualizar(OAssociadoCargo);
			}
		}

		//Persistir e inserir um novo registro 
		private bool inserir(AssociadoCargo OAssociadoCargo) { 

			OAssociadoCargo.setDefaultInsertValues<AssociadoCargo>();

			db.AssociadoCargo.Add(OAssociadoCargo);
			db.SaveChanges();
			return OAssociadoCargo.id > 0;
		}

		//Persistir e atualizar um registro existente 
		private bool atualizar(AssociadoCargo OAssociadoCargo) { 

			//Localizar existentes no banco
			AssociadoCargo dbCargo = this.carregar(OAssociadoCargo.id);

			//Configurar valores padrão
			OAssociadoCargo.setDefaultUpdateValues<AssociadoCargo>();

			//Atualizacao da Empresa
			var CargoEntry = db.Entry(dbCargo);
			CargoEntry.CurrentValues.SetValues(OAssociadoCargo);
			CargoEntry.ignoreFields<AssociadoCargo>(new string[]{"idAssociado"});

			db.SaveChanges();

			return OAssociadoCargo.id > 0;
		}

		//Remover um registro logicamente
		public UtilRetorno excluir(int id) {
			int idUsuarioLogado = User.id();

			db.AssociadoCargo.Where(x => x.id == id)
							.Update(x => new AssociadoCargo{ flagExcluido = "S", dtAlteracao = DateTime.Now, idUsuarioAlteracao = idUsuarioLogado});
			
			UtilRetorno Retorno = UtilRetorno.getInstance();
			Retorno.flagError = false;
			return Retorno;
		}
	}
}