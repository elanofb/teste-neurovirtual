using DAL.Contribuicoes;
using DAL.AssociadosContribuicoes;
using System;
using System.Linq;
using BLL.AssociadosContribuicoes.Events;
using BLL.Configuracoes;
using BLL.Contribuicoes;
using BLL.Core.Events;
using DAL.Associados;
using BLL.Services;
// ReSharper disable All

namespace BLL.AssociadosContribuicoes {

    public class AssociadoContribuicaoGeracaoBL : DefaultBL, IContribuicaoGeracaoBL {

        //Atributos
        private IContribuicaoBL _ContribuicaoBL;
        
        //Servicos
        private IContribuicaoBL OContribuicaoBL => this._ContribuicaoBL = this._ContribuicaoBL ?? new ContribuicaoPadraoBL();


        //Events
        private readonly EventAggregator onContribuicaoVinculada = OnContribuicaoVinculada.getInstance;

        //Construtor
        public AssociadoContribuicaoGeracaoBL() {

        }

        /// <summary>
        ///Vincular uma contribuicao com o associado
        ///Pode-se informar qual contribuição será vinculada 
        ///Se nao for informada nenhuma contribuicao, o sistema irá considerar a contribuicao padrao do associado
        /// </summary>
        public UtilRetorno gerarCobranca(Associado OAssociado, Contribuicao OContribuicao, DateTime? dtVencimento = null, DateTime? dtNovoVencimento = null, bool flagPrimeiraCobranca = false, decimal valorCustomizado = 0) {

            if (OAssociado?.TipoAssociado == null) {
                return UtilRetorno.newInstance(true, "O associado informado não foi localizado.");
            }

            int? idContribuicao = OContribuicao?.id;

            if (idContribuicao.toInt() == 0) {

            }

            if (OContribuicao == null || !OContribuicao.listaContribuicaoVencimento.Any()) {

                OContribuicao = this.OContribuicaoBL.carregar(idContribuicao.toInt());
            }

            if (OContribuicao == null) {
                return UtilRetorno.newInstance(true, "A contribuição informada não foi localizada.");
            }

            var TabelaPreco = OContribuicao.retornarTabelaVigente();

            if (TabelaPreco.id == 0) {
                return UtilRetorno.newInstance(true, "Não há tabela de preços configuradas para a contribuição.");
            }

            var Vencimento = OContribuicao.retornarProximoVencimento(dtVencimento);

            var Preco = flagPrimeiraCobranca ? TabelaPreco.retornarPrecoAtual(UtilNumber.toInt32(OAssociado.idTipoAssociado), DateTime.Today) : TabelaPreco.retornarPreco(UtilNumber.toInt32(OAssociado.idTipoAssociado));

            if (!dtVencimento.HasValue){

               dtVencimento = Vencimento.dtVencimento;

            }

            if (flagPrimeiraCobranca) {

                dtNovoVencimento = DateTime.Today.AddDays(UtilNumber.toInt32(OAssociado.TipoAssociado.diasPrazoPrimeiraCobranca));

                //Caso o vencimento seja por admissão ou por ultimo pagamento, enviamos a data de vencimento vazia no ato do cadastro.
                //Por este motivo atualizamos a competência com base na data de hoje.
                if (OContribuicao.flagVencimentoVariado() && !Vencimento.dtVencimento.HasValue){

                    dtVencimento = DateTime.Today;

                    Vencimento.dtVencimento = dtNovoVencimento;

                    Vencimento.dtInicioVigencia = dtNovoVencimento;

                    Vencimento.dtFimVigencia = dtNovoVencimento.Value.AddYears(1);

                }   
                             
            }

            if (!dtVencimento.HasValue) {

                return UtilRetorno.newInstance(true, "O vencimento não está configurado ou não foi informado para essa contribuição.");

            }

            bool flagVencimentoValido = this.validarVencimento(OContribuicao, dtVencimento.Value);

            if (!flagVencimentoValido) {

                return UtilRetorno.newInstance(true, "O vencimento informado não está dentro das opções válidas para a cobrança informada.");
            }

            AssociadoContribuicao OAssociadoContribuicao = new AssociadoContribuicao();

            this.verificarIsencao(ref OAssociadoContribuicao, Preco, OAssociado);

            OAssociadoContribuicao.idAssociado = OAssociado.id;

            OAssociadoContribuicao.idOrganizacao = OAssociado.idOrganizacao;

            OAssociadoContribuicao.idUnidade = OAssociado.idUnidade;

            OAssociadoContribuicao.idTipoAssociado = UtilNumber.toInt32(OAssociado.idTipoAssociado);

            OAssociadoContribuicao.idContribuicao = idContribuicao.toInt();

            OAssociadoContribuicao.valorOriginal = UtilNumber.toDecimal(Preco.valorFinal);

            OAssociadoContribuicao.valorAtual = valorCustomizado > 0? valorCustomizado : OAssociadoContribuicao.valorOriginal;

            OAssociadoContribuicao.dtVencimentoOriginal = dtVencimento.Value;

            OAssociadoContribuicao.dtVencimentoAtual = dtNovoVencimento ?? OAssociadoContribuicao.dtVencimentoOriginal;

            OAssociadoContribuicao.idContribuicaoVencimento = Vencimento.id;

            OAssociadoContribuicao.dtInicioVigencia = Vencimento.dtInicioVigencia;

            OAssociadoContribuicao.dtFimVigencia = Vencimento.dtFimVigencia;

            return this.gerarCobranca(OAssociadoContribuicao);
        }


