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
            this.pBEditorMap = new System.Windows.Forms.PictureBox();
            this.pBResult = new System.Windows.Forms.PictureBox();
            this.bExport = new System.Windows.Forms.Button();
            this.bClose = new System.Windows.Forms.Button();
            this.bLoad = new System.Windows.Forms.Button();
            this.bRotL = new System.Windows.Forms.Button();
            this.bRotR = new System.Windows.Forms.Button();
            this.bSave = new System.Windows.Forms.Button();
            this.bNew = new System.Windows.Forms.Button();
            this.bSwitch = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.panel2 = new System.Windows.Forms.Panel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.button1 = new System.Windows.Forms.Button();
            this.checkBoxTexture = new System.Windows.Forms.CheckBox();
            this.listBoxTexture = new System.Windows.Forms.ListBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.radioButton4 = new System.Windows.Forms.RadioButton();
            this.radioButton3 = new System.Windows.Forms.RadioButton();
            this.checkBoxHeight = new System.Windows.Forms.CheckBox();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.label5 = new System.Windows.Forms.Label();
            this.textBoxValue = new System.Windows.Forms.TextBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.radioButton8 = new System.Windows.Forms.RadioButton();
            this.radioButton7 = new System.Windows.Forms.RadioButton();
            this.radioButton6 = new System.Windows.Forms.RadioButton();
            this.radioButton5 = new System.Windows.Forms.RadioButton();
            this.label36 = new System.Windows.Forms.Label();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.radioButtonEditM = new System.Windows.Forms.RadioButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.checkBoxGame = new System.Windows.Forms.CheckBox();
            this.checkBoxShadow = new System.Windows.Forms.CheckBox();
            this.bRot = new System.Windows.Forms.Button();
            this.checkBoxPreAR = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.radioButtonPreR = new System.Windows.Forms.RadioButton();
            this.radioButtonPreM = new System.Windows.Forms.RadioButton();
            this.panel3 = new System.Windows.Forms.Panel();
            this.button3 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pBEditorMap)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pBResult)).BeginInit();
            this.panel2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // pBEditorMap
            // 
            this.pBEditorMap.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pBEditorMap.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(140)))));
            this.pBEditorMap.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pBEditorMap.Location = new System.Drawing.Point(140, 70);
            this.pBEditorMap.Name = "pBEditorMap";
            this.pBEditorMap.Size = new System.Drawing.Size(364, 378);
            this.pBEditorMap.TabIndex = 1;
            this.pBEditorMap.TabStop = false;
            this.pBEditorMap.Paint += new System.Windows.Forms.PaintEventHandler(this.pBTextureMap_Paint);
            this.pBEditorMap.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pBEditorMap_MouseDown);
            this.pBEditorMap.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pBEditorMap_MouseMove);
            this.pBEditorMap.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pBEditorMap_MouseUp);
            this.pBEditorMap.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.pBEditorMap_MouseWheel);
            // 
            // pBResult
            // 
            this.pBResult.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pBResult.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(140)))));
            this.pBResult.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pBResult.Location = new System.Drawing.Point(3, 70);
            this.pBResult.Name = "pBResult";
            this.pBResult.Size = new System.Drawing.Size(660, 378);
            this.pBResult.TabIndex = 3;
            this.pBResult.TabStop = false;
            this.pBResult.Paint += new System.Windows.Forms.PaintEventHandler(this.pBRender_Paint);
            this.pBResult.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pBRender_MouseMove);
            this.pBResult.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.pBRender_MouseWheel);
            // 
            // bExport
            // 
            this.bExport.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.bExport.Enabled = false;
            this.bExport.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bExport.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bExport.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(255)))));
            this.bExport.Location = new System.Drawing.Point(321, 3);
            this.bExport.Name = "bExport";
            this.bExport.Size = new System.Drawing.Size(100, 30);
            this.bExport.TabIndex = 4;
            this.bExport.Text = "Export";
            this.bExport.UseVisualStyleBackColor = false;
            // 
            // bClose
            // 
            this.bClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.bClose.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.bClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bClose.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bClose.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(255)))));
            this.bClose.Location = new System.Drawing.Point(1076, 3);
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
            this.bLoad.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bLoad.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bLoad.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(255)))));
            this.bLoad.Location = new System.Drawing.Point(215, 3);
            this.bLoad.Name = "bLoad";
            this.bLoad.Size = new System.Drawing.Size(100, 30);
            this.bLoad.TabIndex = 6;
            this.bLoad.Text = "Load";
            this.bLoad.UseVisualStyleBackColor = false;
            this.bLoad.Click += new System.EventHandler(this.bLoad_Click);
            // 
            // bRotL
            // 
            this.bRotL.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.bRotL.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.bRotL.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bRotL.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bRotL.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(255)))));
            this.bRotL.Location = new System.Drawing.Point(563, 36);
            this.bRotL.Name = "bRotL";
            this.bRotL.Size = new System.Drawing.Size(100, 30);
            this.bRotL.TabIndex = 7;
            this.bRotL.Text = "Rotate - 45°";
            this.bRotL.UseVisualStyleBackColor = false;
            this.bRotL.Click += new System.EventHandler(this.bRotL_Click);
            // 
            // bRotR
            // 
            this.bRotR.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.bRotR.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.bRotR.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bRotR.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bRotR.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(255)))));
            this.bRotR.Location = new System.Drawing.Point(457, 36);
            this.bRotR.Name = "bRotR";
            this.bRotR.Size = new System.Drawing.Size(100, 30);
            this.bRotR.TabIndex = 8;
            this.bRotR.Text = "Rotate + 45°";
            this.bRotR.UseVisualStyleBackColor = false;
            this.bRotR.Click += new System.EventHandler(this.bRotR_Click);
            // 
            // bSave
            // 
            this.bSave.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.bSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bSave.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bSave.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(255)))));
            this.bSave.Location = new System.Drawing.Point(109, 3);
            this.bSave.Name = "bSave";
            this.bSave.Size = new System.Drawing.Size(100, 30);
            this.bSave.TabIndex = 10;
            this.bSave.Text = "Save";
            this.bSave.UseVisualStyleBackColor = false;
            this.bSave.Click += new System.EventHandler(this.bSave_Click);
            // 
            // bNew
            // 
            this.bNew.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.bNew.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bNew.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bNew.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(255)))));
            this.bNew.Location = new System.Drawing.Point(3, 3);
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
            this.bSwitch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bSwitch.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bSwitch.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(255)))));
            this.bSwitch.Location = new System.Drawing.Point(7, 37);
            this.bSwitch.Name = "bSwitch";
            this.bSwitch.Size = new System.Drawing.Size(127, 29);
            this.bSwitch.TabIndex = 12;
            this.bSwitch.Text = "Switch mode";
            this.bSwitch.UseVisualStyleBackColor = false;
            this.bSwitch.Click += new System.EventHandler(this.bSwitch_Click);
            // 
            // timer1
            // 
            this.timer1.Interval = 17;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.panel2.Controls.Add(this.bSwitch);
            this.panel2.Controls.Add(this.groupBox1);
            this.panel2.Controls.Add(this.groupBox2);
            this.panel2.Controls.Add(this.groupBox3);
            this.panel2.Controls.Add(this.label36);
            this.panel2.Controls.Add(this.radioButton2);
            this.panel2.Controls.Add(this.radioButtonEditM);
            this.panel2.Controls.Add(this.pBEditorMap);
            this.panel2.Location = new System.Drawing.Point(12, 51);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(507, 453);
            this.panel2.TabIndex = 14;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.Controls.Add(this.checkBoxTexture);
            this.groupBox1.Controls.Add(this.listBoxTexture);
            this.groupBox1.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox1.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.groupBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(255)))));
            this.groupBox1.Location = new System.Drawing.Point(7, 309);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(127, 139);
            this.groupBox1.TabIndex = 22;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Texture";
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(255)))));
            this.button1.Location = new System.Drawing.Point(6, 37);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(116, 29);
            this.button1.TabIndex = 24;
            this.button1.Text = "edit texture";
            this.button1.UseVisualStyleBackColor = false;
            // 
            // checkBoxTexture
            // 
            this.checkBoxTexture.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.checkBoxTexture.Checked = true;
            this.checkBoxTexture.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxTexture.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.checkBoxTexture.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.checkBoxTexture.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(255)))));
            this.checkBoxTexture.Location = new System.Drawing.Point(6, 20);
            this.checkBoxTexture.Name = "checkBoxTexture";
            this.checkBoxTexture.Size = new System.Drawing.Size(116, 19);
            this.checkBoxTexture.TabIndex = 26;
            this.checkBoxTexture.Text = "enabled";
            this.checkBoxTexture.UseVisualStyleBackColor = false;
            // 
            // listBoxTexture
            // 
            this.listBoxTexture.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.listBoxTexture.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(44)))));
            this.listBoxTexture.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.listBoxTexture.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(255)))));
            this.listBoxTexture.FormattingEnabled = true;
            this.listBoxTexture.IntegralHeight = false;
            this.listBoxTexture.ItemHeight = 15;
            this.listBoxTexture.Location = new System.Drawing.Point(6, 69);
            this.listBoxTexture.Margin = new System.Windows.Forms.Padding(0);
            this.listBoxTexture.Name = "listBoxTexture";
            this.listBoxTexture.Size = new System.Drawing.Size(116, 67);
            this.listBoxTexture.TabIndex = 22;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.radioButton4);
            this.groupBox2.Controls.Add(this.radioButton3);
            this.groupBox2.Controls.Add(this.checkBoxHeight);
            this.groupBox2.Controls.Add(this.radioButton1);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.textBoxValue);
            this.groupBox2.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox2.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.groupBox2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(255)))));
            this.groupBox2.Location = new System.Drawing.Point(7, 176);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(127, 130);
            this.groupBox2.TabIndex = 23;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Height";
            // 
            // radioButton4
            // 
            this.radioButton4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.radioButton4.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.radioButton4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(255)))));
            this.radioButton4.Location = new System.Drawing.Point(9, 102);
            this.radioButton4.Margin = new System.Windows.Forms.Padding(0);
            this.radioButton4.Name = "radioButton4";
            this.radioButton4.Size = new System.Drawing.Size(74, 18);
            this.radioButton4.TabIndex = 26;
            this.radioButton4.Text = "sub -=";
            this.radioButton4.UseVisualStyleBackColor = true;
            // 
            // radioButton3
            // 
            this.radioButton3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.radioButton3.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.radioButton3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(255)))));
            this.radioButton3.Location = new System.Drawing.Point(9, 84);
            this.radioButton3.Margin = new System.Windows.Forms.Padding(0);
            this.radioButton3.Name = "radioButton3";
            this.radioButton3.Size = new System.Drawing.Size(87, 18);
            this.radioButton3.TabIndex = 24;
            this.radioButton3.Text = "add +=";
            this.radioButton3.UseVisualStyleBackColor = true;
            // 
            // checkBoxHeight
            // 
            this.checkBoxHeight.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.checkBoxHeight.Checked = true;
            this.checkBoxHeight.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxHeight.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.checkBoxHeight.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.checkBoxHeight.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(255)))));
            this.checkBoxHeight.Location = new System.Drawing.Point(6, 20);
            this.checkBoxHeight.Name = "checkBoxHeight";
            this.checkBoxHeight.Size = new System.Drawing.Size(116, 19);
            this.checkBoxHeight.TabIndex = 23;
            this.checkBoxHeight.Text = "enabled";
            this.checkBoxHeight.UseVisualStyleBackColor = false;
            // 
            // radioButton1
            // 
            this.radioButton1.Checked = true;
            this.radioButton1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.radioButton1.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.radioButton1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(255)))));
            this.radioButton1.Location = new System.Drawing.Point(9, 66);
            this.radioButton1.Margin = new System.Windows.Forms.Padding(0);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(87, 18);
            this.radioButton1.TabIndex = 22;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = "set =";
            this.radioButton1.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.label5.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.label5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(255)))));
            this.label5.Location = new System.Drawing.Point(6, 43);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(39, 21);
            this.label5.TabIndex = 25;
            this.label5.Text = "value";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // textBoxValue
            // 
            this.textBoxValue.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(44)))));
            this.textBoxValue.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.textBoxValue.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(255)))));
            this.textBoxValue.Location = new System.Drawing.Point(46, 43);
            this.textBoxValue.Margin = new System.Windows.Forms.Padding(0);
            this.textBoxValue.Name = "textBoxValue";
            this.textBoxValue.Size = new System.Drawing.Size(76, 23);
            this.textBoxValue.TabIndex = 24;
            this.textBoxValue.Text = "1";
            this.textBoxValue.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.radioButton8);
            this.groupBox3.Controls.Add(this.radioButton7);
            this.groupBox3.Controls.Add(this.radioButton6);
            this.groupBox3.Controls.Add(this.radioButton5);
            this.groupBox3.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox3.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.groupBox3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(255)))));
            this.groupBox3.Location = new System.Drawing.Point(7, 70);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(127, 103);
            this.groupBox3.TabIndex = 23;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Tools";
            // 
            // radioButton8
            // 
            this.radioButton8.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.radioButton8.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.radioButton8.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(255)))));
            this.radioButton8.Location = new System.Drawing.Point(6, 73);
            this.radioButton8.Margin = new System.Windows.Forms.Padding(0);
            this.radioButton8.Name = "radioButton8";
            this.radioButton8.Size = new System.Drawing.Size(105, 18);
            this.radioButton8.TabIndex = 30;
            this.radioButton8.Text = "fill by texture";
            this.radioButton8.UseVisualStyleBackColor = true;
            // 
            // radioButton7
            // 
            this.radioButton7.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.radioButton7.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.radioButton7.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(255)))));
            this.radioButton7.Location = new System.Drawing.Point(6, 55);
            this.radioButton7.Margin = new System.Windows.Forms.Padding(0);
            this.radioButton7.Name = "radioButton7";
            this.radioButton7.Size = new System.Drawing.Size(105, 18);
            this.radioButton7.TabIndex = 29;
            this.radioButton7.Text = "fill by height";
            this.radioButton7.UseVisualStyleBackColor = true;
            // 
            // radioButton6
            // 
            this.radioButton6.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.radioButton6.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.radioButton6.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(255)))));
            this.radioButton6.Location = new System.Drawing.Point(6, 37);
            this.radioButton6.Margin = new System.Windows.Forms.Padding(0);
            this.radioButton6.Name = "radioButton6";
            this.radioButton6.Size = new System.Drawing.Size(87, 18);
            this.radioButton6.TabIndex = 28;
            this.radioButton6.Text = "draw poly";
            this.radioButton6.UseVisualStyleBackColor = true;
            // 
            // radioButton5
            // 
            this.radioButton5.Checked = true;
            this.radioButton5.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.radioButton5.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.radioButton5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(255)))));
            this.radioButton5.Location = new System.Drawing.Point(6, 19);
            this.radioButton5.Margin = new System.Windows.Forms.Padding(0);
            this.radioButton5.Name = "radioButton5";
            this.radioButton5.Size = new System.Drawing.Size(87, 18);
            this.radioButton5.TabIndex = 27;
            this.radioButton5.TabStop = true;
            this.radioButton5.Text = "draw Line";
            this.radioButton5.UseVisualStyleBackColor = true;
            // 
            // label36
            // 
            this.label36.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label36.BackColor = System.Drawing.Color.Navy;
            this.label36.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label36.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(255)))));
            this.label36.Location = new System.Drawing.Point(3, 6);
            this.label36.Name = "label36";
            this.label36.Size = new System.Drawing.Size(501, 27);
            this.label36.TabIndex = 19;
            this.label36.Text = "Editor";
            this.label36.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.radioButton2.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.radioButton2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(255)))));
            this.radioButton2.Location = new System.Drawing.Point(197, 39);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(50, 19);
            this.radioButton2.TabIndex = 18;
            this.radioButton2.Text = "draw";
            this.radioButton2.UseVisualStyleBackColor = true;
            // 
            // radioButtonEditM
            // 
            this.radioButtonEditM.AutoSize = true;
            this.radioButtonEditM.Checked = true;
            this.radioButtonEditM.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.radioButtonEditM.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.radioButtonEditM.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(255)))));
            this.radioButtonEditM.Location = new System.Drawing.Point(140, 39);
            this.radioButtonEditM.Name = "radioButtonEditM";
            this.radioButtonEditM.Size = new System.Drawing.Size(54, 19);
            this.radioButtonEditM.TabIndex = 17;
            this.radioButtonEditM.TabStop = true;
            this.radioButtonEditM.Text = "move";
            this.radioButtonEditM.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.panel1.Controls.Add(this.checkBoxGame);
            this.panel1.Controls.Add(this.checkBoxShadow);
            this.panel1.Controls.Add(this.bRot);
            this.panel1.Controls.Add(this.checkBoxPreAR);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.radioButtonPreR);
            this.panel1.Controls.Add(this.radioButtonPreM);
            this.panel1.Controls.Add(this.bRotR);
            this.panel1.Controls.Add(this.bRotL);
            this.panel1.Controls.Add(this.pBResult);
            this.panel1.Location = new System.Drawing.Point(525, 51);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(666, 453);
            this.panel1.TabIndex = 15;
            // 
            // checkBoxGame
            // 
            this.checkBoxGame.AutoSize = true;
            this.checkBoxGame.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.checkBoxGame.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.checkBoxGame.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(255)))));
            this.checkBoxGame.Location = new System.Drawing.Point(288, 39);
            this.checkBoxGame.Name = "checkBoxGame";
            this.checkBoxGame.Size = new System.Drawing.Size(53, 19);
            this.checkBoxGame.TabIndex = 24;
            this.checkBoxGame.Text = "game";
            this.checkBoxGame.UseVisualStyleBackColor = true;
            // 
            // checkBoxShadow
            // 
            this.checkBoxShadow.AutoSize = true;
            this.checkBoxShadow.Checked = true;
            this.checkBoxShadow.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxShadow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.checkBoxShadow.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.checkBoxShadow.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(255)))));
            this.checkBoxShadow.Location = new System.Drawing.Point(213, 39);
            this.checkBoxShadow.Name = "checkBoxShadow";
            this.checkBoxShadow.Size = new System.Drawing.Size(69, 19);
            this.checkBoxShadow.TabIndex = 23;
            this.checkBoxShadow.Text = "shadows";
            this.checkBoxShadow.UseVisualStyleBackColor = true;
            // 
            // bRot
            // 
            this.bRot.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.bRot.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.bRot.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bRot.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bRot.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(255)))));
            this.bRot.Location = new System.Drawing.Point(351, 36);
            this.bRot.Name = "bRot";
            this.bRot.Size = new System.Drawing.Size(100, 30);
            this.bRot.TabIndex = 22;
            this.bRot.Text = "Rotate = 0°";
            this.bRot.UseVisualStyleBackColor = false;
            this.bRot.Click += new System.EventHandler(this.bRot_Click);
            // 
            // checkBoxPreAR
            // 
            this.checkBoxPreAR.AutoSize = true;
            this.checkBoxPreAR.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.checkBoxPreAR.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.checkBoxPreAR.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(255)))));
            this.checkBoxPreAR.Location = new System.Drawing.Point(126, 39);
            this.checkBoxPreAR.Name = "checkBoxPreAR";
            this.checkBoxPreAR.Size = new System.Drawing.Size(81, 19);
            this.checkBoxPreAR.TabIndex = 21;
            this.checkBoxPreAR.Text = "auto rotate";
            this.checkBoxPreAR.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.BackColor = System.Drawing.Color.Navy;
            this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(255)))));
            this.label2.Location = new System.Drawing.Point(3, 6);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(660, 27);
            this.label2.TabIndex = 20;
            this.label2.Text = "Preview";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // radioButtonPreR
            // 
            this.radioButtonPreR.AutoSize = true;
            this.radioButtonPreR.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.radioButtonPreR.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.radioButtonPreR.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(255)))));
            this.radioButtonPreR.Location = new System.Drawing.Point(64, 39);
            this.radioButtonPreR.Name = "radioButtonPreR";
            this.radioButtonPreR.Size = new System.Drawing.Size(55, 19);
            this.radioButtonPreR.TabIndex = 19;
            this.radioButtonPreR.Text = "rotate";
            this.radioButtonPreR.UseVisualStyleBackColor = true;
            // 
            // radioButtonPreM
            // 
            this.radioButtonPreM.AutoSize = true;
            this.radioButtonPreM.Checked = true;
            this.radioButtonPreM.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.radioButtonPreM.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.radioButtonPreM.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(255)))));
            this.radioButtonPreM.Location = new System.Drawing.Point(7, 39);
            this.radioButtonPreM.Name = "radioButtonPreM";
            this.radioButtonPreM.Size = new System.Drawing.Size(54, 19);
            this.radioButtonPreM.TabIndex = 18;
            this.radioButtonPreM.TabStop = true;
            this.radioButtonPreM.Text = "move";
            this.radioButtonPreM.UseVisualStyleBackColor = true;
            // 
            // panel3
            // 
            this.panel3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.panel3.Controls.Add(this.button3);
            this.panel3.Controls.Add(this.bNew);
            this.panel3.Controls.Add(this.bClose);
            this.panel3.Controls.Add(this.bExport);
            this.panel3.Controls.Add(this.bLoad);
            this.panel3.Controls.Add(this.bSave);
            this.panel3.Location = new System.Drawing.Point(12, 4);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1179, 41);
            this.panel3.TabIndex = 16;
            // 
            // button3
            // 
            this.button3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.button3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button3.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(255)))));
            this.button3.Location = new System.Drawing.Point(970, 3);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(100, 30);
            this.button3.TabIndex = 12;
            this.button3.Text = "Options";
            this.button3.UseVisualStyleBackColor = false;
            // 
            // FormEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(30)))));
            this.ClientSize = new System.Drawing.Size(1203, 512);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel2);
            this.MinimumSize = new System.Drawing.Size(1210, 530);
            this.Name = "FormEditor";
            this.Text = "2D isoedit";
            ((System.ComponentModel.ISupportInitialize)(this.pBEditorMap)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pBResult)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.PictureBox pBEditorMap;
        private System.Windows.Forms.PictureBox pBResult;
        private System.Windows.Forms.Button bExport;
        private System.Windows.Forms.Button bClose;
        private System.Windows.Forms.Button bLoad;
        private System.Windows.Forms.Button bRotL;
        private System.Windows.Forms.Button bRotR;
        private System.Windows.Forms.Button bSave;
        private System.Windows.Forms.Button bNew;
        private System.Windows.Forms.Button bSwitch;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.RadioButton radioButton2;
        private System.Windows.Forms.RadioButton radioButtonEditM;
        private System.Windows.Forms.RadioButton radioButtonPreR;
        private System.Windows.Forms.RadioButton radioButtonPreM;
        private System.Windows.Forms.Label label36;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox checkBoxPreAR;
        private System.Windows.Forms.ListBox listBoxTexture;
        private System.Windows.Forms.TextBox textBoxValue;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.CheckBox checkBoxHeight;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox checkBoxTexture;
        private System.Windows.Forms.RadioButton radioButton4;
        private System.Windows.Forms.RadioButton radioButton3;
        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.RadioButton radioButton8;
        private System.Windows.Forms.RadioButton radioButton7;
        private System.Windows.Forms.RadioButton radioButton6;
        private System.Windows.Forms.RadioButton radioButton5;
        private System.Windows.Forms.Button bRot;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.CheckBox checkBoxShadow;
        private System.Windows.Forms.CheckBox checkBoxGame;
    }
}

