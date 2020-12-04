var DefaultJS = {

    Class: function(){

        this.init = function(){
            //Atribuição de Máscaras
            $('input:text').setMask();

            //$('select').selectBox();
            $('ul.sf-menu').superfish();

            //
            $("[data-toggle=tooltip").tooltip();

        }

        //
        this.checkClasses = function(){

            if(typeof Vocabulary == 'undefined'){
                alert('Error! Vocabulary Class not found!');
            }

        }

    }
};
var Default = new DefaultJS.Class();

$(document).ready(function(){
    Default.init();
});


