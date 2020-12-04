
using System;
using DAL.Pedidos;

namespace DAL.Configuracoes.Default {

    public static class ConfiguracaoNotificacaoDefault {
        
        public static void completarInformacoes(ConfiguracaoNotificacao Config) {
                
            Config.emailAssociadoDegustacao = !Config.emailAssociadoDegustacao.isEmpty() ? Config.emailAssociadoDegustacao : "hospedagem@sinctec.com.br";

            Config.corpoEmailAssociadoDegustacao = !Config.corpoEmailAssociadoDegustacao.isEmpty() ? Config.corpoEmailAssociadoDegustacao : "";
            
            
            Config.novoUsuarioTitulo = !Config.novoUsuarioTitulo.isEmpty() ? Config.novoUsuarioTitulo : "Novo Usuário - #NOME_ORGANIZACAO#";

            Config.novoUsuarioCorpo = !Config.novoUsuarioCorpo.isEmpty() ? Config.novoUsuarioCorpo : "<p>Prezado #NOME#,</p><br /><p>Voc&ecirc; foi cadastrado no sistema de gest&atilde;o da associação #NOME_ORGANIZACAO#.</p><br /><p>Seu login de acesso &eacute;: <span style=\"font - weight: 700;\">#LOGIN#</span><br />A sua senha provis&oacute;ria &eacute; <span style=\"font-weight: 700;\">#SENHA#</span></p><p>Voc&ecirc; dever&aacute; alterar a sua senha no primeiro acesso ao sistema.<br>Acesse o link: <span style=\"font-weight: 700;\">#DOMINIO#</span></p>";


            Config.reenvioSenhaUsuarioTitulo = !Config.reenvioSenhaUsuarioTitulo.isEmpty() ? Config.reenvioSenhaUsuarioTitulo : "Reenvio de Senha - #NOME_ORGANIZACAO#";

            Config.reenvioSenhaUsuarioCorpo = !Config.reenvioSenhaUsuarioCorpo.isEmpty() ? Config.reenvioSenhaUsuarioCorpo : "<p>Prezado(a) #NOME#,</p><p>Voc&ecirc; solicitou a recupera&ccedil;&atilde;o dos dados de acesso ao sistema de gestão da #NOME_ORGANIZACAO#.</p><p>O seu login para acesso &eacute;: <span style=\"font-weight: 700;\">#LOGIN#</span></p><p>Sua senha provis&oacute;ria &eacute;: <span style=\"font-weight: 700;\">#SENHA#</span></p><p>No seu pr&oacute;ximo acesso, voc&ecirc; ser&aacute; obrigado a informar uma nova senha.<br><br>No caso de d&uacute;vidas entre em contato conosco.</p>";
            

            Config.recuperacaoSenhaUsuarioTitulo = !Config.recuperacaoSenhaUsuarioTitulo.isEmpty() ? Config.recuperacaoSenhaUsuarioTitulo : "Recuperação de senha - #NOME_ORGANIZACAO#";

            Config.recuperacaoSenhaUsuarioCorpo = !Config.recuperacaoSenhaUsuarioCorpo.isEmpty() ? Config.recuperacaoSenhaUsuarioCorpo : "<p>Prezado(a) #NOME#,</p><p>Voc&ecirc; solicitou a recupera&ccedil;&atilde;o dos dados de acesso ao sistema de gestão da #NOME_ORGANIZACAO#.</p><p>O seu login para acesso &eacute;: <span style=\"font-weight: 700;\">#LOGIN#</span></p><p>Sua senha provis&oacute;ria &eacute;: <span style=\"font-weight: 700;\">#SENHA#</span></p><p>No seu pr&oacute;ximo acesso, voc&ecirc; ser&aacute; obrigado a informar uma nova senha.<br><br>No caso de d&uacute;vidas entre em contato conosco.</p><div><br></div>";
            
            
            Config.assuntoEmailMensagemAtendimento = !Config.assuntoEmailMensagemAtendimento.isEmpty() ? Config.assuntoEmailMensagemAtendimento : "Nova Mensagem - Atendimento #NRO_ATENDIMENTO#";

            Config.corpoEmailMensagemAtendimento = !Config.corpoEmailMensagemAtendimento.isEmpty() ? Config.corpoEmailMensagemAtendimento : "Prezado(a) #NOME_PESSOA#, <br /><br />Você recebeu uma nova mensagem referente ao atendimento #NRO_ATENDIMENTO#:<br /><br />#MENSAGEM#<br /><br />Atenciosamente,<br />";


            Config.emailContato = !Config.emailContato.isEmpty() ? Config.emailContato : "hospedagem@sinctec.com.br";

            Config.corpoEmailNovaNotificacao = !Config.corpoEmailNovaNotificacao.isEmpty() ? Config.corpoEmailNovaNotificacao : "<p>Prezado(a) #NOME_ASSOCIADO#, </p><p>Você recebeu uma nova notificação:</p>#NOTIFICACAO# <p>Atenciosamente,</p>";

            completarInformacoesAssociados(Config);

            completarInformacoesNaoAssociados(Config);

            completarInformacoesPedidos(Config);

            completarInformacoesCobrancas(Config);

            completarInformacoesEventos(Config);

        }

