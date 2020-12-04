using System;
using System.Linq;
using BLL.Services;
using DAL.Diretorias;

namespace BLL.Diretorias {

	public class DiretoriaVWBL : DefaultBL, IDiretoriaVWBL {

        //Atributos

        //Propriedades

		//
		public DiretoriaVWBL(){
		}

        //Carregamento de registro único pelo ID
		public DiretoriaVW carregar(int id) {
			
			var query = from Item in db.DiretoriaVW
                        where 
							Item.id == id && 
							Item.flagExcluido == false
						select Item;

            query = query.condicoesSeguranca();

            return query.FirstOrDefault();
		}

		//
		public IQueryable<DiretoriaVW> listar(string valorBusca, bool? ativo) {
			
			var query = from C in db.DiretoriaVW
                        where C.flagExcluido == false
						select C;

            query = query.condicoesSeguranca();

			if (!String.IsNullOrEmpty(valorBusca)) {
				query = query.Where(x => valorBusca.Contains(x.anoFimGestao.ToString()) || valorBusca.Contains(x.anoInicioGestao.ToString()));
			}

			if (ativo != null) {
				query = query.Where(x => x.ativo == ativo);
			}

			return query;
		}
	}
}