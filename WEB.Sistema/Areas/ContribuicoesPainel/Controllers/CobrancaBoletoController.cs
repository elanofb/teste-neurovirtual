using System;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using BLL.AssociadosContribuicoes;
using BLL.Tarefas;
using DAL.AssociadosContribuicoes;
using DAL.Permissao.Security.Extensions;
using DAL.Tarefas;
using WEB.Areas.ContribuicoesPainel.ViewModels;

namespace WEB.Areas.ContribuicoesPainel.Controllers {

    [OrganizacaoFilter]
    public class CobrancaBoletoController : Controller {

        //Atributos
        private IAssociadoContribuicaoFilaBoletoGeracaoBL _AssociadoContribuicaoBoletoGeracaoBL;

        //Propriedades
        private IAssociadoContribuicaoFilaBoletoGeracaoBL OAssociadoContribuicaoBoletoGeracaoBL => _AssociadoContribuicaoBoletoGeracaoBL = _AssociadoContribuicaoBoletoGeracaoBL ?? new AssociadoContribuicaoFilaBoletoGeracaoBL();

        /// <summary>
        /// Gerar as cobranças
        /// </summary>
        [ActionName("gerar-boletos")]
        public ActionResult gerarBoletos() {

            int idContribuicao = UtilRequest.getInt32("idContribuicao");

            int[] idsAssociados = UtilRequest.getListInt("idsAssociados").ToArray();

            var ViewModel = new PainelCobrancaVM();

            ViewModel.carregarDadosContribuicao(idContribuicao, idsAssociados);

            StringBuilder msgRetorno = new StringBuilder();

            if (ViewModel.listaIsentos.Count > 0){

                foreach (var itemIsento in ViewModel.listaIsentos){

                    msgRetorno.AppendLine($"O associado {itemIsento.AssociadoContribuicao.nomeAssociado} não terá o boleto gerado pois é um associado isento.");

                }
                
            }

            if (ViewModel.listaQuitados.Count > 0){

                foreach (var itemQuitado in ViewModel.listaQuitados){

                    msgRetorno.AppendLine($"O associado {itemQuitado.AssociadoContribuicao.nomeAssociado} não terá o boleto gerado pois é o pagamento já foi realizado.");

                }
                
            }

            var idsBoletosGerados = ViewModel.carregarBoletos().Select(x => x.idAssociadoContribuicao).ToList();

            var listaPendentes = ViewModel.listaCobrancas.Where(x => !x.AssociadoContribuicao.flagQuitado() && !idsBoletosGerados.Contains(x.AssociadoContribuicao.id) ).ToList();


            if (!listaPendentes.Any()) {

                return Json(new { error = true, message = "Não foi encontrada nenhuma cobrança em aberto para geração de boletos." }, JsonRequestBehavior.AllowGet);

            }

            var OTarefa = new TarefaSistema();

            OTarefa.idReferencia = idContribuicao;

            OTarefa.idUsuarioInicializacao = User.id();

            OTarefa.titulo = idsAssociados.Length == 0 ? $"Geração de boletos de cobrança para todos os associados: {ViewModel.Contribuicao.descricao}" : $"Geração de boletos de cobrança para associados específicos: {ViewModel.Contribuicao.descricao}";

            //Criar a nova tarefa no sistema
            var OTarefaGeracaoBL = TarefaGerarBoletosContribuicao.getInstance;

            var RetornoTarefa = OTarefaGeracaoBL.criar(OTarefa, false);

            if (RetornoTarefa.flagError) {

                return Json(new { error = true, message = RetornoTarefa.listaErros.FirstOrDefault() }, JsonRequestBehavior.AllowGet);

            }

            var listaNovosItens = listaPendentes.Select(x =>
                                        new AssociadoContribuicaoBoletoGeracao {
                                            idOrganizacao = User.id(),
                                            idAssociadoContribuicao = x.AssociadoContribuicao.id,
                                            dtVencimento = x.AssociadoContribuicao.dtVencimentoOriginal,
                                            idUsuarioGeracao = User.id(),
                                            idTarefa = OTarefa.id
                                        }).ToList();

            //Salvar a lista de contribuicoes que precisam ser geradas
            OAssociadoContribuicaoBoletoGeracaoBL.salvar(listaNovosItens);

            //Registro início do processamento 
            OTarefaGeracaoBL.iniciarProcessamento();

            //Realizar primeira execucao
            OTarefaGeracaoBL.executar();

            return Json(new { error = false, message = $"O sistema irá gerar boletos para {listaNovosItens.Count} cobranças. Isso poderá durar alguns minutos. Ao término da execução, você será notificado." }, JsonRequestBehavior.AllowGet);
        }


    }
}