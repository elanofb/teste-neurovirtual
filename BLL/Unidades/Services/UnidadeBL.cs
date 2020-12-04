using System;
using System.Data.Entity;
using System.Json;
using System.Linq;
using System.Web;
using BLL.Arquivos;
using DAL.Entities;
using DAL.Unidades;
using EntityFramework.Caching;
using EntityFramework.Extensions;
using UTIL.Resources;
using DAL.Pessoas;
using DAL.Documentos;
using DAL.Permissao.Security.Extensions;
using BLL.Services;
using DAL.Arquivos;

namespace BLL.Unidades {

	public class UnidadeBL : DefaultBL, IUnidadeBL {

		// Atributos
        private IArquivoUploadPadraoBL _IArquivoUploadPadraoBL;

        // Propriedades
	    private IArquivoUploadPadraoBL OArquivoUploadPadraoBL => _IArquivoUploadPadraoBL = _IArquivoUploadPadraoBL ?? new ArquivoUploadLogotipoBL();

		//
		public UnidadeBL() { }

		//Carregar registro a partir do ID
		public Unidade carregar(int id, bool flagCache = false) {
			
			var query = from Unid in db.Unidade.Include(x => x.Pessoa)
                        .Include(x => x.Pessoa.listaEnderecos)
                        where Unid.id == id && Unid.flagExcluido == "N"
						select Unid;

		    query = query.condicoesSeguranca();

			if (flagCache) {
				return query.FromCache(CachePolicy.WithDurationExpiration(TimeSpan.FromHours(1))).FirstOrDefault();
			}

			return query.FirstOrDefault();
		}

		//Listagem de registro a partir de parametros
		public IQueryable<Unidade> listar(string valorBusca, string ativo, bool flagTodasUnidades = true) {
			
			var query = from Unid in db.Unidade
										.Include(x => x.Pessoa)
						where Unid.flagExcluido == "N"
						select Unid;

            query = query.condicoesSeguranca();

			if (!String.IsNullOrEmpty(valorBusca)) {

			    string valorBuscaClean = valorBusca.onlyNumber();

				query = query.Where(x => x.Pessoa.nome.Contains(valorBusca) || 
                                        x.sigla.Contains(valorBusca) || 
                                        (x.Pessoa.nroDocumento == valorBuscaClean && x.Pessoa.nroDocumento != "") ||
                                        x.Pessoa.listaEnderecos.Any(o => o.logradouro.Contains(valorBusca) || o.cep.Contains(valorBusca)));
			}

			if (!String.IsNullOrEmpty(ativo)) {
				query = query.Where(x => x.ativo == ativo);
			}
            
			if (!flagTodasUnidades){
			    var idsUnidades = User.idsUnidades();
				query = query.Where(x => idsUnidades.Contains(x.id));
			}

			return query;
		}

		//Salvar dados e imagem (se for enviado)
		public bool salvar(Unidade OUnidade, HttpPostedFileBase Logotipo) {

		    OUnidade.sigla = OUnidade.sigla.abreviar(20);

            OUnidade.Pessoa = OUnidade.Pessoa.limparAtributos();
            
            bool flagSucesso = false;

		    if (OUnidade.id > 0){
		        flagSucesso = this.atualizar(OUnidade);		        
		    }

		    if (OUnidade.id == 0){
		        flagSucesso = this.inserir(OUnidade);
		    }

			if (flagSucesso && Logotipo != null) {
                
                var OArquivo = new ArquivoUpload();

			    OArquivo.idReferenciaEntidade = OUnidade.id;

			    OArquivo.entidade = EntityTypes.UNIDADE;

			    this.OArquivoUploadPadraoBL.salvar(OArquivo, Logotipo);

			}

            return (flagSucesso);
		}

        private bool inserir(Unidade OUnidade) {

            OUnidade.setDefaultInsertValues();

            OUnidade.Pessoa.setDefaultInsertValues();

            OUnidade.Pessoa.listaEnderecos.ToList().ForEach(e => { e.setDefaultInsertValues(); });
            
            db.Unidade.Add(OUnidade);
            db.SaveChanges();

            return (OUnidade.id > 0);
        }

