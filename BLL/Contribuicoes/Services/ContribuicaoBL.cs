using System;
using System.Data.Entity;
using System.Linq;
using BLL.Services;
using DAL.Contribuicoes;


namespace BLL.Contribuicoes {

    public abstract class ContribuicaoBL : DefaultBL, IContribuicaoBL {

        //Carregamento de registro único pelo ID
        public IQueryable<Contribuicao> query(int? idOrganizacaoParam = null) {

            var query = from Contr in db.Contribuicao
                select Contr;
            
            return query;
        }

        //Carregamento de registro único pelo ID
        public Contribuicao carregar(int id) {

            var query = this.query().condicoesSeguranca()
                                    .Include(x => x.listaTabelaPreco)
                                    .Include(x => x.listaContribuicaoPreco)
                                    .Include(x => x.PeriodoContribuicao)
                                    .Include(x => x.UsuarioCadastro);
                        
            return query.FirstOrDefault(x => x.id == id);
        }
        
        //Listagem de Registros
        public virtual IQueryable<Contribuicao> listar(string valorBusca, string ativo) {

            var query = this.query().condicoesSeguranca()
                                    .Include(x => x.PeriodoContribuicao)
                                    .Include(x => x.listaContribuicaoPreco)
                                    .AsNoTracking();

            if (!String.IsNullOrEmpty(valorBusca)) {
                query = query.Where(x => x.descricao.Contains(valorBusca));
            }

            if (!String.IsNullOrEmpty(ativo)) {
                query = query.Where(x => x.ativo == ativo);
            }
            
            return query;
        }

        //Realizar os tratamentos necessários
        //Salvar um novo registro
        public virtual bool salvar(Contribuicao OContribuicao) {

            if (Convert.ToInt32(OContribuicao.mesInicioVigencia) == 0)
                OContribuicao.mesInicioVigencia = 1;

            OContribuicao.dtInicioVigencia = new DateTime(Convert.ToInt32(OContribuicao.anoInicioVigencia), Convert.ToInt32(OContribuicao.mesInicioVigencia), 1, 00, 00, 00);

            if (OContribuicao.id == 0) {
                return this.inserir(OContribuicao);
            }

            return this.atualizar(OContribuicao);
        }

        //Persistir e inserir um novo registro 
        //Inserir Contribuicao e lista de ContribuicaoPreco vinculados
        protected virtual bool inserir(Contribuicao OContribuicao) {

            OContribuicao.setDefaultInsertValues();

            OContribuicao.listaContribuicaoPreco.ForEach(Item => {
                Item.setDefaultInsertValues();
                Item.TipoAssociado = null;
            });

            db.Contribuicao.Add(OContribuicao);
            db.SaveChanges();

            return OContribuicao.id > 0;
        }

        //Persistir e atualizar um registro existente 
        //Atualizar dados da Contribuicao e lista de ContribuicaoPreco
        protected virtual bool atualizar(Contribuicao OContribuicao) {

            //Localizar existentes no banco
            Contribuicao dbContribuicao = this.carregar(OContribuicao.id);

            //Configurar valores padrão
            OContribuicao.setDefaultUpdateValues();

            //Atualizacao da lista de preços enviados
            foreach (var ItemContribuicaoPreco in OContribuicao.listaContribuicaoPreco) {

                var dbContribuicaoPreco = db.ContribuicaoPreco.FirstOrDefault(e => e.id == ItemContribuicaoPreco.id);

                ItemContribuicaoPreco.TipoAssociado = null;

                if (dbContribuicaoPreco == null) {

                    ItemContribuicaoPreco.idContribuicao = OContribuicao.id;

                    ItemContribuicaoPreco.setDefaultInsertValues();

                    ItemContribuicaoPreco.flagSistema = "N";

                    db.ContribuicaoPreco.Add(ItemContribuicaoPreco);


                } else {
                    var ContribuicaoPrecoEntry = db.Entry(dbContribuicaoPreco);

                    ItemContribuicaoPreco.flagSistema = "N";

                    ItemContribuicaoPreco.setDefaultUpdateValues();

                    ItemContribuicaoPreco.ativo = "S";

                    ContribuicaoPrecoEntry.CurrentValues.SetValues(ItemContribuicaoPreco);

                    ContribuicaoPrecoEntry.ignoreFields(new[] { "idTipoAssociado", "idContribuicao" });
                }
            }

            //Atualizacao da Contribuição
            var ContribuicaoEntry = db.Entry(dbContribuicao);
            ContribuicaoEntry.CurrentValues.SetValues(OContribuicao);
            ContribuicaoEntry.ignoreFields(new[] { "idTipoContribuicao" });

            db.SaveChanges();
            return OContribuicao.id > 0;
        }

        //Verificar existencia de registro
        public abstract bool existe(Contribuicao OContribuicao);

        //Excluir registro
        public virtual bool excluir(int id) {
            //throw new Exception("Nao há permissao para excluir uma contribuicao.");

            return false;
        }

    }
}