        //Salvar anuidade unitaria para um associado.
        public UtilRetorno gerarCobranca(AssociadoContribuicao OAssociadoContribuicao) {

            if (!OAssociadoContribuicao.flagImportado) {

                OAssociadoContribuicao.dtPagamento = null;
            }

            OAssociadoContribuicao.setDefaultInsertValues();

            OAssociadoContribuicao.dtExclusao = null;

            OAssociadoContribuicao.Associado = null;

            OAssociadoContribuicao.Contribuicao = null;

            OAssociadoContribuicao.ContribuicaoVencimento = null;

            db.AssociadoContribuicao.Add(OAssociadoContribuicao);

            db.SaveChanges();

            bool flagSucesso = OAssociadoContribuicao.id > 0;

            if (!flagSucesso) {
                return UtilRetorno.newInstance(true, "Não foi possível salvar a contribuição para o associado.");
            }

            //onContribuicaoVinculada.subscribe(new OnContribuicaoVinculadaHandler());

            onContribuicaoVinculada.publish(OAssociadoContribuicao as object);

            return UtilRetorno.newInstance(false, "A cobrança foi gerada com sucesso.", OAssociadoContribuicao);


        }

        /// <summary>
        ///Vincular uma contribuicao com o associado dependente
        ///Pode-se informar qual contribuição será vinculada 
        /// </summary>
        public UtilRetorno gerarCobrancaDependente(Associado OAssociado, AssociadoContribuicao OAssociadoContribuicao) {

            if (OAssociado?.TipoAssociado == null) {
                return UtilRetorno.newInstance(true, "O tipo do associado dependente informado não foi informado.");
            }

            var OContribuicao = OAssociadoContribuicao.Contribuicao;

            if (OContribuicao == null) {

                OContribuicao = this.OContribuicaoBL.carregar(OAssociadoContribuicao.idContribuicao);
            }

            int? idContribuicao = OContribuicao.id;


            var TabelaPreco = OContribuicao.retornarTabelaVigente();

            if (TabelaPreco == null) {
                return UtilRetorno.newInstance(true, "Não há tabela de preços configuradas para a contribuição.");
            }

            var Preco = TabelaPreco.retornarPreco(UtilNumber.toInt32(OAssociado.idTipoAssociado));

            AssociadoContribuicao ODependenteContribuicao = new AssociadoContribuicao();

            this.verificarIsencao(ref ODependenteContribuicao, Preco, OAssociado);

            ODependenteContribuicao.idAssociado = OAssociado.id;

            ODependenteContribuicao.idOrganizacao = OAssociadoContribuicao.idOrganizacao;

            ODependenteContribuicao.idUnidade = OAssociadoContribuicao.idUnidade;

            ODependenteContribuicao.idTipoAssociado = UtilNumber.toInt32(OAssociado.idTipoAssociado);

            ODependenteContribuicao.idContribuicao = idContribuicao.toInt();

            ODependenteContribuicao.valorOriginal = UtilNumber.toDecimal(Preco.valorFinal);

            ODependenteContribuicao.valorAtual = ODependenteContribuicao.valorOriginal;

            ODependenteContribuicao.dtVencimentoOriginal = OAssociadoContribuicao.dtVencimentoOriginal;

            ODependenteContribuicao.dtVencimentoAtual = ODependenteContribuicao.dtVencimentoOriginal;

            ODependenteContribuicao.idContribuicaoVencimento = OAssociadoContribuicao.idContribuicaoVencimento;

            ODependenteContribuicao.dtInicioVigencia = OAssociadoContribuicao.dtInicioVigencia;

            ODependenteContribuicao.dtFimVigencia = OAssociadoContribuicao.dtFimVigencia;

            ODependenteContribuicao.idAssociadoContribuicaoPrincipal = OAssociadoContribuicao.id;

            ODependenteContribuicao.Associado = OAssociado;

            return this.gerarCobranca(ODependenteContribuicao);
        }

