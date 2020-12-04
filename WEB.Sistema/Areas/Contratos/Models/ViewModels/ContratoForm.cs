using System.Collections.Generic;
using System.Web;
using FluentValidation.Attributes;
using DAL.Contratos;
using DAL.Financeiro;

namespace WEB.Areas.Contratos.ViewModels {

    [Validator(typeof(ContratoValidator))]
    public class ContratoForm {


        //Propiedades
        public Contrato Contrato { get; set; }
        public HttpPostedFileBase OArquivoContrato { get; set; }
        public IList<TituloPagamento> listaPagamentos { get; set; }

        //Contrutor
        public ContratoForm() { 
            this.Contrato = new Contrato();
            this.listaPagamentos = new List<TituloPagamento>();
        }
    }
}