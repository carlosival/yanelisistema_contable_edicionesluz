using System;
using System.Collections.Generic;
using System.Text;
using EdicionesLUZ.Modelo;
using System.Data.OleDb;
using System.Data;

namespace EdicionesLUZ.Consulta
{
    class ClienteAccess : ICliente
    {
        DataSetFacturacionTableAdapters.clienteTableAdapter ClienteTB;
        DataSetFacturacionTableAdapters.pedidoTableAdapter PedidoTB;
        DataSetFacturacionTableAdapters.pedidos_costosTableAdapter CostosTB;
        DataSetFacturacionTableAdapters.ficha_costosTableAdapter FichaCostosTB;
        DataSetFacturacion ds;

        public ClienteAccess(DataSetFacturacionTableAdapters.clienteTableAdapter ClienteTB, DataSetFacturacion ds, DataSetFacturacionTableAdapters.pedidoTableAdapter PedidoTB, DataSetFacturacionTableAdapters.pedidos_costosTableAdapter CostosTB, DataSetFacturacionTableAdapters.ficha_costosTableAdapter FichaCostosTB)
        {
            this.ClienteTB = ClienteTB;
            this.PedidoTB = PedidoTB;
            this.CostosTB = CostosTB;
            this.FichaCostosTB = FichaCostosTB;
            this.ds = ds;

        }

        public List<Cliente> TodosClientes()
        {
            List<Cliente> listaclientes = new List<Cliente>();
            foreach (var item in ds.cliente.Rows)
            {
                Cliente tempCliente = CrearCliente(item);

                foreach (var pedItem in ((DataSetFacturacion.clienteRow)item).GetpedidoRows())
                {
                    Pedido pedidotemp = new Pedido();
                    pedidotemp.Fecha_entrega = pedItem.fecha_entrega;
                    pedidotemp.Fecha_expedicion = pedItem.fecha_expedicion;
                    pedidotemp.Forma_pago = pedItem.forma_pago;
                    pedidotemp.Estado = pedItem.estado;
                    pedidotemp.Importe_total = pedItem.importe_total;
                    pedidotemp.ManoObraEncuadernado = pedItem.mano_obra_encuadernado;
                    pedidotemp.ManoObraFotocopia = pedItem.mano_obra_fotocopia;
                    pedidotemp.ManoObraImpresion = pedItem.mano_obra_impresion;
                    pedidotemp.ManoObraPresillado = pedItem.mano_obra_presillado;
                    pedidotemp.Paginas_por_Cara = pedItem.paginas_x_cara;
                    pedidotemp.Observaciones = pedItem.observaciones;
                    pedidotemp.Cantidad_Ejemplares = pedItem.cant_ejemplares;
                    pedidotemp.Cantidad_paginas = pedItem.cantidad_paginas;
                    pedidotemp.Pago_adelantado = pedItem.pago_adelantado;
                    pedidotemp.Tipo_documento = pedidotemp.Tipo_documento;
                    pedidotemp.Tipo_impresion = pedidotemp.Tipo_impresion;

                    foreach (var pedServItem in pedItem.Getpedidos_costosRows())
                    {
                        Servicio serv = new Servicio();
                        var serTable = pedServItem.ficha_costosRow;
                        serv.Material = serTable.material;
                        serv.Precio_unitario = serTable.precio_unitario;
                        serv.Id = serTable.id;
                        serv.Cant_utilizada = serTable.cantidad_material;
                        pedidotemp.Servicios.Add(serv);
                    }
                    tempCliente.Pedidos.Add(pedidotemp);
                }
                listaclientes.Add(tempCliente);
            }
            return listaclientes;
        }

