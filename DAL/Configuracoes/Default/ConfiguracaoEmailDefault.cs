
namespace DAL.Configuracoes.Default {

    public static class ConfiguracaoEmailDefault {

        public static ConfiguracaoEmail carregar() {

            ConfiguracaoEmail Config = new ConfiguracaoEmail();

            Config.contaEmailSistema = "hospedagem@sinctec.com.br";

            Config.senhaEmailSistema = "123mudar";

            Config.servidorPOPEmailSistema = "mail.sinctec.com.br";

            Config.servidorSMTPEmailSistema = "mail.sinctec.com.br";

            Config.flagSSLPOPEmailSistema = "S";

            Config.flagSSLSMTPEmailSistema = "S";

            Config.portaPOPEmailSistema = 995;

            Config.portaSMTPEmailSistema = 587;

            Config.assinaturaEmail = ""
                + "<p class=\"left-15\">"
                    + "<font color=\"#ffffff\"><b class=\"white\">Associatec</b><br />Fone: (55 11) 1111-1111<br /></font>"
                    + "<a href=\"#DOMINIO#\" style=\"color:#ffffff\">#DOMINIO#</a>"
                + "</p>";

            Config.masterpageEmail = ""
                + "<!DOCTYPE html\">"
                + "<html lang=\"pt-br\">"
                    + "<head>"

                        + "<meta charset=\"utf-8\" />"
                        + "<title>#NOME_APLICACAO#</title>"

                        + "<style>"
                            + ".logo{width: 166px;padding-top: 10px;padding-bottom: 10px;}"
                            + ".titulo{margin-right: 15px;}"
                            + ".right{text-align: right;margin-right: 15px;}"
                            + ".borda-titulo{border-bottom: 6px solid #2ca5d0;}"
                            + "p{color: #959499;}"
                            + "b{color: #000000;}"
                            + ".white{color: #ffffff !important;}"
                            + ".back-titulo{background-color: #304149}"
                            + ".back-footer{background-color: #2ca5d0;}"
                            + ".back-footer td p{color: #ffffff !important;}"
                            + ".left-15{margin-left: 15px;}"
                        + "</style>"

                    + "</head>"

                    + "<body>"
                        + "<table align=\"center\" width=\"650\" cellpadding=\"10\" cellspacing=\"0\" border=\"0\" style=\"background-color: #f2f2f2;font-family: 'Lato', Calibri, Arial, sans - serif;\">"

                               + "<tr>"
                                   + "<td><a href=\"\"><img src=\"#URL_LOGO#\" alt=\"logo\" class=\"logo\"></a></td>"
                                   + "<td align=\"right\">#DATA_EMAIL# </td>"
                               + "</tr>"

                               + "<tr height=\"20\">"
                                   + "<td colspan=\"2\" class=\"borda-titulo\" bgcolor=\"#304149\"><br /><p class=\"titulo\" style=\"text-align:right;color:#ffffff;font-size:20px;\">#ASSUNTO#</p><br /></td>"
                               + "</tr>"

                                + "<tr height=\"2\"><td colspan=\"2\" bgcolor=\"#2ca5d0\"></td></tr>"

                               + "<tr><td colspan=\"2\">#CONTEUDO_MENSAGEM#</td></tr>"

                               + "<tr><td colspan=\"2\" bgcolor=\"#2ca5d0\">#ASSINATURA_ENVIO#</td></tr>"

                           + "</table>"
                       + "</body>"
                   + "</html>";

            return Config;
        }
    }
}
