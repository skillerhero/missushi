﻿namespace Missushi.Forms.Gerente {
    partial class FormAgregarZona {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.btnAgregarZona = new System.Windows.Forms.Button();
            this.udCupo = new System.Windows.Forms.NumericUpDown();
            this.lblCupo = new System.Windows.Forms.Label();
            this.txtFoto = new System.Windows.Forms.TextBox();
            this.lblFoto = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.udCupo)).BeginInit();
            this.SuspendLayout();
            // 
            // btnAgregarZona
            // 
            this.btnAgregarZona.Location = new System.Drawing.Point(165, 188);
            this.btnAgregarZona.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnAgregarZona.Name = "btnAgregarZona";
            this.btnAgregarZona.Size = new System.Drawing.Size(86, 31);
            this.btnAgregarZona.TabIndex = 0;
            this.btnAgregarZona.Text = "Agregar";
            this.btnAgregarZona.UseVisualStyleBackColor = true;
            this.btnAgregarZona.Click += new System.EventHandler(this.btnAgregarZona_Click);
            // 
            // udCupo
            // 
            this.udCupo.Location = new System.Drawing.Point(141, 71);
            this.udCupo.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.udCupo.Name = "udCupo";
            this.udCupo.Size = new System.Drawing.Size(137, 27);
            this.udCupo.TabIndex = 1;
            // 
            // lblCupo
            // 
            this.lblCupo.AutoSize = true;
            this.lblCupo.Location = new System.Drawing.Point(62, 73);
            this.lblCupo.Name = "lblCupo";
            this.lblCupo.Size = new System.Drawing.Size(44, 20);
            this.lblCupo.TabIndex = 2;
            this.lblCupo.Text = "Cupo";
            // 
            // txtFoto
            // 
            this.txtFoto.Location = new System.Drawing.Point(141, 123);
            this.txtFoto.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtFoto.Name = "txtFoto";
            this.txtFoto.Size = new System.Drawing.Size(137, 27);
            this.txtFoto.TabIndex = 3;
            // 
            // lblFoto
            // 
            this.lblFoto.AutoSize = true;
            this.lblFoto.Location = new System.Drawing.Point(69, 133);
            this.lblFoto.Name = "lblFoto";
            this.lblFoto.Size = new System.Drawing.Size(39, 20);
            this.lblFoto.TabIndex = 4;
            this.lblFoto.Text = "Foto";
            // 
            // FormAgregarZona
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(617, 389);
            this.Controls.Add(this.lblFoto);
            this.Controls.Add(this.txtFoto);
            this.Controls.Add(this.lblCupo);
            this.Controls.Add(this.udCupo);
            this.Controls.Add(this.btnAgregarZona);
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "FormAgregarZona";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FormAgregarZona";
            this.Load += new System.EventHandler(this.FormAgregarZona_Load);
            ((System.ComponentModel.ISupportInitialize)(this.udCupo)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Button btnAgregarZona;
        private NumericUpDown udCupo;
        private Label lblCupo;
        private TextBox txtFoto;
        private Label lblFoto;
    }
}