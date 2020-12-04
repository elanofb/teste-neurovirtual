using BLL.Core.Events;
using DAL.Associados;
using System.Linq;
using System.Collections.Generic;
using System;
using System.Text;
using BLL.AssociadosContribuicoes;
using BLL.AssociadosOperacoes;
using BLL.Configuracoes;
using BLL.Configuracoes.Interface.IConfiguracaoPromocaoBL;
using BLL.Contribuicoes;
using BLL.Email;
using DAL.Pessoas;
using BLL.Pessoas;
using BLL.Request;
using BLL.Services;
using BLL.Transacoes.Movimentos;
using DAL.Relacionamentos;
using DAL.Configuracoes;
using EntityFramework.Extensions;
using Newtonsoft.Json;

namespace BLL.Associados.Events {

    public class OnAssociadoCadastradoHandler : DefaultBL, IHandler<object> {

        //Atributos
        private IPessoaRelacionamentoBL _PessoaRelacionamentoBL;
        private IContribuicaoBL _ContribuicaoBL;
        private AssociadoSaldoInicialGeradorBL _AssociadoSaldoInicialGeradorBL;
        
        

        //Propridades
        private IPessoaRelacionamentoBL OPessoaRelacionamentoBL => this._PessoaRelacionamentoBL = this._PessoaRelacionamentoBL ?? new PessoaRelacionamentoBL();
        private IContribuicaoBL OContribuicaoBL => this._ContribuicaoBL = this._ContribuicaoBL ?? new ContribuicaoPadraoBL();
        private AssociadoSaldoInicialGeradorBL OAssociadoSaldoInicialGeradorBL => this._AssociadoSaldoInicialGeradorBL = this._AssociadoSaldoInicialGeradorBL ?? new AssociadoSaldoInicialGeradorBL();
        

        protected Associado OAssociado { get; set; }
        
        //Chamador das ações do evento
        public virtual void execute(object source) {
            try {

                this.OAssociado = source as Associado;

                this.OAssociado.TipoAssociado = new TipoAssociadoBL().carregar(UtilNumber.toInt32(OAssociado.idTipoAssociado), this.OAssociado.idOrganizacao);

                this.gerarOcorrencia();

                this.gerarTaxaInscricao();

                this.iniciarProcessoAdmissao();

                this.gerarContribuicao();

                this.dispararEmail();


            } catch (Exception ex) {
                UtilLog.saveError(ex, $"Erro no manipulador do evento de cadastro do associado {this.OAssociado.Pessoa.nome}");
            }
        }

        //Gerar Ocorrencia para histórico do associado
        protected void gerarOcorrencia() {

            try {

                PessoaRelacionamento Ocorrencia = new PessoaRelacionamento();

                Ocorrencia.dtOcorrencia = DateTime.Now;

                Ocorrencia.idPessoa = OAssociado.idPessoa;

                Ocorrencia.idOcorrenciaRelacionamento = OcorrenciaRelacionamentoConst.idRealizacaoCadastro;

                Ocorrencia.observacao = this.OAssociado.observacoes;

                this.OPessoaRelacionamentoBL.salvar(Ocorrencia);

            } catch (Exception ex) {
                UtilLog.saveError(ex, "Erro ao salvar ocorrência após cadastro de associado pelo sistema");
            }
        }

        //Verificar se é necessário gerar a taxa de inscrição para o associado.
        //Se for uma importação, não gera a taxa
        protected void gerarTaxaInscricao() {

            try {

                if (OAssociado.dtImportacao.HasValue) {
                    return;
                }


                if (this.OAssociado.TipoAssociado == null) {
                    return;
                }

                if (!(this.OAssociado.TipoAssociado.valorTaxaInscricao > 0)) {
                    return;
                }

                var GeradorTituloBL = new TituloReceitaGeradorTaxaInscricaoBL();

                GeradorTituloBL.gerar(OAssociado);

            } catch (Exception ex) {
                UtilLog.saveError(ex, "Erro ao gerar taxa de inscrição");
            }

        }

        // Vincular contribuição ao Associado
        protected void gerarContribuicao() {

            try {

                if (OAssociado.dtImportacao.HasValue) {
                    return;
                }

                if (this.OAssociado.TipoAssociado == null) {
                    return;
                }

                if (this.OAssociado.TipoAssociado.flagGerarCobrancaPosCadastro == false) {
                    return;
                }

                new AssociadoContribuicaoGeracaoBL().gerarCobranca(OAssociado, null, null, null, true);

            } catch (Exception ex) {

                UtilLog.saveError(ex, $"Erro ao vincular uma contribuição para o associado {this.OAssociado.Pessoa.nome}");

            }
        }

        //
        protected void dispararEmail() {

            try {
                if (OAssociado.dtImportacao.HasValue) {
                    return;
                }

                ConfiguracaoNotificacao OConfiguracaoNotificacao = ConfiguracaoNotificacaoBL.getInstance.carregar(OAssociado.idOrganizacao);

                if (String.IsNullOrEmpty(OConfiguracaoNotificacao.emailNovoAssociado)) {
                    return;
                }

                List<string> listaEmail = OConfiguracaoNotificacao.emailNovoAssociado.Split(';').ToList();

                var OEmail = EnvioNovoAssociado.factory(OAssociado.idOrganizacao, this.OAssociado.Pessoa.ToEmailList(), null, listaEmail);

                OEmail.enviar(this.OAssociado);

            } catch (Exception ex) {
                UtilLog.saveError(ex, "Erro ao enviar e-mail após cadastro de associado pelo sistema");
            }
        }


        protected void iniciarProcessoAdmissao() {

            if (this.OAssociado.TipoAssociado.flagProcessoAdmissao == true) {

                //this.OInicializadorProcessoAdmissaoBL.iniciarProcesso(this.OAssociado);

            } else {

                var dtAdmissao = DateTime.Now;

                db.Associado.Where(x => x.id == this.OAssociado.id).Update(x => new Associado() {
                    ativo = "S",
                    dtAdmissao = dtAdmissao
                });

                this.OAssociado.dtAdmissao = dtAdmissao;
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        protected void gerarSaldoInicial() {

            try {

                this.OAssociadoSaldoInicialGeradorBL.gerarSaldoInicial(this.OAssociado.id, this.OAssociado.idOrganizacao);
                

            } catch (Exception ex) {

                UtilLog.saveError(ex, "Erro ao configurar o saldo inicial em OnAssociadoCadastradoHandler");

            }

        }
    }
}