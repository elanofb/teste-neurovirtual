using System;
using System.Collections.Generic;
using System.Linq;
using BLL.Associados.Events;
using BLL.Core.Events;
using BLL.Services;
using DAL.Associados;
using DAL.Permissao.Security.Extensions;
using EntityFramework.Extensions;

namespace BLL.AssociadosOperacoes {

    public class AssociadoSituacaoContribuicaoBL : DefaultBL, IAssociadoSituacaoContribuicaoBL {

        // Events
        private EventAggregator onSituacaoContribuicaoAlterada => OnSituacaoContribuicaoAlterada.getInstance;
        
        //
        public void alterarSituacaoContribuicao(int id, string motivoAlteracao) {

            var OAssociado = db.Associado.FirstOrDefault(x => x.id == id);

            db.SaveChanges();

            motivoAlteracao = $"Alteração manual: {motivoAlteracao}";

            var listaParametros = new List<object>();

            listaParametros.Add(id);

            listaParametros.Add(motivoAlteracao);

            this.onSituacaoContribuicaoAlterada.subscribe(new OnSituacaoContribuicaoAlteradaHandler());

            this.onSituacaoContribuicaoAlterada.publish((listaParametros as object));

        }


	    //Atualizar a situacao financeira do associado
        public bool atualizarSituacaoContribuicao(int idAssociado, string flagSituacaoContribuicaoParam) {

            this.onSituacaoContribuicaoAlterada.subscribe(new OnSituacaoContribuicaoAlteradaHandler());

            int idUsuarioLogado = User.id();

            this.db.Associado.Where(x => x.id == idAssociado)
                            .Update(x => new Associado {

                                idUsuarioAlteracao = idUsuarioLogado,

                                dtAlteracao = DateTime.Now

                            });

            var listaParametros = new List<object>();

            listaParametros.Add(idAssociado);

            string descricaoSituacao = "Alteração via rotina automática";

            listaParametros.Add(descricaoSituacao);

            this.onSituacaoContribuicaoAlterada.publish((listaParametros as object));

            return true;
        }
    }

}
