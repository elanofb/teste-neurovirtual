using System;
using System.Linq;
using BLL.Financeiro;
using DAL.Financeiro;
using System.Data.Entity;

namespace BLL.Associados {

    public class TituloReceitaTaxaInscricaoBL : TituloReceitaBL, ITituloReceitaBL {

        //Atributos
        private ITipoAssociadoBL _TipoAssociadoBL;

        //Propriedades
		private int idTipoReceita { get; set; }
        private ITipoAssociadoBL OTipoAssociadoBL => _TipoAssociadoBL = _TipoAssociadoBL ?? new TipoAssociadoBL();

        //Eventos

		//Construtor
		public TituloReceitaTaxaInscricaoBL() {
			this.idTipoReceita = TipoReceitaConst.TAXA_INSCRICAO;
		}

		//
        public override TituloReceita carregarPorReceita(int idReceita) {

			var query = from Tit in db.TituloReceita
										.Include(x => x.Pessoa)
						where
							Tit.idReceita == idReceita && 
							Tit.idTipoReceita == idTipoReceita &&
							Tit.dtExclusao == null
						select
							Tit;

            query = query.condicoesSeguranca();

			return query.FirstOrDefault();
        }


    }
}
