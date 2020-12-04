using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using BLL.Pedidos;
using BLL.Services;
using DAL.Pedidos;
using DAL.Permissao.Security.Extensions;
using FluentValidation.Attributes;

namespace WEB.Areas.Pedidos.ViewModels{
    [Validator(typeof(PedidoMovimentacaoFormValidator))]
    public class PedidoMovimentacaoForm{
        // Atributos Serviço
        private IPedidoBL _IPedidoBL;

        // Propriedades Serviço        
        private IPedidoBL OPedidoBL => _IPedidoBL = _IPedidoBL ?? new PedidoBL();

        //Propriedades
        public List<int> idsPedidos{ get; set; }
        
        
        public List<int> idsPedidosSelecionados{ get; set; }
        public List<Pedido> listaPedidos{ get; set; }
        public string valorBusca{ get; set; }
        public int idStatusPedido{ get; set; }
        public int? idRemover{ get; set; }
        public bool? flagTodos{ get; set; }
        
        // Constantes
        private IPrincipal User => HttpContextFactory.Current.User;
        
        //Construtor
        public PedidoMovimentacaoForm(){
            
            this.idsPedidos = new List<int>();
            this.idsPedidosSelecionados = new List<int>();
            this.listaPedidos = new List<Pedido>();
            
        }
        
        public void montarQuery(){

            var query = this.OPedidoBL.query(User.idOrganizacao()).Where(x => this.idsPedidos.Contains(x.id));
            
            this.listaPedidos = query.Where(x => this.idsPedidosSelecionados.Contains(x.id))
                .Select(x => new{
                    x.id,
                    x.dtCadastro,
                    x.idStatusPedido,
                    Pessoa = new{x.Pessoa.nome},
                    StatusPedido = new{x.StatusPedido.descricao}
                }).ToListJsonObject<Pedido>();
            
        }

        public void adicionarNaLista() {

            if (!valorBusca.isEmpty() && flagTodos != true) {
                
                int valorBuscaPedido = Convert.ToInt32(UtilString.onlyNumber(valorBusca));
                
                var query = this.OPedidoBL.query(User.idOrganizacao()).Where(x => this.idsPedidos.Contains(x.id));

                var idsPedidosEncontrados = query.Where(x => 
                                                    x.id == valorBuscaPedido && 
                                                    !this.idsPedidosSelecionados.Contains(x.id)
                                                )
                                                .Select(x => x.id)
                                                .ToList();
            
                this.idsPedidosSelecionados.AddRange(idsPedidosEncontrados);
                
            }

            if (flagTodos == true) {
                this.idsPedidosSelecionados = this.idsPedidos;
            }

        }

        public void removerDaLista() {

            if (flagTodos == true) {
                return;
            }

            if (this.idRemover > 0) {
                
                var query = this.OPedidoBL.query(User.idOrganizacao()).Where(x => this.idsPedidos.Contains(x.id));

                var OPedido = query.FirstOrDefault(x => x.id == this.idRemover) ?? new Pedido();

                this.idsPedidosSelecionados.RemoveAll(x => x == OPedido.id);

            }


        }
    }
}