using BLL.Permissao;
using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.UsuariosUnidades {

    public interface IUsuarioUnidadeBL  {

        IQueryable<UsuarioUnidade> listar(int? idUsuario, int? idUnidade, string flagExcluido = "N");
        UsuarioUnidade carregar(int id);
        void salvar(int idUsuario, int idUnidade);
        void excluir(int id);
        
    }
}
