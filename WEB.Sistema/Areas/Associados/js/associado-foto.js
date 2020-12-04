function AssociadoFotoClass(){
    

    //Metodo de inicializacao dos plugins
    this.init = function () {

		this.iniciarFileApi();

    };
    

    //Iniciar plugin para upload de foto do associado
    this.iniciarFileApi = function () {

        var baseUrl = new String($("#baseUrlGeral").val()).concat("associados/associadofoto/");
        var idAssociado = $("#Associado_id").val();

        $('#userpic').fileapi({
            url: baseUrl.concat("salvar-foto"),
            data: { "idAssociado": idAssociado },
            accept: 'image/*',
            imageSize: { minWidth: 150, minHeight: 200 },
            paramName: "FileUpload",
            elements: {
                active: { show: '.js-upload', hide: '.js-browse' },
                //preview: {
                //    el: '.js-preview',
                //    width: 200,
                //    height: 200
                //},
                progress: '.js-progress'
            },
            onSelect: function (evt, ui) {

                if (ui.other[0] && typeof ui.other[0].errors !== "undefined") {
                    var errors = ui.other[0].errors;

                    if (errors.minWidth || errors.minHeight) {
                        jM.error("A imagem deve ter o tamanho m&iacute;nimo de 150px x 200px.");
                    }
                }

                var file = ui.files[0];

                if (file) {
                    var url = baseUrl.concat("modal-corte-imagem")

                    $.get(url, function (data) {
                        var Modal = $(data).modal();
                        Modal.on("shown.bs.modal", function (e) {

                            var maxHeight = $(window).height() - 275;
                            $(".popup__body").css("max-height", new String(maxHeight).concat("px"));
                            $(".popup__body").css("overflow-y", "scroll");

                            $('.js-img').cropper({
                                file: file,
                                bgColor: '#fff',
                                maxSize: [863, $(window).height() - 120],
                                minSize: [150, 200],
                                selection: '90%',
                                onSelect: function (coords) {
                                    $('#userpic').fileapi('crop', file, coords);
                                }
                            });

                            $(".js-upload").on("click", function () {
                                $(Modal).modal("hide");
                                $('#userpic').fileapi('upload');
                            })
                        });
                        Modal.on("hide.bs.modal", function (e) {

                        });
                    })
                }
            },
            onComplete: function (evt, uiEvt) {
                var erro = uiEvt.error;

                if (erro == false) {
                    var url = baseUrl.concat("partial-foto/").concat(idAssociado);
                    $.get(url, function (data) {
                        $("#boxFoto").html(data);
                        AssociadoFoto.iniciarFileApi();
                    })
                }
            }
        });
    }

};

var AssociadoFoto = new AssociadoFotoClass();

$(document).ready(function(){

    AssociadoFoto.init();

});
