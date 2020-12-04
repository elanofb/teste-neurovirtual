using System;
using System.Collections.Generic;
using System.Linq;
using BLL.AssociadosOperacoes.Events;
using BLL.Core.Events;
using BLL.Services;
using DAL.Permissao.Security.Extensions;

namespace BLL.AssociadosOperacoes {

    public class AssociadoAdmissaoAlteracaoBL : DefaultBL, IAssociadoAdmissaoAlteracaoBL {

        // Events
        private readonly EventAggregator onAdmissaoAlterada = OnAdmissaoAlterada.getInstance;
        
        //
        public UtilRetorno alterarAdmissao(List<int> idsAssociados, DateTime dtAdmissao, string observacoes) {
            
            var listaAssociados = db.Associado.condicoesSeguranca().Where(x => idsAssociados.Contains(x.id)).ToList();

            if (!listaAssociados.Any()) {
                return UtilRetorno.newInstance(true, "Nenhum associado foi encontrado.");
            }

            int idUsuarioLogado = User.id();

            listaAssociados.ForEach(x => {

                x.dtAdmissao = dtAdmissao;

                x.idUsuarioAlteracao = idUsuarioLogado;
                
            });
            
            db.SaveChanges();

            listaAssociados.ForEach(x => {

                x.observacoes = observacoes;
                
            });
            
            this.onAdmissaoAlterada.subscribe( new OnAdmissaoAlteradaHandler() );

            this.onAdmissaoAlterada.publish( (listaAssociados as object) );

            return UtilRetorno.newInstance(false, "As datas de admissão foram alteradas com sucesso.");
        }

    }

}
