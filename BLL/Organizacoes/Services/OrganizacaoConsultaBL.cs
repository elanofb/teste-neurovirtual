using System;
using System.Collections.Generic;
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
using DAL.Repository.Base;
using UTIL.Resources;

namespace BLL.Organizacoes {

	public class OrganizacaoConsultaBL : IOrganizacaoConsultaBL {

		// Atributos

        // Propriedades
	    private DataContext db { get;  }

        /// <summary>
        /// Construtor
        /// </summary>
        public OrganizacaoConsultaBL(DataContext _db) {

			db = _db;
		}

		//Carregar registro a partir do ID
		public Organizacao carregar(int id) {
			
			var query = from Org in db.Organizacao
                                      .Include(x => x.Pessoa)
                        where Org.id == id && Org.dtExclusao == null
						select Org;


			return query.FirstOrDefault();
		}

		/// <summary>
		/// Listagem de registro a partir de parametros
		/// </summary>
		public IQueryable<Organizacao> query(int idOrganizacaoParam) {

            var query = from Unid in db.Organizacao.Include(x => x.Pessoa)
						where Unid.dtExclusao == null
						select Unid;

			if (idOrganizacaoParam > 0) {
				
				query = query.Where(x => x.id == idOrganizacaoParam);
				
			}

			return query;
		}


		/// <summary>
		/// Listagem de registro a partir de parametros
		/// </summary>
		public List<Organizacao> listarHabilitadas(int idOrganizacaoParam) {

			
			var lista = this.query(idOrganizacaoParam)
							.Where(x => x.idStatusOrganizacao == StatusOrganizacaoConst.PRODUCAO || x.idStatusOrganizacao == StatusOrganizacaoConst.IMPLANTACAO)
							.Select(x => new {
											x.id,
											x.idOrganizacaoGestora,
											x.idPessoa,
											x.idStatusOrganizacao,
											Pessoa = new {
															x.Pessoa.id,
															x.Pessoa.nome
														}
							}).ToListJsonObject<Organizacao>();

			return lista;
		}
	}
}