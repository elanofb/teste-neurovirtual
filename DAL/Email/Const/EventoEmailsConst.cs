namespace DAL.Eventos {

    public class EventoEmailsConst {

        //Módulo básico
        public static readonly string corpoEmailNovaInscricaoEvento = "<p>Prezado(a) #NOME_INSCRITO#,</p><p><br></p><p>Sua inscri&ccedil;&atilde;o para o evento #TITULO_EVENTO# foi reservada com sucesso.</p><p><br></p><p>Guarde esse e-mail com os dados de realiza&ccedil;&atilde;o do evento.</p><p><br></p><p>#INSTRUCOES_PAGAMENTO#</p><p><strong>Local:</strong> #LOCAL_EVENTO#</p><p><strong>Endere&ccedil;o:</strong> #ENDERECO_EVENTO#</p><p><strong>Dia:</strong> #DATAS_EVENTO#.</p><p><br></p><p>Atenciosamente,</p><p><br></p>";

        public static readonly string corpoEmailPagamentoInscricao = "<p>Ol&aacute; #NOME_INSCRITO#!</p><p>	<br></p><p>Recebemos a confirma&ccedil;&atilde;o do pagamento referente a inscri&ccedil;&atilde;o do evento #TITULO_EVENTO#.</p><p>	<br></p><p>Dados do Evento:</p><p>	<br></p><p><strong>Valor:</strong> #VALOR_INSCRICAO#</p><p><strong>Local:</strong> #LOCAL_EVENTO#</p><p><strong>Endere&ccedil;o:</strong> #ENDERECO_EVENTO#</p><p><strong>Dia:</strong> #DATAS_EVENTO#.</p><p>	<br></p><p>Atenciosamente,</p><p>	<br></p>";

        public static readonly string corpoEmailIsencaoInscricao = "<p>Ol&aacute; #NOME_INSCRITO#!</p><p>	<br></p><p>Voc&ecirc; acaba de receber a isen&ccedil;&atilde;o na inscri&ccedil;&atilde;o do evento #TITULO_EVENTO#.</p><p>	<br></p><p>#MOTIVO_ISENCAO#Dados do Evento:</p><p>	<br></p><p><strong>Local:</strong> #LOCAL_EVENTO#</p><p><strong>Endere&ccedil;o:</strong> #ENDERECO_EVENTO#</p><p><strong>Dia:</strong> #DATAS_EVENTO#.</p><p>	<br></p>";
        
        
        //Módulo avançado
        public static readonly string tituloEmailNovaInscricao = "Inscrição recebida - #TITULO_EVENTO#";
        public static readonly string corpoEmailNovaInscricao = "<p>Prezado(a) #NOME_INSCRITO#,</p> <p>Sua inscri&ccedil;&atilde;o para o evento #TITULO_EVENTO# foi recebida com sucesso!</p> <p>O evento ocorrerá em #DATAS_EVENTO#.</p> <p>#INSTRUCOES_PAGAMENTO#</p> <p>Atenciosamente,</p>";
        
        public static readonly string tituloEmailLiberacaoInscricao = "Liberação de Inscrição - #TITULO_EVENTO#";
        public static readonly string corpoEmailLiberacaoInscricao = "<p>Prezado(a) #NOME_INSCRITO#,</p> <p>A an&aacute;lise de sua inscri&ccedil;&atilde;o foi conclu&iacute;da.</p> <p>#INSTRUCOES_PAGAMENTO#</p> <p>Atenciosamente,</p>";
        
        public static readonly string tituloEmailConfirmacaoPagamento = "Pagamento recebido - #TITULO_EVENTO#";
        public static readonly string corpoEmailConfirmacaoPagamento = "<p>Ol&aacute; #NOME_INSCRITO#!</p> <p>Recebemos a confirma&ccedil;&atilde;o do pagamento referente a inscri&ccedil;&atilde;o do evento #TITULO_EVENTO#.</p> <p><strong>Valor da Inscrição:</strong> #VALOR_INSCRICAO#</p> <p>Para acompanhar a sua inscrição <a href=\"#LINK_INSCRICAO#\">clique aqui</a></p> <p>Atenciosamente,</p>";
        
        public static readonly string tituloEmailIsencaoInscrito = "Isenção de Inscrição - #TITULO_EVENTO#";
        public static readonly string corpoEmailIsencaoInscrito = "<p>Ol&aacute; #NOME_INSCRITO#!</p> <p>Voc&ecirc; acaba de receber a isen&ccedil;&atilde;o na inscri&ccedil;&atilde;o do evento #TITULO_EVENTO#.</p><p>#MOTIVO_ISENCAO#</p> <p>Atenciosamente,</p>";
                        
        public static readonly string tituloEmailConvitePalestrante = "Convite para Palestrante - #TITULO_EVENTO#";
        public static readonly string corpoEmailConvitePalestrante = "<p>Prezado(a) #NOME_PALESTRANTE#,</p><p>Voc&ecirc; foi selecionado como palestrante para o evento #TITULO_EVENTO#.</p><p><strong>Local:&nbsp;</strong>#LOCAL_EVENTO#</p><p><strong>Atra&ccedil;&atilde;o: &nbsp;</strong>#TITULO_ATRACAO#</p><p><strong>Fun&ccedil;&atilde;o: </strong>#EVENTO_CARGO#</p><p><strong>Dia:</strong> #DATAS_EVENTO#.</p><p><strong>Hor&aacute;rio:</strong> #HORARIO_INICIO# &agrave;s #HORARIO_FIM#</p><p><strong>Local da Atra&ccedil;&atilde;o:</strong> #LOCAL_ATRACAO#</p><p>Registre o seu aceite atrav&eacute;s do link abaixo.</p><p><a href=\"#LINK_ACEITE#\" style=\"font-weight:bold;font-size:16px;text-decoration:none;color:#00a65a;text-transform:uppercase;\"><img src=\"#IMG_ACEITE#\" class=\"fr-fic fr-dii\">&nbsp;Ciente e de acordo</a></p><p><a href=\"#LINK_NAO_ACEITE#\" style=\"font-weight:bold;font-size:16px;text-decoration:none;color:#dd4b39;text-transform:uppercase;\"><img src=\"#IMG_NAO_ACEITE#\" class=\"fr-fic fr-dii\">&nbsp;N&atilde;o concordo</a></p><p>Atenciosamente,</p>";
        
        public static readonly string tituloEmailPreenchimentoInscricao = "Inscrição para o evento #TITULO_EVENTO#";
        public static readonly string corpoEmailPreenchimentoInscricao = "<p>Prezado(a) participante,</p><p>Voc&ecirc; está recebendo esta mensagem para realizar a inscrição para o evento #TITULO_EVENTO#.</p><p>O convite foi feito por #NOME_COMPRADOR#.</p><p>Acesse o link abaixo e preencha os dados para efetivar sua participação:<br /><a href=\"#LINK_INSCRICAO#\">Link para Inscrição</a></p><p>Atenciosamente,</p>";

        public static readonly string tituloEmailEnvioSenha = "Senha de acesso da inscrição - #TITULO_EVENTO#";
        public static readonly string corpoEmailEnvioSenha = "<p>Prezado #NOME_INSCRITO#!</p> <p>Recebemos a solicitação de recuperação de acesso à área do inscrito para o evento #TITULO_EVENTO#.<br><br> Sua nova senha de acesso é: #NOVA_SENHA# <br><br> Você pode escolher uma senha de sua preferência diretamente na área do inscrito.<br><br>  <a href=\"#LINK_INSCRICAO#\">Clique aqui para acessar sua inscrição</a> </p>";
        
        public static readonly string tituloEmailEnvioCertificado = "Certificado para impressão: #TITULO_EVENTO#";
        public static readonly string corpoEmailEnvioCertificado = "<p>Prezado #NOME_INSCRITO#!</p> <p>Para visualizar e/ou imprimir seu certificado do evento #TITULO_EVENTO#, clique no link abaixo. <br><br>  <a href='#LINK_CERTIFICADO#'>Clique aqui para acessar seu certificado</a> </p>";
        
    }

}
