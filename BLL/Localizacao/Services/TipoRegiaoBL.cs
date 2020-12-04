using System.Linq;
using BLL.Services;
using DAL.Localizacao;

namespace BLL.Localizacao {

	public class TipoRegiaoBL : DefaultBL, ITipoRegiaoBL {

        //Atributos

        //Propriedades

		//Construtor
		public TipoRegiaoBL(){
		}

        /// <summary>
        /// Carregar registro a partir do ID
        /// </summary>
	    public TipoRegiao carregar(int id) {
	        
            var query = from Item in db.TipoRegiao
                        where Item.flagExcluido == "N" && Item.id == id
                        select Item;

	        return query.FirstOrDefault();
	    }

		/// <summary>
        /// Listagem de registros de acordo com parametros informados
        /// </summary>
	    public IQueryable<TipoRegiao> listar(string valorBusca, string ativo) {
	        
            var query = from Item in db.TipoRegiao
                        where Item.flagExcluido == "N"
                        select Item;

	        if (!string.IsNullOrEmpty(valorBusca)) {

	            query = query.Where(x => x.descricao.Contains(valorBusca));

	        }

	        if (!string.IsNullOrEmpty(ativo)) {
	            
                query = query.Where(x => x.ativo == ativo);

	        }

	        return query;

	    } 
	}
}