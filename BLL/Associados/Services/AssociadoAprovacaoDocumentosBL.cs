using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

using BLL.Services;

using DAL.Associados;
using DAL.Permissao.Security.Extensions;

namespace BLL.Associados.Interface {
    
    public class AssociadoAprovacaoDocumentosBL : DefaultBL, IAssociadoAprovacaoDocumentosBL {
        
        public UtilRetorno aprovacaoDocumentos(int idAssociado) {
            UtilRetorno Retorno = new UtilRetorno();
            
            Associado   Old     = db.Associado.Find(idAssociado);
            
            if (Old == null) {
                Retorno.flagError = true;
                Retorno.listaErros.Add("Não foi possível encontrar o associado");

                return Retorno;
            }
            
            DbEntityEntry<Associado> entry = db.Entry(Old);

            var status = Old.dtAprovacaoDocumento.isEmpty();
            
            if(status) {
               Old.idUsuarioAprovacaoDocumento = User.id();
               Old.dtAprovacaoDocumento        = DateTime.Now;
            } else {
                Old.idUsuarioAprovacaoDocumento = null;
                Old.dtAprovacaoDocumento        = null;
            }
            
            entry.CurrentValues.SetValues(Old);
            entry.State = EntityState.Modified;
            
            db.SaveChanges();
            
            var strStatus = status ? "realizada" : "desfeita";
            
            return UtilRetorno.newInstance(false, $"Aprovação de documentos {strStatus} com sucesso!");
        }
    }
}