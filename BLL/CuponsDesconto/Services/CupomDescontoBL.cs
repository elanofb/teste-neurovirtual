using System;
using System.Linq;
using DAL.CuponsDesconto;
using EntityFramework.Extensions;
using System.Collections.Generic;
using System.Json;
using BLL.Services;
using DAL.Permissao.Security.Extensions;

namespace BLL.CuponsDesconto {

	public class CupomDescontoBL : DefaultBL, ICupomDescontoBL {
		
        //
		public IQueryable<CupomDesconto> listar(string valorBusca = "", string ativo = "S") {

			var query = from N in db.CupomDesconto
						where N.flagExcluido == "N"
						select N;

		    query = query.condicoesSeguranca();

			if (!String.IsNullOrEmpty(valorBusca)) {
				query = query.Where(x => x.codigo.Contains(valorBusca) || x.nome.Contains(valorBusca) || x.emailPrincipal.Contains(valorBusca) || x.emailSecundario.Contains(valorBusca));
			}

			if (!String.IsNullOrEmpty(ativo)) {
				query = query.Where(x => x.ativo == ativo);
			}

			return query;
		}

		//
		public CupomDesconto carregar(int id) {
			var query = db.CupomDesconto.Where(x => x.id == id);

		    query = query.condicoesSeguranca();

			return query.FirstOrDefault();
		}

		//
		public CupomDesconto carregarPorCodigo(string codigo) {

			var query = db.CupomDesconto
                .Where(x => x.codigo == codigo
                    && x.flagExcluido == "N"
                    && x.ativo == "S");

		    query = query.condicoesSeguranca();

			return query.FirstOrDefault();
		}

		//Definir se é um insert ou update e enviar o registro para o banco de dados 
		public bool salvar(CupomDesconto OCupomDesconto) {
			
			if (OCupomDesconto.id == 0) {

				return this.inserir(OCupomDesconto);

			} else { 

				return this.atualizar(OCupomDesconto);

			}
		}

		//Persistir e inserir um novo registro 
		private bool inserir(CupomDesconto OCupomDesconto) { 
            
            bool temCupom = false;

            while (!temCupom) {
                OCupomDesconto.codigo = UtilString.randomString(8);

                if (!db.CupomDesconto.Any(x => x.codigo == OCupomDesconto.codigo && x.flagExcluido == "N")) {
                    temCupom = true;
                }
            }

			OCupomDesconto.setDefaultInsertValues<CupomDesconto>();

			db.CupomDesconto.Add(OCupomDesconto);

            db.SaveChanges();

            return OCupomDesconto.id > 0;
		}

		//Persistir e atualizar um registro existente 
		private bool atualizar(CupomDesconto OCupomDesconto) { 

			//Localizar existentes no banco
			CupomDesconto dbCupomDesconto = this.carregar(OCupomDesconto.id);
		    if (dbCupomDesconto == null) {
		        return false;
		    }

			//Configurar valores padrão
			OCupomDesconto.setDefaultUpdateValues<CupomDesconto>();

			//Atualização da Empresa
			var CargoEntry = db.Entry(dbCupomDesconto);
			CargoEntry.CurrentValues.SetValues(OCupomDesconto);

			db.SaveChanges();

			return OCupomDesconto.id > 0;
		}

		//Envia o cupom de desconto por e-mail
		public JsonMessage enviarCupom(int idCupomDesconto) {

			JsonMessage Retorno = new JsonMessage { error = false, message = "Os dados do cupom de desconto foram enviados com sucesso." };

			CupomDesconto OCupomDesconto = this.carregar(idCupomDesconto);

			if (OCupomDesconto == null) {

				Retorno.error = true;

                Retorno.message = "Desculpe, cupom não localizado.";

                return Retorno;
			}

			try {

                List<string> listaEmail = new List<string>();

                if (!String.IsNullOrEmpty(OCupomDesconto.emailPrincipal)) { 
                    listaEmail.Add(OCupomDesconto.emailPrincipal);
                }

                if (!String.IsNullOrEmpty(OCupomDesconto.emailSecundario)) { 
                    listaEmail.Add(OCupomDesconto.emailSecundario);
                }

                IEnvioCupom EnvioEmail = EnvioCupom.factory(OCupomDesconto.idOrganizacao, listaEmail, new List<string>());

                var ORetorno = EnvioEmail.enviar(OCupomDesconto);

                if (ORetorno.flagError) {
                    Retorno = new JsonMessage { error = false, message = "Não foi possível enviar o cupom por e-mail: " + ORetorno.listaErros.FirstOrDefault() };
                }

			} catch (Exception ex) {
				
				string obs = $"Ocorreram problemas para enviar o cupom de desconto de {OCupomDesconto.nome}, cupom: {OCupomDesconto.codigo}, e-mail principal: {OCupomDesconto.emailPrincipal}, e-mail secundário: {OCupomDesconto.emailSecundario}";
				
				Retorno.message = obs;
				
				Retorno.error = true;
				
				UtilLog.saveError(ex, obs);
				
			}

            return Retorno;
			
		}

	    //
		public UtilRetorno excluir(int id) {

            var OCupom = this.carregar(id);

            if (OCupom == null) {
                return UtilRetorno.newInstance(true, "Não foi possível remover esse registro.");
            }

            var flagUsadoQtde = db.TituloReceitaPagamento.Count(x => x.idCupomDesconto == id);

            if (OCupom.flagUtilizado || (OCupom.qtdeUsos > 0 && flagUsadoQtde > 0)) {
                return UtilRetorno.newInstance(true, "Não é possível editar um cupom de desconto que já foi usado.");
            }

            OCupom.flagExcluido = "S";
            OCupom.idUsuarioAlteracao = User.id();
            OCupom.dtAlteracao = DateTime.Now;

            this.db.SaveChanges();

            return UtilRetorno.newInstance(false, "Registro removido com sucesso.");
        }

		//
		public bool registrarUso(int idCupomDesconto) {

		    var cupom = this.carregar(idCupomDesconto);
		    if (cupom == null) {
		        return false;
		    }

			db.CupomDesconto.Where(x => (x.id == idCupomDesconto))
				.Update(x => new CupomDesconto { flagUtilizado = true, dtUso = DateTime.Now });

			return true;
		}
	}
}