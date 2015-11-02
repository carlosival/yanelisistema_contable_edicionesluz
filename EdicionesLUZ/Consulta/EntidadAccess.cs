using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EdicionesLUZ.Interfaces;
using EdicionesLUZ.Modelo;

namespace EdicionesLUZ.Consulta
{
    class EntidadAccess:IEntidad
    {

        public List<Modelo.Pedido> TodosPedidos()
        {
            List<Modelo.Pedido> listapedidos = new List<Pedido>();
            //librosDataSet ds = AccesData.AccesData.Accesspoint.Librosds;

            //librosDataSet.pedidoDataTable pedidos = ds.pedido;
            //foreach (var item in pedidos.Rows)
            //{
            //    Pedido newpedido = new Pedido();

            //    newpedido.Id = ((librosDataSet.pedidoRow)item).Id;
            //    newpedido.Cantidad_paginas = ((librosDataSet.pedidoRow)item).cantidad_paginas;
            //    newpedido.Fecha_entrega = ((librosDataSet.pedidoRow)item).fecha_entrega;
            //    newpedido.Cantidad_Ejemplares = ((librosDataSet.pedidoRow)item).cant_ejemplares;
            //   // newpedido.Cliente=((librosDataSet.pedidoRow)item).ge 
            //    listapedidos.Add(newpedido);
            //}
            return listapedidos;
        }

        public List<Modelo.Servicio> TodosServicios()
        {
            throw new NotImplementedException();
        }

        public Entidad EntidadDadoId(int id)
        {
        //{
        //    librosDataSet ds = AccesData.AccesData.Accesspoint.Librosds;
        //    librosDataSet.entidadDataTable entidad = ds.entidad;


            try
            {
                //librosDataSet.entidadRow ent = entidad.FindById(id);
                Entidad entidadap = new Entidad();
                //entidadap.Cuenta_bancaria = ent.cuenta_bancaria;
                //entidadap.Direccion = ent.direccion;
                //entidadap.Nombre = ent.nombre;
                //entidadap.Representante = ent.representante;
                //entidadap.Pedidos = null;
                
                
                return entidadap;

            }
            catch 
            {

                return null;
            } 
            
        
        }

        List<Cliente> IEntidad.TodosClientes()
        {
            throw new NotImplementedException();
        }


       
    }
}
