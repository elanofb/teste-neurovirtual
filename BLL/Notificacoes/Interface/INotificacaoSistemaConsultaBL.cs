using System;
using System.Linq;
using System.Linq.Expressions;
using DAL.Notificacoes;

namespace BLL.Notificacoes{
    
    public interface INotificacaoSistemaConsultaBL{
        
        /// <summary>
        /// Carregar registro pelo ID
        /// </summary>
        NotificacaoSistema carregar(int id);

        /// <summary>
        /// Montagem de base de consulta de registros 
        /// </summary>
        IQueryable<NotificacaoSistema> query(int idOrganizacaoParam, bool? ativo = true);
        
        
        /// <summary>
        /// Listagem de notifiacoes de acordo com parametros
        /// </summary>
        IQueryable<NotificacaoSistema> listar(string valorBusca, bool? ativo = true);

        /// <summary>
        /// 
        /// </summary>
        IQueryable<object> queryNotificacoesAbertas(int idOrganizacao, Expression<Func<NotificacaoSistema, object>> selectColumn);
    }
}