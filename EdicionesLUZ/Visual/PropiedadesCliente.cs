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
    public partial class PropiedadesCliente : DevExpress.XtraEditors.XtraForm
    {
        public PropiedadesCliente(Cliente cliente)
        {
            InitializeComponent();
            this.cliente = cliente;
            LlenarPropiedades(cliente);
        }

        private void LlenarPropiedades(Cliente cliente)
        {
            lblPedidoId.Text = cliente.Id.ToString();

            direccion.Text = cliente.Direccion;
            nombre.Text = cliente.Nombre_representante;
            carnet.Text = cliente.Carnet_representante;
            cuentabancaria.Text = cliente.Cuenta_Bancaria;
            telefono.Text = cliente.Telefono;
            lblServiciosSolicitados.Text = cliente.Pedidos.Count.ToString();
        }
        public Cliente cliente { get; set; }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}