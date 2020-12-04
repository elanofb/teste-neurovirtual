using System.Collections.Generic;
using System.Web.Mvc;
using System.Linq;
using BLL.SegmentosAtuacao;

namespace WEB.Areas.SegmentosAtuacao.Helpers{
    
    public class SegmentoAtuacaoHelper{

        //Constantes
        private static SegmentoAtuacaoHelper _instance; 

        //Atributos
        private ISegmentoAtuacaoConsultaBL _SegmentoConsultaBL ;

        //Propriedades
        public static SegmentoAtuacaoHelper getInstance     => _instance = _instance ?? new SegmentoAtuacaoHelper();
        private       ISegmentoAtuacaoConsultaBL OSegmentoConsultaBL => _SegmentoConsultaBL = _SegmentoConsultaBL ?? new SegmentoAtuacaoConsultaBL();      
        
        //
        public SelectList selectList(int? selected, string flagTipoPessoa){
            var query = OSegmentoConsultaBL.listar("", true);

            if (flagTipoPessoa.Equals("F")){
                query = query.Where(x => x.flagPessoaFisica == true);
            }

            if (flagTipoPessoa.Equals("J")){
                query = query.Where(x => x.flagPessoaJuridica == true);
            }

            var list = query.OrderBy(x => x.descricao).ToList();

            return new SelectList(list, "id", "descricao", selected);
        }

        public MultiSelectList getSeleListMulti(List<int> selected){

            var query = OSegmentoConsultaBL.listar("", true);

            var list = query.OrderBy(x => x.descricao).ToList();

            return new MultiSelectList(list, "id", "descricao", selected);
        }

    }
}