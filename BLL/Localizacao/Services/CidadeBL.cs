using BLL.Services;
using DAL.Localizacao;
using System;
using System.Data.Entity;
using System.Json;
using System.Linq;
using UTIL.Resources;

namespace BLL.Localizacao {

    public class CidadeBL : DefaultBL, ICidadeBL {
        
        //Atributos
	    private EstadoBL _EstadoBL;

        //Propriedades
        private EstadoBL OEstadoBL => _EstadoBL = _EstadoBL ?? new EstadoBL();

		//Construtor
		public CidadeBL(){
		}

		//Carregar cidade pelo id informado
		public Cidade carregar(int id) {
			
			return db.Cidade.Find(id);
		}

		//Carregar cidade pelo id informado
		public Cidade carregar(string nomeMunicipio, string uf) {
			
			var query = from C in db.Cidade 
                        where C.flagExcluido == "N" &&
                        C.nomeMunicipio == nomeMunicipio &&
                        C.Estado.sigla == uf
                        select C;

            return query.FirstOrDefault();
		}

		//Carregar a cidade a partir da string do nome
		public Cidade carregarPorNome(string name, int? idEstado) {

		    var query = from Reg in db.Cidade
                        where 
                            Reg.flagExcluido == "N" && 
                            Reg.nome.Contains(name)
		                select Reg;

            if (idEstado > 0) {
				query = query.Where(x => x.idEstado == idEstado);
			}

			return query.FirstOrDefault();
		}

		//Carregar a cidade a partir da string do nome e a sigla da cidade
		public Cidade carregarPorNome(string name, string siglaEstado) {

            int idEstado = 0;

            var Estado = OEstadoBL.listar("", "S").FirstOrDefault(x => x.sigla == siglaEstado);

			if (Estado != null) {
				idEstado = Estado.id;
			}

			return carregarPorNome(name, idEstado);
		}

		//criar query para busca de cidades de acordo com parametros informados
		//Pode-se incrementar condições após o retorno
		public IQueryable<Cidade> listar(int idEstado, string valorBusca, string ativo) {

			
			var query = from C in db.Cidade.Include(x => x.Estado) where C.flagExcluido == "N" select C;

			if (idEstado > 0) {
				query = query.Where(x => x.idEstado == idEstado);
			}

			if (!string.IsNullOrEmpty(valorBusca)) {

                var valorBuscaNumerico = UtilNumber.toInt32(valorBusca);

                query = query.Where(x => x.nome.Contains(valorBusca) || x.idMunicipioIBGE == valorBuscaNumerico);
			}

			if (!string.IsNullOrEmpty(ativo)) {
				query = query.Where(x => x.ativo == ativo);
			}

			return query;
		}

		/// <summary>
        /// Capturar uma lista de cidade
        /// </summary>
		public IQueryable<Cidade> listar(int idCidade) {

			
			var query = from C in db.Cidade.Include(x => x.Estado)
                        where
                            C.idEstado == db.Cidade.Where(c => c.id == idCidade).Select(x => x.idEstado).FirstOrDefault() && 
                            C.flagExcluido == "N" && 
                            C.ativo == "S"
                        select C;


			return query;
		}

		//criar query para utilizacao em autocompletes
		//pode-se incremetar a query após o retorno
		public IQueryable<CidadeAutoComplete> autocompletar(int idEstado, string valorBusca) {
			
			var query = (from C in db.Cidade 
						 where 
							C.nomeMunicipio.StartsWith(valorBusca) && 
							C.flagExcluido == "N" 
						 select 
							new CidadeAutoComplete { 
										id = C.id, 
										idEstado = C.idEstado, 
										nome = C.nomeMunicipio, 
										value = C.nomeMunicipio, 
										label = C.nomeMunicipio
							}
						);

			if (idEstado > 0) { 
				query = query.Where(x => x.idEstado == idEstado);
			}
			
			return query;
		}

        //Verificar se deve-se atualizar um registro existente ou criar um novo
        public bool salvar(Cidade OCidade) {

            if(OCidade.id == 0) {
                return this.inserir(OCidade);
            }

            return this.atualizar(OCidade);
        }

        //Persistir o objecto e salvar na base de dados
        private bool inserir(Cidade OCidade) {
            
            OCidade.setDefaultInsertValues();
            db.Cidade.Add(OCidade);
            db.SaveChanges();

            return (OCidade.id > 0);
        }

        //Persistir o objecto e atualizar informações
        private bool atualizar(Cidade OCidade) {
            
            OCidade.setDefaultUpdateValues();

            //Localizar existentes no banco
            Cidade dbCidade = this.carregar(OCidade.id);
            var TipoEntry = db.Entry(dbCidade);
            TipoEntry.CurrentValues.SetValues(OCidade);
            TipoEntry.ignoreFields();

            db.SaveChanges();
            return (OCidade.id > 0);
        }

        //Ativacao - Desativacao de registro
		public JsonMessageStatus alterarStatus(int id) {
			var retorno = new JsonMessageStatus();

			Cidade item = this.carregar(id);
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

        //Remover um registro (exclusao lógica - nao remove-se fisicamente)
        public UtilRetorno excluir(int id) {
            
            var OCidade = this.carregar(id);

		    if (OCidade == null) {
		        return UtilRetorno.newInstance(true, "A cidade informada não foi localizado.");
		    }

            OCidade.flagExcluido = "S";

		    db.SaveChanges();

            return UtilRetorno.newInstance(false, "Registro removido com sucesso.");
            
        }
        

	}
}