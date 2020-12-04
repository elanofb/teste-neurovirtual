namespace DAL.Emails {

    public class ProcessoAvaliacaoEmailsConst {
        
        public static readonly string tituloEmailConviteAvaliador = "Convite para Comissão de Avaliação - #TITULO_PROCESSO#";
        public static readonly string corpoEmailConviteAvaliador = "<p>Prezado(a) #NOME_AVALIADOR#,</p> <p>Você foi selecionado como membro da comissão de avaliação para o processo #TITULO_PROCESSO#.</p> <p><strong>Dia:</strong> #DATAS_PROCESSO#.</p> <p>Leia com atenção o código de ética em anexo e registre o seu aceite através do link abaixo.</p> <p><a href=\"#LINK_ACEITE#\" style=\"font-weight:bold;font-size:16px;text-decoration:none;color:#00a65a;text-transform:uppercase;\"><img src=\"#IMG_ACEITE#\">&nbsp;Ciente e de acordo</a></p> <p><a href=\"#LINK_NAO_ACEITE#\" style=\"font-weight:bold;font-size:16px;text-decoration:none;color:#dd4b39;text-transform:uppercase;\"><img src=\"#IMG_NAO_ACEITE#\">&nbsp;Não concordo</a></p> <p>Atenciosamente,</p>";
        
        public static readonly string tituloEmailNovaInscricao = "Inscrição recebida - #TITULO_PROCESSO#";
        public static readonly string corpoEmailNovaInscricao = "<p>Prezado(a) #NOME_INSCRITO#,</p> <p>Sua inscri&ccedil;&atilde;o para o processo #TITULO_PROCESSO# foi recebida com sucesso. É necessário realizar o pagamento para que seja efetivada.</p> <p>Fique atento aos próximos passos do processo. Você sempre será notificado através de mensagens por e-mail.</p> <p>#INSTRUCOES_PAGAMENTO#</p> <p> <a href='#LINK_INSCRICAO#'>Neste link você poderá consultar os dados de sua inscrição.</a></p> <p>Atenciosamente,</p>";
        
        public static readonly string tituloEmailConfirmacaoPagamento = "Pagamento recebido - #TITULO_PROCESSO#";
        public static readonly string corpoEmailConfirmacaoPagamento = "<p>Ol&aacute; #NOME_INSCRITO#!</p> <p>Recebemos a confirma&ccedil;&atilde;o do pagamento referente a inscri&ccedil;&atilde;o do processo #TITULO_PROCESSO#.</p> <p><strong>Valor da Inscrição:</strong> #VALOR_INSCRICAO#</p> <p>Para acompanhar o seu processo <a href=\"#LINK_INSCRICAO#\">clique aqui</a></p> <p>Atenciosamente,</p>";
        
        public static readonly string tituloEmailCobranca = "Inscrição pendente - #TITULO_PROCESSO#";
        public static readonly string corpoEmailCobranca = "<p>Ol&aacute; #NOME_INSCRITO#!</p> <p>Sua inscri&ccedil;&atilde;o do processo #TITULO_EVENTO# está pendente, realize o pagamento para confirmar sua inscri&ccedil;&atilde;o.</p> <p>#INSTRUCOES_PAGAMENTO#</p><p><strong>Local:</strong> #LOCAL_PROCESSO#</p> <p><strong>Endere&ccedil;o:</strong> #ENDERECO_PROCESSO#</p><p><strong>Dia:</strong> #DATAS_PROCESSO#.</p> <p>Atenciosamente,</p> ";

        public static readonly string tituloEmailAprovacaoDocumentos = "Documentos Aprovados - #TITULO_PROCESSO#";
        public static readonly string corpoEmailAprovacaoDocumentos = "<p>Ol&aacute; #NOME_INSCRITO#!</p> <p>Informamos que os documentos enviados no processo #TITULO_PROCESSO# foram aprovados. <br/> A partir de agora a comiss&atilde;o respons&aacute;vel ir&aacute; avaliar o seu curr&iacute;culo. Aguarde novas informa&ccedil;&otilde;es. <br /><br /> Para acompanhar o seu processo <a href=\"#LINK_INSCRICAO#\">clique aqui</a></p>";

        public static readonly string tituloEmailReprovacaoDocumentos = "Documentos Reprovados - #TITULO_PROCESSO#";
        public static readonly string corpoEmailReprovacaoDocumentos = "<p>Ol&aacute; #NOME_INSCRITO#!</p> <p>Infelizmente um ou mais documentos enviados no processo #TITULO_PROCESSO# foram reprovados. <br/> O prazo para regulariza&ccedil;&atilde;o vai at&eacute; #DATA_PRAZO_REGULARIZACAO#. <br /><br /> Para conhecer os problemas e fazer a regulariza&ccedil;&atilde;o acesse o seu processo <a href=\"#LINK_INSCRICAO#\">clicando aqui</a></p>";

        public static readonly string tituloEmailPendenciaDocumentos = "Documentos pendentes - #TITULO_PROCESSO#";
        public static readonly string corpoEmailPendenciaDocumentos = "<p>Ol&aacute; #NOME_INSCRITO#!</p> <p>Infelizmente um ou mais documentos enviados no processo #TITULO_PROCESSO# est&atilde;o com pend&ecirc;ncia. <br/> Solicitamos que realize os ajustes pertinentes o quanto antes. <br /><br /> Para conhecer os problemas e fazer a regulariza&ccedil;&atilde;o acesse o seu processo <a href=\"#LINK_INSCRICAO#\">clicando aqui</a></p>";

        public static readonly string tituloEmailAnaliseCurriculo = "Análise de Currículo - #TITULO_PROCESSO#";
        public static readonly string corpoEmailAnaliseCurriculo = "<p>Ol&aacute; #NOME_INSCRITO#!</p> <p>Temos um grupo de inscri&ccedil;&atilde;es dispon&iacute;veis para sua avalia&ccedil;&atilde;o. <br> <a href=\"#LINK_INSCRICAO#\">Clique aqui</a></p>";
        
        public static readonly string tituloEmailSenhaInscricao = "Senha de acesso da inscrição - #TITULO_PROCESSO#";
        public static readonly string corpoEmailSenhaInscricao = "<p>Ol&aacute; #NOME_INSCRITO#!</p> <p>Segue abaixo sua senha de acesso para sua inscrição no #TITULO_PROCESSO#. <br> Senha de aceso: #SENHA# <br><br>  <a href=\"#LINK_INSCRICAO#\">Clique aqui para acessar sua inscrição</a></p>";
        
        public static readonly string tituloEmailEnvioDocumentos = "Envio Documentação - #TITULO_PROCESSO#";
        public static readonly string corpoEmailEnvioDocumentos = "<p>Prezado administrador(a), o inscrito #NOME_INSCRITO#, acabou de enviar documentos atualizados para avaliação. Acesse sua área restrita e avalie novamente os documentos assim que possível.</p>";

        public static readonly string tituloEmailAnaliseInscricao = "Análise de Inscrições - #TITULO_PROCESSO#";
        public static readonly string corpoEmailAnaliseInscricao = "<p>Caro #NOME_AVALIADOR#!</p> <p>Você recebeu a etapa #NOME_ETAPA# para avaliar referente ao processo #TITULO_PROCESSO#</p> <p>Acesso link restrito abaixo para realizar as suas considerações. <br> <a href=\"#LINK_AVALIACAO#\">Clique aqui</a></p>";
        
        public static readonly string tituloEmailAnaliseCurriculoFinalizada = "Análise de Currículo Finalizada - #TITULO_PROCESSO#";
        public static readonly string corpoEmailAnaliseCurriculoFinalizada = "<p>Ol&aacute; #NOME_INSCRITO#!</p> <p>A análise de currículo da sua inscrição no #TITULO_PROCESSO# foi finalizada com sucesso! <br><br>  <a href=\"#LINK_INSCRICAO#\">Clique aqui para acessar sua inscrição</a></p>";
        
        public static readonly string tituloEmailAnaliseCurriculoPendencia = "Análise de Currículo Pendente - #TITULO_PROCESSO#";
        public static readonly string corpoEmailAnaliseCurriculoPendencia = "<p>Ol&aacute; #NOME_INSCRITO#!</p> <p>Há pendências no seu currículo enviado para a inscrição no #TITULO_PROCESSO#! <br><br>  <a href=\"#LINK_INSCRICAO#\">Clique aqui para acessar sua inscrição</a></p>";
        
        public static readonly string tituloEmailInscricaoDesclassificada = "Desclassificado - #TITULO_PROCESSO#";
        public static readonly string corpoEmailInscricaoDesclassificada = "<p>Caro #NOME_INSCRITO#!</p> <p>Infelizmente a sua inscri&ccedil;&atilde;o foi desclassificada em #DATA_DESCLASSIFICACAO#. Veja abaixo o motivo: <br> #MOTIVO# <br><br> Esperamos que em breve você possa participar novamente do #TITULO_PROCESSO#.</p>";
        
        public static readonly string tituloEmailEtapaConcluida = "Etapa Concluída - #TITULO_PROCESSO#";
        public static readonly string corpoEmailEtapaConcluida = "<p>Caro #NOME_INSCRITO#!</p> <p>Você concluiu a etapa de #TITULO_ETAPA#.<br><br> Data de Finalização: #DATA_FINALIZACAO#<br> Situação: #SITUACAO#. <br><br>  <a href=\"#LINK_INSCRICAO#\">Clique aqui para acessar sua inscrição</a> </p>";
        
        public static readonly string tituloEmailEnvioSenha = "Senha de acesso da inscrição - #TITULO_PROCESSO#";
        public static readonly string corpoEmailEnvioSenha = "<p>Prezado #NOME_INSCRITO#!</p> <p>Recebemos a solicitação de recuperação de acesso à área do inscrito para o evento #TITULO_PROCESSO#.<br><br> Sua nova senha de acesso é: #NOVA_SENHA# <br><br> Você pode escolher uma senha de sua preferência diretamente na área do inscrito.<br><br>  <a href=\"#LINK_INSCRICAO#\">Clique aqui para acessar sua inscrição</a> </p>";
                
        public static readonly string tituloEmailComprovanteCurriculo = "Recibo de envio curricular - #TITULO_PROCESSO#";
        public static readonly string corpoEmailComprovanteCurriculo = "<p>Ol&aacute; #NOME_INSCRITO#!</p> <p>Recebemos as informações do seu currículo e já estão em análise. Fique atento(a) ao seu e-mail, você será notificado(a) quando a avalição for realizada pela comissão responsável. <br><br>  <a href=\"#LINK_INSCRICAO#\">Clique aqui para acessar sua inscrição</a></p>";
        
        public static readonly string tituloEmailAprovacaoProcesso = "Resultado Final - #TITULO_PROCESSO#";
        public static readonly string corpoEmailAprovacaoProcesso = "<p>Caro #NOME_INSCRITO#,</p> <p>Parabéns!</p><p>Informamos que você foi aprovado(a) no processo #TITULO_PROCESSO#.</p><p>Acesse este <a href=\"#LINK_RESULTADO#\">link</a> e conheça a lista completa dos aprovados.</p>";
        
        public static readonly string tituloEmailReprovacaoProcesso = "Resultado Final - #TITULO_PROCESSO#";
        public static readonly string corpoEmailReprovacaoProcesso = "<p>Caro #NOME_INSCRITO#,</p> <p>Infelizmente informamos que você foi reprovado(a) no processo #TITULO_PROCESSO#.</p><p>Acesse este <a href=\"#LINK_RESULTADO#\">link</a> e conheça a lista completa dos aprovados.</p>";
        
        public static readonly string tituloEmailEtapaAprovada = "Resultado da Etapa #TITULO_ETAPA# - #TITULO_PROCESSO#";
        public static readonly string corpoEmailEtapaAprovada = "<p>Caro #NOME_INSCRITO#,</p> <p>Informamos que sua inscrição foi APROVADA na etapa: #TITULO_ETAPA# no #TITULO_PROCESSO#.</p><p>Consulte a &aacute;rea do inscrito atrav&eacute;s do link abaixo para acompanhar o seu processo</p><p><a href='#LINK#'>Área do Inscrito #TITULO_PROCESSO#</a></p>";
                
        public static readonly string tituloEmailEtapaReprovada = "Resultado da Etapa #TITULO_ETAPA# - #TITULO_PROCESSO#";
        public static readonly string corpoEmailEtapaReprovada = "<p>Caro #NOME_INSCRITO#,</p> <p>Informamos que infelizmente a sua inscri&ccedil;&atilde;o NÃO FOI APROVADA na etapa: #TITULO_ETAPA# no #TITULO_PROCESSO#.</p><p>Consulte a &aacute;rea do inscrito atrav&eacute;s do link abaixo para obter maiores informações</p><p><a href='#LINK#'>Área do Inscrito #TITULO_PROCESSO#</a></p>";
        
        
    }

}
