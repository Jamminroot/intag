using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace intag
{
	internal partial class MainForm : Form
	{
		private const int WmNclbuttondown = 0xA1;
		private const int HtCaption = 0x2;
		private static HashSet<string> _selectedTags;
		private static string _folder;
		private static List<string> _tagsOfParentFolder;
		public MainForm(string path)
		{
			_folder = path;
			InitializeComponent();
			directoryName.Text = path;
			var desktopIniPath = Path.Combine(path, Constants.DesktopIni);
			_selectedTags = IniUtils.GetFolderProperties(desktopIniPath);
			_tagsOfParentFolder = IniUtils.GetNearbyPropertiesValues(path);
			var tagIndex = 0;
			foreach (var tagOfParent in _tagsOfParentFolder)
			{
				tagIndex++;
				AddDynamicButton(tagIndex, tagOfParent);
			}
			ResizeRedraw = true;
			AdjustForHeight();
		}

		private void AdjustForHeight()
		{
			Height = 87 + 25 * (_tagsOfParentFolder.Count / 2 + 2);
			Refresh();
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
			var newButton = new SwitchButton
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
				ForeColor =_selectedTags.Contains(value) ? Color.FromArgb(255, 231, 143) : Color.FromArgb(200,180, 180),
			};
			newButton.Click += (sender, args) =>
			{
				Debug.WriteLine($"Clicked on a button: {value}");
				//propertyInputBox.Text = value;
				if (_selectedTags.Contains(value))
				{
					_selectedTags.Remove(value);
				}
				else
				{
					_selectedTags.Add(value);
				}
				newButton.ForeColor = _selectedTags.Contains(value) ? Color.FromArgb(255, 231, 143) : Color.FromArgb(200, 180, 180);

				//IniUtils.AssignPropertyToFolder(_folder, value, _oldSetOfTagsAsString);
				//Environment.Exit(0);
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
				var tag = propertyInputBox.Text;
				if (!_tagsOfParentFolder.Contains(tag))
				{
					Debug.WriteLine($"Adding new tag {tag}");
					_tagsOfParentFolder.Add(tag);
					_selectedTags.Add(tag);
					AddDynamicButton(_tagsOfParentFolder.Count, tag);
				}
				if (_selectedTags.Contains(tag) || _tagsOfParentFolder.Contains(tag))
				{
					propertyInputBox.Text = "";
				}
				
				//IniUtils.AssignPropertyToFolder(_folder, propertyInputBox.Text, _oldSetOfTagsAsString);
				//Environment.Exit(0);
			}
			if (e.KeyCode == Keys.Escape)
			{
				Environment.Exit(0);
			}
		}

		private void okButton_Click(object sender, EventArgs e)
		{
			IniUtils.AssignPropertyToFolder(_folder, _selectedTags);
			Environment.Exit(0);
		}
	}
}