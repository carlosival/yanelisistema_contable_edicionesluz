namespace EdicionesLUZ.Visual
{
    partial class PantallaInicio
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.marqueeProgressBarControl1 = new DevExpress.XtraEditors.MarqueeProgressBarControl();
            this.lbloPantallaInicio = new DevExpress.XtraEditors.LabelControl();
            this.pictureEdit1 = new DevExpress.XtraEditors.PictureEdit();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.entidadTableAdapter1 = new EdicionesLUZ.DataSetFacturacionTableAdapters.entidadTableAdapter();
            this.dataSetFacturacion1 = new EdicionesLUZ.DataSetFacturacion();
            this.clienteTableAdapter1 = new EdicionesLUZ.DataSetFacturacionTableAdapters.clienteTableAdapter();
            this.ficha_costosTableAdapter1 = new EdicionesLUZ.DataSetFacturacionTableAdapters.ficha_costosTableAdapter();
            this.pedidos_costosTableAdapter1 = new EdicionesLUZ.DataSetFacturacionTableAdapters.pedidos_costosTableAdapter();
            this.pedidoTableAdapter1 = new EdicionesLUZ.DataSetFacturacionTableAdapters.pedidoTableAdapter();
            ((System.ComponentModel.ISupportInitialize)(this.marqueeProgressBarControl1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureEdit1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataSetFacturacion1)).BeginInit();
            this.SuspendLayout();
            // 
            // marqueeProgressBarControl1
            // 
            this.marqueeProgressBarControl1.EditValue = 0;
            this.marqueeProgressBarControl1.Location = new System.Drawing.Point(12, 296);
            this.marqueeProgressBarControl1.Name = "marqueeProgressBarControl1";
            this.marqueeProgressBarControl1.Size = new System.Drawing.Size(455, 14);
            this.marqueeProgressBarControl1.TabIndex = 0;
            // 
            // lbloPantallaInicio
            // 
            this.lbloPantallaInicio.Location = new System.Drawing.Point(12, 277);
            this.lbloPantallaInicio.Name = "lbloPantallaInicio";
            this.lbloPantallaInicio.Size = new System.Drawing.Size(55, 13);
            this.lbloPantallaInicio.TabIndex = 4;
            this.lbloPantallaInicio.Text = "Iniciando...";
            // 
            // pictureEdit1
            // 
            this.pictureEdit1.EditValue = global::EdicionesLUZ.Properties.Resources._428093_images_EquipInformatico;
            this.pictureEdit1.Location = new System.Drawing.Point(12, 12);
            this.pictureEdit1.Name = "pictureEdit1";
            this.pictureEdit1.Properties.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Stretch;
            this.pictureEdit1.Size = new System.Drawing.Size(455, 210);
            this.pictureEdit1.TabIndex = 5;
            // 
            // labelControl1
            // 
            this.labelControl1.Appearance.Font = new System.Drawing.Font("Times New Roman", 10F, System.Drawing.FontStyle.Bold);
            this.labelControl1.Location = new System.Drawing.Point(282, 327);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(185, 15);
            this.labelControl1.TabIndex = 6;
            this.labelControl1.Text = "Diseño y Soluciones Tecnológicas";
            // 
            // labelControl2
            // 
            this.labelControl2.Appearance.Font = new System.Drawing.Font("Times New Roman", 14F, System.Drawing.FontStyle.Bold);
            this.labelControl2.Location = new System.Drawing.Point(84, 228);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(311, 22);
            this.labelControl2.TabIndex = 7;
            this.labelControl2.Text = "Sistema de Facturación Ediciones Luz";
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            this.backgroundWorker1.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
            // 
            // entidadTableAdapter1
            // 
            this.entidadTableAdapter1.ClearBeforeFill = true;
            // 
            // dataSetFacturacion1
            // 
            this.dataSetFacturacion1.DataSetName = "DataSetFacturacion";
            this.dataSetFacturacion1.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // clienteTableAdapter1
            // 
            this.clienteTableAdapter1.ClearBeforeFill = true;
            // 
            // ficha_costosTableAdapter1
            // 
            this.ficha_costosTableAdapter1.ClearBeforeFill = true;
            // 
            // pedidos_costosTableAdapter1
            // 
            this.pedidos_costosTableAdapter1.ClearBeforeFill = true;
            // 
            // pedidoTableAdapter1
            // 
            this.pedidoTableAdapter1.ClearBeforeFill = true;
            // 
            // Pantalla_de__inicio
            // 
            this.Appearance.BackColor = System.Drawing.Color.White;
            this.Appearance.Image = global::EdicionesLUZ.Properties.Resources.banner;
            this.Appearance.Options.UseBackColor = true;
            this.Appearance.Options.UseImage = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(481, 371);
            this.Controls.Add(this.labelControl2);
            this.Controls.Add(this.labelControl1);
            this.Controls.Add(this.pictureEdit1);
            this.Controls.Add(this.lbloPantallaInicio);
            this.Controls.Add(this.marqueeProgressBarControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Pantalla_de__inicio";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            ((System.ComponentModel.ISupportInitialize)(this.marqueeProgressBarControl1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureEdit1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataSetFacturacion1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.MarqueeProgressBarControl marqueeProgressBarControl1;
        private DevExpress.XtraEditors.LabelControl lbloPantallaInicio;
        private DevExpress.XtraEditors.PictureEdit pictureEdit1;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private DataSetFacturacionTableAdapters.entidadTableAdapter entidadTableAdapter1;
        private DataSetFacturacion dataSetFacturacion1;
        private DataSetFacturacionTableAdapters.clienteTableAdapter clienteTableAdapter1;
        private DataSetFacturacionTableAdapters.ficha_costosTableAdapter ficha_costosTableAdapter1;
        private DataSetFacturacionTableAdapters.pedidos_costosTableAdapter pedidos_costosTableAdapter1;
        private DataSetFacturacionTableAdapters.pedidoTableAdapter pedidoTableAdapter1;
    }
}