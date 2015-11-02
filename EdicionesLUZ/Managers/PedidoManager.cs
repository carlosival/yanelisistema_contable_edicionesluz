using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EdicionesLUZ.Interfaces;

using EdicionesLUZ.Modelo;
using EdicionesLUZ.Consulta;

namespace EdicionesLUZ.Managers
{
    public class PedidoManager
    {
        IPedido ipedido;
        Pedido pedido = new Pedido();
        public Pedido Pedido
        {
            get 
            {
                return pedido;
            }
            set 
            {
                pedido = value;
            }
        }


        public PedidoManager()
        {
            //ipedido = new PedidoAccess(); 
        }

        public bool EliminarPedido(Pedido pedido)
        {
            return ipedido.EliminarPedido(pedido); 
        }
        public bool AdicionarPedido(Pedido pedidonew)
        {
            //Pedido pedido = ipedido.AdicionarPedido(pedidonew); 
            //return pedido;
            return false;
        }
        public List<Pedido> TodosPedidos()
        {
            List<Pedido> listpedidos = new List<Pedido>();
            listpedidos = ipedido.TodosPedidos();
            return listpedidos;
        }

        public Pedido PedidoDadoId() { return null; }
    }
}
