namespace CrazyEights
{
    partial class frmMain
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newGameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.quitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cardBackToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.skyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mineralToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fishToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.frogToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.moonFlowerToolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.islandToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.squaresToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.magentaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sandDunesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.spaceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.linesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toyCarsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.undoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.searchToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contentsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.aboutCrazyEightsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dealToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.optionsToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(792, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newGameToolStripMenuItem,
            this.dealToolStripMenuItem,
            this.toolStripMenuItem2,
            this.quitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // newGameToolStripMenuItem
            // 
            this.newGameToolStripMenuItem.Name = "newGameToolStripMenuItem";
            this.newGameToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.newGameToolStripMenuItem.Text = "New Game";
            this.newGameToolStripMenuItem.Click += new System.EventHandler(this.newGameToolStripMenuItem_Click);
            // 
            // quitToolStripMenuItem
            // 
            this.quitToolStripMenuItem.Name = "quitToolStripMenuItem";
            this.quitToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.quitToolStripMenuItem.Text = "Quit";
            this.quitToolStripMenuItem.Click += new System.EventHandler(this.quitToolStripMenuItem_Click);
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cardBackToolStripMenuItem,
            this.toolStripSeparator1,
            this.undoToolStripMenuItem});
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.optionsToolStripMenuItem.Text = "Options";
            this.optionsToolStripMenuItem.DropDownOpening += new System.EventHandler(this.optionsToolStripMenuItem_DropDownOpening);
            // 
            // cardBackToolStripMenuItem
            // 
            this.cardBackToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.skyToolStripMenuItem,
            this.mineralToolStripMenuItem,
            this.fishToolStripMenuItem,
            this.frogToolStripMenuItem,
            this.moonFlowerToolStripMenuItem2,
            this.islandToolStripMenuItem,
            this.squaresToolStripMenuItem,
            this.magentaToolStripMenuItem,
            this.sandDunesToolStripMenuItem,
            this.spaceToolStripMenuItem,
            this.linesToolStripMenuItem,
            this.toyCarsToolStripMenuItem});
            this.cardBackToolStripMenuItem.Name = "cardBackToolStripMenuItem";
            this.cardBackToolStripMenuItem.Size = new System.Drawing.Size(172, 22);
            this.cardBackToolStripMenuItem.Text = "Card Back";
            this.cardBackToolStripMenuItem.DropDownItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.cardBackToolStripMenuItem_DropDownItemClicked);
            // 
            // skyToolStripMenuItem
            // 
            this.skyToolStripMenuItem.Checked = true;
            this.skyToolStripMenuItem.CheckOnClick = true;
            this.skyToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.skyToolStripMenuItem.Image = global::CrazyEights.Properties.Resources.sky_small;
            this.skyToolStripMenuItem.Name = "skyToolStripMenuItem";
            this.skyToolStripMenuItem.Size = new System.Drawing.Size(144, 22);
            this.skyToolStripMenuItem.Tag = "Sky";
            this.skyToolStripMenuItem.Text = "Sky";
            // 
            // mineralToolStripMenuItem
            // 
            this.mineralToolStripMenuItem.CheckOnClick = true;
            this.mineralToolStripMenuItem.Image = global::CrazyEights.Properties.Resources.mineral_small;
            this.mineralToolStripMenuItem.Name = "mineralToolStripMenuItem";
            this.mineralToolStripMenuItem.Size = new System.Drawing.Size(144, 22);
            this.mineralToolStripMenuItem.Tag = "Mineral";
            this.mineralToolStripMenuItem.Text = "Mineral";
            // 
            // fishToolStripMenuItem
            // 
            this.fishToolStripMenuItem.CheckOnClick = true;
            this.fishToolStripMenuItem.Image = global::CrazyEights.Properties.Resources.fish_small;
            this.fishToolStripMenuItem.Name = "fishToolStripMenuItem";
            this.fishToolStripMenuItem.Size = new System.Drawing.Size(144, 22);
            this.fishToolStripMenuItem.Tag = "Fish";
            this.fishToolStripMenuItem.Text = "Fish";
            // 
            // frogToolStripMenuItem
            // 
            this.frogToolStripMenuItem.CheckOnClick = true;
            this.frogToolStripMenuItem.Image = global::CrazyEights.Properties.Resources.frog_small;
            this.frogToolStripMenuItem.Name = "frogToolStripMenuItem";
            this.frogToolStripMenuItem.Size = new System.Drawing.Size(144, 22);
            this.frogToolStripMenuItem.Tag = "Frog";
            this.frogToolStripMenuItem.Text = "Frog";
            // 
            // moonFlowerToolStripMenuItem2
            // 
            this.moonFlowerToolStripMenuItem2.CheckOnClick = true;
            this.moonFlowerToolStripMenuItem2.Image = global::CrazyEights.Properties.Resources.rose_small;
            this.moonFlowerToolStripMenuItem2.Name = "moonFlowerToolStripMenuItem2";
            this.moonFlowerToolStripMenuItem2.Size = new System.Drawing.Size(144, 22);
            this.moonFlowerToolStripMenuItem2.Tag = "Moonflower";
            this.moonFlowerToolStripMenuItem2.Text = "Moon Flower";
            // 
            // islandToolStripMenuItem
            // 
            this.islandToolStripMenuItem.CheckOnClick = true;
            this.islandToolStripMenuItem.Image = global::CrazyEights.Properties.Resources.island_small;
            this.islandToolStripMenuItem.Name = "islandToolStripMenuItem";
            this.islandToolStripMenuItem.Size = new System.Drawing.Size(144, 22);
            this.islandToolStripMenuItem.Tag = "Island";
            this.islandToolStripMenuItem.Text = "Island";
            // 
            // squaresToolStripMenuItem
            // 
            this.squaresToolStripMenuItem.CheckOnClick = true;
            this.squaresToolStripMenuItem.Image = global::CrazyEights.Properties.Resources.squares_small;
            this.squaresToolStripMenuItem.Name = "squaresToolStripMenuItem";
            this.squaresToolStripMenuItem.Size = new System.Drawing.Size(144, 22);
            this.squaresToolStripMenuItem.Tag = "Squares";
            this.squaresToolStripMenuItem.Text = "Squares";
            // 
            // magentaToolStripMenuItem
            // 
            this.magentaToolStripMenuItem.CheckOnClick = true;
            this.magentaToolStripMenuItem.Image = global::CrazyEights.Properties.Resources.magenta_small;
            this.magentaToolStripMenuItem.Name = "magentaToolStripMenuItem";
            this.magentaToolStripMenuItem.Size = new System.Drawing.Size(144, 22);
            this.magentaToolStripMenuItem.Tag = "Magenta";
            this.magentaToolStripMenuItem.Text = "Magenta";
            // 
            // sandDunesToolStripMenuItem
            // 
            this.sandDunesToolStripMenuItem.CheckOnClick = true;
            this.sandDunesToolStripMenuItem.Image = global::CrazyEights.Properties.Resources.sanddunes_small;
            this.sandDunesToolStripMenuItem.Name = "sandDunesToolStripMenuItem";
            this.sandDunesToolStripMenuItem.Size = new System.Drawing.Size(144, 22);
            this.sandDunesToolStripMenuItem.Tag = "Sanddunes";
            this.sandDunesToolStripMenuItem.Text = "Sand Dunes";
            // 
            // spaceToolStripMenuItem
            // 
            this.spaceToolStripMenuItem.CheckOnClick = true;
            this.spaceToolStripMenuItem.Image = global::CrazyEights.Properties.Resources.space_small;
            this.spaceToolStripMenuItem.Name = "spaceToolStripMenuItem";
            this.spaceToolStripMenuItem.Size = new System.Drawing.Size(144, 22);
            this.spaceToolStripMenuItem.Tag = "Space";
            this.spaceToolStripMenuItem.Text = "Space";
            // 
            // linesToolStripMenuItem
            // 
            this.linesToolStripMenuItem.CheckOnClick = true;
            this.linesToolStripMenuItem.Image = global::CrazyEights.Properties.Resources.lines_small;
            this.linesToolStripMenuItem.Name = "linesToolStripMenuItem";
            this.linesToolStripMenuItem.Size = new System.Drawing.Size(144, 22);
            this.linesToolStripMenuItem.Tag = "Lines";
            this.linesToolStripMenuItem.Text = "Lines";
            // 
            // toyCarsToolStripMenuItem
            // 
            this.toyCarsToolStripMenuItem.CheckOnClick = true;
            this.toyCarsToolStripMenuItem.Image = global::CrazyEights.Properties.Resources.cars_small;
            this.toyCarsToolStripMenuItem.Name = "toyCarsToolStripMenuItem";
            this.toyCarsToolStripMenuItem.Size = new System.Drawing.Size(144, 22);
            this.toyCarsToolStripMenuItem.Tag = "Toycars";
            this.toyCarsToolStripMenuItem.Text = "Toy Cars";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(169, 6);
            // 
            // undoToolStripMenuItem
            // 
            this.undoToolStripMenuItem.Enabled = false;
            this.undoToolStripMenuItem.Name = "undoToolStripMenuItem";
            this.undoToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Z)));
            this.undoToolStripMenuItem.Size = new System.Drawing.Size(172, 22);
            this.undoToolStripMenuItem.Text = "Undo Turn";
            this.undoToolStripMenuItem.Click += new System.EventHandler(this.undoToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.searchToolStripMenuItem,
            this.contentsToolStripMenuItem,
            this.toolStripMenuItem1,
            this.aboutCrazyEightsToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // searchToolStripMenuItem
            // 
            this.searchToolStripMenuItem.Name = "searchToolStripMenuItem";
            this.searchToolStripMenuItem.Size = new System.Drawing.Size(174, 22);
            this.searchToolStripMenuItem.Text = "Search";
            this.searchToolStripMenuItem.Click += new System.EventHandler(this.searchToolStripMenuItem_Click);
            // 
            // contentsToolStripMenuItem
            // 
            this.contentsToolStripMenuItem.Name = "contentsToolStripMenuItem";
            this.contentsToolStripMenuItem.Size = new System.Drawing.Size(174, 22);
            this.contentsToolStripMenuItem.Text = "Contents";
            this.contentsToolStripMenuItem.Click += new System.EventHandler(this.contentsToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(171, 6);
            // 
            // aboutCrazyEightsToolStripMenuItem
            // 
            this.aboutCrazyEightsToolStripMenuItem.Name = "aboutCrazyEightsToolStripMenuItem";
            this.aboutCrazyEightsToolStripMenuItem.Size = new System.Drawing.Size(174, 22);
            this.aboutCrazyEightsToolStripMenuItem.Text = "About Crazy Eights";
            this.aboutCrazyEightsToolStripMenuItem.Click += new System.EventHandler(this.aboutCrazyEightsToolStripMenuItem_Click);
            // 
            // dealToolStripMenuItem
            // 
            this.dealToolStripMenuItem.Name = "dealToolStripMenuItem";
            this.dealToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.dealToolStripMenuItem.Text = "Deal";
            this.dealToolStripMenuItem.Click += new System.EventHandler(this.dealToolStripMenuItem_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(149, 6);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.DarkGreen;
            this.BackgroundImage = global::CrazyEights.Properties.Resources.PNG_TableTile2;
            this.ClientSize = new System.Drawing.Size(792, 566);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "frmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Crazy Eights";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmMain_FormClosing);
            this.HelpRequested += new System.Windows.Forms.HelpEventHandler(this.frmMain_HelpRequested);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.frmMain_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Form1_MouseUp);
            this.Resize += new System.EventHandler(this.Form1_Resize);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newGameToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem quitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem cardBackToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem frogToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem moonFlowerToolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem skyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mineralToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem fishToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem islandToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem squaresToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem magentaToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sandDunesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem spaceToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem linesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toyCarsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem undoToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem searchToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem contentsToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem aboutCrazyEightsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem dealToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;

    }
}

