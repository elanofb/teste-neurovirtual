using System;
using DAL.Associados;
using System.Collections.Generic;
using System.Linq;
using BLL.Associados;
using BLL.ConfiguracoesAssociados;
using DAL.ConfiguracoesAssociados;
using DAL.Permissao.Security.Extensions;
using FastMember;
using FluentValidation.Attributes;
using UTIL.Extensions;

namespace WEB.Areas.Associados.ViewModels {

    [Validator(typeof(AssociadoCadastroPJFormValidator))]
    public class AssociadoCadastroPJForm {

        //Atributos
        private IConfiguracaoAssociadoCampoGrupoBL _ConfiguracaoAssociadoCampoGrupoBL;
        private IConfiguracaoAssociadoCampoBL _ConfiguracaoAssociadoCampoBL;
        private IConfiguracaoMembroConsultaBL _ConfiguracaoMembroConsultaBL;


        //Servicos
        private IConfiguracaoAssociadoCampoGrupoBL OConfiguracaoGrupoBL => _ConfiguracaoAssociadoCampoGrupoBL = _ConfiguracaoAssociadoCampoGrupoBL ?? new ConfiguracaoAssociadoCampoGrupoBL();
        private IConfiguracaoAssociadoCampoBL OConfiguracaoCampoBL => _ConfiguracaoAssociadoCampoBL = _ConfiguracaoAssociadoCampoBL ?? new ConfiguracaoAssociadoCampoBL();
        private IConfiguracaoMembroConsultaBL OConfiguracaoMembroConsultaBL => _ConfiguracaoMembroConsultaBL = _ConfiguracaoMembroConsultaBL ?? new ConfiguracaoMembroConsultaBL();

        //Propriedades

        public List<ConfiguracaoAssociadoCampoGrupo> listaGrupos { get; set; }

        public List<ConfiguracaoAssociadoCampo> listaCampos { get; set; }

        public ConfiguracaoAssociadoPJ ConfiguracaoAssociadoPJ { get; set; }
        
        public ConfiguracaoMembro ConfiguracaoMembro { get; set; }

        public Associado Associado { get; set; }
        
        public MembroSaldo Saldo { get; set; }
        
        public bool? flagRetornoAjax { get; set; }

        //Construtor
        public AssociadoCadastroPJForm() {

            this.listaGrupos = new List<ConfiguracaoAssociadoCampoGrupo>();

            this.listaCampos = new List<ConfiguracaoAssociadoCampo>();
        }

        //Atribuir valores padrão para quando estiverem em branco
        public void carregarConfiguracoes() {

            this.ConfiguracaoAssociadoPJ = ConfiguracaoAssociadoPJBL.getInstance.carregar();


        }

        //Atribuir valores padrão para quando estiverem em branco
        public void carregarConfiguracaoMembro() {

            this.ConfiguracaoMembro = OConfiguracaoMembroConsultaBL.carregarPorMembro(this.Associado.id) ?? new ConfiguracaoMembro();

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
        protected void carregarGrupos() {

            int idOrganizacao = HttpContextFactory.Current.User.idOrganizacao();

            var query = this.OConfiguracaoGrupoBL.listarFromCacheOrDefault(idOrganizacao, true)
                                            .Where(x => x.idTipoCampoCadastro == TipoCampoCadastroConst.PJ && x.ativo != false);

            this.listaGrupos = query.OrderBy(x => x.ordemExibicao ?? 10000).ToList();
            
            if (!this.listaGrupos.Any()) {

                this.listaGrupos = this.OConfiguracaoGrupoBL.listarFromDefault()
                                            .Where(x => x.idTipoCampoCadastro == TipoCampoCadastroConst.PJ && x.ativo != false)
                                            .OrderBy(x => x.ordemExibicao ?? 10000).ToList();
            }
        }

        /// <summary>
        /// Montar query e carregar lista de campos
        /// </summary>
        protected void carregarCampos(bool flagTelaEdicao = false) {

            int idOrganizacao = HttpContextFactory.Current.User.idOrganizacao();

            var query = this.OConfiguracaoCampoBL.listarFromCacheOrDefault(idOrganizacao, true)
                                            .Where(x => x.idTipoCampoCadastro == TipoCampoCadastroConst.PJ && x.flagAreaAdm == true && x.ativo != false);

            if(flagTelaEdicao) {
                query = query.Where(x => x.flagEdicao == true);
            } else {
                query = query.Where(x => x.flagCadastro == true);
            }

            if (this.Associado.idTipoAssociado > 0) {
                query = query.Where(x => x.listaTipoAssociado.Any(y => y.idTipoAssociado == this.Associado.idTipoAssociado) || !x.listaTipoAssociado.Any());
            }

            this.listaCampos = query.OrderBy(x => x.ordemExibicao).ToList();

            if (!this.listaCampos.Any()) {

                this.listaCampos = this.OConfiguracaoCampoBL.listarFromDefault()
                                            .Where(x => x.idTipoCampoCadastro == TipoCampoCadastroConst.PJ && x.flagAreaAdm == true && x.ativo != false)
                                            .OrderBy(x => x.ordemExibicao ?? 10000).ToList();
            }
        }

        /// <summary>
        /// Atribuir os valores fixos 
        /// </summary>
        public AssociadoCadastroPJForm atribuirValoresFixos(AssociadoCadastroPJForm ViewModel) {

            var listaCamposFixos = this.listaCampos.Where(x => !x.valorFixo.isEmpty()).ToList();

            var objectAcessor = ObjectAccessor.Create(ViewModel);

            foreach (var OCampoFixo in listaCamposFixos) {

                objectAcessor.assignValueToProperty(OCampoFixo.name, OCampoFixo.valorFixo);

            }
            return (AssociadoCadastroPJForm)objectAcessor.Target;
        }

        /// <summary>
        /// Atribuir os valores fixos 
        /// </summary>
        public void carregarValorCampos(AssociadoCadastroPJForm ViewModel) {

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