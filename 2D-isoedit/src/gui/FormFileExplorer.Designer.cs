namespace program
{
    partial class FormFileExplorer
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
            this.textBoxDst = new System.Windows.Forms.TextBox();
            this.listBoxExplorer = new System.Windows.Forms.ListBox();
            this.bClose = new System.Windows.Forms.Button();
            this.bLoad = new System.Windows.Forms.Button();
            this.buttonEnd = new System.Windows.Forms.Button();
            this.buttonOk = new System.Windows.Forms.Button();
            this.buttonUp = new System.Windows.Forms.Button();
            this.listBox = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // textBoxDst
            // 
            this.textBoxDst.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxDst.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(44)))));
            this.textBoxDst.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxDst.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.textBoxDst.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(255)))));
            this.textBoxDst.Location = new System.Drawing.Point(47, 9);
            this.textBoxDst.Margin = new System.Windows.Forms.Padding(0);
            this.textBoxDst.Name = "textBoxDst";
            this.textBoxDst.Size = new System.Drawing.Size(622, 23);
            this.textBoxDst.TabIndex = 26;
            this.textBoxDst.Text = "1";
            this.textBoxDst.TextChanged += new System.EventHandler(this.textBoxDst_TextChanged);
            // 
            // listBoxExplorer
            // 
            this.listBoxExplorer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listBoxExplorer.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(44)))));
            this.listBoxExplorer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.listBoxExplorer.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.listBoxExplorer.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(255)))));
            this.listBoxExplorer.FormattingEnabled = true;
            this.listBoxExplorer.IntegralHeight = false;
            this.listBoxExplorer.ItemHeight = 15;
            this.listBoxExplorer.Location = new System.Drawing.Point(147, 42);
            this.listBoxExplorer.Margin = new System.Windows.Forms.Padding(0);
            this.listBoxExplorer.Name = "listBoxExplorer";
            this.listBoxExplorer.Size = new System.Drawing.Size(522, 319);
            this.listBoxExplorer.TabIndex = 28;
            // 
            // bClose
            // 
            this.bClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.bClose.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.bClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bClose.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bClose.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(255)))));
            this.bClose.Location = new System.Drawing.Point(721, 116);
            this.bClose.Name = "bClose";
            this.bClose.Size = new System.Drawing.Size(100, 30);
            this.bClose.TabIndex = 29;
            this.bClose.Text = "Close";
            this.bClose.UseVisualStyleBackColor = false;
            // 
            // bLoad
            // 
            this.bLoad.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.bLoad.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bLoad.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bLoad.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(255)))));
            this.bLoad.Location = new System.Drawing.Point(-143, 116);
            this.bLoad.Name = "bLoad";
            this.bLoad.Size = new System.Drawing.Size(100, 30);
            this.bLoad.TabIndex = 30;
            this.bLoad.Text = "Load";
            this.bLoad.UseVisualStyleBackColor = false;
            // 
            // buttonEnd
            // 
            this.buttonEnd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonEnd.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(64)))));
            this.buttonEnd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonEnd.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonEnd.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(255)))));
            this.buttonEnd.Location = new System.Drawing.Point(569, 370);
            this.buttonEnd.Name = "buttonEnd";
            this.buttonEnd.Size = new System.Drawing.Size(100, 30);
            this.buttonEnd.TabIndex = 31;
            this.buttonEnd.Text = "Cancel";
            this.buttonEnd.UseVisualStyleBackColor = false;
            this.buttonEnd.Click += new System.EventHandler(this.button1_Click);
            // 
            // buttonOk
            // 
            this.buttonOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonOk.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(64)))));
            this.buttonOk.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonOk.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonOk.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(255)))));
            this.buttonOk.Location = new System.Drawing.Point(463, 370);
            this.buttonOk.Name = "buttonOk";
            this.buttonOk.Size = new System.Drawing.Size(100, 30);
            this.buttonOk.TabIndex = 32;
            this.buttonOk.Text = "Load";
            this.buttonOk.UseVisualStyleBackColor = false;
            this.buttonOk.Click += new System.EventHandler(this.buttonOk_Click);
            // 
            // buttonUp
            // 
            this.buttonUp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonUp.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(64)))));
            this.buttonUp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonUp.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonUp.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(255)))));
            this.buttonUp.Location = new System.Drawing.Point(12, 9);
            this.buttonUp.Name = "buttonUp";
            this.buttonUp.Size = new System.Drawing.Size(32, 23);
            this.buttonUp.TabIndex = 33;
            this.buttonUp.Text = "^";
            this.buttonUp.UseVisualStyleBackColor = false;
            this.buttonUp.Click += new System.EventHandler(this.button3_Click);
            // 
            // listBox
            // 
            this.listBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(44)))));
            this.listBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.listBox.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.listBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(255)))));
            this.listBox.FormattingEnabled = true;
            this.listBox.IntegralHeight = false;
            this.listBox.ItemHeight = 15;
            this.listBox.Location = new System.Drawing.Point(12, 42);
            this.listBox.Margin = new System.Windows.Forms.Padding(0);
            this.listBox.Name = "listBox";
            this.listBox.Size = new System.Drawing.Size(125, 319);
            this.listBox.TabIndex = 34;
            // 
            // FormFileExplorer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(64)))));
            this.ClientSize = new System.Drawing.Size(678, 412);
            this.Controls.Add(this.listBox);
            this.Controls.Add(this.buttonUp);
            this.Controls.Add(this.buttonOk);
            this.Controls.Add(this.buttonEnd);
            this.Controls.Add(this.bClose);
            this.Controls.Add(this.bLoad);
            this.Controls.Add(this.listBoxExplorer);
            this.Controls.Add(this.textBoxDst);
            this.Name = "FormFileExplorer";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "FileExplorer";
            this.TopMost = true;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox textBoxDst;
        private System.Windows.Forms.ListBox listBoxExplorer;
        private System.Windows.Forms.Button bClose;
        private System.Windows.Forms.Button bLoad;
        private System.Windows.Forms.Button buttonEnd;
        private System.Windows.Forms.Button buttonOk;
        private System.Windows.Forms.Button buttonUp;
        private System.Windows.Forms.ListBox listBox;
    }
}