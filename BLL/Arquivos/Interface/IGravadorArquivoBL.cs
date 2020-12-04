using System;
using System.IO;
using System.Web;
using DAL.Arquivos;

namespace BLL.Arquivos {

    public interface IGravadorArquivoBL {
        /// <summary>
        /// 
        /// </summary>
        UtilRetorno salvar(ArquivoUpload OArquivoUpload, HttpPostedFileBase OArquivo, string subFolder);

        /// <summary>
        /// 
        /// </summary>
        FileInfo gravarArquivo(HttpPostedFileBase OArquivo, string subFolder);
    }

}