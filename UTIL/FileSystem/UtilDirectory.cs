using System;
using System.IO;

namespace UTIL.FileSystem {

    public class UtilDirectory : IUtilDirectory {
        
        /// <summary>
        /// Retorna o diretorio base da aplicacao
        /// </summary>
        public string obterDiretorioAplicacao() {
            
            return AppDomain.CurrentDomain.BaseDirectory.Replace("bin\\Debug", "");
            
        }

        /// <summary>
        /// Obter o diretorio da organizacao
        /// </summary>
        public string obterDiretorioUploadOrganizacao(int idOrganizacao, bool flagIncludeBase = true) {

            string baseApp = this.obterDiretorioAplicacao();

            string baseAppUpload = "upload";

            string baseAppOrganizacao = UtilConfig.pathOrganizacao(idOrganizacao);

            string diretorio = string.Empty;

            if (flagIncludeBase) {

                diretorio = baseApp;
            }

            diretorio = Path.Combine(diretorio, baseAppUpload, baseAppOrganizacao);
            
            return diretorio;
        }

        /// <summary>
        /// 
        /// </summary>
        public DirectoryInfo criarDiretorio(string pathDiretorio) {
            
            DirectoryInfo Dir = new DirectoryInfo(pathDiretorio);

            if (Dir.Exists) {
                
                return Dir;
            }
            
            Dir.Create();
            
            return Dir;
        }
    }

}
