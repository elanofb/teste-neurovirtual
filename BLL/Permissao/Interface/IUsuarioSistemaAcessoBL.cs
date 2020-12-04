using System;

namespace BLL.Permissao {

    public interface IUsuarioSistemaAcessoBL {

        UtilRetorno login(string login, string senha);

        UtilRetorno recuperarSenha(string login);

        UtilRetorno criarNovaSenha(int idUsuario);
    }
}