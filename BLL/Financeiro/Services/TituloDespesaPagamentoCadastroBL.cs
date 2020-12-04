using System;
using System.Linq;
using System.Linq.Expressions;
using BLL.Services;
using DAL.Financeiro;
using DAL.Repository.Base;
using EntityFramework.Extensions;

namespace BLL.Financeiro {

    public class TituloDespesaPagamentoCadastroBL : DefaultBL, ITituloDespesaPagamentoCadastroBL {

        //Definir se é um insert ou update e enviar o registro para o banco de dados 
        public TituloDespesaPagamento salvar(TituloDespesaPagamento OTituloDespesaPagamento) {

            OTituloDespesaPagamento.TituloDespesa = null;

            OTituloDespesaPagamento.MeioPagamento = null;

            OTituloDespesaPagamento.FormaPagamento = null;

            OTituloDespesaPagamento.ativo = true;

            if (OTituloDespesaPagamento.id == 0) {
                return this.inserir(OTituloDespesaPagamento);
            }

            return this.atualizar(OTituloDespesaPagamento);
        }

        //Persistir e inserir um novo registro 
        private TituloDespesaPagamento inserir(TituloDespesaPagamento OTituloDespesaPagamento) {

            OTituloDespesaPagamento.setDefaultInsertValues();

            using (var dataContext = new DataContext()) {

                dataContext.Configuration.AutoDetectChangesEnabled = false;

                dataContext.Configuration.ValidateOnSaveEnabled = false;

                dataContext.TituloDespesaPagamento.Add(OTituloDespesaPagamento);

                dataContext.SaveChanges();

            }

            return OTituloDespesaPagamento;
        }
        
        /// <summary>
        /// Persistir e atualizar um registro existente
        /// </summary>
        private TituloDespesaPagamento atualizar(TituloDespesaPagamento OTituloDespesaPagamento) {

            //Localizar existentes no banco
            //Nao aplicado condicoes de seguranca pois a atualização é usada no checkout 
            TituloDespesaPagamento dbPagamento = this.db.TituloDespesaPagamento.Find(OTituloDespesaPagamento.id);

            //Configurar valores padrão
            OTituloDespesaPagamento.setDefaultUpdateValues();

            //Atualizacao do pagamento
            var PagamentoEntry = db.Entry(dbPagamento);
            PagamentoEntry.CurrentValues.SetValues(OTituloDespesaPagamento);
            PagamentoEntry.ignoreFields();

            db.SaveChanges();

            return OTituloDespesaPagamento;
        }
        
        /// <summary>
        /// 
        /// </summary>
        public void atualizar(int id, Expression<Func<TituloDespesaPagamento, TituloDespesaPagamento>> updateExpression) {

            db.TituloDespesaPagamento.Where(x => x.id == id).Update(updateExpression);

        }          
        
        /// <summary>
        /// 
        /// </summary>
        public void atualizar(int[] ids, Expression<Func<TituloDespesaPagamento, TituloDespesaPagamento>> updateExpression) {

            db.TituloDespesaPagamento.Where(x => ids.Contains(x.id)).Update(updateExpression);

        }          
    }
}