using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace intag
{
    internal partial class MainForm : Form
    {
        private const int WmNclbuttondown = 0xA1;
        private const int HtCaption = 0x2;

        /// <summary>
        /// Key is object, value is tags assigned to it
        /// </summary>
        private static Dictionary<string, SortedSet<string>> _selectedTags;
        private static Dictionary<string, SortedSet<string>> _selectedTagsOnLoad;
        private static string[] _objects;
        private List<SwitchButton> _buttons = new();
        private static SortedSet<string> _tagOptions;

        public MainForm(string[] batch)
        {
            _objects = batch.Where(b=>!string.IsNullOrWhiteSpace(b)).ToArray();
            InitializeComponent();
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

        private static bool Changed => !(_selectedTagsOnLoad.All(pair => _selectedTags.ContainsKey(pair.Key) && _selectedTags[pair.Key].SetEquals(pair.Value)) && _selectedTags.All(pair => _selectedTagsOnLoad.ContainsKey(pair.Key) && _selectedTagsOnLoad[pair.Key].SetEquals(pair.Value)));

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
            return _selectedTags.Where(sel => sel.Value.Contains(tag)).Select(pair=>pair.Key).ToArray();
        }

        private void UpdateButton(ref SwitchButton button, string tag)
        {
            var withTag = ObjectsWithTag(tag);
            if (withTag.Length == _selectedTags.Count)
            {
                button.ForeColor = Color.FromArgb(255, 231, 143);
                button.Text = tag;
            }
            else if (withTag.Length > 0)
            {
                button.ForeColor = Color.FromArgb(255, 231, 143);
                button.Text = $"{tag} (x{withTag.Length})";
            }
            else
            {
                button.ForeColor = Color.FromArgb(200, 180, 180);
                button.Text = tag;
            }
        }

        private void UpdateButtonTooltip(ref SwitchButton button, string tag)
        {
            var withTag = ObjectsWithTag(tag);
            if (withTag.Length == _selectedTags.Count)
            {
                ToolTipHint.SetToolTip(button, "[All objects]");
            }
            else if (withTag.Length > 0)
            {
                ToolTipHint.SetToolTip(button, string.Join("\n", withTag));;
            }
            else
            {
                ToolTipHint.SetToolTip(button, "[None]");
            }
        }

        private void SelectTagOption(string tag)
        {
            foreach (var obj in _objects)
            {
                if (_selectedTags.ContainsKey(obj))
                {
                    _selectedTags[obj].Add(tag);
                }
                else
                {
                    _selectedTags[obj] = new SortedSet<string> {tag};
                }
            }
        }

        private void AddDynamicButton(int index, string tag, bool selected = false)
        {
            var newButton = new SwitchButton
            {
                Text = tag,
                Location = new Point(10 + index % 2 * 120, 80 + ((index - 1) / 2 - 1) * 25),
                Size = new Size(115, 23),
                TabStop = false,
                FlatStyle = FlatStyle.Flat,
                FlatAppearance =
                {
                    BorderSize = 0
                },
                Selected = selected,
                Tag = tag,
                Font = new Font("Calibri", 9.25F, FontStyle.Bold, GraphicsUnit.Point, 0),
            };
            if (selected)
            {
                SelectTagOption(tag);
            }
            newButton.Click += (sender, args) =>
            {
                Debug.WriteLine($"Clicked on a button: {tag}");
                //propertyInputBox.Text = value;
                var withTag = ObjectsWithTag(tag);

                if (withTag.Length>0)
                {
                    foreach (var pair in _selectedTags)
                    {
                        pair.Value.Remove(tag);
                    }
                }
                else
                {
                    SelectTagOption(tag);
                }
                UpdateButton(ref newButton, tag);
                UpdateButtonTooltip(ref newButton, tag);

                //IniUtils.AssignPropertyToFolder(_folder, value, _oldSetOfTagsAsString);
                //Environment.Exit(0);
            };
            Controls.Add(newButton);
            UpdateButton(ref newButton, tag);
            UpdateButtonTooltip(ref newButton, tag);
        }
        private void FormDeactivate(object sender, EventArgs e)
        {
            if (Changed) { FileUtils.AssignTags(_selectedTags); }
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
                if (propertyInputBox.Text.Length == 0)
                    FormDeactivate(sender, e);

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
            if (_selectedTags.Any(pair=>pair.Value.Contains(tag)) || _tagOptions.Contains(tag))
            {
                propertyInputBox.Text = "";
            }
        }

        private void addButton_Click(object sender, EventArgs e)
        {
            AddTagOption();
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
                if (control is not SwitchButton button) continue;
                button.Selected = false;
                UpdateButton(ref button, (string)button.Tag);
                UpdateButtonTooltip(ref button, (string)button.Tag);
            }
        }
    }
}