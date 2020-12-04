using FluentValidation.Attributes;
using DAL.Popups;

namespace WEB.Areas.Popups.ViewModels {

    [Validator(typeof(PopupFormValidation))]
    public class PopupForm {
        
        public HomePopup OHomePopup { get; set; }

        public PopupForm() { 
            this.OHomePopup = new HomePopup();
        }

    }

}