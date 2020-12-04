using System.Web.Mvc;
using System.Linq;
using BLL.Associados;

namespace WEB.Areas.Associados.Helpers{
    public class AssociadoHelper{

        private static AssociadoBL _AssociadoBL;

        public static AssociadoBL getService(){
            _AssociadoBL = _AssociadoBL ?? new AssociadoBL();
            return _AssociadoBL;
        }

        //
        public static SelectList selectList(int? selected){

            var list = getService().listar(0, "", "","S")
                                   .Select( x => new { value = x.id, text = x.Pessoa.nome})
								   .OrderBy(x => x.text).ToList();

            return new SelectList(list, "value", "text", selected);

        }

        //
        public static SelectList SelectAllList(int? selected, int idTipoAssociado, string flagSituacao, string valorBusca, string ativo){
        
            var list = getService().listar(idTipoAssociado, flagSituacao, valorBusca, ativo)
                                   .Select( x => new { value = x.id, text = x.Pessoa.nome})
								   .OrderBy(x => x.text).ToList();

            return new SelectList(list, "value", "text", selected);
        }

        public static SelectList selectListPessoa(int? selected){

            var list = getService().listar(0, "", "","S").ToList();

            var optionsAssociados = list.Select(x => new {value = x.idPessoa.ToString(), text = x.Pessoa.nome.ToUpper()}).OrderBy(x => x.text).ToList();

            return new SelectList(optionsAssociados, "value", "text", selected);
        }
		
		//Lista de status possíveis para um associado
        public static SelectList selectListAtivo(string selected, bool? flagEmAdmissao = false, bool? flagBloqueado = false) {
            var list = new[] { 
                    new{value = "S", text = "Ativo"},
                    new{value = "N", text = "Desativado"}
            }.ToList();

            if (flagEmAdmissao == true) {
                list.Add(new { value = "E", text = "Em admissão" });
            }

            if (flagBloqueado == true) {
                list.Add(new { value = "B", text = "Bloqueado" });
            }

            return new SelectList(list, "value", "text", selected);
        }

        public static SelectList selectListAtivoSemAdmissao(string selected) {
            var list = new[] { 
                    new{value = "S", text = "Ativo"},
                    new{value = "N", text = "Desativado"}
            };
            return new SelectList(list, "value", "text", selected);
        }

    }
}