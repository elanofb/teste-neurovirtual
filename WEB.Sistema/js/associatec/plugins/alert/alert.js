function jMObject() {

    this.textButtonYes = "Ok";

    this.textButtonNo = "Cancelar";

    this.titleConfirmation = "Confirma&ccedil;&atilde;o";

    //
    this.showSimpleDialog = function (message, title, onClose, className) {
        
        swal({
            title: title,
            html: message,
            type: className
        }).then(function () {

            if (typeof(onClose) == 'function'){
                onClose();
            }

        });

    };

    //
    this.info = function (message, onClose, title) {
        if (typeof (title) == 'undefined' || title == '') {
            title= "Informa&ccedil;&atilde;o!";
        }
        var className = "warning";
        this.showSimpleDialog(message, title, onClose, className);
    };

    //
    this.error = function (message, onClose, title) {
        if (typeof (title) == 'undefined' || title == '') {
            title= "Falha!";
        }
        var className = "error";
        this.showSimpleDialog(message, title, onClose, className);
    };

    //
    this.success = function (message, onClose, title) {
        if (typeof (title) == 'undefined' || title == '') {
            title= "Sucesso!";
        }
        var className = "success";
        this.showSimpleDialog(message, title, onClose, className);
    };

    //
    this.process = function (message, onClose, width) {
        alert('Not implementated');
    };

    //
    this.confirmation = function (message, onClickButtonYes, onClickButtonNo, onEscape) {

        swal({

            title: jM.titleConfirmation,
            html: message,
            type: 'question',
            showCancelButton: true,
            confirmButtonText: jM.textButtonYes,
            cancelButtonText: jM.textButtonNo,
            buttonsStyling: true

        }).then(function () {
            
            if (typeof (onClickButtonYes) == 'function') {
                onClickButtonYes();
            }

        }, function (dismiss) {

            // dismiss can be 'cancel', 'overlay',
            // 'close', and 'timer'
            if (dismiss === 'overlay' || dismiss === 'close') {
                
                if (typeof (onEscape) == 'function') {
                    onEscape();
                }

                DefaultSistema.reiniciarBotao();

            }

            if (dismiss === 'cancel') {

                if (typeof (onClickButtonNo) == 'function') {
                    onClickButtonNo();
                }

            }

        })

    };

}

var jM = new jMObject();