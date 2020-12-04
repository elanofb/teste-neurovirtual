using System;
using System.Linq;
using FluentValidation;
using BLL.Associados;
using BLL.AssociadosContribuicoes;
using BLL.Contribuicoes;
using DAL.Associados;
using DAL.Contribuicoes;

namespace WEB.Areas.AssociadosContribuicoes.ViewModels{

    //
    public class AssociadoContribuicaoFormValidator : AbstractValidator<AssociadoContribuicaoPartialForm> {
        
		//Atributos
		private IAssociadoContribuicaoBL _AssociadoContribuicaoBL; 
		private IAssociadoBL _AssociadoBL; 
		private IContribuicaoBL _ContribuicaoBL; 

		//Propriedades
		private IAssociadoContribuicaoBL OAssociadoContribuicaoBL => (this._AssociadoContribuicaoBL = this._AssociadoContribuicaoBL ?? new AssociadoContribuicaoBL() );
        private IAssociadoBL OAssociadoBL => (this._AssociadoBL = this._AssociadoBL ?? new AssociadoBL() );
		private IContribuicaoBL OContribuicaoBL  => this._ContribuicaoBL = this._ContribuicaoBL ?? new ContribuicaoPadraoBL();

        //Construtor
        public AssociadoContribuicaoFormValidator() {
            
            RuleFor(x => x.AssociadoContribuicao.idAssociado)
				.NotEmpty().WithMessage("Informe a qual associado será vinculado à contribuição.");

            RuleFor(x => x.AssociadoContribuicao.idContribuicao)
                .GreaterThan(0).WithMessage("Informe qual é a contribuição/plano.")
                .Must((x, idContribuicao) => this.existeTipoAssociado(x)).WithMessage("Informe o tipo do associado no cadastro do mesmo antes de gerar a cobrança.")
                .Must((x, idContribuicao) => !this.existe(x)).WithMessage("Já existe uma contribuição com essa data de vencimento para o associado.");
            
            RuleFor(x => x.AssociadoContribuicao.dtVencimentoOriginal)
				.NotEmpty().WithMessage("Informe o vencimento da cobrança.");

            RuleFor(x => x.AssociadoContribuicao.dtInicioVigencia)
				.GreaterThan(new DateTime(1990, 1, 1)).WithMessage("Informe uma data válida.")
                .LessThan(DateTime.Today.AddYears(2)).WithMessage("A data de início da vigência não pode ser tão longa.")
                .Must((x, dtVencimentoAtual) => this.validarInicioVigencia(x)).WithMessage("O data de início de vigência é inválida pois já há uma cobrança nessa data.");

            RuleFor(x => x.AssociadoContribuicao.dtFimVigencia)
                .GreaterThanOrEqualTo(x => x.AssociadoContribuicao.dtInicioVigencia).WithMessage("O fim da vigência deve ser maior do que o início.")
                .LessThan(DateTime.Today.AddYears(2)).WithMessage("A data de fim da vigência não pode ser tão longa.")
                .Must((x, dtVencimentoAtual) => this.validarFimVigencia(x)).WithMessage("O data de fim de vigência é inválida pois já há uma cobrança nessa data.");

	        RuleFor(x => x.AssociadoContribuicao.valorAtual)
		        .GreaterThan(0).When(x => x.AssociadoContribuicao.flagIsento != true).WithMessage("O valor da cobrança deve ser maior do que zero.");

        }

        //Verificar se o contato já existe
        private bool existe(AssociadoContribuicaoPartialForm ViewModel) {

            var flagExiste = this.OAssociadoContribuicaoBL.listar(ViewModel.AssociadoContribuicao.idContribuicao, ViewModel.AssociadoContribuicao.idAssociado, null, null)
                                                    .Any(x => x.dtVencimentoOriginal == ViewModel.AssociadoContribuicao.dtVencimentoOriginal);

			return flagExiste;
        }

        //Verificar se o contato já existe
        private bool validarInicioVigencia(AssociadoContribuicaoPartialForm ViewModel) {

            var flagExiste = this.OAssociadoContribuicaoBL.listar(ViewModel.AssociadoContribuicao.idContribuicao, ViewModel.AssociadoContribuicao.idAssociado, null, null)
                                                    .Any(x => ViewModel.AssociadoContribuicao.dtInicioVigencia >= x.dtInicioVigencia && ViewModel.AssociadoContribuicao.dtInicioVigencia <= x.dtInicioVigencia);

			return !flagExiste;
        }

        //Verificar se o contato já existe
        private bool validarFimVigencia(AssociadoContribuicaoPartialForm ViewModel) {

            var flagExiste = this.OAssociadoContribuicaoBL.listar(ViewModel.AssociadoContribuicao.idContribuicao, ViewModel.AssociadoContribuicao.idAssociado, null, null)
                                                    .Any(x => ViewModel.AssociadoContribuicao.dtFimVigencia >= x.dtInicioVigencia && ViewModel.AssociadoContribuicao.dtFimVigencia <= x.dtFimVigencia);

			return !flagExiste;
        }

        //Verificar se o contato já existe
        private bool validarVencimento(AssociadoContribuicaoPartialForm ViewModel) {

            var OContribuicao = this.OContribuicaoBL.carregar(ViewModel.AssociadoContribuicao.idContribuicao);

            if (OContribuicao == null) {
                return true;
            }

            if (OContribuicao.idTipoVencimento != TipoVencimentoConst.FIXO_PELA_CONTRIBUICAO) {
                return true;
            }

            byte dia = (byte) ViewModel.AssociadoContribuicao.dtVencimentoAtual.Day;

            byte mes = (byte) ViewModel.AssociadoContribuicao.dtVencimentoAtual.Month;

            var listaVencimentos = OContribuicao.listaContribuicaoVencimento.Where(x => x.dtExclusao == null).ToList();

            bool flagExiste = listaVencimentos.Any(x => x.diaVencimento == dia && x.mesVencimento == mes);
            
			return flagExiste;
        }

        //Verificar se o contato já existe
        private bool existeTipoAssociado(AssociadoContribuicaoPartialForm ViewModel) {
			Associado OAssociado = this.OAssociadoBL.carregar(ViewModel.AssociadoContribuicao.idAssociado);

			if (OAssociado.idTipoAssociado > 0) {
				return true;
			}
			return false;
        }


    }
}
