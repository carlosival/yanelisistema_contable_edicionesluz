
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EdicionesLUZ.Modelo;

namespace EdicionesLUZ.Interfaces
{
    public interface IPedido
    {
        bool EliminarPedido(Pedido pedido);
        List<Pedido> TodosPedidos();
        Pedido PedidoDadoId(string pedido);
        bool AdicionarPedido(Cliente clientenew, Pedido pedidoNew);

        DateTime PedidoMasReciente();

        Pedido PedidoConIddadoPedido(Pedido pedido);
    }
}
