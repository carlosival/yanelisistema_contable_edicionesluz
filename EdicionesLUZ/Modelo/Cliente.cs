using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EdicionesLUZ.Modelo
{
    public class Cliente
    {
       
        public int Id
        {
            get;
            set;
        }

        public string Cuenta_Bancaria
        {
           get;
            set;
        }

        public string Empresa
        {
            get;
            set;
        }

        public string Provincia
        {
            get;
            set;
        }

        public string Nombre_representante
        {
            get;
            set;
        }

        public string Direccion
        {
            get;
            set;
        }

        public string Carnet_representante
        {
            get;
            set;
        }

        public string Telefono
        {
            get;
            set;
        }

        private List<Pedido> pedidos;
        public List<Pedido> Pedidos 
        {
            get 
            {
                if (pedidos == null)
                    pedidos = new List<Pedido>();
                return pedidos;
            }
            set { pedidos = value; }
        }
    }
}
