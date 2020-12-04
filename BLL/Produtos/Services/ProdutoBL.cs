using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Json;
using System.Linq;
using System.Web;
using BLL.Arquivos;
using BLL.Services;
using DAL.Arquivos;
using DAL.Entities;
using DAL.Permissao.Security.Extensions;
using DAL.Produtos;
using EntityFramework.Extensions;
using UTIL.Resources;

namespace BLL.Produtos {

    public class ProdutoBL : DefaultBL, IProdutoBL {

        //Atributos
        private IArquivoUploadFotoBL _IArquivoUploadFotoBL;

        //Propriedades
        private IArquivoUploadFotoBL OArquivoUploadFotoBL => _IArquivoUploadFotoBL = _IArquivoUploadFotoBL ?? new ArquivoUploadFotoBL();

        //Construtor
        
        //
        public IQueryable<Produto> query(int? idOrganizacaoParam = null) {

            var query = from Obj in db.Produto
                        where Obj.flagExcluido == false
                        select Obj;
            
            if (idOrganizacaoParam == null) {
                idOrganizacaoParam = idOrganizacao;
            }

            if (idOrganizacaoParam > 0) {
                query = query.Where(x => x.idOrganizacao == idOrganizacaoParam);
            }

            return query;

        }

        //
        public Produto carregar(int id) {
            
            var query = this.query().condicoesSeguranca();
            
            return query.FirstOrDefault(x => x.id == id);

        }
        
        //
        public IQueryable<Produto> listar(int idTipoProduto, string valorBusca, bool? ativo, string flagProdServ = "") {

            var query = this.query().condicoesSeguranca();

            if (idTipoProduto > 0) {
                query = query.Where(x => x.idTipoProduto == idTipoProduto);
            }

            if (!String.IsNullOrEmpty(valorBusca)) {
                query = query.Where(x => x.nome.Contains(valorBusca) || x.descricaoResumida.Contains(valorBusca));
            }

            if (ativo != null) {
                query = query.Where(x => x.ativo == ativo);
            }

            if (!String.IsNullOrEmpty(flagProdServ)) {

                if (flagProdServ == ProdutoConst.PRODUTO) {
                    query = query.Where(x => x.TipoProduto.flagProduto == true);
                }

                if (flagProdServ == ProdutoConst.SERVICO) {
                    query = query.Where(x => x.TipoProduto.flagServico == true);
                }

            }
            
            return query.AsNoTracking();
        }

        //Listagem de produtos usados em campos de autocomplemento
        public IQueryable<ProdutoAutoComplete> autocompletar(string valorBusca) {
            
            var query = this.query().Include(x => x.TipoProduto)
                            .Where(x => (x.nome.Contains(valorBusca) || String.IsNullOrEmpty(valorBusca)) &&
                                         x.flagExcluido == false && x.ativo == true)
                            .Select(x => new ProdutoAutoComplete { value = x.nome, id = x.id, label = x.nome,
                                                                   peso = x.peso, valor = x.valor });
            
            query = query.condicoesSeguranca();

            return query.AsNoTracking();
        }

        //Verificar se deve-se atualizar um registro existente ou criar um novo
        public bool salvar(Produto OProduto, HttpPostedFileBase OImagem) {

            OProduto.TipoProduto = null;

            OProduto.OFornecedor = null;

            var flagSucesso = false;

            OProduto.nome = OProduto.nome.abreviar(255).toUppercaseWords();

            OProduto.descricaoResumida = OProduto.descricaoResumida.abreviar(255);

            OProduto.descricaoCompleta = OProduto.descricaoCompleta.abreviar(1000);

            OProduto.codigoContabil = OProduto.codigoContabil.abreviar(1000);

            if (OProduto.id > 0) {
                flagSucesso = this.atualizar(OProduto);
            }

            if (OProduto.id == 0) {
                flagSucesso = this.inserir(OProduto);
            }


            if (flagSucesso && OImagem != null) {

                var OArquivo = new ArquivoUpload();

                OArquivo.idReferenciaEntidade = OProduto.id;

                OArquivo.entidade = EntityTypes.PRODUTO;

                var listaThumbs = new List<ThumbDTO>();

                listaThumbs.Add(new ThumbDTO { folderName = "h50", height = 50, width = 0 });

                listaThumbs.Add(new ThumbDTO { folderName = "sistema", height = 50, width = 0 });

                listaThumbs.Add(new ThumbDTO { folderName = "box", height = 130, width = 0 });

                this.OArquivoUploadFotoBL.salvar(OArquivo, OImagem, "", listaThumbs);

            }

            return flagSucesso;
        }

        //Persistir o objecto e salvar na base de dados
        private bool inserir(Produto OProduto) {

            OProduto.qtde = 0;

            OProduto.setDefaultInsertValues();

            db.Produto.Add(OProduto);

            db.SaveChanges();

            return (OProduto.id > 0);
        }

        //Persistir o objecto e atualizar informações
        private bool atualizar(Produto OProduto) {

            //Localizar existentes no banco
            var dbProduto = this.carregar(OProduto.id);

            if (dbProduto == null) {
                return false;
            }

            var dbEntry = db.Entry(dbProduto);

            OProduto.setDefaultUpdateValues();

            dbEntry.CurrentValues.SetValues(OProduto);
            
            dbEntry.ignoreFields(new[] {"valorGanhoDiario", "qtdeDiasDuracao", "qtdePontosPlanoCarreira"});

            db.SaveChanges();

            return (OProduto.id > 0);

        }

        //Verificar existencia de produtos com mesmo nome para evitar duplicidades
        public bool existe(string nome, int id) {

            var query = this.query();

            query = query.Where(x => x.nome == nome && x.id != id);

            query = query.condicoesSeguranca();

            var OProduto = query.AsNoTracking().Take(1).FirstOrDefault();

            return (OProduto != null);
        }

        //Alteracao de status
        public JsonMessageStatus alterarStatus(int id) {
            var retorno = new JsonMessageStatus();

            var item = this.carregar(id);

            if (item == null) {
                retorno.error = true;
                retorno.message = NotificationMessages.invalid_register_id;
            } else {
                item.ativo = (item.ativo != true);
                db.SaveChanges();
                retorno.active = item.ativo == true ? "S" : "N";
                retorno.message = NotificationMessages.updateSuccess;
            }
            return retorno;
        }

        //Remover o registro do sistema (exclusao lógico - não se remove fiscamente do banco de dados)
        public UtilRetorno excluir(int id) {

            var Objeto = this.carregar(id);

            if (Objeto == null) {
                return UtilRetorno.newInstance(true, "O registro informado não foi localizado.");
            }

            Objeto.flagExcluido = true;
            
            Objeto.dtAlteracao = DateTime.Now;;

            Objeto.idUsuarioAlteracao = User.id();

            db.SaveChanges();

            return UtilRetorno.newInstance(false, "Registro removido com sucesso.");

        }

        public void atualizarEstoque(int idProduto, int quantidadeOperacao, string tipoAcao = "add") {

            if (tipoAcao == "add") {
                db.Produto
                    .Where(x => x.id == idProduto)
                    .Update(x => new Produto { qtde = (x.qtde + quantidadeOperacao) });
            } else {
                db.Produto
                    .Where(x => x.id == idProduto)
                    .Update(x => new Produto { qtde = (x.qtde - quantidadeOperacao) });
            }
        }
        
    }
}