using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using DAL.Pessoas;

namespace DAL.Permissao.Security.Extensions {

    public static class SecurityExtensions {

        //Verificar se há login ativo na pagina
        public static bool hasLogin(this IPrincipal User) {

            if (String.IsNullOrEmpty(SecurityCookie.userId) || String.IsNullOrEmpty(SecurityCookie.userName) || User.idPerfil() == 0) {

                return false;

            }

            return true;
        }

        //Configurar os cookies de seguranca a partir de um associado
        public static void signInFromEntity(this IPrincipal User, UsuarioSistema OUsuario) {

            SecurityCookie.userId = UtilCrypt.toBase64Encode( OUsuario.id );

            SecurityCookie.idPerfil = UtilCrypt.toBase64Encode(OUsuario.idPerfilAcesso);

            SecurityCookie.descricaoPerfil = OUsuario.PerfilAcesso.descricao;

            SecurityCookie.userName = OUsuario.nome;

            SecurityCookie.userEmail = OUsuario.Pessoa.emailPrincipal();

            SecurityCookie.dtCadastro = OUsuario.dtCadastro.ToShortDateString();

            SecurityCookie.flagAlterarSenha = OUsuario.flagAlterarSenha;

            SecurityCookie.flagTodasUnidades = OUsuario.PerfilAcesso.flagTodasUnidades == true ? "S" : "N";

            SecurityCookie.flagOrganizacao = OUsuario.PerfilAcesso.flagOrganizacao== true ? "S" : "N";

            SecurityCookie.flagSomenteCadastroProprio = OUsuario.PerfilAcesso.flagSomenteCadastroProprio == true ? "S" : "N";

            SecurityCookie.idOrganizacao = UtilCrypt.toBase64Encode(OUsuario.idOrganizacao.toInt());

            SecurityCookie.nomeOrganizacao = OUsuario.Organizacao == null ? "" : OUsuario.Organizacao.Pessoa.nome;
        }

        //Configurar os cookies de seguranca a partir de um associado
        public static void signLojas(this IPrincipal User, int[] idsLojasAcesso, int[] idsClientes) {
            SecurityCookie.idsLojas = string.Join(",", idsLojasAcesso);
            SecurityCookie.idsClientes = string.Join(",", idsClientes);
        }

        public static void signUnidades(this IPrincipal User, int[] idsUnidadesAcesso) {
            SecurityCookie.idsUnidades = string.Join(",", idsUnidadesAcesso);
        }

        public static void signOrganizacoes(this IPrincipal User, int[] idsOrganizacoes) {
            SecurityCookie.idsOrganizacoes = string.Join(",", idsOrganizacoes);
        }

        //Destruir os cookies de seguranca a partir de um associado
        public static void signOut(this IPrincipal User) {

            SecurityCookie.userId = null;

            SecurityCookie.idPerfil = null;

            SecurityCookie.descricaoPerfil = null;

            SecurityCookie.userName = null;

            SecurityCookie.userEmail = null;

            SecurityCookie.dtCadastro = null;

            SecurityCookie.flagAlterarSenha = null;

            SecurityCookie.flagTodasUnidades = null;

            SecurityCookie.flagOrganizacao = null;

            SecurityCookie.flagSomenteCadastroProprio = null;

            SecurityCookie.flagCliente = null;

            SecurityCookie.idsLojas = null;

            SecurityCookie.idsUnidades = null;

            SecurityCookie.idUnidade = null;

            //SecurityCookie.idOrganizacao = null;

            //SecurityCookie.nomeOrganizacao = null;


        }

        public static void setUnidade(this IPrincipal User, string unidade, string nomeUnidade = "") {
            SecurityCookie.idUnidade = unidade;
            SecurityCookie.nomeUnidade = nomeUnidade;
        }

        public static void setOrganizacao(this IPrincipal User, string idOrganizacao, string nomeOrganizacao = "") {

            SecurityCookie.idOrganizacao = UtilCrypt.toBase64Encode(idOrganizacao.toInt());

            SecurityCookie.nomeOrganizacao = nomeOrganizacao;
        }

        public static void setLatitude(this IPrincipal User, string latitude) {
            SecurityCookie.latitude = latitude;
        }

        public static void setLongitude(this IPrincipal User, string longitude) {
            SecurityCookie.longitude = longitude;
        }

        //
        public static int id(this IPrincipal User) {

            string idString = SecurityCookie.userId;

            if (string.IsNullOrEmpty(idString)){
                return 0;
            }

            string cookieUser = UtilCrypt.toBase64Decode(idString);

            return UtilNumber.toInt32(cookieUser);
        }

        //
        public static int idPerfil(this IPrincipal User) {

            string idString = SecurityCookie.idPerfil;

            if (string.IsNullOrEmpty(idString)){
                return 0;
            }

            string cookiePerfil = UtilCrypt.toBase64Decode(idString);

            return UtilNumber.toInt32(cookiePerfil);
        }

