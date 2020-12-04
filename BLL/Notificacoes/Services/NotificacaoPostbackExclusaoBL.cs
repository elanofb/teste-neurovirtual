using System;
using System.Json;
using System.Linq;
using BLL.Services;
using DAL.Notificacoes;
using DAL.Permissao.Security.Extensions;
using EntityFramework.Extensions;

namespace BLL.Notificacoes {

    public class NotificacaoPostbackExclusaoBL : DefaultBL, INotificacaoPostbackExclusaoBL {

	    //
	    public JsonMessage excluir(int[] ids) {
		    
		    var ORetorno = new JsonMessage();
		    
		    int idUsuarioLogado = User.id();

		    db.NotificacaoPostback.condicoesSeguranca().Where(x => ids.Contains(x.id))
			  .Update(x => new NotificacaoPostback {
				    
				  dtExclusao = DateTime.Now,
				    
				  idUsuarioExclusao = idUsuarioLogado
				    
			  });

		    ORetorno.error = false;

		    ORetorno.message = "O(s) Postback(s) foi(ram) removido(s) com sucesso.";
		    
		    return ORetorno;
	    }

    }
    
}