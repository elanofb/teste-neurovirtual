using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using BLL.Services;
using DAL.Permissao.Security.Extensions;

namespace BLL.Arquivos {

    public class ExportarArquivoAssociadoVWBL : DefaultBL, IExportarArquivoAssociadoVWBL {

        public string exportar(List<string> listaUrlArquivosAssociados) {
            
            string baseName = String.Concat("ARQUIVOS_", User.id(), "_", UtilString.onlyNumber(DateTime.Now.ToString()));
            string nomePathTempArquivo = String.Concat(UtilConfig.pathAbsTempFiles, "/", baseName);

            if (!Directory.Exists(nomePathTempArquivo)) {
                Directory.CreateDirectory(nomePathTempArquivo);
            }

            foreach (var urlArquivoAssociado in listaUrlArquivosAssociados) {
                
                string basePath = User.idOrganizacao() > 0 ? UtilConfig.pathAbsUpload(idOrganizacao) : $"{UtilConfig.pathAbsUploadFiles}upload/";
                
                string pathArquivoAssociado = String.Concat(basePath,urlArquivoAssociado);

                string pathFile = Path.Combine(UtilConfig.pathAbsRaiz, pathArquivoAssociado);

                if (!File.Exists(pathFile)) {
                    continue;
                }

                var OFileInfo = new FileInfo(pathFile);
                UtilIO.copiarArquivo(OFileInfo, nomePathTempArquivo, OFileInfo.Name);
            }

            string nomeArquivoZip = String.Concat(baseName, ".zip");
            string nomeFullArquivoZip = string.Concat(UtilConfig.pathAbsTempFiles, "/", nomeArquivoZip);

            ZipFile.CreateFromDirectory(nomePathTempArquivo, nomeFullArquivoZip, CompressionLevel.Optimal, true);

            var DirectoryInfo = new DirectoryInfo(nomePathTempArquivo);
            DirectoryInfo.Delete(true);

            return nomeArquivoZip;
        }
    }
}