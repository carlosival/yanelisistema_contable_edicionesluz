using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using EdicionesLUZ.Modelo;
using EdicionesLUZ.Visual;
using EdicionesLUZ.Managers;
using DevExpress.XtraEditors.Controls;
using EdicionesLUZ.Factura;
using DevExpress.Utils;

namespace EdicionesLUZ
{
    public partial class Wizard : DevExpress.XtraEditors.XtraForm
    {
        Pedido pedido;
        Cliente cliente;
        DateTime fechaultimoPedido;
        EntidadManager entidadManager;
        DataSetFacturacionTableAdapters.clienteTableAdapter ClienteTB;
        DataSetFacturacionTableAdapters.pedidoTableAdapter PedidoTB;
        DataSetFacturacionTableAdapters.pedidos_costosTableAdapter CostosTB;
        DataSetFacturacionTableAdapters.ficha_costosTableAdapter FichaCostosTB;
        DataSetFacturacion ds;
        public event EventHandler pedidoInsertado;

       
        public EntidadManager EntidadManager
        {
            get
            {
                if (entidadManager == null)
                    entidadManager = new EntidadManager(ClienteTB, ds,PedidoTB, CostosTB,FichaCostosTB );
                return entidadManager;
            }
            set { entidadManager = value; }
        }
       

        public Wizard(DateTime fechaultimoPedido, DataSetFacturacionTableAdapters.clienteTableAdapter ClienteTB, DataSetFacturacion ds, DataSetFacturacionTableAdapters.pedidoTableAdapter PedidoTB, DataSetFacturacionTableAdapters.pedidos_costosTableAdapter CostosTB, DataSetFacturacionTableAdapters.ficha_costosTableAdapter FichaCostosTB)
        {
            InitializeComponent();
            wizardNuevoPedido.NextText = "Próximo";
            wizardNuevoPedido.CancelText = "Cancelar";
            this.fechaultimoPedido = fechaultimoPedido;

            this.ClienteTB = ClienteTB;
            this.PedidoTB = PedidoTB;
            this.CostosTB = CostosTB;
            this.FichaCostosTB = FichaCostosTB;
            this.ds = ds;
            
            pedido = new Pedido();
            cliente = new Cliente();
            wpgFinal.AllowBack = false;
            wpgInsertando.AllowBack = false;

        }

        private void Wizard_Load(object sender, EventArgs e)
        {
            this.clienteTableAdapter.Fill(this.dataSetFacturacion.cliente);
            cbxEmpresa.DataSource = EntidadManager.TodosClientes();
            cbxEmpresa.SelectedIndex = -1;

            datefechaentrega.DateTime = FechaUltimoPedidoMasUno();
            datefechaexpedicion.DateTime = DateTime.Today;
        }

