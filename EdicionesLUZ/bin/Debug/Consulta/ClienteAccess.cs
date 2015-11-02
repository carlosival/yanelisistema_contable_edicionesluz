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
            pedidoNew.Estado = "Pendiente";
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
                this.PedidoTB.Insert(pedidoNew.Fecha_entrega, pedidoNew.Fecha_expedicion, idtemp, pedidoNew.Forma_pago,
                                    pedidoNew.Descuentos, pedidoNew.Tipo_documento, pedidoNew.Tipo_impresion,
                                     pedidoNew.Color_impresion, pedidoNew.Estado, pedidoNew.Cantidad_paginas,
                                     pedidoNew.Coste_total, pedidoNew.Importe_total,pedidoNew.Pago_adelantado,
                                     pedidoNew.Observaciones, pedidoNew.Cantidad_Ejemplares, pedidoNew.ManoObraFotocopia,
                                    pedidoNew.ManoObraImpresion, pedidoNew.ManoObraImpresion, pedidoNew.Paginas_por_Cara,
                                    pedidoNew.ManoObraEncuadernado,pedidoNew.ValorAgregado, pedidoNew.ManoObraCorte);
                this.PedidoTB.Update(this.ds.pedido);
                this.PedidoTB.Fill(this.ds.pedido);
                int? idpedido = PedidoTB.UltimoIdPedidoInsertado();

                foreach (var item in pedidoNew.Servicios)
                {
                    DataSetFacturacion.ficha_costosDataTable fichacostosId = FichaCostosTB.GetIdByMaterial(item.Material);
                    int idfichacostos = ((DataSetFacturacion.ficha_costosRow)fichacostosId.Rows[0]).id;
                    this.CostosTB.Insert(idpedido, idfichacostos);
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
                ClienteTB.Delete(cliente.Id, cliente.Cuenta_Bancaria, cliente.Provincia, cliente.Nombre_representante, cliente.Carnet_representante, cliente.Direccion, cliente.Telefono, cliente.Empresa);
                this.ClienteTB.Update(this.ds.cliente);
                this.ClienteTB.Fill(this.ds.cliente);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Cliente ClienteDadoId(int id)
        {
            throw new NotImplementedException();
        }

        public Cliente ClienteDadoCarnet()
        {
            throw new NotImplementedException();
        }

        public List<Cliente> ClienteDadoNombre()
        {
            throw new NotImplementedException();
        }


        public List<Cliente> ClienteByNombre(string Cliente)
        {
           List<Cliente> listaclientes = new List<Cliente>();
           foreach (var item in ds.cliente.Rows)
           {
               if (((DataSetFacturacion.clienteRow)item).empresa.Contains(Cliente))
               {
                  Cliente tempCliente=  CrearCliente(item);
                  listaclientes.Add(tempCliente);
               }
           }
           return listaclientes;
        }

        private static Cliente CrearCliente(object item)
        {
            Cliente clientetemp = new Cliente();
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
            DataSetFacturacion.clienteDataTable clienteReturn = ClienteTB.GetClienteById(idcliente);
            clientetemp= CrearCliente(clienteReturn);
            return clientetemp;
        }
    }
}
