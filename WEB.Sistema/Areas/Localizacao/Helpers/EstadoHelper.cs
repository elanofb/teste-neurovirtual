using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Linq;
using BLL.Localizacao;
using DAL.Localizacao;
using EntityFramework.Extensions;
using EntityFramework.Caching;

namespace WEB.Areas.Localizacao.Helpers{
    public class EstadoHelper{

        // Atrbiutos
        private static IEstadoBL _EstadoBL;

        // Propriedades
        public static IEstadoBL OEstadoBL => _EstadoBL = _EstadoBL ?? new EstadoBL();

        //
        public static SelectList selectList(int? selected){
            
            var list = OEstadoBL.listar("", "S")
                                .FromCache( CachePolicy.WithDurationExpiration(TimeSpan.FromHours(1)) )
                                .ToList();

            list = list.OrderBy(x => x.sigla).ToList();

            return new SelectList(list, "id", "sigla", selected);
        }

        //
		public static SelectList selectListString(string selected) {
			var list = OEstadoBL.listar("", "S").OrderBy(x => x.sigla);
			return new SelectList(list, "sigla", "sigla", selected);
		}


        //
        public static string getUf(int? id){
            var OUf = OEstadoBL.carregarPorId(UtilNumber.toInt32(id)) ?? new Estado();
            return OUf.sigla;
        }

        //
        public static MultiSelectList getSeleListMulti(List<int> selected){

            var list = OEstadoBL.listar("", "S").OrderBy(x => x.sigla).FromCache( CachePolicy.WithDurationExpiration(TimeSpan.FromHours(1))).ToList();

            return new MultiSelectList(list, "id", "sigla", selected);
        }
    }
}