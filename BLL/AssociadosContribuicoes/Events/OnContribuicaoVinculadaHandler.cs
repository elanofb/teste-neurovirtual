using System;
using System.Collections.Generic;
using System.Linq;
using BLL.BoletosBancarios;
using BLL.Core.Events;
using DAL.AssociadosContribuicoes;
using DAL.Relacionamentos;
using DAL.Pessoas;
using BLL.Pessoas;
using BLL.Comissoes;
using BLL.Configuracoes;
using BLL.Financeiro;
using BLL.Services;
using DAL.Comissoes;
using DAL.Contribuicoes;
using DAL.Financeiro;
using DAL.Financeiro.Entities;
using DAL.Permissao.Security.Extensions;
using EntityFramework.Extensions;

namespace BLL.AssociadosContribuicoes.Events {

    public class OnContribuicaoVinculadaHandler : DefaultBL, IHandler<object> {

        //Atributos

        //Propridades
        private IAssociadoContribuicaoBL OAssociadoContribuicaoBL;

        //Chamador das ações do evento
        public void execute(object source) {

            try{ 
                 
                var NovaContribuicao = (source as AssociadoContribuicao);

                if (NovaContribuicao == null) {
                    throw new Exception("A contribuição informada é nula.");
                }

                this.OAssociadoContribuicaoBL = new AssociadoContribuicaoBL();

                var OAssociadoContribuicao = this.OAssociadoContribuicaoBL.carregar(NovaContribuicao.id);

                this.gerarOcorrencia(OAssociadoContribuicao.Associado.idPessoa, OAssociadoContribuicao.valorAtual);

                this.gerarTituloFinanceiro(OAssociadoContribuicao);

                this.gerarAssociadoContribuicaoComissao(OAssociadoContribuicao);

            } catch (Exception ex) {

                var NovaContribuicao = (source as AssociadoContribuicao);

                UtilLog.saveError(ex, $"Erro no manipulador de evento: OnContribuicaoVinculadaHandler, Contribuicao: {NovaContribuicao?.id}");

            }
        }

        //Gerar Ocorrencia para histórico do associado
        private void gerarOcorrencia(int idPessoa, decimal valor) {

            PessoaRelacionamento Ocorrencia = new PessoaRelacionamento();

            Ocorrencia.dtOcorrencia = DateTime.Now;

            Ocorrencia.idPessoa = idPessoa;

            Ocorrencia.idOcorrenciaRelacionamento = OcorrenciaRelacionamentoConst.idGeracaoCobranca;

            Ocorrencia.observacao = $"Contribuicao gerada com valor: {valor}";

            var OPessoaRelacionamentoBL = new PessoaRelacionamentoBL();

            OPessoaRelacionamentoBL.salvar(Ocorrencia);

        }

        /// <summary>
        /// Gerar os titulos de receita referentes a cobranca
        /// Se houver configuracao gerar boleto para a contribuicao
        /// </summary>
        private void gerarTituloFinanceiro(AssociadoContribuicao OAssociadoContribuicao) {

            if (OAssociadoContribuicao.flagIsento == true) {
                return;
            }

            ITituloReceitaGeradorBL OGeradorTituloBL;

            if (OAssociadoContribuicao.idAssociadoContribuicaoPrincipal > 0) {

                OGeradorTituloBL = new TituloReceitaGeradorContribuicaoDependenteBL();

            } else {

                OGeradorTituloBL = new TituloReceitaGeradorContribuicaoBL();

            }

            var RetornoTitulo = OGeradorTituloBL.gerar(OAssociadoContribuicao);

            if (OAssociadoContribuicao.Contribuicao.flagGerarBoleto != true) {
                return;
            }

            //Se for uma contribuicao de dependente, removemos o boleto anterior para criar um novo com valor atualizado
            if (OAssociadoContribuicao.idAssociadoContribuicaoPrincipal > 0) {

                var OTituloReceita = RetornoTitulo.info as TituloReceita ?? new TituloReceita();

                this.removerPagamentoDesatualizado(OTituloReceita.id);
            }

            this.gerarBoletos(OAssociadoContribuicao);
        }

        /// <summary>
        /// Remover as parcelas de boleto que foram geradas anteriormente e precisam ser atualizadas
        /// Geralmente utilizado para atualizar valor de uma contribuicao que teve um ou mais dependente incluído
        /// </summary>
        private void removerPagamentoDesatualizado(int idTituloReceita) {

            int? idUsuarioLogado = null;

            if (User.id() > 0) {
                idUsuarioLogado = User.id();
            }

            db.TituloReceitaPagamento.Where(x => x.idTituloReceita == idTituloReceita)
                                    .Update(x => new TituloReceitaPagamento {
                                        dtExclusao = DateTime.Now,
                                        idUsuarioExclusao = idUsuarioLogado,
                                        motivoExclusao = "Remoção de boleto para inclusão de valor para dependente"
                                    });

        }

        /// <summary>
        /// Se houver a configuracao, gerar boletos para a cobrança
        /// </summary>
        private void gerarBoletos(AssociadoContribuicao OAssociadoContribuicao) {

            //Gerar TituloReceitaPagamento
            var OGeradorPagamento = new AssociadoContribuicaoBoletoGeracaoBL();

            var RetornoInfo = OGeradorPagamento.gerarPagamentoBoleto(OAssociadoContribuicao);

            //Gerar arquivo do boleto
            var OGeradorBoleto = new GeracaoBoletoBL();

            var OPagamento = RetornoInfo.info as TituloReceitaPagamento;
            if (OPagamento == null) {
                return;
            }

            OGeradorBoleto.gerarBoleto(OPagamento);

        }

        /// <summary>
        /// Caso exista configuração de comissionamento gerar o registro para consulta posterior
        /// </summary>
        private void gerarAssociadoContribuicaoComissao(AssociadoContribuicao OAssociadoContribuicao) {


            var OConfiguracaoComissao = ConfiguracaoComissaoBL.getInstance.carregar();

            if (OConfiguracaoComissao.flagHabilitar != true) {
                return;
            }

            var idRepresentante = 0;

            if (idRepresentante == 0) {
                return;
            }

            var OPlanoComissaoRepresentanteBL = new PlanoComissaoRepresentanteBL();

            var OPlanoComissaoRepresentante = OPlanoComissaoRepresentanteBL.listar(idRepresentante, 0, TipoPlanoComissaoConst.CONTRIBUICAO)
                                                                                .FirstOrDefault(x => x.PlanoComissao.ativo == true && x.Representante.ativo == "S" && x.Representante.dtExclusao == null);

            if (OPlanoComissaoRepresentante == null) {
                return;
            }

            var idsPerfisComissionavies = OConfiguracaoComissao.listaPerfisComissionaveis.Select(x => x.idPerfilAcesso).ToList();
            if (!idsPerfisComissionavies.Contains(OPlanoComissaoRepresentante.Representante.idPerfilAcesso)) {
                return;
            }

            var OAssociadoContribuicaoComissao = new AssociadoContribuicaoComissao();

            OAssociadoContribuicaoComissao.idAssociadoContribuicao = OAssociadoContribuicao.id;

            OAssociadoContribuicaoComissao.idPlanoComissao = OPlanoComissaoRepresentante.idPlanoComissao;

            OAssociadoContribuicaoComissao.idRepresentante = OPlanoComissaoRepresentante.idRepresentante;

            var OAssociadoContribuicaoComissaoBL = new AssociadoContribuicaoComissaoBL();

            OAssociadoContribuicaoComissaoBL.salvar(OAssociadoContribuicaoComissao);
        }
    }
}