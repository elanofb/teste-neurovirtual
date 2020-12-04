var AppAutoComplete = {
    
    Class: function(){
        
        this.completeCitiesBR = function(elementSufix, url){
            var objUf = $("[rel=autocompleteUf"+ elementSufix+"]");
            var objCityName = $("[rel=autocomplete"+ elementSufix+"]");
            var objCityId = $("[rel=autocompleteId"+ elementSufix+"]");

            if(url == '' || url == 'undefined'){
                alert('Error: Attr url is not defined in input autocomplete.');
            }
            
            $(objCityName).autocomplete({

                source: function(request, response) {
                    $.ajax({url: url, dataType: "json",
                        data: {term : request.term, estado_id : objUf.val()},
                        success: function(data) {
                            if(objUf.val() == "0" || objUf.val() == ""){
                                jM.error(Vocabulary.select_state);
                                $(objUf).focus();
                            }else{
                                response(data);
                            }
                        }
                    });
                },
                minLength: 3,
                delay: 1,
                select: function(item, ui){
                    $(objCityId).val(ui.item.id);
                }
            });
        }
    
        
    }
};
var AppComplete = new AppAutoComplete.Class();

$(document).ready(function(){
   $("input[type=text][rel*=autocomplete]").each(function(){
       var sufixo = $(this).attr('rel').replace('autocomplete', '');
       var url = $(this).attr('url');
       AppComplete.completeCitiesBR(sufixo, url);
   });
   
});
