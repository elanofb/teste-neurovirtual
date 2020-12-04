using System;
using System.Web.Mvc;
using System.Linq;
using BLL.Empresas;

namespace WEB.Areas.Empresas.Helpers{

    public class EmpresaHelper{

        private static EmpresaBL _EmpresaBL;

        public static EmpresaBL getService(){
            if(_EmpresaBL == null){
                _EmpresaBL = new EmpresaBL();
            }
            return _EmpresaBL;
        }

        //
        public static SelectList selectList(int? selected){            

            var list = getService().listar("", "S").Select(x => new {id = x.id, nomeFantasia = x.Pessoa.nome}).ToList();
            return new SelectList(list, "id", "nomeFantasia", UtilNumber.toInt32(selected));
        }

    }
}