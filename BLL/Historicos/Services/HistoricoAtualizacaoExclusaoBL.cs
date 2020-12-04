using System;
using System.Linq;
using BLL.Historicos.Interfaces;
using BLL.Services;
using DAL.Historicos;
using DAL.Permissao.Security.Extensions;
using EntityFramework.Extensions;

namespace BLL.Historicos.Services {
    
    public class HistoricoAtualizacaoExclusaoBL : DefaultBL, IHistoricoAtualizacaoExclusaoBL {
        
        public bool excluir(int[] ids) {

            db.HistoricoAtualizacao.condicoesSeguranca()
                .Where(x => ids.Contains(x.id))
                .Update(x => new HistoricoAtualizacao {
                    dtExclusao = DateTime.Now,
                    idUsuarioExclusao = User.id()
                });

            return true;
        }
    }
}