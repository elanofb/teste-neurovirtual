using System;
using System.Linq;
using DAL.Atendimentos;
using DAL.Permissao.Security.Extensions;
using EntityFramework.Extensions;

namespace BLL.Atendimentos {

	public class AtendimentoExclusaoBL : AtendimentoConsultaBL, IAtendimentoExclusaoBL {

        //
        public AtendimentoExclusaoBL() {
		}

        //
        public bool excluir(int id) {

            var Objeto = this.carregar(id);

            var idUsuario = User.id();

            db.Atendimento
                .Where(x => x.id == Objeto.id)
                .Update(x => new Atendimento {
                    flagExcluido = true,
                    dtAlteracao = DateTime.Now,
                    idUsuarioAlteracao = idUsuario
                });

            return true;
        }

    }
}