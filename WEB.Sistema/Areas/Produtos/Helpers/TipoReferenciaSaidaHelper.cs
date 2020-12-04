using System.Web.Mvc;
using System.Linq;
using DAL.Produtos;
using BLL.Produtos;
using BLL.Funcionarios;

namespace WEB.Areas.Produtos.Helpers {
	public class TipoReferenciaSaidaHelper {

		//Atributos
        private static TipoReferenciaSaidaBL _TipoReferenciaSaidaBL;
        private static FuncionarioConsultaBL _FuncionarioConsultaBL;

		//Propriedades
        private static TipoReferenciaSaidaBL OTipoReferenciaSaidaBL{ get{ return (_TipoReferenciaSaidaBL = _TipoReferenciaSaidaBL ?? new TipoReferenciaSaidaBL() ); } }
        private static FuncionarioConsultaBL OFuncionarioConsultaBL{ get{ return ( _FuncionarioConsultaBL = _FuncionarioConsultaBL ?? new FuncionarioConsultaBL() ); } }

		//
		public static SelectList selectList(int? selected) {
			var list = OTipoReferenciaSaidaBL.listar("", "S").OrderBy(x => x.descricao).ToList();
			return new SelectList(list, "id", "descricao", selected);
		}

        public static SelectList selectListReferencias(int? selected, int idTipoReferencia) {

            switch (idTipoReferencia) { 

                case (int)TipoReferenciaSaidaEnum.FUNCIONARIOS :

                    var listFuncionarios = OFuncionarioConsultaBL.listar("", "S").OrderBy(x => x.Pessoa.nome).Select(x => new { id = x.id, nome = x.Pessoa.nome }).ToList();
			        return new SelectList(listFuncionarios, "id", "nome", selected);

                case (int)TipoReferenciaSaidaEnum.OUTROS :

                    var list = new[] { new { id = "9999", nome = "Outros" } };
                    return new SelectList(list, "id", "nome", selected);
            }

            var listDefault = new[] { new { id = "", nome = "..." } };
            return new SelectList(listDefault, "id", "nome", selected);
		}

    	public static string getReferencia(int idReferencia, int idTipoReferencia) {
          
            switch (idTipoReferencia) { 

                case (int)TipoReferenciaSaidaEnum.FUNCIONARIOS :

                    return OFuncionarioConsultaBL.carregar(idReferencia).Pessoa.nome;			        

                case (int)TipoReferenciaSaidaEnum.OUTROS :

                    return "Outros";
            }

            return "";
		}
	}
}