namespace DataProvider.Components.PropertyGrid
{
    partial class AdvancedPropertyGrid
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
            this.lblInfo = new System.Windows.Forms.Label();
            this.flowBody = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // lblInfo
            // 
            this.lblInfo.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblInfo.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblInfo.Location = new System.Drawing.Point(0, 195);
            this.lblInfo.Name = "lblInfo";
            this.lblInfo.Size = new System.Drawing.Size(415, 60);
            this.lblInfo.TabIndex = 0;
            // 
            // flowBody
            // 
            this.flowBody.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowBody.Location = new System.Drawing.Point(0, 0);
            this.flowBody.Name = "flowBody";
            this.flowBody.Size = new System.Drawing.Size(415, 195);
            this.flowBody.TabIndex = 1;
            // 
            // AdvandcedPropertyGrid
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.flowBody);
            this.Controls.Add(this.lblInfo);
            this.Name = "AdvandcedPropertyGrid";
            this.Size = new System.Drawing.Size(415, 255);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblInfo;
        private System.Windows.Forms.Panel flowBody;
    }
}
