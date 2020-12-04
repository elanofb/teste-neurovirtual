using System;
using System.Collections.Generic;
using System.Linq;
using BLL.Associados;
using BLL.AssociadosOperacoes.Events;
using BLL.Configuracoes.Interface.IConfiguracaoPromocaoBL;
using BLL.Core.Events;
using BLL.Services;
using BLL.Transacoes.Debitos;
using BLL.Transacoes.Movimentos;
using BLL.Transacoes.Transferencias;
using DAL.Associados;
using DAL.Configuracoes;
using DAL.Permissao.Security.Extensions;
using DAL.Transacoes;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;

namespace BLL.AssociadosOperacoes {

    //public class AssociadoSaldoInicialGeradorBL : DefaultBL, IAssociadoSaldoInicialGeradorBL {
    public class AssociadoSaldoInicialGeradorBL : DefaultBL{
           
        // Atributos
        private IAssociadoConsultaBL _AssociadoConsultaBL;
        private IMembroSaldoConsultaBL _MembroSaldoConsultaBL;
        private IConfiguracaoPromocaoConsultaBL _ConfiguracaoPromocaoConsultaBL;
        private ITransferenciaFacade _TransferenciaFacade;  
        private ICarregadorDadosOperacao _CarregadorDados;

        // Propriedades
        private IAssociadoConsultaBL OAssociadoConsultaBL => this._AssociadoConsultaBL = this._AssociadoConsultaBL ?? new AssociadoConsultaBL();
        private IMembroSaldoConsultaBL OMembroSaldoConsultaBL => this._MembroSaldoConsultaBL = this._MembroSaldoConsultaBL ?? new MembroSaldoConsultaBL();
        private IConfiguracaoPromocaoConsultaBL OConfiguracaoPromocaoConsultaBL => this._ConfiguracaoPromocaoConsultaBL = this._ConfiguracaoPromocaoConsultaBL ?? new ConfiguracaoPromocaoConsultaBL();
        private ITransferenciaFacade OTransferenciaFacade => this._TransferenciaFacade = this._TransferenciaFacade ?? new TransferenciaFacade();
        private ICarregadorDadosOperacao CarregadorDados => _CarregadorDados =_CarregadorDados ?? new CarregadorDadosOperacao();
        
       
        //Gerar saldo inicial quando houve promoção ativa
        public UtilRetorno gerarSaldoInicial(int id, int idOrganizacaoParam) {
            
            UtilRetorno ORetorno = new UtilRetorno{ flagError = false};
            
            ConfiguracaoPromocao OConfiguracaoPromocao = this.OConfiguracaoPromocaoConsultaBL.carregar();
            
            if (OConfiguracaoPromocao.id == 0 || OConfiguracaoPromocao.valorPremioNovoMembro <= 0) {
                
                ORetorno.flagError = true;
                ORetorno.listaErros.Add("Não existem promoções disponíveis!");

                return ORetorno;
            }
            
            if (OConfiguracaoPromocao.dtInicioPremioNovoMembro > DateTime.Now.Date || OConfiguracaoPromocao.dtFimPremioNovoMembro < DateTime.Now.Date) {
                
                ORetorno.flagError = true;
                ORetorno.listaErros.Add("Não existem promoções disponíveis!");

                return ORetorno;
                
            }                        
            
            if (id == 0) {
                
                ORetorno.flagError = true;
                ORetorno.listaErros.Add("Não foi possível localizar o membro!");

                return ORetorno;

            }
            
            Associado OMembro = this.OAssociadoConsultaBL.queryNoFilter().FirstOrDefault(x => x.id == id) ?? new Associado();
            
            if (OMembro.id.toInt() == 0) {
                
                ORetorno.flagError = true;
                ORetorno.listaErros.Add("Não foi possível localizar o membro!");

                return ORetorno;
                
            }
            
            if (OMembro.idTipoCadastro == AssociadoTipoCadastroConst.COMERCIANTE) {
                
                ORetorno.flagError = true;
                ORetorno.listaErros.Add("Tipo de cadastro inválido para aplicação da promoção!");
                
                return ORetorno;
                
            }
            
            MembroSaldo OSaldo = this.OMembroSaldoConsultaBL.query(id, idOrganizacaoParam).FirstOrDefault() ?? new MembroSaldo();
            
            if (OSaldo.saldoAtual > 0) {
                
                ORetorno.flagError = true;
                ORetorno.listaErros.Add("O membro já possui saldo no sistema!");

                return ORetorno;
                
            }                        
            
            MovimentoOperacaoDTO OTransacao = new MovimentoOperacaoDTO();
            
            OTransacao.nroContaOrigem = 1;
            OTransacao.nroContaDestino = OMembro.nroAssociado.toInt();
            OTransacao.valorOperacao = OConfiguracaoPromocao.valorPremioNovoMembro.toDecimal();
            OTransacao.flagIgnorarSenha = true;            
            
            OTransacao = this.CarregadorDados.carregar(OTransacao);
            
            ORetorno =  this.OTransferenciaFacade.transferir(OTransacao);
            
            return ORetorno;

        }

    }

} 
