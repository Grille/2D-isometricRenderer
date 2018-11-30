namespace program
{
    partial class FormEditor 
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormEditor));
            this.pBResult = new System.Windows.Forms.PictureBox();
            this.bExport = new System.Windows.Forms.Button();
            this.bClose = new System.Windows.Forms.Button();
            this.bLoadMap = new System.Windows.Forms.Button();
            this.bRotL = new System.Windows.Forms.Button();
            this.bRotR = new System.Windows.Forms.Button();
            this.bNew = new System.Windows.Forms.Button();
            this.renderTimer = new System.Windows.Forms.Timer(this.components);
            this.panel1 = new System.Windows.Forms.Panel();
            this.bRot = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.radioButtonShadowLow = new System.Windows.Forms.RadioButton();
            this.radioButtonShadowOf = new System.Windows.Forms.RadioButton();
            this.radioButtonShadowHigh = new System.Windows.Forms.RadioButton();
            this.panel4 = new System.Windows.Forms.Panel();
            this.checkBoxPreAR = new System.Windows.Forms.CheckBox();
            this.radioButtonPreM = new System.Windows.Forms.RadioButton();
            this.radioButtonPreR = new System.Windows.Forms.RadioButton();
            this.label3 = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.bLoadTexture = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pBResult)).BeginInit();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // pBResult
            // 
            this.pBResult.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pBResult.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(25)))), ((int)(((byte)(45)))));
            this.pBResult.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pBResult.Location = new System.Drawing.Point(3, 41);
            this.pBResult.Name = "pBResult";
            this.pBResult.Size = new System.Drawing.Size(632, 448);
            this.pBResult.TabIndex = 3;
            this.pBResult.TabStop = false;
            this.pBResult.Paint += new System.Windows.Forms.PaintEventHandler(this.pBRender_Paint);
            this.pBResult.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pBRender_MouseMove);
            this.pBResult.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.pBRender_MouseWheel);
            // 
            // bExport
            // 
            this.bExport.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(64)))));
            this.bExport.Enabled = false;
            this.bExport.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bExport.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bExport.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(255)))));
            this.bExport.Location = new System.Drawing.Point(323, 5);
            this.bExport.Name = "bExport";
            this.bExport.Size = new System.Drawing.Size(100, 26);
            this.bExport.TabIndex = 4;
            this.bExport.Text = "Export";
            this.bExport.UseVisualStyleBackColor = false;
            // 
            // bClose
            // 
            this.bClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.bClose.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(64)))));
            this.bClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bClose.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bClose.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(255)))));
            this.bClose.Location = new System.Drawing.Point(533, 5);
            this.bClose.Name = "bClose";
            this.bClose.Size = new System.Drawing.Size(100, 26);
            this.bClose.TabIndex = 5;
            this.bClose.Text = "Close";
            this.bClose.UseVisualStyleBackColor = false;
            this.bClose.Click += new System.EventHandler(this.bClose_Click);
            // 
            // bLoadMap
            // 
            this.bLoadMap.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(64)))));
            this.bLoadMap.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bLoadMap.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bLoadMap.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(255)))));
            this.bLoadMap.Location = new System.Drawing.Point(111, 5);
            this.bLoadMap.Name = "bLoadMap";
            this.bLoadMap.Size = new System.Drawing.Size(100, 26);
            this.bLoadMap.TabIndex = 6;
            this.bLoadMap.Text = "Load Map";
            this.bLoadMap.UseVisualStyleBackColor = false;
            this.bLoadMap.Click += new System.EventHandler(this.bLoad_Click);
            // 
            // bRotL
            // 
            this.bRotL.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(64)))));
            this.bRotL.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bRotL.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bRotL.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(255)))));
            this.bRotL.Location = new System.Drawing.Point(366, 6);
            this.bRotL.Name = "bRotL";
            this.bRotL.Size = new System.Drawing.Size(40, 26);
            this.bRotL.TabIndex = 7;
            this.bRotL.Text = "-45°";
            this.bRotL.UseVisualStyleBackColor = false;
            this.bRotL.Click += new System.EventHandler(this.bRotL_Click);
            // 
            // bRotR
            // 
            this.bRotR.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(64)))));
            this.bRotR.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bRotR.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bRotR.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(255)))));
            this.bRotR.Location = new System.Drawing.Point(320, 6);
            this.bRotR.Name = "bRotR";
            this.bRotR.Size = new System.Drawing.Size(40, 26);
            this.bRotR.TabIndex = 8;
            this.bRotR.Text = "+45°";
            this.bRotR.UseVisualStyleBackColor = false;
            this.bRotR.Click += new System.EventHandler(this.bRotR_Click);
            // 
            // bNew
            // 
            this.bNew.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(64)))));
            this.bNew.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bNew.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bNew.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(255)))));
            this.bNew.Location = new System.Drawing.Point(5, 5);
            this.bNew.Name = "bNew";
            this.bNew.Size = new System.Drawing.Size(100, 26);
            this.bNew.TabIndex = 11;
            this.bNew.Text = "New";
            this.bNew.UseVisualStyleBackColor = false;
            this.bNew.Click += new System.EventHandler(this.bNew_Click);
            // 
            // renderTimer
            // 
            this.renderTimer.Interval = 17;
            this.renderTimer.Tick += new System.EventHandler(this.renderTimer_Tick);
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(64)))));
            this.panel1.Controls.Add(this.bRot);
            this.panel1.Controls.Add(this.pBResult);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Controls.Add(this.panel4);
            this.panel1.Controls.Add(this.bRotR);
            this.panel1.Controls.Add(this.bRotL);
            this.panel1.Location = new System.Drawing.Point(3, 44);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(638, 494);
            this.panel1.TabIndex = 15;
            // 
            // bRot
            // 
            this.bRot.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(64)))));
            this.bRot.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bRot.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bRot.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(255)))));
            this.bRot.Location = new System.Drawing.Point(274, 6);
            this.bRot.Name = "bRot";
            this.bRot.Size = new System.Drawing.Size(40, 26);
            this.bRot.TabIndex = 22;
            this.bRot.Text = "=0°";
            this.bRot.UseVisualStyleBackColor = false;
            this.bRot.Click += new System.EventHandler(this.bRot_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.label1);
            this.panel2.Controls.Add(this.radioButtonShadowLow);
            this.panel2.Controls.Add(this.radioButtonShadowOf);
            this.panel2.Controls.Add(this.radioButtonShadowHigh);
            this.panel2.Location = new System.Drawing.Point(409, 0);
            this.panel2.Margin = new System.Windows.Forms.Padding(0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(227, 38);
            this.panel2.TabIndex = 31;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(255)))));
            this.label1.Location = new System.Drawing.Point(17, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(56, 15);
            this.label1.TabIndex = 25;
            this.label1.Text = "shadows:";
            // 
            // radioButtonShadowLow
            // 
            this.radioButtonShadowLow.AutoSize = true;
            this.radioButtonShadowLow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.radioButtonShadowLow.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.radioButtonShadowLow.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(255)))));
            this.radioButtonShadowLow.Location = new System.Drawing.Point(133, 5);
            this.radioButtonShadowLow.Name = "radioButtonShadowLow";
            this.radioButtonShadowLow.Size = new System.Drawing.Size(43, 19);
            this.radioButtonShadowLow.TabIndex = 27;
            this.radioButtonShadowLow.Text = "low";
            this.radioButtonShadowLow.UseVisualStyleBackColor = true;
            this.radioButtonShadowLow.CheckedChanged += new System.EventHandler(this.radioButtonShadowLow_CheckedChanged);
            // 
            // radioButtonShadowOf
            // 
            this.radioButtonShadowOf.AutoSize = true;
            this.radioButtonShadowOf.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.radioButtonShadowOf.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.radioButtonShadowOf.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(255)))));
            this.radioButtonShadowOf.Location = new System.Drawing.Point(183, 6);
            this.radioButtonShadowOf.Name = "radioButtonShadowOf";
            this.radioButtonShadowOf.Size = new System.Drawing.Size(35, 19);
            this.radioButtonShadowOf.TabIndex = 28;
            this.radioButtonShadowOf.Text = "of";
            this.radioButtonShadowOf.UseVisualStyleBackColor = true;
            this.radioButtonShadowOf.CheckedChanged += new System.EventHandler(this.radioButtonShadowOf_CheckedChanged);
            // 
            // radioButtonShadowHigh
            // 
            this.radioButtonShadowHigh.AutoSize = true;
            this.radioButtonShadowHigh.Checked = true;
            this.radioButtonShadowHigh.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.radioButtonShadowHigh.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.radioButtonShadowHigh.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(255)))));
            this.radioButtonShadowHigh.Location = new System.Drawing.Point(79, 5);
            this.radioButtonShadowHigh.Name = "radioButtonShadowHigh";
            this.radioButtonShadowHigh.Size = new System.Drawing.Size(48, 19);
            this.radioButtonShadowHigh.TabIndex = 26;
            this.radioButtonShadowHigh.TabStop = true;
            this.radioButtonShadowHigh.Text = "high";
            this.radioButtonShadowHigh.UseVisualStyleBackColor = true;
            this.radioButtonShadowHigh.CheckedChanged += new System.EventHandler(this.radioButtonShadowHigh_CheckedChanged);
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.checkBoxPreAR);
            this.panel4.Controls.Add(this.radioButtonPreM);
            this.panel4.Controls.Add(this.radioButtonPreR);
            this.panel4.Controls.Add(this.label3);
            this.panel4.Location = new System.Drawing.Point(3, 0);
            this.panel4.Margin = new System.Windows.Forms.Padding(0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(259, 38);
            this.panel4.TabIndex = 30;
            // 
            // checkBoxPreAR
            // 
            this.checkBoxPreAR.AutoSize = true;
            this.checkBoxPreAR.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.checkBoxPreAR.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.checkBoxPreAR.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(255)))));
            this.checkBoxPreAR.Location = new System.Drawing.Point(173, 6);
            this.checkBoxPreAR.Name = "checkBoxPreAR";
            this.checkBoxPreAR.Size = new System.Drawing.Size(81, 19);
            this.checkBoxPreAR.TabIndex = 21;
            this.checkBoxPreAR.Text = "auto rotate";
            this.checkBoxPreAR.UseVisualStyleBackColor = true;
            // 
            // radioButtonPreM
            // 
            this.radioButtonPreM.AutoSize = true;
            this.radioButtonPreM.Checked = true;
            this.radioButtonPreM.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.radioButtonPreM.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.radioButtonPreM.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(255)))));
            this.radioButtonPreM.Location = new System.Drawing.Point(55, 6);
            this.radioButtonPreM.Name = "radioButtonPreM";
            this.radioButtonPreM.Size = new System.Drawing.Size(54, 19);
            this.radioButtonPreM.TabIndex = 18;
            this.radioButtonPreM.TabStop = true;
            this.radioButtonPreM.Text = "move";
            this.radioButtonPreM.UseVisualStyleBackColor = true;
            // 
            // radioButtonPreR
            // 
            this.radioButtonPreR.AutoSize = true;
            this.radioButtonPreR.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.radioButtonPreR.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.radioButtonPreR.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(255)))));
            this.radioButtonPreR.Location = new System.Drawing.Point(112, 6);
            this.radioButtonPreR.Name = "radioButtonPreR";
            this.radioButtonPreR.Size = new System.Drawing.Size(55, 19);
            this.radioButtonPreR.TabIndex = 19;
            this.radioButtonPreR.Text = "rotate";
            this.radioButtonPreR.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.label3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(255)))));
            this.label3.Location = new System.Drawing.Point(13, 8);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(36, 15);
            this.label3.TabIndex = 25;
            this.label3.Text = "tools:";
            // 
            // panel3
            // 
            this.panel3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(64)))));
            this.panel3.Controls.Add(this.bLoadTexture);
            this.panel3.Controls.Add(this.button3);
            this.panel3.Controls.Add(this.bNew);
            this.panel3.Controls.Add(this.bClose);
            this.panel3.Controls.Add(this.bExport);
            this.panel3.Controls.Add(this.bLoadMap);
            this.panel3.Location = new System.Drawing.Point(3, 4);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(638, 37);
            this.panel3.TabIndex = 16;
            // 
            // bLoadTexture
            // 
            this.bLoadTexture.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(64)))));
            this.bLoadTexture.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bLoadTexture.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bLoadTexture.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(255)))));
            this.bLoadTexture.Location = new System.Drawing.Point(217, 5);
            this.bLoadTexture.Name = "bLoadTexture";
            this.bLoadTexture.Size = new System.Drawing.Size(100, 26);
            this.bLoadTexture.TabIndex = 13;
            this.bLoadTexture.Text = "Load Texture";
            this.bLoadTexture.UseVisualStyleBackColor = false;
            this.bLoadTexture.Click += new System.EventHandler(this.bLoadTexture_Click);
            // 
            // button3
            // 
            this.button3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(64)))));
            this.button3.Enabled = false;
            this.button3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button3.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(255)))));
            this.button3.Location = new System.Drawing.Point(427, 5);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(100, 26);
            this.button3.TabIndex = 12;
            this.button3.Text = "Options";
            this.button3.UseVisualStyleBackColor = false;
            // 
            // FormEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(30)))));
            this.ClientSize = new System.Drawing.Size(643, 546);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(650, 300);
            this.Name = "FormEditor";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "2D isoedit";
            this.Resize += new System.EventHandler(this.FormEditor_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.pBResult)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pBResult;
        private System.Windows.Forms.Button bExport;
        private System.Windows.Forms.Button bClose;
        private System.Windows.Forms.Button bLoadMap;
        private System.Windows.Forms.Button bRotL;
        private System.Windows.Forms.Button bRotR;
        private System.Windows.Forms.Button bNew;
        private System.Windows.Forms.Timer renderTimer;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.RadioButton radioButtonPreR;
        private System.Windows.Forms.RadioButton radioButtonPreM;
        private System.Windows.Forms.CheckBox checkBoxPreAR;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Button bRot;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button bLoadTexture;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RadioButton radioButtonShadowOf;
        private System.Windows.Forms.RadioButton radioButtonShadowHigh;
        private System.Windows.Forms.RadioButton radioButtonShadowLow;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Panel panel2;
    }
}

