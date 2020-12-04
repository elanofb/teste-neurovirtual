using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using BLL.Financeiro;
using DAL.Financeiro;
using DAL.Financeiro.Entities;

namespace WEB.Areas.Financeiro.Controllers {

    public class DescontoAntecipacaoController : Controller {

        //Atributos
        private ITituloReceitaBL _TituloReceitaBL;

        //Propriedades
        private ITituloReceitaBL OTituloReceitaBL => _TituloReceitaBL = _TituloReceitaBL ?? new TituloReceitaPadraoBL();

        /// <summary>
        /// Exibir detalhamento dos descontos por antecipação (se houverem)
        /// </summary>
        [ActionName("modal-detalhe-descontos")]
        public PartialViewResult modalDetalheDescontos(int? idTituloReceita){

            int id = idTituloReceita.toInt();

            var OTituloReceita = OTituloReceitaBL.listar(0, 0, 0, "", false)
                                                 .Include(x => x.listaDescontosAntecipacao)
                                                 .FirstOrDefault(x => x.id == id) ?? new TituloReceita();

            OTituloReceita.listaDescontosAntecipacao = OTituloReceita.listaDescontosAntecipacao ?? new List<TituloReceitaDescontoAntecipacao>();

            return PartialView(OTituloReceita);

        }
    }
}