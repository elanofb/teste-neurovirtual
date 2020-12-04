using System;
using System.Linq;
using FluentValidation;
using BLL.Contribuicoes;
using DAL.Contribuicoes;

namespace WEB.Areas.Contribuicoes.ViewModels{

    //
    public class ContribuicaoPadraoFormValidator : AbstractValidator<ContribuicaoPadraoForm> {
        
		//Atributos
		private IContribuicaoBL _ContribuicaoPadraoBL; 
        private IPeriodoContribuicaoBL _PeriodoContribuicaoBL; 

		//Propriedades
		private IContribuicaoBL OContribuicaoPadraoBL => this._ContribuicaoPadraoBL = this._ContribuicaoPadraoBL ?? new ContribuicaoPadraoBL();
		private IPeriodoContribuicaoBL OPeriodoContribuicaoBL => this._PeriodoContribuicaoBL = this._PeriodoContribuicaoBL ?? new PeriodoContribuicaoBL();

        //Construtor
        public ContribuicaoPadraoFormValidator() {

		    this.validarContribuicao();

            this.validarGeracaoAutomatica();
        }

        //
        private void validarContribuicao() {

            RuleFor(x => x.Contribuicao.descricao)
                .NotEmpty().WithMessage("Informe um título.");

            RuleFor(x => x.Contribuicao.dtValidade)
                .GreaterThan(DateTime.Today.AddYears(-50)).WithMessage("Informe uma data de validade mais recente.");

            RuleFor(x => x.Contribuicao.emailCobrancaTitulo)
                .NotEmpty().WithMessage("Informe o título para os e-mails de cobrança.");

            RuleFor(x => x.Contribuicao.emailCobrancaHtml)
                .Length(0, 3000).WithMessage("A mensagem de cobrança ultrapassa o tamanho limite permitido.");

            //Somente para novo cadastro
            When(x => x.Contribuicao.id == 0, () => {

                RuleFor(x => x.Contribuicao.idPeriodoContribuicao)
                    .NotEmpty().WithMessage("Informe qual é o período de cobrança.");

                RuleFor(x => x.Contribuicao.idTipoVencimento)
                    .NotEmpty().WithMessage("Informe como deve ser o vencimento.");

                    RuleFor(x => x.Contribuicao.ativo)
                        .NotEmpty().WithMessage("Informe o status.");
            });

            

            //Se o tipo de vencimento for pela data de admissão, somente pode ser escolhido o perído anuidade
            When(x => x.Contribuicao.idTipoVencimento == TipoVencimentoConst.VENCIMENTO_PELA_ADMISSAO_ASSOCIADO, () => {

                RuleFor(x => x.Contribuicao.idPeriodoContribuicao)
                    .Must((x, idPeriodoContribuicao) => idPeriodoContribuicao == PeriodoContribuicaoConst.ANUAL).WithMessage("Para vencimentos com base na admissão do associado, somente o período de cobrança ANUAL está disponível.");
                
            });

            //Se for cobrança com vencimento fixo deve ser informado os dias de vencimento
            When(x => x.Contribuicao.idTipoVencimento == TipoVencimentoConst.FIXO_PELA_CONTRIBUICAO, () => {

                RuleFor(x => x.Contribuicao.idTipoVencimento)
                    .Must( (x, idTipoVencimento) => x.Contribuicao.listaContribuicaoVencimento.Any() ).WithMessage("As datas de vencimento não foram informadas.");
                

                RuleFor(x => x.Contribuicao.listaContribuicaoVencimento).SetCollectionValidator(new ContribuicaoVencimentoValidator());

            });

            

        }

        //
        private void validarGeracaoAutomatica() {

            RuleFor(x => x.Contribuicao.flagGerarCobrancaAutomatica)
                .NotEmpty().WithMessage("Informe se o sistema deve gerar as contribuições de forma automática.");


            When(x => x.Contribuicao.id == 0, () => {

                When(x => x.Contribuicao.flagGerarCobrancaAutomatica == true, () => {
                    RuleFor(x => x.Contribuicao.qtdeDiasEnvioCobranca)
                        .NotEmpty().WithMessage("Informe quantos dias antes do vencimento o sistema deve enviar a cobrança.")
                        .Must((x, qtdeDiasEnvioCobranca) => validarQtdeDias(x)).WithMessage("A quantidade de dias deve ser menor do que o período de cobrança.");
                });

            });

            When(x => x.Contribuicao.id > 0, () => {

                When(x => x.Contribuicao.flagGerarCobrancaAutomatica == true, () => {
                    RuleFor(x => x.Contribuicao.qtdeDiasEnvioCobranca)
                        .NotEmpty().WithMessage("Informe quantos dias antes do vencimento o sistema deve enviar a cobrança.")
                        .Must((x, qtdeDiasEnvioCobranca) => validarQtdeDias(x.Contribuicao)).WithMessage("A quantidade de dias deve ser menor do que o período de cobrança.");
                });

            });
                

        }

		//Verificar se a quantidade de dias de antecedencia nao antecede o periodo da contribuicao
        private bool validarQtdeDias(ContribuicaoPadraoForm ViewModel) {

            var OPeriodo = this.OPeriodoContribuicaoBL.carregar(UtilNumber.toInt32(ViewModel.Contribuicao.idPeriodoContribuicao));

            if (OPeriodo == null) {
                return false;
            }

            if (OPeriodo.qtdeDias <= ViewModel.Contribuicao.qtdeDiasEnvioCobranca) {
                return false;
            }

			return true;
        }

		//Verificar se a quantidade de dias de antecedencia nao antecede o periodo da contribuicao
        private bool validarQtdeDias(Contribuicao OContribuicao) {

            var OContribuicaoDb = this.OContribuicaoPadraoBL.carregar(OContribuicao.id);

            var OPeriodo = OContribuicaoDb.PeriodoContribuicao;

            if (OPeriodo == null) {
                return false;
            }

            if (OPeriodo.qtdeDias <= OContribuicao.qtdeDiasEnvioCobranca) {
                return false;
            }

			return true;
        }

    }
}