        private static void completarInformacoesAssociados(ConfiguracaoNotificacao Config) {

            Config.emailNovoAssociado = !Config.emailNovoAssociado.isEmpty() ? Config.emailNovoAssociado : "hospedagem@sinctec.com.br";

            Config.tituloEmailRecuperacaoSenhaAssociado = !Config.tituloEmailRecuperacaoSenhaAssociado.isEmpty() ? Config.tituloEmailRecuperacaoSenhaAssociado : "Recuperação de Senha - #NOME_ORGANIZACAO#";

            Config.corpoEmailRecuperacaoSenhaAssociado = !Config.corpoEmailRecuperacaoSenhaAssociado.isEmpty() ? Config.corpoEmailRecuperacaoSenhaAssociado : "Caro(a) #NOME# <br /><br />Recebemos a solicitação de recuperação de sua senha para a área restrita #SIGLA_ASSOCIACAO#.<br />Clique no link a seguir para gerar uma nova senha: <a href=\"#LINK_RECUPERACAO#\">Link de recuperação.</a>";

            Config.reenvioSenhaAssociadoTitulo = !Config.reenvioSenhaAssociadoTitulo.isEmpty() ? Config.reenvioSenhaAssociadoTitulo : "Reenvio de Senha - #NOME_ORGANIZACAO#";

            Config.reenvioSenhaAssociadoCorpo = !Config.reenvioSenhaAssociadoCorpo.isEmpty() ? Config.reenvioSenhaAssociadoCorpo : "<p>Prezado(a) #NOME#,</p><p>Voc&ecirc; solicitou uma nova senha de acesso para a área restrita #NOME_ORGANIZACAO#.</p><p>O seu login para acesso &eacute;: <span style=\"font-weight: 700;\">#LOGIN#</span></p><p>Sua senha provis&oacute;ria &eacute;: <span style=\"font-weight: 700;\">#SENHA#</span></p><br><br><p>No caso de d&uacute;vidas entre em contato conosco.</p>";

            Config.corpoEmailNovoAssociado = !Config.corpoEmailNovoAssociado.isEmpty() ? Config.corpoEmailNovoAssociado : "Olá #TRATAMENTO# #NOME#!<br />Seja bem vindo à #NOME_ORGANIZACAO#, sua inscrição já foi processada.<br><br>Seu número de Inscrição é: <strong>#NUMERO_ASSOCIADO#</strong><br /><br /><b>Sua solicitação de filiação foi recebida com sucesso! #COMPLEMENTO_MENSAGEM_SUCESSO#</b><br /><br />Obrigado,Equipe #NOME_ORGANIZACAO#.";

            Config.assuntoEmailFichaAssociado = !Config.assuntoEmailFichaAssociado.isEmpty() ? Config.assuntoEmailFichaAssociado :  "Solicitação de filiação – #NOME#";

            Config.corpoEmailFichaAssociado = !Config.corpoEmailFichaAssociado.isEmpty() ? Config.corpoEmailFichaAssociado : "Olá,<br />Recebemos um novo cadastro para associado. Segue abaixo os dados e o link para sua ficha cadastral completa:<br><br>#NOME#<br />#DESCRICAO_DOCUMENTO#: #NRO_DOCUMENTO#<br />Número de Inscrição: #NUMERO_ASSOCIADO#<br />Tipo de Associado: #TIPO_ASSOCIADO#<br />Endereço: #ENDERECO#<br /><br /><a href=\"#LINK#\">Clique aqui</a> para visualizar a ficha completa.<br /><br />Atenciosamente,<br />Obrigado.";
            
        }