        //
        public static string descricaoPerfil(this IPrincipal User) {
            return SecurityCookie.descricaoPerfil;
        }

        //
        public static string name(this IPrincipal User) {
            return SecurityCookie.userName;
        }

        //
        public static string email(this IPrincipal User) {
            return SecurityCookie.userEmail;
        }

        //
        public static string dtCadastro(this IPrincipal User) {
            return SecurityCookie.dtCadastro;
        }

        //
        public static string flagAlterarSenha(this IPrincipal User) {
            return SecurityCookie.flagAlterarSenha;
        }

        //
        public static bool flagDesenvolvedor(this IPrincipal User) {

            int idPerfil = User.idPerfil();

            return idPerfil == PerfilAcessoConst.DESENVOLVEDOR; 

        }
        
        //
        public static bool flagCliente(this IPrincipal User) {
            string flagCliente = SecurityCookie.flagCliente;

            return flagCliente == "S";
        }

        //
        public static bool flagTodasUnidades(this IPrincipal User) {
            string flagTodasUnidades = SecurityCookie.flagTodasUnidades;

            return flagTodasUnidades == "S";
        }

        /// <summary>
        /// 
        /// </summary>
        public static bool flagMultiOrganizacao(this IPrincipal User) {
            
            string flagOrganizacao = SecurityCookie.flagOrganizacao;

            return flagOrganizacao == "S";
        }

        //
        public static bool flagSomenteCadastroProprio(this IPrincipal User) {
            string flagSomenteCadastroProprio = SecurityCookie.flagSomenteCadastroProprio;
            return flagSomenteCadastroProprio == "S";
        }

        //
        public static int idUnidade(this IPrincipal User) {
            return UtilNumber.toInt32(SecurityCookie.idUnidade);
        }

        //
        public static string nomeUnidade(this IPrincipal User) {
            return SecurityCookie.nomeUnidade;
        }

        //
        public static int idOrganizacao(this IPrincipal User) {

            string idString = SecurityCookie.idOrganizacao;

            if (string.IsNullOrEmpty(idString)){
                return 0;
            }

            string cookieOrganizacao = UtilCrypt.toBase64Decode(idString);

            return UtilNumber.toInt32(cookieOrganizacao);
        }

        //
        public static string pastaOrganizacao(this IPrincipal User) {
            return string.Concat("upload/organizacao_", User.idOrganizacao().ToString().PadLeft(15, '0'));
        }

        //
        public static string nomeOrganizacao(this IPrincipal User) {
            return SecurityCookie.nomeOrganizacao;
        }

        //Retornar os dados da latitude de acesso do usuário
        public static string latitude(this IPrincipal User) {
            return SecurityCookie.latitude;
        }

        //Retornar os dados longitude de acesso do usuário
        public static string longitude(this IPrincipal User) {
            return SecurityCookie.longitude;
        }

        //Retornar a lista de clientes para os quais o usupario tem permissao de acesso
        public static List<int> idsClientes(this IPrincipal User) {

            string ids = SecurityCookie.idsClientes;

            List<int> idsClientes = new List<int>();

            if (String.IsNullOrEmpty(ids)) {
                return idsClientes;
            }

            idsClientes = ids.Split(',').Select(UtilNumber.toInt32).Where(x => x > 0).ToList();

            return idsClientes;
        }

        //Retornar a lista de lojas para as quais o usuario tem permissao
        public static List<int> idsLojas(this IPrincipal User) {

            List<int> idsLojas = new List<int>();

            string ids = SecurityCookie.idsLojas;

            if (string.IsNullOrEmpty(ids)) {
                return idsLojas;
            }

            idsLojas = ids.Split(',').Select(UtilNumber.toInt32).Where(x => x > 0).ToList();

            return idsLojas;
        }

        //Retornar a lista de unidades para as quais o usuario tem permissao
        public static List<int> idsUnidades(this IPrincipal User) {

            List<int> idsUnidades = new List<int>();

            string ids = SecurityCookie.idsUnidades;

            if (string.IsNullOrEmpty(ids)) {
                return idsUnidades;
            }

            idsUnidades = ids.Split(',').Select(UtilNumber.toInt32).Where(x => x > 0).ToList();

            return idsUnidades;
        }

        //Retornar a lista de unidades para as quais o usuario tem permissao
        public static List<int> idsOrganizacoes(this IPrincipal User) {

            List<int> idsOrganizacoes = new List<int>();

            string ids = SecurityCookie.idsOrganizacoes;

            if (string.IsNullOrEmpty(ids)) {
                return idsOrganizacoes;
            }

            idsOrganizacoes = ids.Split(',').Select(UtilNumber.toInt32).Where(x => x > 0).ToList();

            return idsOrganizacoes;
        }


    }
}

