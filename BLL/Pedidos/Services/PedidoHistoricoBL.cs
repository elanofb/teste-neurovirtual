using System;
using System.Data.Entity;
using System.Linq;
using BLL.Services;
using DAL.Pedidos;
using DAL.Permissao.Security.Extensions;

namespace BLL.Pedidos {

    public class PedidoHistoricoBL : DefaultBL, IPedidoHistoricoBL {

        //
        public PedidoHistoricoBL() {
        }

        //
        public IQueryable<PedidoHistorico> listar(int idPedido) {

            var query = from PedOcor in db.PedidoHistorico
                                          .Include(x => x.Pedido)
                                          .Include(x => x.TipoOcorrenciaPedido)
                                          .Include(x => x.UsuarioOcorrencia)
                        where PedOcor.flagExcluido == "N"
                        select PedOcor;

            if (idPedido > 0) {
                query = query.Where(x => x.idPedido == idPedido);
            }

            return query;
        }

        //Criar um objeto de nova ocorrencia
        public PedidoHistorico criarNovaOcorrencia(int idPedido, int idOcorrencia, string obs) {

            PedidoHistorico OPedidoOcorrencia = new PedidoHistorico();

            OPedidoOcorrencia.idPedido = idPedido;

            OPedidoOcorrencia.idOcorrenciaPedido = idOcorrencia;

            OPedidoOcorrencia.observacoes = obs;

            OPedidoOcorrencia.idUsuarioOcorrencia = User.id();

            OPedidoOcorrencia.setDefaultInsertValues<PedidoHistorico>();

            OPedidoOcorrencia.dtOcorrencia = DateTime.Now;

            return OPedidoOcorrencia;
        }

        //
        public void criarOcorrenciaPedidoCriado(int idPedido) {

            int idOcorrencia = Convert.ToInt32(TipoOcorrenciaPedidoConst.CRIACAO_PEDIDO);

            PedidoHistorico OPedidoOcorrencia = this.criarNovaOcorrencia(idPedido, idOcorrencia, "");

            this.salvar(OPedidoOcorrencia);
        }

        //
        public void criarOcorrenciaFaturamentoPedido(int idPedido) {

            int idOcorrencia = TipoOcorrenciaPedidoConst.FATURAMENTO;

            PedidoHistorico OPedidoOcorrencia = this.criarNovaOcorrencia(idPedido, idOcorrencia, "");

            this.salvar(OPedidoOcorrencia);
        }

		//
		public void criarOcorrenciaAtendido(int idPedido, string obs) {

            int idOcorrencia = TipoOcorrenciaPedidoConst.ATENDIMENTO_PEDIDO;

            PedidoHistorico OPedidoOcorrencia = this.criarNovaOcorrencia(idPedido, idOcorrencia, obs);

            this.salvar(OPedidoOcorrencia);
		}
        
        //
        public void criarOcorrenciaEmMontagem(int idPedido, string obs) {

            int idOcorrencia = TipoOcorrenciaPedidoConst.PREPARACAO_PEDIDO;

            PedidoHistorico OPedidoOcorrencia = this.criarNovaOcorrencia(idPedido, idOcorrencia, obs);

            this.salvar(OPedidoOcorrencia);
        }
        
        //
        public void criarOcorrenciaPreparado(int idPedido, string obs) {

            int idOcorrencia = TipoOcorrenciaPedidoConst.PREPARACAO_PEDIDO;

            PedidoHistorico OPedidoOcorrencia = this.criarNovaOcorrencia(idPedido, idOcorrencia, obs);

            this.salvar(OPedidoOcorrencia);
        }
        
        
        //
        public void criarOcorrenciaAguardandoExpedicao(int idPedido, string obs) {

            int idOcorrencia = TipoOcorrenciaPedidoConst.ATENDIMENTO_PEDIDO;

            PedidoHistorico OPedidoOcorrencia = this.criarNovaOcorrencia(idPedido, idOcorrencia, obs);

            this.salvar(OPedidoOcorrencia);
        }

		//
		public void criarOcorrenciaExpedido(int idPedido, string obs) {

            int idOcorrencia = TipoOcorrenciaPedidoConst.EXPEDICAO;

            PedidoHistorico OPedidoOcorrencia = this.criarNovaOcorrencia(idPedido, idOcorrencia, obs);

            this.salvar(OPedidoOcorrencia);
		}

		//
		public void criarOcorrenciaCancelado(int idPedido, string obs) {

            int idOcorrencia = TipoOcorrenciaPedidoConst.CANCELAMENTO;

            PedidoHistorico OPedidoOcorrencia = this.criarNovaOcorrencia(idPedido, idOcorrencia, obs);

            this.salvar(OPedidoOcorrencia);
		}

		//
		public void criarOcorrenciaPago(int idPedido, string obs) {

            int idOcorrencia = TipoOcorrenciaPedidoConst.CONFIRMACAO_PAGAMENTO;

            PedidoHistorico OPedidoOcorrencia = this.criarNovaOcorrencia(idPedido, idOcorrencia, obs);

            this.salvar(OPedidoOcorrencia);
		}

        //
        public void criarOcorrenciaFinalizado(int idPedido, string obs) {

            int idOcorrencia = TipoOcorrenciaPedidoConst.FINALIZACAO;

            PedidoHistorico OPedidoOcorrencia = this.criarNovaOcorrencia(idPedido, idOcorrencia, obs);

            this.salvar(OPedidoOcorrencia);
        }

        //
        //Verificar se deve-se atualizar um registro existente ou criar um novo
        public bool salvar(PedidoHistorico OPedidoHistorico) {

            if (OPedidoHistorico.id == 0) {
                return this.inserir(OPedidoHistorico);
            }

            return this.atualizar(OPedidoHistorico);
        }

        //Persistir o objecto e salvar na base de dados
        private bool inserir(PedidoHistorico OPedidoHistorico) {

            OPedidoHistorico.setDefaultInsertValues();
            db.PedidoHistorico.Add(OPedidoHistorico);

            db.SaveChanges();

            return (OPedidoHistorico.id > 0);
        }

        //Persistir o objecto e atualizar informações
        private bool atualizar(PedidoHistorico OPedidoHistorico) {

            OPedidoHistorico.setDefaultUpdateValues();

            //Localizar existentes no PedidoHistorico
            PedidoHistorico dbPedidoHistorico = this.db.PedidoHistorico.Find(OPedidoHistorico.id);
            var TipoEntry = db.Entry(dbPedidoHistorico);
            TipoEntry.CurrentValues.SetValues(OPedidoHistorico);
            TipoEntry.ignoreFields();

            db.SaveChanges();
            return (OPedidoHistorico.id > 0);
        }
    }
}