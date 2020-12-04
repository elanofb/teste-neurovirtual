using System;
using BLL.Core.Events;
using BLL.UsuariosUnidades;
using DAL.Permissao.Security.Extensions;

namespace BLL.UsuariosInternos.Events {

    public class OnUsuarioInternoCadastradoHandler : IHandler<object> {

        //Atributos
        private IUsuarioUnidadeBL _UsuarioUnidadeBL;

        //Propriedades
        private IUsuarioUnidadeBL OUsuarioUnidadeBL => this._UsuarioUnidadeBL = this._UsuarioUnidadeBL ?? new UsuarioUnidadeBL();

        private int idUsuarioInterno { get; set; }

		//Executar ações para o evento
		public void execute(object source) {

            this.idUsuarioInterno = UtilNumber.toInt32(source);

		    if (idUsuarioInterno == 0) {
		        throw new InvalidOperationException("O ID do usuário não foi informado.");
		    }

            this.vincularUnidade();

		}

        // Vincular a unidade que o usuário responsável está logado ao usuário cadastrado
        private void vincularUnidade() {
            
            var idUnidadeLogada = HttpContextFactory.Current.User.idUnidade();    

            // Se o usuário não estiver logado em nenhuma unidade, não realizar vínculo
            if (idUnidadeLogada == 0) {
                return;
            }

            this.OUsuarioUnidadeBL.salvar(this.idUsuarioInterno, idUnidadeLogada);

        }
        

	}
}