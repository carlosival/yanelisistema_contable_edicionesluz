using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EdicionesLUZ.Modelo;

namespace EdicionesLUZ.Consulta
{
    class ServicioAccess: EdicionesLUZ.Interfaces.IServicio 
    {
        DataSetFacturacionTableAdapters.clienteTableAdapter ClienteTB;
        DataSetFacturacionTableAdapters.pedidoTableAdapter PedidoTB;
        DataSetFacturacionTableAdapters.pedidos_costosTableAdapter CostosTB;
        DataSetFacturacionTableAdapters.ficha_costosTableAdapter FichaCostosTB;
        DataSetFacturacion ds;

        public ServicioAccess(DataSetFacturacionTableAdapters.clienteTableAdapter ClienteTB, DataSetFacturacion ds, DataSetFacturacionTableAdapters.pedidoTableAdapter PedidoTB, DataSetFacturacionTableAdapters.pedidos_costosTableAdapter CostosTB, DataSetFacturacionTableAdapters.ficha_costosTableAdapter FichaCostosTB)
        {
            this.ClienteTB = ClienteTB;
            this.PedidoTB = PedidoTB;
            this.CostosTB = CostosTB;
            this.FichaCostosTB = FichaCostosTB;
            this.ds = ds;
            //CostosTB.Fill(ds.pedidos_costos);
            //FichaCostosTB.Fill(ds.ficha_costos);
        }

        public int ServicioDadoMaterial(string material) 
        {
            DataSetFacturacion.ficha_costosDataTable costos = ds.ficha_costos;
            foreach (var costo in costos)
            {
                if (costo.material.Equals(material))
                {
                    return costo.id;
                }
            }
            return 0;        
        }

        public double PrecioDadoTipoPapel(string tipoPapel, int canthojas, string tipoImpresion, string colorImpresion)
        {
            throw new NotImplementedException();
        }

        public double PrecioDadoManoObra(string manoObra)
        {
            throw new NotImplementedException();
        }

        public double PrecioEncuadernadoDadoTipoPresillas(string tipoPresillas)
        {
            throw new NotImplementedException();
        }

        public Modelo.Servicio ServicioDadoId(int id)
        {
            try
            {
                DataSetFacturacion.ficha_costosDataTable cost = FichaCostosTB.GetCostosById(id);
                Servicio servicio = new Servicio();
                servicio.Id = ((DataSetFacturacion.ficha_costosRow)cost[0]).id;
                servicio.Material = ((DataSetFacturacion.ficha_costosRow)cost[0]).material;
                servicio.Precio_unitario = ((DataSetFacturacion.ficha_costosRow)cost[0]).precio_unitario;

                return servicio;
            }
            catch (Exception ex)
            {
                throw ex ;
            }
        }

        public double CostoDadoMaterial(string material)
        {
            DataSetFacturacion.ficha_costosDataTable costos = ds.ficha_costos;
            foreach (var costo in costos)
            {
                if (costo.material.Equals(material))
                {
                    return costo.precio_unitario;
                }
            }
            return 0;
        }

        public List<Servicio> ServiciosDadoIdPedido(int p)
        {
            List<Servicio> servicio = new List<Servicio>();
            DataSetFacturacion.pedidos_costosDataTable servicios = CostosTB.ServiciosdadoIdPedido(p);
            foreach (var item in servicios)
            {
              DataSetFacturacion.ficha_costosDataTable costos = FichaCostosTB.GetCostosById(((DataSetFacturacion.pedidos_costosRow)item).idcostos);
              foreach (var item2 in costos)
              {
                  Servicio a = new Servicio();
                  a.Id = ((DataSetFacturacion.ficha_costosRow)item2).id;
                  a.Material = ((DataSetFacturacion.ficha_costosRow)item2).material;
                  a.Precio_unitario = ((DataSetFacturacion.ficha_costosRow)item2).precio_unitario;
                  servicio.Add(a);
              }
                
            }
            return servicio;
        }
    }
}
