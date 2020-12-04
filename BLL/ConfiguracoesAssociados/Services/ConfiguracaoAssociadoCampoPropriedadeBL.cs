using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using BLL.Services;
using DAL.ConfiguracoesAssociados;
using DAL.Permissao.Security.Extensions;

namespace BLL.ConfiguracoesAssociados {

    public class ConfiguracaoAssociadoCampoPropriedadeBL : DefaultBL, IConfiguracaoAssociadoCampoPropriedadeBL {

        //
        public ConfiguracaoAssociadoCampoPropriedade carregar(int id, int? idOrganizacaoInf = null) {

            if (idOrganizacao > 0 && idOrganizacaoInf == null) {
                idOrganizacaoInf = idOrganizacao;
            }

            var query = db.ConfiguracaoAssociadoCampoPropriedade
                            .Include(x => x.ConfiguracaoAssociadoCampo)
                            .Where(x => x.id == id && x.dtExclusao == null);

            if (idOrganizacaoInf > 0) {
                query = query.Where(x => x.ConfiguracaoAssociadoCampo.idOrganizacao == idOrganizacaoInf);
            }

            if (idOrganizacaoInf == 0) {
                query = query.Where(x => x.ConfiguracaoAssociadoCampo.idOrganizacao == null);
            }

            var OConfiguracao = query.FirstOrDefault();

            return OConfiguracao;
        }

        //Configuracoes gerais
        public IQueryable<ConfiguracaoAssociadoCampoPropriedade> listar(int idCampo, int? idOrganizacaoInf = null) {

            if (idOrganizacao > 0 && idOrganizacaoInf == null) {

                idOrganizacaoInf = idOrganizacao;

            }

            var query = db.ConfiguracaoAssociadoCampoPropriedade
                            .Include(x => x.ConfiguracaoAssociadoCampo)
                            .Include(x => x.UsuarioCadastro)
                            .Where(x => x.dtExclusao == null)
                            .AsNoTracking();

            if (idCampo > 0) {
                query = query.Where(x => x.idConfiguracaoAssociadoCampo == idCampo);
            }

            if (idOrganizacao > 0) {
                query = query.Where(x => x.ConfiguracaoAssociadoCampo.idOrganizacao == idOrganizacao);
            }

            if (idOrganizacaoInf == 0) {
                query = query.Where(x => x.ConfiguracaoAssociadoCampo.idOrganizacao == null);
            }

            return query;
        }

        //
        public bool salvar(ConfiguracaoAssociadoCampoPropriedade OConfiguracao) {
            
            if (OConfiguracao.id == 0) {
                return this.inserir(OConfiguracao);
            }

            return this.atualizar(OConfiguracao);
        }


        //
        private bool inserir(ConfiguracaoAssociadoCampoPropriedade OConfiguracao) {

            OConfiguracao.setDefaultInsertValues();

            db.ConfiguracaoAssociadoCampoPropriedade.Add(OConfiguracao);

            db.SaveChanges();

            return OConfiguracao.id > 0;
        }

        //
        private bool atualizar(ConfiguracaoAssociadoCampoPropriedade OConfiguracao) {

            ConfiguracaoAssociadoCampoPropriedade dbRegistro = this.carregar(OConfiguracao.id);

            var TipoEntry = db.Entry(dbRegistro);

            OConfiguracao.setDefaultUpdateValues();

            TipoEntry.CurrentValues.SetValues(OConfiguracao);

            TipoEntry.State = EntityState.Modified;

            TipoEntry.ignoreFields();

            db.SaveChanges();

            return OConfiguracao.id > 0;
        }

        public bool clonarPropriedadesCampo(int idCampoClone, int idCampo) {

            var idUsuarioLogado = User.id();

            var listaPropriedadesClone = this.listar(idCampoClone).ToList();
            if (!listaPropriedadesClone.Any()) {
                return false;
            }

            var listaPropriedades = new List<ConfiguracaoAssociadoCampoPropriedade>();
            foreach (var OPropriedadesClone in listaPropriedadesClone) {

                var OPropriedade = new ConfiguracaoAssociadoCampoPropriedade();
                OPropriedade.idConfiguracaoAssociadoCampo = idCampo;
                OPropriedade.idUsuarioCadastro = idUsuarioLogado;
                OPropriedade.nome = OPropriedadesClone.nome;
                OPropriedade.valor = OPropriedadesClone.valor;
                OPropriedade.dtCadastro = DateTime.Now;

                listaPropriedades.Add(OPropriedade);
            }

            db.ConfiguracaoAssociadoCampoPropriedade.AddRange(listaPropriedades);
            db.SaveChanges();

            return true;
        }

        //
        public UtilRetorno excluir(int id) {

            var Registro = this.carregar(id);

            if (Registro == null) {
                return UtilRetorno.newInstance(true, "O registro informado não foi localizado.");
            }

            Registro.dtExclusao = DateTime.Now;

            Registro.idUsuarioExclusao = User.id();

            this.db.SaveChanges();

            var ORetorno = UtilRetorno.newInstance(false, "O registro foi removido com sucesso.");

            ORetorno.info = Registro.ConfiguracaoAssociadoCampo.idTipoCampoCadastro;

            return ORetorno;
        }
    }
}