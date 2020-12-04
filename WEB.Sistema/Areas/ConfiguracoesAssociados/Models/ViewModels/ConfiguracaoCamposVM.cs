using System;
using System.Collections.Generic;
using System.Linq;
using BLL.ConfiguracoesAssociados;
using DAL.ConfiguracoesAssociados;

namespace WEB.Areas.ConfiguracoesAssociados.Models.ViewModels {

    public class ConfiguracaoCamposVM {

        //Atributos
        private IConfiguracaoAssociadoCampoGrupoBL _ConfiguracaoAssociadoCampoGrupoBL;
        private IConfiguracaoAssociadoCampoBL _ConfiguracaoAssociadoCampoBL;

        //Servicos
        private IConfiguracaoAssociadoCampoGrupoBL OConfiguracaoGrupoBL => _ConfiguracaoAssociadoCampoGrupoBL = _ConfiguracaoAssociadoCampoGrupoBL ?? new ConfiguracaoAssociadoCampoGrupoBL();
        private IConfiguracaoAssociadoCampoBL OConfiguracaoCampoBL => _ConfiguracaoAssociadoCampoBL = _ConfiguracaoAssociadoCampoBL ?? new ConfiguracaoAssociadoCampoBL();

        //Propriedades
        public int idTipoCampoCadastro { get; set; }

        public List<int> idsTipoAssociado { get; set; }

        public int? idOrganizacao { get; set; }

        public List<ConfiguracaoAssociadoCampoGrupo> listaGrupos { get; set; }

        public List<ConfiguracaoAssociadoCampo> listaCampos { get; set; }

        //Construtor
        public ConfiguracaoCamposVM() {

            this.idsTipoAssociado = new List<int>();

            this.listaGrupos = new List<ConfiguracaoAssociadoCampoGrupo>();

            this.listaCampos = new List<ConfiguracaoAssociadoCampo>();

        }

        /// <summary>
        /// Carregar dados da viewmodel
        /// </summary>
        public void carregarDados() {

            this.carregarGrupos();

            this.carregarCampos();
        }

        /// <summary>
        /// Montar query e carregar lista dos grupos
        /// </summary>
        private void carregarGrupos() {

            var query = this.OConfiguracaoGrupoBL.listar(UtilNumber.toInt32(idOrganizacao));

            query = query.Where(x => x.idTipoCampoCadastro == idTipoCampoCadastro);

            this.listaGrupos = query.ToList().OrderBy(x => x.ordemExibicao ?? 10000).ToList();
        }

        /// <summary>
        /// Montar query e carregar lista de campos
        /// </summary>
        private void carregarCampos() {

            var query = this.OConfiguracaoCampoBL.listar(0, null, UtilNumber.toInt32(idOrganizacao));

            query = query.Where(x => x.idTipoCampoCadastro == idTipoCampoCadastro);

            if (this.idsTipoAssociado.Any()) {
                query = query.Where(x => x.listaTipoAssociado.Any(y => this.idsTipoAssociado.Contains(y.idTipoAssociado)) || !x.listaTipoAssociado.Any());
            }

            this.listaCampos = query.ToList().OrderBy(x => x.ordemExibicao ?? 10000).ToList();
        }
    }
}