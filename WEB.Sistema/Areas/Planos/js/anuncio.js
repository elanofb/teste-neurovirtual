function ObjAnuncio() {

    this.init = function () {

        CKEDITOR.replace('editor',
            {
                toolbar: 'Custom',
                height: 700
            }
        );    };

};

var Anuncio = new ObjAnuncio();

$(document).ready(function () {
    Anuncio.init();
});
