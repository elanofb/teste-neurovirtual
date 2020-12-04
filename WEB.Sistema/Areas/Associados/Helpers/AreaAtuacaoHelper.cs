using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using BLL.AreasAtuacao;

namespace WEB.Areas.Associados.Helpers{

    public class AreaAtuacaoHelper{

        private static AreaAtuacaoBL _AreaAtuacaoBL;
        private static AreaAtuacaoTipoAssociadoBL _AreaAtuacaoTipoAssociadoBL;        

        public static AreaAtuacaoBL getService() {
            _AreaAtuacaoBL = _AreaAtuacaoBL ?? new AreaAtuacaoBL();
            return _AreaAtuacaoBL;
        }

        public static AreaAtuacaoTipoAssociadoBL getServiceTipoAssociado() {
            _AreaAtuacaoTipoAssociadoBL = _AreaAtuacaoTipoAssociadoBL ?? new AreaAtuacaoTipoAssociadoBL();
            return _AreaAtuacaoTipoAssociadoBL;
        }

        public static SelectList selectList(int? selected, int idTipoAssociado) {
        
            var lista = getServiceTipoAssociado().listar(idTipoAssociado, "S").Select(x => new { x.AreaAtuacao.id, x.AreaAtuacao.descricao }).ToList();

            if (lista.Count == 0) {
                lista = getService().listar("", "S").OrderBy(x => x.descricao).Select(x => new { x.id, x.descricao }).ToList();               
            }

            return new SelectList(lista, "id", "descricao", selected);
        }
        
        //
        public static MultiSelectList multiSelectList(List<int> selected, List<int> idsExclude = null){
            
            var query = getService().listar("", "S");

            if (idsExclude != null) {
                query = query.Where(x => !idsExclude.Contains(x.id));
            }
            
            var listaAreas = query.OrderBy(x => x.id).AsNoTracking().ToList();
            
            return new MultiSelectList(listaAreas, "id", "descricao", selected);

        }

    }
}