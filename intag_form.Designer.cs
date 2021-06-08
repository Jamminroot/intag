
using System.Drawing;

namespace intag
{
    partial class MainForm
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
            this.directoryName = new System.Windows.Forms.Label();
            this.buttonCancel = new System.Windows.Forms.RoundedButton();
            this.propertyInputBox = new System.Windows.Forms.RoundedTextBox();
            this.buttonOk = new System.Windows.Forms.RoundedButton();
            this.SuspendLayout();

            // 
            // directoryName
            // 
            this.directoryName.AutoSize = true;
            this.directoryName.Font = new System.Drawing.Font("Calibri", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.directoryName.ForeColor = System.Drawing.Color.FromArgb(((int) (((byte) (255)))), ((int) (((byte) (231)))), ((int) (((byte) (143)))));
            this.directoryName.Location = new System.Drawing.Point(7, 9);
            this.directoryName.Name = "directoryName";
            this.directoryName.Size = new System.Drawing.Size(58, 13);
            this.directoryName.TabIndex = 4;
            this.directoryName.Text = "<DirName>";

            // 
            // buttonCancel
            // 
            this.buttonCancel.BackColor = System.Drawing.SystemColors.Window;
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonCancel.Font = new System.Drawing.Font("Calibri", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.buttonCancel.Location = new System.Drawing.Point(88, 51);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 3;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = false;
            this.buttonCancel.Click += new System.EventHandler(this.CancelClick);

            // 
            // tagInputBox
            // 
            this.propertyInputBox.Font = new System.Drawing.Font("Calibri", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.propertyInputBox.Location = new System.Drawing.Point(10, 25);
            this.propertyInputBox.MaxLength = 64;
            this.propertyInputBox.Name = "propertyInputBox";
            this.propertyInputBox.Size = new System.Drawing.Size(234, 21);
            this.propertyInputBox.TabIndex = 0;
            this.propertyInputBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.PropertyInputBoxKeyDown);

            // 
            // buttonOk
            // 
            this.buttonOk.BackColor = System.Drawing.SystemColors.Window;
            this.buttonOk.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonOk.Font = new System.Drawing.Font("Calibri", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.buttonOk.Location = new System.Drawing.Point(169, 51);
            this.buttonOk.Name = "buttonOk";
            this.buttonOk.Size = new System.Drawing.Size(75, 23);
            this.buttonOk.TabIndex = 1;
            this.buttonOk.Text = "OK";
            this.buttonOk.UseVisualStyleBackColor = false;
            this.buttonOk.Click += new System.EventHandler(this.OkClick);

            // 
            // IntagForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int) (((byte) (46)))), ((int) (((byte) (46)))), ((int) (((byte) (46)))));
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size(256, 87);
            this.ControlBox = false;
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.propertyInputBox);
            this.Controls.Add(this.directoryName);
            this.Controls.Add(this.buttonOk);
            this.Cursor = System.Windows.Forms.Cursors.Hand;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "MainForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "intag";
            this.TopMost = true;
            this.Deactivate += new System.EventHandler(this.FormDeactivate);
            this.Load += new System.EventHandler(this.FormLoad);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.FormMouseDown);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.RoundedButton buttonOk;
        private System.Windows.Forms.Label directoryName;
        private System.Windows.Forms.RoundedTextBox propertyInputBox;
        private System.Windows.Forms.RoundedButton buttonCancel;
    }
}

