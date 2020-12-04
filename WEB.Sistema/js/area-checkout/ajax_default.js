var ObjAjax = function () {

    this.vars = {
        method: 'post',
        returnType: 'json',
        callback: null,
        alertMessageDefault: null,
        urlAction: '',
        params: {}
    }

    this.init = function (url, params, callback, confirm, method, returnType, alertMessageDefault) {
        this.vars.urlAction = url;
        this.vars.params = params;
        this.vars.callback = (callback == '' || callback == undefined ? null : callback);
        this.vars.alertMessageDefault = (alertMessageDefault === false ? false : true);
        this.vars.method = (method == '' || method == undefined ? 'post' : method);
        this.vars.returnType = (returnType == '' || returnType == undefined ? 'json' : returnType);
        
        if (confirm != null && confirm != 'undefined') {
            fYes = function () { Ajax.send(); };
            fNo = function () { return false; };
            jM.confirmation(confirm, fYes, fNo);
        } else {
            this.send();
        }
    }

    this.send = function () {
        var options = this.vars;
        $.ajax({
            type: this.vars.method,
            data: this.vars.params,
            url: this.vars.urlAction,
            dataType: this.vars.returnType,
            traditional: true,
            success: function (data) {
                //Se não houver função callback atribui-se "false"
                if (options.callback == null) {
                    options.callback = function (data) { return false; };
                }
                //alert((options.alertMessageDefault == true));
                if (options.alertMessageDefault === true) {
                    if (data == null) {
                        options.callback(data);
                    } else {
                        if ((data.error != 'undefined' && data.error == true) || (data.flagError != 'undefined' && data.flagError == true)) {

                            if (typeof (data.message) != 'undefined') {
                                jM.error(data.message);
                            } else if (data.listaErros != undefined && data.listaErros.length > 0) {
                                jM.error(data.listaErros.join("<br/>"));
                            } else {
                                jM.error("Erro durante a opera&ccedil;&atilde;o");
                            }
                        } else {
                            //Se houver mensagem de retorno abre-se o dialog e depois executa a função callback, caso contrário apenas dispara a função
                            if (typeof (data.message) != 'undefined') {
                                jM.success(data.message);
                            } else if (data.listaErros != undefined && data.listaErros.Length > 0) {
                                jM.success(data.listaErros.join("<br/>"));
                            }
                            options.callback(data);
                        }
                    }
                } else {
                    options.callback(data);
                }
            },
            error: function (err) {
                console.dir(err);
                //alert(err.message);
                //jM.error('ERROR '+err.toString());
            }
        });
    };


    //
    this.updateAjaxList = function (element, url, params) {
        var func = function (data) {
        	$(element).html(data);
        	$(element).find("input:text").setMask();
        	DefaultSistema.iniciarLinksAcao();
        	try {
        		$(element).find(".datepicker").datepicker({
        			format: "dd/mm/yyyy",
        			todayBtn: "linked",
        			language: "pt-BR",
        			orientation: "bottom left",
        			autoclose: true,
        			todayHighlight: true
        		});
        	} catch (e) {
        	}
        };
        var Assync = new ObjAjax();
        Assync.init($("#baseUrlGeral").val() + url, params, func, null, "post", "html", false);
    };


}

var Ajax = new ObjAjax();