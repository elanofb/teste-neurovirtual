using System;
using System.Data.Entity;
using System.Linq;
using DAL.Associados;
using DAL.Repository.Base;
using EntityFramework.Extensions;

namespace BLL.Associados {

	public class AssociadoAbrangenciaBL : TableRepository<AssociadoAbrangencia>, IAssociadoAbrangenciaBL {

		//
		public AssociadoAbrangenciaBL() {
		}

		//Load de um registro a partir do ID
		public AssociadoAbrangencia carregar(int id) {
			var db = this.getDataContext();
			var query = from PesCar in db.AssociadoAbrangencia
										.Include(x => x.Associado)
										.Include(x => x.Associado.Pessoa)
										.Include(x => x.Cidade)
						where PesCar.id == id && PesCar.flagExcluido == "N"
						select PesCar;
			AssociadoAbrangencia OAssociadoAbrangencia = query.FirstOrDefault();
			return OAssociadoAbrangencia;
		}

		//Listagem de registros do banco a partir dos paramentros informados
		public IQueryable<AssociadoAbrangencia> listar(int idAssociado, string ativo) {
			var db = this.getDataContext();
			var query = (from PesCar in db.AssociadoAbrangencia
										.Include(x => x.Associado)
										.Include(x => x.Cidade)
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
		public bool existe(AssociadoAbrangencia OAssociadoAbrangencia, int idDesconsiderado) {
			var db = this.getDataContext();
			var query = (from PesCar in db.AssociadoAbrangencia
						where PesCar.id != idDesconsiderado && PesCar.flagExcluido == "N"
						select PesCar
						).AsNoTracking();

			query = query.Where(x => x.idAssociado == OAssociadoAbrangencia.idAssociado);
			query = query.Where(x => x.idCidade == OAssociadoAbrangencia.idCidade);

			var Item = query.Take(1).FirstOrDefault();
			return (Item == null ? false : true);
		}
		
		//Definir se é um insert ou update e enviar o registro para o banco de dados 
		public bool salvar(AssociadoAbrangencia OAssociadoAbrangencia) {
			
			var db = this.getDataContext();

		    OAssociadoAbrangencia.Cidade = null;

			if (OAssociadoAbrangencia.id == 0) { 
				return this.inserir(OAssociadoAbrangencia);
			} else { 
				return this.atualizar(OAssociadoAbrangencia);
			}
		}

		//Persistir e inserir um novo registro 
		private bool inserir(AssociadoAbrangencia OAssociadoAbrangencia) { 
			var db = this.getDataContext();

			OAssociadoAbrangencia.setDefaultInsertValues<AssociadoAbrangencia>();

			db.AssociadoAbrangencia.Add(OAssociadoAbrangencia);
			db.SaveChanges();
			return OAssociadoAbrangencia.id > 0;
		}

		//Persistir e atualizar um registro existente 
		private bool atualizar(AssociadoAbrangencia OAssociadoAbrangencia) { 
			var db = this.getDataContext();

			//Localizar existentes no banco
			AssociadoAbrangencia dbAbrangencia = this.carregar(OAssociadoAbrangencia.id);

			//Configurar valores padrão
			OAssociadoAbrangencia.setDefaultUpdateValues<AssociadoAbrangencia>();

			//Atualizacao da Empresa
			var AbrangenciaEntry = db.Entry(dbAbrangencia);
			AbrangenciaEntry.CurrentValues.SetValues(OAssociadoAbrangencia);
			AbrangenciaEntry.ignoreFields<AssociadoAbrangencia>(new string[]{"idAssociado", "ativo"});

			db.SaveChanges();

			return OAssociadoAbrangencia.id > 0;
		}

		//Remover um registro logicamente
		public UtilRetorno excluir(int idAbrangencia, int idAssociado) {

			var db = this.getDataContext();

			db.AssociadoAbrangencia.Where(x => x.idCidade == idAbrangencia && x.idAssociado == idAssociado)
							.Delete();
			
			UtilRetorno Retorno = UtilRetorno.getInstance();
			Retorno.flagError = false;
			return Retorno;
		}

		//Remover um registro logicamente
		public UtilRetorno excluirPorId(int id) {

			var db = this.getDataContext();

			db.AssociadoAbrangencia.Where(x => x.id == id)
							.Delete();
			
			UtilRetorno Retorno = UtilRetorno.getInstance();
			Retorno.flagError = false;
			return Retorno;
		}

    }
}