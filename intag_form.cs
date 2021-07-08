using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;

namespace intag
{
	internal partial class MainForm : Form
	{
		private const int WmNclbuttondown = 0xA1;
		private const int HtCaption = 0x2;
		private static string _folder;
		private static string _oldValue;

		public MainForm(string path)
		{
			InitializeComponent();
			_folder = path;
			directoryName.Text = _folder;
			_oldValue = IniUtils.GetFolderProperties(Path.Combine(_folder, "Desktop.ini"));
			var nearbyValues = IniUtils.GetNearbyPropertiesValues(path);
			var tagIndex = 0;
			foreach (var nearbyValue in nearbyValues)
			{
				tagIndex++;
				AddDynamicButton(tagIndex, nearbyValue);
			}
			Height += 25 * ((tagIndex - 1) / 2);
			propertyInputBox.Text = _oldValue;
		}

		protected override void OnCreateControl()
		{
			base.OnCreateControl();
			Region = Region.FromHrgn(CreateRoundRectRgn(2, 3, Width, Height, 15, 15));
		}

		[DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
		private static extern IntPtr CreateRoundRectRgn(int nLeftRect, int nTopRect, int nRightRect, int nBottomRect, int nWidthEllipse, int nHeightEllipse);

		[DllImport("user32.dll")]
		private static extern bool ReleaseCapture();

		[DllImport("user32.dll")]
		private static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

		private void AddDynamicButton(int index, string value)
		{
			var newButton = new Button
			{
				Text = value,
				Location = new Point(10 + index % 2 * 120, 80 + ((index - 1) / 2 - 1) * 25),
				Size = new Size(115, 23),
				TabStop = false,
				FlatStyle = FlatStyle.Flat,
				FlatAppearance =
				{
					BorderSize = 0
				},
				Font = new Font("Calibri", 9.25F, FontStyle.Bold, GraphicsUnit.Point, 0),
				ForeColor = Color.FromArgb(255, 231, 143),
			};
			newButton.Click += (sender, args) =>
			{
				propertyInputBox.Text = value;
				IniUtils.AssignPropertyToFolder(_folder, value, _oldValue);
				Environment.Exit(0);
			};
			Controls.Add(newButton);
		}
		private void FormDeactivate(object sender, EventArgs e)
		{
			Environment.Exit(1);
		}

		private void FormMouseDown(object sender, MouseEventArgs e)
		{
			if (e.Button != MouseButtons.Left) return;
			ReleaseCapture();
			SendMessage(Handle, WmNclbuttondown, HtCaption, 0);
		}

		private void FormLoad(object sender, EventArgs e)
		{
			Left = Cursor.Position.X - Width / 2;
			Top = Cursor.Position.Y - Height / 2;
			propertyInputBox.Focus();
			propertyInputBox.Select();
		}

		private void PropertyInputBoxKeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Enter)
			{
				IniUtils.AssignPropertyToFolder(_folder, propertyInputBox.Text, _oldValue);
				Environment.Exit(0);
			}
			if (e.KeyCode == Keys.Escape)
			{
				Environment.Exit(0);
			}
		}
	}
}