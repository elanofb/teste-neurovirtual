using System.Collections.Generic;
using System.Linq;
using BLL.Contribuicoes;
using DAL.Contribuicoes;
using FluentValidation.Attributes;

namespace WEB.Areas.Contribuicoes.ViewModels {

    [Validator(typeof(ContribuicaoPrecoValidator))]
    public class ContribuicaoPrecoForm {

        //Atributos
        private IContribuicaoTabelaPrecoBL _ContribuicaoTabelaPrecoBL;
        private IContribuicaoBL _ContribuicaoBL;

        //Serviços
        public IContribuicaoTabelaPrecoBL OContribuicaoTabelaPrecoBL => _ContribuicaoTabelaPrecoBL = _ContribuicaoTabelaPrecoBL ?? new ContribuicaoTabelaPrecoBL();
        public IContribuicaoBL OContribuicaoBL => _ContribuicaoBL = _ContribuicaoBL ?? new ContribuicaoPadraoBL();

        //Propriedades
        public Contribuicao Contribuicao { get; set; }

        public ContribuicaoPreco ContribuicaoPreco { get; set; }

        public ContribuicaoTabelaPreco ContribuicaoTabelaPreco { get; set; }

        public List<int?> idsTipoJaCadastrados { get; set; }
         
        //Construtor
        public ContribuicaoPrecoForm() {

            this.ContribuicaoPreco = new ContribuicaoPreco();

            this.ContribuicaoTabelaPreco = new ContribuicaoTabelaPreco();

            this.Contribuicao = new Contribuicao();

            this.idsTipoJaCadastrados = new List<int?>();
        }

        //
        public void carregarDados(int idTabelaPreco) {

            this.ContribuicaoTabelaPreco = this.OContribuicaoTabelaPrecoBL.carregar(idTabelaPreco) ?? new ContribuicaoTabelaPreco();

            this.ContribuicaoPreco.idTabelaPreco = idTabelaPreco;

            this.Contribuicao = this.OContribuicaoBL.carregar(this.ContribuicaoTabelaPreco.idContribuicao) ?? new Contribuicao();

            if (this.ContribuicaoPreco.id == 0) {
                this.idsTipoJaCadastrados = this.ContribuicaoTabelaPreco.listaPrecos.Where(x => x.flagExcluido == "N").Select(x => x.idTipoAssociado).ToList();
            }

            if (this.Contribuicao.idPeriodoContribuicao == PeriodoContribuicaoConst.ANUAL) {

                this.configurarDescontos();

            }

        }

        //
        private void configurarDescontos() {

            this.ContribuicaoPreco.listaDesconto = this.ContribuicaoPreco.listaDesconto.Where(x => x.dtExclusao == null).ToList();

            if (this.ContribuicaoPreco.listaDesconto.Count < 3) {

                int i = 0;

                while (i < 3) {

                    this.ContribuicaoPreco.listaDesconto.Add(new ContribuicaoPrecoDesconto());

                    i++;
                }
            }
        }

    }
}