using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraBars;
using EdicionesLUZ.Modelo;
using EdicionesLUZ.Managers;
using EdicionesLUZ.Visual;
using System.Threading;
using DevExpress.XtraEditors;
using EdicionesLUZ.Factura;
using System.IO;

namespace EdicionesLUZ
{
    public partial class SIFOC : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        Wizard Wizard; 
        Wizard wizard
        {
            get 
            {
                    Wizard = new Wizard(entidad.PedidoMasReciente(), clienteTableAdapter, dataSetFacturacion, pedidoTableAdapter, pedidos_costosTableAdapter1, ficha_costosTableAdapter1);
                    Wizard.pedidoInsertado += new EventHandler(wizard_pedidoInsertado);
                    return Wizard;
                
            }
        }
        EntidadManager entidad;

        Pedido SelectedPedido
        {
            get 
            {

                return GetSelectedPedido();
            }
        }

        private Pedido GetSelectedPedido()
        {
            int[] selecteds = gridView2.GetSelectedRows();
            object row = gridView2.GetRow(selecteds[0]);

            string id = ((System.Data.DataRowView)(row)).Row.ItemArray[0].ToString();

            Pedido pedido = entidad.PedidoDadoId(id);


            return pedido;
        }

        Cliente SelectedCliente
        {
            get
            {
                return GetSelectedCliente() ;
            }
        }

        private Cliente GetSelectedCliente()
        {
            int[] selecteds = gridView1.GetSelectedRows();
            object row = gridView1.GetRow(selecteds[0]);

            string id = ((System.Data.DataRowView)(row)).Row.ItemArray[0].ToString();

            Cliente cliente = entidad.ClienteDadoId(id);

            return cliente;
        }

        public SIFOC()
        {
            showSplashScreen();
            InitializeComponent();

            clienteTableAdapter.Fill(dataSetFacturacion.cliente);
            pedidoTableAdapter.Fill(dataSetFacturacion.pedido);
            pedidos_costosTableAdapter1.Fill(dataSetFacturacion.pedidos_costos);
            ficha_costosTableAdapter1.Fill(dataSetFacturacion.ficha_costos);

            entidad = new EntidadManager(clienteTableAdapter, dataSetFacturacion, pedidoTableAdapter, pedidos_costosTableAdapter1, ficha_costosTableAdapter1);
            
            gridView1.GroupPanelText = "Clientes - Pedidos";
            gridControlPedidos.Dock = DockStyle.Fill;
            gridView2.GroupPanelText = "Pedidos";
            
            RefreshDataClientes();
            RefrescarDataPedidos();

            DevExpress.Data.CurrencyDataController.DisableThreadingProblemsDetection = true;
        }


        private void showSplashScreen()
        {
            using (PantallaInicio fsplash = new PantallaInicio())
            {
                if (fsplash.ShowDialog() == System.Windows.Forms.DialogResult.Cancel) this.Close();
            }
        }

        private void RefrescarDataPedidos()
        {
            gridControlPedidos.RefreshDataSource();
            ValidarBotones();
        }

        private void ValidarBotones()
        {
           
            if (gridView2.RowCount > 0)
            {
                btnCancelar.Enabled = true;
                btnPropiedades.Enabled = true;
                btnFacturar.Enabled = true;
            }
        }

        private void btnNuevoServicio_ItemClick(object sender, ItemClickEventArgs e)
        {
            wizard.ShowDialog();
        }

        void wizard_pedidoInsertado(object sender, EventArgs e)
        {
            RefrescarDataPedidos();
        }

        private void btnFacturar_ItemClick(object sender, ItemClickEventArgs e)
        {
            if(gridView2.RowCount > 0)
            {
            CrearFactura();
            }
        }

        public void CrearFactura()
        {
            string ruta = Application.StartupPath;
            Pedido pedido = SelectedPedido;
            Cliente cliente = entidad.ClienteDadoIdPedido(pedido.Id);
            pedido.Servicios = entidad.ServiciosDadoIdPedido(pedido.Id);
            List<VServicios> vservicios = wizard.Llenarlistasvservicios(pedido);
             
            //Se crea la carpeta que contiene las facturas
            CrearDirectorioFacturas();

            //las facturas con el mismo id se sobreescriben
            FacturaModelo factura = new FacturaModelo(pedido, cliente, vservicios);
            factura.CreateDocument(false);
            factura.ExportToPdf("D:/Sistema Contable Ediciones Luz/Facturas/Factura "+pedido.Id.ToString()+".pdf");
            factura.ShowPreviewDialog();
    
            //printControl1.PrintingSystem = factura.PrintingSystem;
            //printControl1.Dock = DockStyle.Fill;
            //printControl1.Visible = true;

            //VFactura vfactura = new VFactura();
            //vfactura.Pedido = pedido;
            //vfactura.Cliente = cliente;
            //vfactura.VServicio = vservicios;
            //vfactura.ShowDialog();
        }

        void RefreshDataClientes()
        {
            gridControlClientes.RefreshDataSource();
        }
        private void btnrefrescar_ItemClick(object sender, ItemClickEventArgs e)
        {
            RefrescarDataPedidos();
        }

