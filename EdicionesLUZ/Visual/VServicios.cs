using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EdicionesLUZ.Visual
{
  public class VServicios
    {
        private string servicio;
        public string Servicio
        {
            get
            {
                return servicio;
            }
            set
            {
                servicio = value;
            }
        }

        private double precio_unitario;
        public double Precio_unitario
        {
            get { return precio_unitario; }
            set { precio_unitario = value; }
        }

        private double mano_obra;
        public double Mano_Obra
        {
            get { return mano_obra; }
            set { mano_obra = value; }
        }
  
        public double Coste_total
        {
            get
            {
                if (cant_utilizada != 0)
                    return Cant_utilizada * Precio_unitario + Mano_Obra;
                else
                    return Mano_Obra;
            }

        } 

        private double cant_utilizada;
        public double Cant_utilizada
        {
            get 
            { 
                return cant_utilizada; 
            }
            set
            {
                    cant_utilizada = value;
            }
        }

        private string unidad_medida;
        public string Unidad_medida
        {
            get { return unidad_medida; }
            set { unidad_medida = value; }
        }



    }
}
