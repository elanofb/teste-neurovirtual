function ObjMaterialApoio() {

    this.init = function () {

        MaterialApoio.select2();
        
        MaterialApoio.carregarArquivos();

    }

    this.select2 = function () {
        $("#idCategoria").select2({
            width: 'resolve',
            dropdownCssClass: "bigdrop",
            allowClear: true,
            placeholder: "Selecione...",

            //Apresenta o link para cadastrar um novo tipo de material de apoio quando não encontrar resultados
            "language": {
                "noResults": function () {
                    return "Sem resultados. <a href='javascript:;' class='bold pull-right' onclick='TipoMaterialApoio.modalCategoria()'> &nbsp;&nbsp;<i class='fa fa-plus-o'></i> Adicionar Categoria</a>";
                }
            }, escapeMarkup: function (markup) { return markup; }
        });
    }
    
    this.carregarArquivos = function () {
        
        DefaultSistema.carregarConteudo($("#boxArquivosListar"), EditableCustom.listenerEditables);
        
    }

}

var MaterialApoio = new ObjMaterialApoio();

$(document).ready(function () {

    MaterialApoio.init();
});