using System;
using System.Linq;
using System.Data.Entity;
using DAL.Funcionarios;
using BLL.Services;

namespace BLL.Funcionarios {

	public class FuncionarioConsultaBL : DefaultBL, IFuncionarioConsultaBL {

		//
		public FuncionarioConsultaBL() {
		}
		
		// 
		public IQueryable<Funcionario> query(int? idOrganizacaoParam = null) {
            
			var query = from FUNC in db.Funcionario
				where FUNC.flagExcluido == "N"
				select FUNC;
            
			if (idOrganizacaoParam == null) {
				idOrganizacaoParam = idOrganizacao;
			}

			if (idOrganizacaoParam > 0) {
				query = query.Where(x => x.idOrganizacao == idOrganizacaoParam);
			}

			return query;

		}

		//Carregar Funcionario pelo CNPJ
		public Funcionario carregar(int id) {
            
			var query = (from Emp in 
							 db.Funcionario 
							 .Include(x => x.Pessoa)
						 where 
							Emp.id == id && 
							Emp.flagExcluido == "N" 
						 select Emp
						 );

            query = query.condicoesSeguranca();

			return query.FirstOrDefault();
		}

		//Buscar Funcionario pelo CNPJ
		public Funcionario carregar(string cnpj) {

			string cnpjLimpo = UtilString.onlyNumber(cnpj);

			var query = (from OFuncionario in db.Funcionario 
							                .Include(x => x.Pessoa)
                                            .Include(x => x.Cargo)
						 where 
							OFuncionario.Pessoa.nroDocumento == cnpjLimpo && 
							OFuncionario.flagExcluido == "N" 
						 select OFuncionario
						 );

            query = query.condicoesSeguranca();

			return query.FirstOrDefault();
		}

		
		//Listagem das Funcionarios com possibilidade de busca pelos filtros
		public IQueryable<Funcionario> listar(string valorBusca, string ativo) {

			var query = db.Funcionario
							.Include(x => x.Pessoa)
                            .Include(x => x.Cargo)
							.Where(x => x.flagExcluido == "N");

            query = query.condicoesSeguranca();

			if (!String.IsNullOrEmpty(valorBusca)) {

                string nroDocumento = UtilString.onlyNumber(valorBusca);

				query = query.Where(x => x.Pessoa.nome.Contains(valorBusca) ||  x.Pessoa.nroDocumento.Contains(nroDocumento) || x.Pessoa.razaoSocial.Contains(valorBusca));
			}

			if (!String.IsNullOrEmpty(ativo)) {
				query = query.Where(x => x.ativo == ativo);
			}

			return query;
		}

		//Verificar existencia de dados para evitar duplicidades
		public bool existe(string cnpj, string email, int idDesconsiderado) {

			var query = from Emp in db.Funcionario.AsNoTracking()
							.Include(x => x.Pessoa)
						where
							Emp.id != idDesconsiderado && 
							Emp.flagExcluido == "N"
						select 
							Emp;

            query = query.condicoesSeguranca();

			if (!String.IsNullOrEmpty(cnpj)) { 
				cnpj = UtilString.onlyAlphaNumber(cnpj);
				query = query.Where(x => x.Pessoa.nroDocumento == cnpj);
			}

			if (!String.IsNullOrEmpty(email)) { 
				query = query.Where(x => x.Pessoa.emailPrincipal == email || x.Pessoa.emailSecundario == email);
			}

			bool flagExiste = (query.Any());
			return flagExiste;
		}

	}
}