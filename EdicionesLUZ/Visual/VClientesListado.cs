using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using EdicionesLUZ.Modelo;

namespace EdicionesLUZ.Visual
{
    public partial class VClientesListado : DevExpress.XtraEditors.XtraForm
    {
        private List<Modelo.Cliente> listclientes;
        public event EventHandler closed;

        Cliente SelectedCliente
        {
            get
            {
                return (Cliente)gridView1.GetFocusedRow();
            }
        }

        public VClientesListado()
        {
            InitializeComponent();
        }

        public VClientesListado(List<Modelo.Cliente> listclientes)
        {
            // TODO: Complete member initialization
            InitializeComponent();
            this.listclientes = listclientes;
            gridControl1.DataSource = listclientes;
        }

        private void VClientesListado_Load(object sender, EventArgs e)
        {

        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            if (closed != null)
                closed(SelectedCliente, e);
            Close();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}