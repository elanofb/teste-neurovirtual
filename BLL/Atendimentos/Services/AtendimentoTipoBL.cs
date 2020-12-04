using System;
using System.Json;
using System.Linq;
using BLL.Services;
using DAL.Atendimentos;
using DAL.Permissao.Security.Extensions;
using EntityFramework.Extensions;
using UTIL.Resources;

namespace BLL.Atendimentos {

	public class AtendimentoTipoBL : DefaultBL, IAtendimentoTipoBL {

		//
		public AtendimentoTipoBL() {
		}

	    //
	    public IQueryable<AtendimentoTipo> query(int? idOrganizacaoParam = null) {
	        
		    var query = from Obj in db.AtendimentoTipo
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
        public AtendimentoTipo carregar(int id) {

            var query = this.query().condicoesSeguranca();

            return query.FirstOrDefault(x => x.id == id);
        }


		//
		public IQueryable<AtendimentoTipo> listar(string valorBusca, bool? ativo, int? idOrganizacaoParam = null) {
            
            var query = from Obj in db.AtendimentoTipo
						where Obj.flagExcluido == false
						select Obj;
            
            if (!String.IsNullOrEmpty(valorBusca)) {
				query = query.Where(x => x.descricao.Contains(valorBusca));
			}

			if (ativo.HasValue) {
				query = query.Where(x => x.ativo == ativo);
			}

            if (idOrganizacaoParam == null) {
                idOrganizacaoParam = idOrganizacao;
            }
            
            if (idOrganizacaoParam > 0) {
                query = query.Where(x => x.idOrganizacao == idOrganizacaoParam);
            }

			return query;
		}

        //
		public bool existe(string descricao, int id) {
            
			var query = from Obj in db.AtendimentoTipo
						where Obj.descricao == descricao && Obj.id != id && Obj.flagExcluido == false
						select Obj;

            query = query.condicoesSeguranca();

			var OAtendimentoTipo = query.Take(1).FirstOrDefault();

			return (OAtendimentoTipo != null);
		}

        //
        public bool salvar(AtendimentoTipo OAtendimentoTipo) {

            if(OAtendimentoTipo.id == 0) {
                return this.inserir(OAtendimentoTipo);
            }

            return this.atualizar(OAtendimentoTipo);
        }

        //
        private bool inserir(AtendimentoTipo OAtendimentoTipo) {

            OAtendimentoTipo.setDefaultInsertValues();

            db.AtendimentoTipo.Add(OAtendimentoTipo);

            db.SaveChanges();

            return (OAtendimentoTipo.id > 0);
        }

        //
        private bool atualizar(AtendimentoTipo OAtendimentoTipo) {

            OAtendimentoTipo.setDefaultUpdateValues();
            
            AtendimentoTipo dbAtendimentoTipo = this.carregar(OAtendimentoTipo.id);

            if (dbAtendimentoTipo == null) {
                return false;
            }

            var dbEntry = db.Entry(dbAtendimentoTipo);

            dbEntry.CurrentValues.SetValues(OAtendimentoTipo);

            dbEntry.ignoreFields();

            db.SaveChanges();

            return (OAtendimentoTipo.id > 0);
        }

        //
        public bool excluir(int id) {

            var Objeto = this.carregar(id);

            var idUsuario = User.id();

            db.AtendimentoTipo
                .Where(x => x.id == Objeto.id)
                .Update(x => new AtendimentoTipo {
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