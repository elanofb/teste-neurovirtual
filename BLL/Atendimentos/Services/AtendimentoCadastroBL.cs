using System;
using System.Json;
using BLL.Core.Events;
using DAL.Atendimentos;
using UTIL.Resources;

namespace BLL.Atendimentos {

	public class AtendimentoCadastroBL : AtendimentoConsultaBL, IAtendimentoCadastroBL {

        // Events
	    private readonly EventAggregator onAtendimentoCadastrado = OnAtendimentoCadastrado.getInstance;

        //
        public AtendimentoCadastroBL() {
		}

        //
        public bool salvar(Atendimento OAtendimento) {

            if(OAtendimento.id == 0) {
                return this.inserir(OAtendimento);
            }

            return this.atualizar(OAtendimento);
        }

        //
        private bool inserir(Atendimento OAtendimento) {

            OAtendimento.idStatusAtendimento = AtendimentoStatusConst.EM_ABERTO;

            OAtendimento.setDefaultInsertValues();

            db.Atendimento.Add(OAtendimento);

            db.SaveChanges();

            var flagSucesso = OAtendimento.id > 0;

            if (flagSucesso) { 

                this.onAtendimentoCadastrado.subscribe(new OnAtendimentoCadastradoHandler());

                this.onAtendimentoCadastrado.publish((object) OAtendimento);
                    
            }

            return flagSucesso;
        }

        //
        private bool atualizar(Atendimento OAtendimento) {

            OAtendimento.setDefaultUpdateValues();
            
            Atendimento dbAtendimento = this.carregar(OAtendimento.id);

            if (dbAtendimento == null) {
                return false;
            }

            var dbEntry = db.Entry(dbAtendimento);

            dbEntry.CurrentValues.SetValues(OAtendimento);

            dbEntry.ignoreFields();

            db.SaveChanges();

            return (OAtendimento.id > 0);
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