using System;
using DAL.Financeiro;
using FluentValidation;

namespace WEB.Areas.Financeiro.ViewModels{
    
    public class BaixaTituloReceitaFormValidator : AbstractValidator<BaixaTituloReceitaForm>{
        

        public BaixaTituloReceitaFormValidator(){

            this.validarPagamento();

            this.validarPagamentoBoleto();

            this.validarPagamentoDeposito();

            this.validarPagamentoCheque();

            this.validarPagamentoCredito();
        }

                //Validar os dados principais do pagamento
        private void validarPagamento() {

            RuleFor(x => x.TituloReceitaPagamento.idMeioPagamento)
               .NotEmpty().WithMessage("Informe qual foi o meio de pagamento utilizado.");


            RuleFor(x => x.TituloReceitaPagamento.valorRecebido)
               .GreaterThan(0).WithMessage("Informe o valor recebido.");

            RuleFor(x => x.TituloReceitaPagamento.dtPagamento)
				.NotEmpty().WithMessage("Informe a data em que o pagamento foi realizado")
				.LessThanOrEqualTo(DateTime.Today).WithMessage("A data de pagamento não pode estar no futuro.");

        }

        //
        private void validarPagamentoBoleto() {

            When(x => x.TituloReceitaPagamento.idMeioPagamento == MeioPagamentoConst.BOLETO_BANCARIO, () => {

                RuleFor(x => x.TituloReceitaPagamento.nroDocumento)
				    .NotEmpty().WithMessage("Informe o número do boleto pago.");

            });
            
        }

        //
        private void validarPagamentoDeposito() {

            When(x => x.TituloReceitaPagamento.idMeioPagamento == MeioPagamentoConst.DEPOSITO_BANCARIO, () => {

                                                                                                        });

        }
        

        //
        private void validarPagamentoCheque() {

            When(x => x.TituloReceitaPagamento.idMeioPagamento == MeioPagamentoConst.CHEQUE, () => {

                RuleFor(x => x.TituloReceitaPagamento.nroConta)
                    .NotEmpty().WithMessage("Informe o número da conta.");

                RuleFor(x => x.TituloReceitaPagamento.nroDocumento)
                    .NotEmpty().WithMessage("Informe o número do cheque.");

                RuleFor(x => x.TituloReceitaPagamento.nroAgencia)
                    .NotEmpty().WithMessage("Informe o número da agência.");

            });
        }

        //
        private void validarPagamentoCredito() {

            When(x => x.TituloReceitaPagamento.idMeioPagamento == MeioPagamentoConst.CARTAO_CREDITO, () => {

                RuleFor(x => x.TituloReceitaPagamento.idFormaPagamento)
                    .NotEmpty().WithMessage("Informe com qual bandeira o pagamento foi realizado.");
            });
        }
    }
}
