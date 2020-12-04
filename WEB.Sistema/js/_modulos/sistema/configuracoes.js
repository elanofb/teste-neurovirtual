function ObjConfiguracoes() {

    this.init = function () {
        this.setInputFile();
    }

    this.setInputFile = function () {
        $('#FileLogotipo').fileinput({
            showPreview: true,
            allowedPreviewTypes: ["image", "audio", "video", "text"]
        });
    };
}

var Configuracoes = new ObjConfiguracoes();

$(document).ready(function () {

    Configuracoes.init();
});