using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using BLL.Localizacao;
using DAL.Localizacao;
using System.Threading.Tasks;

namespace WEB.Areas.Localizacao.Controllers {
    [AllowAnonymous]
    public class DefaultController : Controller {

        //Constantes

        //Atributos
        private ICidadeBL _CidadeBL;
        private CepBrasilBL _CepBrasilBL;

        //Propriedades
        private ICidadeBL OCidadeBL { get { return (this._CidadeBL = this._CidadeBL ?? new CidadeBL()); } }
        private CepBrasilBL OCepBrasilBL { get { return (this._CepBrasilBL = this._CepBrasilBL ?? new CepBrasilBL()); } }

        /**
		 * 
		 */
        [ActionName("carregar-cidades")]
        public ActionResult carregarCidades() {

            int idEstado = UtilRequest.getInt32("idEstado");

            if (idEstado == 0) {

                return Json(new List<Cidade>(), JsonRequestBehavior.AllowGet);

            }

            List<Cidade> listaCidades = OCidadeBL.listar(idEstado, "", "S").ToList();

            return Json(listaCidades, JsonRequestBehavior.AllowGet);
        }

        /**
		 * 
		 */
        [ActionName("buscar-endereco")]
        public async Task<ActionResult> buscarEndereco(string cep) {

            CepBrasil CepBrasil = await this.OCepBrasilBL.buscarEndereco(cep);

            return Json(CepBrasil, JsonRequestBehavior.AllowGet);
        }

        /**
		 * 
		 */
        public ActionResult buscarAutocomplete(string term, int? idEstado) {
            var query = this.OCidadeBL.autocompletar(UtilNumber.toInt32(idEstado), term);
            var listaCidades = query.ToList();
            return Json(listaCidades, JsonRequestBehavior.AllowGet);
        }

    }
}