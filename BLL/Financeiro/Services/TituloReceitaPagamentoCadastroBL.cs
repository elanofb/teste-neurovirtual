using System;
using System.Linq;
using System.Linq.Expressions;
using DAL.Financeiro;
using DAL.Repository.Base;
using EntityFramework.Extensions;

namespace BLL.Financeiro {

    public class TituloReceitaPagamentoCadastroBL : ITituloReceitaPagamentoCadastroBL {

        //Atributos

        //Servicos
        public DataContext db { get; }

        /// <summary>
        /// Construtor
        /// </summary>
        public TituloReceitaPagamentoCadastroBL(DataContext _db) {

            this.db = _db;
        }
        
        //Definir se é um insert ou update e enviar o registro para o banco de dados 
        

        public TituloReceitaPagamento salvar(TituloReceitaPagamento OTituloReceitaPagamento) {

            OTituloReceitaPagamento.TituloReceita = null;

            OTituloReceitaPagamento.MeioPagamento = null;

            OTituloReceitaPagamento.FormaPagamento = null;
            
            OTituloReceitaPagamento.Categoria = null;
            
            OTituloReceitaPagamento.CentroCusto = null;
            
            OTituloReceitaPagamento.MacroConta = null;
            
            OTituloReceitaPagamento.ContaBancaria = null;
            
            OTituloReceitaPagamento.CidadeRecibo = null;
            
            OTituloReceitaPagamento.CupomDesconto = null;
            
            OTituloReceitaPagamento.GatewayPagamento = null;
            
            OTituloReceitaPagamento.Organizacao = null;
            
            OTituloReceitaPagamento.StatusPagamento = null;
            
            OTituloReceitaPagamento.UsuarioBaixa = null;
            
            OTituloReceitaPagamento.UsuarioExclusao = null;
            
            OTituloReceitaPagamento.ativo = true;
          

            if (OTituloReceitaPagamento.id == 0) {
                return this.inserir(OTituloReceitaPagamento);
            }

            return this.atualizar(OTituloReceitaPagamento);
        }

        //Persistir e inserir um novo registro 
        public TituloReceitaPagamento inserir(TituloReceitaPagamento OTituloReceitaPagamento) {

            OTituloReceitaPagamento.setDefaultInsertValues();

            db.Configuration.AutoDetectChangesEnabled = false;

            db.Configuration.ValidateOnSaveEnabled = false;

            db.TituloReceitaPagamento.Add(OTituloReceitaPagamento);

            db.SaveChanges();

            return OTituloReceitaPagamento;
        }

        //Persistir e atualizar um registro existente 
        public TituloReceitaPagamento atualizar(TituloReceitaPagamento OTituloReceitaPagamento) {

            //Localizar existentes no banco
            //Nao aplicado condicoes de seguranca pois a atualização é usada no checkout 
            TituloReceitaPagamento OPagamento = this.db.TituloReceitaPagamento.Find(OTituloReceitaPagamento.id);

            //Configurar valores padrão
            OTituloReceitaPagamento.setDefaultUpdateValues();

            //Atualizacao do pagamento
            var PagamentoEntry = db.Entry(OPagamento);
            PagamentoEntry.CurrentValues.SetValues(OTituloReceitaPagamento);
            PagamentoEntry.ignoreFields();

            db.SaveChanges();

            return OTituloReceitaPagamento;
        }

