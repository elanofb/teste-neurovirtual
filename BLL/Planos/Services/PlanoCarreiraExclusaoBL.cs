using System;
using System.Json;
using System.Linq;
using BLL.Services;
using DAL.Permissao.Security.Extensions;
using DAL.Planos;
using EntityFramework.Extensions;

namespace BLL.Planos {

    public class PlanoCarreiraExclusaoBL : DefaultBL, IPlanoCarreiraExclusaoBL {
        
        //
        public JsonMessage excluir(int[] ids) {
		    
            var ORetorno = new JsonMessage();
		    
            int idUsuarioLogado = User.id();
            
            db.PlanoCarreira.condicoesSeguranca()
                .Where(x => ids.Contains(x.id))
                .Update(x => new PlanoCarreira { dtExclusao = DateTime.Now, idUsuarioExclusao = idUsuarioLogado });
            
            ORetorno.error = false;
            
            ORetorno.message = "O(s) plano(s) foi(ram) removido(s) com sucesso.";
		    
            return ORetorno;
        }

    }
}