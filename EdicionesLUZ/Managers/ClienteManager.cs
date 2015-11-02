using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EdicionesLUZ.Modelo;


namespace EdicionesLUZ.Managers
{
    class ClienteManager
    {
        ICliente icliente;
        
        Cliente cliente;
        public Cliente Cliente
        {
            get 
            {
                if(cliente ==null)
                    cliente = new Cliente();
                return cliente;
            }
            set { cliente = value; }
        }
        
        public ClienteManager()
        {
            //icliente= new ClienteMock();
            
        }
        public bool EliminarCliente(Cliente cliente)
        {
            return icliente.EliminarCliente(cliente);
        }

       

        public List<Cliente> TodosClientes()
        {
            List<Cliente> listclientes = new List<Cliente>();
            listclientes = icliente.TodosClientes();
            return listclientes;
        }
    }
}
