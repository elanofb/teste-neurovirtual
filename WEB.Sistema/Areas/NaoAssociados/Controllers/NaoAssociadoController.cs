using System;
using System.Linq;
using System.Web.Mvc;
using BLL.NaoAssociados;
using PagedList;
using WEB.Areas.AssociadosConsultas.ViewModels;
using WEB.Areas.NaoAssociados.ViewModels;
using WEB.Helpers;
using MoreLinq;

namespace WEB.Areas.NaoAssociados.Controllers {

	public class NaoAssociadoController : Controller {

		//Atributos
		private INaoAssociadoRelatorioVWBL _NaoAssociadoRelatorioVWBL;
		
		//Propriedades
		private INaoAssociadoRelatorioVWBL ONaoAssociadoRelatorioVWBL => (this._NaoAssociadoRelatorioVWBL = this._NaoAssociadoRelatorioVWBL ?? new NaoAssociadoRelatorioVWBL());
		
	    //Listagem dos nao associados do sistema
		public ActionResult listar() {
            
            var idTipoAssociado = UtilRequest.getInt32("idTipoAssociado");

            var valorBusca = UtilRequest.getString("valorBusca");

            var ativo = UtilRequest.getString("flagAtivo");

            var flagTipoSaida = UtilRequest.getString("tipoSaida");

			var listaAssociados = this.ONaoAssociadoRelatorioVWBL.listar(idTipoAssociado, "", valorBusca, ativo);

            if (flagTipoSaida == TipoSaidaHelper.EXCEL) {
                var OAssociadoConsultaExportacao = new NaoAssociadoConsultaExportacao();
                OAssociadoConsultaExportacao.baixarExcel(listaAssociados.ToList());
            }

		    var query = listaAssociados.Select(x => new ItemListaNaoAssociado {
                id = x.id,
				nroAssociado = x.nroAssociado,
                flagTipoPessoa = x.flagTipoPessoa,
                nome = x.nome,
                descricaoTipoAssociado = x.descricaoTipoAssociado,
                razaoSocial = x.razaoSocial,
                nroDocumento = x.nroDocumento,
                nroTelefone = x.telefones,
                email = x.emails,
                dtCadastro = x.dtCadastro,
                ativo = x.ativo
		    });

			query = query.OrderBy(x => x.nome);

            var lista = query.ToPagedList(UtilRequest.getNroPagina(), UtilRequest.getNroRegistros());


            lista.ForEach(Item => {
                Item.nroTelefone = Item.nroTelefone.isEmpty() ? "" : Item.nroTelefone.Split(',').FirstOrDefault(x => !x.isEmpty());
                Item.email = Item.email.isEmpty() ? "" : Item.email.Split(',').FirstOrDefault(x => !x.isEmpty());
            });

            return View(lista);
		}
	}
}