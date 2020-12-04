using System.Linq;
using BLL.Services;
using DAL.Pessoas;
using System.Data.Entity;

namespace BLL.Pessoas {

	public class PessoaTelefoneConsultaBL : DefaultBL, IPessoaTelefoneConsultaBL {

		//
		public PessoaTelefoneConsultaBL() {
		}

		/// <summary>
        /// Carregar um Telefone a partir do id
        /// </summary>
		public PessoaTelefone carregar(int id) {

			var query = from PesEnd in db.PessoaTelefone
						where PesEnd.id == id &&
                              PesEnd.dtExclusao == null
						select PesEnd;
            
			return query.FirstOrDefault();
		}
		
		/// <summary>
        /// Carregar um Telefone a partir do idPessoa e idTipoTelefone
        /// </summary>
		public PessoaTelefone carregar(int idPessoa, int idTipoTelefone) {

			var query = from PesEnd in db.PessoaTelefone
						where PesEnd.idPessoa == idPessoa && 
						      PesEnd.idTipoTelefone == idTipoTelefone &&
                              PesEnd.dtExclusao == null
						select PesEnd;
            
			return query.FirstOrDefault();
		}

		/// <summary>
        /// Montagem de query conforme paramentros informados
        /// </summary>
		public IQueryable<PessoaTelefone> listar(int idPessoa) {

			var query = from PesEnd in db.PessoaTelefone.Include(x => x.TipoTelefone)
						where PesEnd.dtExclusao == null
						select PesEnd;

		    if (idPessoa > 0) {
		        query = query.Where(x => x.idPessoa == idPessoa);
		    }

			return query;

		}

	}
}