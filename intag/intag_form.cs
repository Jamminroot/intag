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
	internal partial class IntagForm : Form
	{
		private const string MagicGuid = "{F29F85E0-4FF9-1068-AB91-08002B27B3D9}";
		private const string DesktopMagicProperty = "Prop5=31, ";

		private const string DesktopMagicSection = @"
[" + MagicGuid + @"]
" + DesktopMagicProperty;

		private const string EmptyDesktopIniTemplate = @"[ViewState]
Mode=
Vid=
FolderType=Generic
" + DesktopMagicSection;

		private const int WmNclbuttondown = 0xA1;
		private const int HtCaption = 0x2;

		private static string _filePath;
		private static string _oldTag;

		private void AddTagButton(int index, string tag)
		{
			
			var newButton = new RoundedButton
			{
				Text = tag, Location = new Point(10 + (index % 2) * 120, 105 + (( index -1 ) / 2 -1 ) * 25), Size = new Size(115, 23),
				Font = new Font("Calibri", 9.25F, FontStyle.Bold, GraphicsUnit.Point, 0),
				ForeColor = Color.FromArgb(255, 231, 143),
			};
			newButton.Click += (sender, args) =>
			{
				tagInputBox.Text = tag;
				AssignTagToFolder(_filePath, tag);
				Thread.Sleep(100);
				Environment.Exit(0);
			}; 
			Controls.Add(newButton);
		}
		
		public IntagForm(string path)
		{
			InitializeComponent();
			directoryName.Text = path;
			var nearbyTags = GetNearbyTags(path);
			var tagIndex = 0;
			foreach (var tag in nearbyTags)
			{
				tagIndex++;
				AddTagButton(tagIndex, tag);
			}
			this.Height += 25 * ((tagIndex + 1) / 2);
			_filePath = Path.Combine(path, "Desktop.ini");
			_oldTag = GetFolderTag(_filePath);
			tagInputBox.Text = _oldTag;
		}

		[DllImport("user32.dll")]
		public static extern bool ReleaseCapture();

		[DllImport("user32.dll")]
		public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

		protected override void OnCreateControl()
		{
			base.OnCreateControl();
			Region = Region.FromHrgn(CreateRoundRectRgn(2, 3, Width, Height, 15, 15)); //play with these values till you are happy
		}

		private static IEnumerable<string> GetNearbyTags(string targetFolder)
		{
			var directoryInfo = new DirectoryInfo(targetFolder).Parent;
			if (directoryInfo == null) return null;
			var result = new HashSet<string>();
			foreach (var child in directoryInfo.GetDirectories())
			{
				var tag = GetFolderTag(Path.Combine(child.FullName, "Desktop.ini"));
				if (string.IsNullOrWhiteSpace(tag)) continue;
				result.Add(tag);
			}
			return result;
		}
		
		private static void AssignTagToFolder(string filePath, string tag)
		{
			if (string.IsNullOrWhiteSpace(tag) && tag == _oldTag)
			{
				return;
			}
			var newFile = false;
			if (!File.Exists(filePath))
			{
				using (File.Create(filePath)) { }
				newFile = true;
			}
			var contents = File.ReadAllText(filePath);
			if (string.IsNullOrWhiteSpace(contents)) { File.WriteAllText(filePath, EmptyDesktopIniTemplate + tag); }
			else if (!IsCoorectSectionInDesktopIni(filePath)) { contents += DesktopMagicSection + tag; }
			else if (IsTagsPropertyPresent(filePath))
			{
				var lines = File.ReadAllLines(filePath);
				var magicSectionFound = false;
				var tagAdded = false;
				for (var i = 0; i < lines.Length; i++)
				{
					if (lines[i].Contains(MagicGuid) && !magicSectionFound) { magicSectionFound = true; }
					if (lines[i].ToLower().Trim().StartsWith("prop5") && magicSectionFound)
					{
						lines[i] = DesktopMagicProperty + tag;
						contents = string.Join("\n", lines);
						tagAdded = true;
						break;
					}
				}
				if (!tagAdded && magicSectionFound)
				{
					// Hmm...
				}
				else if (!tagAdded && !magicSectionFound) { contents += DesktopMagicSection + tag; }
			}
			else { contents += DesktopMagicSection + tag; }
			ForcedWriteToFile(filePath, contents, newFile);
		}

		[DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
		private static extern IntPtr CreateRoundRectRgn(int nLeftRect, // x-coordinate of upper-left corner
			int nTopRect, // y-coordinate of upper-left corner
			int nRightRect, // x-coordinate of lower-right corner
			int nBottomRect, // y-coordinate of lower-right corner
			int nWidthEllipse, // width of ellipse
			int nHeightEllipse // height of ellipse
		);

		private static void ForcedWriteToFile(string path, string contents, bool isNew)
		{
			var f = new FileIOPermission(FileIOPermissionAccess.AllAccess, path);
			f.AddPathList(FileIOPermissionAccess.AllAccess, new FileInfo(path).DirectoryName);
			try
			{
				f.Demand();
				var fi = new FileInfo(path);
				var wasHidden = File.Exists(path) && (fi.Attributes & FileAttributes.Hidden) > 0 || isNew;
				var wasSystem = File.Exists(path) && (fi.Attributes & FileAttributes.System) > 0 || isNew;
				var wasReadonly = File.Exists(path) && fi.IsReadOnly || isNew;
				if (File.Exists(path))
				{
					fi.IsReadOnly = false;
					fi.Attributes ^= FileAttributes.Hidden | FileAttributes.System;
				}
				var tmp = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString() + ".intag");
				File.WriteAllText(tmp, contents);
				try { File.Copy(tmp, path, true); }
				catch (UnauthorizedAccessException)
				{
					//ignore - idk tf is that, but that fixes it?..
				}
				if (wasHidden) { fi.Attributes |= FileAttributes.Hidden; }
				if (wasSystem) { fi.Attributes |= FileAttributes.System; }
				if (wasReadonly) { fi.IsReadOnly = true; }
			}
			catch (SecurityException) {  }
		}

		private static string GetFolderTag(string path)
		{
			if (!File.Exists(path) || !IsCoorectSectionInDesktopIni(path)) return "";
			var lines = File.ReadAllLines(path);
			foreach (var line in lines)
			{
				if (!line.ToLower().StartsWith("prop5")) continue;
				var tag = line.Substring(5).Trim('=', '\t', ' ').Substring(3).Trim();
				return tag;
			}
			return "";
		}

		private static bool IsCoorectSectionInDesktopIni(string filePath)
		{
			return new Regex(@"\[" + MagicGuid + @"\].*prop5", RegexOptions.IgnoreCase | RegexOptions.Singleline).IsMatch(File.ReadAllText(filePath));
		}

		private static bool IsTagsPropertyPresent(string filePath)
		{
			var contents = File.ReadAllText(filePath);
			var allTextAfterMagicGuid = contents.Substring(contents.IndexOf(MagicGuid) + MagicGuid.Length);
			return new Regex(@".*prop5", RegexOptions.IgnoreCase | RegexOptions.Singleline).IsMatch(allTextAfterMagicGuid);
		}

		private void buttonCancel_Click(object sender, EventArgs e)
		{
			Environment.Exit(1);
		}

		private void buttonOk_Click(object sender, EventArgs e)
		{
			AssignTagToFolder(_filePath, tagInputBox.Text);
			Environment.Exit(0);
		}

		private void intag_form_Deactivate(object sender, EventArgs e)
		{
			Environment.Exit(1);
		}

		private void intag_form_MouseDown(object sender, MouseEventArgs e)
		{
			if (e.Button != MouseButtons.Left) return;
			ReleaseCapture();
			SendMessage(Handle, WmNclbuttondown, HtCaption, 0);
		}

		private void tagInputBox_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode != Keys.Enter) return;
			AssignTagToFolder(_filePath, tagInputBox.Text);
			Environment.Exit(0);
		}

		private void IntagForm_Load(object sender, EventArgs e)
		{
			this.Left = Cursor.Position.X - this.Width/2;
			this.Top = Cursor.Position.Y - this.Height/2;
			tagInputBox.Focus();
			tagInputBox.Select();
		}
	}
}