        public bool AdicionarCliente(Cliente clientenew, Pedido pedidoNew)
        {
            try
            {
                pedidoNew.Estado = "pendiente";
                DataSetFacturacion.clienteDataTable clienteId = ClienteTB.GetIdByEmpresa(clientenew.Empresa);
                int idtemp = ObtenerIdCliente(clienteId);
                if (idtemp == 0)
                {
                    this.ClienteTB.Insert(clientenew.Cuenta_Bancaria, clientenew.Provincia, clientenew.Nombre_representante, clientenew.Carnet_representante, clientenew.Direccion, clientenew.Telefono, clientenew.Empresa);
                    this.ClienteTB.Update(this.ds.cliente);
                    this.ClienteTB.Fill(this.ds.cliente);
                    DataSetFacturacion.clienteDataTable cliente = ClienteTB.GetIdByEmpresa(clientenew.Empresa);
                    idtemp = ObtenerIdCliente(cliente);
                }
                else
                {
                    if (idtemp > 0)
                    {
                        if (((DataSetFacturacion.clienteRow)clienteId.Rows[0]).cuenta_bancaria != (clientenew.Cuenta_Bancaria)
                            || ((DataSetFacturacion.clienteRow)clienteId.Rows[0]).direccion != (clientenew.Direccion)
                            || ((DataSetFacturacion.clienteRow)clienteId.Rows[0]).telefono != (clientenew.Telefono)
                            || ((DataSetFacturacion.clienteRow)clienteId.Rows[0]).empresa != (clientenew.Empresa)
                            || ((DataSetFacturacion.clienteRow)clienteId.Rows[0]).nombre_representante != (clientenew.Nombre_representante)
                            || ((DataSetFacturacion.clienteRow)clienteId.Rows[0]).provincia != (clientenew.Provincia))
                        {
                            ClienteTB.Update(clientenew.Cuenta_Bancaria, clientenew.Provincia, clientenew.Nombre_representante, clientenew.Carnet_representante,
                                clientenew.Direccion, clientenew.Telefono, clientenew.Empresa, idtemp,
                                ((DataSetFacturacion.clienteRow)clienteId.Rows[0]).cuenta_bancaria, ((DataSetFacturacion.clienteRow)clienteId.Rows[0]).provincia,
                                ((DataSetFacturacion.clienteRow)clienteId.Rows[0]).nombre_representante, ((DataSetFacturacion.clienteRow)clienteId.Rows[0]).carnet_representante,
                                ((DataSetFacturacion.clienteRow)clienteId.Rows[0]).direccion, ((DataSetFacturacion.clienteRow)clienteId.Rows[0]).telefono,
                                ((DataSetFacturacion.clienteRow)clienteId.Rows[0]).empresa);
                        }
                    }
                }

                if (pedidoNew.Tipo_impresion == null)
                    pedidoNew.Tipo_impresion = "";
                if (pedidoNew.Color_impresion == null)
                    pedidoNew.Color_impresion = "";
                if (pedidoNew.Observaciones == null)
                    pedidoNew.Observaciones = ""; 

                this.PedidoTB.Insert(pedidoNew.Fecha_entrega, pedidoNew.Fecha_expedicion, idtemp, pedidoNew.Forma_pago,
                                    pedidoNew.Descuentos, pedidoNew.Tipo_documento, pedidoNew.Tipo_impresion,
                                     pedidoNew.Color_impresion, pedidoNew.Estado, pedidoNew.Coste_total,
                                     pedidoNew.Cantidad_paginas, pedidoNew.Importe_total, pedidoNew.Pago_adelantado,
                                     pedidoNew.Observaciones, pedidoNew.Cantidad_Ejemplares, pedidoNew.ManoObraFotocopia,
                                    pedidoNew.ManoObraImpresion, pedidoNew.ManoObraPresillado, pedidoNew.Paginas_por_Cara,
                                    pedidoNew.ManoObraEncuadernado, pedidoNew.ValorAgregado, pedidoNew.ManoObraCorte, clientenew.Nombre_representante, 
                                    pedidoNew.CosteTonel, pedidoNew.CostePapel, pedidoNew.ManoObraDisenno, pedidoNew.Cantidad_Hojas_Mecanografia
                                    );
                this.PedidoTB.Update(this.ds.pedido);
                this.PedidoTB.Fill(this.ds.pedido);
                int? idpedido = PedidoTB.IdUltimoPedidoInsertado();

                foreach (var item in pedidoNew.Servicios)
                {
                    DataSetFacturacion.ficha_costosDataTable fichacostosId = FichaCostosTB.GetIdByMaterial(item.Material);
                    int idfichacostos = ((DataSetFacturacion.ficha_costosRow)fichacostosId.Rows[0]).id;
                    this.CostosTB.Insert(idpedido, idfichacostos, item.Material);
                    this.CostosTB.Update(this.ds.pedidos_costos);
                    this.CostosTB.Fill(this.ds.pedidos_costos);
                }
                return true;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private int ObtenerIdCliente(DataSetFacturacion.clienteDataTable clienteId)
        {
            try
            {
                int id = 0;
                if (clienteId.Rows.Count > 0)
                    id = ((DataSetFacturacion.clienteRow)clienteId.Rows[0]).Id;
                return id;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        bool ICliente.EliminarCliente(Cliente cliente)
        {
            try
            {
                foreach (var client in ds.cliente)
                {
                    if (((DataSetFacturacion.clienteRow)client).Id == cliente.Id)
                    {
                        foreach (var item in ((DataSetFacturacion.clienteRow)client).GetpedidoRows())
                        {
                            foreach (var serv in ((DataSetFacturacion.pedidoRow)item).Getpedidos_costosRows())
                            {
                                CostosTB.Delete(serv.Id, serv.idpedido, serv.idcostos, serv.material);
                            }

                            PedidoTB.Delete(item.Id, item.fecha_entrega, item.fecha_expedicion, item.cliente, item.forma_pago, item.descuentos, item.tipo_documento, item.tipo_impresion,
                                item.color_impresion, item.estado, item.coste_total, item.cantidad_paginas, item.importe_total, item.pago_adelantado, item.observaciones,
                                item.cant_ejemplares, item.mano_obra_fotocopia, item.mano_obra_impresion, item.mano_obra_presillado, item.paginas_x_cara, item.mano_obra_encuadernado,
                                item.valor_agregado, item.mano_obra_corte, item.nombre_cliente, item.costotonel, item.costopapel, item.mano_obra_disenno, item.cantidad_hojas_mecanografia);
                        }
                    }

                    ClienteTB.Delete(client.Id, client.cuenta_bancaria, client.provincia, client.nombre_representante, client.carnet_representante,
                        client.direccion, client.telefono, client.empresa);
                    break;
                }
                this.CostosTB.Fill(ds.pedidos_costos);
                this.PedidoTB.Fill(this.ds.pedido);
                this.ClienteTB.Fill(ds.cliente);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private Cliente UsandoMetodoGetClienteById(int id)
        {
            Cliente clientetemp = new Cliente();
            DataSetFacturacion.clienteDataTable item = ClienteTB.GetClienteById(id);

            clientetemp.Id = id; 
            clientetemp.Nombre_representante = ((DataSetFacturacion.clienteRow)item.Rows[0]).nombre_representante;
            clientetemp.Carnet_representante = ((DataSetFacturacion.clienteRow)item.Rows[0]).carnet_representante;
            clientetemp.Provincia = ((DataSetFacturacion.clienteRow)item.Rows[0]).provincia;
            clientetemp.Telefono = ((DataSetFacturacion.clienteRow)item.Rows[0]).telefono;
            clientetemp.Direccion = ((DataSetFacturacion.clienteRow)item.Rows[0]).direccion;
            clientetemp.Cuenta_Bancaria = ((DataSetFacturacion.clienteRow)item.Rows[0]).cuenta_bancaria;
            clientetemp.Empresa = ((DataSetFacturacion.clienteRow)item.Rows[0]).empresa;

            clientetemp.Pedidos = CrearPedidos(((DataSetFacturacion.clienteRow)item.Rows[0]).GetpedidoRows());
            return clientetemp;
        }

        private List<Pedido> CrearPedidos(DataSetFacturacion.pedidoRow[] pedidoRow)
        {
            List<Pedido> listapedidos = new List<Pedido>();

            foreach (var item in pedidoRow)
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

                    ServicioAccess servicioap = new ServicioAccess(ClienteTB, ds, PedidoTB, CostosTB, FichaCostosTB);
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

        public List<Cliente> ClienteByNombre(string Cliente)
        {
            List<Cliente> listaclientes = new List<Cliente>();
            foreach (var item in ds.cliente.Rows)
            {
                if (((DataSetFacturacion.clienteRow)item).empresa.Contains(Cliente))
                {
                    Cliente clientetemp = CrearCliente(item);
                    listaclientes.Add(clientetemp);
                }
            }
            return listaclientes;
        }

        private static Cliente CrearCliente(object item)
        {
            Cliente clientetemp = new Modelo.Cliente();
            clientetemp.Nombre_representante = ((DataSetFacturacion.clienteRow)item).nombre_representante;
            clientetemp.Carnet_representante = ((DataSetFacturacion.clienteRow)item).carnet_representante;
            clientetemp.Provincia = ((DataSetFacturacion.clienteRow)item).provincia;
            clientetemp.Telefono = ((DataSetFacturacion.clienteRow)item).telefono;
            clientetemp.Direccion = ((DataSetFacturacion.clienteRow)item).direccion;
            clientetemp.Cuenta_Bancaria = ((DataSetFacturacion.clienteRow)item).cuenta_bancaria;
            clientetemp.Empresa = ((DataSetFacturacion.clienteRow)item).empresa;
            return clientetemp;
        }

        public Cliente ClienteDadoIdPedido(int idpedido)
        {
            Cliente clientetemp = new Cliente();
            DataSetFacturacion.pedidoDataTable pedido = this.PedidoTB.GetIdClienteByIdPedido(idpedido);
            int idcliente = ((DataSetFacturacion.pedidoRow)pedido.Rows[0]).cliente;
            clientetemp = UsandoMetodoGetClienteById(idcliente);
            return clientetemp;
        }

        public Cliente ClienteDadoId(string id)
        {
            return UsandoMetodoGetClienteById(Convert.ToInt16(id));
        }
    }
}
