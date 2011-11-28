namespace CrazyEights
{
    partial class SuitOverrideDialog
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
            this.radioButtonClubs = new System.Windows.Forms.RadioButton();
            this.radioButtonDiamonds = new System.Windows.Forms.RadioButton();
            this.radioButtonHearts = new System.Windows.Forms.RadioButton();
            this.button1 = new System.Windows.Forms.Button();
            this.radioButtonSpades = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // radioButtonClubs
            // 
            this.radioButtonClubs.AutoSize = true;
            this.radioButtonClubs.Location = new System.Drawing.Point(12, 32);
            this.radioButtonClubs.Name = "radioButtonClubs";
            this.radioButtonClubs.Size = new System.Drawing.Size(51, 17);
            this.radioButtonClubs.TabIndex = 0;
            this.radioButtonClubs.TabStop = true;
            this.radioButtonClubs.Text = "Clubs";
            this.radioButtonClubs.UseVisualStyleBackColor = true;
            // 
            // radioButtonDiamonds
            // 
            this.radioButtonDiamonds.AutoSize = true;
            this.radioButtonDiamonds.Location = new System.Drawing.Point(12, 55);
            this.radioButtonDiamonds.Name = "radioButtonDiamonds";
            this.radioButtonDiamonds.Size = new System.Drawing.Size(72, 17);
            this.radioButtonDiamonds.TabIndex = 1;
            this.radioButtonDiamonds.TabStop = true;
            this.radioButtonDiamonds.Text = "Diamonds";
            this.radioButtonDiamonds.UseVisualStyleBackColor = true;
            // 
            // radioButtonHearts
            // 
            this.radioButtonHearts.AutoSize = true;
            this.radioButtonHearts.Location = new System.Drawing.Point(102, 32);
            this.radioButtonHearts.Name = "radioButtonHearts";
            this.radioButtonHearts.Size = new System.Drawing.Size(56, 17);
            this.radioButtonHearts.TabIndex = 2;
            this.radioButtonHearts.TabStop = true;
            this.radioButtonHearts.Text = "Hearts";
            this.radioButtonHearts.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(48, 86);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 3;
            this.button1.Text = "Ok";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // radioButtonSpades
            // 
            this.radioButtonSpades.AutoSize = true;
            this.radioButtonSpades.Location = new System.Drawing.Point(102, 55);
            this.radioButtonSpades.Name = "radioButtonSpades";
            this.radioButtonSpades.Size = new System.Drawing.Size(61, 17);
            this.radioButtonSpades.TabIndex = 4;
            this.radioButtonSpades.TabStop = true;
            this.radioButtonSpades.Text = "Spades";
            this.radioButtonSpades.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(9, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(88, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Choose a suit:";
            // 
            // SuitOverrideDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(171, 121);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.radioButtonSpades);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.radioButtonHearts);
            this.Controls.Add(this.radioButtonDiamonds);
            this.Controls.Add(this.radioButtonClubs);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "SuitOverrideDialog";
            this.Text = "Wild Card";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SuitOverrideDialog_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RadioButton radioButtonClubs;
        private System.Windows.Forms.RadioButton radioButtonDiamonds;
        private System.Windows.Forms.RadioButton radioButtonHearts;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.RadioButton radioButtonSpades;
        private System.Windows.Forms.Label label1;
    }
}