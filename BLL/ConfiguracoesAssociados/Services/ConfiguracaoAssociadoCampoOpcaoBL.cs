using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using BLL.Caches;
using BLL.Services;
using DAL.ConfiguracoesAssociados;
using DAL.Permissao.Security.Extensions;

namespace BLL.ConfiguracoesAssociados {

    public class ConfiguracaoAssociadoCampoOpcaoBL : DefaultBL, IConfiguracaoAssociadoCampoOpcaoBL {

        //
        public ConfiguracaoAssociadoCampoOpcao carregar(int id, int? idOrganizacaoInf = null) {

            if (idOrganizacao > 0 && idOrganizacaoInf == null) {
                idOrganizacaoInf = idOrganizacao;
            }

            var query = db.ConfiguracaoAssociadoCampoOpcao
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
        public IQueryable<ConfiguracaoAssociadoCampoOpcao> listar(int idCampo, int? idOrganizacaoInf = null) {

            if (idOrganizacao > 0 && idOrganizacaoInf == null) {
                idOrganizacaoInf = idOrganizacao;
            }

            var query = db.ConfiguracaoAssociadoCampoOpcao
                            .Include(x => x.ConfiguracaoAssociadoCampo)
                            .Include(x => x.UsuarioCadastro)
                            .Where(x => x.dtExclusao == null)
                            .AsNoTracking();

            if (idCampo > 0) {
                query = query.Where(x => x.idConfiguracaoAssociadoCampo == idCampo);
            }

            if (idOrganizacao > 0) {
                query = query.Where(x => x.ConfiguracaoAssociadoCampo.idOrganizacao == idOrganizacaoInf);
            }

            if (idOrganizacaoInf == 0) {
                query = query.Where(x => x.ConfiguracaoAssociadoCampo.idOrganizacao == null);
            }

            return query;
        }

        //
        public bool salvar(ConfiguracaoAssociadoCampoOpcao OConfiguracao) {
            
            if (OConfiguracao.id == 0) {
                return this.inserir(OConfiguracao);
            }

            return this.atualizar(OConfiguracao);
        }


        //
        private bool inserir(ConfiguracaoAssociadoCampoOpcao OConfiguracao) {

            OConfiguracao.setDefaultInsertValues();

            db.ConfiguracaoAssociadoCampoOpcao.Add(OConfiguracao);

            db.SaveChanges();

            return OConfiguracao.id > 0;
        }

        //
        private bool atualizar(ConfiguracaoAssociadoCampoOpcao OConfiguracao) {

            ConfiguracaoAssociadoCampoOpcao dbRegistro = this.carregar(OConfiguracao.id);

            var TipoEntry = db.Entry(dbRegistro);

            OConfiguracao.setDefaultUpdateValues();

            TipoEntry.CurrentValues.SetValues(OConfiguracao);

            TipoEntry.State = EntityState.Modified;

            TipoEntry.ignoreFields();

            db.SaveChanges();

            return OConfiguracao.id > 0;
        }

        public bool clonarOpcoesCampo(int idCampoClone, int idCampo) {

            var idUsuarioLogado = User.id();

            var listaPropriedadesClone = this.listar(idCampoClone).ToList();
            if (!listaPropriedadesClone.Any()) {
                return false;
            }

            var listaPropriedades = new List<ConfiguracaoAssociadoCampoOpcao>();
            foreach (var OPropriedadesClone in listaPropriedadesClone) {

                var OPropriedade = new ConfiguracaoAssociadoCampoOpcao();
                OPropriedade.idConfiguracaoAssociadoCampo = idCampo;
                OPropriedade.idUsuarioCadastro = idUsuarioLogado;
                OPropriedade.texto = OPropriedadesClone.texto;
                OPropriedade.value = OPropriedadesClone.value;
                OPropriedade.dtCadastro = DateTime.Now;

                listaPropriedades.Add(OPropriedade);
            }

            db.ConfiguracaoAssociadoCampoOpcao.AddRange(listaPropriedades);

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