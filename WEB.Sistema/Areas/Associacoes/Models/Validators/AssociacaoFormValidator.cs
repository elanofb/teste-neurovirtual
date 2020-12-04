using System;
using System.Linq;
using FluentValidation;
using BLL.Organizacoes;

namespace WEB.Areas.Associacoes.ViewModels{
    
    //
	public class AssociacaoFormValidator : AbstractValidator<AssociacaoForm> {

        private IOrganizacaoBL _AssociacaoBL;
        private IOrganizacaoBL OAssociacaoBL => this._AssociacaoBL = this._AssociacaoBL ?? new OrganizacaoBL();

        public AssociacaoFormValidator() {

            RuleFor(x => x.Associacao.Pessoa.nome)
                .NotEmpty().WithMessage("Informe o nome fantasia da Associação.");

            RuleFor(x => x.Associacao.Pessoa.nroDocumento)
                .NotEmpty().WithMessage("O CNPJ da associação é uma informação obrigatória.");

            When(x => !String.IsNullOrEmpty(x.Associacao.Pessoa.nroDocumento), () => {
                RuleFor(x => x.Associacao.Pessoa.nroDocumento)
                        .Must((x, nroDocumento) => UtilValidation.isCPFCNPJ(x.Associacao.Pessoa.nroDocumento)).WithMessage("Informe um CNPJ válido.")
                        .Must((x, nroDocumento) => !this.existeCNPJ(x)).WithMessage("Já existe uma Associação cadastrada com esse CNPJ.");
            });

            When(x => x.Associacao.idOrganizacaoGestora > 0, () => {
                RuleFor(x => x.Associacao.idOrganizacaoGestora).Must((x, gestora) => !this.unidadesVinculadas(x.Associacao.id)).WithMessage("Uma associação gestora não pode ser vincula");
            });
            
        }
        
        #region Validacoes Banco
        //Verificar existência de CNPJ para evitar duplicidades  
        private bool existeCNPJ(AssociacaoForm ViewModel) {
            int idDesconsiderado = ViewModel.Associacao.id;

            bool flagExiste = this.OAssociacaoBL.listar("", null)
                                                    .Any(x => x.Pessoa.nroDocumento == ViewModel.Associacao.Pessoa.nroDocumento && x.id != idDesconsiderado);

            return flagExiste;
        }

	    private bool unidadesVinculadas(int? idOrganizacao) {

	        bool flagExiste = this.OAssociacaoBL.listar("", null).Any(x => x.idOrganizacaoGestora == idOrganizacao);

            return flagExiste;
	    }
        #endregion

    }
}