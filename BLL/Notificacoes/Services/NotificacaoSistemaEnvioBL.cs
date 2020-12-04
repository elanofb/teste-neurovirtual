using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using BLL.Core.Events;
using BLL.Notificacoes.Events;
using BLL.Services;
using DAL.Notificacoes;
using DAL.Permissao.Security.Extensions;

namespace BLL.Notificacoes {

    public class NotificacaoSistemaEnvioBL : DefaultBL, INotificacaoSistemaEnvioBL {

        // Atributos

        // Propriedades

        // Eventos
        private readonly EventAggregator onNotificacaoCadastrada = OnNotificacaoCadastrada.getInstance;

        public NotificacaoSistemaEnvio carregar(int id) {
            var query = from NP in db.NotificacaoSistemaEnvio.Include(x => x.NotificacaoSistema)
                        where NP.id == id && NP.flagExcluido == false
                        select NP;

            query = query.condicoesSeguranca();
            
            return query.FirstOrDefault();
        }
        
        public IQueryable<NotificacaoSistemaEnvio> listar(int idReferencia, int idNotificacao) {

            var query = from NP in db.NotificacaoSistemaEnvio
                                    .Include(x => x.NotificacaoSistema)
                        where 
                            //NP.flagExcluido == false && 
                            (NP.idReferencia > 0) && 
                            NP.NotificacaoSistema.flagExcluido == false
                        select NP;

            query = query.condicoesSeguranca();

            if (idReferencia > 0) { 
                query = query.Where(x => x.idReferencia == idReferencia);
            }

            if (idNotificacao > 0) { 
                query = query.Where(x => x.idNotificacao == idNotificacao);
            }

            return query;

        }

        public IQueryable<NotificacaoSistemaEnvio> listar(int idTarefa, bool? flagEnviado) {

			var query = from Cob in db.NotificacaoSistemaEnvio.Include(x => x.NotificacaoSistema)
						select Cob;

            query = query.condicoesSeguranca();

			if (idTarefa > 0) { 
				query = query.Where(x => x.idTarefa == idTarefa);
			}

			if (flagEnviado != null) { 
				query = query.Where(x => x.flagEnvioEmail == flagEnviado);
			}

			return query;
		}

        /// <summary>
        /// Salvar o registro
        /// </summary>
        public bool salvar(NotificacaoSistemaEnvio oNotificacaoSistemaEnvio) {

            oNotificacaoSistemaEnvio.NotificacaoSistema = null;

            var flagSucesso = false;

            if (oNotificacaoSistemaEnvio.id > 0) {
                flagSucesso = this.atualizar(oNotificacaoSistemaEnvio);
            }

            if (oNotificacaoSistemaEnvio.id == 0) {
                flagSucesso = this.inserir(oNotificacaoSistemaEnvio);
            }
            
            if (flagSucesso) { 

                var listaParametros = new List<object>();
                listaParametros.Add(new List<NotificacaoSistemaEnvio> { oNotificacaoSistemaEnvio });
                listaParametros.Add(new NotificacaoSistema { id = oNotificacaoSistemaEnvio.idNotificacao });

                this.onNotificacaoCadastrada.subscribe(new OnNotificacaoCadastradaHandler());
                this.onNotificacaoCadastrada.publish((listaParametros as object));
            }

            return flagSucesso;
        }

        /// <summary>
        /// Persistir o objecto e salvar na base de dados
        /// </summary>
        private bool inserir(NotificacaoSistemaEnvio oNotificacaoSistemaEnvio) {

            oNotificacaoSistemaEnvio.setDefaultInsertValues();

            db.NotificacaoSistemaEnvio.Add(oNotificacaoSistemaEnvio);

            db.SaveChanges();

            return (oNotificacaoSistemaEnvio.id > 0);
        }

        /// <summary>
        /// Persistir o objecto e atualizar informações
        /// </summary>
        private bool atualizar(NotificacaoSistemaEnvio oNotificacaoSistemaEnvio) {
            
            var dbNotificacaoSistemaEnvio = this.carregar(oNotificacaoSistemaEnvio.id);

            if (dbNotificacaoSistemaEnvio == null) {
                return false;
            }

            var tipoEntry = db.Entry(dbNotificacaoSistemaEnvio);

            oNotificacaoSistemaEnvio.setDefaultUpdateValues();

            tipoEntry.CurrentValues.SetValues(oNotificacaoSistemaEnvio);

            tipoEntry.ignoreFields();

            db.SaveChanges();
            return (oNotificacaoSistemaEnvio.id > 0);
        }

        /// <summary>
        /// Excluir o registro
        /// </summary>
        public UtilRetorno excluir(int id) {

			NotificacaoSistemaEnvio oNotificacaoSistemaEnvio = this.carregar(id);

			if (oNotificacaoSistemaEnvio == null) {
				return UtilRetorno.newInstance(true, "O registro não foi localizado.");
			}

			oNotificacaoSistemaEnvio.flagExcluido = true;

            oNotificacaoSistemaEnvio.motivoExclusao = "Exclusão manual.";

            oNotificacaoSistemaEnvio.idUsuarioAlteracao = User.id();

            oNotificacaoSistemaEnvio.dtAlteracao = DateTime.Now;

            db.SaveChanges();

			return UtilRetorno.newInstance(false, "O registro foi removido com sucesso.");
        }

    }

}