        private void btnCancelar_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (gridView2.RowCount > 0)
            {
                if (XtraMessageBox.Show("Usted está seguro que desea eliminar el pedido seleccionado ?", "Eliminar Pedido", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                {
                    entidad.EliminarPedido(SelectedPedido);
                    RefrescarDataPedidos();
                }
            }
        } 

        private void ribbon_SelectedPageChanged(object sender, EventArgs e)
        {
            if (ribbonControl.SelectedPage == rpCliente)
            {
                gridControlPedidos.Visible = false;
                gridControlFichaCostos.Visible = false; 
                gridControlClientes.Visible = true;
                gridControlClientes.Dock = DockStyle.Fill;
                RefreshDataClientes();
            }
            if (ribbonControl.SelectedPage == rpInicio)
            {
                gridControlPedidos.Visible = true;
                gridControlFichaCostos.Visible = false; 

                gridControlClientes.Visible = false;
                gridControlPedidos.Dock = DockStyle.Fill;
                RefrescarDataPedidos();
            }
        }


        private void btnPropiedades_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (gridView1.RowCount > 0)
            {
                PropiedadesPedido propiedaes = new PropiedadesPedido(SelectedPedido, entidad.ClienteDadoIdPedido(SelectedPedido.Id));
                propiedaes.ShowDialog();
            }

        }

        private void SIFOC_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'dataSetFacturacion.cliente' table. You can move, or remove it, as needed.
            this.clienteTableAdapter.Fill(this.dataSetFacturacion.cliente);
            // TODO: This line of code loads data into the 'dataSetFacturacion.pedido' table. You can move, or remove it, as needed.
            this.pedidoTableAdapter.Fill(this.dataSetFacturacion.pedido);
        }

        private void NuevoPedido_Click(object sender, EventArgs e)
        {
            wizard.Show();
        }

        private void Facturar_Click(object sender, EventArgs e)
        {
            if (gridView2.RowCount > 0)
            {
                CrearFactura();
            }
        }

        private void Eliminar_Click(object sender, EventArgs e)
        {
            if (gridView2.RowCount > 0)
            {
                entidad.EliminarPedido(SelectedPedido);
                RefrescarDataPedidos();
            }
        }

        private void Refrescar_Click(object sender, EventArgs e)
        {
            RefrescarDataPedidos();
        }

        private void Propiedades_Click(object sender, EventArgs e)
        {
            if (gridView2.RowCount > 0)
            {
                PropiedadesPedido propiedaes = new PropiedadesPedido(SelectedPedido, entidad.ClienteDadoIdPedido(SelectedPedido.Id));
                propiedaes.ShowDialog();
            }
        }

        private void btnPropEntidad_ItemClick(object sender, ItemClickEventArgs e)
        {
            PropiedadesEntidad prop = new PropiedadesEntidad();
            prop.ShowDialog();
        }

        private void btnPropCliente_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (gridView1.RowCount > 0)
            {
                PropiedadesCliente propiedaes = new PropiedadesCliente(SelectedCliente);
                propiedaes.ShowDialog();
            }
        }

        private void refrescarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RefreshDataClientes();
        }

        private void propiedadesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (gridView1.RowCount > 0)
            {
                PropiedadesCliente propiedaes = new PropiedadesCliente(SelectedCliente);
                propiedaes.ShowDialog();
            }
        }

        private void eliminarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (gridView1.RowCount > 0)
            {
                entidad.EliminarCliente(SelectedCliente);
                RefreshDataClientes();
            }
        }

        private void btnEliminarCliente_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (gridView1.RowCount > 0){
                if (XtraMessageBox.Show("Usted está seguro que desea eliminar el cliente seleccionado ?. Al eliminar un cliente se eliminarán todos los pedidos asociados al mismo. ", "Eliminar Cliente", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                {
                    entidad.EliminarCliente(SelectedCliente);
                    RefreshDataClientes();
                }
            }
        }

        private void btnRefrescarFichaCostos_ItemClick(object sender, ItemClickEventArgs e)
        {
            gridControlFichaCostos.Visible = true;
            gridControlClientes.Visible = false;
            gridControlPedidos.Visible = false;

            gridControlFichaCostos.Dock = DockStyle.Fill;

            gridControlFichaCostos.RefreshDataSource();
        }

        private void btnVerClientes_ItemClick(object sender, ItemClickEventArgs e)
        {
            gridControlFichaCostos.Visible = false;
            gridControlClientes.Visible = true;
            gridControlPedidos.Visible = false;

            gridControlClientes.Dock = DockStyle.Fill;

            gridControlClientes.RefreshDataSource();
        }

        private void barButtonItem3_ItemClick(object sender, ItemClickEventArgs e)
        {
            Help.ShowHelp(this, helpProvider1.HelpNamespace, HelpNavigator.Topic, Application.StartupPath);
        }

        private void barButtonItem1_ItemClick(object sender, ItemClickEventArgs e)
        {
            SaveFileDialog save = new SaveFileDialog();
            save.ShowDialog();
        }

        private void btnIrFacturas_ItemClick(object sender, ItemClickEventArgs e)
        {
            CrearDirectorioFacturas();
            System.Diagnostics.Process.Start(@"D:\Sistema Contable Ediciones Luz/Facturas");
        }

        private static void CrearDirectorioFacturas()
        {
            DirectoryInfo dr = new DirectoryInfo("D:/Sistema Contable Ediciones Luz/Facturas");
            if (!dr.Exists)
            {
                dr.Create();
            }
        }

        private void irAFacturasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CrearDirectorioFacturas();
            System.Diagnostics.Process.Start(@"D:\Sistema Contable Ediciones Luz/Facturas");
        }

      
    }
}