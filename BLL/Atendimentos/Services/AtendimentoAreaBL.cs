using System;
using System.Json;
using System.Linq;
using BLL.Services;
using DAL.Atendimentos;
using DAL.Permissao.Security.Extensions;
using EntityFramework.Extensions;
using UTIL.Resources;

namespace BLL.Atendimentos {

	public class AtendimentoAreaBL : DefaultBL, IAtendimentoAreaBL {

		//
		public AtendimentoAreaBL() {
		}

		//
		public IQueryable<AtendimentoArea> query(int? idOrganizacaoParam = null) {
            
			var query = from Obj in db.AtendimentoArea
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
        public AtendimentoArea carregar(int id) {

            var query = this.query().condicoesSeguranca();

            return query.FirstOrDefault(x => x.id == id);
        }


		//
		public IQueryable<AtendimentoArea> listar(string valorBusca, bool? ativo, int? idOrganizacaoParam = null) {
            
            var query = from Obj in db.AtendimentoArea
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
            
			var query = from Obj in db.AtendimentoArea
						where Obj.descricao == descricao && Obj.id != id && Obj.flagExcluido == false
						select Obj;

            query = query.condicoesSeguranca();

			var OAtendimentoArea = query.Take(1).FirstOrDefault();

			return (OAtendimentoArea != null);
		}

        //
        public bool salvar(AtendimentoArea OAtendimentoArea) {

            if(OAtendimentoArea.id == 0) {
                return this.inserir(OAtendimentoArea);
            }

            return this.atualizar(OAtendimentoArea);
        }

        //
        private bool inserir(AtendimentoArea OAtendimentoArea) {

            OAtendimentoArea.setDefaultInsertValues();

            db.AtendimentoArea.Add(OAtendimentoArea);

            db.SaveChanges();

            return (OAtendimentoArea.id > 0);
        }

        //
        private bool atualizar(AtendimentoArea OAtendimentoArea) {

            OAtendimentoArea.setDefaultUpdateValues();
            
            AtendimentoArea dbAtendimentoArea = this.carregar(OAtendimentoArea.id);

            if (dbAtendimentoArea == null) {
                return false;
            }

            var dbEntry = db.Entry(dbAtendimentoArea);

            dbEntry.CurrentValues.SetValues(OAtendimentoArea);

            dbEntry.ignoreFields();

            db.SaveChanges();

            return (OAtendimentoArea.id > 0);
        }

        //
        public bool excluir(int id) {

            var Objeto = this.carregar(id);

            var idUsuario = User.id();

            db.AtendimentoArea
                .Where(x => x.id == Objeto.id)
                .Update(x => new AtendimentoArea {
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