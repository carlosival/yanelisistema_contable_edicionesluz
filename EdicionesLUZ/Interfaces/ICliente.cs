using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EdicionesLUZ.Modelo;

namespace EdicionesLUZ
{
   public interface ICliente
    {
       bool EliminarCliente(Cliente cliente);
       bool AdicionarCliente(Cliente clientenew, Pedido pedidoNew);
       
      List<Cliente> TodosClientes();

       List<Cliente> ClienteByNombre(string Cliente);

       Cliente ClienteDadoIdPedido(int p);

       Cliente ClienteDadoId(string id);
    }
}
