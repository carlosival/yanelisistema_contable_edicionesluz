using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using EdicionesLUZ.Modelo;
using EdicionesLUZ.Factura;

namespace EdicionesLUZ.Visual
{
    public partial class VFactura : Form
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

        public VFactura(Pedido Pedido, Cliente Cliente, List<VServicios> VServicio)
        {
            InitializeComponent();
            FacturaModelo factura = new FacturaModelo(Pedido, Cliente, VServicio);
            try
            {
                printControl1.PrintingSystem = factura.PrintingSystem;
                factura.CreateDocument(false);
                factura.ShowPreview();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public VFactura()
        {
            InitializeComponent();
            
        }

        private void VFactura_Load(object sender, EventArgs e)
        {
            try
            {
                FacturaModelo factura = new FacturaModelo(Pedido, Cliente, VServicio);
                printControl1.PrintingSystem = factura.PrintingSystem;
                factura.CreateDocument(false);
            }
            catch (AccessViolationException ex)
            {
                MessageBox.Show("Lo sentimos, en este momento no se puede guardar el documento.", "Informacion", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            //factura.ShowPreview();
        }

        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            SaveFileDialog save = new SaveFileDialog();
            save.ShowDialog();
        }

       
    }
}
