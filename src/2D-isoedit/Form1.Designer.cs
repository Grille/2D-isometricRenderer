namespace _2Deditor
{
    partial class Form1
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.pBHeightMap = new System.Windows.Forms.PictureBox();
            this.pBTextureMap = new System.Windows.Forms.PictureBox();
            this.pBRender = new System.Windows.Forms.PictureBox();
            this.bExport = new System.Windows.Forms.Button();
            this.bClose = new System.Windows.Forms.Button();
            this.bLoad = new System.Windows.Forms.Button();
            this.bRotL = new System.Windows.Forms.Button();
            this.bRotR = new System.Windows.Forms.Button();
            this.bSave = new System.Windows.Forms.Button();
            this.bNew = new System.Windows.Forms.Button();
            this.bSwitch = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.pBHeightMap)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pBTextureMap)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pBRender)).BeginInit();
            this.SuspendLayout();
            // 
            // pBHeightMap
            // 
            this.pBHeightMap.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(140)))));
            this.pBHeightMap.Location = new System.Drawing.Point(12, 12);
            this.pBHeightMap.Name = "pBHeightMap";
            this.pBHeightMap.Size = new System.Drawing.Size(300, 300);
            this.pBHeightMap.TabIndex = 0;
            this.pBHeightMap.TabStop = false;
            this.pBHeightMap.Paint += new System.Windows.Forms.PaintEventHandler(this.pBHeightMap_Paint);
            this.pBHeightMap.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pBHeightMap_MouseDown);
            this.pBHeightMap.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pBHeightMap_MouseMove);
            this.pBHeightMap.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.pBHeightMap_MouseWheel);
            // 
            // pBTextureMap
            // 
            this.pBTextureMap.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(140)))));
            this.pBTextureMap.Location = new System.Drawing.Point(318, 12);
            this.pBTextureMap.Name = "pBTextureMap";
            this.pBTextureMap.Size = new System.Drawing.Size(300, 300);
            this.pBTextureMap.TabIndex = 1;
            this.pBTextureMap.TabStop = false;
            this.pBTextureMap.Paint += new System.Windows.Forms.PaintEventHandler(this.pBTextureMap_Paint);
            this.pBTextureMap.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pBTextureMap_MouseDown);
            this.pBTextureMap.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pBTextureMap_MouseMove);
            this.pBTextureMap.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.pBTextureMap_MouseWheel);
            // 
            // pBRender
            // 
            this.pBRender.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(140)))));
            this.pBRender.Location = new System.Drawing.Point(624, 12);
            this.pBRender.Name = "pBRender";
            this.pBRender.Size = new System.Drawing.Size(300, 300);
            this.pBRender.TabIndex = 3;
            this.pBRender.TabStop = false;
            this.pBRender.Paint += new System.Windows.Forms.PaintEventHandler(this.pBRender_Paint);
            this.pBRender.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pBRender_MouseDown);
            this.pBRender.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pBRender_MouseMove);
            this.pBRender.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.pBRender_MouseWheel);
            // 
            // bExport
            // 
            this.bExport.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.bExport.Enabled = false;
            this.bExport.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bExport.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(255)))));
            this.bExport.Location = new System.Drawing.Point(956, 246);
            this.bExport.Name = "bExport";
            this.bExport.Size = new System.Drawing.Size(100, 30);
            this.bExport.TabIndex = 4;
            this.bExport.Text = "Export";
            this.bExport.UseVisualStyleBackColor = false;
            // 
            // bClose
            // 
            this.bClose.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.bClose.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bClose.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(255)))));
            this.bClose.Location = new System.Drawing.Point(956, 282);
            this.bClose.Name = "bClose";
            this.bClose.Size = new System.Drawing.Size(100, 30);
            this.bClose.TabIndex = 5;
            this.bClose.Text = "Close";
            this.bClose.UseVisualStyleBackColor = false;
            this.bClose.Click += new System.EventHandler(this.bClose_Click);
            // 
            // bLoad
            // 
            this.bLoad.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.bLoad.Enabled = false;
            this.bLoad.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bLoad.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(255)))));
            this.bLoad.Location = new System.Drawing.Point(956, 210);
            this.bLoad.Name = "bLoad";
            this.bLoad.Size = new System.Drawing.Size(100, 30);
            this.bLoad.TabIndex = 6;
            this.bLoad.Text = "Load";
            this.bLoad.UseVisualStyleBackColor = false;
            // 
            // bRotL
            // 
            this.bRotL.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.bRotL.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bRotL.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(255)))));
            this.bRotL.Location = new System.Drawing.Point(956, 48);
            this.bRotL.Name = "bRotL";
            this.bRotL.Size = new System.Drawing.Size(100, 30);
            this.bRotL.TabIndex = 7;
            this.bRotL.Text = "Rotate -45°";
            this.bRotL.UseVisualStyleBackColor = false;
            this.bRotL.Click += new System.EventHandler(this.bRotL_Click);
            // 
            // bRotR
            // 
            this.bRotR.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.bRotR.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bRotR.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(255)))));
            this.bRotR.Location = new System.Drawing.Point(956, 12);
            this.bRotR.Name = "bRotR";
            this.bRotR.Size = new System.Drawing.Size(100, 30);
            this.bRotR.TabIndex = 8;
            this.bRotR.Text = "Rotate +45°";
            this.bRotR.UseVisualStyleBackColor = false;
            this.bRotR.Click += new System.EventHandler(this.bRotR_Click);
            // 
            // bSave
            // 
            this.bSave.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.bSave.Enabled = false;
            this.bSave.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bSave.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(255)))));
            this.bSave.Location = new System.Drawing.Point(956, 174);
            this.bSave.Name = "bSave";
            this.bSave.Size = new System.Drawing.Size(100, 30);
            this.bSave.TabIndex = 10;
            this.bSave.Text = "Save";
            this.bSave.UseVisualStyleBackColor = false;
            // 
            // bNew
            // 
            this.bNew.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.bNew.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bNew.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(255)))));
            this.bNew.Location = new System.Drawing.Point(956, 138);
            this.bNew.Name = "bNew";
            this.bNew.Size = new System.Drawing.Size(100, 30);
            this.bNew.TabIndex = 11;
            this.bNew.Text = "New";
            this.bNew.UseVisualStyleBackColor = false;
            this.bNew.Click += new System.EventHandler(this.bNew_Click);
            // 
            // bSwitch
            // 
            this.bSwitch.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.bSwitch.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bSwitch.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(255)))));
            this.bSwitch.Location = new System.Drawing.Point(956, 84);
            this.bSwitch.Name = "bSwitch";
            this.bSwitch.Size = new System.Drawing.Size(100, 30);
            this.bSwitch.TabIndex = 12;
            this.bSwitch.Text = "Switch mode";
            this.bSwitch.UseVisualStyleBackColor = false;
            this.bSwitch.Click += new System.EventHandler(this.bSwitch_Click);
            // 
            // timer1
            // 
            this.timer1.Interval = 1;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(44)))));
            this.ClientSize = new System.Drawing.Size(1064, 324);
            this.Controls.Add(this.bSwitch);
            this.Controls.Add(this.bNew);
            this.Controls.Add(this.bSave);
            this.Controls.Add(this.bRotR);
            this.Controls.Add(this.bRotL);
            this.Controls.Add(this.bLoad);
            this.Controls.Add(this.bClose);
            this.Controls.Add(this.bExport);
            this.Controls.Add(this.pBRender);
            this.Controls.Add(this.pBTextureMap);
            this.Controls.Add(this.pBHeightMap);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.Text = "2D isoedit";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pBHeightMap)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pBTextureMap)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pBRender)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pBHeightMap;
        private System.Windows.Forms.PictureBox pBTextureMap;
        private System.Windows.Forms.PictureBox pBRender;
        private System.Windows.Forms.Button bExport;
        private System.Windows.Forms.Button bClose;
        private System.Windows.Forms.Button bLoad;
        private System.Windows.Forms.Button bRotL;
        private System.Windows.Forms.Button bRotR;
        private System.Windows.Forms.Button bSave;
        private System.Windows.Forms.Button bNew;
        private System.Windows.Forms.Button bSwitch;
        private System.Windows.Forms.Timer timer1;
    }
}

