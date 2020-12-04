using System;
using System.Linq;
using BLL.ContasBancarias;
using BLL.DadosBancarios.Interfaces;
using BLL.DadosBancarios.Services;
using BLL.Financeiro;
using BLL.FinanceiroLancamentos;
using DAL.ContasBancarias;
using DAL.DadosBancarios;
using DAL.Financeiro;
using FluentValidation;

namespace WEB.Areas.FinanceiroLancamentos.ViewModels {

    public class DespesaCadastroFormValidator : AbstractValidator<DespesaCadastroForm> {

        //Atributos
        private CredorVWBL _CredorVWBL;
        private IModoPagamentoDespesaConsultaBL _ModoPagamentoDespesaConsultaBL;
        private IContaBancariaBL _ContaBancariaBL;
        private IDadoBancarioConsultaBL _DadoBancarioConsultaBL;

        //Propriedades
        private IModoPagamentoDespesaConsultaBL OModoPagamentoDespesaConsultaBL => (this._ModoPagamentoDespesaConsultaBL = this._ModoPagamentoDespesaConsultaBL ?? new ModoPagamentoDespesaConsultaBL());
        private IContaBancariaBL OContaBancariaBL => (this._ContaBancariaBL = this._ContaBancariaBL ?? new ContaBancariaBL());
        private IDadoBancarioConsultaBL ODadoBancarioConsultaBL => (this._DadoBancarioConsultaBL = this._DadoBancarioConsultaBL ?? new DadoBancarioConsultaBL());
        private CredorVWBL OCredorVWBL => _CredorVWBL = _CredorVWBL ?? new CredorVWBL();

        public DespesaCadastroFormValidator() {

            RuleFor(x => x.TituloDespesa.descricao)
                .NotEmpty().WithMessage("Titulo é obrigatório.");

            RuleFor(x => x.idReferenciaPessoa)
                .NotEmpty().WithMessage("Informe o credor.")
                .Must((x, credor) => this.verificarCredor(x.idReferenciaPessoa)).WithMessage("O credor informado não pode ser localizado");

            RuleFor(x => x.flagTipoRepeticao)
                .NotEmpty().WithMessage("Informe o tipo de repetição");

            When(x => x.flagTipoRepeticao == TipoRepeticaoConst.NENHUMA, () => {
                RuleFor(x => x.TituloDespesa.dtVencimento)
                    .NotEmpty().WithMessage("Informe a data de vencimento");

                RuleFor(x => x.TituloDespesa.valorTotal)
                    .NotEmpty().WithMessage("Informe o valor da despesa")
                    .GreaterThan(0).WithMessage("O valor da despesa deve ser maior que zero");
            });

            When(x => !(x.TituloDespesa.idContaBancariaFavorecida > 0), () => {
                RuleFor(x => x.TituloDespesa.idContaBancariaFavorecida)
                    .Must((x, modoPagamento) => !this.verificarModoPagamento(x.TituloDespesa.idModoPagamento.toInt())).WithMessage("Informe a conta bancária do favorecido");
            });
            
            RuleFor(x => x.TituloDespesa.idContaBancariaFavorecida)
                .Must((x, contaBancaria) => this.verificarConta(x.TituloDespesa)).WithMessage("Para modo de pagamento conta corrente ou conta poupança, a conta do favorecido deve ser do mesmo banco da conta debitada, caso não seja, utilize DOC ou TED");

            When(x => x.flagTipoRepeticao == TipoRepeticaoConst.PARCELAMENTO, () => {

                RuleFor(x => x.TituloDespesa.qtdeRepeticao)
                    .NotEmpty().WithMessage("Informe a Qntd. de parcelas.")
                    .GreaterThan(1).WithMessage("A quantidade de parcelas não pode ser menor que 2.");

                RuleFor(x => x.TituloDespesa.qtdeRepeticao)
                    .Must((x, parcelas) => (x.TituloDespesa.listaTituloDespesaPagamento?.Count == x.TituloDespesa.qtdeRepeticao))
                    .WithMessage("Não foi registrado todas as parcelas");

                When(x => x.flagValorTotalParcelamento == "S", () => {
                    RuleFor(x => x.TituloDespesa.valorTotal)
                        .NotEmpty().WithMessage("Informe o valor total da despesa")
                        .GreaterThan(0).WithMessage("O valor total da despesa deve ser maior que zero");

                    RuleFor(x => x.TituloDespesa.qtdeRepeticao)
                        .Must((x, parcelas) => (x.TituloDespesa.listaTituloDespesaPagamento.Sum(y => y.valorOriginal) == x.TituloDespesa.valorTotal))
                        .WithMessage(x => $"O valor total de {x.TituloDespesa.valorTotal} difere da soma das parcelas de R${x.TituloDespesa.listaTituloDespesaPagamento.Sum(y => y.valorOriginal).ToString("C")}");
                });

                When(x => x.flagValorTotalParcelamento != "S", () => {
                    RuleFor(x => x.valorParcelas)
                        .NotEmpty().WithMessage("Informe o valor da parcela")
                        .GreaterThan(0).WithMessage("O valor da parcela deve ser maior que zero");
                });

                When(x => x.TituloDespesa.dtVencimento != null, () => {
                    RuleFor(x => x.TituloDespesa.qtdeRepeticao)
                        .Must((x, parcelas) => !(x.TituloDespesa.listaTituloDespesaPagamento.Any(y => y.dtVencimento < x.TituloDespesa.dtVencimento)))
                        .WithMessage("Não pode haver uma parcela com Dt. Vencimento menor que a data do primeiro vencimento");
                });
            });
        }

