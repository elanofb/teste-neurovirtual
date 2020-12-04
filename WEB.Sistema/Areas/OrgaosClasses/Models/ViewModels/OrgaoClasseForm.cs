using FluentValidation.Attributes;
using DAL.OrgaosClasses;

namespace WEB.Areas.OrgaosClasses.ViewModels {

    [Validator(typeof(OrgaoClasseFormValidator))]
    public class OrgaoClasseForm {

        public OrgaoClasse OrgaoClasse { get; set; }

    }

}