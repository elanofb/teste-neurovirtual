using System;
using System.Collections.Generic;
using System.Linq;
using BLL.AssociadosOperacoes.Events;
using BLL.Core.Events;
using BLL.Services;
using DAL.Permissao.Security.Extensions;

namespace BLL.AssociadosOperacoes {

    public class AssociadoDesativacaoBL : DefaultBL, IAssociadoDesativacaoBL {

        // Events
        private readonly EventAggregator onDesativacao = OnDesativacao.getInstance;
        
        //Desativar um determinado associado
        public UtilRetorno desativarAssociados(List<int> idsAssociados, int idMotivoDesativacao, string motivoDesativacao) {
            
            var listaAssociados = db.Associado.condicoesSeguranca().Where(x => idsAssociados.Contains(x.id)).ToList();

            if (!listaAssociados.Any()) {
                return UtilRetorno.newInstance(true, "Nenhum associado foi encontrado.");
            }

            int idUsuarioLogado = User.id();

            listaAssociados.ForEach(x => {

                x.dtDesativacao = DateTime.Now;

                x.idMotivoDesativacao = idMotivoDesativacao;

                x.idUsuarioDesativacao = idUsuarioLogado;

                x.observacaoDesativacao = motivoDesativacao;

                x.ativo = "N";

                x.Pessoa.ativo = "N";
                
            });
            
            db.SaveChanges();

            this.onDesativacao.subscribe( new OnDesativacaoHandler() );

            this.onDesativacao.publish( (listaAssociados as object) );

            return UtilRetorno.newInstance(false, "Associado desativado com sucesso.");
        }

    }

}
