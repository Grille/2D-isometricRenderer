namespace Program
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
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormEditor));
            renderTimer = new System.Windows.Forms.Timer(components);
            menuStrip1 = new System.Windows.Forms.MenuStrip();
            fgdToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            importToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            exportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            dgfToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            toolboxToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            textureToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            displayToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            fullscrenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            autoRotateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            debugToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            pBResult = new System.Windows.Forms.PictureBox();
            toolStrip1 = new System.Windows.Forms.ToolStrip();
            toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            toolStripButtonDrag = new System.Windows.Forms.ToolStripButton();
            toolStripButtonRotate = new System.Windows.Forms.ToolStripButton();
            toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            statusStrip1 = new System.Windows.Forms.StatusStrip();
            toolStripStatusLabelRenderTime = new System.Windows.Forms.ToolStripStatusLabel();
            menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pBResult).BeginInit();
            toolStrip1.SuspendLayout();
            statusStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // renderTimer
            // 
            renderTimer.Interval = 1;
            renderTimer.Tick += renderTimer_Tick;
            // 
            // menuStrip1
            // 
            menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { fgdToolStripMenuItem, toolsToolStripMenuItem, displayToolStripMenuItem });
            menuStrip1.Location = new System.Drawing.Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Padding = new System.Windows.Forms.Padding(7, 2, 0, 2);
            menuStrip1.Size = new System.Drawing.Size(728, 24);
            menuStrip1.TabIndex = 17;
            menuStrip1.Text = "menuStrip1";
            // 
            // fgdToolStripMenuItem
            // 
            fgdToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { newToolStripMenuItem, openToolStripMenuItem, saveToolStripMenuItem, toolStripSeparator3, importToolStripMenuItem, exportToolStripMenuItem, toolStripSeparator1, dgfToolStripMenuItem });
            fgdToolStripMenuItem.Name = "fgdToolStripMenuItem";
            fgdToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            fgdToolStripMenuItem.Text = "File";
            fgdToolStripMenuItem.Click += fgdToolStripMenuItem_Click;
            // 
            // newToolStripMenuItem
            // 
            newToolStripMenuItem.Enabled = false;
            newToolStripMenuItem.Name = "newToolStripMenuItem";
            newToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N;
            newToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            newToolStripMenuItem.Text = "New";
            // 
            // openToolStripMenuItem
            // 
            openToolStripMenuItem.Name = "openToolStripMenuItem";
            openToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O;
            openToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            openToolStripMenuItem.Text = "Open";
            openToolStripMenuItem.Click += openHighMapToolStripMenuItem_Click;
            // 
            // saveToolStripMenuItem
            // 
            saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            saveToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S;
            saveToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            saveToolStripMenuItem.Text = "Save";
            saveToolStripMenuItem.Click += saveRenderToolStripMenuItem_Click;
            // 
            // toolStripSeparator3
            // 
            toolStripSeparator3.Name = "toolStripSeparator3";
            toolStripSeparator3.Size = new System.Drawing.Size(145, 6);
            // 
            // importToolStripMenuItem
            // 
            importToolStripMenuItem.Name = "importToolStripMenuItem";
            importToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.I;
            importToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            importToolStripMenuItem.Text = "Import";
            importToolStripMenuItem.Click += importToolStripMenuItem_Click;
            // 
            // exportToolStripMenuItem
            // 
            exportToolStripMenuItem.Name = "exportToolStripMenuItem";
            exportToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.E;
            exportToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            exportToolStripMenuItem.Text = "Export";
            exportToolStripMenuItem.Click += exportToolStripMenuItem_Click;
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new System.Drawing.Size(145, 6);
            // 
            // dgfToolStripMenuItem
            // 
            dgfToolStripMenuItem.Name = "dgfToolStripMenuItem";
            dgfToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Q;
            dgfToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            dgfToolStripMenuItem.Text = "Quit";
            dgfToolStripMenuItem.Click += quitToolStripMenuItem_Click;
            // 
            // toolsToolStripMenuItem
            // 
            toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { toolboxToolStripMenuItem, textureToolStripMenuItem });
            toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            toolsToolStripMenuItem.Size = new System.Drawing.Size(46, 20);
            toolsToolStripMenuItem.Text = "Tools";
            // 
            // toolboxToolStripMenuItem
            // 
            toolboxToolStripMenuItem.Enabled = false;
            toolboxToolStripMenuItem.Name = "toolboxToolStripMenuItem";
            toolboxToolStripMenuItem.Size = new System.Drawing.Size(116, 22);
            toolboxToolStripMenuItem.Text = "Toolbox";
            toolboxToolStripMenuItem.Click += toolboxToolStripMenuItem_Click;
            // 
            // textureToolStripMenuItem
            // 
            textureToolStripMenuItem.Enabled = false;
            textureToolStripMenuItem.Name = "textureToolStripMenuItem";
            textureToolStripMenuItem.Size = new System.Drawing.Size(116, 22);
            textureToolStripMenuItem.Text = "Texture";
            textureToolStripMenuItem.Click += rotateToolStripMenuItem_Click;
            // 
            // displayToolStripMenuItem
            // 
            displayToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { fullscrenToolStripMenuItem, toolStripSeparator2, settingsToolStripMenuItem, toolStripSeparator4, toolStripMenuItem1, autoRotateToolStripMenuItem, debugToolStripMenuItem });
            displayToolStripMenuItem.Name = "displayToolStripMenuItem";
            displayToolStripMenuItem.Size = new System.Drawing.Size(57, 20);
            displayToolStripMenuItem.Text = "Display";
            // 
            // fullscrenToolStripMenuItem
            // 
            fullscrenToolStripMenuItem.Name = "fullscrenToolStripMenuItem";
            fullscrenToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F11;
            fullscrenToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            fullscrenToolStripMenuItem.Text = "Fullscreen";
            fullscrenToolStripMenuItem.Click += fullscrenToolStripMenuItem_Click;
            // 
            // toolStripSeparator2
            // 
            toolStripSeparator2.Name = "toolStripSeparator2";
            toolStripSeparator2.Size = new System.Drawing.Size(177, 6);
            // 
            // settingsToolStripMenuItem
            // 
            settingsToolStripMenuItem.Enabled = false;
            settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            settingsToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            settingsToolStripMenuItem.Text = "Settings";
            settingsToolStripMenuItem.Click += settingsToolStripMenuItem_Click;
            // 
            // toolStripSeparator4
            // 
            toolStripSeparator4.Name = "toolStripSeparator4";
            toolStripSeparator4.Size = new System.Drawing.Size(177, 6);
            // 
            // toolStripMenuItem1
            // 
            toolStripMenuItem1.BackColor = System.Drawing.SystemColors.ButtonFace;
            toolStripMenuItem1.Checked = true;
            toolStripMenuItem1.CheckOnClick = true;
            toolStripMenuItem1.CheckState = System.Windows.Forms.CheckState.Checked;
            toolStripMenuItem1.Name = "toolStripMenuItem1";
            toolStripMenuItem1.Size = new System.Drawing.Size(180, 22);
            toolStripMenuItem1.Text = "Shadow";
            toolStripMenuItem1.CheckedChanged += toolStripMenuItem1_CheckedChanged;
            // 
            // autoRotateToolStripMenuItem
            // 
            autoRotateToolStripMenuItem.BackColor = System.Drawing.SystemColors.ButtonFace;
            autoRotateToolStripMenuItem.CheckOnClick = true;
            autoRotateToolStripMenuItem.Name = "autoRotateToolStripMenuItem";
            autoRotateToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            autoRotateToolStripMenuItem.Text = "AutoRotate";
            // 
            // debugToolStripMenuItem
            // 
            debugToolStripMenuItem.BackColor = System.Drawing.SystemColors.ButtonFace;
            debugToolStripMenuItem.CheckOnClick = true;
            debugToolStripMenuItem.Name = "debugToolStripMenuItem";
            debugToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            debugToolStripMenuItem.Text = "Debug";
            debugToolStripMenuItem.Click += debugToolStripMenuItem_Click;
            // 
            // pBResult
            // 
            pBResult.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            pBResult.BackColor = System.Drawing.Color.FromArgb(20, 25, 45);
            pBResult.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            pBResult.Location = new System.Drawing.Point(0, 49);
            pBResult.Margin = new System.Windows.Forms.Padding(0);
            pBResult.Name = "pBResult";
            pBResult.Size = new System.Drawing.Size(728, 438);
            pBResult.TabIndex = 3;
            pBResult.TabStop = false;
            pBResult.Paint += pBRender_Paint;
            pBResult.MouseDown += pBResult_MouseDown;
            pBResult.MouseMove += pBRender_MouseMove;
            pBResult.MouseWheel += pBRender_MouseWheel;
            // 
            // toolStrip1
            // 
            toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { toolStripLabel1, toolStripButtonDrag, toolStripButtonRotate, toolStripSeparator5 });
            toolStrip1.Location = new System.Drawing.Point(0, 24);
            toolStrip1.Name = "toolStrip1";
            toolStrip1.Size = new System.Drawing.Size(728, 25);
            toolStrip1.TabIndex = 22;
            toolStrip1.Text = "toolStrip1";
            // 
            // toolStripLabel1
            // 
            toolStripLabel1.Name = "toolStripLabel1";
            toolStripLabel1.Size = new System.Drawing.Size(33, 22);
            toolStripLabel1.Text = "tools";
            // 
            // toolStripButtonDrag
            // 
            toolStripButtonDrag.Checked = true;
            toolStripButtonDrag.CheckOnClick = true;
            toolStripButtonDrag.CheckState = System.Windows.Forms.CheckState.Checked;
            toolStripButtonDrag.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            toolStripButtonDrag.Image = (System.Drawing.Image)resources.GetObject("toolStripButtonDrag.Image");
            toolStripButtonDrag.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButtonDrag.Name = "toolStripButtonDrag";
            toolStripButtonDrag.Size = new System.Drawing.Size(23, 22);
            toolStripButtonDrag.Text = "toolStripButton1";
            toolStripButtonDrag.Click += toolStripButtonDrag_Click;
            // 
            // toolStripButtonRotate
            // 
            toolStripButtonRotate.CheckOnClick = true;
            toolStripButtonRotate.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            toolStripButtonRotate.Image = (System.Drawing.Image)resources.GetObject("toolStripButtonRotate.Image");
            toolStripButtonRotate.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButtonRotate.Name = "toolStripButtonRotate";
            toolStripButtonRotate.Size = new System.Drawing.Size(23, 22);
            toolStripButtonRotate.Text = "toolStripButton2";
            toolStripButtonRotate.Click += toolStripButtonRotate_Click;
            // 
            // toolStripSeparator5
            // 
            toolStripSeparator5.Name = "toolStripSeparator5";
            toolStripSeparator5.Size = new System.Drawing.Size(6, 25);
            // 
            // statusStrip1
            // 
            statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { toolStripStatusLabelRenderTime });
            statusStrip1.Location = new System.Drawing.Point(0, 487);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Padding = new System.Windows.Forms.Padding(1, 0, 16, 0);
            statusStrip1.Size = new System.Drawing.Size(728, 22);
            statusStrip1.TabIndex = 23;
            statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabelRenderTime
            // 
            toolStripStatusLabelRenderTime.BackColor = System.Drawing.SystemColors.Control;
            toolStripStatusLabelRenderTime.Name = "toolStripStatusLabelRenderTime";
            toolStripStatusLabelRenderTime.Size = new System.Drawing.Size(95, 17);
            toolStripStatusLabelRenderTime.Text = "RenderTime 0ms";
            // 
            // FormEditor
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            BackColor = System.Drawing.Color.FromArgb(20, 20, 30);
            ClientSize = new System.Drawing.Size(728, 509);
            Controls.Add(pBResult);
            Controls.Add(statusStrip1);
            Controls.Add(toolStrip1);
            Controls.Add(menuStrip1);
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            IsMdiContainer = true;
            KeyPreview = true;
            MainMenuStrip = menuStrip1;
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            MinimumSize = new System.Drawing.Size(744, 548);
            Name = "FormEditor";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            Text = "2D isoedit";
            KeyDown += FormEditor_KeyDown;
            KeyUp += FormEditor_KeyUp;
            Resize += FormEditor_Resize;
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pBResult).EndInit();
            toolStrip1.ResumeLayout(false);
            toolStrip1.PerformLayout();
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private System.Windows.Forms.Timer renderTimer;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fgdToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem dgfToolStripMenuItem;
        private System.Windows.Forms.PictureBox pBResult;
        private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolboxToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem textureToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem displayToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem fullscrenToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem autoRotateToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem importToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exportToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripButton toolStripButtonDrag;
        private System.Windows.Forms.ToolStripButton toolStripButtonRotate;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelRenderTime;
        private System.Windows.Forms.ToolStripMenuItem debugToolStripMenuItem;
    }
}

