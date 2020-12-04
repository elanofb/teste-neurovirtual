using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using BLL.Notificacoes;
using BLL.Services;
using DAL.Associados;
using DAL.Emails;
using DAL.Notificacoes;
using DAL.Contribuicoes;
using BLL.Core.Events;
using BLL.Notificacoes.Events;
using BLL.Organizacoes;
using DAL.Permissao;
using DAL.Repository.Base;

namespace BLL.AvisosNotificacoes.Services {

    public class MensageiroAppFacadeBL : DefaultBL, IMensageiroAppFacadeBL {

        // Atributos
        private IPessoaDeviceConsultaBL _IPessoaDeviceConsultaBL;

        // Propriedades
        private IPessoaDeviceConsultaBL OPessoaDeviceConsultaBL => _IPessoaDeviceConsultaBL = _IPessoaDeviceConsultaBL ?? new PessoaDeviceConsultaBL();
        
        //
        public UtilRetorno enviar(NotificacaoSistema ONotificacao) {

            var idsPessoas = ONotificacao.listaPessoa.Select(x => x.idPessoa).ToList();
            
            var listaDispositivos = this.OPessoaDeviceConsultaBL.listar("").Where(x => idsPessoas.Contains(x.idPessoa))
                                        .Select(x => new {
                                            x.idPessoa, x.idDevice, x.token    
                                        }).ToListJsonObject<PessoaDevice>();

            var ORetorno = new UtilRetorno();

            if (ORetorno.flagError) {
                
                return ORetorno;
            }

            var OMensageiroApp = MensageiroAppFactoryBL.getInstance.factory(ONotificacao);

            if (OMensageiroApp == null) {
                
                return UtilRetorno.newInstance(true, "Não foi possível encontrar as configurações de envio do gateway selecionado para envio da mensagem.");
            }

            ORetorno = OMensageiroApp.enviar(ONotificacao, listaDispositivos);

            return ORetorno;
        }

    }
    
}
