using BLL.Core.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using DAL.Pessoas;
using BLL.Services;
using DAL.Notificacoes;
using DAL.Relacionamentos;
using DAL.Repository.Base;

namespace BLL.AssociadosOperacoes.Events {

    public class OnNovaSenhaEnviadaHandler : DefaultBL, IHandler<object> {
        
        // Propriedades
        private List<NotificacaoSistemaEnvio> listaNotificacoesEnvio;
        private NotificacaoSistema ONotificacaoSistema;
        private string senhaProvisoria;

        //Chamador das ações do evento
        public void execute(object source) {

            try {

                var listaParametros = source as List<object>;

                this.listaNotificacoesEnvio = (listaParametros[0] as List<NotificacaoSistemaEnvio>);

                this.ONotificacaoSistema = (listaParametros[1] as NotificacaoSistema);

                this.senhaProvisoria = listaParametros[2] as string;

                if(listaNotificacoesEnvio == null) {
                    throw new Exception("Nenhum item de notificação foi enviado para geração das ocorrências.");
                }

                this.gerarOcorrencia();

            } catch (Exception ex) {
                UtilLog.saveError(ex, "Erro no manipulador de evento: OnNovaSenhaEnviadaHandler");
            }

        }

        //Gerar Ocorrencia para histórico dos associados
        public void gerarOcorrencia() {

            var listaOcorrenciasGeradas = new List<PessoaRelacionamento>();

            var idsAssociados = this.listaNotificacoesEnvio.Select(x => x.idReferencia.toInt()).ToList();

            var idsPessoas = db.Associado.condicoesSeguranca().Where(x => idsAssociados.Contains(x.id)).Select(x => x.idPessoa).Distinct().ToList();

            foreach (var idPessoa in idsPessoas) {

                var Ocorrencia = new PessoaRelacionamento();

                Ocorrencia.dtOcorrencia = this.ONotificacaoSistema.dtCadastro;

                Ocorrencia.idPessoa = idPessoa;

                Ocorrencia.idOcorrenciaRelacionamento = OcorrenciaRelacionamentoConst.idEnvioNovaSenhaAssociado;
                
                Ocorrencia.observacao = String.Concat("A senha do associado foi alterada para: ", this.senhaProvisoria);

                listaOcorrenciasGeradas.Add(Ocorrencia);

            }

            using(var ctx = new DataContext()) {

                ctx.Configuration.AutoDetectChangesEnabled = false;

                ctx.Configuration.ValidateOnSaveEnabled = false;

                listaOcorrenciasGeradas.ForEach(x => {
                    x.setDefaultInsertValues();
                });

                ctx.PessoaRelacionamento.AddRange(listaOcorrenciasGeradas);

                ctx.SaveChanges();

            }

        }
        
    }

}