using System;
using System.Linq;
using BLL.Services;
using DAL.Emails;
using DAL.Permissao.Security.Extensions;
using EntityFramework.Extensions;

namespace BLL.Emails {

    public class MensagemEmailExclusaoBL : MensagemEmailConsultaBL, IMensagemEmailExclusaoBL {
                                        
        public bool excluir(int id) {
            db.MensagemEmail
                .Where(x => x.id == id && x.dtExclusao == null)
                .Update(x => new MensagemEmail { dtExclusao = DateTime.Now, dtAlteracao = DateTime.Now,idUsuarioAlteracao = User.id(), idUsuarioExclusao = User.id() });
            return true;    
        }
        
    }
}