using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EdicionesLUZ.Modelo
{
     public class Pedido
    {
        private int id;
        public int Id
        {
            get
            {
                return id;
            }
            set
            {
                id = value;
            }
        }

        private DateTime fecha_entrega;
        public DateTime Fecha_entrega
        {
            get
            {
                return fecha_entrega;
            }
            set
            {
                fecha_entrega = value;
            }
        }

        private DateTime fecha_expedicion;
        public DateTime Fecha_expedicion
        {
            get
            {
                return fecha_expedicion;
            }
            set
            {
                fecha_expedicion = value;
            }
        }

        private string forma_pago;
        public string Forma_pago
        {
            get
            {
                return forma_pago;
            }
            set
            {
                forma_pago = value;
            }
        }

        private double descuentos;
        public double Descuentos
        {
            get
            {
                return descuentos;
            }
            set
            {
                descuentos = value;
            }
        }

        private string tipo_documento;
        public string Tipo_documento
        {
            get
            {
                return tipo_documento;
            }
            set
            {
                tipo_documento = value;
            }
        }

        private string tipo_impresion;
        public string Tipo_impresion
        {
            get
            {
                return tipo_impresion;
            }
            set
            {
                tipo_impresion = value;
            }
        }

        private string color_impresion;
        public string Color_impresion
        {
            get
            {
                return color_impresion;
            }
            set
            {
                color_impresion = value;
            }
        }

        private string estado;
        public string Estado
        {
            get
            {
                return estado;
            }
            set
            {
                estado = value;
            }
        }

        private double cantidad_paginas;
        public double Cantidad_paginas
        {
            get
            {
                return cantidad_paginas;
            }
            set
            {
                cantidad_paginas = value;
            }
        }

        private int cantidad_ejemplares;
        public int Cantidad_Ejemplares
        {
            get
            {
                return cantidad_ejemplares;
            }
            set
            {
                cantidad_ejemplares = value;
            }
        }

        private int paginas_por_cara;
        public int Paginas_por_Cara
        {
            get
            {
                return paginas_por_cara;
            }
            set
            {
                paginas_por_cara = value;
            }
        }

        private double coste_total;
        public double Coste_total
        {
            get
            {
                return coste_total;
            }
            set
            {
                coste_total = value;
            }
        }

        private double importe_total;
        public double Importe_total
        {
            get
            {
                return importe_total;
            }
            set
            {
                importe_total = value;
                Pago_adelantado = Math.Round(importe_total / 2, MidpointRounding.AwayFromZero);
            }
        }

        private double pago_adelantado;
        public double Pago_adelantado
        {
            get
            {
                return pago_adelantado;
            }
            set
            {
                pago_adelantado = value;
            }
        }

        private string observaciones;
        public string Observaciones
        {
            get
            {
                return observaciones;
            }
            set
            {
                observaciones = value;
            }
        }


        private List<Servicio> servicios;
        public List<Servicio> Servicios
        {
            get
            {
                if (servicios == null)
                    servicios = new List<Servicio>();
                return servicios;
            }
            set
            {
                servicios = value;
            }
        }

        private double manoObraImpresion;
        public double ManoObraImpresion
        {
            get
            {
                return manoObraImpresion;
            }
            set
            {
                manoObraImpresion = value;
            }
        }

        private double manoObraFotocopia;
        public double ManoObraFotocopia
        {
            get
            {
                return manoObraFotocopia;
            }
            set
            {
                manoObraFotocopia = value;
            }
        }

        private double manoObraEncuadernado;
        public double ManoObraEncuadernado
        {
            get
            {
                return manoObraEncuadernado;
            }
            set
            {
                manoObraEncuadernado = value;
            }
        }

        private double manoObraPresillado;
        public double ManoObraPresillado
        {
            get
            {
                return manoObraPresillado;
            }
            set
            {
                manoObraPresillado = value;
            }
        }

        private double valorAgregado;
        public double ValorAgregado
        {
            get
            {
                return valorAgregado;
            }
            set
            {
                valorAgregado = value;
            }
        }

        private double manoObraCorte;
        public double ManoObraCorte
        {
            get
            {
                return manoObraCorte;
            }
            set
            {
                manoObraCorte = value;
            }
        }

        public string Nombre_Cliente { get; set; }

        public double ManoObraDisenno { get; set; }

        public int Cantidad_Hojas_Mecanografia { get; set; }

        public double CosteTonel { get; set; }

        public double CostePapel { get; set; }

    }

}
