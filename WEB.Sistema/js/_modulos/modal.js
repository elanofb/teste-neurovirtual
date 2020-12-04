function modalView(obj) {
    closeModalView();

    var dialog = $('<div id="dialogWindow"><p class="loading-bar"></p></div>').dialog({
        autoOpen: true,
        modal: true,
        position: ['top', 100],
        hide: 'fade'
    });

    var url =  $(obj).attr("href");
    var extra_param = $(obj).attr("param");
    var params = eval('(' + extra_param + ')');
    var title = $(obj).attr("title");
    if (title == '' || title == undefined) { title = Vocabulary.project_title; }

    dialog.load(url, params,
        function (responseText, textStatus, XMLHttpRequest) {
            $("#dialogWindow").find("input:text").setMask();
            $("#dialogWindow").find("input[type='text'][alt='date']").datepicker();

            dialog.dialog({
                close: function (event, ui) {
                    dialog.remove();
                    closeModalView();
                }
            });
        }
    );
    return false;
}

function closeModalView(){
    $("#dialogWindow").dialog("destroy");
    $("#dialogWindow").remove();
}

