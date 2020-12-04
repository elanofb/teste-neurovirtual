using System;
using System.Linq;
using BLL.Historicos.Interfaces;
using BLL.Services;
using DAL.Historicos;
using DAL.Permissao.Security.Extensions;
using EntityFramework.Extensions;

namespace BLL.Historicos.Services {
    
    public class HistoricoAtualizacaoFinalizacaoBL : DefaultBL, IHistoricoAtualizacaoFinalizacaoBL {
        
        public bool finalizarAnalise(int[] ids, bool flagAprovado) {
            
            db.HistoricoAtualizacao.condicoesSeguranca()
                .Where(x => ids.Contains(x.id))
                .Update(x => new HistoricoAtualizacao {
                    dtAnalise = DateTime.Now,
                    idUsuarioAnalise  = User.id(),
                    flagAprovado =  flagAprovado
                });

            return true;
            
        }
    }
}