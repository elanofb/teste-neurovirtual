function jMObject() {

    //
    this.showSimpleDialog = function (message, title, onClose, className) {

        var btClass = "btn-success";
        if (className == 'bb-error') {
            btClass = "btn-danger";
        }

        if (className == 'bb-info') {
            btClass = "btn-primary";
        }

        bootbox.dialog({
            message: message,
            title: title,
            className: className,
            buttons: {
                main: {
                    label: "OK",
                    className: btClass,
                    callback: function () {
                        if (typeof(onClose) == 'function'){
                            onClose();
                        }
                    }
                }
            }
        });    
    };
    
    //
    this.info = function (message, onClose, title) {
        if (typeof (title) == 'undefined' || title == '') {
            title= "Informa&ccedil;&atilde;o!";
        }
        var className = "bb-info";
        this.showSimpleDialog(message, title, onClose, className);
    };
	

    //
    this.error = function (message, onClose, title) {
        if (typeof (title) == 'undefined' || title == '') {
            title= "Falha!";
        }
        var className = "bb-error";
        this.showSimpleDialog(message, title, onClose, className);
    };
	
    //
    this.success = function (message, onClose, title) {
        if (typeof (title) == 'undefined' || title == '') {
            title= "Sucesso!";
        }
        var className = "bb-success";
        this.showSimpleDialog(message, title, onClose, className);
    };
	
    //
    this.process = function (message, onClose, width) {
        alert('Not implementated');
    };
	
    //
    this.confirmation = function (message, onClickButtonYes, onClickButtonNo) {
        bootbox.dialog({
            message: message,
            title: "Confirma&ccedil;&atilde;o",
            buttons: {

                main: {
                    label: "Cancelar",
                    className: "btn-default",
                    callback: function () {
                        if (typeof (onClickButtonNo) == 'function') {
                            onClickButtonNo();
                        }
                    }
                },
                success: {
                    label: "OK",
                    className: "btn-primary",
                    callback: function () {
                        if (typeof (onClickButtonYes) == 'function') {
                            onClickButtonYes();
                        }
                    }
                }
            }
        });

    };
    

}

var jM = new jMObject();
