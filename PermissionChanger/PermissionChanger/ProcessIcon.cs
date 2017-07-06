using System;
using System.Windows.Forms;
using PermissionChanger.Properties;

namespace PermissionChanger
{
    class ProcessIcon : IDisposable
    {
        private NotifyIcon notifyIcon;
        private ProcessIconContextMenu processIconContextMenu;

        public ProcessIcon()
        {
            notifyIcon = new NotifyIcon();
        }

        public void Display()
        {
            notifyIcon.MouseClick += NotifyIcon_MouseClick;

            notifyIcon.Text = "PermissionSwitch Application";
            notifyIcon.Visible = true;
            notifyIcon.Icon = Resources.Shield;
            processIconContextMenu = new ProcessIconContextMenu();
            notifyIcon.ContextMenuStrip = processIconContextMenu.Create();
            processIconContextMenu.TriggerOpen();
        }

        private void NotifyIcon_MouseClick(object sender, MouseEventArgs e)
        {
            if(e.Button == MouseButtons.Left)
            {
                processIconContextMenu?.TriggerOpen();
            }
        }

        public void Dispose()
        {
            notifyIcon.Dispose();
        }
    }

    class ProcessIconContextMenu
    {
        bool formShown = false;

        public ContextMenuStrip Create()
        {
            ContextMenuStrip contextMenuStrip = new ContextMenuStrip();
            ToolStripMenuItem toolStripMenuItem;
            ToolStripSeparator toolStripSeparator;

            // Open.
            toolStripMenuItem = new ToolStripMenuItem();
            toolStripMenuItem.Text = "Open";
            toolStripMenuItem.Click += ToolStripMenuItem_Open_Click;
            toolStripMenuItem.Image = Resources.Lock;
            contextMenuStrip.Items.Add(toolStripMenuItem);

            // About.
            toolStripMenuItem = new ToolStripMenuItem();
            toolStripMenuItem.Text = "About";
            toolStripMenuItem.Click += ToolStripMenuItem_About_Click;
            toolStripMenuItem.Image = Resources.Info;
            contextMenuStrip.Items.Add(toolStripMenuItem);

            // Separator.
            toolStripSeparator = new ToolStripSeparator();
            contextMenuStrip.Items.Add(toolStripSeparator);

            // Exit.
            toolStripMenuItem = new ToolStripMenuItem();
            toolStripMenuItem.Text = "Exit";
            toolStripMenuItem.Click += ToolStripMenuItem_Exit_Click;
            toolStripMenuItem.Image = Resources.Exit;
            contextMenuStrip.Items.Add(toolStripMenuItem);

            return contextMenuStrip;
        }

        public void TriggerOpen()
        {
            ToolStripMenuItem_Open_Click(new object(), new EventArgs());
        }

        private void ToolStripMenuItem_Exit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void ToolStripMenuItem_About_Click(object sender, EventArgs e)
        {
            if (formShown) return;

            OpenForm(new About());
        }

        private void ToolStripMenuItem_Open_Click(object sender, EventArgs e)
        {
            if (formShown) return;

            OpenForm(new PermissionChanger());
        }

        private void OpenForm(Form form)
        {
            formShown = true;
            form.ShowDialog();
            formShown = false;
        }
    }
}
