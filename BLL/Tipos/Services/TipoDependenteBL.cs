using System;
using System.Data.Entity;
using System.Linq;
using BLL.Services;
using DAL.Permissao.Security.Extensions;
using DAL.Tipos;
using DAL.Repository.Base;
using EntityFramework.Extensions;

namespace BLL.Tipos {

	public class TipoDependenteBL : DefaultBL, ITipoDependenteBL {

		//Construtor
		public TipoDependenteBL(){
		}

		//Carregamento de um registro específico
		public TipoDependente carregar(int id) { 

			var query = (	from PesCon in db.TipoDependente
							where 
								PesCon.id == id &&
								PesCon.flagExcluido == "N"
							select
								PesCon
						);

			return query.FirstOrDefault();

		}

		//Listagem de registros de acordo com parametros informados
		public IQueryable<TipoDependente> listar(string ativo) {

			var query = from Cont in db.TipoDependente
									.AsNoTracking()
						where Cont.flagExcluido == "N"
						select Cont;


			if (!String.IsNullOrEmpty(ativo)) { 
				query = query.Where(x => x.ativo == ativo);
			}

			return query;
		}

		//Definir se é um insert ou update e enviar o registro para o banco de dados 
		public bool salvar(TipoDependente ODependente) {

			if (ODependente.id == 0) { 
				return this.inserir(ODependente);
			} else { 
				return this.atualizar(ODependente);
			}
		}

		//Persistir e inserir um novo registro 
		private bool inserir(TipoDependente ODependente) { 
			
			ODependente.setDefaultInsertValues<TipoDependente>();

			db.TipoDependente.Add(ODependente);
			db.SaveChanges();
			return ODependente.id > 0;
		}

		//Persistir e atualizar um registro existente 
		private bool atualizar(TipoDependente ODependente) { 
			
			//Localizar existentes no banco
			TipoDependente dbDependente = this.carregar(ODependente.id);

			//Configurar valores padrão
			ODependente.setDefaultUpdateValues<TipoDependente>();

			db.SaveChanges();

			return ODependente.id > 0;
		}

		//Carregamento de um registro específico
		public bool existe(string descricao, int idDesconsiderar) { 
			
            var query = (	from PesCon in db.TipoDependente.AsNoTracking()
							where 
								PesCon.id != idDesconsiderar &&
								PesCon.flagExcluido == "N"
							select
								PesCon
						);

			if (!String.IsNullOrEmpty(descricao)) { 
				query = query.Where(x => x.descricao == descricao);
			}

			bool flagExiste = (query.Any());
			return flagExiste;

		}

		//Remover um registro logicamente
		public UtilRetorno excluir(int id) {
			int idUsuarioLogado = User.id();
			
			db.TipoDependente.Where(x => x.id == id)
							.Update(x => new TipoDependente{ flagExcluido = "S", idUsuarioAlteracao = idUsuarioLogado, dtAlteracao = DateTime.Now});
			
			UtilRetorno Retorno = UtilRetorno.getInstance();
			Retorno.flagError = false;
			return Retorno;
		}
	}
}