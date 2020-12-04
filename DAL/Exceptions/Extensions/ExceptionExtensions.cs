using System.Data.Entity.Core;
using System.Data.Entity.Validation;
using System.Text;

namespace DAL.Exceptions.Extensions {

    public static class ExceptionExtensions {

        /// <summary>
        /// 
        /// </summary>
        public static StringBuilder carregarDescricaoErro(this DbEntityValidationException ex) {
            
            StringBuilder ErroDescricao = new StringBuilder();

            foreach (var eve in ex.EntityValidationErrors) {

                ErroDescricao.AppendLine($"Entidade do tipo {eve.Entry.Entity.GetType().Name} no estado {eve.Entry.State} tem os seguintes erros de validação: ");

                foreach (var ve in eve.ValidationErrors) {

                    string erroItem = $" Erro: {ve.ErrorMessage}";

                    ErroDescricao.AppendLine(erroItem);
                }
            }

            return ErroDescricao;
        }
        
        /// <summary>
        /// 
        /// </summary>
        public static StringBuilder carregarDescricaoErro(this UpdateException ex) {
            
            StringBuilder ErroDescricao = new StringBuilder();

            /*foreach (var eve in ex.) {

                ErroDescricao.AppendLine($"Entidade do tipo {eve.Entry.Entity.GetType().Name} no estado {eve.Entry.State} tem os seguintes erros de validação: ");

                foreach (var ve in eve.ValidationErrors) {

                    string erroItem = $" Erro: {ve.ErrorMessage}";

                    ErroDescricao.AppendLine(erroItem);
                }
            }*/

            return ErroDescricao;
        }        
    }

}