        private void wizardNuevoPedido_NextClick(object sender, DevExpress.XtraWizard.WizardCommandButtonClickEventArgs e)
        {
            wizardNuevoPedido.NextText = "Próximo";
            wizardNuevoPedido.CancelText = "Cancelar";

            wpgCliente.AllowNext = false;
            wpgTipoPedido.AllowNext = false;
            wpgImpresion.AllowNext = false;
            wpgEncuadernado.AllowNext = false;   
            wpgFotocopia.AllowNext = false;
            wpgDiseño.AllowNext = false;
            wpgMecanografia.AllowNext = false;
            wpgCorte.AllowNext = false;

            ValidadPaginaEncuadernadoWizard();
            ValidarPaginaClienteWizard();
            ValidadPaginaImpresionWizard();
            ValidadPaginaFotocopiaWizard();
            ValidandoPaginaDiseño();
            ValidarPaginaCorteWizard();
            ValidarPaginaMecanografiaWizard();

            /* Pagina de Datos del Cliente*/
            if (wizardNuevoPedido.SelectedPage.Name == wpgCliente.Name)
            {
                if (cliente == null)
                    cliente = new Cliente();
                cliente.Nombre_representante = txtnombrecliente.Text;
                cliente.Empresa = cbxEmpresa.Text;
                cliente.Carnet_representante = txtcarnetcliente.Text;
                cliente.Direccion = txtdireccioncliente.Text;
                cliente.Provincia = cbxprovincia.Text;
                cliente.Telefono =  txttelefonocliente.Text;
                cliente.Cuenta_Bancaria =  txtcuentabancariacliente.Text;
            }

            /* Pagina tipo de servicio*/
            if (wizardNuevoPedido.SelectedPage.Name == wpgTipoPedido.Name)
            {
                pedido.Tipo_documento = cbxtipodocumento.Text;
                if (checkTipoServicio.SelectedItems.Count > 0)
                {
                    wpgTipoPedido.AllowNext = true;
                }
            }

            /* Pagina de impresion*/
            if (wizardNuevoPedido.SelectedPage.Name == wpgImpresion.Name)
            {
                pedido.Cantidad_Ejemplares = Convert.ToInt16(txtcantejemplares.Text);
                pedido.Cantidad_paginas = Convert.ToDouble(txtcantpag.Text);
                pedido.Tipo_impresion = cbxtipoimpresion.Text;
                pedido.Color_impresion = cbxcolorimpresion.Text;
                pedido.Paginas_por_Cara = Convert.ToInt16(txtpaginasxhoja.Text);

                if (pedido.Servicios == null)
                    pedido.Servicios = new List<Servicio>();

                if ((((CheckedListBoxItem)checkTipoServicio.GetItem(2)).CheckState == CheckState.Checked))
                {
                    pedido.ManoObraPresillado = pedido.Cantidad_Ejemplares * 1;
                }

                if (!ExisteServicio(cbxTipoToner.Text))
                {
                    Servicio serv = CrearServicio(cbxTipoToner.Text);
                    serv.Unidad_medida = "U";
                    pedido.Servicios.Add(serv);
                }
                pedido.ManoObraImpresion = Math.Round(Convert.ToDouble(txtManoObraImpresion.Text)*9.40, MidpointRounding.AwayFromZero);
                if (!ExisteServicio(cbxtipopapel.Text))
                {
                    Servicio serv = CrearServicio(cbxtipopapel.Text);
                    pedido.Servicios.Add(serv);
                }
                lblInfDesc.Text = "Hasta el momento el cliente tiene un importe de: " + CalcularImporteTotal().ToString() + ".00 CUP .Especifique si desea realizarle algún descuento.";
            }

            /*Pagina de Fotocopia*/
            if (wizardNuevoPedido.SelectedPage.Name == wpgFotocopia.Name)
            {
                pedido.Cantidad_Ejemplares = Convert.ToInt16(txtEjemplaresFotocopia.Text);
                pedido.Cantidad_paginas = Convert.ToDouble(txtPaginasFotocopia.Text);
                pedido.Tipo_impresion = cbxTipoFotocopia.Text;
                pedido.Paginas_por_Cara = Convert.ToInt16(txtPagxHojaFotocopia.Text);
                pedido.Color_impresion = "b/n";
                pedido.ManoObraFotocopia = Math.Round(Convert.ToDouble(txtManoObraFotocopia.Text) * 9.40, MidpointRounding.AwayFromZero);

                if (pedido.Servicios == null)
                    pedido.Servicios = new List<Servicio>();

                 if ((((CheckedListBoxItem)checkTipoServicio.GetItem(2)).CheckState == CheckState.Checked))
                {
                    pedido.ManoObraPresillado = pedido.Cantidad_Ejemplares * 1;
                }

                if (!ExisteServicio(cbxTipoTonerFotocopia.Text))
                {
                    Servicio serv = CrearServicio(cbxTipoTonerFotocopia.Text);
                    pedido.Servicios.Add(serv);
                }

                if (!ExisteServicio(cbxTipoPapelFotocopia.Text))
                {
                    Servicio serv = CrearServicio(cbxTipoPapelFotocopia.Text);
                    pedido.Servicios.Add(serv);
                }
                lblInfDesc.Text = "Hasta el momento el cliente tiene un importe de: " + CalcularImporteTotal().ToString() + ".00 CUP. Especifique si desea realizarle algún descuento.";

            }

            /*Pagina de Encuadernado*/
            if (wizardNuevoPedido.SelectedPage.Name == wpgEncuadernado.Name)
            {
                pedido.ManoObraEncuadernado = Math.Round(Convert.ToDouble(txtManoObraEncuadernado.Text)*9.40, MidpointRounding.AwayFromZero);
                lblInfDesc.Text = "Hasta el momento el cliente tiene un importe de: " + CalcularImporteTotal().ToString() + ".00 CUP. Especifique si desea realizarle algún descuento.";

            }

            /**Pagina de Corte*/
            if (wizardNuevoPedido.SelectedPage == wpgCorte)
            {
                pedido.ManoObraCorte = Math.Round(Convert.ToDouble(txtmanoobraCorte.Text)* 9.40, MidpointRounding.AwayFromZero);
                lblInfDesc.Text = "Hasta el momento el cliente tiene un importe de: " + CalcularImporteTotal().ToString() + ".00 CUP. Especifique si desea realizarle algún descuento.";

            }

            /*Pagina de diseño*/
            if (wizardNuevoPedido.SelectedPage.Name == wpgDiseño.Name)
            {
            //Adicionando diseño a los servicios
                bool temp = false; 
                foreach (var item in checkDiseño.Items)
                {
                    if (((CheckedListBoxItem)item).CheckState == CheckState.Checked)
                    {
                        temp = ExisteServicio(((CheckedListBoxItem)item).Value.ToString());
                        if (!temp)
                            pedido.Servicios.Add(CrearServicio(((CheckedListBoxItem)item).Value.ToString()));
                    }
                }

                pedido.ManoObraDisenno = Math.Round(Convert.ToDouble(txtmanoobradiseño.Text), MidpointRounding.AwayFromZero);
                lblInfDesc.Text = "Hasta el momento el cliente tiene un importe de: " + CalcularImporteTotal().ToString() + ".00 CUP. Especifique si desea realizarle algún descuento.";
            }

            /*Pagina de mecanografia*/
            if (wizardNuevoPedido.SelectedPage.Name == wpgMecanografia.Name)
            {
                //Adicionando mecanografia a los servicios
                bool temp = false;
                pedido.Cantidad_Hojas_Mecanografia = Convert.ToInt16(txtcantidadhojasmecanografia.Text);
                foreach (var item in checkMecanografia.Items)
                {
                    if (((CheckedListBoxItem)item).CheckState == CheckState.Checked)
                    {
                        temp = ExisteServicio(((CheckedListBoxItem)item).Value.ToString());
                        if (!temp)
                            pedido.Servicios.Add(CrearServicio(((CheckedListBoxItem)item).Value.ToString()));
                    }
                }
               
                lblInfDesc.Text = "Hasta el momento el cliente tiene un importe de: " + CalcularImporteTotal().ToString() + ".00 CUP. Especifique si desea realizarle algún descuento.";
            }


            /* Pagina resumen, seria el next de datos del servicio */
            if (wizardNuevoPedido.SelectedPage.Name == wpgFechaPago.Name)
            {
               
                wizardNuevoPedido.NextText = "Guardar";
                pedido.Forma_pago = cbxformapago.Text;

                //if (cliente.Cuenta_Bancaria != "")
                //{
                //    cbxformapago.
                //}

                if (txtdescuentos.Text != "")
                {
                    int desc = Convert.ToInt16(txtdescuentos.Text);
                    pedido.Descuentos = Math.Round(Convert.ToDouble(desc), MidpointRounding.AwayFromZero);
                }
                
                pedido.Observaciones = memoObservaciones.Text;
                LlenarPaginaResumen();
            }
            /*Pagina para guardar*/
            if (wizardNuevoPedido.SelectedPage.Name == wpgResumen.Name)
            {
                backgroundWorker1.RunWorkerAsync();
            }
        }

        private void ValidarPaginaMecanografiaWizard()
        {
            if (checkMecanografia.CheckedItems.Count >0  && txtcantidadhojasmecanografia.Text != "" )
                wpgMecanografia.AllowNext = true;
        }

        private void ValidarPaginaCorteWizard()
        {
            if (txtmanoobraCorte.Text != "")
                wpgCorte.AllowNext = true;
        }

        private void ValidandoPaginaDiseño()
        {
            if (checkDiseño.CheckedItems.Count > 0 && txtmanoobradiseño.Text != "")
                wpgDiseño.AllowNext = true;
        }

        private bool ExisteServicio(string item)
        {
            foreach (var serv in pedido.Servicios)
            {
                if (serv.Material == item)
                {
                    return true;
                }
            }
            return false;
        }

