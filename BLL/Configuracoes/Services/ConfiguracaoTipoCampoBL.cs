using System;
using System.Linq;
using BLL.Services;
using DAL.Configuracoes;

namespace BLL.Configuracoes {

    public class ConfiguracaoTipoCampoBL : DefaultBL, IConfiguracaoTipoCampoBL {

        //Atributos

        //Servicos

        /// <summary>
        /// Carregar registro pelo ID
        /// </summary>
        public ConfiguracaoTipoCampo carregar(int id) {

            var query = from Reg in db.ConfiguracaoTipoCampo
                        where
                            Reg.id == id &&
                            Reg.dtExclusao == null
                        select Reg;

            ConfiguracaoTipoCampo OConfiguracaoTipoCampo = query.FirstOrDefault();

            return OConfiguracaoTipoCampo;
        }

        /// <summary>
        /// Montagem de query LINQ
        /// </summary>
        public IQueryable<ConfiguracaoTipoCampo> listar(string descricao, bool? ativo = true) {

            var query = from Reg in db.ConfiguracaoTipoCampo
                        where Reg.dtExclusao == null
                        select Reg;

            if (!descricao.isEmpty()) {
                query = query.Where(x => x.descricao.Contains(descricao));
            }

            if (ativo.HasValue) {
                query = query.Where(x => x.ativo == ativo);
            }

            return query;
        }

        /// <summary>
        /// Incluir ou atualizar um registro em base de dados
        /// </summary>
        public bool salvar(ConfiguracaoTipoCampo OConfiguracaoTipoCampo) {

            if (OConfiguracaoTipoCampo.id == 0) {

                return this.inserir(OConfiguracaoTipoCampo);

            }

            return this.atualizar(OConfiguracaoTipoCampo);
        }

        //Persistir o objecto e salvar na base de dados
        private bool inserir(ConfiguracaoTipoCampo OConfiguracaoTipoCampo) {

            OConfiguracaoTipoCampo.setDefaultInsertValues();

            db.ConfiguracaoTipoCampo.Add(OConfiguracaoTipoCampo);

            db.SaveChanges();

            return (OConfiguracaoTipoCampo.id > 0);
        }

        //Persistir o objecto e atualizar informações
        private bool atualizar(ConfiguracaoTipoCampo OConfiguracaoTipoCampo) {

            OConfiguracaoTipoCampo.setDefaultUpdateValues();

            //Localizar existentes no BoletoContaEmissao
            ConfiguracaoTipoCampo dbRegistro = this.carregar(OConfiguracaoTipoCampo.id);

            var TipoEntry = db.Entry(dbRegistro);

            TipoEntry.CurrentValues.SetValues(OConfiguracaoTipoCampo);

            TipoEntry.ignoreFields();

            db.SaveChanges();

            return OConfiguracaoTipoCampo.id > 0;
        }

    }
}