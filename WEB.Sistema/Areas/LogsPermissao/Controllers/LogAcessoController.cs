using System.Linq;
using System.Web.Mvc;
using BLL.LogsPermissao;
using WEB.App_Infrastructure;
using DAL.LogsPermissao;
using System.Collections.Generic;
using System;
using PagedList;
using WEB.Areas.LogsPermissao.ViewModels;
using BLL.Permissao;

namespace WEB.Areas.LogsPermissao.Controllers {
    
    public class LogAcessoController : BaseSistemaController {

		//Constantes

		//Atributos
        private LogUsuarioSistemaAcessoBL _LogUsuarioSistemaAcessoBL;
        private UsuarioSistemaBL _UsuarioSistemaBL;

        //Propriedades
        private LogUsuarioSistemaAcessoBL OLogUsuarioSistemaAcessoBL => (this._LogUsuarioSistemaAcessoBL = this._LogUsuarioSistemaAcessoBL ?? new LogUsuarioSistemaAcessoBL());
        private UsuarioSistemaBL OUsuarioSistemaBL => (this._UsuarioSistemaBL = this._UsuarioSistemaBL ?? new UsuarioSistemaBL());

        //Events


        //GET
        [HttpGet]
		public ActionResult listar(int id){

            var ViewModel = new LogAcessoVM();
            ViewModel.UsuarioSistema = this.OUsuarioSistemaBL.carregar(id);

            DateTime? dtAcessoIni = UtilRequest.getDateTime("dtAcessoIni");
            DateTime? dtAcessoFim = UtilRequest.getDateTime("dtAcessoFim");

            List<LogUsuarioSistemaAcesso> listaLogUsuarioSistemaAcesso = this.OLogUsuarioSistemaAcessoBL.listar(id).OrderByDescending(x => x.dtAcesso).ToList();

            if (dtAcessoIni != null) {
                 listaLogUsuarioSistemaAcesso = listaLogUsuarioSistemaAcesso.Where(x => x.dtAcesso.Date >= dtAcessoIni).ToList();
             }

             if (dtAcessoFim != null) {
                 listaLogUsuarioSistemaAcesso = listaLogUsuarioSistemaAcesso.Where(x => x.dtAcesso.Date <= dtAcessoFim).ToList();
             }

            ViewModel.listaLogUsuarioSistemaAcesso = listaLogUsuarioSistemaAcesso.ToPagedList(UtilRequest.getNroPagina(), UtilRequest.getNroRegistros());

            return View(ViewModel);
        }
    }
}