        //Atualizar os dados de um associado e os objetos relacionados
        private bool atualizar(Unidade OUnidade) {

            Unidade dbUnidade = this.carregar(OUnidade.id);

            if (dbUnidade == null) {
                return false;
            }

            var entryUnidade = db.Entry(dbUnidade);
            OUnidade.setDefaultUpdateValues();
            entryUnidade.CurrentValues.SetValues(OUnidade);
            entryUnidade.State = EntityState.Modified;
            entryUnidade.ignoreFields(new[] { "idPessoa", "ativo"});

            var entryPessoa = db.Entry(dbUnidade.Pessoa);
            OUnidade.Pessoa.setDefaultUpdateValues();
            OUnidade.Pessoa.id = dbUnidade.Pessoa.id;
            OUnidade.Pessoa.idUsuarioAlteracao = UtilNumber.toInt32(OUnidade.idUsuarioAlteracao);
            entryPessoa.CurrentValues.SetValues(OUnidade.Pessoa);
            entryPessoa.State = EntityState.Modified;
            entryPessoa.ignoreFields<Pessoa>();

            this.atualizarEnderecos(OUnidade, dbUnidade);

            db.SaveChanges();

            return (OUnidade.id > 0);
        }

        private void atualizarEnderecos(Unidade OUnidade, Unidade dbUnidade) {

            foreach (var OPessoaEndereco in OUnidade.Pessoa.listaEnderecos) {

                OPessoaEndereco.idUsuarioAlteracao = UtilNumber.toInt32(OUnidade.idUsuarioAlteracao);
                var dbEndereco = dbUnidade.Pessoa.listaEnderecos.FirstOrDefault(e => e.id == OPessoaEndereco.id);

                if (dbEndereco != null) {
                    var EntryEndereco = db.Entry(dbEndereco);
                    OPessoaEndereco.setDefaultUpdateValues();
                    EntryEndereco.CurrentValues.SetValues(OPessoaEndereco);
                    EntryEndereco.ignoreFields(new[] { "idPessoa" });
                    EntryEndereco.State = EntityState.Modified;
                } else {
                    OPessoaEndereco.idPessoa = dbUnidade.idPessoa;
                    OPessoaEndereco.setDefaultInsertValues();
                    db.PessoaEndereco.Add(OPessoaEndereco);
                    db.SaveChanges();
                }
            }
        }

        //Alteracao de status
        public JsonMessageStatus alterarStatus(int id) {
			var retorno = new JsonMessageStatus();

			Unidade item = this.carregar(id);

			if (item == null) {
				retorno.error = true;
				retorno.message = NotificationMessages.invalid_register_id;
			} else {
				item.ativo = (item.ativo == "S" ? "N" : "S");
				db.SaveChanges();
				retorno.active = item.ativo;
				retorno.message = NotificationMessages.updateSuccess;
			}
			return retorno;
		}

        //Verificar se já existe um registro com o documento/email informado, que possua id diferente do informado
        public bool existe(string nroDocumento, int idDesconsiderado) {

            nroDocumento = UtilString.onlyNumber(nroDocumento);

            var query = from Ass in db.Unidade
                                    .Include(x => x.Pessoa)
                        where
                            Ass.id != idDesconsiderado &&
                            Ass.flagExcluido == "N" && 
                            Ass.Pessoa.flagExcluido == "N"
                        select Ass;

            if (!String.IsNullOrEmpty(nroDocumento)) {
                query = query.Where(x => x.Pessoa.nroDocumento == nroDocumento && x.Pessoa.idTipoDocumento == TipoDocumentoConst.CNPJ);
            }

            query = query.condicoesSeguranca();

            var OUnidade = query.Take(1).FirstOrDefault();

            return (OUnidade != null);
        }

        //Excluir um registro a partir de um array  de ids
        public JsonMessage delete(int[] id) {

			int idUsuarioAlteracao = User.id();
			this.db.Unidade.Where(x => (id.Contains(x.id))).Update(x => new Unidade { flagExcluido = "S", dtAlteracao = DateTime.Now, idUsuarioAlteracao = idUsuarioAlteracao });

			return new JsonMessage { error = false, message = NotificationMessages.delete_success };
		}
	}
}