        private Servicio CrearServicio(string nombreservicio)
        {
            Servicio servicio = new Servicio();
            servicio.Material = nombreservicio;
            servicio.Precio_unitario = EntidadManager.CostoDadoMaterial(nombreservicio);


            if (pedido.ManoObraImpresion != 0 || pedido.ManoObraFotocopia != 0)
                servicio.Cant_utilizada = CantHojas(pedido);

            if (pedido.Cantidad_Hojas_Mecanografia != 0.0)
            {
                servicio.Cant_utilizada = pedido.Cantidad_Hojas_Mecanografia;
                costomecanografia = servicio.Precio_unitario;
            }

            return servicio;
        }

        private void ValidarPaginaClienteWizard()
        {
            if (txtnombrecliente.Text != "" && cbxEmpresa.Text != "" && txtcarnetcliente.Text != "" &&
                    txtcarnetcliente.Text.Length == 11 && txtcuentabancariacliente.Text.Length >= 16 &&
                    txtdireccioncliente.Text != "" && txttelefonocliente.Text != "")
                wpgCliente.AllowNext = true;
        }

        private void ValidadPaginaImpresionWizard()
        {
            if (txtcantejemplares.Text != "" && txtcantpag.Text != "" && txtpaginasxhoja.Text != "" &&
           txtManoObraImpresion.Text != "")
                wpgImpresion.AllowNext = true;
        }

        private void ValidadPaginaFotocopiaWizard()
        {
            if (txtEjemplaresFotocopia.Text != "" && txtPaginasFotocopia.Text != "" &&
           txtManoObraFotocopia.Text != "")
                wpgFotocopia.AllowNext = true;
        }

        private void ValidadPaginaEncuadernadoWizard()
        {
            if (txtManoObraEncuadernado.Text != "")
                wpgEncuadernado.AllowNext = true;
        }

        private DateTime FechaUltimoPedidoMasUno()
        {
            if (fechaultimoPedido.Year == 1980)
                return DateTime.Today;
            else
            {
               fechaultimoPedido = fechaultimoPedido.AddDays((Double)1);
            }
            return fechaultimoPedido;
        }

        private void LlenarPaginaResumen()
        {
           List<VServicios> vservcios = new List<VServicios>();
            lblclienter.Text = cliente.Nombre_representante;
            lbldescuentor.Text = pedido.Descuentos.ToString() + " % ";
            double costoTotal = CalcularCostoTotal();
            double importeTotal = CalcularImporteTotal();

            if (pedido.Descuentos != 0.0)
            {
                double descuentos = (pedido.Descuentos / 100) * importeTotal;
                importeTotal = importeTotal - Math.Round(descuentos, MidpointRounding.AwayFromZero);
                importeTotal = Math.Round(importeTotal, MidpointRounding.AwayFromZero);
            }

            lblcostor.Text = costoTotal.ToString(); 
            pedido.Coste_total = costoTotal;
            lblimporter.Text = importeTotal.ToString();
            pedido.Importe_total = importeTotal;
            lblfechaexpr.Text = pedido.Fecha_expedicion.ToShortDateString();
            lblfechaentr.Text = pedido.Fecha_entrega.ToShortDateString();

            lblPagoAdelantado.Text = pedido.Pago_adelantado.ToString();

            vservcios = Llenarlistasvservicios(pedido);
            gridControl1.DataSource = null;
            gridControl1.DataSource = vservcios;
        }

        public List<VServicios> Llenarlistasvservicios(Pedido pedidonew)
        {
            List<VServicios> vservcios = new List<VServicios>();

            if (pedidonew.ManoObraImpresion != 0.0)
            {
                VServicios vserv = new VServicios();
                vserv.Servicio = "Papel";
                vserv.Cant_utilizada =Math.Round(CantHojas(pedidonew), MidpointRounding.AwayFromZero) * pedidonew.Cantidad_Ejemplares;
                vserv.Precio_unitario = pedidonew.CostePapel;
                
                vservcios.Add(vserv);

                VServicios vserv2 = new VServicios();
                vserv2.Servicio = "Impresión " + pedidonew.Tipo_impresion;
                vserv2.Cant_utilizada = pedidonew.Cantidad_Ejemplares * pedidonew.Cantidad_paginas;
                vserv2.Precio_unitario = pedidonew.CosteTonel; 
                vservcios.Add(vserv2);
            }

            if (pedidonew.ManoObraFotocopia != 0.0)
            {
                VServicios vserv = new VServicios();
                vserv.Servicio = "Papel";
                vserv.Cant_utilizada = Math.Round(CantHojas(pedidonew), MidpointRounding.AwayFromZero) * pedidonew.Cantidad_Ejemplares;
                vserv.Precio_unitario = pedidonew.CostePapel;
                vservcios.Add(vserv);

                VServicios vserv2 = new VServicios();
                vserv2.Servicio = "Fotocopia " + pedidonew.Tipo_impresion;
                vserv2.Cant_utilizada = pedidonew.Cantidad_Ejemplares * pedidonew.Cantidad_paginas;
                vserv2.Precio_unitario = pedidonew.CosteTonel;
                vservcios.Add(vserv2);
            }

            if (pedidonew.ManoObraPresillado != 0.0)
            {
                VServicios vserv = new VServicios();
                vserv.Servicio = "Presillado";
                vserv.Cant_utilizada = pedidonew.Cantidad_Ejemplares;
                vserv.Precio_unitario = 1.00;
                vservcios.Add(vserv);
            }

            if (pedidonew.ManoObraEncuadernado != 0.0)
            {
                VServicios vserv = new VServicios();
                vserv.Servicio = "Encuadernado";
                vserv.Cant_utilizada = pedidonew.Cantidad_Ejemplares;
                vserv.Mano_Obra = pedidonew.ManoObraEncuadernado;
                vservcios.Add(vserv);
            }

            if (pedidonew.ManoObraCorte != 0.0)
            {
                VServicios vserv = new VServicios();
                vserv.Servicio = "Corte";
                vserv.Cant_utilizada = pedidonew.Cantidad_Ejemplares;
                vserv.Mano_Obra = pedidonew.ManoObraCorte;
                vservcios.Add(vserv);
            }

            if (pedidonew.ManoObraDisenno != 0.0)
            {
                VServicios vserv = new VServicios();
                vserv.Servicio = "Diseño";
                vserv.Mano_Obra = pedidonew.ManoObraDisenno;
                vservcios.Add(vserv);
            }

            if (pedidonew.Cantidad_Hojas_Mecanografia != 0.0)
            {
                foreach (var item in  pedidonew.Servicios)
                {
                    if (item.Material.Contains("Mecanografia"))
                    {
                        VServicios vserv = new VServicios();
                        vserv.Servicio = item.Material;
                        vserv.Cant_utilizada = pedidonew.Cantidad_Hojas_Mecanografia;
                        vserv.Precio_unitario = item.Precio_unitario;
                        vserv.Mano_Obra = CalcularCostoMecanografia(pedidonew);
                        vservcios.Add(vserv);
                    }
                }  
            }

            if (pedidonew.ValorAgregado != 0.0 || pedidonew.ManoObraFotocopia != 0.0 || pedidonew.ManoObraImpresion != 0.0)
            {
                VServicios vservAgregado = new VServicios();
                vservAgregado.Servicio = "Valor Agregado";
                vservAgregado.Mano_Obra += pedidonew.ValorAgregado;
                vservAgregado.Mano_Obra += pedidonew.ManoObraFotocopia;
                vservAgregado.Mano_Obra += pedidonew.ManoObraImpresion;
                vservcios.Add(vservAgregado);
            }

            if (pedidonew.Descuentos != 0.0)
            {
                VServicios vservAgregado = new VServicios();
                vservAgregado.Servicio = "Descuentos";
                double descuentos = CalcularImporteTotal() - pedidonew.Importe_total;
                vservAgregado.Mano_Obra = descuentos;
                vservcios.Add(vservAgregado);
            }
            return vservcios;
        }

