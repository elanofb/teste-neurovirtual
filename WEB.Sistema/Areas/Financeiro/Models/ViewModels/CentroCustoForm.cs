using System;
using System.Collections.Generic;
using System.Linq;
using BLL.Financeiro;
using FluentValidation.Attributes;
using DAL.Financeiro;
using WEB.ViewModels;

namespace WEB.Areas.Financeiro.ViewModels {

    [Validator(typeof(CentroCustoValidator))]
    public class CentroCustoForm {
        //Atributos      
        private ICentroCustoMacroContaBL _CentroCustoMacroContaBL;

        //Propriedades
        private ICentroCustoMacroContaBL OCentroCustoMacroContaBL => (this._CentroCustoMacroContaBL = this._CentroCustoMacroContaBL ?? new CentroCustoMacroContaBL());


        public CentroCusto CentroCusto { get; set; }
        public List<MacroConta> listaMacroConta { get; set; }
        public List<CheckBoxItens> listaSelecionados { get; set; }

        ///Propriedade Auxiliar modal
        public string group { get; set; }

        ///
        public CentroCustoForm() {
            this.CentroCusto = new CentroCusto();
            this.listaSelecionados = new List<CheckBoxItens>();
        }

        ///
        public void carregarCheckboxes() {

            if (this.CentroCusto.id == 0) return;

            var lista = OCentroCustoMacroContaBL.listar(0, this.CentroCusto.id);

            listaMacroConta = listaMacroConta.OrderBy(x => UtilString.onlyNumber(x.codigoFiscal).toInt()).ThenBy(x => x.descricao).ToList();
            
            foreach (var OMacroConta in listaMacroConta)
            {
                var Item = new CheckBoxItens { id = OMacroConta.id, isChecked = lista.Any(x => x.idMacroConta == OMacroConta.id), descricao = !OMacroConta.codigoFiscal.isEmpty() ? OMacroConta.codigoFiscal +" - "+ OMacroConta.descricao : OMacroConta.descricao};

                this.listaSelecionados.Add(Item);
            }

            this.listaSelecionados = this.listaSelecionados.ToList();
        }
    }
}