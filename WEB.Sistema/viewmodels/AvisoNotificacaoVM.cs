using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentValidation;
using FluentValidation.Attributes;
using DAL.Entities;
using DAL.Permissao;

namespace WEB.ViewModels {

    [Validator(typeof(AvisoNotificacaoVMValidator))]
    public class AvisoNotificacaoVM {
        public int id { get; set; }
        //
        public string titulo { get; set; }
        public DateTime? dtEnvio { get; set; }
        public DateTime? dtMensagem { get; set; }
        public DateTime? dtCadastro { get; set; }
        public DateTime? dtInicioEnvio {get; set; }
        public string notificacao { get; set; }
        public string ativo { get; set; }

        public string flagMobile { get; set; }

        //
        public string flagTipoEnvioAssociado { get; set; }
        public string flagTipoEnvioUsuario { get; set; }

        //
       // public IList<NotificacaoPessoa> listaTodosAssoc { get; set; }
       // public IList<UsuarioSistema> listaTodosUsusarios { get; set; }

        public AvisoNotificacaoVM() {
       //     this.listaTodosAssoc = new List<NotificacaoPessoa>();
       //     this.listaTodosUsusarios = new List<UsuarioSistema>();
        }
    }

    public class AvisoNotificacaoVMValidator : AbstractValidator<AvisoNotificacaoVM> {
        public AvisoNotificacaoVMValidator() {
            RuleFor(x => x.titulo).NotEmpty().WithMessage("O campo título deve ser preenchido");
            RuleFor(x => x.dtInicioEnvio).NotEmpty().WithMessage("O campo data envio deve ser preenchido");
            RuleFor(x => x.notificacao).NotEmpty().WithMessage("O campo mensagem deve ser preenchida");

            RuleFor(x => x.flagTipoEnvioAssociado)
                .NotEmpty().When(x => String.IsNullOrEmpty(x.flagTipoEnvioUsuario)).WithMessage("O campo 'Envio para associados' deve ser preenchido quando o 'Envio para usuario sistema' estiver vazio.");

            RuleFor(x => x.flagTipoEnvioUsuario)
                .NotEmpty()
                .When(x => String.IsNullOrEmpty(x.flagTipoEnvioAssociado))
                .WithMessage("O campo 'Envio para usuario sistema' deve ser preenchido quando o 'Envio para associados' estiver vazio.");

            RuleFor(x => x.flagMobile).NotEmpty().When(x => String.IsNullOrEmpty(x.flagMobile)).WithMessage("O campo 'Enviar para app mobile' deve ter uma opção escolhida. ");

            When(x => x.flagTipoEnvioAssociado == "ESP" , () => {
            RuleFor(x => x.flagTipoEnvioAssociado)
                    .Must((VM,valor) => conterAssociado(VM))
                    .WithMessage("Você deve adiconar pelo menos um associado na aba Associados Específicos.");
            });

            When(x => x.flagTipoEnvioUsuario == "USU" || x.flagTipoEnvioUsuario == "PER" , () => {
            RuleFor(x => x.flagTipoEnvioUsuario)
                    .Must((VM,valor) =>  conterUsuario(VM))
                    .WithMessage("Você deve adiconar pelo menos um usuário na aba Usuários Específicos ou Perfis Especificos.");
            });

        }

        private bool conterAssociado(AvisoNotificacaoVM VM) {
            return  SessionNotificacoes.getListAssociadosEspecificos().Count > 0;
        }
        
        private bool conterUsuario(AvisoNotificacaoVM VM) {
            return (SessionNotificacoes.getListUsuariosEspecificos().Count > 0 || SessionNotificacoes.getListPerfisEspecificos().Count > 0);
        }
    }

}