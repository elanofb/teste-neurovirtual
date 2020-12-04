using System;
using System.Linq;
using BLL.Services;
using DAL.Financeiro;
using DAL.Permissao.Security.Extensions;
using EntityFramework.Extensions;

namespace BLL.Financeiro {

    public class TituloReceitaExclusaoBL : DefaultBL, ITituloReceitaExclusaoBL{

        //Atributos

        //Propriedades

        //eventos

		/// <summary>
        /// Realizar a exclusao pelo ID do registro
        /// </summary>
		public virtual UtilRetorno excluir(int idTituloReceita, string motivo = "") {

            TituloReceita dbTitulo = this.db.TituloReceita.condicoesSeguranca().FirstOrDefault(x => x.id == idTituloReceita);

            if(dbTitulo == null) {
                return UtilRetorno.newInstance(true, "O título informado não pôde ser localizado.");
            }

            dbTitulo.dtExclusao = DateTime.Now;

            dbTitulo.idUsuarioExclusao = User.id();

		    dbTitulo.motivoExclusao = motivo;

            int qtdeAfetados = this.db.SaveChanges();

		    if (qtdeAfetados == 0){

		        return UtilRetorno.newInstance(true, "Houve um problema ao remover o registro, tente novamente mais tarde.");
		    }

            this.removerParcelas(dbTitulo.id, User.id(), motivo);

            return UtilRetorno.newInstance(false, "O registro foi removido com sucesso.");
        }

        /// <summary>
        /// Realizar a exclusao pelo tipo de receita e pela referencia
        /// </summary>
        public UtilRetorno excluir(int idTipoReceita, int idReferencia, string motivo) {

            TituloReceita dbTitulo = this.db.TituloReceita.condicoesSeguranca().FirstOrDefault(x => x.idTipoReceita == idTipoReceita && x.idReceita == idReferencia);

            if(dbTitulo == null) {
                return UtilRetorno.newInstance(true, "O título informado não pôde ser localizado.");
            }

            return this.excluir(dbTitulo.id, motivo);
        }

        /// <summary>
        /// Remover parcelas do titulo 
        /// </summary>
        private void removerParcelas(int idTitulo, int idUsuario, string motivo = "Usuário mudou forma de pagamento.") {

            this.db.TituloReceitaPagamento
                    .Where(x => x.idTituloReceita == idTitulo && x.dtExclusao == null)
                    .Update(x => new TituloReceitaPagamento {
                        dtExclusao = DateTime.Now,
                        idUsuarioExclusao = idUsuario,
                        motivoExclusao = motivo
                    });
        }

    }
}