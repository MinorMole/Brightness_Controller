namespace BrightnessControl
{
    partial class BrightnessControl
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BrightnessControl));
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.assignHotKeyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.hotKeyToolStripMenuItem = new System.Windows.Forms.ToolStripTextBox();
            this.increaseBrightnessToolStripMenuItem = new System.Windows.Forms.ToolStripTextBox();
            this.decreaseBrightnessToolStripMenuItem = new System.Windows.Forms.ToolStripTextBox();
            this.confirmToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.removeHotkeyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addStartupToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.reloadMonitorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.ContextMenuStrip = this.contextMenuStrip1;
            this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
            this.notifyIcon1.Text = "Brightness Controller";
            this.notifyIcon1.Visible = true;
            this.notifyIcon1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon1_MouseClick);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.assignHotKeyToolStripMenuItem,
            this.addStartupToolStripMenuItem,
            this.reloadMonitorToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(262, 132);
            this.contextMenuStrip1.Opening += new System.ComponentModel.CancelEventHandler(this.ContextMenuStrip1_Opening);
            // 
            // assignHotKeyToolStripMenuItem
            // 
            this.assignHotKeyToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.hotKeyToolStripMenuItem,
            this.increaseBrightnessToolStripMenuItem,
            this.decreaseBrightnessToolStripMenuItem,
            this.confirmToolStripMenuItem,
            this.removeHotkeyToolStripMenuItem});
            this.assignHotKeyToolStripMenuItem.Name = "assignHotKeyToolStripMenuItem";
            this.assignHotKeyToolStripMenuItem.Size = new System.Drawing.Size(261, 32);
            this.assignHotKeyToolStripMenuItem.Text = "Assign HotKey";
            // 
            // hotKeyToolStripMenuItem
            // 
            this.hotKeyToolStripMenuItem.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.hotKeyToolStripMenuItem.Name = "hotKeyToolStripMenuItem";
            this.hotKeyToolStripMenuItem.ReadOnly = true;
            this.hotKeyToolStripMenuItem.Size = new System.Drawing.Size(150, 31);
            this.hotKeyToolStripMenuItem.Text = "HotKey : None";
            // 
            // increaseBrightnessToolStripMenuItem
            // 
            this.increaseBrightnessToolStripMenuItem.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.increaseBrightnessToolStripMenuItem.Name = "increaseBrightnessToolStripMenuItem";
            this.increaseBrightnessToolStripMenuItem.ReadOnly = true;
            this.increaseBrightnessToolStripMenuItem.Size = new System.Drawing.Size(110, 31);
            this.increaseBrightnessToolStripMenuItem.Text = "Increase Brightness";
            this.increaseBrightnessToolStripMenuItem.KeyDown += new System.Windows.Forms.KeyEventHandler(this.increaseBrightnessToolStripMenuItem_KeyDown);
            // 
            // decreaseBrightnessToolStripMenuItem
            // 
            this.decreaseBrightnessToolStripMenuItem.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.decreaseBrightnessToolStripMenuItem.Name = "decreaseBrightnessToolStripMenuItem";
            this.decreaseBrightnessToolStripMenuItem.ReadOnly = true;
            this.decreaseBrightnessToolStripMenuItem.Size = new System.Drawing.Size(110, 31);
            this.decreaseBrightnessToolStripMenuItem.Text = "Decrease Brightness";
            this.decreaseBrightnessToolStripMenuItem.KeyDown += new System.Windows.Forms.KeyEventHandler(this.decreaseBrightnessToolStripMenuItem_KeyDown);
            // 
            // confirmToolStripMenuItem
            // 
            this.confirmToolStripMenuItem.Name = "confirmToolStripMenuItem";
            this.confirmToolStripMenuItem.Size = new System.Drawing.Size(240, 34);
            this.confirmToolStripMenuItem.Text = "Confirm";
            this.confirmToolStripMenuItem.Click += new System.EventHandler(this.confirmToolStripMenuItem_Click);
            // 
            // removeHotkeyToolStripMenuItem
            // 
            this.removeHotkeyToolStripMenuItem.Name = "removeHotkeyToolStripMenuItem";
            this.removeHotkeyToolStripMenuItem.Size = new System.Drawing.Size(240, 34);
            this.removeHotkeyToolStripMenuItem.Text = "Remove Hotkey";
            this.removeHotkeyToolStripMenuItem.Click += new System.EventHandler(this.removeHotkeyToolStripMenuItem_Click);
            // 
            // addStartupToolStripMenuItem
            // 
            this.addStartupToolStripMenuItem.Name = "addStartupToolStripMenuItem";
            this.addStartupToolStripMenuItem.Size = new System.Drawing.Size(261, 32);
            this.addStartupToolStripMenuItem.Text = "Windows Startup (Off)";
            this.addStartupToolStripMenuItem.Click += new System.EventHandler(this.AddStartupToolStripMenuItem_Click);
            // 
            // reloadMonitorToolStripMenuItem
            // 
            this.reloadMonitorToolStripMenuItem.Name = "reloadMonitorToolStripMenuItem";
            this.reloadMonitorToolStripMenuItem.Size = new System.Drawing.Size(261, 32);
            this.reloadMonitorToolStripMenuItem.Text = "Reload Monitor";
            this.reloadMonitorToolStripMenuItem.Click += new System.EventHandler(this.ReloadMonitorToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(261, 32);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // BrightnessControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.ClientSize = new System.Drawing.Size(878, 76);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "BrightnessControl";
            this.Opacity = 0.01D;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Brightness Controller";
            this.TopMost = true;
            this.Deactivate += new System.EventHandler(this.BrightnessControl_Deactivate);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.BrightnessControl_FormClosing);
            this.Move += new System.EventHandler(this.Form1_Move);
            this.Resize += new System.EventHandler(this.BrightnessControl_Resize);
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem assignHotKeyToolStripMenuItem;
        private System.Windows.Forms.ToolStripTextBox increaseBrightnessToolStripMenuItem;
        private System.Windows.Forms.ToolStripTextBox decreaseBrightnessToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem confirmToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem removeHotkeyToolStripMenuItem;
        private System.Windows.Forms.ToolStripTextBox hotKeyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addStartupToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem reloadMonitorToolStripMenuItem;
    }
}