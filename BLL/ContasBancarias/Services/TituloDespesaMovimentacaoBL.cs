using System;
using System.Data.Entity;
using System.Linq;
using BLL.Financeiro;
using DAL.Financeiro;

namespace BLL.ContasBancarias {

	public class TituloDespesaMovimentacaoBL : TituloDespesaBL {

		//Atributos

		//Propriedades
        private int? idTipoDespesa { get; set; }

        /// <summary>
        /// Construtor
        /// </summary>
        public TituloDespesaMovimentacaoBL() {

            this.idTipoDespesa = null;

        }

	    /// <summary>
	    /// Montagem de consulta com condições específicas
	    /// </summary>
	    public override IQueryable<TituloDespesa> query(int? idOrganizacaoParam = null){

	        var queryPedido = base.query(idOrganizacaoParam);

	        queryPedido = queryPedido.Where(x => x.idTipoDespesa == this.idTipoDespesa);
            
	        return queryPedido;

	    }

		// Carregar um titulo a partir do tipo da Despesa e da Despesa
		public override TituloDespesa carregarPorDespesa(int id) {
			
			var query = from Tit in db.TituloDespesa
										.Include(x => x.CentroCusto)
										.Include(x => x.Pessoa)
						where
							Tit.idDespesa == id && 
							Tit.idTipoDespesa == this.idTipoDespesa &&
							Tit.dtExclusao == null
						select
							Tit;

		    query = query.condicoesSeguranca();

			return query.FirstOrDefault();
		}

	}

}