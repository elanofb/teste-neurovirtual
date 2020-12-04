using System;
using System.Data.Entity;
using System.Linq;
using BLL.Financeiro;
using DAL.Financeiro;

namespace BLL.ContasBancarias {

	public class TituloReceitaMovimentacaoBL : TituloReceitaBL {

		//Atributos

		//Propriedades
        private int idTipoReceita { get; set; }

        /// <summary>
        /// Construtor
        /// </summary>
        public TituloReceitaMovimentacaoBL() {

            this.idTipoReceita = TipoReceitaConst.TRANSFERENCIA;

        }

	    /// <summary>
	    /// Montagem de consulta com condições específicas
	    /// </summary>
	    public override IQueryable<TituloReceita> query(int? idOrganizacaoParam = null){

	        var queryPedido = base.query(idOrganizacaoParam);

	        queryPedido = queryPedido.Where(x => x.idTipoReceita == this.idTipoReceita);
            
	        return queryPedido;

	    }

		// Carregar um titulo a partir do tipo da receita e da receita
		public override TituloReceita carregarPorReceita(int id) {
			
			var query = from Tit in db.TituloReceita
										.Include(x => x.CentroCusto)
										.Include(x => x.Pessoa)
						where
							Tit.idReceita == id && 
							Tit.idTipoReceita == this.idTipoReceita &&
							Tit.dtExclusao == null
						select
							Tit;

		    query = query.condicoesSeguranca();

			return query.FirstOrDefault();
		}

	}

}