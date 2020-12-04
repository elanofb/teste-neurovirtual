using System.Collections.Generic;
using System.Linq;
using BLL.Services;
using DAL.ConfiguracoesAssociados;
using DAL.Permissao.Security.Extensions;

namespace BLL.ConfiguracoesAssociados {

    public class ConfiguracaoAssociadoCampoTipoAssociadoClonagemBL : DefaultBL, IConfiguracaoAssociadoCampoTipoAssociadoClonagemBL {
        
        //Atributos
        private IConfiguracaoAssociadoCampoTipoAssociadoBL _ConfiguracaoAssociadoCampoTipoAssociadoBL;

        //Services
        private IConfiguracaoAssociadoCampoTipoAssociadoBL OConfiguracaoAssociadoCampoTipoAssociadoBL => _ConfiguracaoAssociadoCampoTipoAssociadoBL = _ConfiguracaoAssociadoCampoTipoAssociadoBL ?? new ConfiguracaoAssociadoCampoTipoAssociadoBL();

        public bool clonarConfiguracaoCampos(int idOrganizacaoInf, int idTipoAssociadoOrigem, List<int> listIdTipoAssociadoDestino) {

            var idUsuarioLogado = User.id();
            if (idOrganizacao > 0) {
                idOrganizacaoInf = idOrganizacao;
            }

            if (idOrganizacaoInf == 0) {
                return false;
            }
            
            List<ConfiguracaoAssociadoCampoTipoAssociado> listaNovasConfiguracoes = new List<ConfiguracaoAssociadoCampoTipoAssociado>();

            var listIdsCampos = OConfiguracaoAssociadoCampoTipoAssociadoBL.listar(idOrganizacaoInf).Where(x => x.idTipoAssociado == idTipoAssociadoOrigem).Select(x => x.idConfiguracaoAssociadoCampo).ToList();
            
            var idsConfiguracaoCampoTipoAssociadoExcluir = OConfiguracaoAssociadoCampoTipoAssociadoBL.listar(idOrganizacaoInf).Where(x => listIdTipoAssociadoDestino.Contains(x.idTipoAssociado)).Select(x => x.id).ToList();

            if (idsConfiguracaoCampoTipoAssociadoExcluir.Any()) {
                OConfiguracaoAssociadoCampoTipoAssociadoBL.excluir(idsConfiguracaoCampoTipoAssociadoExcluir);
            }

            foreach (var idTipoAssociadoDestino in listIdTipoAssociadoDestino) {

                foreach (var idConfiguracaoCampo in listIdsCampos) {
                    
                    var ONovaConfiguracao = new ConfiguracaoAssociadoCampoTipoAssociado();

                    ONovaConfiguracao.idConfiguracaoAssociadoCampo = idConfiguracaoCampo;
                    ONovaConfiguracao.idTipoAssociado = idTipoAssociadoDestino;
                    ONovaConfiguracao.idUsuarioCadastro = idUsuarioLogado;

                    listaNovasConfiguracoes.Add(ONovaConfiguracao);
                    
                }
                
            }

            return OConfiguracaoAssociadoCampoTipoAssociadoBL.salvarEmLote(listaNovasConfiguracoes);
            
        }
    }
}