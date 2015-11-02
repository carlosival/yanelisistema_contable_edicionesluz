using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EdicionesLUZ.Interfaces;
using EdicionesLUZ.Modelo;
using EdicionesLUZ.Consulta;

namespace EdicionesLUZ.Managers
{
   public class EntidadManager
    {
        IEntidad ientidad;
        ICliente icliente;
        IServicio iservicio;
        IPedido ipedido;
        Entidad entidad;
        DataSetFacturacionTableAdapters.clienteTableAdapter ClienteTB;
        DataSetFacturacionTableAdapters.pedidoTableAdapter PedidoTB;
        DataSetFacturacionTableAdapters.pedidos_costosTableAdapter CostosTB;
        DataSetFacturacionTableAdapters.ficha_costosTableAdapter FichaCostosTB;
        DataSetFacturacion ds;

        public Entidad Entidad
        {
            get
            {
                if (entidad == null)
                    entidad = new Entidad();
                return entidad;
            }
            set { entidad = value; }
        }

        public EntidadManager(DataSetFacturacionTableAdapters.clienteTableAdapter ClienteTB, DataSetFacturacion ds, DataSetFacturacionTableAdapters.pedidoTableAdapter PedidoTB, DataSetFacturacionTableAdapters.pedidos_costosTableAdapter CostosTB, DataSetFacturacionTableAdapters.ficha_costosTableAdapter FichaCostosTB)
        {
            this.ClienteTB = ClienteTB;
            this.PedidoTB = PedidoTB;
            this.CostosTB = CostosTB;
            this.FichaCostosTB = FichaCostosTB;

            this.ds = ds;

            ientidad = new EntidadAccess();
            icliente = new ClienteAccess(ClienteTB,ds, PedidoTB,CostosTB,FichaCostosTB);
            iservicio = new ServicioAccess(ClienteTB, ds, PedidoTB, CostosTB, FichaCostosTB);
            ipedido = new PedidoAccess(ClienteTB, ds, PedidoTB, CostosTB, FichaCostosTB);
        }

        public bool AdicionarCliente(Cliente clientenew, Pedido pedidonew)
        {
            return icliente.AdicionarCliente(clientenew, pedidonew);
        }
        public bool EliminarCliente(Cliente cliente)
        {
            return icliente.EliminarCliente(cliente);
        }

        public double CostoDadoMaterial(string material)
        {
            return iservicio.CostoDadoMaterial(material);
        }

        public List<Cliente> TodosClientes()
        {
            return icliente.TodosClientes();
        }

        public List<Modelo.Pedido> TodosPedidos()
        {
            return ipedido.TodosPedidos();
        }

        public List<Modelo.Servicio> TodosServicios()
        {
            return TodosServicios();
        }
        public void EliminarPedido(Pedido pedido) 
        {
            ipedido.EliminarPedido(pedido);
        }

        public DateTime PedidoMasReciente() 
        {
            return ipedido.PedidoMasReciente();
        }

        public void Load()
        {
            int i = 0;

            while (i < 1000)
            {
                i++;
            }
        }

        internal List<Cliente> ClienteDadoNombre(string Cliente)
        {
          List<Cliente> listclientes=  icliente.ClienteByNombre(Cliente);
          return listclientes;
        }

        internal Cliente ClienteDadoIdPedido(int p)
        {
            Cliente cliente = icliente.ClienteDadoIdPedido(p);
            return cliente;
        }

        internal Pedido PedidoConIdDadoPedido(Pedido pedido)
        {
            Pedido ped = ipedido.PedidoConIddadoPedido(pedido);
            return ped;
        }

        internal Pedido PedidoDadoId(string var)
        {
            Pedido ped = ipedido.PedidoDadoId(var);
            return ped;
      
        }

        internal Cliente ClienteDadoId(string id)
        {
            Cliente cliente = icliente.ClienteDadoId(id);
            return cliente;
        }

        internal List<Servicio> ServiciosDadoIdPedido(int p)
        {
            List<Servicio> servcios = new List<Servicio>();
            servcios = iservicio.ServiciosDadoIdPedido(p);
            return servcios;
        }
    }
}
