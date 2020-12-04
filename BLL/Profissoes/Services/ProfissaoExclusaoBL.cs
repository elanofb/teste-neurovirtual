using System;
using System.Linq;
using BLL.Services;
using DAL.Profissoes;
using DAL.Permissao.Security.Extensions;
using EntityFramework.Extensions;

namespace BLL.Profissoes {

    public class ProfissaoExclusaoBL : DefaultBL, IProfissaoExclusaoBL {

        //
        public ProfissaoExclusaoBL(){

        }

        //Excluir uma empresa logicamente
        public bool excluir(int id) {
            int idUsuarioLogado = User.id();

            db.Profissao
                .Where(x => x.id == id)
                .Update(x => new Profissao { dtExclusao = DateTime.Now, idUsuarioExclusao = idUsuarioLogado });

            return true;
        }

    }
}