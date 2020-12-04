using System;
using System.Linq;
using System.Security.Principal;
using BLL.Associados;
using BLL.Transacoes.Debitos;
using DAL.Associados;
using DAL.Permissao.Security.Extensions;
using DAL.Pessoas;
using DAL.Transacoes;

namespace WEB.Areas.Transacoes.ViewModels {

    public class DebitoVM {
        
        //Atributos
        private ValidadorDebito _ValidadorDebito;
        private IDebitoFacade _DebitoFacade;
        private IAssociadoConsultaBL _AssociadoConsultaBL;

        //Propriedades
        private ValidadorDebito OValidadorDebito => this._ValidadorDebito = this._ValidadorDebito ?? new ValidadorDebito();
        private IDebitoFacade ODebitoFacade => this._DebitoFacade = this._DebitoFacade ?? new DebitoFacade();
        private IAssociadoConsultaBL OAssociadoConsultaBL => this._AssociadoConsultaBL = this._AssociadoConsultaBL ?? new AssociadoConsultaBL();
        
        // Propriedades
        public Movimento OMovimento { get; set; }
        public MovimentoResumoVW OMovimentoVW { get; set; }
        public MovimentoOperacaoDTO OMovimentoOperacaoDTO { get; set; }
        
        // Constantes
        private IPrincipal User => HttpContextFactory.Current.User;
        
        // Construtor
        public DebitoVM() {
            this.OMovimentoOperacaoDTO = new MovimentoOperacaoDTO();
            this.OMovimentoOperacaoDTO.MembroOrigem = new Associado();
            this.OMovimentoOperacaoDTO.MembroOrigem.Pessoa = new Pessoa();
        }

        public void carregarParametros(){
            
            this.OMovimentoOperacaoDTO.nroContaDestino = UtilRequest.getInt32("nroContaDestino");
            this.OMovimentoOperacaoDTO.valorOperacao = UtilRequest.getDecimal("valorOperacao");
            
            this.carregarMembroDestino();
        }
        
        //
        public UtilRetorno debitar() {

            var ORetorno = UtilRetorno.newInstance(false);
            
            ORetorno = ODebitoFacade.debitar(this.OMovimentoOperacaoDTO);

            this.OMovimento = ORetorno.info as Movimento;
            
            return ORetorno;
        }                
        
        public UtilRetorno validar(){

            var ORetorno = UtilRetorno.newInstance(false);
            
            ORetorno = OValidadorDebito.validar(this.OMovimentoOperacaoDTO);
            
            this.OMovimentoVW = ORetorno.info as MovimentoResumoVW;

            return ORetorno;
        }

        private void carregarMembroDestino() {

            this.OMovimentoOperacaoDTO.MembroDestino = OAssociadoConsultaBL.queryNoFilter(User.idOrganizacao())
                .FirstOrDefault(x => x.nroAssociado == this.OMovimentoOperacaoDTO.nroContaDestino);

        }
    }
}