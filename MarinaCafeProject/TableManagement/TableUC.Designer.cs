namespace MarinaCafeProject
{
    partial class TableUC
    {
        /// <summary> 
        ///Gerekli tasarımcı değişkeni.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        ///Kullanılan tüm kaynakları temizleyin.
        /// </summary>
        ///<param name="disposing">yönetilen kaynaklar dispose edilmeliyse doğru; aksi halde yanlış.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Bileşen Tasarımcısı üretimi kod

        /// <summary> 
        /// Tasarımcı desteği için gerekli metot - bu metodun 
        ///içeriğini kod düzenleyici ile değiştirmeyin.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TableUC));
            this.lbl_number = new Bunifu.UI.WinForms.BunifuLabel();
            this.SuspendLayout();
            // 
            // lbl_number
            // 
            this.lbl_number.AllowParentOverrides = false;
            this.lbl_number.AutoEllipsis = false;
            this.lbl_number.AutoSize = false;
            this.lbl_number.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lbl_number.CursorType = System.Windows.Forms.Cursors.Hand;
            this.lbl_number.Enabled = false;
            this.lbl_number.Font = new System.Drawing.Font("NSimSun", 45F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_number.Location = new System.Drawing.Point(33, 14);
            this.lbl_number.Name = "lbl_number";
            this.lbl_number.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lbl_number.Size = new System.Drawing.Size(104, 61);
            this.lbl_number.TabIndex = 0;
            this.lbl_number.Text = "1";
            this.lbl_number.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.lbl_number.TextFormat = Bunifu.UI.WinForms.BunifuLabel.TextFormattingOptions.Default;
            // 
            // TableUC
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.BackgroundImage = global::MarinaCafeProject.Properties.Resources.table_full;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.Controls.Add(this.lbl_number);
            this.Cursor = System.Windows.Forms.Cursors.Hand;
            this.DoubleBuffered = true;
            this.Name = "TableUC";
            this.Size = new System.Drawing.Size(169, 191);
            this.ResumeLayout(false);

        }

        #endregion

        private Bunifu.UI.WinForms.BunifuLabel lbl_number;
    }
}
