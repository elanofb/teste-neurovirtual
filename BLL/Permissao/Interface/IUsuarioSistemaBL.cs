using System;
using System.Json;
using System.Linq;
using DAL.Permissao;

namespace BLL.Permissao {
    public interface IUsuarioSistemaBL {

        UsuarioSistema carregar(int id);

        IQueryable<UsuarioSistema> listar(int idPerfilAcesso, string valorBusca, string ativo);

        bool salvar(UsuarioSistema OUsuarioSistema);

        UtilRetorno excluir(int idUsuario, int idUsuarioExclusao);

        JsonMessageStatus alterarStatus(int id);

        UtilRetorno alterarSenha(int idUsuario, string senha);
    }
}