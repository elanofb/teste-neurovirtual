using System;
using System.Json;
using System.Linq;
using DAL.Repository.Base;
using EntityFramework.Extensions;
using DAL.Popups;
using BLL.Services;
using DAL.Permissao.Security.Extensions;
using UTIL.Resources;

namespace BLL.Popups {

	public class HomePopupBL : DefaultBL, IHomePopupBL {

        //Atributos

        //Propriedades

        //Construtor
		public HomePopupBL() {

        }
		
        //
		public IQueryable<HomePopup> listar(string valorBusca = "", bool? ativo = true, int? idPortal = 0) {
			var query = from N in db.HomePopup
						where N.flagExcluido == false
						select N;

            query = query.condicoesSeguranca();

            if (idPortal > 0) {
                query = query.Where(x => x.idPortal == idPortal);
            }

			if (!String.IsNullOrEmpty(valorBusca)) {
				query = query.Where(x => x.titulo.Contains(valorBusca) || x.conteudo.Contains(valorBusca));
			}

			if (ativo != null) {
				query = query.Where(x => x.ativo == ativo);
			}

			return query;
		}

		//
		public HomePopup carregar(int id) {

            var query = db.HomePopup.Where(x => x.id == id);

            query = query.condicoesSeguranca();

			return query.FirstOrDefault();
		}

        public HomePopup carregarPopupDisponivel() { 
            var dtFiltro = DateTime.Today.AddDays(-1);
            var query = db.HomePopup.Where(x => x.ativo == true && x.flagExcluido == false &&
                                           DateTime.Today >= x.dtInicioExibicao &&
                                           dtFiltro < x.dtFimExibicao);

            query = query.condicoesSeguranca();

            return query.FirstOrDefault();
        }

		//
		public bool salvar(HomePopup OHomePopup) {

            if (OHomePopup.ativo == true) { 
                this.desativarTodos();
            }

			if(OHomePopup.id == 0) {
                return this.inserir(OHomePopup);
            }

            return this.atualizar(OHomePopup);
		}

        private void desativarTodos() { 

            var idOrganizacao = User.idOrganizacao();

            db.HomePopup.Where(x => x.idOrganizacao == idOrganizacao).Update(x => new HomePopup { ativo = false, dtAlteracao = DateTime.Now });
        }

        private bool inserir(HomePopup OHomePopup) {
            OHomePopup.setDefaultInsertValues<HomePopup>();
            db.HomePopup.Add(OHomePopup);
            db.SaveChanges();
            return (OHomePopup.id > 0);
        }

        private bool atualizar(HomePopup OHomePopup) {

            HomePopup dbHomePopup = this.carregar(OHomePopup.id);

            if (dbHomePopup == null) {
                return false;
            }

            var TipoEntry = db.Entry(dbHomePopup);
			OHomePopup.setDefaultUpdateValues<HomePopup>();
            TipoEntry.CurrentValues.SetValues(OHomePopup);
            TipoEntry.State = System.Data.Entity.EntityState.Modified;
            TipoEntry.ignoreFields<HomePopup>();
            
            db.SaveChanges();
            return (OHomePopup.id > 0);
        }

        //Alteracao de status
        public JsonMessageStatus alterarStatus(int id) {
	        var retorno = new JsonMessageStatus();

	        var item = this.carregar(id);

	        if (item == null) {
		        retorno.error = true;
		        retorno.message = NotificationMessages.invalid_register_id;
	        } else {
		        item.ativo = item.ativo != true;
		        db.SaveChanges();
		        retorno.active = item.ativo == true ? "S" : "N";
		        retorno.message = NotificationMessages.updateSuccess;
	        }
	        return retorno;
        }

		//
		public bool excluir(int[] ids) {
			db.HomePopup.Where(x => ids.Contains(x.id))
				.Update(x => new HomePopup { flagExcluido = true, dtAlteracao = DateTime.Now });

			var listaCheck = db.HomePopup.Where(x => ids.Contains(x.id) && x.flagExcluido == false).ToList();
			return (listaCheck.Count == 0);
		}

		//
		public bool existe(string titulo, int id) {

			var query = from C in db.HomePopup
						where C.titulo == titulo && C.id != id && C.flagExcluido == false
						select C;

            query = query.condicoesSeguranca();

			var OBanco = query.Take(1).FirstOrDefault();

			return (OBanco == null ? false : true);

		}
	}
}