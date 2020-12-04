using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DAL.Associados.Config {

    public class CadastroForm {

        public string idForm { get; set; }

        public List<CadastroBlocoItem> listaBlocos { get; set; }

        //
        public CadastroForm() {

            this.listaBlocos = new List<CadastroBlocoItem>();

        }

        //
        public List<CadastroCampo> listaCampos() {
            
            var listaCampos = new List<CadastroCampo>();

            this.listaBlocos.ForEach( item => {

                listaCampos.AddRange(item.listaCampos);

            });

            return listaCampos;
        }

        //Configurar valor
        public void setValor(string box, string nomeCampo, object valor) {

            var OBloco = this.listaBlocos.FirstOrDefault(x => x.idDOM == box);

            if (OBloco == null) {
                return;
            }

            OBloco.listaCampos.Where(x => x.name == nomeCampo).ToList().ForEach(item => {

                item.defaultValue = valor == null? "": valor.ToString();

            });

        }

        //
        public void carregarBlocosECampos(bool flagEdicao) {

            var queryBlocos = this.listaBlocos.Where(x => x.flagExibir == true);

            if (flagEdicao) {
                queryBlocos = queryBlocos.Where(x => x.flagEdicao == true || x.flagEdicao == null);
            }

            this.listaBlocos = queryBlocos.ToList();

            foreach (var OBloco in this.listaBlocos) {

                var queryCampos = OBloco.listaCampos.Where(x => x.flagExibir == true);

                if (flagEdicao) {
                    queryCampos = queryCampos.Where(x => x.flagEdicao == true || x.flagEdicao == null);
                } else {
                    queryCampos = queryCampos.Where(x => x.flagCadastro == true || x.flagCadastro == null);
                }


                OBloco.listaCampos = queryCampos.ToList();
            }
        }

        //Carregar o formulario com dados enviados via requisicao
        public void carregarInformacoes() {

            this.listaBlocos.ForEach(itemBloco => {

                foreach (var OCampo in itemBloco.listaCampos) {

                    var postedValue = HttpContext.Current.Request[OCampo.name];

                    if (postedValue == null) {
                        continue;
                    }

                    if (!string.IsNullOrEmpty(OCampo.defaultValue) && OCampo.flagNaoAlterarDefault == true) {
                        continue;
                    }

                    OCampo.value = postedValue;
                }
            });

        }
         
    }
}
