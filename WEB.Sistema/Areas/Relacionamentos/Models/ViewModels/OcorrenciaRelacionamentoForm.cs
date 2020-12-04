using FluentValidation.Attributes;
using DAL.Relacionamentos;
using BLL.Relacionamentos;

namespace WEB.Areas.Relacionamentos.ViewModels{

    [Validator(typeof(OcorrenciaRelacionamentoValidator))]
	public class OcorrenciaRelacionamentoForm{

        // Constantes

        // Atributos
        private IOcorrenciaRelacionamentoPadraoBL _OcorrenciaRelacionamentoPadraoBL { get; set; }

        // Propriedades
        public OcorrenciaRelacionamentoPadrao OcorrenciaRelacionamento { get; set; }
        private IOcorrenciaRelacionamentoPadraoBL OOcorrenciaRelacionamentoPadraoBL { get { return (this._OcorrenciaRelacionamentoPadraoBL = this._OcorrenciaRelacionamentoPadraoBL ?? new OcorrenciaRelacionamentoPadraoBL()); } }

        public OcorrenciaRelacionamentoForm() { 
            this.OcorrenciaRelacionamento = new OcorrenciaRelacionamentoPadrao();
        }

	}

}