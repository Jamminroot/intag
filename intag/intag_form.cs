using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace intag
{
	internal partial class MainForm : Form
	{
		private const int WmNclbuttondown = 0xA1;
		private const int HtCaption = 0x2;
		private Color _accentColor = Color.FromArgb(255, 231, 200);
		private Color _disabledColor = Color.FromArgb(120, 100, 120);

		/// <summary>
		/// Key is object, value is tags assigned to it
		/// </summary>
		private static Dictionary<string, SortedSet<string>> _selectedTags;

		private static Dictionary<string, SortedSet<string>> _selectedTagsOnLoad;
		private static string[] _objects;
		private static SortedSet<string> _tagOptions;

		private enum SelectionState
		{
			None,
			Some,
			All
		}
		
		protected override void OnHandleCreated(EventArgs e)
		{
			WindowUtils.EnableAcrylic(this, Color.Black.WithAlpha(128));
			base.OnHandleCreated(e);
		}

		protected override void OnPaintBackground(PaintEventArgs e)
		{
			e.Graphics.Clear(Color.Transparent);
		}

		public MainForm(string[] batch)
		{
			InitializeComponent();
			if (RegUtils.TryGetSystemColorizationColor(out var accent))
			{
				_accentColor = accent;
				selectedObjectsLabel.ForeColor = _accentColor;
				_disabledColor = accent.WithBrightness(0.5f);
			}
			_objects = batch.Where(b => !string.IsNullOrWhiteSpace(b)).ToArray();
			
			if (_objects.Length == 1)
			{
				selectedObjectsLabel.Text = _objects[0];
			}
			else
			{
				selectedObjectsLabel.Text = "Multiselect (" + _objects.Length + ")";
				ToolTipHint.SetToolTip(selectedObjectsLabel, string.Join("\n", _objects));
			}
			_selectedTags = FileUtils.GetObjectsTags(_objects);
			_selectedTagsOnLoad = _selectedTags.ToDictionary(entry => entry.Key, entry => new SortedSet<string>(entry.Value));
			_tagOptions = FileUtils.GetNearbyTags(_objects[0]);
			var tagIndex = 0;
			foreach (var tagOption in _tagOptions)
			{
				tagIndex++;
				AddDynamicButton(tagIndex, tagOption);
			}
			ResizeRedraw = true;
			AdjustFormHeight();
		}

		private static bool Changed =>
			!(_selectedTagsOnLoad.All(pair => _selectedTags.ContainsKey(pair.Key) && _selectedTags[pair.Key].SetEquals(pair.Value)) &&
			  _selectedTags.All(pair => _selectedTagsOnLoad.ContainsKey(pair.Key) && _selectedTagsOnLoad[pair.Key].SetEquals(pair.Value)));

		private void AdjustFormHeight()
		{
			Height = 87 + 25 * (_tagOptions.Count / 2 + 2);
			Refresh();
		}

		protected override void OnCreateControl()
		{
			base.OnCreateControl();
			Region = Region.FromHrgn(CreateRoundRectRgn(2, 3, Width, Height, 3, 3));
		}

		[DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
		private static extern IntPtr CreateRoundRectRgn(int nLeftRect, int nTopRect, int nRightRect, int nBottomRect, int nWidthEllipse, int nHeightEllipse);

		[DllImport("user32.dll")]
		private static extern bool ReleaseCapture();

		[DllImport("user32.dll")]
		private static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
		
		private static string[] ObjectsWithTag(string tag)
		{
			return _selectedTags.Where(sel => sel.Value.Contains(tag)).Select(pair => pair.Key).ToArray();
		}

		private static SelectionState TagState(string tag, out string[] withTag)
		{
			withTag = ObjectsWithTag(tag);
			if (withTag.Length == _selectedTags.Count)
			{
				return SelectionState.All;
			}
			return withTag.Length > 0 ? SelectionState.Some : SelectionState.None;
			
		}
		
		private void UpdateButton(Control button, string tag)
		{
			var state = TagState(tag, out var withTag);
			switch (state)
			{
				case SelectionState.All:
					button.ForeColor = _accentColor;
					button.Text = tag;
					break;
				case SelectionState.Some:
					button.ForeColor = _accentColor;
					button.Text = $"{tag} (x{withTag.Length})";
					break;
				case SelectionState.None:
				default:
					button.ForeColor = _disabledColor;
					button.Text = tag;
					break;
			}
		}

		private void UpdateButtonTooltip(Control button, string tag)
		{
			var state = TagState(tag, out var withTag);
			switch (state)
			{
				case SelectionState.All:
					ToolTipHint.SetToolTip(button, "[All objects]");
					break;
				case SelectionState.Some:
					ToolTipHint.SetToolTip(button, string.Join("\n", withTag));
					break;
				case SelectionState.None:
					default:
					ToolTipHint.SetToolTip(button, "[None]");
					break;
					
			}
		}

		private void AssignTagToObjects(string tag)
		{
			Debug.WriteLine($"Assigning tag {tag} to {_objects.Length} objects");
			foreach (var obj in _objects)
			{
				if (_selectedTags.ContainsKey(obj))
				{
					Debug.WriteLine($"Assigning tag {tag} to {obj}");
					_selectedTags[obj].Add(tag);
				}
				else
				{
					Debug.WriteLine($"Assigning tag {tag} to {obj} (new set)");
					_selectedTags[obj] = new SortedSet<string> { tag };
				}
			}
		}

		private void DeleteTagFromObjects(string tag)
		{
			Debug.WriteLine($"Deleting tag {tag} from {_objects.Length} objects");
			foreach (var obj in _objects)
			{
				if (_selectedTags.ContainsKey(obj))
				{
					Debug.WriteLine($"Deleting tag {tag} from {obj}");
					_selectedTags[obj].Remove(tag);
				}
			}
		}
		
		private void OnMouseEnter(Button btn, EventArgs e)
		{
			btn.BackColor = _accentColor; // or Color.Red or whatever you want
			btn.ForeColor = Color.White;
		}

		
		private void OnMouseLeave(Button btn, EventArgs e)
		{
			btn.BackColor = Color.Transparent;
			var state = TagState((string)btn.Tag, out _);
			btn.ForeColor = state == SelectionState.None ? _disabledColor : _accentColor;
		}

		private void AddDynamicButton(int index, string tag, bool selected = false)
		{
			var newButton = new Button
			{
				Text = tag,
				Location = new Point(10 + index % 2 * 130, 80 + ((index - 1) / 2 - 1) * 25),
				Size = new Size(125, 23),
				TabStop = false,
				FlatStyle = FlatStyle.Flat,
				FlatAppearance = { BorderSize = 0 },
				BackColor = Color.Transparent,
				Tag = tag,
				Font = new Font("Calibri", 9.25F, FontStyle.Bold, GraphicsUnit.Point, 0),
			};
			if (selected)
			{
				AssignTagToObjects(tag);
			}
			newButton.MouseEnter += (sender, args) => OnMouseEnter(newButton, args);
			newButton.MouseLeave += (sender, args) => OnMouseLeave(newButton, args);
			newButton.Click += (sender, args) =>
			{
				Debug.WriteLine($"Clicked on a button: {tag}");

				//propertyInputBox.Text = value;
				var state = TagState(tag, out _);
				if (state == SelectionState.None || state == SelectionState.Some)
				{
					AssignTagToObjects(tag);
				}
				else
				{
					DeleteTagFromObjects(tag);
				}
				UpdateButton(newButton, tag);
				UpdateButtonTooltip(newButton, tag);

				//IniUtils.AssignPropertyToFolder(_folder, value, _oldSetOfTagsAsString);
				//Environment.Exit(0);
			};
			Controls.Add(newButton);
			UpdateButton(newButton, tag);
			UpdateButtonTooltip(newButton, tag);
		}

		private void FormDeactivate(object sender, EventArgs e)
		{
			if (Changed)
			{
				FileUtils.AssignTags(_selectedTags);
			}
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
				if (propertyInputBox.Text.Length == 0) FormDeactivate(sender, e);
				AddTagOption();
			}
			if (e.KeyCode == Keys.Escape)
			{
				Environment.Exit(0);
			}
		}

		private void AddTagOption()
		{
			var tag = propertyInputBox.Text;
			if (!_tagOptions.Contains(tag))
			{
				Debug.WriteLine($"Adding new tag {tag}");
				_tagOptions.Add(tag);
				AddDynamicButton(_tagOptions.Count, tag, true);
			}
			if (_selectedTags.Any(pair => pair.Value.Contains(tag)) || _tagOptions.Contains(tag))
			{
				propertyInputBox.Text = "";
			}
		}

		private void addButton_Click(object sender, EventArgs e)
		{
			AddTagOption();
		}

		private void addButton_MouseEnter(object sender, EventArgs e)
		{
			addButton.BackColor = _accentColor;
		}

		private void addButton_MouseLeave(object sender, EventArgs e)
		{
			addButton.BackColor = Color.Transparent;
		}

		private void clearButton_MouseEnter(object sender, EventArgs e)
		{
			clearButton.BackColor = _accentColor;
		}

		private void clearButton_MouseLeave(object sender, EventArgs e)
		{
			clearButton.BackColor = Color.Transparent;
		}

		private void propertyInputBox_MouseEnter(object sender, EventArgs e)
		{
			this.ToolTipHint.SetToolTip(this.propertyInputBox, propertyInputBox.Text);
		}

		private void clearButton_Click(object sender, EventArgs e)
		{
			foreach (var key in _selectedTags.Keys.ToArray())
			{
				_selectedTags[key] = new SortedSet<string>();
			}
			foreach (var control in Controls)
			{
				if (control is not Button button) continue;
				UpdateButton(button, (string)button.Tag);
				UpdateButtonTooltip(button, (string)button.Tag);
			}
		}
	}
}