using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.Threading;

namespace EdicionesLUZ.Visual
{
    public partial class PantallaInicio : DevExpress.XtraEditors.XtraForm
    {
        
        public PantallaInicio()
        {
            InitializeComponent();
            backgroundWorker1.RunWorkerAsync();
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            Thread.Sleep(3000);

            this.Invoke((MethodInvoker)(() => setMessage("Conectando a la bases de datos..")));

            Thread.Sleep(3000);

            this.Invoke((MethodInvoker)(() => setMessage("Cargando archivos de configuración...")));

            Thread.Sleep(3000);

            this.Invoke((MethodInvoker)(() => setMessage("Iniciando la aplicación...")));

            Thread.Sleep(3000);

            if (this.InvokeRequired) this.Invoke(new Action(finishProcess));
            
        }

        private void finishProcess()
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;

            this.Close();
        }


        public void setMessage(string msg)
        {
            lbloPantallaInicio.Text = msg;
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            
        }

    }
}