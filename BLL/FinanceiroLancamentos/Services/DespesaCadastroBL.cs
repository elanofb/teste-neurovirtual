using System;
using System.Linq;
using DAL.Financeiro;
using BLL.Core.Events;
using BLL.FinanceiroLancamentos.Events;
using BLL.Pessoas;
using BLL.Services;
using DAL.Pessoas;

namespace BLL.FinanceiroLancamentos {

    public class DespesaCadastroBL : DefaultBL, IDespesaCadastroBL {

        //Atributos
        private IPessoaBL _PessoaBL;
        
        //Propriedades
        private IPessoaBL OPessoaBL => _PessoaBL = _PessoaBL ?? new PessoaBL();
        
        //Eventos
        private readonly EventAggregator onDespesaCadastrada = OnDespesaCadastrada.getInstance;

        /// <summary>
        /// 
        /// </summary>
        public bool salvar(TituloDespesa OTituloDespesa) {
            
            OTituloDespesa.codigoBoleto = OTituloDespesa.codigoBoleto.onlyNumber().abreviar(50);
            
            OTituloDespesa.nroDocumento = OTituloDespesa.nroDocumento.abreviar(50);
            
            OTituloDespesa.nroContrato = OTituloDespesa.nroContrato.abreviar(20);
            
            OTituloDespesa.descricao = OTituloDespesa.descricao.abreviar(500);
            
            OTituloDespesa.Pessoa = OPessoaBL.carregar(OTituloDespesa.idPessoa.toInt()) ?? new Pessoa();
            
            //Preencher dados credor
            OTituloDespesa.nomePessoaCredor = OTituloDespesa.Pessoa.nome.abreviar(100);
            OTituloDespesa.documentoPessoaCredor = OTituloDespesa.Pessoa.nroDocumento.abreviar(20);
            OTituloDespesa.nroTelPrincipalCredor = OTituloDespesa.Pessoa.nroTelPrincipal.abreviar(15);
            OTituloDespesa.nroTelSecundarioCredor = OTituloDespesa.Pessoa.nroTelSecundario.abreviar(15);
            OTituloDespesa.emailPrincipalCredor = OTituloDespesa.Pessoa.emailPrincipal.abreviar(50);
            
            OTituloDespesa.observacao = OTituloDespesa.observacao.abreviar(400);

            OTituloDespesa.Pessoa = null;
            OTituloDespesa.ContaBancaria = null;

            OTituloDespesa.codigoBoleto = OTituloDespesa.codigoBoleto.onlyNumber(); 
            
            OTituloDespesa.ContaBancariaFavorecida = null;
            
            OTituloDespesa.setDefaultInsertValues();

            if (OTituloDespesa.listaTituloDespesaPagamento.Any()) {

                foreach (var Item in OTituloDespesa.listaTituloDespesaPagamento.ToList()) {

                    Item.nroNotaFiscal = OTituloDespesa.nroNotaFiscal;
                    
                    Item.nroDocumento = OTituloDespesa.nroDocumento.abreviar(50);
                    
                    Item.nroContrato = OTituloDespesa.nroContrato.abreviar(20);
                    
                    Item.idContaBancariaFavorecida = OTituloDespesa.idContaBancariaFavorecida;
                    
                    Item.setDefaultInsertValues();

                }
            }

            db.TituloDespesa.Add(OTituloDespesa);

            db.SaveChanges();

            bool flagSucesso = OTituloDespesa.id > 0;

            if (flagSucesso) {
                this.onDespesaCadastrada.subscribe(new OnDespesaCadastradaHandler());
                this.onDespesaCadastrada.publish(OTituloDespesa as object);
            }

            return flagSucesso;
        }
    }
}