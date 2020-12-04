using System;
using System.IO;
using System.Web;
using BLL.Services;
using DAL.Permissao.Security.Extensions;
using UTIL.Upload;

namespace BLL.Configuracoes {

    public class ConfiguracaoImagemBL : DefaultBL, IConfiguracaoImagemBL {

        //Constantes
        public static readonly string IMAGEM_TOPO_SISTEMA = "logotipo_topo.png";

        public static readonly string IMAGEM_LOGIN_SISTEMA = "logotipo_login.png";

        public static readonly string IMAGEM_EMAIL_SISTEMA = "logotipo_email.png";

        public static readonly string IMAGEM_PRINT_SISTEMA = "logotipo_print.png";

        public static readonly string IMAGEM_BG_LOGIN = "bg_login.png";

        public static readonly string IMAGEM_RODAPE_SISTEMA = "logotipo_rodape.png";

        //Propriedades
        private static string baseUrl => string.Concat(UtilConfig.linkAbsSistemaUpload(HttpContextFactory.Current.User.idOrganizacao()), "logotipo/");

        private static string basePath => string.Concat(UtilConfig.pathAbsUpload(HttpContextFactory.Current.User.idOrganizacao()), "logotipo/");

        public static string urlSistemaLogin => string.Concat(baseUrl, IMAGEM_LOGIN_SISTEMA);

        public static string urlSistemaTopo => string.Concat(baseUrl, IMAGEM_TOPO_SISTEMA);

        public static string urlSistemaRodape => string.Concat(baseUrl, IMAGEM_RODAPE_SISTEMA);

        public static string urlSistemaEmail => string.Concat(baseUrl, IMAGEM_EMAIL_SISTEMA);

        public static string urlSistemaPrint => string.Concat(baseUrl, IMAGEM_PRINT_SISTEMA);

        public static string urlBgLogin => string.Concat(baseUrl, IMAGEM_BG_LOGIN);

        //
        public static string linkImagemOrganizacao(int idOrganizacao, string tipoImagem) {

            

            string path = Path.Combine(UtilConfig.pathAbsUpload(idOrganizacao), "logotipo/");

            string url = string.Concat(UtilConfig.linkAbsSistemaUpload(idOrganizacao), "logotipo/");

            string urlDefault = string.Concat(UtilConfig.linkAbsSistema, "img/default/");

            if (idOrganizacao == 0) {

                var User = HttpContextFactory.Current.User;

                idOrganizacao = User.idOrganizacao();
                
                path = string.Concat(UtilConfig.pathAbsUpload(idOrganizacao), "logotipo/");

                url = string.Concat(UtilConfig.linkAbsSistemaUpload(idOrganizacao), "logotipo/");

            }

            string pathImagem = path;

            if (tipoImagem == IMAGEM_TOPO_SISTEMA) {

                pathImagem = string.Concat(pathImagem, IMAGEM_TOPO_SISTEMA);

                if (File.Exists(pathImagem)) {
                    return string.Concat(url, IMAGEM_TOPO_SISTEMA);
                }
                return string.Concat(urlDefault, IMAGEM_TOPO_SISTEMA);
            }

            if (tipoImagem == IMAGEM_RODAPE_SISTEMA) {

                pathImagem = string.Concat(pathImagem, IMAGEM_RODAPE_SISTEMA);

                if (File.Exists(pathImagem)) {
                    return string.Concat(url, IMAGEM_RODAPE_SISTEMA);
                }
                return string.Concat(urlDefault, IMAGEM_RODAPE_SISTEMA);
            }

            if (tipoImagem == IMAGEM_LOGIN_SISTEMA) {

                pathImagem = string.Concat(pathImagem, IMAGEM_LOGIN_SISTEMA);

                if (File.Exists(pathImagem)) {
                    return string.Concat(url, IMAGEM_LOGIN_SISTEMA);
                }
                return string.Concat(urlDefault, IMAGEM_LOGIN_SISTEMA);
            }

            if (tipoImagem == IMAGEM_EMAIL_SISTEMA) {

                pathImagem = string.Concat(pathImagem, IMAGEM_EMAIL_SISTEMA);

                if (File.Exists(pathImagem)) {
                    return string.Concat(url, IMAGEM_EMAIL_SISTEMA);
                }
                return string.Concat(urlDefault, IMAGEM_EMAIL_SISTEMA);
            }

            if (tipoImagem == IMAGEM_PRINT_SISTEMA) {

                pathImagem = string.Concat(pathImagem, IMAGEM_PRINT_SISTEMA);

                if (File.Exists(pathImagem)) {
                    return string.Concat(url, IMAGEM_PRINT_SISTEMA);
                }
                return string.Concat(urlDefault, IMAGEM_PRINT_SISTEMA);
            }

            if (tipoImagem == IMAGEM_BG_LOGIN) {

                pathImagem = string.Concat(pathImagem, IMAGEM_BG_LOGIN);

                if (File.Exists(pathImagem)) {
                    return string.Concat(url, IMAGEM_BG_LOGIN);
                }
                return string.Concat(urlDefault, IMAGEM_BG_LOGIN);
            }

            return "";
        }


        //Salvar as imagens utilizadas por padrão no sistema
        public UtilRetorno salvar(HttpPostedFileBase OArquivo, string tipoImagem, int idOrganizacaoInfo = 0) {

            if (OArquivo == null) {
                return UtilRetorno.newInstance(true, "O arquivo informado é nulo.");
            }

            if (UploadConfig.getExtension(OArquivo) != ".png") {
                return UtilRetorno.newInstance(true, "A imagem não está em formato .png");
            }

            if (tipoImagem == IMAGEM_TOPO_SISTEMA) {
                this.salvarImagem(OArquivo, IMAGEM_TOPO_SISTEMA, idOrganizacaoInfo);
            }

            if (tipoImagem == IMAGEM_LOGIN_SISTEMA) {
                this.salvarImagem(OArquivo, IMAGEM_LOGIN_SISTEMA, idOrganizacaoInfo);
            }

            if (tipoImagem == IMAGEM_EMAIL_SISTEMA) {
                this.salvarImagem(OArquivo, IMAGEM_EMAIL_SISTEMA, idOrganizacaoInfo);
            }


            if (tipoImagem == IMAGEM_PRINT_SISTEMA) {
                this.salvarImagem(OArquivo, IMAGEM_PRINT_SISTEMA, idOrganizacaoInfo);
            }

            if (tipoImagem == IMAGEM_BG_LOGIN) {
                this.salvarImagem(OArquivo, IMAGEM_BG_LOGIN, idOrganizacaoInfo);
            }

            if (tipoImagem == IMAGEM_RODAPE_SISTEMA) {
                this.salvarImagem(OArquivo, IMAGEM_RODAPE_SISTEMA, idOrganizacaoInfo);
            }

            return UtilRetorno.newInstance(false, "A imagem foi salva com sucesso!");
        }

        //
        private void salvarImagem(HttpPostedFileBase OArquivo, string nomeImagem, int idOrganizacaoInfo = 0) {

            if (User.idOrganizacao() > 0) {
                idOrganizacaoInfo = User.idOrganizacao();
            }

            string pathFolder = Path.Combine(UtilConfig.pathAbsUpload(idOrganizacaoInfo), "logotipo");

            string pathImagem = Path.Combine(pathFolder, nomeImagem);

            if (File.Exists(pathImagem)) {
                File.Delete(pathImagem);
            }

            if (!Directory.Exists(pathFolder)) {
                UtilIO.createFolder(pathFolder);
            }

            OArquivo.SaveAs(pathImagem);
        }
    }
}
