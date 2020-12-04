function ClassCorreio() {
    var idBoxEmails = "#boxCaixaEmails";
    var idBoxModal = "#myModal";

    this.init = function () {
        this.timerNovosEmails();
    };

    //Iniciar plugin popover
    this.iniciarEditores = function () {

        $(".froala-editor").each(function() {

            $(this).froalaEditor({
                language: 'pt_br',
                height: 200,
                toolbarButtons: ['bold', 'italic', 'underline', '|', 'color', '|', 'align', 'formatOL', 'formatUL', 'insertHR', 'insertLink', 'html'],
                toolbarButtonsXS: ['bold', 'italic', 'underline', '|', 'color', '|', 'align', 'formatOL', 'formatUL', 'insertHR', 'insertLink', 'html'],
                toolbarButtonsSM: ['bold', 'italic', 'underline', '|', 'color', '|', 'align', 'formatOL', 'formatUL', 'insertHR', 'insertLink', 'html'],
                toolbarButtonsMD: ['bold', 'italic', 'underline', '|', 'color', '|', 'align', 'formatOL', 'formatUL', 'insertHR', 'insertLink', 'html']
            });


        });

    };

    /**
    * Rotina timer para atualizar os e-mails
    */
    this.timerNovosEmails = function () {
        this.buscarNovosEmails(false);

        setInterval(function () {
            CorreioInterno.buscarNovosEmails(true);
        }, (1000 * 60 * 10));

    };

    /**
    * Buscar novos e-mails no servidor
    */
    this.buscarNovosEmails = function (flagBuscarEmails) {
        $("#btAtualizarCaixa").addClass("fa-spin");
        $("#btAtualizarCaixa").attr("title", "Buscando novos e-mails...").tooltip('show');

        var valorBusca = $("#buscarEmail").val();
        var url = $("#boxCaixaEmails").data("url");

        $.get(url+"?flagBuscarEmails=" + flagBuscarEmails + "&valorBusca=" + valorBusca,
            function (response) {
                $(idBoxEmails).html(response);
                DefaultSistema.iniciarLinksAcao();
                $("[data-toggle=tooltip]").tooltip();
                CorreioInterno.atualizarTotalEmails();
                CorreioInterno.iniciarEditores();
            }
        );
        
    };

    /**
    * Evento disparado clicar em algum dos botões de paginação
    */
    this.onSuccessPaginacao = function (response) {
        $(idBoxEmails).loadingOverlay('remove');
        $(idBoxEmails).find("[data-toggle=tooltip]").tooltip();
    };

    /**
    * Abrir modal para visualizar um e-mail
    */
    this.visualizarNovoEmail = function (elemento) {
        $(idBoxEmails).loadingOverlay();
        CorreioInterno.fecharModal();

        var url = $(elemento).attr("data-url");
        var id = $(elemento).attr("data-id");

        $.get(url, {}, function (response) {
            var Modal = $(response).modal({ title: "Nova Mensagem" });

            Modal.on('shown.bs.modal', function (e) {
                $(elemento).button("reset");
                DefaultSistema.iniciarLinksAcao();
                CorreioInterno.iniciarEditor();
                CorreioInterno.registrarVisualizacao(id);
                $(idBoxEmails).loadingOverlay('remove');
            });

            Modal.on('hidden.bs.modal', function (e) {
                CorreioInterno.fecharModal();
            });
        });
    };

    /**
    * Enviar evento de visualizacao do e-mail
    */
    this.registrarVisualizacao = function (id) {
        if (typeof (id) == "undefined") {
            return;
        }

        var url = $("#tbCaixaEntrada").data("url-visualizacao");

        $.get(url, { id: id }, function (response) {
            //console.log(response);
            $("#tbCaixaEntrada").find("tr.email-" + id).find("td").removeClass("nao-lido");
            CorreioInterno.atualizarTotalEmails();
        });
    }

    /**
    * Marcar todos os e-mails da página
    */
    this.marcarEmails = function () {
        var elemento = "button.checkbox-toggle";

        if ($(elemento).find("i").hasClass("fa-square-o")) {
            $(elemento).find("i").removeClass("fa-square-o").addClass("fa-check-square-o");

            $("input[name=checkbox-email]").each(function () {
                $(this).prop("checked", "checked");
            });

        } else {
            $(elemento).find("i").removeClass("fa-check-square-o").addClass("fa-square-o");

            $("input[name=checkbox-email]").each(function () {
                $(this).removeAttr("checked");
            });
        }
    }

    /**
    * Fechar a janela de leitura 
    */
    this.fecharModal = function () {
        $(idBoxModal).modal("hide");
        $(idBoxModal).remove();
        $(".modal-backdrop").remove();
        $(".modal").remove();
    }

    /**
    * Enviar mensagens em lote para lixeira
    */
    this.encaminharEmLote = function () {
        var arraySelecionados = $("input[name=checkbox-email]:checked");
        var qtdeSelecionados = arraySelecionados.length;

        if (qtdeSelecionados == 0) {
            jM.error("Selecione qual mensagem deseja encaminhar.");
            return false;
        }

        if (qtdeSelecionados > 1) {
            jM.error("Encaminhe apenas uma mensagem de cada vez.");
            return false;
        }

        $(arraySelecionados).each(function () {
            var id = $(this).val();
            var elemento = $('<button/>', { 'data-id-encaminhamento': id, 'data-url': 'criar-novo-email' });
            CorreioInterno.criarNovoEmail(elemento);

        });

    }

    /**
    * Enviar mensagens em lote para lixeira
    */
    this.enviarParaLixeiraLote = function () {
        var arraySelecionados = $("input[name=checkbox-email]:checked");
        var qtdeSelecionados = arraySelecionados.length;
        if (qtdeSelecionados == 0) {
            jM.error("Selecione quais mensagens devem ser removidas.");
            return false;
        }

        var fYes = function () {
            $(idBoxEmails).loadingOverlay();
            $(arraySelecionados).each(function () {
                CorreioInterno.enviarParaLixeira($(this).val(), false);
            });

            CorreioInterno.buscarNovosEmails();
            $(idBoxEmails).loadingOverlay('remove');
        }
        
        jM.confirmation("Confirma o envio dessa(s) mensagen(s) para a lixeira?", fYes, function () { return false; });
    }

    /**
    * Enviar mensagem para lixeira
    */
    this.enviarParaLixeira = function (id, flagConfirmacao) {
        var fYes = function () {
            $.post("enviar-para-lixeira",
                { id: id },
                function (response) {

                    //Se nao estiver dentro do loop da remoção em lote, atualiza os e-mails.
                    if (flagConfirmacao == true) {
                        CorreioInterno.buscarNovosEmails();
                    }
                    CorreioInterno.fecharModal();
                }
            );
        }

        if (flagConfirmacao === true) {
            jM.confirmation("Confirma o envio da mensagem para a lixeira?", fYes, function () { return false; });
        } else {
            fYes();
        }
    }


    /**
    * Restaurar Mensagem da Lixeira
    */
    this.restaurarMensagem = function () {

        var arraySelecionados = $("input[name=checkbox-email]:checked");
        var qtdeSelecionados = arraySelecionados.length;
        if (qtdeSelecionados == 0) {
            jM.error("Selecione quais mensagens deseja restaurar.");
            return false;
        }

        var fYes = function () {
            $(arraySelecionados).each(function () {
                var id = $(this).val();

                $.post("restaurar-mensagem",
                { id: id },
                function (response) {
                    console.log(response);
                });
            });
            CorreioInterno.buscarNovosEmails();
        }

        jM.confirmation("Confirma a recupera&ccedil;&atilde;o dessas mensagens?", fYes, function () { return false; });
    }

    /**
    * Abrir modal para escrever um novo e-mail
    */
    this.criarNovoEmail = function (elemento) {
        var url = $(elemento).attr("data-url");
        var idEmailEncaminhamento = $(elemento).attr("data-id-encaminhamento");
        var idEmailResposta = $(elemento).attr("data-id-resposta");

        $.get(url,
            { idEmailEncaminhamento: idEmailEncaminhamento, idEmailResposta: idEmailResposta },
            function (response) {
                var Modal = $(response).modal({ title: "Nova Mensagem" });

                Modal.on('shown.bs.modal', function (e) {
                    $(elemento).button("reset");
                    CorreioInterno.onSuccessNovoEmail();
                });

                Modal.on('hidden.bs.modal', function (e) {
                    CorreioInterno.fecharModal();
                });
        });
    };


    /**
    * Buscar e-mails para o autocomplete dos destinos
    */
    this.iniciarBuscaEmails = function () {
        $(".buscar-emails").each(function () {

            var combo = $(this);
            var urlBusca = combo.attr("data-url");
            var placeholderTexto = combo.attr("placeholder");

            combo.select2({
                placeholder: placeholderTexto,
                minimumInputLength: 3,
                allowClear: true,
                multiple: true,
                ajax: {
                    url: urlBusca,
                    dataType: "json",
                    data: function (params) {
                        return {
                            term: params.term, // search term
                            page: params.page
                        };
                    },
                    delay: 300,
                    processResults: function (data, page) {
                        return {
                            results: data.listaEmails
                        };
                    },
                    cache: false
                },
                //Allow manually entered text in drop down.
                createSearchChoice: function (term, data) {
                    if ($(data).filter(function () {
                      return (this.text == null || this.text.localeCompare(term) === 0) ;
                    }).length === 0) {
                        return { id: term, text: term };
                    }
                },
                language: "pt-BR",
                tags: true
            });

        });
    };

    /**
    * Iniciar o editor utilizado para digitar a mensagem de um novo e-mail
    */
    this.iniciarEditor = function () {
        $('#editor-email').wysihtml5({
            "font-styles": true, //Font styling, e.g. h1, h2, etc
            "color": true, //Button to change color of font
            "emphasis": true, //Italics, bold, etc
            "textAlign": true, //Text align (left, right, center, justify)
            "lists": true, //(Un)ordered lists, e.g. Bullets, Numbers
            "blockquote": false, //Button to insert quote
            "link": false, //Button to insert a link
            "table": false, //Button to insert a table
            "image": false, //Button to insert an image
            "video": false, //Button to insert video
            "html": false //Button which allows you to edit the generated HTML
        });
    };


    /**
    * Evento disparado após o carregamento do ajax form de envio do e-mail
    */
    this.onSuccessNovoEmail = function (response) {
        DefaultSistema.iniciarLinksAcao();
        CorreioInterno.iniciarEditor();
        CorreioInterno.iniciarBuscaEmails();

        if (response == "OK") {
            CorreioInterno.fecharModal();
            jM.success("A mensagem foi enviada com sucesso!");
            CorreioInterno.atualizarTotalEmails();
        }

    };


    /**
    * Exibir mensagem de loading após click no botão de envio de novo e-mail
    */
    this.exibirLoad = function (elemento) {
        $(elemento).button("loading");
        setTimeout(function () {
            $(elemento).button("reset");
        }, 8000);
    }


    /**
    * Atualizar totais de e-mail
    */
    this.atualizarTotalEmails = function () {

        var url = $("#listaTotaisEmails").data("url");

        $.get(url, {}, function (response) {
            $("#lblCaixaEntrada").html(response.qtdeNaoLidos);
            $("#lblLixeira").html(response.qtdeLixeira);
            //$("#").html(response.qtdeEnviados);
        });
    }

    /**
    * Success form
    */
    this.onSuccessForm = function (response) {

        console.log(response);

        var targetBox = $("#boxAreaTrabalho");
        
        if (response.error == undefined) {

            toastr.options.positionClass = "toast-top-right";

            toastr.error("Preencha os campos corretamente.", 'Erro!', { timeOut: "8000" });

            if (targetBox != '' && targetBox != 'undefined') {

                $(targetBox).html(response);

            }

            CorreioInterno.iniciarEditores();
            
            return;
        }

        if (response.error == false){
            
            jM.success(response.message, function () {
                location.reload();
            });
            
            return;
            
        }

        jM.error(response.message);
        
        DefaultSistema.reiniciarBotao();

    }

};

var CorreioInterno = new ClassCorreio();

$(document).ready(function () {
    CorreioInterno.init();
});