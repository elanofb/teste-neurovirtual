using System;
using System.IO;

namespace UTIL.FileSystem {

    public class UtilFile : IUtilFile {
        
        //Dependencias
        private IUtilDirectory UtilDirectory { get;  }

        /// <summary>
        /// Construtor
        /// </summary>
        public UtilFile(IUtilDirectory _UtilDirectory) {

            UtilDirectory = _UtilDirectory;
        }
        
        /// <summary>
        /// Cria um novo arquivo com base nas informacoes passadas
        /// Pode sobrepor um arquivo existente
        /// </summary>
        public FileInfo criarArquivo(ArquivoInfo DadosArquivo) {

            string baseFolder = UtilDirectory.obterDiretorioUploadOrganizacao(DadosArquivo.idOrganizacao);
            
            string pathDiretorio = Path.Combine(baseFolder, DadosArquivo.nomeDiretorioPai);

            UtilDirectory.criarDiretorio(pathDiretorio);

            string pathArquivo = Path.Combine(pathDiretorio, $"{DadosArquivo.nome}{DadosArquivo.extensao}");

            FileInfo NovoArquivo = new FileInfo(pathArquivo);

            if (NovoArquivo.Exists && !DadosArquivo.flagSobreporSeExistir) {
                
                return NovoArquivo;
                
            }
            
            NovoArquivo.Create().Close();
            
            if (!DadosArquivo.conteudoTexto.isEmpty()) {

                this.escrever(NovoArquivo.FullName, DadosArquivo.conteudoTexto);

            }

            return NovoArquivo;
            
        }

        /// <summary>
        /// Gera conteúdo dentro de um arquivo existente
        /// </summary>
        public bool escrever(string pathArquivo, string conteudoArquivo) {

            if (!File.Exists(pathArquivo)) {
                
                return false;
            }

            using (TextWriter Writer = File.AppendText(pathArquivo)) {

                Writer.Write(conteudoArquivo);

                Writer.Close();
            }

            return true;
        }
        
    }

}
