using System;
using BLL.Associados;
using BLL.NaoAssociados;
using DAL.PedidosTemp;
using DAL.Pessoas;
using FluentValidation.Attributes;
using WEB.Areas.Associados.Extensions;

namespace WEB.Areas.Pedidos.ViewModels{
    
    [Validator(typeof(PedidoCadastroFormValidator))]
    public class PedidoCadastroForm {

        // Atributos Serviço
        private IAssociadoBL _AssociadoBL;
        private INaoAssociadoBL _NaoAssociadoBL;

        // Propriedades Serviço
        private IAssociadoBL OAssociadoBL => this._AssociadoBL = this._AssociadoBL ?? new AssociadoBL();
        private INaoAssociadoBL ONaoAssociadoBL => this._NaoAssociadoBL = this._NaoAssociadoBL ?? new NaoAssociadoBL();

        //Propriedades
        public PedidoTemp Pedido { get; set;}

        public DadosCompradorDTO DadosComprador { get; set; }
        
		//Construtor
		public PedidoCadastroForm() {
            
		}
        
        //
        public void carregarDadosComprador(int idPessoa) {
            
            var DadosMembro = this.OAssociadoBL.carregarAssociadoPessoa(idPessoa);

            var perfilAssociado = "Membro";

            if (DadosMembro == null) {

                DadosMembro = this.ONaoAssociadoBL.carregarPorPessoa(idPessoa);

                perfilAssociado = "Comerciante";
            }
            
            this.DadosComprador = new DadosCompradorDTO() {
                
                nome = DadosMembro.Pessoa.nome,
                nroDocumento = UtilString.formatCPFCNPJ(DadosMembro.Pessoa.nroDocumento),
                telPrincipal = DadosMembro.Pessoa.formatarTelPrincipal(),
                telSecundario = DadosMembro.Pessoa.formatarTelSecundario(),
                emailPrincipal = DadosMembro.Pessoa.emailPrincipal(),
                emailSecundario = DadosMembro.Pessoa.emailSecundario(),
                ativo = DadosMembro.ativo,
                descricaoStatus = DadosMembro.exibirStatus(),
                descricaoSituacao = "",
                tipo = perfilAssociado

            };

        }

        // 
        public PedidoTemp prencherDadosFinanceiros(PedidoTemp OPedidoTemp) {           

            OPedidoTemp.flagFaturamentoCadastro = this.Pedido.flagFaturamentoCadastro;

            OPedidoTemp.idCentroCusto = this.Pedido.idCentroCusto;

            OPedidoTemp.idMacroConta = this.Pedido.idMacroConta;

            OPedidoTemp.idCategoriaTitulo = this.Pedido.idCategoriaTitulo;

            OPedidoTemp.idContaBancaria = this.Pedido.idContaBancaria;

            OPedidoTemp.codigoContabil = this.Pedido.codigoContabil;

            OPedidoTemp.dtVencimento = this.Pedido.dtVencimento;
            
            OPedidoTemp.flagBoletoBancarioPermitido = this.Pedido.flagBoletoBancarioPermitido;

            OPedidoTemp.flagCartaoCreditoPermitido = this.Pedido.flagCartaoCreditoPermitido;

            OPedidoTemp.flagDepositoPermitido = this.Pedido.flagDepositoPermitido;

            return OPedidoTemp;

        }

        // 
        public PedidoTemp prencherDadosDataEntrega(PedidoTemp OPedidoTemp){

            OPedidoTemp.flagPagamentoNaEntrega = this.Pedido.flagPagamentoNaEntrega;

            OPedidoTemp.dtAgendamentoEntrega = this.Pedido.dtAgendamentoEntrega;

            OPedidoTemp.flagPeriodo = this.Pedido.flagPeriodo;

            return OPedidoTemp;

        }

    }

}