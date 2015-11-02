using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EdicionesLUZ.Interfaces;
using System.Data;
using EdicionesLUZ.Modelo;

namespace EdicionesLUZ.Consulta
{
    class PedidoAccess:IPedido
    {
        DataSetFacturacionTableAdapters.clienteTableAdapter ClienteTB;
        DataSetFacturacionTableAdapters.pedidoTableAdapter PedidoTB;
        DataSetFacturacionTableAdapters.pedidos_costosTableAdapter CostosTB;
        DataSetFacturacionTableAdapters.ficha_costosTableAdapter FichaCostosTB;
        DataSetFacturacion ds;

        public PedidoAccess(DataSetFacturacionTableAdapters.clienteTableAdapter ClienteTB, DataSetFacturacion ds, DataSetFacturacionTableAdapters.pedidoTableAdapter PedidoTB, DataSetFacturacionTableAdapters.pedidos_costosTableAdapter CostosTB, DataSetFacturacionTableAdapters.ficha_costosTableAdapter FichaCostosTB)
        {
            this.ClienteTB = ClienteTB;
            this.PedidoTB = PedidoTB;
            this.CostosTB = CostosTB;
            this.FichaCostosTB = FichaCostosTB;
            this.ds = ds;
        }

        public bool EliminarPedido(Modelo.Pedido pedidoNew)
        {
            try
            {
                foreach (var item in ds.pedido)
                {
                    if (((DataSetFacturacion.pedidoRow)item).Id == pedidoNew.Id)
                    {
                        foreach (var serv in ((DataSetFacturacion.pedidoRow)item).Getpedidos_costosRows())
                        {
                            CostosTB.Delete(serv.Id, serv.idpedido, serv.idcostos, serv.material);
                        }

                        PedidoTB.Delete(item.Id, item.fecha_entrega, item.fecha_expedicion, item.cliente, item.forma_pago, item.descuentos, item.tipo_documento, item.tipo_impresion,
                            item.color_impresion, item.estado, item.coste_total, item.cantidad_paginas, item.importe_total, item.pago_adelantado, item.observaciones,
                            item.cant_ejemplares, item.mano_obra_fotocopia, item.mano_obra_impresion, item.mano_obra_presillado, item.paginas_x_cara, item.mano_obra_encuadernado,
                            item.valor_agregado, item.mano_obra_corte, item.nombre_cliente,item.costotonel,item.costopapel, pedidoNew.ManoObraDisenno, pedidoNew.Cantidad_Hojas_Mecanografia); 
                        break;
                    }
                }
                this.CostosTB.Fill(ds.pedidos_costos);
                this.PedidoTB.Fill(this.ds.pedido);

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<Modelo.Pedido> TodosPedidos()
        {
            List<Modelo.Pedido> listapedidos = new List<Pedido>();
            foreach (var item in ds.pedido.Rows)
            {
                Pedido newpedido = new Pedido();
                newpedido.Id = ((DataSetFacturacion.pedidoRow)item).Id;
                newpedido.Cantidad_Ejemplares = ((DataSetFacturacion.pedidoRow)item).cant_ejemplares;
                newpedido.Color_impresion = ((DataSetFacturacion.pedidoRow)item).color_impresion;
                newpedido.Coste_total = ((DataSetFacturacion.pedidoRow)item).coste_total;
                newpedido.Descuentos = ((DataSetFacturacion.pedidoRow)item).descuentos;
                newpedido.Estado = ((DataSetFacturacion.pedidoRow)item).estado;
                newpedido.ValorAgregado = ((DataSetFacturacion.pedidoRow)item).valor_agregado;
                newpedido.Fecha_entrega = ((DataSetFacturacion.pedidoRow)item).fecha_entrega;
                newpedido.Fecha_expedicion = ((DataSetFacturacion.pedidoRow)item).fecha_expedicion;
                newpedido.Forma_pago = ((DataSetFacturacion.pedidoRow)item).forma_pago;
                newpedido.Observaciones = ((DataSetFacturacion.pedidoRow)item).observaciones;
                newpedido.Paginas_por_Cara = ((DataSetFacturacion.pedidoRow)item).paginas_x_cara;
                newpedido.Pago_adelantado = ((DataSetFacturacion.pedidoRow)item).pago_adelantado;
                newpedido.Tipo_documento = ((DataSetFacturacion.pedidoRow)item).tipo_documento;
                newpedido.Tipo_impresion = ((DataSetFacturacion.pedidoRow)item).tipo_impresion;

                foreach (var item_cost in ((DataSetFacturacion.pedidoRow)item).Getpedidos_costosRows())
                {

                    ServicioAccess servicioap = new ServicioAccess(ClienteTB,ds, PedidoTB,CostosTB,FichaCostosTB);
                    Servicio sv = servicioap.ServicioDadoId(item_cost.idpedido);
                    if (sv != null)
                    {
                        newpedido.Servicios.Add(sv);
                    }
                }
                listapedidos.Add(newpedido);
            }
            return listapedidos;

        }

      

        public bool AdicionarPedido(Cliente clientenew, Pedido pedidoNew)
        {
            try
            {
                pedidoNew.Estado = "pendiente";
                DataSetFacturacion.clienteDataTable clienteId = ClienteTB.GetIdByEmpresa(clientenew.Empresa);
                int id = ((DataSetFacturacion.clienteRow)clienteId.Rows[0]).Id;

                if (pedidoNew.Tipo_impresion == null)
                    pedidoNew.Tipo_impresion = "";
                if (pedidoNew.Color_impresion == null)
                    pedidoNew.Color_impresion = "";
                if (pedidoNew.Observaciones == null)
                    pedidoNew.Observaciones = ""; 

                this.PedidoTB.Insert(pedidoNew.Fecha_entrega, pedidoNew.Fecha_expedicion, id, pedidoNew.Forma_pago,
                                     pedidoNew.Descuentos, pedidoNew.Tipo_documento, pedidoNew.Tipo_impresion,
                                     pedidoNew.Color_impresion, pedidoNew.Estado, pedidoNew.Cantidad_paginas,
                                     pedidoNew.Coste_total, pedidoNew.Importe_total, pedidoNew.Pago_adelantado,
                                     pedidoNew.Observaciones, pedidoNew.Cantidad_Ejemplares, pedidoNew.ManoObraFotocopia,
                                     pedidoNew.ManoObraImpresion, pedidoNew.ManoObraPresillado, pedidoNew.Paginas_por_Cara,
                                     pedidoNew.ManoObraEncuadernado, pedidoNew.ValorAgregado, pedidoNew.ManoObraCorte, ((DataSetFacturacion.clienteRow)clienteId.Rows[0]).nombre_representante, 
                                     pedidoNew.CostePapel, pedidoNew.CostePapel, pedidoNew.ManoObraDisenno, pedidoNew.Cantidad_Hojas_Mecanografia);
                this.PedidoTB.Update(this.ds.pedido);
                this.PedidoTB.Fill(this.ds.pedido);
                return true;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public DateTime PedidoMasReciente()
        {
            DataSetFacturacion.pedidoDataTable pedidos = ds.pedido;
            DateTime recenttime = new DateTime(1980, 1, 1);

            foreach (var pedido in pedidos)
            {
                if (recenttime.CompareTo(pedido.fecha_entrega) < 0)
                    recenttime = pedido.fecha_entrega;
            }
            return recenttime;
        }

        public Pedido PedidoConIddadoPedido(Pedido pedidoNew)
        {
            DataSetFacturacion.pedidoDataTable pedidoId = PedidoTB.GetPedidoConIdByPedido(
                                     pedidoNew.Fecha_entrega, pedidoNew.Fecha_expedicion, pedidoNew.Forma_pago, pedidoNew.Tipo_documento, pedidoNew.Tipo_impresion,
                                     pedidoNew.Color_impresion, pedidoNew.Estado,Convert.ToDecimal( pedidoNew.Cantidad_paginas),
                                     pedidoNew.Cantidad_Ejemplares,  
                                     pedidoNew.Paginas_por_Cara );
            if(pedidoId.Count >0)
            pedidoNew.Id = ((DataSetFacturacion.pedidoRow)pedidoId.Rows[0]).Id;
            return pedidoNew; 
        }

        public Pedido PedidoDadoId(string var)
        {
              DataSetFacturacion.pedidoDataTable pedido = PedidoTB.PedidoById(Convert.ToInt16(var));
              Pedido newpedido = new Pedido(); 
                newpedido.Id = ((DataSetFacturacion.pedidoRow)pedido.Rows[0]).Id;
                newpedido.Cantidad_Ejemplares = ((DataSetFacturacion.pedidoRow)pedido.Rows[0]).cant_ejemplares;
                newpedido.Color_impresion = ((DataSetFacturacion.pedidoRow)pedido.Rows[0]).color_impresion;
                newpedido.Coste_total = ((DataSetFacturacion.pedidoRow)pedido.Rows[0]).coste_total;
                newpedido.Descuentos = ((DataSetFacturacion.pedidoRow)pedido.Rows[0]).descuentos;
                newpedido.Estado = ((DataSetFacturacion.pedidoRow)pedido.Rows[0]).estado;
                newpedido.ValorAgregado = ((DataSetFacturacion.pedidoRow)pedido.Rows[0]).valor_agregado;
                newpedido.Fecha_entrega = ((DataSetFacturacion.pedidoRow)pedido.Rows[0]).fecha_entrega;
                newpedido.Fecha_expedicion = ((DataSetFacturacion.pedidoRow)pedido.Rows[0]).fecha_expedicion;
                newpedido.Forma_pago = ((DataSetFacturacion.pedidoRow)pedido.Rows[0]).forma_pago;
                newpedido.Observaciones = ((DataSetFacturacion.pedidoRow)pedido.Rows[0]).observaciones;
                newpedido.Paginas_por_Cara = ((DataSetFacturacion.pedidoRow)pedido.Rows[0]).paginas_x_cara;
                newpedido.Pago_adelantado = ((DataSetFacturacion.pedidoRow)pedido.Rows[0]).pago_adelantado;
                newpedido.Tipo_documento = ((DataSetFacturacion.pedidoRow)pedido.Rows[0]).tipo_documento;
                newpedido.Tipo_impresion = ((DataSetFacturacion.pedidoRow)pedido.Rows[0]).tipo_impresion;
                newpedido.ManoObraCorte = ((DataSetFacturacion.pedidoRow)pedido.Rows[0]).mano_obra_corte;
                newpedido.ManoObraEncuadernado = ((DataSetFacturacion.pedidoRow)pedido.Rows[0]).mano_obra_encuadernado;
                newpedido.ManoObraFotocopia = ((DataSetFacturacion.pedidoRow)pedido.Rows[0]).mano_obra_fotocopia;
                newpedido.ManoObraImpresion = ((DataSetFacturacion.pedidoRow)pedido.Rows[0]).mano_obra_impresion;
                newpedido.ManoObraPresillado = ((DataSetFacturacion.pedidoRow)pedido.Rows[0]).mano_obra_presillado;
                newpedido.Cantidad_paginas = ((DataSetFacturacion.pedidoRow)pedido.Rows[0]).cantidad_paginas;
                newpedido.Importe_total = ((DataSetFacturacion.pedidoRow)pedido.Rows[0]).importe_total;
                newpedido.CostePapel = ((DataSetFacturacion.pedidoRow)pedido.Rows[0]).costopapel;
                newpedido.CosteTonel = ((DataSetFacturacion.pedidoRow)pedido.Rows[0]).costotonel;
                newpedido.ManoObraDisenno = ((DataSetFacturacion.pedidoRow)pedido.Rows[0]).mano_obra_disenno;
                newpedido.Cantidad_Hojas_Mecanografia = ((DataSetFacturacion.pedidoRow)pedido.Rows[0]).cantidad_hojas_mecanografia;
            return newpedido;
               
        }

    }
}
