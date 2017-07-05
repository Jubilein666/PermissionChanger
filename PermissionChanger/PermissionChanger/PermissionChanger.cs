using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace PermissionChanger
{
    public partial class PermissionChanger : Form
    {
        private DirectoryInformations _directoryInformations;
        private string _configSaveFile;
        public PermissionChanger()
        {
            InitializeComponent();
            _configSaveFile = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\PermissionChanger\config.psw";
            GetDirectoryInformations();
            InitializeTableLayoutPanel();
        }

        private void GetDirectoryInformations()
        {
            if (!Directory.Exists(Path.GetDirectoryName(_configSaveFile)))
                Directory.CreateDirectory(Path.GetDirectoryName(_configSaveFile));

            if (File.Exists(_configSaveFile))
            {
                FileStream fileStream = File.OpenRead(_configSaveFile);
                var binaryFormatter = new BinaryFormatter();
                _directoryInformations = (DirectoryInformations)binaryFormatter.Deserialize(fileStream);
                fileStream.Close();
            }
            else
            {
                _directoryInformations = new DirectoryInformations();
            }
        }

        private void SaveConfig()
        {
            FileStream fileStream = File.Create(_configSaveFile);
            var binaryFormatter = new BinaryFormatter();
            binaryFormatter.Serialize(fileStream, _directoryInformations);
            fileStream.Close();
        }
        private void buttonAddDir_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            if (fbd.ShowDialog() != DialogResult.OK) return;

            if (_directoryInformations.Any(x => x.Directory == fbd.SelectedPath))
            {
                MessageBox.Show("Folder already selected.");
                return;
            }

            if (_directoryInformations.Where(x => fbd.SelectedPath.Contains(x.Directory) && x.CheckRestictedPermission()).Count() > 0)
            {
                MessageBox.Show("A parent directory already has restricted permissions.");
                return;
            }

            var dirControl = new DirectoryControl(fbd.SelectedPath);
            _directoryInformations.Add(dirControl);

            //TableLayoutPanel

            tableLayoutPanel1.Controls.Remove((Control)sender);

            tableLayoutPanel1.RowCount++;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            for (int i = 0; i < tableLayoutPanel1.RowCount - 1; i++)
                tableLayoutPanel1.RowStyles[i] = new RowStyle(SizeType.Absolute, 30F);

            tableLayoutPanel1.Controls.Add(new Label()
            {
                Anchor = AnchorStyles.Left,
                AutoSize = true,
                Name = "label" + tableLayoutPanel1.RowCount.ToString(),
                Text = fbd.SelectedPath.Truncate()
            }, 0, tableLayoutPanel1.RowCount - 2);
            var toggleSwitch = new JCS.ToggleSwitch()
            {
                Anchor = AnchorStyles.Right,
                Style = JCS.ToggleSwitch.ToggleSwitchStyle.Android,
                Name = "toggleSwitch" + tableLayoutPanel1.RowCount.ToString(),
                OffForeColor = Color.FromArgb(141, 123, 141),
                OffText = "OFF",
                OffFont = new Font(this.Font.FontFamily, 8, FontStyle.Bold),
                OnForeColor = Color.White,
                OnText = "ON",
                OnFont = new Font(this.Font.FontFamily, 8, FontStyle.Bold),
                Size = new Size(78, 23),
                Checked = dirControl.CheckRestictedPermission(),
                Tag = dirControl
            };
            toggleSwitch.CheckedChanged += ToggleSwitch_CheckedChanged;
            tableLayoutPanel1.Controls.Add(toggleSwitch, 1, tableLayoutPanel1.RowCount - 2);

            tableLayoutPanel1.Controls.Add((Control)sender, 0, tableLayoutPanel1.RowCount - 1);

            int calcHeight = (tableLayoutPanel1.RowCount * 30) + 50;
            int maxHeight = Screen.FromControl(this).Bounds.Height - 100;

            this.Size = new Size(this.Size.Width, Math.Min(calcHeight, maxHeight));

            //TableLayoutPanel

            SaveConfig();
        }

        private void InitializeTableLayoutPanel()
        {
            tableLayoutPanel1.Controls.Clear();
            tableLayoutPanel1.RowStyles.Clear();

            tableLayoutPanel1.RowCount = _directoryInformations.Count + 2;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
            tableLayoutPanel1.Controls.Add(new Label()
            {
                Anchor = AnchorStyles.None,
                AutoSize = true,
                Font = new Font(this.Font.FontFamily, this.Font.Size, FontStyle.Bold),
                Name = "labelDirectory",
                Text = "Directory"
            }, 0, 0);
            tableLayoutPanel1.Controls.Add(new Label()
            {
                Anchor = AnchorStyles.None,
                AutoSize = true,
                Font = new Font(this.Font.FontFamily, this.Font.Size, FontStyle.Bold),
                Name = "labelOnOff",
                Text = "On/Off"
            }, 1, 0);

            for (int i = 1; i < tableLayoutPanel1.RowCount - 1; i++)
            {
                tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
                tableLayoutPanel1.Controls.Add(new Label()
                {
                    Anchor = AnchorStyles.Left,
                    AutoSize = true,
                    Name = "label" + tableLayoutPanel1.RowCount.ToString(),
                    Text = _directoryInformations[i - 1].Directory.Truncate()
                }, 0, i);
                var toggleSwitch = new JCS.ToggleSwitch()
                {
                    Anchor = AnchorStyles.Right,
                    Style = JCS.ToggleSwitch.ToggleSwitchStyle.Android,
                    Name = "toggleSwitch" + tableLayoutPanel1.RowCount.ToString(),
                    OffForeColor = Color.FromArgb(141, 123, 141),
                    OffText = "OFF",
                    OffFont = new Font(this.Font.FontFamily, 8, FontStyle.Bold),
                    OnForeColor = Color.White,
                    OnText = "ON",
                    OnFont = new Font(this.Font.FontFamily, 8, FontStyle.Bold),
                    Size = new Size(78, 23),
                    Checked = _directoryInformations[i - 1].CheckRestictedPermission(),
                    Tag = _directoryInformations[i - 1]
                };
                toggleSwitch.CheckedChanged += ToggleSwitch_CheckedChanged;
                tableLayoutPanel1.Controls.Add(toggleSwitch, 1, i);
            }

            tableLayoutPanel1.Controls.Add(buttonAddDir, 0, tableLayoutPanel1.RowCount - 1);

            int calcHeight = (tableLayoutPanel1.RowCount * 30) + 50;
            int maxHeight = Screen.FromControl(this).Bounds.Height - 100;

            this.Size = new Size(this.Size.Width, Math.Min(calcHeight, maxHeight));
        }

        private void ToggleSwitch_CheckedChanged(object sender, EventArgs e)
        {
            var toggleSwitch = (JCS.ToggleSwitch)sender;
            var dirControl = (DirectoryControl)toggleSwitch.Tag;

            if (toggleSwitch.Checked)
            {
                dirControl.RemoveFullControl();
            }
            else
            {
                dirControl.AddFullControl();
            }
        }
    }
}
