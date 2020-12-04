function ObjInstituicao() {

    this.init = function () {

    };

    this.loadTitulo = function (selectedCity) {
        var codInstituicao = $("select[rel=idInstituicao]").val();

        var callback = function (data) {

            var combo = $("select[rel=idTipoTitulo]");
            combo.find("option").not("option:first").remove();
            combo.find("option:first").html(Vocabulary.loading);
            combo.find("option:first").removeAttr("selected");
            

            if (data.length > 0) {

                var selected = false;
                $.each(data, function (key, item) {
                    selected = (item.id == selectedCity ? true : false);
                    combo.append(new Option(item.descricao, item.id, selected));

                });
                
                combo.val(selectedCity);
            }

            combo.find("option:first").html(Vocabulary.select);

        }

        Ajax.init($("#baseUrlGeral").val() + 'tipotitulo/carregatitulo', { "idInstituicao": codInstituicao }, callback, null);
    };

}

var Instituicao = new ObjInstituicao();

$(document).ready(function () {
    Instituicao.init();
});

