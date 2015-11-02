using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using EdicionesLUZ.Modelo;
using EdicionesLUZ.Visual;
using System.Collections.Generic;

namespace EdicionesLUZ.Factura
{
    public partial class FacturaModelo : DevExpress.XtraReports.UI.XtraReport
    {

        Pedido pedido;
        public Pedido Pedido
        {
            get { return pedido; }
            set { pedido = value; }
        }

        Cliente cliente;
        public Cliente Cliente
        {
            get { return cliente; }
            set { cliente = value; }
        }

        public List<VServicios> VServicio { get; set; }

        public FacturaModelo(Pedido pedido, Cliente cliente, List<VServicios> VServicio)
        {
            InitializeComponent();
            

            this.Pedido = pedido;
            this.Cliente = cliente;
            this.VServicio = VServicio; 

            if (Cliente != null)
            {
                if (Cliente.Carnet_representante != null)
                    xrlCarnetRepresentante.Text = Cliente.Carnet_representante.ToString();
                if (Cliente.Nombre_representante != null)
                    xrlNombreRepresentante.Text = Cliente.Nombre_representante;
                if (Cliente.Provincia != null)
                    xrlProvincia.Text = Cliente.Provincia;
                if (Cliente.Telefono != null)
                    xrlTelefono.Text = Cliente.Telefono.ToString();
                if (Cliente.Cuenta_Bancaria != null)
                    xrlCuentaBancaria.Text = Cliente.Cuenta_Bancaria.ToString();
                if (Cliente.Direccion != null)
                    xrlDireccion.Text = Cliente.Direccion;

                xrlIdFactura.Text = Pedido.Id.ToString();
                if (VServicio != null)
                {
                    foreach (var item in VServicio)
                    {
                        ConstruirServicios(item, VServicio.IndexOf(item));  
                    }
                }
                xrlImp.Text =Convert.ToDecimal(Pedido.Importe_total).ToString("N");
               
                xrlFecha.Text = DateTime.Today.Date.ToShortDateString();
                xrlNombreCliente.Text = Cliente.Nombre_representante;
                xrlcliente.Text = Cliente.Empresa;
                xrlIdFactura.Text = Pedido.Id.ToString();

                xrlTrabajo.Text = Pedido.Tipo_documento;
            }
        }

