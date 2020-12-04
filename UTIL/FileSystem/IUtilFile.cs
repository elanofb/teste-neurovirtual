using System.IO;

namespace UTIL.FileSystem {

    public interface IUtilFile {
        
        /// <summary>
        /// Cria um novo arquivo com base nas informacoes passadas
        /// Pode sobrepor um arquivo existente
        /// </summary>
        FileInfo criarArquivo(ArquivoInfo DadosArquivo);

        /// <summary>
        /// Gerar conteudo em um arquivo existente
        /// </summary>
        bool escrever(string pathArquivo, string conteudoArquivo);
    }

}