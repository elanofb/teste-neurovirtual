using System;
using System.IO;
using System.Web;
using BLL.Services;
using DAL.Arquivos;
using UTIL.FileSystem;

namespace BLL.Arquivos {

    public class GravadorArquivoBL : DefaultBL, IGravadorArquivoBL {
        
        //Dependencias
        private IUtilDirectory UtilDirectory { get; }
        private IArquivoUploadCadastroBL ArquivoUploadCadastroBL { get; }

        /// <summary>
        /// 
        /// </summary>
        public GravadorArquivoBL(IUtilDirectory _UtilDirectory, 
                                 IArquivoUploadCadastroBL _ArquivoUploadCadastroBL) {

            UtilDirectory = _UtilDirectory;

            ArquivoUploadCadastroBL = _ArquivoUploadCadastroBL;
        }

        /// <summary>
        /// 
        /// </summary>
        public UtilRetorno salvar(ArquivoUpload OArquivoUpload, HttpPostedFileBase OArquivo, string subFolder) {

            var OArquivoSalvo = this.gravarArquivo(OArquivo, subFolder);

            if (!OArquivoSalvo.Exists) {
                
                return UtilRetorno.newInstance(true, "Não foi possível salvar o arquivo em disco.");
            }

            OArquivoUpload.extensao = OArquivoSalvo.Extension;

            OArquivoUpload.path = $"{subFolder}/{OArquivoSalvo.Name}";
            
            OArquivoUpload.pathThumb = OArquivoUpload.path;

            OArquivoUpload.contentType = OArquivo.ContentType;
            
            OArquivoUpload.nomeArquivo = OArquivoSalvo.Name;

            var OArquivoUploadSalvo = this.ArquivoUploadCadastroBL.salvar(OArquivoUpload);

            if (OArquivoUploadSalvo == null) {
                
                OArquivoSalvo.Delete();
                
                return UtilRetorno.newInstance(true, "Não foi possível salvar os dados do arquivo.");
                
            }

            return UtilRetorno.newInstance(false, "", OArquivoUploadSalvo);
            
        }
        
        /// <summary>
        /// 
        /// </summary>
        public FileInfo gravarArquivo(HttpPostedFileBase OArquivo, string subFolder) {

            string basePathFull = UtilDirectory.obterDiretorioUploadOrganizacao(this.idOrganizacao);
            
            string pathArquivo = Path.Combine(basePathFull, subFolder);

            if (!Directory.Exists(pathArquivo)) {

                UtilDirectory.criarDiretorio(pathArquivo);
                
            }

            string nomeArquivo = OArquivo.FileName;
            
            string nomeArquivoCompleto = Path.Combine(pathArquivo, nomeArquivo);

            int cont = 1;
            
            while (File.Exists(nomeArquivoCompleto)) {
                
                nomeArquivo = String.Concat(Path.GetFileNameWithoutExtension(OArquivo.FileName), "_", cont.ToString().PadLeft(2, '0'), Path.GetExtension(OArquivo.FileName));
                
                nomeArquivoCompleto = Path.Combine(pathArquivo, nomeArquivo);
                
                cont++;
            }

            OArquivo.SaveAs(nomeArquivoCompleto);

            return new FileInfo(nomeArquivoCompleto);            
        }
    }

}
