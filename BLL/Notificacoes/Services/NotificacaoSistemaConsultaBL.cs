using System;
using System.Linq;
using System.Linq.Expressions;
using BLL.Services;
using DAL.Notificacoes;
using DAL.Repository.Base;

namespace BLL.Notificacoes {

    public class NotificacaoSistemaConsultaBL : INotificacaoSistemaConsultaBL {

        // Atributos

        //Servicos
        private DataContext db { get; }

        /// <summary>
        /// Construtor
        /// </summary>
        public NotificacaoSistemaConsultaBL(DataContext _db) {

            db = _db;
        }

        /// <summary>
        /// Carregar registro pelo ID
        /// </summary>
        public NotificacaoSistema carregar(int id) {

            var query = from NS in db.NotificacaoSistema
                        where
                            NS.id == id &&
                            NS.flagExcluido == false
                        select NS;

            query = query.condicoesSeguranca();

            NotificacaoSistema ONotificacaoSistema = query.FirstOrDefault();

            return ONotificacaoSistema;
        }


        /// <summary>
        /// Montagem de consulta conforme necessidade
        /// </summary>
        public IQueryable<NotificacaoSistema> query(int idOrganizacaoParam, bool? ativo = true) {

            var query = from NS in db.NotificacaoSistema
                        where NS.flagExcluido == false
                        select NS;


            if (idOrganizacaoParam > 0) {
                query = query.Where(x => x.idOrganizacao  == idOrganizacaoParam);
            }


            if (ativo != null) {
                query = query.Where(x => x.ativo == ativo);
            }

            return query;
        }


        /// <summary>
        /// 
        /// </summary>
        public IQueryable<NotificacaoSistema> listar(string valorBusca, bool? ativo) {

            var baseQuery = this.query(0).condicoesSeguranca();

            if (ativo.HasValue) {
                
                baseQuery = baseQuery.Where(x => x.ativo == ativo);
                
            }

            if (!valorBusca.isEmpty()) {
                
                baseQuery = baseQuery.Where(x => x.titulo.Contains(valorBusca) || x.tituloPush.Contains(valorBusca));
            }
            
            
            return baseQuery;
        }
        
        /// <summary>
        /// Base query para captar notificacoes nao encerradas
        /// </summary>
        public IQueryable<object> queryNotificacoesAbertas(int idOrganizacaoParam, Expression<Func<NotificacaoSistema, object>> selectCampos) {


            var baseQuery = this.query(idOrganizacaoParam)
                                .Where(x => x.dtFinalizacaoEnvioEmail == null)
                                .Select( selectCampos );
            
            return baseQuery;
        }

    }
}
