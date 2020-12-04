using System;
using DAL.Associados;
using System.Collections.Generic;
using System.Linq;
using BLL.ConfiguracoesAssociados;
using DAL.ConfiguracoesAssociados;
using DAL.Permissao.Security.Extensions;
using FastMember;
using UTIL.Extensions;

namespace WEB.Areas.AssociadosDependentes.ViewModels {

    //[Validator(typeof(AssociadoCadastroPFFormValidator))]
    public class AssociadoDependenteCadastroForm {

        //Atributos
        private IConfiguracaoAssociadoCampoGrupoBL _ConfiguracaoAssociadoCampoGrupoBL;
        private IConfiguracaoAssociadoCampoBL _ConfiguracaoAssociadoCampoBL;


        //Servicos
        private IConfiguracaoAssociadoCampoGrupoBL OConfiguracaoGrupoBL => _ConfiguracaoAssociadoCampoGrupoBL = _ConfiguracaoAssociadoCampoGrupoBL ?? new ConfiguracaoAssociadoCampoGrupoBL();
        private IConfiguracaoAssociadoCampoBL OConfiguracaoCampoBL => _ConfiguracaoAssociadoCampoBL = _ConfiguracaoAssociadoCampoBL ?? new ConfiguracaoAssociadoCampoBL();

        //Propriedades

        public List<ConfiguracaoAssociadoCampoGrupo> listaGrupos { get; set; }

        public List<ConfiguracaoAssociadoCampo> listaCampos { get; set; }

        public ConfiguracaoAssociadoPF ConfiguracaoAssociadoPF { get; set; }

        public Associado Associado { get; set; }

        //Construtor
        public AssociadoDependenteCadastroForm() {

            this.listaGrupos = new List<ConfiguracaoAssociadoCampoGrupo>();

            this.listaCampos = new List<ConfiguracaoAssociadoCampo>();

        }

        //Atribuir valores padrão para quando estiverem em branco
        public void carregarConfiguracoes() {
            this.ConfiguracaoAssociadoPF = ConfiguracaoAssociadoPFBL.getInstance.carregar();
        }

        /// <summary>
        /// Carregar os dados
        /// </summary>
        public void carregaDados(bool flagTelaEdicao = false) {

            this.carregarGrupos();

            this.carregarCampos(flagTelaEdicao);

        }

        /// <summary>
        /// Montar query e carregar lista dos grupos
        /// </summary>
        private void carregarGrupos() {

            int idOrganizacao = HttpContextFactory.Current.User.idOrganizacao();

            var query = this.OConfiguracaoGrupoBL.listarFromCacheOrDefault(idOrganizacao, true).Where(x => x.idTipoCampoCadastro == TipoCampoCadastroConst.DP);

            this.listaGrupos = query.OrderBy(x => x.ordemExibicao ?? 10000)
                                    .ToList();
        }

        /// <summary>
        /// Montar query e carregar lista de campos
        /// </summary>
        private void carregarCampos(bool flagTelaEdicao = false) {

            int idOrganizacao = HttpContextFactory.Current.User.idOrganizacao();

            var query = this.OConfiguracaoCampoBL.listarFromCacheOrDefault(idOrganizacao, false).Where(x => x.idTipoCampoCadastro == TipoCampoCadastroConst.DP);

            if(flagTelaEdicao) {
                query = query.Where(x => x.flagEdicao == true);
            } else {
                query = query.Where(x => x.flagCadastro == true);
            }

            if (this.Associado.idTipoAssociado > 0) {
                query = query.Where(x => x.listaTipoAssociado.Any(y => y.idTipoAssociado == this.Associado.idTipoAssociado) || !x.listaTipoAssociado.Any());
            }
            
            this.listaCampos = query.OrderBy(x => x.ordemExibicao)
                                    .ToList();

        }

        /// <summary>
        /// Atribuir os valores fixos 
        /// </summary>
        public AssociadoDependenteCadastroForm atribuirValoresFixos(AssociadoDependenteCadastroForm ViewModel) {

            var listaCamposFixos = this.listaCampos.Where(x => !x.valorFixo.isEmpty()).ToList();

            var objectAcessor = ObjectAccessor.Create(ViewModel);

            foreach (var OCampoFixo in listaCamposFixos) {

                objectAcessor.assignValueToProperty(OCampoFixo.name, OCampoFixo.valorFixo);

            }

            return (AssociadoDependenteCadastroForm)objectAcessor.Target;
        }

        /// <summary>
        /// Atribuir os valores fixos 
        /// </summary>
        public void carregarValorCampos(AssociadoDependenteCadastroForm ViewModel) {

            var listaCamposNaoFixos = this.listaCampos.Where(x => x.valorFixo.isEmpty()).ToList();

            var objectAcessor = ObjectAccessor.Create(ViewModel);

            foreach (var OCampo in listaCamposNaoFixos) {

                try {

                    OCampo.valorAtual = objectAcessor.getValueProperty(OCampo.name).stringOrEmpty();

                } catch (ArgumentOutOfRangeException ex) {

                    UtilLog.saveError(ex, $"Erro ao atribuir valor {OCampo.name}");

                } catch (Exception ex) {

                    UtilLog.saveError(ex, $"Erro ao atribuir valor {OCampo.name}");
                }
            }
        }
    }
}