
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
            this.components = new System.ComponentModel.Container();
            this.selectedObjectsLabel = new System.Windows.Forms.Label();
            this.propertyInputBox = new System.Windows.Forms.RoundedTextBox();
            this.addButton = new System.Windows.Forms.Button();
            this.ToolTipHint = new System.Windows.Forms.ToolTip(this.components);
            this.SuspendLayout();

            // 
            // selectedObjectsLabel
            // 
            this.selectedObjectsLabel.AutoSize = true;
            this.selectedObjectsLabel.Font = new System.Drawing.Font("Calibri", 9.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.selectedObjectsLabel.ForeColor = System.Drawing.Color.FromArgb(((int) (((byte) (255)))), ((int) (((byte) (231)))), ((int) (((byte) (143)))));
            this.selectedObjectsLabel.Location = new System.Drawing.Point(7, 9);
            this.selectedObjectsLabel.Name = "selectedObjectsLabel";
            this.selectedObjectsLabel.Size = new System.Drawing.Size(54, 15);
            this.selectedObjectsLabel.TabIndex = 4;
            this.selectedObjectsLabel.Text = "Selected";
            this.ToolTipHint.SetToolTip(this.selectedObjectsLabel, "SSS");

            // 
            // propertyInputBox
            // 
            this.propertyInputBox.Font = new System.Drawing.Font("Calibri", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.propertyInputBox.Location = new System.Drawing.Point(10, 25);
            this.propertyInputBox.MaxLength = 64;
            this.propertyInputBox.Name = "propertyInputBox";
            this.propertyInputBox.Size = new System.Drawing.Size(185, 21);
            this.propertyInputBox.TabIndex = 0;
            this.propertyInputBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.PropertyInputBoxKeyDown);
            this.propertyInputBox.MouseEnter += new System.EventHandler(this.propertyInputBox_MouseEnter);

            // 
            // addButton
            // 
            this.addButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.addButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte) (204)));
            this.addButton.ForeColor = System.Drawing.SystemColors.Info;
            this.addButton.Location = new System.Drawing.Point(201, 25);
            this.addButton.Name = "addButton";
            this.addButton.Size = new System.Drawing.Size(43, 21);
            this.addButton.TabIndex = 5;
            this.addButton.Text = "NEW";
            this.addButton.UseVisualStyleBackColor = true;
            this.addButton.Click += new System.EventHandler(this.addButton_Click);

            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.BackColor = System.Drawing.Color.FromArgb(((int) (((byte) (46)))), ((int) (((byte) (46)))), ((int) (((byte) (46)))));
            this.ClientSize = new System.Drawing.Size(256, 87);
            this.ControlBox = false;
            this.Controls.Add(this.addButton);
            this.Controls.Add(this.propertyInputBox);
            this.Controls.Add(this.selectedObjectsLabel);
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

        private System.Windows.Forms.ToolTip ToolTipHint;

        private System.Windows.Forms.Button addButton;

        #endregion

        private System.Windows.Forms.Label selectedObjectsLabel;
        private System.Windows.Forms.RoundedTextBox propertyInputBox;
    }
}

