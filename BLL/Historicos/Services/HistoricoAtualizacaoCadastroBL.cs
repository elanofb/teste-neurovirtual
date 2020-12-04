using System;
using BLL.Historicos.Interfaces;
using DAL.Historicos;

namespace BLL.Historicos.Services {
    
    public class HistoricoAtualizacaoCadastroBL : HistoricoAtualizacaoConsultaBL, IHistoricoAtualizacaoCadastroBL{
        
        public bool salvar(HistoricoAtualizacao OHistoricoAtualizacao){

            OHistoricoAtualizacao.Associado = null;
            OHistoricoAtualizacao.NaoAssociado = null;
            OHistoricoAtualizacao.Pessoa = null;            
                        
            OHistoricoAtualizacao.emailOrigem = OHistoricoAtualizacao.emailOrigem.abreviar(100);            
            OHistoricoAtualizacao.browser = OHistoricoAtualizacao.browser.abreviar(100);
            
            if (OHistoricoAtualizacao.id == 0) {
                return this.inserir(OHistoricoAtualizacao);
            }

            return this.atualizar(OHistoricoAtualizacao);
        }
        
        private bool inserir(HistoricoAtualizacao OHistoricoAtualizacao) {
        
            OHistoricoAtualizacao.setDefaultInsertValues();            
            db.HistoricoAtualizacao.Add(OHistoricoAtualizacao);
            db.SaveChanges();
            
            return (OHistoricoAtualizacao.id > 0);
        }

        private bool atualizar(HistoricoAtualizacao OHistoricoAtualizacao) {

            OHistoricoAtualizacao.setDefaultUpdateValues();
            var dbHistoricoAtualizacao = this.carregar(OHistoricoAtualizacao.id);
            
            if (dbHistoricoAtualizacao == null) {
                return false;
            }
            
            var ItemEntry = db.Entry(dbHistoricoAtualizacao);
            ItemEntry.CurrentValues.SetValues(OHistoricoAtualizacao);
            ItemEntry.ignoreFields();
                
            db.SaveChanges();
            
            return (OHistoricoAtualizacao.id > 0);
        }
                
                
    }
}