        private void ConstruirServicios(VServicios item, int p)
        {
            if (item.Servicio == "Descuentos")
            {
                    xrlDescuentos.Visible = true;
                    xrlValorDescuentos.Visible = true;
                    xrlValorDescuentos.Text ="-" + Convert.ToDecimal(item.Mano_Obra).ToString("N") + ".00";
 
            }
            if (p == 0 && item.Servicio != "Descuentos")
            {
                xrlmat1.Text = item.Servicio;
                xrlmat1.Size = new System.Drawing.Size(1800, 58);

                if(item.Cant_utilizada == 0)
                    xrlcant1.Text = "";
                else
                    xrlcant1.Text = item.Cant_utilizada.ToString();

                if (item.Precio_unitario != 0)
                {
                    xrlcosto.Text = Convert.ToDecimal(item.Precio_unitario).ToString("N"); 
                    xrlimp1.Text =Convert.ToDecimal((item.Precio_unitario * item.Cant_utilizada)).ToString("N");
                }
                else
                {
                    xrlcosto.Text = "";
                    xrlimp1.Text =Convert.ToDecimal(item.Mano_Obra).ToString("N");
                }

                xrlmat1.Visible = true;
                xrlcosto.Visible = true;
                xrlcant1.Visible = true;
                xrlimp1.Visible = true;
            }

            if (p == 1 && item.Servicio != "Descuentos")
            {
                xrlmat2.Text = item.Servicio;
                xrlmat2.Size = new System.Drawing.Size(1800, 58);

                if (item.Cant_utilizada == 0)
                    xrlcant2.Text = "";
                else
                    xrlcant2.Text = item.Cant_utilizada.ToString();

                if (item.Precio_unitario != 0)
                {
                    xrlcosto2.Text = item.Precio_unitario.ToString() ;
                    xrlimp2.Text = Convert.ToDecimal((item.Precio_unitario * item.Cant_utilizada)).ToString("N");
                }
                else
                {
                    xrlcosto2.Text = "";
                    xrlimp2.Text = Convert.ToDecimal(item.Mano_Obra).ToString("N");
                }
                
                xrlmat2.Visible = true;
                xrlcosto2.Visible = true;
                xrlcant2.Visible = true;
                xrlimp2.Visible = true;
            }
            if (p == 2 &&  item.Servicio != "Descuentos")
            {
                xrlmat3.Text = item.Servicio;
                xrlmat3.Size = new System.Drawing.Size(1800, 58);


                if (item.Cant_utilizada == 0)
                    xrlcant3.Text = "";
                else
                    xrlcant3.Text = item.Cant_utilizada.ToString();

                if (item.Precio_unitario != 0)
                {
                    xrlcosto3.Text =Convert.ToDecimal(item.Precio_unitario).ToString("N");
                    xrlimp3.Text =Convert.ToDecimal((item.Precio_unitario * item.Cant_utilizada)).ToString("N"); 
                }
                else
                {
                    xrlcosto3.Text = "";
                    xrlimp3.Text = Convert.ToDecimal(item.Mano_Obra).ToString("N");
                }
                xrlmat3.Visible = true;
                xrlcosto3.Visible = true;
                xrlcant3.Visible = true;
                xrlimp3.Visible = true;
            }
            if (p == 3 && item.Servicio != "Descuentos")
            {
                xrlmat4.Text = item.Servicio;
                xrlmat4.Size = new System.Drawing.Size(1800, 58);

                if (item.Cant_utilizada == 0)
                    xrlcant4.Text = "";
                else
                    xrlcant4.Text = item.Cant_utilizada.ToString();

                if (item.Precio_unitario != 0)
                {
                    xrlcosto4.Text = Convert.ToDecimal(item.Precio_unitario).ToString("N");
                    xrlimp4.Text = Convert.ToDecimal((item.Precio_unitario * item.Cant_utilizada)).ToString("N"); 
                }
                else
                {
                    xrlcosto4.Text = "";
                    xrlimp4.Text = Convert.ToDecimal(item.Mano_Obra).ToString("N");
                }
               
                xrlmat4.Visible = true;
                xrlcant4.Visible = true;
                xrlcosto4.Visible = true;
                xrlimp4.Visible = true;
            }
            if (p == 4 && item.Servicio != "Descuentos")
            {
                xrlmat5.Text = item.Servicio;
                xrlmat5.Size = new System.Drawing.Size(1800, 58);
                if (item.Cant_utilizada == 0)
                    xrlcant5.Text = "";
                else
                    xrlcant5.Text = item.Cant_utilizada.ToString();

                if (item.Precio_unitario != 0)
                {
                    xrlcosto5.Text = item.Precio_unitario.ToString(); 
                    xrlimp5.Text = Convert.ToDecimal((item.Precio_unitario * item.Cant_utilizada)).ToString("N"); ;
                }
                else
                {
                    xrlcosto5.Text = "";
                    xrlimp5.Text = Convert.ToDecimal(item.Mano_Obra).ToString("N"); 
                }
               
                xrlmat5.Visible = true;
                xrlcant5.Visible = true;
                xrlcosto5.Visible = true;
                xrlimp5.Visible = true;
            }
            if (p == 5 && item.Servicio != "Descuentos")
            {
                xrlmat6.Text = item.Servicio;
                xrlmat5.Size = new System.Drawing.Size(1800, 58);
                if (item.Cant_utilizada == 0)
                    xrlcant6.Text = "";
                else
                    xrlcant6.Text = item.Cant_utilizada.ToString();

                if (item.Precio_unitario != 0)
                {
                    xrlcosto6.Text = Convert.ToDecimal(item.Precio_unitario).ToString("N") ;
                    xrlimp6.Text = Convert.ToDecimal((item.Precio_unitario * item.Cant_utilizada)).ToString("N") ;
                }
                else
                {
                    xrlcosto6.Text = "";
                    xrlimp6.Text = Convert.ToDecimal(item.Mano_Obra).ToString("N");
                }
                xrlmat6.Visible = true;
                xrlcant6.Visible = true;
                xrlcosto6.Visible = true;
                xrlimp6.Visible = true;
            }
            
        }

    }
}
