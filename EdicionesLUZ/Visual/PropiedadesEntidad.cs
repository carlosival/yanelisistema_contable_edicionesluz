using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace EdicionesLUZ.Visual
{
    public partial class PropiedadesEntidad : DevExpress.XtraEditors.XtraForm
    {
        //DataSetFacturacion.entidadRow entidad;
         DataSetFacturacion.entidadDataTable   entidad; 

        public PropiedadesEntidad()
        {
            InitializeComponent();
            LlenarDatos();
        }

        private void LlenarDatos()
        {
            entidad = entidadTableAdapter.GetEntidadById(1);

            txtContribuyente.Text = ((DataSetFacturacion.entidadRow)entidad.Rows[0]).no_contribuyente;
            txtCuentaBancaria.Text = ((DataSetFacturacion.entidadRow)entidad.Rows[0]).cuenta_bancaria;
            txtdireccion.Text = ((DataSetFacturacion.entidadRow)entidad.Rows[0]).direccion;
            txtTelefono.Text = ((DataSetFacturacion.entidadRow)entidad.Rows[0]).telefono;
        }

        private void PropiedadesEntidad_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'dataSetFacturacion1.entidad' table. You can move, or remove it, as needed.
            
            this.entidadTableAdapter.Fill(this.dataSetFacturacion1.entidad);
            
            
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            ActualizarDatos();
            this.Close();
        }

        private void ActualizarDatos()
        {
            entidadTableAdapter.Update(((DataSetFacturacion.entidadRow)entidad.Rows[0]).nombre, ((DataSetFacturacion.entidadRow)entidad.Rows[0]).representante, txtdireccion.Text, txtCuentaBancaria.Text,
                                        ((DataSetFacturacion.entidadRow)entidad.Rows[0]).carnet_representante, txtTelefono.Text, txtContribuyente.Text, ((DataSetFacturacion.entidadRow)entidad.Rows[0]).Id,
                                        ((DataSetFacturacion.entidadRow)entidad.Rows[0]).nombre, ((DataSetFacturacion.entidadRow)entidad.Rows[0]).representante,
                                         ((DataSetFacturacion.entidadRow)entidad.Rows[0]).direccion, ((DataSetFacturacion.entidadRow)entidad.Rows[0]).cuenta_bancaria,
                                        ((DataSetFacturacion.entidadRow)entidad.Rows[0]).carnet_representante, ((DataSetFacturacion.entidadRow)entidad.Rows[0]).telefono,
                                        ((DataSetFacturacion.entidadRow)entidad.Rows[0]).no_contribuyente);
            this.entidadTableAdapter.Update(this.dataSetFacturacion1.entidad);
            this.entidadTableAdapter.Fill(this.dataSetFacturacion1.entidad);
            LlenarDatos();
        }

        private void btnAplicar_Click(object sender, EventArgs e)
        {
            ActualizarDatos();
        }

    }
}