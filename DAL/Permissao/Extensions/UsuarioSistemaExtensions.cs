using System;

namespace DAL.Permissao {

    public static class UsuarioSistemaExtensions {

        public static bool isDegustacao(this UsuarioSistema OUsuario) {

            if (OUsuario.dtInicioDegustacao.HasValue && 
                OUsuario.dtFimDegustacao.HasValue) {

                return true;
            }

            return false;
        }

        public static bool periodoDegustacaoAtivo (this UsuarioSistema OUsuario) {

            if (DateTime.Now >= OUsuario.dtInicioDegustacao && 
                DateTime.Now <= OUsuario.dtFimDegustacao) {

                return true;
            }

            return false;
        }

    }

}
