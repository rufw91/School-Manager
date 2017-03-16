namespace UmanyiSMS.Controls
{
    partial class PDFViewer
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PDFViewer));
            this.acrobatViewer = new AxAcroPDFLib.AxAcroPDF();
            ((System.ComponentModel.ISupportInitialize)(this.acrobatViewer)).BeginInit();
            this.SuspendLayout();
            // 
            // acrobatViewer
            // 
            this.acrobatViewer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.acrobatViewer.Enabled = true;
            this.acrobatViewer.Location = new System.Drawing.Point(0, 0);
            this.acrobatViewer.Name = "acrobatViewer";
            this.acrobatViewer.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("acrobatViewer.OcxState")));
            this.acrobatViewer.Size = new System.Drawing.Size(150, 150);
            this.acrobatViewer.TabIndex = 0;
            // 
            // PDFViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.acrobatViewer);
            this.Name = "PDFViewer";
            ((System.ComponentModel.ISupportInitialize)(this.acrobatViewer)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private AxAcroPDFLib.AxAcroPDF acrobatViewer;



    }
}
