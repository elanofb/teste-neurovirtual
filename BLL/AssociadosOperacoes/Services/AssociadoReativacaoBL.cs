using System;
using System.Collections.Generic;
using System.Linq;
using BLL.AssociadosOperacoes.Events;
using BLL.Core.Events;
using BLL.Services;
using DAL.Permissao.Security.Extensions;

namespace BLL.AssociadosOperacoes {

    public class AssociadoReativacaoBL : DefaultBL, IAssociadoReativacaoBL {

        // Events
        private readonly EventAggregator onAssociadoAtivado = OnAssociadoAtivado.getInstance;
        
        //Reativar um associado que estava com status inativo
        public UtilRetorno reativarAssociados(List<int> idsAssociados, string observacoes) {
            
            var listaAssociados = db.Associado.condicoesSeguranca().Where(x => idsAssociados.Contains(x.id)).ToList();

            if (!listaAssociados.Any()) {
                return UtilRetorno.newInstance(true, "Nenhum associado foi encontrado.");
            }

            int idUsuarioLogado = User.id();

            listaAssociados.ForEach(x => {

                x.dtReativacao = DateTime.Now;

                x.idUsuarioReativacao = idUsuarioLogado;

                x.ativo = "S";

                x.Pessoa.ativo = "S";

                x.Pessoa.flagExcluido = "N";
                
            });
            
            db.SaveChanges();

            listaAssociados.ForEach(x => {

                x.observacoes = observacoes;
                
            });
            
            this.onAssociadoAtivado.subscribe( new OnAssociadoAtivadoHandler() );

            this.onAssociadoAtivado.publish( (listaAssociados as object) );

            return UtilRetorno.newInstance(false, "Associado(s) reativado(s) com sucesso.");
        }

    }

}
