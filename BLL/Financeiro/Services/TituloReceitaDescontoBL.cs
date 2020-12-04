using System;
using System.Linq;
using BLL.CuponsDesconto;
using BLL.Services;
using DAL.Financeiro;
using EntityFramework.Extensions;

namespace BLL.Financeiro {

    public class TituloReceitaDescontoBL : DefaultBL, ITituloReceitaDescontoBL {

        //Atributos
        private ICupomDescontoBL _CupomDescontoBL;
        private ITituloReceitaBL _TituloReceitaBL;

        //Propriedades
        private ICupomDescontoBL OCupomDescontoBL => _CupomDescontoBL = _CupomDescontoBL ?? new CupomDescontoBL();
        private ITituloReceitaBL OTituloReceitaBL => _TituloReceitaBL = _TituloReceitaBL ?? new TituloReceitaPadraoBL();


        //Salvar os dados do cupom de desconto
        public TituloReceita salvarCupomDesconto(int idTituloReceita, int idCupomDesconto) {

            var OTitulo = this.OTituloReceitaBL.carregar(idTituloReceita);

            var OCupom = this.OCupomDescontoBL.carregar(idCupomDesconto);

            if (OCupom.dtUso.HasValue) {
                return OTitulo;
            }


            db.TituloReceita.Where(x => x.id == idTituloReceita)
                            .Update(x => new TituloReceita {
                                   idCupomDesconto = OCupom.id,
                                   valorDesconto = OCupom.valorDesconto,
                                   motivoDesconto = $"Utilização do cupom de desconto Nº {OCupom.codigo}",
                                }
                            );

            this.OCupomDescontoBL.registrarUso(OCupom.id);

            return OTitulo;
        }
    }
}