        private static void completarInformacoesNaoAssociados(ConfiguracaoNotificacao Config) {

            Config.emailNovoNaoAssociado = !Config.emailNovoNaoAssociado.isEmpty() ? Config.emailNovoNaoAssociado : "hospedagem@sinctec.com.br";

            Config.corpoEmailNovoNaoAssociado = !Config.corpoEmailNovoNaoAssociado.isEmpty() ? Config.corpoEmailNovoNaoAssociado : "Olá #TRATAMENTO# #NOME#!<br />Seja bem vindo à #NOME_ORGANIZACAO#, sua inscrição já foi processada.<br><br>Seu número de Inscrição é: <strong>#NUMERO_ASSOCIADO#</strong><br /><br /><b>Sua solicitação de filiação foi recebida com sucesso!</b><br /></p>";

            Config.assuntoEmailFichaNaoAssociado = !Config.assuntoEmailFichaNaoAssociado.isEmpty() ? Config.assuntoEmailFichaNaoAssociado : "Solicitação de filiação – #NOME#";

            Config.corpoEmailFichaNaoAssociado = !Config.corpoEmailFichaNaoAssociado.isEmpty() ? Config.corpoEmailFichaNaoAssociado : "Olá,<br />Recebemos uma nova solicitação de filiação. Segue abaixo os dados cadastrais para serem avaliados:<br><br>#NOME#<br />#DESCRICAO_DOCUMENTO#: #NRO_DOCUMENTO#<br />E-mail: #EMAIL#<br />Telefone: #TELEFONE#<br />Tipo de Associado: #TIPO_ASSOCIADO#<br />Endereço: #ENDERECO#<br /><br /><a href=\"#LINK#\">Clique aqui</a> para visualizar a ficha completa.<br /><br />Atenciosamente,<br />";
            
        }

        private static void completarInformacoesPedidos(ConfiguracaoNotificacao Config) {

            Config.emailPedidos = !Config.emailPedidos.isEmpty() ? Config.emailPedidos : "hospedagem@sinctec.com.br";

            Config.corpoEmailNovoPedido = !Config.corpoEmailNovoPedido.isEmpty() ? Config.corpoEmailNovoPedido :  "";

            Config.corpoEmailCobrancaPedido = !Config.corpoEmailCobrancaPedido.isEmpty() ? Config.corpoEmailCobrancaPedido : "";

            Config.corpoEmailPagamentoPedido = !Config.corpoEmailPagamentoPedido.isEmpty() ? Config.corpoEmailPagamentoPedido : "";

            Config.tituloEmailNovoPedido = !Config.tituloEmailNovoPedido.isEmpty() ? Config.tituloEmailNovoPedido : PedidoEmailsConst.tituloEmailNovoPedido;

            Config.corpoEmailNovoPedido = !Config.corpoEmailNovoPedido.isEmpty() ? Config.corpoEmailNovoPedido : PedidoEmailsConst.corpoEmailNovoPedido;

            Config.tituloEmailFaturamentoPedido = !Config.tituloEmailFaturamentoPedido.isEmpty() ? Config.tituloEmailFaturamentoPedido : PedidoEmailsConst.tituloEmailFaturamentoPedido;

            Config.corpoEmailFaturamentoPedido = !Config.corpoEmailFaturamentoPedido.isEmpty() ? Config.corpoEmailFaturamentoPedido : PedidoEmailsConst.corpoEmailFaturamentoPedido;

        }

