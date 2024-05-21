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
            renderTimer = new System.Windows.Forms.Timer(components);
            menuStrip1 = new System.Windows.Forms.MenuStrip();
            fgdToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            importToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            importTextureToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            exportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            dgfToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            displayToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            fullscrenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            toolStripMenuItemShaders = new System.Windows.Forms.ToolStripMenuItem();
            autoRotateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            debugToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            toolStrip1 = new System.Windows.Forms.ToolStrip();
            toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            toolStripButtonDrag = new System.Windows.Forms.ToolStripButton();
            toolStripButtonRotate = new System.Windows.Forms.ToolStripButton();
            toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            ButtonReset = new System.Windows.Forms.ToolStripButton();
            ButtonLeft = new System.Windows.Forms.ToolStripButton();
            ButtonRight = new System.Windows.Forms.ToolStripButton();
            toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            toolStripLabel4 = new System.Windows.Forms.ToolStripLabel();
            TextBoxRotate = new System.Windows.Forms.ToolStripTextBox();
            toolStripLabel2 = new System.Windows.Forms.ToolStripLabel();
            TextBoxTilt = new System.Windows.Forms.ToolStripTextBox();
            toolStripLabel3 = new System.Windows.Forms.ToolStripLabel();
            TextBoxHeight = new System.Windows.Forms.ToolStripTextBox();
            toolStripLabel5 = new System.Windows.Forms.ToolStripLabel();
            TextBoxScaling = new System.Windows.Forms.ToolStripTextBox();
            statusStrip1 = new System.Windows.Forms.StatusStrip();
            toolStripStatusLabelRenderTime = new System.Windows.Forms.ToolStripStatusLabel();
            pBResult = new Grille.Graphics.Isometric.WinForms.RenderSurface();
            importNormalsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            menuStrip1.SuspendLayout();
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
            menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { fgdToolStripMenuItem, displayToolStripMenuItem });
            menuStrip1.Location = new System.Drawing.Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Padding = new System.Windows.Forms.Padding(7, 2, 0, 2);
            menuStrip1.Size = new System.Drawing.Size(784, 24);
            menuStrip1.TabIndex = 17;
            menuStrip1.Text = "menuStrip1";
            // 
            // fgdToolStripMenuItem
            // 
            fgdToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { importToolStripMenuItem, importTextureToolStripMenuItem, importNormalsToolStripMenuItem, exportToolStripMenuItem, toolStripSeparator1, dgfToolStripMenuItem });
            fgdToolStripMenuItem.Name = "fgdToolStripMenuItem";
            fgdToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            fgdToolStripMenuItem.Text = "File";
            fgdToolStripMenuItem.Click += fgdToolStripMenuItem_Click;
            // 
            // importToolStripMenuItem
            // 
            importToolStripMenuItem.Image = Properties.Resources.OpenFile;
            importToolStripMenuItem.Name = "importToolStripMenuItem";
            importToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.I;
            importToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            importToolStripMenuItem.Text = "Import";
            importToolStripMenuItem.Click += ImportMenuItemClick;
            // 
            // importTextureToolStripMenuItem
            // 
            importTextureToolStripMenuItem.Image = Properties.Resources.OpenFile;
            importTextureToolStripMenuItem.Name = "importTextureToolStripMenuItem";
            importTextureToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            importTextureToolStripMenuItem.Text = "Import Texture";
            importTextureToolStripMenuItem.Click += ImportTextureMenuItemClick;
            // 
            // exportToolStripMenuItem
            // 
            exportToolStripMenuItem.Image = Properties.Resources.Save;
            exportToolStripMenuItem.Name = "exportToolStripMenuItem";
            exportToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.E;
            exportToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            exportToolStripMenuItem.Text = "Export";
            exportToolStripMenuItem.Click += ExportMenuItemClick;
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new System.Drawing.Size(177, 6);
            // 
            // dgfToolStripMenuItem
            // 
            dgfToolStripMenuItem.Image = Properties.Resources.Exit;
            dgfToolStripMenuItem.Name = "dgfToolStripMenuItem";
            dgfToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Q;
            dgfToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            dgfToolStripMenuItem.Text = "Quit";
            dgfToolStripMenuItem.Click += quitToolStripMenuItem_Click;
            // 
            // displayToolStripMenuItem
            // 
            displayToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { fullscrenToolStripMenuItem, toolStripSeparator2, settingsToolStripMenuItem, toolStripSeparator4, toolStripMenuItemShaders, autoRotateToolStripMenuItem, debugToolStripMenuItem });
            displayToolStripMenuItem.Name = "displayToolStripMenuItem";
            displayToolStripMenuItem.Size = new System.Drawing.Size(57, 20);
            displayToolStripMenuItem.Text = "Display";
            // 
            // fullscrenToolStripMenuItem
            // 
            fullscrenToolStripMenuItem.Image = Properties.Resources.FullScreen;
            fullscrenToolStripMenuItem.Name = "fullscrenToolStripMenuItem";
            fullscrenToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F11;
            fullscrenToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            fullscrenToolStripMenuItem.Text = "Fullscreen";
            fullscrenToolStripMenuItem.Click += fullscrenToolStripMenuItem_Click;
            // 
            // toolStripSeparator2
            // 
            toolStripSeparator2.Name = "toolStripSeparator2";
            toolStripSeparator2.Size = new System.Drawing.Size(149, 6);
            // 
            // settingsToolStripMenuItem
            // 
            settingsToolStripMenuItem.Enabled = false;
            settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            settingsToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            settingsToolStripMenuItem.Text = "Settings";
            settingsToolStripMenuItem.Click += settingsToolStripMenuItem_Click;
            // 
            // toolStripSeparator4
            // 
            toolStripSeparator4.Name = "toolStripSeparator4";
            toolStripSeparator4.Size = new System.Drawing.Size(149, 6);
            // 
            // toolStripMenuItemShaders
            // 
            toolStripMenuItemShaders.BackColor = System.Drawing.SystemColors.ButtonFace;
            toolStripMenuItemShaders.Image = Properties.Resources.ShaderSpot;
            toolStripMenuItemShaders.Name = "toolStripMenuItemShaders";
            toolStripMenuItemShaders.Size = new System.Drawing.Size(152, 22);
            toolStripMenuItemShaders.Text = "Shader";
            toolStripMenuItemShaders.CheckedChanged += toolStripMenuItem1_CheckedChanged;
            // 
            // autoRotateToolStripMenuItem
            // 
            autoRotateToolStripMenuItem.BackColor = System.Drawing.SystemColors.ButtonFace;
            autoRotateToolStripMenuItem.CheckOnClick = true;
            autoRotateToolStripMenuItem.Image = Properties.Resources.Rotate;
            autoRotateToolStripMenuItem.Name = "autoRotateToolStripMenuItem";
            autoRotateToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            autoRotateToolStripMenuItem.Text = "AutoRotate";
            // 
            // debugToolStripMenuItem
            // 
            debugToolStripMenuItem.BackColor = System.Drawing.SystemColors.ButtonFace;
            debugToolStripMenuItem.CheckOnClick = true;
            debugToolStripMenuItem.Image = Properties.Resources.StatusInformationOutline;
            debugToolStripMenuItem.Name = "debugToolStripMenuItem";
            debugToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            debugToolStripMenuItem.Text = "Debug";
            debugToolStripMenuItem.Click += DebugMenuItemClick;
            // 
            // toolStrip1
            // 
            toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { toolStripLabel1, toolStripButtonDrag, toolStripButtonRotate, toolStripSeparator5, ButtonReset, ButtonLeft, ButtonRight, toolStripSeparator3, toolStripLabel4, TextBoxRotate, toolStripLabel2, TextBoxTilt, toolStripLabel3, TextBoxHeight, toolStripLabel5, TextBoxScaling });
            toolStrip1.Location = new System.Drawing.Point(0, 24);
            toolStrip1.Name = "toolStrip1";
            toolStrip1.Size = new System.Drawing.Size(784, 25);
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
            toolStripButtonDrag.Image = Properties.Resources.MoveGlyph;
            toolStripButtonDrag.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButtonDrag.Name = "toolStripButtonDrag";
            toolStripButtonDrag.Size = new System.Drawing.Size(23, 22);
            toolStripButtonDrag.Text = "Drag";
            toolStripButtonDrag.Click += toolStripButtonDrag_Click;
            // 
            // toolStripButtonRotate
            // 
            toolStripButtonRotate.CheckOnClick = true;
            toolStripButtonRotate.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            toolStripButtonRotate.Image = Properties.Resources.Rotate;
            toolStripButtonRotate.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButtonRotate.Name = "toolStripButtonRotate";
            toolStripButtonRotate.Size = new System.Drawing.Size(23, 22);
            toolStripButtonRotate.Text = "Rotate";
            toolStripButtonRotate.Click += toolStripButtonRotate_Click;
            // 
            // toolStripSeparator5
            // 
            toolStripSeparator5.Name = "toolStripSeparator5";
            toolStripSeparator5.Size = new System.Drawing.Size(6, 25);
            // 
            // ButtonReset
            // 
            ButtonReset.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            ButtonReset.Image = Properties.Resources.RestoreDefaultView;
            ButtonReset.ImageTransparentColor = System.Drawing.Color.Magenta;
            ButtonReset.Name = "ButtonReset";
            ButtonReset.Size = new System.Drawing.Size(23, 22);
            ButtonReset.Text = "Restore View";
            ButtonReset.Click += ButtonReset_Click;
            // 
            // ButtonLeft
            // 
            ButtonLeft.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            ButtonLeft.Image = Properties.Resources.RotateLeft;
            ButtonLeft.ImageTransparentColor = System.Drawing.Color.Magenta;
            ButtonLeft.Name = "ButtonLeft";
            ButtonLeft.Size = new System.Drawing.Size(23, 22);
            ButtonLeft.Text = "45° Left";
            ButtonLeft.Click += ButtonLeft_Click;
            // 
            // ButtonRight
            // 
            ButtonRight.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            ButtonRight.Image = Properties.Resources.RotateRight;
            ButtonRight.ImageTransparentColor = System.Drawing.Color.Magenta;
            ButtonRight.Name = "ButtonRight";
            ButtonRight.Size = new System.Drawing.Size(23, 22);
            ButtonRight.Text = "45° Right";
            ButtonRight.Click += ButtonRight_Click;
            // 
            // toolStripSeparator3
            // 
            toolStripSeparator3.Name = "toolStripSeparator3";
            toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripLabel4
            // 
            toolStripLabel4.Name = "toolStripLabel4";
            toolStripLabel4.Size = new System.Drawing.Size(38, 22);
            toolStripLabel4.Text = "Angle";
            // 
            // TextBoxRotate
            // 
            TextBoxRotate.Name = "TextBoxRotate";
            TextBoxRotate.Size = new System.Drawing.Size(100, 25);
            TextBoxRotate.Text = "10";
            TextBoxRotate.TextChanged += TextBoxRotate_TextChanged;
            // 
            // toolStripLabel2
            // 
            toolStripLabel2.Name = "toolStripLabel2";
            toolStripLabel2.Size = new System.Drawing.Size(23, 22);
            toolStripLabel2.Text = "Tilt";
            // 
            // TextBoxTilt
            // 
            TextBoxTilt.Name = "TextBoxTilt";
            TextBoxTilt.Size = new System.Drawing.Size(100, 25);
            TextBoxTilt.Text = "0.5";
            TextBoxTilt.TextChanged += TextBoxTilt_TextChanged;
            // 
            // toolStripLabel3
            // 
            toolStripLabel3.Name = "toolStripLabel3";
            toolStripLabel3.Size = new System.Drawing.Size(43, 22);
            toolStripLabel3.Text = "Height";
            // 
            // TextBoxHeight
            // 
            TextBoxHeight.Name = "TextBoxHeight";
            TextBoxHeight.Size = new System.Drawing.Size(100, 25);
            TextBoxHeight.Text = "255";
            TextBoxHeight.TextChanged += TextBoxHeight_TextChanged;
            // 
            // toolStripLabel5
            // 
            toolStripLabel5.Name = "toolStripLabel5";
            toolStripLabel5.Size = new System.Drawing.Size(45, 22);
            toolStripLabel5.Text = "Scaling";
            // 
            // TextBoxScaling
            // 
            TextBoxScaling.Name = "TextBoxScaling";
            TextBoxScaling.Size = new System.Drawing.Size(100, 25);
            TextBoxScaling.Text = "1";
            TextBoxScaling.TextChanged += TextBoxScaling_TextChanged;
            // 
            // statusStrip1
            // 
            statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { toolStripStatusLabelRenderTime });
            statusStrip1.Location = new System.Drawing.Point(0, 539);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Padding = new System.Windows.Forms.Padding(1, 0, 16, 0);
            statusStrip1.Size = new System.Drawing.Size(784, 22);
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
            // pBResult
            // 
            pBResult.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            pBResult.BackColor = System.Drawing.Color.FromArgb(20, 25, 45);
            pBResult.DebugInfoEnabled = false;
            pBResult.DefaultMouseEventsEnabled = true;
            pBResult.DrawBoundings = true;
            pBResult.Location = new System.Drawing.Point(0, 49);
            pBResult.Margin = new System.Windows.Forms.Padding(0);
            pBResult.Name = "pBResult";
            pBResult.OnLeftMouseDown = Grille.Graphics.Isometric.WinForms.RenderSurface.LDownAction.Drag;
            pBResult.Size = new System.Drawing.Size(784, 490);
            pBResult.TabIndex = 25;
            pBResult.Text = "renderSurface1";
            pBResult.Paint += pBRender_Paint;
            // 
            // importNormalsToolStripMenuItem
            // 
            importNormalsToolStripMenuItem.Image = Properties.Resources.OpenFile;
            importNormalsToolStripMenuItem.Name = "importNormalsToolStripMenuItem";
            importNormalsToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            importNormalsToolStripMenuItem.Text = "Import Normals";
            importNormalsToolStripMenuItem.Click += importNormalsToolStripMenuItem_Click;
            // 
            // FormEditor
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            BackColor = System.Drawing.Color.FromArgb(20, 20, 30);
            ClientSize = new System.Drawing.Size(784, 561);
            Controls.Add(pBResult);
            Controls.Add(statusStrip1);
            Controls.Add(toolStrip1);
            Controls.Add(menuStrip1);
            IsMdiContainer = true;
            KeyPreview = true;
            MainMenuStrip = menuStrip1;
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            MinimumSize = new System.Drawing.Size(744, 548);
            Name = "FormEditor";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            Text = "2D isoedit";
            Load += FormEditor_Load;
            KeyDown += FormEditor_KeyDown;
            KeyUp += FormEditor_KeyUp;
            Resize += FormEditor_Resize;
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
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
        private System.Windows.Forms.ToolStripMenuItem displayToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem fullscrenToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemShaders;
        private System.Windows.Forms.ToolStripMenuItem autoRotateToolStripMenuItem;
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
        private System.Windows.Forms.ToolStripMenuItem importTextureToolStripMenuItem;
        private System.Windows.Forms.ToolStripButton ButtonLeft;
        private System.Windows.Forms.ToolStripButton ButtonRight;
        private System.Windows.Forms.ToolStripButton ButtonReset;
        private System.Windows.Forms.ToolStripTextBox TextBoxRotate;
        private Grille.Graphics.Isometric.WinForms.RenderSurface pBResult;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripLabel toolStripLabel2;
        private System.Windows.Forms.ToolStripTextBox TextBoxTilt;
        private System.Windows.Forms.ToolStripLabel toolStripLabel3;
        private System.Windows.Forms.ToolStripTextBox TextBoxHeight;
        private System.Windows.Forms.ToolStripLabel toolStripLabel4;
        private System.Windows.Forms.ToolStripLabel toolStripLabel5;
        private System.Windows.Forms.ToolStripTextBox TextBoxScaling;
        private System.Windows.Forms.ToolStripMenuItem importNormalsToolStripMenuItem;
    }
}

