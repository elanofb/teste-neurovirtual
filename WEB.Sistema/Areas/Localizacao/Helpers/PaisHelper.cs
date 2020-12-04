using BLL.Localizacao;
using DAL.Localizacao;
using EntityFramework.Caching;
using EntityFramework.Extensions;
using System;
using System.Linq;
using System.Web.Mvc;

namespace WEB.Areas.Localizacao.Helpers {
    public class PaisHelper {

        private static IPaisBL _PaisBL;

        public static IPaisBL OPaisBL => _PaisBL = _PaisBL ?? new PaisBL();

        //
        public static SelectList selectList(string selected){
            var list = OPaisBL.listar("", "S").OrderBy(x => x.nome).FromCache( CachePolicy.WithDurationExpiration(TimeSpan.FromHours(1)) ).ToList();
            return new SelectList(list, "id", "nome", selected);
        }

        //
        public static string getNomePais(string id){
            var OPais = OPaisBL.carregar(id) ?? new Pais();
            return OPais.nome;
        }

    }
}