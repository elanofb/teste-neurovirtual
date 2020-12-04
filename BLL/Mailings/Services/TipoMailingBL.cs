using System;
using System.Data.Entity;
using System.Linq;
using DAL.Mailings;
using DAL.Repository.Base;
using EntityFramework.Extensions;

namespace BLL.Mailings {

	public class TipoMailingBL : TableRepository<TipoMailing>, ITipoMailingBL {

		//Construtor
		public TipoMailingBL(){
		}

		//Carregamento de um registro específico
		public TipoMailing carregar(int id) { 
			var db = this.getDataContext();

			var query = (	from Tipo in db.TipoMailing
							where 
								Tipo.id == id &&
								Tipo.flagExcluido == "N"
							select
								Tipo
						);

			return query.FirstOrDefault();

		}

		//Listagem de registros de acordo com parametros informados
		public IQueryable<TipoMailing> listar(string ativo, string descricao) {
			var db = this.getDataContext();
			var query = from Cont in db.TipoMailing
									.AsNoTracking()
						where Cont.flagExcluido == "N"
						select Cont;


			if (!String.IsNullOrEmpty(ativo)) { 
				query = query.Where(x => x.ativo == ativo);
			}

            if(!String.IsNullOrEmpty(descricao)) {
                query = query.Where(x => x.descricao.Contains(descricao));
            }

            return query;
		}

		//Definir se é um insert ou update e enviar o registro para o banco de dados 
		public bool salvar(TipoMailing ODependente) {
			
			var db = this.getDataContext();

			if (ODependente.id == 0) { 
				return this.inserir(ODependente);
			} else { 
				return this.atualizar(ODependente);
			}
		}

		//Persistir e inserir um novo registro 
		private bool inserir(TipoMailing ODependente) { 
			var db = this.getDataContext();

			ODependente.setDefaultInsertValues<TipoMailing>();

			db.TipoMailing.Add(ODependente);
			db.SaveChanges();
			return ODependente.id > 0;
		}

		//Persistir e atualizar um registro existente 
		private bool atualizar(TipoMailing OTipoMailing) {
            var db = this.getDataContext();

            TipoMailing dbTipoMailing = this.carregar(OTipoMailing.id);

            var entryAssociado = db.Entry(dbTipoMailing);
            OTipoMailing.setDefaultUpdateValues();
            entryAssociado.CurrentValues.SetValues(OTipoMailing);
            entryAssociado.State = EntityState.Modified;

            db.SaveChanges();

            return (OTipoMailing.id > 0);
		}

		//Carregamento de um registro específico
		public bool existe(string descricao, int idDesconsiderar) { 
			var db = this.getDataContext();

			var query = (	from PesCon in db.TipoMailing.AsNoTracking()
							where 
								PesCon.id != idDesconsiderar &&
								PesCon.flagExcluido == "N"
							select
								PesCon
						);

			if (!String.IsNullOrEmpty(descricao)) { 
				query = query.Where(x => x.descricao == descricao);
			}

			bool flagExiste = (query.Count() > 0);
			return flagExiste;

		}

		//Remover um registro logicamente
		public bool excluir(int id) {
            this.getDataContext().TipoMailing.Where(x => x.id == id)
                .Update(x => new TipoMailing { flagExcluido = "S", dtAlteracao = DateTime.Now });

            var listaCheck = this.getDataContext().TipoMailing.Where(x => x.id == id && x.flagExcluido == "N").ToList();
            return (listaCheck.Count == 0);
        }
	}
}