        //Verificar se o associado em questao sera isento para essa contribuica ou nao
        public void verificarIsencao(ref AssociadoContribuicao OAssociadoContribuicao, ContribuicaoPreco Preco, Associado OAssociado) {

            //int idadeIsencao = ConfiguracaoContribuicaoBL.getInstance.carregar().idadeIsencao ?? 0;

            //OAssociadoContribuicao.flagIsento = false;

            //if (!OAssociado.Pessoa.dtNascimento.HasValue || OAssociado.Pessoa.dtNascimento == DateTime.MinValue) {
            //    return;
            //}

            //if (OAssociado.Pessoa.calcularIdade() >= idadeIsencao && (idadeIsencao > 0)) {

            //    OAssociadoContribuicao.flagIsento = true;

            //    OAssociadoContribuicao.motivoIsencao = $"Associado isento por ser nascido em {OAssociado.Pessoa.dtNascimento.exibirData()}. Idade igual ou superior à {idadeIsencao} anos";

            //    OAssociadoContribuicao.observacoes = OAssociadoContribuicao.motivoIsencao;
            //}

            if (Preco.flagIsento == true) {

                OAssociadoContribuicao.dtIsencao = DateTime.Now;
                
                OAssociadoContribuicao.flagIsento = true;

                OAssociadoContribuicao.motivoIsencao = $"Associado isento por ter o perfil {OAssociado.TipoAssociado.descricao}";

                OAssociadoContribuicao.observacoes = OAssociadoContribuicao.motivoIsencao;
            }
        }

        //Verificar se o contato já existe
        public bool validarVencimento(Contribuicao OContribuicao, DateTime dtVencimento) {

            if (OContribuicao.idTipoVencimento != TipoVencimentoConst.FIXO_PELA_CONTRIBUICAO) {

                return true;

            }

            byte dia = (byte)dtVencimento.Day;

            byte mes = (byte)dtVencimento.Month;

            var listaVencimentos = OContribuicao.listaContribuicaoVencimento.Where(x => x.dtExclusao == null).ToList();

            bool flagExiste = listaVencimentos.Any(x => x.diaVencimento == dia && x.mesVencimento == mes);

            return flagExiste;
        }

    }
}