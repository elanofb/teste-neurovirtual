using System;
using System.Data.Entity;
using System.Linq;
using DAL.Hotsites;
using BLL.Services;

namespace BLL.Hotsites {

	public class HotsiteConsultaBL : DefaultBL, IHotsiteConsultaBL {

		//Constantes
		private static IHotsiteConsultaBL _instance;

		//Propriedades

		//Servicos
		public static IHotsiteConsultaBL getInstance => _instance = _instance ?? new HotsiteConsultaBL();
		
		//
		public HotsiteConsultaBL() {
		}

		// 
		public IQueryable<Hotsite> query(int? idOrganizacaoParam = null) {

			var query = from Hot in db.Hotsite
						where Hot.dtExclusao == null
						select Hot;
            
			if (idOrganizacaoParam == null) {
				idOrganizacaoParam = idOrganizacao;
			}

			if (idOrganizacaoParam > 0) {
				query = query.Where(x => x.idOrganizacao == idOrganizacaoParam);
			}

			return query;

		}
		
		//Carregar Hotsite pelo CNPJ
		public Hotsite carregar(int id) {
            
			var query = (from Hot in 
							 db.Hotsite 
						 where 
							 Hot.id == id && 
							 Hot.dtExclusao == null 
						 select Hot
						 );

            query = query.condicoesSeguranca();

			return query.FirstOrDefault();
		}
		
		//Listagem das Hotsites com possibilidade de busca pelos filtros
		public IQueryable<Hotsite> listar(string valorBusca, bool? ativo) {

			var query = db.Hotsite
							.Where(x => x.dtExclusao == null);

            query = query.condicoesSeguranca();

			if (!String.IsNullOrEmpty(valorBusca)) {

				query = query.Where(x => x.dominios.Contains(valorBusca));
			}

			if (!ativo.isEmpty()) {
				query = query.Where(x => x.ativo == ativo);
			}

			return query;
		}

	}
}