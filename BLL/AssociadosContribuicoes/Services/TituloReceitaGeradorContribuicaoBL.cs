using System;
using System.Collections.Generic;
using BLL.Configuracoes;
using BLL.Financeiro;
using DAL.AssociadosContribuicoes;
using DAL.Contribuicoes;
using DAL.Financeiro;

namespace BLL.AssociadosContribuicoes {

    public class TituloReceitaGeradorContribuicaoBL : TituloReceitaGeradorBL {

        //Atributos
        private ITituloReceitaBL _TituloReceitaBL;

        //Propriedades
        private int idTipoReceita { get; set; }

        private ITituloReceitaBL OTituloReceitaBL => _TituloReceitaBL = _TituloReceitaBL ?? new TituloReceitaContribuicaoBL();


        //Construtor
        public TituloReceitaGeradorContribuicaoBL() {

            this.idTipoReceita = TipoReceitaConst.CONTRIBUICAO;

        }

        //Metodo para geracao do titulo de receita
        public override UtilRetorno gerarLote(object OrigemTitulo) {

            List<AssociadoContribuicao> listaOrigemTitulo = (OrigemTitulo as List<AssociadoContribuicao>);

            if (listaOrigemTitulo == null) {
                return UtilRetorno.newInstance(true, "A lista informada é nula");
            }

            foreach (var Item in listaOrigemTitulo) {
                this.gerar(Item);
            }

            return UtilRetorno.newInstance(false, "A relação de títulos foi gerada com sucesso.");
        }

        //Metodo para geracao do titulo de receita
        public override UtilRetorno gerar(object OrigemTitulo) {

            AssociadoContribuicao AssociadoContribuicao = (OrigemTitulo as AssociadoContribuicao);

            if (AssociadoContribuicao == null) {
                return UtilRetorno.newInstance(true, "O registro Associado Contribuição está nulo.");
            }

            if (AssociadoContribuicao.Contribuicao == null) {
                return UtilRetorno.newInstance(true, "O registro Contribuição está nulo.");
            }

            //Verificar se o titulo já existe
            var OTituloReceita = this.OTituloReceitaBL.carregarPorReceita(AssociadoContribuicao.id);

            if (OTituloReceita != null) {

                return UtilRetorno.newInstance(false, "O título já foi gerado anteriormente.", OTituloReceita);

            }

            var OAssociado = AssociadoContribuicao.Associado;

            var Contribuicao = AssociadoContribuicao.Contribuicao;

            var ConfiguracaoContribuicao = ConfiguracaoContribuicaoBL.getInstance.carregar(AssociadoContribuicao.idOrganizacao);

            OTituloReceita = new TituloReceita();

            OTituloReceita.idPessoa = AssociadoContribuicao.Associado.idPessoa;

            OTituloReceita.idTipoReceita = (byte)idTipoReceita;

            OTituloReceita.idReceita = AssociadoContribuicao.id;

            OTituloReceita.idOrganizacao = AssociadoContribuicao.idOrganizacao;

            OTituloReceita.idUnidade = AssociadoContribuicao.idUnidade;

            OTituloReceita.qtdeRepeticao = 1;

            OTituloReceita.mesCompetencia = (byte?)AssociadoContribuicao.dtInicioVigencia?.Month;

            OTituloReceita.anoCompetencia = (short?)AssociadoContribuicao.dtInicioVigencia?.Year;

            if (OTituloReceita.mesCompetencia > 0 && OTituloReceita.anoCompetencia > 0){

                byte? diaCompetencia = AssociadoContribuicao.dtInicioVigencia?.Day.toByte();

                diaCompetencia = diaCompetencia.toByte() > 0 ? diaCompetencia.toByte() : (byte)1;

                OTituloReceita.dtCompetencia = new DateTime(OTituloReceita.anoCompetencia.toInt(), OTituloReceita.mesCompetencia.toInt(), diaCompetencia.toInt());

            }

            OTituloReceita.idContaBancaria = Contribuicao.idContaBancaria > 0 ? Contribuicao.idContaBancaria : ConfiguracaoContribuicao.idContaBancariaPadrao;

            OTituloReceita.idCentroCusto = Contribuicao.idCentroCusto > 0 ? Contribuicao.idCentroCusto : ConfiguracaoContribuicao.idCentroCustoPadrao;

            OTituloReceita.idMacroConta = Contribuicao.idMacroConta > 0 ? Contribuicao.idMacroConta : ConfiguracaoContribuicao.idMacroContaPadrao;

            OTituloReceita.idCategoria = Contribuicao.idCategoriaTitulo;

            OTituloReceita.limiteParcelamento = Contribuicao.qtdeLimiteParcelas ?? 1;

            OTituloReceita.flagCartaoCreditoPermitido = Contribuicao.flagCartaoCreditoPermitido;

            OTituloReceita.flagBoletoBancarioPermitido = Contribuicao.flagBoletoBancarioPermitido;

            OTituloReceita.flagDepositoPermitido = Contribuicao.flagDepositoPermitido;

            OTituloReceita.descricao = $"Cobrança {Contribuicao.descricao} - {AssociadoContribuicao.Associado.Pessoa.nome}";

            if (AssociadoContribuicao.dtInicioVigencia.HasValue) {

                OTituloReceita.descricao = $"{OTituloReceita.descricao}. Vigência: {AssociadoContribuicao.dtInicioVigencia.exibirData()} à {AssociadoContribuicao.dtFimVigencia.exibirData()}";
            }

            OTituloReceita.valorTotal = AssociadoContribuicao.valorAtual;

            OTituloReceita.dtVencimentoOriginal = AssociadoContribuicao.dtVencimentoOriginal;

            OTituloReceita.dtVencimento = AssociadoContribuicao.dtVencimentoAtual;

            this.preencherRecibo(ref OTituloReceita, OAssociado.Pessoa);

            var TabelaPreco = Contribuicao.retornarTabelaVigente();

            OTituloReceita.listaDescontosAntecipacao = TabelaPreco.retornarListaDescontos(AssociadoContribuicao.idTipoAssociado, AssociadoContribuicao.dtVencimentoOriginal);

            this.salvar(OTituloReceita);

            return UtilRetorno.newInstance(false, "O título foi gerado com sucesso.", OTituloReceita);
        }

    }
}