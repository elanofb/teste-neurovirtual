
namespace DAL.Configuracoes {

	public static class ConfiguracaoSistemaExtensions {

        /// <summary>
        /// 
        /// </summary>
        public static string primeiroDominio(this ConfiguracaoSistema OConfiguracaoSistema) {

            var dominios = OConfiguracaoSistema?.dominios;

            var lista = dominios?.Split('\n');

            if(lista == null) {
                return "";
            }

            return lista[0];

        }	
        
	}
}