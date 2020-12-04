using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using BLL.Caches;
using BLL.Telefones;
using DAL.Telefones;

namespace WEB.Areas.Telefones.Helpers {

    public class TipoTelefoneHelper {

        //Atributos
        private static TipoTelefoneHelper _instance;
        private ITipoTelefoneBL _TipoTelefoneBL;

        //Propriedades
        public static TipoTelefoneHelper getInstance => _instance = _instance ?? new TipoTelefoneHelper();
        private ITipoTelefoneBL OTipoTelefoneBL => this._TipoTelefoneBL = this._TipoTelefoneBL ?? new TipoTelefoneBL();

        //Private Constructor
        private TipoTelefoneHelper() {

        }

        /// <summary>
        /// Dropdown com opcao de unidades cadastradas no sistema
        /// </summary>
        public SelectList selectList(int? selected, bool flagCache = true) {

            string keyCache = "dd_tipo_telefone";


            if (!flagCache) {

                var query = this.OTipoTelefoneBL.listar("");

                var lista = query.ToList();

                var subList = lista.Select(x => new { value = x.id, text = x.descricao }).ToList();

                return new SelectList(subList, "value", "text", selected);

            }

            var OCacheService = CacheService.getInstance;

            var listaCache = OCacheService.carregar<List<TipoTelefone>>(keyCache);

            if (listaCache == null) {

                var query = this.OTipoTelefoneBL.listar("");

                listaCache = query.ToList();

                OCacheService.adicionar(keyCache, listaCache);
            }

            var list = listaCache.Select(x => new { value = x.id, text = x.descricao }).ToList();
            ;

            return new SelectList(list, "value", "text", selected);

        }

        //Combo com relacao de contribuicoes
        public SelectList selectListTipoTelefone(string selected) {

            var list = new[] {
                    new{value = "F", text = "Tel. Fixo"},
                    new{value = "C", text = "Tel. Celular"}
            };

            return new SelectList(list, "value", "text", selected);
        }

    }
}