        private double CalcularImporteTotal()
        {
            double costo = CalcularCostoTotal();
            double importeTotal =0 ;
            if (txtValorAgregado.Text != "")
            {
                double valorAgregado  = costo * Convert.ToDouble(txtValorAgregado.Text);
                importeTotal = costo + valorAgregado;
                pedido.ValorAgregado = valorAgregado;
            }
            else
                importeTotal = costo;

            return Math.Round(importeTotal, MidpointRounding.AwayFromZero);
        }

        private double CalcularCostoTotal()
        {
            double costo = 0;
            double total = 0;
            double costoImpresion = 0;
            if (pedido.ManoObraImpresion != 0.0 || pedido.ManoObraFotocopia != 0.0)
            {
                costoImpresion = CalcularCostoImpresion(pedido);
            }
            if (pedido.ManoObraFotocopia != 0.0)
                costo += pedido.ManoObraFotocopia;
            if (pedido.ManoObraImpresion != 0.0)
                costo += pedido.ManoObraImpresion;

            if (pedido.ManoObraPresillado != 0.0)
                costo += pedido.ManoObraPresillado;
            if (pedido.ManoObraEncuadernado != 0.0)
                costo += pedido.ManoObraEncuadernado;
            if (pedido.ManoObraCorte != 0.0)
                costo += pedido.ManoObraCorte;
            if (pedido.ManoObraDisenno != 0.0)
                costo += pedido.ManoObraDisenno;
            
            total = costo + costoImpresion + CalcularCostoMecanografia(pedido);
            return Math.Round(total, MidpointRounding.AwayFromZero);
        }

        private double CalcularCostoImpresion(Pedido pedido)
        {
            double costoImpresion = 0;
            double costototalpapel = 0;
            double costototaltoner = 0;
            double canthojas =  CantHojas(pedido);

            foreach (var item in pedido.Servicios)
            {
                if (item.Precio_unitario == 0)
                {
                    item.Precio_unitario = EntidadManager.CostoDadoMaterial(item.Material);
                }

                if (item.Material.Contains("Papel"))
               {
                   pedido.CostePapel = item.Precio_unitario;
                   costototalpapel = item.Precio_unitario * Math.Round(canthojas, MidpointRounding.AwayFromZero) * pedido.Cantidad_Ejemplares;
                }

                if (item.Material.Contains("Toner"))
                {
                    pedido.CosteTonel = item.Precio_unitario;
                    if (pedido.Tipo_impresion == "Tiro y Retiro")
                        costototaltoner = (pedido.CosteTonel * canthojas) * 2 * pedido.Cantidad_Ejemplares;
                    else
                        costototaltoner = pedido.CosteTonel * canthojas * pedido.Cantidad_Ejemplares;
                }
            }
            costoImpresion = costototalpapel + costototaltoner;
            return costoImpresion;
        }
       
        private double CalcularCostoMecanografia(Pedido pedido)
        {
            double costomecanografiatotal = 0;

                foreach (var item in pedido.Servicios)
                {
                    if (item.Material.Contains("Mecanografia"))
                    {
                        if (item.Precio_unitario == 0)
                        {
                            item.Precio_unitario = EntidadManager.CostoDadoMaterial(item.Material);
                        }
                        costomecanografia = item.Precio_unitario;
                        break;
                    }
                }
                costomecanografiatotal = costomecanografia * pedido.Cantidad_Hojas_Mecanografia; 
            
            return costomecanografiatotal;
        }

        private double CantHojas(Pedido pedido)
        {
            double cantHojas = 0;
            if (pedido.Tipo_impresion == "Tiro y Retiro")
                cantHojas = (pedido.Cantidad_paginas / 2) / pedido.Paginas_por_Cara;
            else
                cantHojas = (pedido.Cantidad_paginas / 1) / pedido.Paginas_por_Cara;
            return cantHojas;
        }

