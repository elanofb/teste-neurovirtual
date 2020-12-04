var AssociadoAprovacaoDocumentos = new function() {
    this.init = () => {
        console.log("associados-aprovacao-documentos loaded!!");
    };
    
    this.aprovarDocumentos = (elemen) => {
        
        var url = $(elemen).data("url");
        
        var funcYes = () => {
            $.get(
                url,
                (response) => {
                    if (response.error === true) {
                        jM.error(response.message);
                    }
                    if (response.error === false) {
                        
                        jM.success(response.message);
                        location.reload();
                        
                    }
                }
            );
        };
        
        jM.confirmation("Deseja confirmar a operação?", funcYes, () => false);
        
    };
}();

AssociadoAprovacaoDocumentos.init();
