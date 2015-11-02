using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EdicionesLUZ.Modelo
{
     public class Entidad
    {
        private List<Pedido> pedidos;
        public List<Pedido> Pedidos
        {
            get
            {
                return pedidos;
            }
            set
            {
                pedidos = value;
            }
        }

        private string representante;
        public string Representante
        {
            get
            {
                return representante;
            }
            set
            {
                representante = value;
            }
        }

        private string cuenta_bancaria;
        public string Cuenta_bancaria
        {
            get
            {
                return cuenta_bancaria;
            }
            set
            {
                cuenta_bancaria = value;
            }
        }

        private string direccion;
        public string Direccion
        {
            get
            {
                return direccion;
            }
            set
            {
                direccion = value;
            }
        }

        private string nombre;
        public string Nombre
        {
            get
            {
                return nombre;
            }
            set
            {
                nombre = value;
            }
        }
    }
}