        private void wizardNuevoPedido_CancelClick(object sender, CancelEventArgs e)
        {
            if (XtraMessageBox.Show("Usted está seguro que desea salir?", "Salir", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
            {
                this.Close();
            }
        }

        private void wizardNuevoPedido_FinishClick(object sender, CancelEventArgs e)
        {
            if (checkEdit1.CheckState == CheckState.Checked)
            {

                FacturaModelo factura = new FacturaModelo(EntidadManager.PedidoConIdDadoPedido(pedido), cliente, this.Llenarlistasvservicios(pedido));
                //printControl1.PrintingSystem = factura.PrintingSystem;
                factura.CreateDocument(false);
                factura.ExportToPdf("D:/Sistema Contable Ediciones Luz/Facturas/Factura " + pedido.Id.ToString() + ".pdf");
                factura.ShowPreviewDialog();

                //VFactura vfactura = new VFactura();
                //vfactura.Pedido = EntidadManager.PedidoConIdDadoPedido(pedido); 
                //vfactura.Cliente = cliente;
                //vfactura.VServicio = this.Llenarlistasvservicios(pedido);
                //vfactura.Show();
            }
            this.Close();
        }

        //Eventos para validar la entrada de datos
        private void txtnombrecliente_KeyPress(object sender, KeyPressEventArgs e)
        {
            Validacion.ValidarSoloLetras(e);
            ValidarPaginaClienteWizard();
        }

        private void txtcarnetcliente_KeyPress(object sender, KeyPressEventArgs e)
        {
            Validacion.ValidarSoloNumerosConCaracteres(e);
            ValidarPaginaClienteWizard();
         }

        private void txtprovinciacliente_KeyPress(object sender, KeyPressEventArgs e)
        {
            Validacion.ValidarSoloLetras(e);
            ValidarPaginaClienteWizard();
        }

        private void txttelefonocliente_KeyPress(object sender, KeyPressEventArgs e)
        {
            Validacion.ValidarSoloNumeros(e);
            ValidarPaginaClienteWizard();
        
        }

        private void txtcuentabancariacliente_KeyPress(object sender, KeyPressEventArgs e)
        {
            Validacion.ValidarSoloNumerosConCaracteres(e);
            ValidarPaginaClienteWizard();
        }

        private void txtcantejemplares_KeyPress(object sender, KeyPressEventArgs e)
        {
            Validacion.ValidarSoloNumerosConCaracteres(e);
            ValidadPaginaImpresionWizard();
        }

        private void txtcantpag_KeyPress(object sender, KeyPressEventArgs e)
        {
            Validacion.ValidarSoloNumerosConCaracteres(e);
            ValidadPaginaImpresionWizard();
        }

        private void txtdescuentos_KeyPress(object sender, KeyPressEventArgs e)
        {
            Validacion.ValidarSoloNumeros(e);
        }

        private void datefechaentrega_DateTimeChanged(object sender, EventArgs e)
        {
           bool validacion = false;
           validacion = Validacion.ValidarConcordanciaFechas(datefechaexpedicion.DateTime, datefechaentrega.DateTime);
           if (!validacion)
           {
               dxErrorProvider1.SetError(datefechaentrega, "La fecha de entrega del servicio debe ser despues de la fecha de confeccion del pedido.");
               wpgFechaPago.AllowNext = false;
           }
           else
           {
               dxErrorProvider1.SetError(datefechaentrega, string.Empty);
               wpgFechaPago.AllowNext = true;
               pedido.Fecha_entrega = (DateTime)datefechaentrega.DateTime;
           }
             
        }

        private void txtcarnetcliente_TextChanged(object sender, EventArgs e)
        {
            if (txtcarnetcliente.Text.Length != 11)
            {
                dxErrorProvider1.SetError(txtcarnetcliente, "El carnet de identidad tiene 11 digitos");
            }
            if (txtcarnetcliente.Text.Length == 11)
            {
                dxErrorProvider1.SetError(txtcarnetcliente, "");
                ValidarPaginaClienteWizard();
            }
        }

        private void txtcuentabancariacliente_TextChanged(object sender, EventArgs e)
        {
            if (txtcuentabancariacliente.Text.Length < 16)
            {
                dxErrorProvider1.SetError(txtcuentabancariacliente, "La cuenta bancaria tiene de 16 a 20 digitos.");
                wpgCliente.AllowNext = false;
            }
            if (txtcuentabancariacliente.Text.Length == 16)
            {
                dxErrorProvider1.SetError(txtcuentabancariacliente, "");
                ValidarPaginaClienteWizard();
            }
           
        }

        private void cbxprovincia_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void cbxtipodocumento_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void cbxtipoimpresion_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void cbxcolorimpresion_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void cbxtipopapel_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void cbxtipopresilla_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void cbxformapago_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void txtpaginasxhoja_KeyPress(object sender, KeyPressEventArgs e)
        {
            Validacion.ValidarSoloNumerosConCaracteres(e);
        }

        private void wizardNuevoPedido_PrevClick(object sender, DevExpress.XtraWizard.WizardCommandButtonClickEventArgs e)
        {
            wpgCliente.AllowNext = true;
            wpgTipoPedido.AllowNext = true;
            wpgImpresion.AllowNext = true;
            wpgEncuadernado.AllowNext = true;
            wpgFotocopia.AllowNext = true;
            wpgDiseño.AllowNext = true;
            wpgCorte.AllowNext = true;

            if (wizardNuevoPedido.SelectedPage == wpgResumen)
            {
                wizardNuevoPedido.NextText = "Próximo";
            }
            if (wizardNuevoPedido.SelectedPage == wpgFinal)
            {
                XtraMessageBox.Show("Lo sentimos, los datos de este pedido fueron insertados. No se puede deshacer esta operación", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            entidadManager.AdicionarCliente(cliente, pedido);
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            wizardNuevoPedido.SelectedPage = wpgFinal;
            
        }

        private void checkTipoServicio_ItemCheck(object sender, DevExpress.XtraEditors.Controls.ItemCheckEventArgs e)
        {
            if ((((CheckedListBoxItem)checkTipoServicio.GetItem(0)).CheckState == CheckState.Checked)) 
            {
                wpgImpresion.Visible = true;
                wpgTipoPedido.AllowNext = true;
                ((CheckedListBoxItem)checkTipoServicio.GetItem(1)).Enabled = false;
            }
            if ((((CheckedListBoxItem)checkTipoServicio.GetItem(1)).CheckState == CheckState.Checked))
            {
                wpgFotocopia.Visible = true;
                wpgTipoPedido.AllowNext = true;
                ((CheckedListBoxItem)checkTipoServicio.GetItem(0)).Enabled = false;
            }
            if ((((CheckedListBoxItem)checkTipoServicio.GetItem(2)).CheckState == CheckState.Checked))
            {
                if (wpgImpresion.Visible == true || wpgFotocopia.Visible == true)
                {
                    wpgTipoPedido.AllowNext = true;
                    ((CheckedListBoxItem)checkTipoServicio.GetItem(3)).Enabled = false;
                }
                else
                {
                    XtraMessageBox.Show("lo sentimos, debe elegir primero un servicio de Impresión o de Fotocopia", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    ((CheckedListBoxItem)checkTipoServicio.GetItem(2)).CheckState = CheckState.Unchecked;
                }
            }
            if ((((CheckedListBoxItem)checkTipoServicio.GetItem(3)).CheckState == CheckState.Checked))
            {
                wpgEncuadernado.Visible = true;
                wpgTipoPedido.AllowNext = true;
                ((CheckedListBoxItem)checkTipoServicio.GetItem(2)).Enabled = false;
            }
            if ((((CheckedListBoxItem)checkTipoServicio.GetItem(5)).CheckState == CheckState.Checked))
            {
                wpgDiseño.Visible = true;
                wpgTipoPedido.AllowNext = true;
            }

            if ((((CheckedListBoxItem)checkTipoServicio.GetItem(4)).CheckState == CheckState.Checked))
            {
                wpgCorte.Visible = true;
                wpgTipoPedido.AllowNext = true;
            }

            if ((((CheckedListBoxItem)checkTipoServicio.GetItem(6)).CheckState == CheckState.Checked))
            {
                wpgMecanografia.Visible = true;
                wpgTipoPedido.AllowNext = true;
            }

            if ((((CheckedListBoxItem)checkTipoServicio.GetItem(0)).CheckState == CheckState.Unchecked &&
                ((CheckedListBoxItem)checkTipoServicio.GetItem(1)).CheckState == CheckState.Unchecked &&
                ((CheckedListBoxItem)checkTipoServicio.GetItem(2)).CheckState == CheckState.Unchecked &&
                ((CheckedListBoxItem)checkTipoServicio.GetItem(3)).CheckState == CheckState.Unchecked) &&
                ((CheckedListBoxItem)checkTipoServicio.GetItem(4)).CheckState == CheckState.Unchecked &&
                ((CheckedListBoxItem)checkTipoServicio.GetItem(5)).CheckState == CheckState.Unchecked &&
                ((CheckedListBoxItem)checkTipoServicio.GetItem(6)).CheckState == CheckState.Unchecked)
                wpgTipoPedido.AllowNext = false;

            if ((((CheckedListBoxItem)checkTipoServicio.GetItem(0)).CheckState == CheckState.Unchecked))
            {
                wpgImpresion.Visible = false;

                txtcantejemplares.Text = ""; 
                txtcantpag.Text = "";
                txtpaginasxhoja.Text = "";
                txtManoObraImpresion.Text = "";
                cbxtipoimpresion.SelectedIndex = 0;
                cbxcolorimpresion.SelectedIndex = 0;
                cbxtipopapel.SelectedIndex = 0;
                cbxTipoToner.SelectedIndex = 0; 

                pedido.ManoObraImpresion = 0.0;
                pedido.Cantidad_Ejemplares = 0; 

                
                ((CheckedListBoxItem)checkTipoServicio.GetItem(1)).Enabled = true;
            }

            if ((((CheckedListBoxItem)checkTipoServicio.GetItem(1)).CheckState == CheckState.Unchecked))
            {
                wpgFotocopia.Visible = false;
                txtPaginasFotocopia.Text = "";
                txtEjemplaresFotocopia.Text = "";
                txtPagxHojaFotocopia.Text = "";
                txtManoObraFotocopia.Text = "";
                pedido.ManoObraFotocopia = 0.0; 
                ((CheckedListBoxItem)checkTipoServicio.GetItem(0)).Enabled = true;
            }

            if ((((CheckedListBoxItem)checkTipoServicio.GetItem(2)).CheckState == CheckState.Unchecked))
            {
                List<Servicio> listtemp = new List<Servicio>();
                foreach (var item in pedido.Servicios)
                {
                    if (item.Material.Contains("Presillado"))
                        listtemp.Add(item);
                }
                foreach (var item in listtemp)
                {
                    pedido.Servicios.Remove(item);
                }
                pedido.ManoObraPresillado = 0.0;
                ((CheckedListBoxItem)checkTipoServicio.GetItem(3)).Enabled = true;
            }

            if ((((CheckedListBoxItem)checkTipoServicio.GetItem(3)).CheckState == CheckState.Unchecked))
            {
                wpgEncuadernado.Visible = false;
                txtManoObraEncuadernado.Text = "";
                pedido.ManoObraEncuadernado = 0.0; 
                ((CheckedListBoxItem)checkTipoServicio.GetItem(2)).Enabled = true;
            }

            if ((((CheckedListBoxItem)checkTipoServicio.GetItem(5)).CheckState == CheckState.Unchecked))
            {
                wpgDiseño.Visible = false;
                txtmanoobradiseño.Text = "";

                foreach (var item in checkDiseño.SelectedItems)
                {
                    ((CheckedListBoxItem)item).CheckState = CheckState.Unchecked;
                }


                List<Servicio> listtemp = new List<Servicio>();
                foreach (var item in pedido.Servicios)
                {
                    if (item.Material.Contains("Diseño"))
                        listtemp.Add(item); 
                }
                foreach (var item in listtemp)
                {
                    pedido.Servicios.Remove(item);
                }

                pedido.ManoObraDisenno = 0.0;
                     
            }
            if ((((CheckedListBoxItem)checkTipoServicio.GetItem(6)).CheckState == CheckState.Unchecked))
            {
                wpgMecanografia.Visible = false;
                txtcantidadhojasmecanografia.Text = "";
                foreach (var item in checkMecanografia.SelectedItems)
                {
                    ((CheckedListBoxItem)item).CheckState = CheckState.Unchecked;
                }

                List<Servicio> listtemp = new List<Servicio>();
                foreach (var item in pedido.Servicios)
                {
                    if (item.Material.Contains("Mecanografia"))
                        listtemp.Add(item);
                }
                foreach (var item in listtemp)
                {
                    pedido.Servicios.Remove(item);
                }
                pedido.Cantidad_Hojas_Mecanografia = 0;
            }

            if ((((CheckedListBoxItem)checkTipoServicio.GetItem(4)).CheckState == CheckState.Unchecked))
            {
                wpgCorte.Visible = false;
                txtmanoobradiseño.Text = ""; 
                pedido.ManoObraCorte = 0.0;
            }
        }

        private void checkDiseño_ItemCheck(object sender, DevExpress.XtraEditors.Controls.ItemCheckEventArgs e)
        {
            ValidandoPaginaDiseño();
            if ((((CheckedListBoxItem)checkDiseño.GetItem(0)).CheckState == CheckState.Checked))
            {
                ((CheckedListBoxItem)checkDiseño.GetItem(1)).Enabled = false;
                ((CheckedListBoxItem)checkDiseño.GetItem(2)).Enabled = false;
                ((CheckedListBoxItem)checkDiseño.GetItem(3)).Enabled = false;
                ((CheckedListBoxItem)checkDiseño.GetItem(4)).Enabled = false;
                ((CheckedListBoxItem)checkDiseño.GetItem(5)).Enabled = false;
                
            }
            if ((((CheckedListBoxItem)checkDiseño.GetItem(1)).CheckState == CheckState.Checked))
            {
                ((CheckedListBoxItem)checkDiseño.GetItem(0)).Enabled = false;
                ((CheckedListBoxItem)checkDiseño.GetItem(2)).Enabled = false;
                ((CheckedListBoxItem)checkDiseño.GetItem(3)).Enabled = false;
                ((CheckedListBoxItem)checkDiseño.GetItem(4)).Enabled = false;
                ((CheckedListBoxItem)checkDiseño.GetItem(5)).Enabled = false;
                
            }
            if ((((CheckedListBoxItem)checkDiseño.GetItem(2)).CheckState == CheckState.Checked))
            {
                ((CheckedListBoxItem)checkDiseño.GetItem(0)).Enabled = false;
                ((CheckedListBoxItem)checkDiseño.GetItem(1)).Enabled = false;
                ((CheckedListBoxItem)checkDiseño.GetItem(3)).Enabled = false;
                ((CheckedListBoxItem)checkDiseño.GetItem(4)).Enabled = false;
                ((CheckedListBoxItem)checkDiseño.GetItem(5)).Enabled = false;
                
            }
            if ((((CheckedListBoxItem)checkDiseño.GetItem(3)).CheckState == CheckState.Checked))
            {
                ((CheckedListBoxItem)checkDiseño.GetItem(0)).Enabled = false;
                ((CheckedListBoxItem)checkDiseño.GetItem(1)).Enabled = false;
                ((CheckedListBoxItem)checkDiseño.GetItem(2)).Enabled = false;
                ((CheckedListBoxItem)checkDiseño.GetItem(4)).Enabled = false;
                ((CheckedListBoxItem)checkDiseño.GetItem(5)).Enabled = false;
                
            }
            if ((((CheckedListBoxItem)checkDiseño.GetItem(4)).CheckState == CheckState.Checked))
            {
                ((CheckedListBoxItem)checkDiseño.GetItem(0)).Enabled = false;
                ((CheckedListBoxItem)checkDiseño.GetItem(1)).Enabled = false;
                ((CheckedListBoxItem)checkDiseño.GetItem(2)).Enabled = false;
                ((CheckedListBoxItem)checkDiseño.GetItem(3)).Enabled = false;
                ((CheckedListBoxItem)checkDiseño.GetItem(5)).Enabled = false;
               
            }
            if ((((CheckedListBoxItem)checkDiseño.GetItem(5)).CheckState == CheckState.Checked))
            {
                ((CheckedListBoxItem)checkDiseño.GetItem(0)).Enabled = false;
                ((CheckedListBoxItem)checkDiseño.GetItem(1)).Enabled = false;
                ((CheckedListBoxItem)checkDiseño.GetItem(2)).Enabled = false;
                ((CheckedListBoxItem)checkDiseño.GetItem(4)).Enabled = false;
                ((CheckedListBoxItem)checkDiseño.GetItem(3)).Enabled = false;
               
            }

            if (((CheckedListBoxItem)checkDiseño.GetItem(0)).CheckState == CheckState.Unchecked &&
                ((CheckedListBoxItem)checkDiseño.GetItem(1)).CheckState == CheckState.Unchecked &&
                ((CheckedListBoxItem)checkDiseño.GetItem(2)).CheckState == CheckState.Unchecked &&
                ((CheckedListBoxItem)checkDiseño.GetItem(3)).CheckState == CheckState.Unchecked &&
                ((CheckedListBoxItem)checkDiseño.GetItem(4)).CheckState == CheckState.Unchecked &&
                ((CheckedListBoxItem)checkDiseño.GetItem(5)).CheckState == CheckState.Unchecked)
            {
                ((CheckedListBoxItem)checkDiseño.GetItem(0)).Enabled = true;
                ((CheckedListBoxItem)checkDiseño.GetItem(1)).Enabled = true;
                ((CheckedListBoxItem)checkDiseño.GetItem(2)).Enabled = true;
                ((CheckedListBoxItem)checkDiseño.GetItem(3)).Enabled = true;
                ((CheckedListBoxItem)checkDiseño.GetItem(4)).Enabled = true;
                ((CheckedListBoxItem)checkDiseño.GetItem(5)).Enabled = true;
            }
            if (((CheckedListBoxItem)checkDiseño.GetItem(0)).CheckState == CheckState.Unchecked &&
                ((CheckedListBoxItem)checkDiseño.GetItem(1)).CheckState == CheckState.Unchecked &&
                ((CheckedListBoxItem)checkDiseño.GetItem(2)).CheckState == CheckState.Unchecked &&
                ((CheckedListBoxItem)checkDiseño.GetItem(3)).CheckState == CheckState.Unchecked &&
                ((CheckedListBoxItem)checkDiseño.GetItem(4)).CheckState == CheckState.Unchecked &&
                ((CheckedListBoxItem)checkDiseño.GetItem(5)).CheckState == CheckState.Unchecked)
                wpgDiseño.AllowNext = false;
        }

        private void txtManoObraImpresion_EditValueChanged(object sender, EventArgs e)
        {
            ValidadPaginaImpresionWizard();
        }

        private void txtEjemplaresFotocopia_EditValueChanged(object sender, EventArgs e)
        {
            ValidadPaginaFotocopiaWizard();
        }

        private void txtPaginasFotocopia_EditValueChanged(object sender, EventArgs e)
        {
            ValidadPaginaFotocopiaWizard();
        }

        private void txtManoObraFotocopia_EditValueChanged(object sender, EventArgs e)
        {
            ValidadPaginaFotocopiaWizard();
        }

        private void txtManoObraEncuadernado_EditValueChanged(object sender, EventArgs e)
        {
            ValidadPaginaEncuadernadoWizard();
        }

        private void cbxEmpresa_SelectionChangeCommitted(object sender, EventArgs e)
        {
            LleanarDatosCliente();
        }

        private void LleanarDatosCliente()
        {
            Cliente tempCliente = cbxEmpresa.SelectedItem as Cliente;
            if (tempCliente != null)
            {
                txtnombrecliente.Enabled = true;
                txtnombrecliente.Text = tempCliente.Nombre_representante;

                txtcarnetcliente.Enabled = true;
                txtcarnetcliente.Text = tempCliente.Carnet_representante;

                txtcuentabancariacliente.Enabled = true;
                txtcuentabancariacliente.Text = tempCliente.Cuenta_Bancaria;

                txtdireccioncliente.Enabled = true;
                txtdireccioncliente.Text = tempCliente.Direccion;

                txttelefonocliente.Enabled = true;
                txttelefonocliente.Text = tempCliente.Telefono;

                cbxprovincia.Enabled = true;
                cbxprovincia.Text = tempCliente.Provincia;

                ValidarPaginaClienteWizard();
            }
        }

        private void cbxEmpresa_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == (char)Keys.Return)
            {
                LleanarDatosCliente();
            }
        }

        private void Wizard_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (pedidoInsertado != null)
                pedidoInsertado(sender, null);
        }

        private void datefechaexpedicion_EditValueChanged(object sender, EventArgs e)
        {
            pedido.Fecha_expedicion = (DateTime)datefechaexpedicion.EditValue; 
        }

        private void datefechaentrega_EditValueChanged(object sender, EventArgs e)
        {
            pedido.Fecha_expedicion = (DateTime)datefechaentrega.EditValue;
        }

        private void txtmanoobraCorte_KeyPress(object sender, KeyPressEventArgs e)
        {
            Validacion.ValidarSoloNumerosConCaracteres(e);
            ValidarPaginaCorteWizard();
        }

        private void checkMecanografia_ItemCheck(object sender, DevExpress.XtraEditors.Controls.ItemCheckEventArgs e)
        {
            if ((((CheckedListBoxItem)checkMecanografia.GetItem(0)).CheckState == CheckState.Checked))
            {
                ((CheckedListBoxItem)checkMecanografia.GetItem(1)).Enabled = false;
            }
            if ((((CheckedListBoxItem)checkMecanografia.GetItem(1)).CheckState == CheckState.Checked))
            {
                ((CheckedListBoxItem)checkMecanografia.GetItem(0)).Enabled = false;  
            }


            if (((CheckedListBoxItem)checkMecanografia.GetItem(0)).CheckState == CheckState.Unchecked &&
              ((CheckedListBoxItem)checkMecanografia.GetItem(1)).CheckState == CheckState.Unchecked)
            {
                ((CheckedListBoxItem)checkMecanografia.GetItem(0)).Enabled = true;
                ((CheckedListBoxItem)checkMecanografia.GetItem(1)).Enabled = true;
                wpgMecanografia.AllowNext = false;
            }
            ValidarPaginaMecanografiaWizard();
        }

        private void txtManoObraImpresion_KeyPress(object sender, KeyPressEventArgs e)
        {
            Validacion.ValidarSoloNumerosConCaracteres(e);
        }

        private void txtManoObraFotocopia_KeyPress(object sender, KeyPressEventArgs e)
        {
            Validacion.ValidarSoloNumerosConCaracteres(e);
        }

        private void txtManoObraEncuadernado_KeyPress(object sender, KeyPressEventArgs e)
        {
            Validacion.ValidarSoloNumerosConCaracteres(e);
        }

        private void txtcantidadhojasmecanografia_KeyPress(object sender, KeyPressEventArgs e)
        {
            Validacion.ValidarSoloNumerosConCaracteres(e);
            ValidarPaginaMecanografiaWizard();
        }

        private void txtmanoobradiseño_KeyPress(object sender, KeyPressEventArgs e)
        {
            Validacion.ValidarSoloNumerosConCaracteres(e);
            ValidandoPaginaDiseño();
        }

        private void txtValorAgregado_KeyUp(object sender, KeyEventArgs e)
        {
            lblInfDesc.Text = "Hasta el momento el cliente tiene un importe de: " + CalcularImporteTotal().ToString() + ".00 CUP .ESpecifique si desea realizarle algún descuento.";
        }

        public double costopapel { get; set; }

        public double costotonel { get; set; }

        public double costomecanografia { get; set; }

        private void txtcantidadhojasmecanografia_TextChanged(object sender, EventArgs e)
        {
            ValidacionMayorCero(txtcantidadhojasmecanografia, wpgMecanografia);
        }

        private void ValidacionMayorCero(TextEdit tx, DevExpress.XtraWizard.WizardPage wp)
        {
            if (tx.Text == "0")
            {
                dxErrorProvider1.SetError(tx, "Debe introducir un valor mayor que cero.");
                wp.AllowNext = false;
            }
            if (tx.Text != "0")
            {
                dxErrorProvider1.SetError(tx, "");
            }
        }
        
        private void txtcantejemplares_TextChanged(object sender, EventArgs e)
        {
            ValidacionMayorCero(txtcantejemplares, wpgImpresion);
        }

        private void txtcantpag_TextChanged(object sender, EventArgs e)
        {
            ValidacionMayorCero(txtcantpag, wpgImpresion);
        }

        private void txtpaginasxhoja_TextChanged(object sender, EventArgs e)
        {
            ValidacionMayorCero(txtpaginasxhoja, wpgImpresion);
        }

        private void txtManoObraImpresion_TextChanged(object sender, EventArgs e)
        {
            ValidacionMayorCero(txtManoObraImpresion, wpgImpresion);
        }

        private void txtEjemplaresFotocopia_TextChanged(object sender, EventArgs e)
        {
            ValidacionMayorCero(txtEjemplaresFotocopia, wpgFotocopia);
        }

        private void txtPaginasFotocopia_TextChanged(object sender, EventArgs e)
        {
            ValidacionMayorCero(txtPaginasFotocopia, wpgFotocopia);
        }

        private void txtPagxHojaFotocopia_TextChanged(object sender, EventArgs e)
        {
            ValidacionMayorCero(txtPagxHojaFotocopia, wpgFotocopia);
        }

        private void txtManoObraFotocopia_TextChanged(object sender, EventArgs e)
        {
            ValidacionMayorCero(txtManoObraFotocopia, wpgFotocopia);
        }

        private void txtManoObraEncuadernado_TextChanged(object sender, EventArgs e)
        {
            ValidacionMayorCero(txtManoObraEncuadernado, wpgEncuadernado);
        }

        private void txtmanoobraCorte_TextChanged(object sender, EventArgs e)
        {
            ValidacionMayorCero(txtmanoobraCorte, wpgCorte);
        }

        private void txtmanoobradiseño_TextChanged(object sender, EventArgs e)
        {
            ValidacionMayorCero(txtmanoobradiseño, wpgDiseño);
        }

        private void txtValorAgregado_TextChanged(object sender, EventArgs e)
        {
            ValidacionMayorCero(txtValorAgregado, wpgFechaPago);
        }

        private void txtdescuentos_TextChanged(object sender, EventArgs e)
        {
            ValidacionMayorCero(txtdescuentos, wpgFechaPago);
        }


    }
}