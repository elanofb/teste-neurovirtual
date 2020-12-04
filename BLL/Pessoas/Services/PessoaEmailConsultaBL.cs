using System.Linq;
using BLL.Services;
using DAL.Pessoas;
using System.Data.Entity;

namespace BLL.Pessoas {

	public class PessoaEmailConsultaBL : DefaultBL, IPessoaEmailConsultaBL {

		//
		public PessoaEmailConsultaBL() {
		}

		/// <summary>
        /// Carregar um Email a partir do id
        /// </summary>
		public PessoaEmail carregar(int id) {

			var query = from PesEnd in db.PessoaEmail
						where PesEnd.id == id &&
                              PesEnd.dtExclusao == null
						select PesEnd;
            
			return query.FirstOrDefault();
		}
		
		/// <summary>
        /// Carregar um Email a partir do idPessoa e idTipoEmail
        /// </summary>
		public PessoaEmail carregar(int idPessoa, int idTipoEmail) {

			var query = from PesEnd in db.PessoaEmail
						where PesEnd.idPessoa == idPessoa && 
						      PesEnd.idTipoEmail == idTipoEmail &&
                              PesEnd.dtExclusao == null
						select PesEnd;
            
			return query.FirstOrDefault();
		}

		/// <summary>
        /// Montagem de query conforme paramentros informados
        /// </summary>
		public IQueryable<PessoaEmail> listar(int idPessoa) {

			var query = from PesEnd in db.PessoaEmail.Include(x => x.TipoEmail)
						where PesEnd.dtExclusao == null
						select PesEnd;

		    if (idPessoa > 0) {
		        query = query.Where(x => x.idPessoa == idPessoa);
		    }

			return query;

		}

	}
}