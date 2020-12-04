using System;
using System.Json;
using System.Linq;
using BLL.Services;
using DAL.Atendimentos;
using DAL.Permissao.Security.Extensions;
using EntityFramework.Extensions;
using UTIL.Resources;

namespace BLL.Atendimentos {

	public class AtendimentoHistoricoBL : DefaultBL, IAtendimentoHistoricoBL {

		//
		public AtendimentoHistoricoBL() {
		    
		}
	    
	    //
	    public IQueryable<AtendimentoHistorico> query(int? idOrganizacaoParam = null) {
	        
	        var query = from Obj in db.AtendimentoHistorico
                        where Obj.flagExcluido == false
                        select Obj;

	        if (idOrganizacaoParam == null) {
	            idOrganizacaoParam = idOrganizacao;
	        }

	        if (idOrganizacaoParam > 0) {
		        query = query.Where(x => x.idOrganizacao == idOrganizacaoParam);
	        }

	        return query;

	    }

	    //
        public AtendimentoHistorico carregar(int id) {

            var query = from Obj in db.AtendimentoHistorico
						where Obj.id == id && Obj.flagExcluido == false
						select Obj;

            query = query.condicoesSeguranca();

            return query.FirstOrDefault();
        }


		//
		public IQueryable<AtendimentoHistorico> listar(int idAtendimento) {
            
            var query = from Obj in db.AtendimentoHistorico
						where Obj.flagExcluido == false
						select Obj;

		    query = query.condicoesSeguranca();

            if (idAtendimento > 0) {
                query = query.Where(x => x.idAtendimento == idAtendimento);
            }
            
			return query;
		}
        
        //
        public bool salvar(AtendimentoHistorico OAtendimentoHistorico) {

            if(OAtendimentoHistorico.id == 0) {
                return this.inserir(OAtendimentoHistorico);
            }

            return this.atualizar(OAtendimentoHistorico);
        }

        //
        private bool inserir(AtendimentoHistorico OAtendimentoHistorico) {

            OAtendimentoHistorico.setDefaultInsertValues();

            db.AtendimentoHistorico.Add(OAtendimentoHistorico);

            db.SaveChanges();

            return (OAtendimentoHistorico.id > 0);
        }

        //
        private bool atualizar(AtendimentoHistorico OAtendimentoHistorico) {

            OAtendimentoHistorico.setDefaultUpdateValues();
            
            AtendimentoHistorico dbAtendimentoHistorico = this.carregar(OAtendimentoHistorico.id);

            if (dbAtendimentoHistorico == null) {
                return false;
            }

            var dbEntry = db.Entry(dbAtendimentoHistorico);

            dbEntry.CurrentValues.SetValues(OAtendimentoHistorico);

            dbEntry.ignoreFields(new [] { "idAtendimento" });

            db.SaveChanges();

            return (OAtendimentoHistorico.id > 0);
        }

        //
        public bool excluir(int id) {

            var Objeto = this.carregar(id);

            var idUsuario = User.id();

            db.AtendimentoHistorico
                .Where(x => x.id == Objeto.id)
                .Update(x => new AtendimentoHistorico {
                    flagExcluido = true,
                    dtAlteracao = DateTime.Now,
                    idUsuarioAlteracao = idUsuario
                });

            return true;
        }

        public JsonMessageStatus alterarStatus(int id) {

            var retorno = new JsonMessageStatus();

            var Objeto = this.carregar(id);

            if (Objeto == null) {

                retorno.error = true;
                retorno.message = NotificationMessages.invalid_register_id;
            } else {

                Objeto.ativo = Objeto.ativo != true;
                db.SaveChanges();

                retorno.active = Objeto.ativo == true ? "S" : "N";
                retorno.message = "Os dados foram alterados com sucesso.";
            }
            return retorno;
        }
    }
}