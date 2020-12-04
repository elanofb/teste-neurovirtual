using BLL.Localizacao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using UTIL.UtilClasses;
using WEB.App_Infrastructure;

namespace WEB.Areas.Localizacao.Controllers {

    public class UtilidadeJsonController : BaseSistemaController {
        
        // Atributos
        public IEstadoBL _IEstadoBL { get; set; }
        public ICidadeBL _ICidadeBL { get; set; }
        public IPaisBL _IPaisBL { get; set; }

        // Propriedades
        private IEstadoBL OEstadoBL => _IEstadoBL = _IEstadoBL ?? new EstadoBL();
        private ICidadeBL OCidadeBL => _ICidadeBL = _ICidadeBL ?? new CidadeBL();
        private IPaisBL OPaisBL => _IPaisBL = _IPaisBL ?? new PaisBL();

        //
		public UtilidadeJsonController() { 

		} 

		[ActionName("listar-json-estados")]
        public JsonResult listarJsonEstados() {

            var listaEstados = this.OEstadoBL.listar("", "")
                                   .Select(x => new { value = x.id, text = x.sigla }).ToList();

		    return Json(listaEstados, JsonRequestBehavior.AllowGet);

        }

        [ActionName("listar-json-cidades")]
        public JsonResult listarJsonCidades() {

            var idEstado = UtilRequest.getInt32("idEstado");

            if (idEstado == 0) {
                return Json(new List<OptionSelect>(), JsonRequestBehavior.AllowGet);
            }

            var listaCidades = this.OCidadeBL.listar(idEstado, "", "")
                                   .Select(x => new { value = x.id, text = x.nomeMunicipio }).ToList();

            return Json(listaCidades, JsonRequestBehavior.AllowGet);

        }
        
        [ActionName("listar-json-paises")]
        public JsonResult listarJsonPaises() {
        
            var listaPaises = this.OPaisBL.listar("", "")
                .Select(x => new { value = x.id, text = x.nome }).ToList();
                
            return Json(listaPaises, JsonRequestBehavior.AllowGet);

        }

    }
}