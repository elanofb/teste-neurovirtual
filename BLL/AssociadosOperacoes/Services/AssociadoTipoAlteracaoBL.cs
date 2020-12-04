using System;
using System.Collections.Generic;
using System.Linq;
using BLL.AssociadosOperacoes.Events;
using BLL.Core.Events;
using BLL.Services;
using DAL.Permissao.Security.Extensions;

namespace BLL.AssociadosOperacoes {

    public class AssociadoTipoAlteracaoBL : DefaultBL, IAssociadoTipoAlteracaoBL {

        // Events
        private readonly EventAggregator onTipoAlterado = OnTipoAlterado.getInstance;
        
        //
        public UtilRetorno alterarTipo(List<int> idsAssociados, int idTipoAssociado, string observacoes) {
            
            var listaAssociados = db.Associado.condicoesSeguranca().Where(x => idsAssociados.Contains(x.id)).ToList();
            
            if (!listaAssociados.Any()) {
                return UtilRetorno.newInstance(true, "Nenhum associado foi encontrado.");
            }
            
            int idUsuarioLogado = User.id();

            listaAssociados.ForEach(x => {

                x.idTipoAssociado = idTipoAssociado;

                x.idUsuarioAlteracao = idUsuarioLogado;
                
            });
            
            db.SaveChanges();

            listaAssociados.ForEach(x => {

                x.observacoes = observacoes;
                
            });
            
            this.onTipoAlterado.subscribe( new OnTipoAlteradoHandler() );

            this.onTipoAlterado.publish( (listaAssociados as object) );

            return UtilRetorno.newInstance(false, "O tipo dos associados foram alterados com sucesso.");
        }

    }

}