        /// <summary>
        /// 
        /// </summary>
        public UtilRetorno atualizarDadosPagamento(TituloReceitaPagamento OPagamento) {

            var vTarifasBancarias = OPagamento.valorTarifasBancarias;
            
            var vTarifasTransacao = OPagamento.valorTarifasTransacao;
                                        
            if (OPagamento.idMeioPagamento == MeioPagamentoConst.DINHEIRO || OPagamento.idMeioPagamento == MeioPagamentoConst.DEPOSITO_BANCARIO || OPagamento.idMeioPagamento == MeioPagamentoConst.TRANSFERENCIA_ELETRONICA) {
		        
                vTarifasBancarias = 0;
		        
                vTarifasTransacao = 0;
            }

            if (OPagamento.valorRecebido.toDecimal() <= 0) {

                OPagamento.valorRecebido = OPagamento.valorOriginal;
            }

            OPagamento.valorRecebido = decimal.Add(OPagamento.valorRecebido.toDecimal(), OPagamento.valorJuros.toDecimal());

            OPagamento.idStatusPagamento = StatusPagamentoConst.PAGO;
            
            OPagamento.limparDados();

            this.db.TituloReceitaPagamento
                .Where(x => x.id == OPagamento.id)
                .Update(x => new TituloReceitaPagamento {
                                                            valorTarifasTransacao = vTarifasTransacao,
                                                            valorTarifasBancarias = vTarifasBancarias,

                                                            valorRecebido = OPagamento.valorRecebido,
                                                            valorJuros = OPagamento.valorJuros,
                                                            
                                                            dtPagamento = OPagamento.dtPagamento,
                                                            dtPrevisaoCredito = OPagamento.dtPrevisaoCredito ?? x.dtPrevisaoCredito,
                                                            dtCredito = OPagamento.dtCredito ?? x.dtCredito,
                                                            
                                                            idStatusPagamento = OPagamento.idStatusPagamento,
                                                            idMeioPagamento = OPagamento.idMeioPagamento > 0? OPagamento.idMeioPagamento : x.idMeioPagamento,
                                                            idFormaPagamento = OPagamento.idFormaPagamento > 0? OPagamento.idFormaPagamento : x.idFormaPagamento,
                                                            
                                                            codigoAutorizacao = string.IsNullOrEmpty(OPagamento.codigoAutorizacao)? x.codigoAutorizacao : OPagamento.codigoAutorizacao,
                                                            tid = string.IsNullOrEmpty(OPagamento.tid)? x.tid : OPagamento.tid,
                                                            nroBanco = string.IsNullOrEmpty(OPagamento.nroBanco)? x.nroBanco : OPagamento.nroBanco,
                                                            nroDocumento = string.IsNullOrEmpty(OPagamento.nroDocumento)? x.nroDocumento : OPagamento.nroDocumento,
                                                            nroAgencia = string.IsNullOrEmpty(OPagamento.nroAgencia)? x.nroAgencia : OPagamento.nroAgencia,
                                                            nroDigitoAgencia = string.IsNullOrEmpty(OPagamento.nroDigitoAgencia)? x.nroDigitoAgencia : OPagamento.nroDigitoAgencia,
                                                            nroConta = string.IsNullOrEmpty(OPagamento.nroConta)? x.nroConta : OPagamento.nroConta,
                                                            nroDigitoConta = string.IsNullOrEmpty(OPagamento.nroDigitoConta)? x.nroDigitoConta : OPagamento.nroDigitoConta,
                                                            
                                                            idUsuarioBaixa = OPagamento.idUsuarioBaixa,
                                                            dtBaixa = OPagamento.dtBaixa,
                                                            idUsuarioAlteracao = OPagamento.idUsuarioAlteracao,
                                                            flagBaixaAutomatica = OPagamento.flagBaixaAutomatica,
                                                            dtExclusao = null
                                                        });

            return UtilRetorno.newInstance(false);
        }
        
        /// <summary>
        /// 
        /// </summary>
        public void atualizar(int id, Expression<Func<TituloReceitaPagamento, TituloReceitaPagamento>> updateExpression) {

            db.TituloReceitaPagamento.Where(x => x.id == id).Update(updateExpression);

        }          
        
        /// <summary>
        /// 
        /// </summary>
        public void atualizar(int[] ids, Expression<Func<TituloReceitaPagamento, TituloReceitaPagamento>> updateExpression) {

            db.TituloReceitaPagamento.Where(x => ids.Contains(x.id)).Update(updateExpression);

        }    
    }
}
