namespace DAL.Pedidos {

    public class PedidoEmailsConst {

        public static readonly string tituloEmailNovoPedido = "Pedido #ID_PEDIDO# recebido";

        public static readonly string corpoEmailNovoPedido = "Caro #CLIENTE#, <br><br>Recebemos o seu pedido de número <strong>#ID_PEDIDO#</strong>. <br><br> #INFO_PGTO#<br><br> #INFO_ENTREGA#<br><br> Você também pode acompanhar o andamento e os detalhes de seu pedido através da área do associado.";
        
        public static readonly string tituloEmailFaturamentoPedido = "Pedido #ID_PEDIDO# faturado";

        public static readonly string corpoEmailFaturamentoPedido = "Caro #CLIENTE#, <br><br>O seu pedido nº #ID_PEDIDO# foi fechado para faturamento.<br><br> O valor total apurado é de #VALOR_PEDIDO#.<br><br> Caso ainda não tenha realizado o pagamento, <a href='#LINK_PGTO#'>clique aqui</a> e realize agora mesmo.<br><br> Você pode acompanhar o andamento e os detalhes de seu pedido através da área do associado.";        

    }

}
