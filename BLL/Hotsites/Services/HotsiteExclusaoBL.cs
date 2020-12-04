using System;
using System.Linq;
using DAL.Hotsites;
using EntityFramework.Extensions;
using DAL.Permissao.Security.Extensions;

namespace BLL.Hotsites {

	public class HotsiteExclusaoBL : HotsiteConsultaBL, IHotsiteExclusaoBL {

		//
		public HotsiteExclusaoBL() {
		}

		//Excluir um Hotsite logicamente
		public bool excluir(int id) {
			
			int idUsuarioLogado = User.id();

			db.Hotsite
				.Where(x => x.id == id)
				.Update(x => new Hotsite { dtExclusao = DateTime.Now, idUsuarioExclusao = idUsuarioLogado });

			return true;
			
		}

	}
}