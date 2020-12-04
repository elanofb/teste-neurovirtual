using System;
using System.Security.Principal;

namespace DAL.Permissao.Security.Extensions {

    public static class SecurityExtensionsCheckout {

        //Verificar se há login ativo na pagina
        public static bool hasOrganizacaoCheckout(this IPrincipal User) {

            if (String.IsNullOrEmpty(SecurityCookieCheckout.idOrganizacao) || String.IsNullOrEmpty(SecurityCookieCheckout.idOrganizacao)) {
                return false;
            }

            return true;
        }

        //Configurar os cookies de seguranca a partir de um associado
        public static void signInCheckout(this IPrincipal User, int idOrganizacao) {

            SecurityCookieCheckout.idOrganizacao = UtilCrypt.toBase64Encode(idOrganizacao);

            SecurityCookie.idOrganizacao = UtilCrypt.toBase64Encode(idOrganizacao);
        }


        //Destruir os cookies de seguranca a partir de um associado
        public static void signOutCheckout(this IPrincipal User) {

            SecurityCookieCheckout.idOrganizacao = null;

        }

        //Id de organização para o checkout
        public static int idOrganizacaoCheckout(this IPrincipal User){

            string idString = SecurityCookieCheckout.idOrganizacao;

            if (string.IsNullOrEmpty(idString)){
                return 0;
            }

            string cookieOrganizacao = UtilCrypt.toBase64Decode(idString);

            return UtilNumber.toInt32(cookieOrganizacao);
        }


    }
}

