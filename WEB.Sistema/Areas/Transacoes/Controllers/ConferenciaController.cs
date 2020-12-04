using System;
using System.Linq;
using System.Web.Mvc;
using BLL.Membros;
using BLL.Transacoes;
using DAL.Associados;
using MvcFlashMessages;
using PagedList;
using WEB.App_Infrastructure;

namespace WEB.Areas.Transacoes.Controllers {

    public class ConferenciaController : BaseSistemaController {
        
        //Atributos
        private IConferenciaSaldoBL _Consulta;
        private IMembroSaldoCadastroBL _Saldo;
        private IMovimentoCadastroBL _MovimentoCadastro;
        
        //Propriedades
        private IConferenciaSaldoBL Consulta => _Consulta = _Consulta ?? new ConferenciaSaldoBL();
        private IMembroSaldoCadastroBL Saldo => _Saldo = _Saldo ?? new MembroSaldoCadastroBL();
        private IMovimentoCadastroBL MovimentoCadastro => _MovimentoCadastro = _MovimentoCadastro ?? new MovimentoCadastroBL();
        
        // GET
        public ActionResult listar() {

            var listaRegistros = Consulta.query().Where(x => x.valorSaldoAtual != x.valorSaldoMovimento).OrderBy(x => x.nroMembro).ToPagedList(UtilRequest.getNroPagina(), UtilRequest.getNroRegistros());
        
            return View(listaRegistros);
        }

        /// <summary>
        /// 
        /// </summary>
        public ActionResult atualizar(int idMembro) {

            var Item = Consulta.query().FirstOrDefault(x => x.valorSaldoAtual != x.valorSaldoMovimento && x.idMembro == idMembro);

            if (Item == null) {
                
                this.Flash(UtilMessage.TYPE_MESSAGE_ERROR, UtilMessage.error("Falha!", "O registro informado não possui inconsistências."));

                return RedirectToAction("listar");
            }

            var dtAtualizacao = DateTime.Now;
            
            Saldo.atualizarOuInserir(new int[]{ Item.idMembro }, x => new MembroSaldo{ saldoAtual = Item.valorSaldoMovimento, dtAtualizacaoSaldo = dtAtualizacao});
            
            MovimentoCadastro.atualizarSincronizacao(new int[]{ Item.idMembro }, dtAtualizacao);
            
            this.Flash(UtilMessage.TYPE_MESSAGE_SUCCESS, UtilMessage.success("Operação Concluída!", "A sincronização de saldo foi realizada com sucesso!"));
            
            return RedirectToAction("listar");
        }
        
        /// <summary>
        /// 
        /// </summary>
        [ActionName("atualizar-tudo")]
        public ActionResult atualizarTudo() {

            var listaRegistros = Consulta.query().Where(x => x.valorSaldoAtual != x.valorSaldoMovimento).OrderBy(x => x.nroMembro).ToPagedList(UtilRequest.getNroPagina(), 1000);

            if (!listaRegistros.Any()) {
                
                this.Flash(UtilMessage.TYPE_MESSAGE_ERROR, UtilMessage.error("Falha!", "Nenhum registro foi localizado para sincronização."));

                return RedirectToAction("listar");
            }

            
            var dtAtualizacao = DateTime.Now;

            foreach (var Item in listaRegistros) {

                Saldo.atualizarOuInserir(new int[]{ Item.idMembro }, x => new MembroSaldo{ saldoAtual = Item.valorSaldoMovimento, dtAtualizacaoSaldo = dtAtualizacao});
            
                MovimentoCadastro.atualizarSincronizacao(new int[]{ Item.idMembro }, dtAtualizacao);
                
            }
            
            this.Flash(UtilMessage.TYPE_MESSAGE_SUCCESS, UtilMessage.success("Operação Concluída!", "A sincronização de saldo foi realizada com sucesso!"));
            
            return RedirectToAction("listar");
        }
    }

}
