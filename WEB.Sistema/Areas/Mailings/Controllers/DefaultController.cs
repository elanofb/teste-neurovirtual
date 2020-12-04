using System;
using System.Linq;
using System.Web.Mvc;
using PagedList;
using BLL.Mailings;
using WEB.Helpers;
using DAL.Mailings;
using System.Web.UI.WebControls;
using MvcFlashMessages;

namespace WEB.Areas.Mailings.Controllers {

	public class DefaultController : Controller {

		//Constantes

		//Atributos
		private MailingBL _MailingBL;

		//Propriedades
		private IMailingBL OMailingBL => _MailingBL = _MailingBL ?? new MailingBL();
		
		//Construtor
		public DefaultController() { 
				
		}


		//
		public ActionResult index() {

            this.Flash(UtilMessage.TYPE_MESSAGE_INFO, "<strong>Instruções:</strong> Utilize o módulo de mailings para exportar sua base de contatos e utilizá-la para campanhas, promoções, etc.");

			var descricao = UtilRequest.getString("valorBusca");
            var flagTipoSaida = UtilRequest.getString("tipoSaida");

			var listaMailings = this.OMailingBL.listar(descricao, "S", 0, 0).OrderBy(x => x.nome);

            if (flagTipoSaida == TipoSaidaHelper.EXCEL) {
                this.baixarExcel(listaMailings);
            }

			return View(listaMailings.ToPagedList(UtilRequest.getNroPagina(), UtilRequest.getNroRegistros()));
		}

        //Exportacao do cadastro para formato EXCEL
		//Download do documento gerado
        public void baixarExcel(IQueryable<Mailing> listaMailings) {

            var OlistaMailings = listaMailings.Select(x => new {
                                                    ID = x.id,
                                                    Nome = x.nome,
                                                    Email = x.email,
                                                    Cadastro = x.dtCadastro.Day + "/" + x.dtCadastro.Month + "/" + x.dtCadastro.Year
                                                }).ToList();

     
            var grid = new GridView();
            grid.DataSource = OlistaMailings;
            grid.DataBind();
            new UTIL.Excel.UtilExcel().downloadExcel(this.HttpContext.Response,grid, "ListaMailing.xls");
        }

		//
		[HttpPost]
		public ActionResult excluir(int[] id) {
			return Json(this.OMailingBL.excluir(id[0]));
		}
	}
}