        /// <summary>
        /// Verifica se o credor informado existe de acordo com sua categoria
        /// </summary>
        private bool verificarCredor(string idReferenciaPessoa) {

            if (string.IsNullOrEmpty(idReferenciaPessoa)) {
                return false;
            }

            var array = idReferenciaPessoa.Split('#');

            var flagCategoriaPessoa = array[0];

            var idPessoa = Convert.ToInt32(array[1]);

            return this.OCredorVWBL.listar("").Any(x => x.flagCategoriaPessoa == flagCategoriaPessoa && x.idPessoa == idPessoa);
        }

        /// <summary>
        /// Verifica se o modo de pagamento necessita de conta bancaria
        /// </summary>
        private bool verificarModoPagamento(int idModoPagamento = 0) {

            return OModoPagamentoDespesaConsultaBL.query().Any(x => x.id == idModoPagamento && x.flagContaBancaria == true);
            
        }

        /// <summary>
        /// Verifica se o modo de pagamento é necessário que os bancos das contas de débito e crédito sejam iguais
        /// </summary>
        private bool verificarConta(TituloDespesa TituloDespesa) {

            if (TituloDespesa.idModoPagamento == (int) ModoPagamentoDespesaEnum.CONTA_CORRENTE_MESMO_TITULAR
                || TituloDespesa.idModoPagamento == (int) ModoPagamentoDespesaEnum.CONTA_CORRENTE_OUTRO_TITULAR
                || TituloDespesa.idModoPagamento == (int) ModoPagamentoDespesaEnum.CONTA_POUPANCA) {

                TituloDespesa.ContaBancaria = OContaBancariaBL.carregar(TituloDespesa.idContaBancaria.toInt()) ?? new ContaBancaria();
                TituloDespesa.ContaBancariaFavorecida = ODadoBancarioConsultaBL.carregar(TituloDespesa.idContaBancariaFavorecida.toInt()) ?? new DadoBancario();

                if (TituloDespesa.ContaBancaria.idBanco != TituloDespesa.ContaBancariaFavorecida.idBanco) {
                    return false;
                }
                
            }

            return true;

        }
    }
}
