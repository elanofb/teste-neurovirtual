using System.Collections;
using System.Web.Mvc;
using BLL.Caches;
using BLL.Documentos;
using DAL.Documentos;
using System.Linq;

namespace WEB.Helpers{

    public class TipoDocumentoHelper{

        //Constantes
        private static TipoDocumentoHelper _instance; 

        //Atributos
         private static TipoDocumentoBL _TipoDocumentoBL;

        //Propriedades
        public static TipoDocumentoHelper getInstance => _instance = _instance ?? new TipoDocumentoHelper();
        private TipoDocumentoBL OTipoDocumentoBL   => _TipoDocumentoBL = _TipoDocumentoBL ?? new TipoDocumentoBL();
       

        //Carregar combo de selecao dos tipos de associados
        public SelectList selectList(bool? flagPF, bool? flagPJ, int? selected) {


            int idCategoriaPessoal = CategoriaDocumentoConst.DOCUMENTO_PESSOAL;

            var query = OTipoDocumentoBL.listar(idCategoriaPessoal, "");

            if (flagPF.HasValue) {
                query = query.Where(x => x.flagPF == flagPF);
            }

            if (flagPJ.HasValue) {
                query = query.Where(x => x.flagPJ == flagPJ);
            }

           var listaItens = query.OrderBy(x => x.descricao)
                                .Select(x => new {
                                    x.id,
                                    descricao = x.nome
                                }).ToList();

            return new SelectList(listaItens, "id", "descricao", selected);
        }

        //Carregar combo de selecao dos tipos de associados
        public SelectList selectList(int? selected) {

            var listaItens = CacheService.getInstance.carregar(CacheService.TIPO_DOCUMENTO_DD_SIMPLES);

            if (listaItens != null) {

                return new SelectList((IEnumerable)listaItens, "id", "descricao", selected);
            }

            int idCategoriaPessoal = CategoriaDocumentoConst.DOCUMENTO_PESSOAL;

            listaItens = OTipoDocumentoBL.listar(idCategoriaPessoal, "").OrderBy(x => x.descricao)
                                    .Select(x => new {
                                        x.id,
                                        descricao = x.nome
                                    }).ToList();

            CacheService.getInstance.adicionar(CacheService.TIPO_DOCUMENTO_DD_SIMPLES, listaItens);

            return new SelectList((IEnumerable) listaItens, "id", "descricao", selected);
        }

    }
}