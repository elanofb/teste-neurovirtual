using BLL.Permissao;
using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Organizacoes;

namespace BLL.Organizacoes {

    public interface IUsuarioOrganizacaoBL {

        IQueryable<UsuarioOrganizacao> listar(int? idUsuario, int? idOrganizacao, string flagExcluido = "N");
        UsuarioOrganizacao carregar(int id);
        void salvar(int idUsuario, int idOrganizacao);
        void excluir(int id);
        
    }
}
