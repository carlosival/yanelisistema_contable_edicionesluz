using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EdicionesLUZ.Modelo
{
    public class Servicio
    {
        public Servicio() { }
        public Servicio(string material)
        {
            this.Material = material;

        }
        public Servicio(string material, double precio_unitario, int cant_utilizada)
        {
            this.Material = material;
            this.Precio_unitario = precio_unitario;
            this.Cant_utilizada = cant_utilizada;
            
        }

        private int id;
        public int Id
        { 
            get { return id; }
            set { id = value; }
        }

        private string material; 
        public string Material
        {
            get
            {
                return material; 
            }
            set
            {
                material = value; 
            }
        }

        private double precio_unitario;
        public double Precio_unitario
        {
            get { return precio_unitario;}
            set { precio_unitario = value;}
        }

        private double cant_utilizada;
        public double Cant_utilizada
        {
            get { return cant_utilizada; }
            set { cant_utilizada = value;  }
        }

        private string unidad_medida;
        public string Unidad_medida
        {
            get { return unidad_medida; }
            set { unidad_medida = value; }
        }
    }
}
