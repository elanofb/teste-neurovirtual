function ClassWizardSinctec() {

    var nameCookieWizard = "current_wizard";
    var classBoxWizard = ".wizard-sinctec";
    var classCurrentItem = "current";
    

    //Metodo de inicializacao da classe
    this.init = function () {

        this.iniciarWizards();
    };

    //Iniciar tratamento para blocos em formato wizard
    this.iniciarWizards = function () {
        this.listenerLinkWizard();
        this.changeStep();
    }

    //Ouvinte para mudancas de aba
    this.listenerLinkWizard = function () {
        var linksWizard = $(classBoxWizard).find("ul.tablist > li > a");
        linksWizard.on('click', function () {
            var info = $(this).attr("href");
            setCurrentWizard(info);
            WizardSinctec.changeStep();
        });
    }

    //Capturar o bloco selecionado
    //Desativar todas as abas
    //Ativar somente a que possui o valor correto
    this.changeStep = function () {
        var currentStep = getCurrentWizard();
        if (currentStep == '' || typeof (currentStep) == 'undefined') {
            return;
        }

        $(classBoxWizard).find("ul.tablist > li").removeClass(classCurrentItem);
        $(classBoxWizard).find(".content-wizard").removeClass(classCurrentItem);

        $("a[href=#" + currentStep + "]").parent().addClass(classCurrentItem);
        $("div[data-role=" + currentStep + "]").addClass(classCurrentItem);
    }

    //Definir valor e salvar a aba do wizard em cookie
    var setCurrentWizard = function (info) {
        Cookies.set(nameCookieWizard, info.replace("#", ""), { expires: 7 });
    }

    //Definir valor e salvar a aba do wizard em cookie
    var getCurrentWizard = function () {
        var currentStep = Cookies.get(nameCookieWizard);
        console.log(currentStep);
        return currentStep;
    }
};

var WizardSinctec = new ClassWizardSinctec();

$(document).ready(function(){
    WizardSinctec.init();
});
