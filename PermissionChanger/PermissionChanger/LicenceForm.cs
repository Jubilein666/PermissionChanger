using System;
using System.Windows.Forms;
using PermissionChanger.Properties;

namespace PermissionChanger
{
    public partial class LicenceForm : Form
    {
        public LicenceForm()
        {
            InitializeComponent();
            richTextBox1.Text = Resources.LICENCE;
        }
    }
}
