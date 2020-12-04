using System.Data.Entity;
using System.Linq;
using BLL.Financeiro;
using DAL.Financeiro;

namespace BLL.AssociadosContribuicoes {

    public class TituloReceitaMensalidadeBL : TituloReceitaBL {

        //Atributos
        private AssociadoMensalidadeBL _AssociadoMensalidadeBL;

        //Propriedades
        private int idTipoReceita { get; set; }
        private AssociadoMensalidadeBL OAssociadoMensalidadeBL => (this._AssociadoMensalidadeBL = this._AssociadoMensalidadeBL ?? new AssociadoMensalidadeBL());

        //Construtor
        public TituloReceitaMensalidadeBL() {
            this.idTipoReceita = TipoReceitaConst.MENSALIDADE;
        }

        // Carregar um titulo a partir do tipo da receita e da receita
        public override TituloReceita carregarPorReceita(int id) {

            var query = from Tit in db.TituloReceita
                                        .Include(x => x.TipoReceita)
                                        .Include(x => x.Pessoa)
                        where
                            Tit.idReceita == id &&
                            Tit.idTipoReceita == idTipoReceita &&
                            Tit.dtExclusao == null
                        select
                            Tit;

            return query.FirstOrDefault();
        }


    }
}