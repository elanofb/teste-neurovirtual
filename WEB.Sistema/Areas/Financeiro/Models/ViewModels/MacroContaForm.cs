using System.Collections.Generic;
using System.Linq;
using BLL.Financeiro;
using FluentValidation.Attributes;
using DAL.Financeiro;
using WEB.ViewModels;

namespace WEB.Areas.Financeiro.ViewModels {

    [Validator(typeof(MacroContaValidator))]
    public class MacroContaForm {

		//Atributos        
		private ICentroCustoMacroContaBL _CentroCustoMacroContaBL;
		
        //Propriedades
		private ICentroCustoMacroContaBL OCentroCustoMacroContaBL => (this._CentroCustoMacroContaBL = this._CentroCustoMacroContaBL ?? new CentroCustoMacroContaBL());
        
        public MacroConta MacroConta { get; set; }
        public List<CentroCusto> listaCentroCusto { get; set; }
        public List<CheckBoxItens> listaSelecionados { get; set; }

        ///Propriedade Auxiliar modal
        public string group { get; set; }

        //
        public MacroContaForm() {
            this.MacroConta = new MacroConta();
            this.listaSelecionados = new List<CheckBoxItens>();
        }

        //
        public void carregarCheckboxes() {

            if (this.MacroConta.id == 0) return;

            var lista = OCentroCustoMacroContaBL.listar(this.MacroConta.id, 0);

            foreach (var OCentroCusto in listaCentroCusto) {

                var Item = new CheckBoxItens { id = OCentroCusto.id, isChecked = lista.Any(x => x.idCentroCusto == OCentroCusto.id), descricao = OCentroCusto.descricao};

                this.listaSelecionados.Add(Item);

            }

            this.listaSelecionados = this.listaSelecionados.OrderBy(x => x.descricao).ToList();
        }
    }
}