        private static void completarInformacoesCobrancas(ConfiguracaoNotificacao Config) {

            Config.emailCobrancaContribuicao = !Config.emailCobrancaContribuicao.isEmpty() ? Config.emailCobrancaContribuicao : "hospedagem@sinctec.com.br";

            Config.tituloEmailCobrancaContribuicao = !Config.tituloEmailCobrancaContribuicao.isEmpty() ? Config.tituloEmailCobrancaContribuicao : "Cobrança #NOME_ORGANIZACAO#";

            Config.corpoEmailCobrancaContribuicao = !Config.corpoEmailCobrancaContribuicao.isEmpty() ? Config.corpoEmailCobrancaContribuicao : "Prezado(a) #NOME_ASSOCIADO#, <br /><br />Sua contribuição no valor de <strong>#VALOR#</strong> e com vencimento em <strong>#VENCIMENTO#</strong> está disponível para pagamento.<br /><br />Através do link: <a href=\"#LINK_PGTO#\"><strong>#LINK_PGTO#</strong></a><br/><br/>Se você preferir, acesse a área do associado com seu login e senha para visualizar suas contribuições em aberto. <br /><br />Caso o pagamento já tenha sido realizado, pedimos gentilmente que desconsidere este e-mail.<br /><br />Atenciosamente,<br /><br />";

            Config.tituloEmailPagamentoContribuicao = !Config.tituloEmailPagamentoContribuicao.isEmpty() ? Config.tituloEmailPagamentoContribuicao : "Confirmação de Pagamento";

            Config.corpoEmailPagamentoContribuicao = !Config.corpoEmailPagamentoContribuicao.isEmpty() ? Config.corpoEmailPagamentoContribuicao : "Olá Caro #NOME#!<br /><br />Recebemos o pagamento da contribuição no valor de <strong>#VALOR#</strong> e com vencimento em <strong>#VENCIMENTO#</strong>.<br /><br />Na área do associado, você poderá visualizar seu histórico de pagamentos e os respectivos recibos.<br /><br />#DADOS_PAGAMENTO#<br /><br />Atenciosamente,<br /><br />";

            Config.tituloEmailCobranca = !Config.tituloEmailCobranca.isEmpty() ? Config.tituloEmailCobranca : "Cobrança #NOME_ORGANIZACAO#";

            Config.corpoEmailCobranca = !Config.corpoEmailCobranca.isEmpty() ? Config.corpoEmailCobranca : "Prezado(a) #NOME_PESSOA#, <br /><br />Você já pode realizar o pagamento de: <strong> #DESCRICAO_PAGAMENTO#</strong>.<br /><br />Acesse: <a href=\"#LINK_PGTO#\"><strong>Link de pagamento</strong></a><br/>Caso você seja um membro da associação, acesse a área do associado com seu login e senha para visualizar seus pagamentos em aberto. <br /><br />Caso o pagamento já tenha sido realizado, pedimos gentilmente que desconsidere este e-mail.<br /><br />Atenciosamente,<br /><br />";

        }

        private static void completarInformacoesEventos(ConfiguracaoNotificacao Config) {

            Config.emailInscricaoEvento = !Config.emailInscricaoEvento.isEmpty() ? Config.emailInscricaoEvento : "hospedagem@sinctec.com.br";

            Config.corpoEmailNovaInscricaoEvento = !Config.corpoEmailNovaInscricaoEvento.isEmpty() ? Config.corpoEmailNovaInscricaoEvento : "Prezado(a) #NOME_INSCRITO#, <br /><br />Sua inscrição para o evento #TITULO_EVENTO# foi reservada com sucesso.<br /><br />Guarde esse e-mail com os dados de realização do evento.<br /><br /> #INSTRUCOES_PAGAMENTO#<br /><strong>Local:</strong> #LOCAL_EVENTO#<br /><strong>Endereço:</strong> #ENDERECO_EVENTO#<br /><strong>Dia:</strong> #DATAS_EVENTO#.<br /><br />Atenciosamente,<br /><br />";

            Config.corpoEmailCobrancaInscricaoEvento = !Config.corpoEmailCobrancaInscricaoEvento.isEmpty() ? Config.corpoEmailCobrancaInscricaoEvento : "";

            Config.corpoEmailPagamentoInscricao = !Config.corpoEmailPagamentoInscricao.isEmpty() ? Config.corpoEmailPagamentoInscricao : "Olá #NOME_INSCRITO#!<br /><br />Recebemos a confirmação do pagamento referente a inscrição do evento #TITULO_EVENTO#.<br /><br />Dados do Evento:<br /><br /><strong>Valor:</strong> #VALOR_INSCRICAO#<br /><strong>Local:</strong> #LOCAL_EVENTO#<br /><strong>Endereço:</strong> #ENDERECO_EVENTO#<br /><strong>Dia:</strong> #DATAS_EVENTO#.<br /><br />Atenciosamente,<br /><br />";

            Config.corpoEmailIsencaoInscricao = !Config.corpoEmailIsencaoInscricao.isEmpty() ? Config.corpoEmailIsencaoInscricao : "Olá #NOME_INSCRITO#!<br /><br />Você acaba de receber a isenção na inscrição do evento #TITULO_EVENTO#.<br /><br />#MOTIVO_ISENCAO#Dados do Evento:<br /><br /><strong>Local:</strong> #LOCAL_EVENTO#<br /><strong>Endereço:</strong> #ENDERECO_EVENTO#<br /><strong>Dia:</strong> #DATAS_EVENTO#.<br /><br />";

            Config.corpoEmailEnvioCerficadoEvento = !Config.corpoEmailEnvioCerficadoEvento.isEmpty() ? Config.corpoEmailEnvioCerficadoEvento : "";

        }

    }
}
