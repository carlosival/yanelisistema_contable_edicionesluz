using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OleDb;

namespace EdicionesLUZ.AccesData
{
    class AccesData
    {
        static readonly AccesData accesspoint= new AccesData();

        private  AccesData() 
        {
        }

        public static AccesData Accesspoint
        {
            get
            {
                return accesspoint;
            }
           
        }


        
        }

        //private  librosDataSet createDataSet()
        //{
            
        //    //librosDataSetTableAdapters.clienteTableAdapter clienteta = new librosDataSetTableAdapters.clienteTableAdapter();
        //    //librosDataSetTableAdapters.pedidoTableAdapter pedidota = new librosDataSetTableAdapters.pedidoTableAdapter();
        //    //librosDataSetTableAdapters.entidadTableAdapter entidadta = new librosDataSetTableAdapters.entidadTableAdapter();
        //    //librosDataSetTableAdapters.ficha_costosTableAdapter costosta = new librosDataSetTableAdapters.ficha_costosTableAdapter();
        //    //librosDataSetTableAdapters.pedidos_costosTableAdapter ped_costosta = new librosDataSetTableAdapters.pedidos_costosTableAdapter();

        //    //t  clienteta.Fill(this.librosds.cliente);
        //    //pedidota.Fill(this.librosds.pedido);
        //    //entidadta.Fill(this.librosds.entidad);
        //    //costosta.Fill(this.librosds.ficha_costos);
        //    //ped_costosta.Fill(this.librosds.pedidos_costos);
           

        //    //return this.librosds;
        //    return null;
        //}
    
    }

  
