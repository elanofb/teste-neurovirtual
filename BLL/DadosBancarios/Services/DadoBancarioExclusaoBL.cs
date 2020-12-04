using System;
using System.Linq;
using BLL.DadosBancarios.Interfaces;
using BLL.Services;
using DAL.DadosBancarios;
using DAL.Permissao.Security.Extensions;
using EntityFramework.Extensions;

namespace BLL.DadosBancarios.Services {
    
    public class DadoBancarioExclusaoBL : DefaultBL, IDadoBancarioExclusaoBL {
        
        public bool excluir(int[] ids) {

            db.DadoBancario.condicoesSeguranca()
                .Where(x => ids.Contains(x.id))
                .Update(x => new DadoBancario {
                    dtExclusao = DateTime.Now,
                    idUsuarioExclusao = User.id()
                });

            return true;
        }
    }
}