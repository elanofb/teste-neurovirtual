using System;
using BLL.Institucionais;
using FluentValidation;

namespace WEB.Areas.Institucionais.ViewModels
{
    public class TipoConvenioValidator : AbstractValidator<TipoConvenioForm>
    {
        //Atributos
        private ITipoConvenioBL _TipoConvenioBL;

        //Propriedades
        private ITipoConvenioBL OTipoConvenioBL => (this._TipoConvenioBL = this._TipoConvenioBL ?? new TipoConvenioBL());

        //Construtor
        public TipoConvenioValidator()
        {

            RuleFor(x => x.TipoConvenio.descricao)
               .NotEmpty().WithMessage("O campo 'descrição' é obrigatório.");

            RuleFor(x => x.TipoConvenio.descricao)
                .Must((x, descricao) => !this.existe(x))
                .WithMessage("Já existe um registro cadastrado com essa descrição.");

        }

        //Validação de duplicados
        public bool existe(TipoConvenioForm ViewModel) {

            int idDesconsiderado = UtilNumber.toInt32(ViewModel.TipoConvenio.id);
            int idOrganizacao = UtilNumber.toInt32(ViewModel.TipoConvenio.idOrganizacao);
            return this.OTipoConvenioBL.existe(UtilString.cleanAccents(ViewModel.TipoConvenio.descricao).ToLower().Replace(" ", "_"), idDesconsiderado);
        }
    }
}