using System;
using System.Data.Entity;
using System.Linq;
using BLL.Financeiro;
using DAL.Financeiro;

namespace BLL.AssociadosContribuicoes {

    public class TituloReceitaContribuicaoBL : TituloReceitaBL {

        //Atributos

        //Propriedades
        private int idTipoReceita { get; set; }

        //Eventos

        //Construtor
        public TituloReceitaContribuicaoBL() {

            this.idTipoReceita = TipoReceitaConst.CONTRIBUICAO;

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