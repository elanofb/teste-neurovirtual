using BLL.Core.Events;
using BLL.Permissao;
using BLL.UsuariosInternos.Events;
using DAL.Permissao;

namespace BLL.UsuariosInternos {

    public class UsuarioInternoBL : UsuarioSistemaBL {
		
		// Eventos
        private EventAggregator onUsuarioInternoCadastrado = OnUsuarioInternoCadastrado.getInstance;

        //Construtor
        public UsuarioInternoBL() {

        }
     
        public override bool salvar(UsuarioSistema OUsuarioInterno) {

            var flagSucesso = base.salvar(OUsuarioInterno);

            // Após salvar, vincular a unidade que o usuário responsável está logado ao usuário cadastrado
            if (flagSucesso) {                

                //Registro e disparo do Evento
                this.onUsuarioInternoCadastrado.subscribe(new OnUsuarioInternoCadastradoHandler());
                this.onUsuarioInternoCadastrado.publish((OUsuarioInterno.id as object));

            }

            return flagSucesso;

        }

	}
}