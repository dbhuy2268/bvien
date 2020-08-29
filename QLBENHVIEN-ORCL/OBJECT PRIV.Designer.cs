namespace QLBENHVIEN_ORCL
{
    partial class OBJECT_PRIV
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
            this.grv_all_Objects = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.grv_all_Objects)).BeginInit();
            this.SuspendLayout();
            // 
            // grv_all_Objects
            // 
            this.grv_all_Objects.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grv_all_Objects.Location = new System.Drawing.Point(6, 107);
            this.grv_all_Objects.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.grv_all_Objects.Name = "grv_all_Objects";
            this.grv_all_Objects.RowHeadersWidth = 82;
            this.grv_all_Objects.RowTemplate.Height = 33;
            this.grv_all_Objects.Size = new System.Drawing.Size(670, 358);
            this.grv_all_Objects.TabIndex = 0;
            // 
            // OBJECT_PRIV
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(682, 471);
            this.Controls.Add(this.grv_all_Objects);
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "OBJECT_PRIV";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "QUẢN LÝ QUYỀN";
            this.Load += new System.EventHandler(this.OBJECT_PRIV_Load);
            ((System.ComponentModel.ISupportInitialize)(this.grv_all_Objects)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView grv_all_Objects;
    }
}