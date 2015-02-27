namespace Starehe.Views
{
    partial class Test
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Test));
            this.adobePDFViwer = new AxAcroPDFLib.AxAcroPDF();
            ((System.ComponentModel.ISupportInitialize)(this.adobePDFViwer)).BeginInit();
            this.SuspendLayout();
            // 
            // adobePDFViwer
            // 
            this.adobePDFViwer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.adobePDFViwer.Enabled = true;
            this.adobePDFViwer.Location = new System.Drawing.Point(0, 0);
            this.adobePDFViwer.Name = "adobePDFViwer";
            this.adobePDFViwer.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("adobePDFViwer.OcxState")));
            this.adobePDFViwer.Size = new System.Drawing.Size(150, 150);
            this.adobePDFViwer.TabIndex = 0;
            // 
            // Test
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.adobePDFViwer);
            this.Name = "Test";
            ((System.ComponentModel.ISupportInitialize)(this.adobePDFViwer)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private AxAcroPDFLib.AxAcroPDF adobePDFViwer;
    }
}
