using System;
using System.Web.Mvc;
using System.Linq;
using BLL.Localizacao;
using DAL.Localizacao;
using EntityFramework.Extensions;
using EntityFramework.Caching;

namespace WEB.Areas.Localizacao.Helpers{
    public class CidadeHelper{

        // Atributos
        private static ICidadeBL _ICidadeBL;

        // Propriedades
        public static ICidadeBL OCidadeBL => _ICidadeBL = _ICidadeBL ?? new CidadeBL();

        //
        public static SelectList selectList(int? idEstado, int? selected){

			int idEstadoTratado = (UtilNumber.toInt32(idEstado) == 0 ? 999 : UtilNumber.toInt32(idEstado));

            var list = OCidadeBL.listar(idEstadoTratado, "", "S").OrderBy(x => x.nome).FromCache( CachePolicy.WithDurationExpiration(TimeSpan.FromHours(1)) ).ToList();

            return new SelectList(list, "id", "nome", selected);
        }

        /// <summary>
        /// Select list com busca a partir do ID de uma cidade (Ira buscar todas as cidades que estão dentro do mesmo estado da cidade informada)
        /// </summary>
        public static SelectList selectListCidade(int? selected){

            var list = OCidadeBL.listar(selected.toInt())
                                .OrderBy(x => x.nome)
                                .FromCache( CachePolicy.WithDurationExpiration(TimeSpan.FromHours(1)) )
                                .ToList();

            return new SelectList(list, "id", "nome", selected);
        }

        //
        public static MultiSelectList selectListMulti(int? idEstado, int[] selected){

            int idEstadoTratado = idEstado.toInt() == 0 ? 999 : idEstado.toInt();

            var list = OCidadeBL.listar(idEstadoTratado, "", "S").OrderBy(x => x.nome).FromCache( CachePolicy.WithDurationExpiration(TimeSpan.FromHours(1)) ).ToList();

            return new MultiSelectList(list, "id", "nome", selected);
        }

        //
        public static string getNomeCidade(int? id){
            var OCidade = OCidadeBL.carregar(UtilNumber.toInt32(id)) ?? new Cidade();
            return OCidade.nome;
        }

    }
}