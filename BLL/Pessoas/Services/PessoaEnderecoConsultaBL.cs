using System.Linq;
using BLL.Services;
using DAL.Pessoas;
using System.Data.Entity;

namespace BLL.Pessoas {

	public class PessoaEnderecoConsultaBL : DefaultBL, IPessoaEnderecoConsultaBL {

		//
		public PessoaEnderecoConsultaBL() {
		}

		/// <summary>
        /// Carregar um endereco a partir do id
        /// </summary>
		public PessoaEndereco carregar(int id) {

			var query = from PesEnd in db.PessoaEndereco
						where PesEnd.id == id &&
                              PesEnd.dtExclusao == null
						select PesEnd;
            
			return query.FirstOrDefault();
		}
		
		/// <summary>
        /// Carregar um endereco a partir do idPessoa e idTipoEndereco
        /// </summary>
		public PessoaEndereco carregar(int idPessoa, int idTipoEndereco) {

			var query = from PesEnd in db.PessoaEndereco
						where PesEnd.idPessoa == idPessoa && 
						      PesEnd.idTipoEndereco == idTipoEndereco &&
                              PesEnd.dtExclusao == null
						select PesEnd;
            
			return query.FirstOrDefault();
		}

		/// <summary>
        /// Montagem de query conforme paramentros informados
        /// </summary>
		public IQueryable<PessoaEndereco> listar(int idPessoa) {

			var query = from PesEnd in db.PessoaEndereco.Include(x => x.Estado).Include(x => x.Cidade).Include(x => x.TipoEndereco)
						where PesEnd.dtExclusao == null
						select PesEnd;

		    if (idPessoa > 0) {
		        query = query.Where(x => x.idPessoa == idPessoa);
		    }

			return query;

		}

	}
}