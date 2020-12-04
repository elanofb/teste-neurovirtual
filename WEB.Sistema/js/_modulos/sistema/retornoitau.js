function ObjRetornoItau(){
    
	this.init = function () {
		$('#formRetornoItau').ajaxForm();

    };
    

    /**
     * Carregamento 
     */
	this.submitRetorno = function () {
		$('#formRetornoItau').ajaxSubmit({
				dataType: 'json',
				success: function (data) {
					console.log(data);
				}
		});

	};



};

var RetornoItau = new ObjRetornoItau();

$(document).ready(function(){
	RetornoItau.init();
});
