using System.Linq;
using BLL.Core.Events;
using BLL.Financeiro.Events;
using BLL.Services;
using DAL.Financeiro;
using EntityFramework.Extensions;

namespace BLL.Financeiro {

    public abstract class TituloReceitaQuitacaoBL : DefaultBL, ITituloReceitaQuitacaoBL{

        //Atributos

        //Propriedades

        //eventos

        readonly EventAggregator onTituloQuitado = OnTituloQuitado.getInstance;


        //Registrar a liquidacao do titulo (pagamento total)
        //Cada classe filha pode sobreescrever esse metodo para que as acoes de pagamento sejam feitas
        public virtual TituloReceita liquidar(TituloReceita OTituloReceita) {

            var listaParcelas = this.db.TituloReceitaPagamento
                                                .Where(x => x.idTituloReceita == OTituloReceita.id && x.dtExclusao == null)
                                                .ToList();

            var listaParcelasPendentes = listaParcelas.Where(x => x.dtPagamento == null).ToList();

            if(listaParcelasPendentes.Count > 0) {

                return OTituloReceita;

            }

            OTituloReceita.dtQuitacao = listaParcelas.Max(x => x.dtPagamento);

            this.db.TituloReceita.Where(x => x.id == OTituloReceita.id)
                                 .Update(x => new TituloReceita {
                                     dtQuitacao = OTituloReceita.dtQuitacao,
                                     ativo = true
                                 }
                                );

            this.onTituloQuitado.subscribe(new OnTituloQuitadoHandler());

            this.onTituloQuitado.publish((OTituloReceita as object));

            return OTituloReceita;
        }


    }
}