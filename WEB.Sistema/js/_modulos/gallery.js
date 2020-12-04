function GalleryDefault(){
    
    this.init = function(){
        
        //Ouvinte do evento de exclusão dos registros
        $("a.delete-file-gallery").on('click', function(){
            DefaultAction.action(this, 'deleteFile');
            return false;

        });
    }

};

var GalleryDefault = new GalleryDefault();
$(document).ready(function(){
    GalleryDefault.init();
});
