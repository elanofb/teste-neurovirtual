using System;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using BLL.Services;
using PagedList;
using BLL.Transacoes.Movimentos;
using DAL.Transacoes;
using WEB.Areas.Movimentacoes.ViewModels;
using WEB.Helpers;

namespace WEB.Areas.Movimentacoes.Controllers {

    public class MovimentoPedidoConsultaController : Controller {

        // Atributos
        private IExtratoPedidoConsultaBL _ExtratoConsultaBL { get; set; }

        // Propriedades
        private IExtratoPedidoConsultaBL OExtratoConsultaBL => this._ExtratoConsultaBL = this._ExtratoConsultaBL ?? new ExtratoPedidoConsultaBL();

        //Listagem para consulta de Relacionamentos existentes
        [ActionName("partial-lista-movimento")]
        public ActionResult partialListaMovimento() {

            int idPedido = UtilRequest.getInt32("idPedido");

            byte idTipoTransacao = (byte)TipoTransacaoEnum.GANHO_PLANOS;

            var query = OExtratoConsultaBL.query(idPedido, 0, null, null).Where(x => x.idPedido == idPedido && x.idTipoTransacao == idTipoTransacao);
            
            var listaOcorrencias = query.ToList().OrderByDescending(x => x.dtCadastro).ToList();

            return View(listaOcorrencias);
        }



    }
}