using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using BLL.Caches;
using BLL.Services;
using DAL.ConfiguracoesAssociados;
using DAL.Permissao.Security.Extensions;
using BLL.Configuracoes.Services;
using EntityFramework.Extensions;
using Newtonsoft.Json;

namespace BLL.ConfiguracoesAssociados {

    public class ConfiguracaoAssociadoCampoTipoAssociadoBL : DefaultBL, IConfiguracaoAssociadoCampoTipoAssociadoBL {

        //Propriedades
        private readonly string chaveCache = "lista_campos_associado";

        /// <summary>
        /// Carregar as configurações de campos do tipo de associado
        /// </summary>
        public ConfiguracaoAssociadoCampoTipoAssociado carregar(int id, int? idOrganizacaoParam = null) {

            if (idOrganizacao > 0 && idOrganizacaoParam == null) {
                idOrganizacaoParam = idOrganizacao;
            }

            var query = db.ConfiguracaoAssociadoCampoTipoAssociado
                            .Include(x => x.AssociadoCampo)
                            .Where(x => x.id == id && x.dtExclusao == null);

            if (idOrganizacaoParam > 0) {
                query = query.Where(x => x.AssociadoCampo.idOrganizacao == idOrganizacaoParam);
            }

            if (idOrganizacaoParam == 0) {
                query = query.Where(x => x.AssociadoCampo.idOrganizacao == null);
            }

            var OConfiguracao = query.FirstOrDefault();

            return OConfiguracao;
        }

        /// <summary>
        /// Carregar as configurações de campos dos tipos de associados
        /// </summary>
        public IQueryable<ConfiguracaoAssociadoCampoTipoAssociado> listar(int? idOrganizacaoParam = null) {

            if (idOrganizacaoParam.toInt() == 0) {

                idOrganizacaoParam = idOrganizacao;
            }

            var query = db.ConfiguracaoAssociadoCampoTipoAssociado
                            .Include(x => x.AssociadoCampo)
                            .Where(x => x.dtExclusao == null)
                            .AsNoTracking();

            if (idOrganizacaoParam > 0) {
                query = query.Where(x => x.AssociadoCampo.idOrganizacao == idOrganizacaoParam);
            }

            if (idOrganizacaoParam == 0) {
                query = query.Where(x => x.AssociadoCampo.idOrganizacao == null);
            }

            return query;
        }

        /// <summary>
        /// Salvar um novo registro o atualizar
        /// </summary>
        public bool salvar(ConfiguracaoAssociadoCampoTipoAssociado OConfiguracaoTipoAssociado) {

            CacheService.getInstance.remover(chaveCache);

            if (OConfiguracaoTipoAssociado.id == 0) {
                return this.inserir(OConfiguracaoTipoAssociado);
            }

            return this.atualizar(OConfiguracaoTipoAssociado);
        }

        /// <summary>
        /// Inserir novos registros em lote
        /// </summary>
        public bool salvarEmLote(List<ConfiguracaoAssociadoCampoTipoAssociado> listaConfiguracaoTipoAssociado) {

            CacheService.getInstance.remover(chaveCache);
            
            listaConfiguracaoTipoAssociado.ForEach(Item => {
                Item.setDefaultInsertValues();
            });

            db.ConfiguracaoAssociadoCampoTipoAssociado.AddRange(listaConfiguracaoTipoAssociado);

            db.SaveChanges();

            return listaConfiguracaoTipoAssociado.All(x => x.id > 0);
        }

        /// <summary>
        /// Inserir um novo registro
        /// </summary>
        private bool inserir(ConfiguracaoAssociadoCampoTipoAssociado OConfiguracaoTipoAssociado) {

            OConfiguracaoTipoAssociado.setDefaultInsertValues();

            db.ConfiguracaoAssociadoCampoTipoAssociado.Add(OConfiguracaoTipoAssociado);

            db.SaveChanges();

            return OConfiguracaoTipoAssociado.id > 0;
        }

        /// <summary>
        /// Atualizar os dados de um registro existente
        /// </summary>
        private bool atualizar(ConfiguracaoAssociadoCampoTipoAssociado OConfiguracaoTipoAssociado) {

            ConfiguracaoAssociadoCampoTipoAssociado dbRegistro = this.carregar(OConfiguracaoTipoAssociado.id);
            if (dbRegistro == null) {
                return false;
            }

            var TipoEntry = db.Entry(dbRegistro);

            OConfiguracaoTipoAssociado.setDefaultUpdateValues();

            TipoEntry.CurrentValues.SetValues(OConfiguracaoTipoAssociado);

            TipoEntry.State = EntityState.Modified;

            TipoEntry.ignoreFields();

            db.SaveChanges();

            return OConfiguracaoTipoAssociado.id > 0;
        }

        /// <summary>
        /// Excluir um registro
        /// </summary>
        public UtilRetorno excluir(List<int> ids) {

            if (!ids.Any()) {
                return UtilRetorno.newInstance(true, "O registro informado não pode ser removido.");
            }

            var idUsuario = User.id();

            db.ConfiguracaoAssociadoCampoTipoAssociado
                .Where(x => ids.Contains(x.id))
                .Update(x => new ConfiguracaoAssociadoCampoTipoAssociado { dtExclusao = DateTime.Now, idUsuarioExclusao = idUsuario });
            
            this.db.SaveChanges();

            CacheService.getInstance.remover(chaveCache);

            return UtilRetorno.newInstance(false, "O registro foi removido com sucesso.");
        }
    }
}