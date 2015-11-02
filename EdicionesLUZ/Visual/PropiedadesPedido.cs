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
    public partial class PropiedadesPedido : DevExpress.XtraEditors.XtraForm
    {
        private Modelo.Pedido SelectedPedido;
        Cliente cliente;

        public PropiedadesPedido()
        {
            InitializeComponent();
        }

        public PropiedadesPedido(Modelo.Pedido SelectedPedido, Cliente cliente)
        {
            InitializeComponent();
            this.SelectedPedido = SelectedPedido;
            this.cliente = cliente;

            LlenarPropiedades(SelectedPedido, cliente);
        }

        private void LlenarPropiedades(Modelo.Pedido SelectedPedido, Cliente cliente)
        {
            lblPedidoId.Text = SelectedPedido.Id.ToString();

            direccion.Text = cliente.Direccion;
            nombre.Text = cliente.Nombre_representante;
            carnet.Text = cliente.Carnet_representante;
            cuentabancaria.Text = cliente.Cuenta_Bancaria;
            telefono.Text = cliente.Telefono;


            if (SelectedPedido.Tipo_impresion == "")
                SelectedPedido.Tipo_impresion = "-";

            if (SelectedPedido.Color_impresion == "")
                SelectedPedido.Color_impresion = "-";  


            tipodoc.Text = SelectedPedido.Tipo_documento;
            fechentrega.Text = SelectedPedido.Fecha_entrega.ToString();
            fechexpedicion.Text = SelectedPedido.Fecha_expedicion.ToString();
            tipoimpresion.Text = SelectedPedido.Tipo_impresion;
            colorimpresion.Text = SelectedPedido.Color_impresion;

            if (SelectedPedido.Cantidad_paginas == 0)
                cantpag.Text = "-";
            else
                cantpag.Text = SelectedPedido.Cantidad_paginas.ToString();

            if (SelectedPedido.Cantidad_Ejemplares == 0)
                cantejemp.Text = "-";
            else
                   cantejemp.Text = SelectedPedido.Cantidad_Ejemplares.ToString();

            formpago.Text = SelectedPedido.Forma_pago;
           
            moimpresion.Text = SelectedPedido.ManoObraImpresion.ToString();
            mofotocopuia.Text = SelectedPedido.ManoObraFotocopia.ToString();
            moencuadernado.Text = SelectedPedido.ManoObraFotocopia.ToString();
            mopresillado.Text = SelectedPedido.ManoObraPresillado.ToString();
            mocorte.Text = SelectedPedido.ManoObraCorte.ToString();
            valoragregado.Text = SelectedPedido.ValorAgregado.ToString();

            pagoadelantado.Text = SelectedPedido.Pago_adelantado.ToString();
            descuentos.Text = SelectedPedido.Descuentos.ToString();
            costototal.Text = SelectedPedido.Coste_total.ToString();
            importetotal.Text = SelectedPedido.Importe_total.ToString();
        }


        private void btnAceptar_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}