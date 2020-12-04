using System;
using System.Data.Entity;
using System.Json;
using System.Linq;
using System.Web;
using BLL.Arquivos;
using DAL.Organizacoes;
using EntityFramework.Extensions;
using DAL.Pessoas;
using BLL.Services;
using DAL.Arquivos;
using DAL.Entities;
using DAL.Permissao;
using DAL.Permissao.Security.Extensions;
using UTIL.Resources;

namespace BLL.Organizacoes {

	public class OrganizacaoBL : DefaultBL, IOrganizacaoBL {

		// Atributos
	    private IArquivoUploadPadraoBL _IArquivoUploadPadraoBL;

        // Propriedades
	    private IArquivoUploadPadraoBL OArquivoUploadPadraoBL => _IArquivoUploadPadraoBL = _IArquivoUploadPadraoBL ?? new ArquivoUploadLogotipoBL();

        //
        public OrganizacaoBL(){
		}

		//Carregar registro a partir do ID
		public Organizacao carregar(int id) {
			
			var query = from Org in db.Organizacao
                                      .Include(x => x.Pessoa)
                                      .Include(x => x.Pessoa.listaEnderecos)
                        where Org.id == id && Org.dtExclusao == null
						select Org;


			return query.FirstOrDefault();
		}

		//Listagem de registro a partir de parametros
		public IQueryable<Organizacao> listar(string valorBusca, bool? ativo, bool flagTodasOrganizacoes = false) {

            var query = from Unid in db.Organizacao
										.Include(x => x.Pessoa)
										.Include(x => x.OrganizacaoGestora)
                                        .Include(x => x.StatusOrganizacao)
						where Unid.dtExclusao == null
						select Unid;

			if (!string.IsNullOrEmpty(valorBusca)) {
				query = query.Where(x => x.Pessoa.nome.Contains(valorBusca) || x.Pessoa.nroDocumento.Contains(valorBusca));
			}

		    if (idOrganizacao > 0 && !(flagTodasOrganizacoes && (User.idPerfil() == PerfilAcessoConst.DESENVOLVEDOR || User.flagMultiOrganizacao()))) {
		        query = query.Where(x => x.id == idOrganizacao);
		    }

            if (ativo.HasValue) {
				query = query.Where(x => x.ativo == ativo);
			}

			return query;
		}

		/// <summary>
        /// Enviar para salvar ou atualizar um registro no banco de dados e salvar o logotipo da Organizacao
        /// </summary>
		public bool salvar(Organizacao OOrganizacao, HttpPostedFileBase Logotipo) {

            OOrganizacao.Pessoa = OOrganizacao.Pessoa.limparAtributos();

            bool flagSucesso = false;

            if (OOrganizacao.id  > 0) {
                flagSucesso = this.atualizar(OOrganizacao);
            }

		    if (OOrganizacao.id == 0) {
		        flagSucesso = this.inserir(OOrganizacao);
            }

			if (flagSucesso && Logotipo != null) {

                var OArquivo = new ArquivoUpload();

			    OArquivo.idReferenciaEntidade = OOrganizacao.id;

			    OArquivo.entidade = EntityTypes.ORGANIZACAO;
                
			    this.OArquivoUploadPadraoBL.salvar(OArquivo, Logotipo);

			}

            return (flagSucesso);
		}

        /// <summary>
        /// Cadastro de um novo registro no banco de dados
        /// </summary>
        private bool inserir(Organizacao OOrganizacao) {

            OOrganizacao.setDefaultInsertValues();

            OOrganizacao.Pessoa.setDefaultInsertValues();

            OOrganizacao.Pessoa.listaEnderecos.ToList().ForEach(e => { e.setDefaultInsertValues(); });
            
            db.Organizacao.Add(OOrganizacao);

            db.SaveChanges();

            return (OOrganizacao.id > 0);
        }

        //Atualizar os dados de um associado e os objetos relacionados
        private bool atualizar(Organizacao OOrganizacao) {

            Organizacao dbOrganizacao = this.carregar(OOrganizacao.id);

            var entryOrganizacao = db.Entry(dbOrganizacao);
            OOrganizacao.setDefaultUpdateValues();
            entryOrganizacao.CurrentValues.SetValues(OOrganizacao);
            entryOrganizacao.State = EntityState.Modified;
            entryOrganizacao.ignoreFields(new[] { "idPessoa", "ativo"});

            var entryPessoa = db.Entry(dbOrganizacao.Pessoa);
            OOrganizacao.Pessoa.setDefaultUpdateValues();
            OOrganizacao.Pessoa.id = dbOrganizacao.Pessoa.id;
            OOrganizacao.Pessoa.idUsuarioAlteracao = UtilNumber.toInt32(OOrganizacao.idUsuarioAlteracao);
            entryPessoa.CurrentValues.SetValues(OOrganizacao.Pessoa);
            entryPessoa.State = EntityState.Modified;
            entryPessoa.ignoreFields<Pessoa>();

            this.atualizarEnderecos(OOrganizacao, dbOrganizacao);

            db.SaveChanges();

            return (OOrganizacao.id > 0);
        }

        /// <summary>
        /// Atualizacao dos endereços
        /// </summary>
        private void atualizarEnderecos(Organizacao OOrganizacao, Organizacao dbOrganizacao) {

            foreach (var OPessoaEndereco in OOrganizacao.Pessoa.listaEnderecos) {

                OPessoaEndereco.idUsuarioAlteracao = UtilNumber.toInt32(OOrganizacao.idUsuarioAlteracao);
                var dbEndereco = dbOrganizacao.Pessoa.listaEnderecos.FirstOrDefault(e => e.id == OPessoaEndereco.id);

                if (dbEndereco != null) {
                    var EntryEndereco = db.Entry(dbEndereco);
                    OPessoaEndereco.setDefaultUpdateValues();
                    EntryEndereco.CurrentValues.SetValues(OPessoaEndereco);
                    EntryEndereco.ignoreFields(new[] { "idPessoa" });
                    EntryEndereco.State = EntityState.Modified;
                } else {
                    OPessoaEndereco.idPessoa = dbOrganizacao.idPessoa;
                    OPessoaEndereco.setDefaultInsertValues();
                    db.PessoaEndereco.Add(OPessoaEndereco);
                    db.SaveChanges();
                }
            }
        }
        
        /// <summary>
        /// Ativar ou desativar um registro
        /// </summary>
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

        /// <summary>
        ///  Remover o registro de forma lógica no sistema
        /// </summary>
        public UtilRetorno excluir(Organizacao OOrganizacao) {

            db.Organizacao.Where(x => x.id == OOrganizacao.id)
                            .Update(
                                x =>
                                    new Organizacao {
                                        dtExclusao = DateTime.Now,
                                        idUsuarioExclusao = OOrganizacao.idUsuarioExclusao,
                                        motivoExclusao = OOrganizacao.motivoExclusao
                                    });   

			return UtilRetorno.newInstance(false, "O registro foi removido com sucesso.");
		}
	}
}