using System;
using System.Linq;
using System.Data.Entity;
using System.Json;
using BLL.Services;
using DAL.ContasBancarias;
using DAL.Permissao.Security.Extensions;
using EntityFramework.Extensions;

namespace BLL.ContasBancarias {

    public class ContaBancariaBL : DefaultBL, IContaBancariaBL {

        public const string keyCache = "conta_bancaria";

        public IQueryable<ContaBancaria> query(int? idOrganizacaoParam = null) {
            
            var query = from CB in db.ContaBancaria
                where 
                    CB.flagExcluido == false
                select CB;
            
            if (idOrganizacaoParam == null) {
                idOrganizacaoParam = idOrganizacao;
            }
            
            if (idOrganizacaoParam > 0) {
                query = query.Where(x => x.idOrganizacao == idOrganizacaoParam);
            }
            
            return query;

        }
        
        //Carregamento de registro pelo ID
        public ContaBancaria carregar(int id) {

            var query = db.ContaBancaria.condicoesSeguranca()
                          .Include(x => x.Cidade).Include(x => x.OBanco)
                          .Where(x => x.flagExcluido == false);
            
            return query.FirstOrDefault(x => x.id == id);

        }

        //Listagem de registros de acordo com filtros
        public IQueryable<ContaBancaria> listar(string valorBusca, bool? ativo) {

            var query = from P in db.ContaBancaria.condicoesSeguranca()
                                                  .Include(x => x.Cidade)
                                                  .Include(x => x.OBanco)
                        where P.flagExcluido == false
                        select P;

            if (!String.IsNullOrEmpty(valorBusca)) {
                query = query.Where(x => x.descricao.Contains(valorBusca));
            }

            if (ativo.HasValue) {
                query = query.Where(x => x.ativo == ativo);
            }
            
            return query;

        }

        //Verificar se já existe um registro com a descrição informada, no entanto, que possua id diferente do informado
        public bool existe(ContaBancaria OContaBancaria, bool descricao) {

            var query = db.ContaBancaria.condicoesSeguranca().Where(x => x.flagExcluido == false);

            if (OContaBancaria.id > 0) {
                query = query.Where(x => x.id != OContaBancaria.id);
            }
            
            if (descricao) {
                query = query.Where(x => x.descricao == OContaBancaria.descricao);
                return query.Any();
            }

            return query.Any();
        }

        //Verificar se deve-se atualizar um registro existente ou criar um novo
        public bool salvar(ContaBancaria OContaBancaria) {

            OContaBancaria.documentoTitular = OContaBancaria.documentoTitular.onlyNumber().abreviar(14);

            OContaBancaria.cep = UtilString.onlyNumber(OContaBancaria.cep);

            OContaBancaria.Cidade = null;

            if (OContaBancaria.id == 0) {
                return this.inserir(OContaBancaria);
            }

            return this.atualizar(OContaBancaria);

        }

        //Persistir o objecto e salvar na base de dados
        private bool inserir(ContaBancaria OContaBancaria) {

            OContaBancaria.setDefaultInsertValues();

            db.ContaBancaria.Add(OContaBancaria);

            db.SaveChanges();

            return OContaBancaria.id > 0;
        }

        //Persistir o objecto e atualizar informações
        private bool atualizar(ContaBancaria OContaBancaria) {

            OContaBancaria.setDefaultUpdateValues();

            //Localizar existentes no BoletoContaEmissao
            var dbContaBancaria = this.carregar(OContaBancaria.id);

            if (dbContaBancaria == null) {
                return false;
            }

            var TipoEntry = db.Entry(dbContaBancaria);

            TipoEntry.CurrentValues.SetValues(OContaBancaria);

            TipoEntry.ignoreFields();

            db.SaveChanges();

            return OContaBancaria.id > 0;

        }

        //Remover um registro (exclusao lógica - nao remove-se fisicamente)
        public bool excluir(int id) {

            var idUsuario = User.id();

            db.ContaBancaria
                .Where(x => x.id == id)
                .Update(x => new ContaBancaria { flagExcluido = true, dtAlteracao = DateTime.Now, idUsuarioAlteracao = idUsuario });

            return true;
        }

        /// <summary>
        /// Alteracao de status
        /// </summary>
        public JsonMessageStatus alterarStatus(int id) {

            var retorno = new JsonMessageStatus();

            var Objeto = this.carregar(id);

            if (Objeto == null) {

                retorno.error = true;

                retorno.message = "O registro informado não foi encontrado.";

                return retorno;
            }

            Objeto.ativo = Objeto.ativo != true;

            db.SaveChanges();

            retorno.active = Objeto.ativo == true ? "S" : "N";

            retorno.message = "Os dados foram alterados com sucesso.";

            return retorno;
        }

    }
}