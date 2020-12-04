function ComboSelect(){
    
    this.init = function(){
        //$("select").selectBox();   
        ComboSelect.checkEditSelect();
    };
    
    //
    this.checkEditSelect = function(){
        //$('select[edit=false]').not("[value='']").selectBox("disable");
        //$('form[edit=false] select').selectBox("disable");
    };
    
     /**
     * Caso o elemento esteja configurado com plugin selectbox, faz a reinicialização do mesmo para pegar as mudanças do combo
     */
    this.resetSelectBox = function(element, enable){
        if(element.hasClass('selectBox')){
            element.selectBox("destroy");
            element.selectBox();
            if(enable){
                element.selectBox('enable');
            }else{
                element.selectBox('disable');
            }
        }
    }

     /**
     * Preenchimento de itens de um combo de seleção
     */
    this.loadSelectBox = function(combo, rows, selectedItem){
        combo.find("option").not("option:first").remove();
        combo.find("option:first").html(Vocabulary.loading);
        combo.find("option:first").removeAttr("selected");

        //ComboSelect.resetSelectBox(combo, false);
        var selected = false;
        $.each(rows, function(key, item){
            selected = (item.id == selectedItem? true: false);
            combo.append(new Option(item.name, item.id, selected));
        });
        combo.find("option:first").html(Vocabulary.select);

        //ComboSelect.resetSelectBox(combo, true);
    }

};

var ComboSelect = new ComboSelect();
$(document).ready(function(){
    ComboSelect.init();
});
