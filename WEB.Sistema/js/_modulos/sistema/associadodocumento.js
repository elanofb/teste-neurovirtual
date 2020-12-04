function ObjAssociadoDocumento() {

    this.init = function () {
        var idCategoriaDocumento = $("select[rel=idCategoriaDocumento]").val();
        var idTipoDocumento = $("select[rel=idTipoDocumento]").val();

        AssociadoDocumento.loadDocuments(idTipoDocumento);
    };

    this.loadDocuments = function (selectedDocument) {
        var idCategoriaDocumento = $("select[rel=idCategoriaDocumento]").val();

        var callback = function (data) {
            var combo = $("select[rel=idTipoDocumento]");
            
            if (data.length > 0) {
                combo.find("option").not("option:first").remove();
                combo.find("option:first").html(Vocabulary.loading);
                combo.find("option:first").removeAttr("selected");

                var selected = false;
                $.each(data, function (key, item) {
                    selected = (item.id == selectedDocument ? true : false);
                    combo.append(new Option(item.nome, item.id, selected));

                });
                combo.find("option:first").html(Vocabulary.select);
                combo.val(selectedDocument);

                $(combo).select2();
            } else {
                combo.find("option").not("option:first").remove();
                combo.find("option:first").html(Vocabulary.select);
                $(combo).select2();
            }
        }

        Ajax.init($("#baseUrlGeral").val() + 'associadodocumento/getTiposDocumento', { "idCategoriaDocumento": idCategoriaDocumento }, callback, null);
    }
};

var AssociadoDocumento = new ObjAssociadoDocumento();

$(document).ready(function () {
    AssociadoDocumento.init();
});