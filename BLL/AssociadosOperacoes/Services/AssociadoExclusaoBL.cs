using System;
using System.Collections.Generic;
using System.Linq;
using BLL.AssociadosOperacoes.Events;
using BLL.Core.Events;
using BLL.Services;
using DAL.Permissao.Security.Extensions;

namespace BLL.AssociadosOperacoes {

    public class AssociadoExclusaoBL : DefaultBL, IAssociadoExclusaoBL {

        // Events
        private readonly EventAggregator onAssociadoExcluido = OnAssociadoExcluido.getInstance;
        
        //Excluir um associado
        public UtilRetorno excluirAssociados(List<int> idsAssociados, int idMotivoDesligamento, string observacoes) {
            
            var listaAssociados = db.Associado.Where(x => idsAssociados.Contains(x.id)).ToList();

            if (!listaAssociados.Any()) {
                return UtilRetorno.newInstance(true, "Nenhum associado foi encontrado.");
            }

            int idUsuarioLogado = User.id();

            listaAssociados.ForEach(x => {

                x.dtExclusao = DateTime.Now;

                x.idUsuarioExclusao = idUsuarioLogado;

                x.idMotivoDesligamento = idMotivoDesligamento;

                x.observacaoDesligamento = observacoes;

                x.ativo = "N";

                x.Pessoa.ativo = "N";

                x.Pessoa.flagExcluido = "S";
                
            });
            
            db.SaveChanges();
			
            this.onAssociadoExcluido.subscribe( new AssociadoExcluidoHandler() );
            this.onAssociadoExcluido.publish( ((object) listaAssociados) );
			
            return UtilRetorno.newInstance(false, "Associado(s) desligado(s) com sucesso.");
        }

    }

}
