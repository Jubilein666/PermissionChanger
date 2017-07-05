using System;
using System.Windows.Forms;
using System.Reflection;

namespace PermissionChanger
{
    public partial class About : Form
    {
        public About()
        {
            InitializeComponent();
            this.Text = $"Info about {AssemblyTitle}";
            this.labelProgrammName.Text = AssemblyProduct;
            this.labelVersion.Text = AssemblyVersion;
            this.labelCopyright.Text = AssemblyCopyright;

            this.linkLabelToggleSwitch.Text = "ToggleSwitch Winforms Control by Johnny J. under CPOL";
            this.linkLabelToggleSwitch.Links.Add(0, 29, "https://www.codeproject.com/Articles/1029499/ToggleSwitch-Winforms-Control");
            this.linkLabelToggleSwitch.Links.Add(33, 9, "https://www.codeproject.com/Members/JohnnyJorgensen");
            this.linkLabelToggleSwitch.Links.Add(49, 4, "https://www.codeproject.com/info/cpol10.aspx");

            this.linkLabelImages.Text = "small-n-flat Icons by Paomedia under CC BY 3.0";
            this.linkLabelImages.Links.Add(0, 18, "https://www.iconfinder.com/iconsets/small-n-flat");
            this.linkLabelImages.Links.Add(22, 8, "https://www.iconfinder.com/paomedia");
            this.linkLabelImages.Links.Add(37, 9, "https://creativecommons.org/licenses/by/3.0/");
        }

        #region Assemblyattributaccessoren

        public string AssemblyTitle
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyTitleAttribute), false);
                if (attributes.Length > 0)
                {
                    AssemblyTitleAttribute titleAttribute = (AssemblyTitleAttribute)attributes[0];
                    if (titleAttribute.Title != "")
                    {
                        return titleAttribute.Title;
                    }
                }
                return System.IO.Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().CodeBase);
            }
        }

        public string AssemblyVersion
        {
            get
            {
                return Assembly.GetExecutingAssembly().GetName().Version.ToString();
            }
        }

        public string AssemblyProduct
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyProductAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyProductAttribute)attributes[0]).Product;
            }
        }

        public string AssemblyCopyright
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyCopyrightAttribute)attributes[0]).Copyright;
            }
        }
        #endregion

        private void buttonLicence_Click(object sender, EventArgs e)
        {
            new LicenceForm().ShowDialog();
        }
    }
}
