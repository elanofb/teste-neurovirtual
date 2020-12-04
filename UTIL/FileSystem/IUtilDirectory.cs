using System.IO;

namespace UTIL.FileSystem {

    public interface IUtilDirectory {
        
        /// <summary>
        /// Retorna o diretorio base da aplicacao
        /// </summary>
        string obterDiretorioAplicacao();

        /// <summary>
        /// Obter o diretorio de uploads da organizacao
        /// </summary>
        string obterDiretorioUploadOrganizacao(int idOrganizacao, bool flagIncludeBase = true);

        /// <summary>
        /// Local onde deve ser criada a pasta
        /// </summary>
        DirectoryInfo criarDiretorio(string pathDiretorio);

    }

}