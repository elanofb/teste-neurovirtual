using System.Web.Mvc;
using DAL.Pessoas;

namespace WEB.Areas.AssociadosConsultas.Helpers {

    public class AssociadoArquivoHelper {

        private static AssociadoArquivoHelper _instance;

        public static AssociadoArquivoHelper getInstance => _instance = _instance ?? new AssociadoArquivoHelper();


        //Select Lista para tipos de arquivos do associado
        public static SelectList selectList(int selected) {
            
            var list = new[] {
                new{value = PessoaArquivoConst.ASSOCIADO, text = "Documentos do Associado"},
                new{value = PessoaArquivoConst.FOTO_ASSOCIADO, text = "Foto do Associado"},
                new{value = PessoaArquivoConst.ASSOCIADO_RELACIONAMENTO, text = "Documentos do Relacionamento"}
            };
            
            return new SelectList(list, "value", "text", selected);
        }

        public static string getLabelTipoArquivo(int idEntidadeArquivo)
        {
            if(idEntidadeArquivo == PessoaArquivoConst.ASSOCIADO) return "Documentos do Associado";
                
            if (idEntidadeArquivo == PessoaArquivoConst.FOTO_ASSOCIADO) return "Foto do Associado";
                
            if (idEntidadeArquivo == PessoaArquivoConst.ASSOCIADO_RELACIONAMENTO) return "Documentos do Relacionamento";

            return "";
        }
    }
}