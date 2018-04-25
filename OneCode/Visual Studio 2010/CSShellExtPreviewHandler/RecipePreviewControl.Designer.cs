namespace CSShellExtPreviewHandler
{
    partial class RecipePreviewControl
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
            this.pctRecipe = new System.Windows.Forms.PictureBox();
            this.lbTitle = new System.Windows.Forms.Label();
            this.tbComments = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.pctRecipe)).BeginInit();
            this.SuspendLayout();
            // 
            // pctRecipe
            // 
            this.pctRecipe.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.pctRecipe.Location = new System.Drawing.Point(13, 27);
            this.pctRecipe.Name = "pctRecipe";
            this.pctRecipe.Size = new System.Drawing.Size(269, 142);
            this.pctRecipe.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pctRecipe.TabIndex = 5;
            this.pctRecipe.TabStop = false;
            // 
            // lbTitle
            // 
            this.lbTitle.AutoSize = true;
            this.lbTitle.Location = new System.Drawing.Point(10, 11);
            this.lbTitle.Name = "lbTitle";
            this.lbTitle.Size = new System.Drawing.Size(23, 13);
            this.lbTitle.TabIndex = 4;
            this.lbTitle.Text = "title";
            // 
            // tbComments
            // 
            this.tbComments.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tbComments.Location = new System.Drawing.Point(13, 175);
            this.tbComments.Multiline = true;
            this.tbComments.Name = "tbComments";
            this.tbComments.ReadOnly = true;
            this.tbComments.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.tbComments.Size = new System.Drawing.Size(269, 157);
            this.tbComments.TabIndex = 3;
            // 
            // RecipePreviewControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pctRecipe);
            this.Controls.Add(this.lbTitle);
            this.Controls.Add(this.tbComments);
            this.Name = "RecipePreviewControl";
            this.Size = new System.Drawing.Size(293, 342);
            ((System.ComponentModel.ISupportInitialize)(this.pctRecipe)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pctRecipe;
        private System.Windows.Forms.Label lbTitle;
        private System.Windows.Forms.TextBox tbComments;

    }
}
