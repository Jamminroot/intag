
using System.Drawing;
using System.Windows.Forms.VisualStyles;

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
            components = new System.ComponentModel.Container();
            selectedObjectsLabel = new System.Windows.Forms.Label();
            propertyInputBox = new System.Windows.Forms.RoundedTextBox();
            addButton = new System.Windows.Forms.Button();
            ToolTipHint = new System.Windows.Forms.ToolTip(components);
            clearButton = new System.Windows.Forms.Button();
            SuspendLayout();
            // 
            // selectedObjectsLabel
            // 
            selectedObjectsLabel.AutoSize = true;
            selectedObjectsLabel.BackColor = Color.Transparent;
            selectedObjectsLabel.Font = new Font("Calibri", 9.25F, FontStyle.Bold, GraphicsUnit.Point);
            selectedObjectsLabel.ForeColor = Color.FromArgb(255, 231, 143);
            selectedObjectsLabel.Location = new Point(12, 8);
            selectedObjectsLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            selectedObjectsLabel.Name = "selectedObjectsLabel";
            selectedObjectsLabel.Size = new Size(54, 15);
            selectedObjectsLabel.TabIndex = 4;
            selectedObjectsLabel.Text = "Selected";
            ToolTipHint.SetToolTip(selectedObjectsLabel, "SSS");
            // 
            // propertyInputBox
            // 
            propertyInputBox.BackColor = SystemColors.Info;
            propertyInputBox.Font = new Font("Calibri", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            propertyInputBox.Location = new Point(12, 29);
            propertyInputBox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            propertyInputBox.MaxLength = 64;
            propertyInputBox.Name = "propertyInputBox";
            propertyInputBox.Size = new Size(200, 24);
            propertyInputBox.TabIndex = 0;
            propertyInputBox.KeyDown += PropertyInputBoxKeyDown;
            propertyInputBox.MouseEnter += propertyInputBox_MouseEnter;
            // 
            // addButton
            // 
            addButton.AccessibleRole = System.Windows.Forms.AccessibleRole.ButtonDropDown;
            addButton.BackColor = Color.Transparent;
            addButton.FlatAppearance.BorderSize = 0;
            addButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            addButton.Font = new Font("Microsoft Sans Serif", 7F, FontStyle.Bold, GraphicsUnit.Point);
            addButton.ForeColor = Color.Transparent;
            addButton.Image = Properties.Resources.add;
            addButton.Location = new Point(217, 26);
            addButton.Margin = new System.Windows.Forms.Padding(0, 0, 0, 0);
            addButton.Name = "addButton";
            addButton.Size = new Size(24, 24);
            addButton.TabIndex = 5;
            addButton.UseVisualStyleBackColor = true;
            addButton.Click += addButton_Click;
            addButton.MouseEnter += addButton_MouseEnter;
            addButton.MouseLeave += addButton_MouseLeave;
            // 
            // clearButton
            // 
            clearButton.BackColor = Color.Transparent;
            clearButton.FlatAppearance.BorderSize = 0;
            clearButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            clearButton.Font = new Font("Microsoft Sans Serif", 7F, FontStyle.Bold, GraphicsUnit.Point);
            clearButton.ForeColor = Color.Transparent;
            clearButton.Image = Properties.Resources.remove;
            clearButton.Location = new Point(241, 26);
            clearButton.Margin = new System.Windows.Forms.Padding(0, 0, 0, 0);
            clearButton.Name = "clearButton";
            clearButton.Size = new Size(24, 24);
            clearButton.TabIndex = 6;
            clearButton.UseVisualStyleBackColor = true;
            clearButton.Click += clearButton_Click;
            clearButton.MouseEnter += clearButton_MouseEnter;
            clearButton.MouseLeave += clearButton_MouseLeave;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            AutoSize = true;
            BackColor = Color.FromArgb(46, 46, 46);
            ClientSize = new Size(275, 100);
            ControlBox = false;
            Controls.Add(clearButton);
            Controls.Add(addButton);
            Controls.Add(propertyInputBox);
            Controls.Add(selectedObjectsLabel);
            Cursor = System.Windows.Forms.Cursors.Hand;
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "MainForm";
            ShowIcon = false;
            ShowInTaskbar = false;
            SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            Text = "intag";
            TopMost = true;
            Deactivate += FormDeactivate;
            Load += FormLoad;
            MouseDown += FormMouseDown;
            ResumeLayout(false);
            PerformLayout();
        }

        private System.Windows.Forms.Button clearButton;

        private System.Windows.Forms.ToolTip ToolTipHint;

        private System.Windows.Forms.Button addButton;

        #endregion

        private System.Windows.Forms.Label selectedObjectsLabel;
        private System.Windows.Forms.RoundedTextBox propertyInputBox;
    }
}

