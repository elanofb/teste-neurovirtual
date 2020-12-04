
$.extend($.fn.dataTable.defaults, {
    "iDisplayLength": 20,
    "aLengthMenu": [10, 20, 25, 50, 100, 1000],
    "sDom": '<"top">rt<"bottom"lip><"clear">',

    "ordering": true
});

//Inicialização do datatable
function iniciarDataTable() {
    if ($(".tSortable").length > 0) {

        $(".tSortable").each(function () {
            var table = $(this);
            configurarTabela(table);
        });
    }
};

//Configuracao de cada tabela no data table
function configurarTabela(table) {
    var ordemIndex = table.attr("data-ordem-index");
    var ordemDirecao = table.attr("data-ordem-dir");
    var indexColunasHidden = table.attr("data-colunas-hidden");
    var indexColunasWidth = table.attr("data-colunas-width");
    var flagPaginacao = table.attr("data-pager");

    if (typeof (ordemIndex) == 'undefined') ordemIndex = 1;
    if (typeof (ordemDirecao) == 'undefined') ordemIndex = 'asc';
    if (typeof (indexColunasHidden) == 'undefined') indexColunasHidden = '[]';
    if (typeof (indexColunasWidth) == 'undefined') indexColunasWidth = '[]';
    if (typeof (flagPaginacao) == 'undefined') flagPaginacao = true;

    //Definição do Data Table
    var oDataTable = table.DataTable({
        "order": [ordemIndex, ordemDirecao],
        paging: (flagPaginacao === "false" ? false : true),
        autoWidth: false,
        "language": {
            "sEmptyTable": "Nenhum registro encontrado",
            "sInfo": "Mostrando de _START_ at&eacute; _END_ de _TOTAL_ registros",
            "sInfoEmpty": "Mostrando 0 at&eacute; 0 de 0 registros",
            "sInfoFiltered": "(Filtrados de _MAX_ registros)",
            "sInfoPostFix": "",
            "sInfoThousands": ".",
            "sLengthMenu": "_MENU_ Resultados por p&aacute;gina",
            "sLoadingRecords": "Carregando...",
            "sProcessing": "Processando...",
            "sZeroRecords": "Nenhum registro encontrado",
            "sSearch": "Pesquisar",
            "oPaginate": {
                "sNext": "Pr&oacute;ximo",
                "sPrevious": "Anterior",
                "sFirst": "Primeiro",
                "sLast": "&aUcute;ltimo"
            },
            "oAria": {
                "sSortAscending": ": Ordenar colunas de forma ascendente",
                "sSortDescending": ": Ordenar colunas de forma descendente"
            }
        }
    });

    //Verificar colunas ocultas por padrão
    var arrayColunasHidden = eval("(" + indexColunasHidden + ")");
    $.each(arrayColunasHidden, function (index, value) {
        oDataTable.column(value).visible(false);
    });

}

//
$(document).ready(function () {
    iniciarDataTable();
});