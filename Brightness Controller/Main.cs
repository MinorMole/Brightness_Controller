using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace BrightnessControl
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    public struct PHYSICAL_MONITOR
    {
        public IntPtr hPhysicalMonitor;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
        public string szPhysicalMonitorDescription;
    }

    public partial class BrightnessControl : Form
    {

        // HotKey

        [DllImport("user32.dll")]
        public static extern bool RegisterHotKey(IntPtr hWnd, int id, int fsModifiers, int vlc);

        BrightnessController[] bc;
        Label[] label;
        TrackBar[] trackBar;
        int decreaseHotkeyValue=0, increaseHotkeyValue=0;
        public BrightnessControl()
        {
            InitializeComponent();
            Opacity = 0;

            //HotKey
            try
            {
                RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("Software\\Brightness Controller");
                string decreaseBrightness_HotKey_reg = registryKey.GetValue("decreaseBrightness_HotKey", "").ToString();
                string increaseBrightness_HotKey_reg = registryKey.GetValue("increaseBrightness_HotKey", "").ToString();
                registryKey.Dispose();

                if (decreaseBrightness_HotKey_reg != "" && increaseBrightness_HotKey_reg != "")
                {
                    hotKeyToolStripMenuItem.Text = "HotKey : " + decreaseBrightness_HotKey_reg + " & " + increaseBrightness_HotKey_reg;

                    increaseHotkeyValue = (Int32)registryKey.GetValue("increaseBrightness_HotKey_Value", 0);
                    decreaseHotkeyValue = (Int32)registryKey.GetValue("decreaseBrightness_HotKey_Value", 0);
                    registryKey.Dispose();

                    int FirstHotkeyId = 1;
                    Boolean F6Registered = RegisterHotKey(Handle, FirstHotkeyId, 0x0000, decreaseHotkeyValue);

                    int SecondHotkeyId = 2;
                    Boolean F7Registered = RegisterHotKey(Handle, SecondHotkeyId, 0x0000, increaseHotkeyValue);

                    if (!F6Registered)
                    {
                        MessageBox.Show("Global Hotkey " + decreaseBrightness_HotKey_reg + " couldn't be registered !");
                    }

                    if (!F7Registered)
                    {
                        MessageBox.Show("Global Hotkey " + increaseBrightness_HotKey_reg + " couldn't be registered !");
                    }
                }

            }
            catch { }
            //HotKey

            Show();
            scanScreen();
            dynamicComponent();
            Hide();
            Visible = false;
            Opacity = 100;
            WindowState = FormWindowState.Minimized;
            notifyIcon1.Visible = true;
            GC.Collect();
        }

        protected override void WndProc(ref Message m)
        {
            // 5. Catch when a HotKey is pressed !
            if (m.Msg == 0x0312)
            {
                int id = m.WParam.ToInt32();
                // MessageBox.Show(string.Format("Hotkey #{0} pressed", id));

                // 6. Handle what will happen once a respective hotkey is pressed
                switch (id)
                {
                    case 1:

                        //  trackBar[0].Value = 25;

                        for (int num = 0; num < trackBar.Length; num++)
                        {

                            int curValue = trackBar[num].Value;
     
                            if (curValue < 5 && Math.Abs(curValue) % 5 > 0)
                            {
                                curValue = 0;
                            }
                            else
                            {
                                curValue -= 5;
                            }


                            if (curValue >= 0)
                            {
                                trackBar[num].Value = curValue;
                                bc[num].SetBrightness(trackBar[num].Value);
                                updateBrightnessValueDisplay(num);
                            }

                            //Show Form When Using HotKey

                            //Show();
                            //WindowState = FormWindowState.Normal;
                            //Location = new Point((Screen.PrimaryScreen.WorkingArea.Width - Width), (Screen.PrimaryScreen.WorkingArea.Height - Height));
                            //Activate();

                        }
                        break;

                    case 2:

                        for (int num = 0; num < trackBar.Length; num++)
                        {
                            int curValue = trackBar[num].Value;

                            if (curValue > 95 && Math.Abs(curValue) % 5 > 0)
                            {
                                curValue = 100;
                            }
                            else
                            {
                                curValue += 5;
                            }

                            if (curValue <= 100)
                            {
                                trackBar[num].Value = curValue;
                                bc[num].SetBrightness(trackBar[num].Value);
                                updateBrightnessValueDisplay(num);
                            }

                            //Show Form When Using HotKey

                            //Show();
                            //WindowState = FormWindowState.Normal;
                            //Location = new Point((Screen.PrimaryScreen.WorkingArea.Width - Width), (Screen.PrimaryScreen.WorkingArea.Height - Height));
                            //Activate();

                        }
                        break;

                    default:

                        break;
                }
            }

            base.WndProc(ref m);
        }

        private void dynamicComponent()
        {
            int currentDeep = 12;
            label = new Label[bc.Length];
            trackBar = new TrackBar[bc.Length];
            for (int i = 0; i < bc.Length; i++)
            {

                trackBar[i] = new TrackBar();
                trackBar[i].Location = new Point(96, currentDeep);
                trackBar[i].Size = new Size(565, 45);
                trackBar[i].Scroll += new EventHandler(trackBar_Scroll);
                trackBar[i].LargeChange = 5;
                trackBar[i].Name = i.ToString();
                trackBar[i].SendToBack();

                label[i] = new Label();
                label[i].AutoSize = true;
                label[i].Location = new Point(10, currentDeep + 9);
                label[i].Size = new Size(20, 13);
                label[i].ForeColor = Color.White;
                label[i].Text = "Monitor " + i;
                label[i].BringToFront();

                Controls.Add(trackBar[i]);
                Controls.Add(label[i]);

                currentDeep += trackBar[i].Height;
            }
            for (int i = 0; i < bc.Length; i++)
            {
                updateTrackBar(trackBar[i], bc[i].getBrightnessStat());
            }
            ClientSize = new Size(trackBar[0].Location.X + trackBar[0].Width + 6, trackBar[trackBar.Length - 1].Location.Y + trackBar[trackBar.Length - 1].Height - 3);
            Location = new Point((Screen.PrimaryScreen.WorkingArea.Width - Width), (Screen.PrimaryScreen.WorkingArea.Height - Height));

            // Check Startup with Windows
            if (System.Reflection.Assembly.GetEntryAssembly().Location == Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Microsoft\\Windows\\Start Menu\\Programs\\Startup\\Brightness Controller.exe")
            {
                addStartupToolStripMenuItem.Enabled = false;
                addStartupToolStripMenuItem.Text = "Windows Startup (On)";
            }
            else
            {
                addStartupToolStripMenuItem.Text = "Windows Startup (Off)";
            }

            // Delete Old Update File
            TryAgain: try
            {
                string OldExeLocation = System.Reflection.Assembly.GetEntryAssembly().Location + ".old";
                if (File.Exists(OldExeLocation))
                {
                    File.Delete(OldExeLocation);
                }
            }
            catch (Exception)
            {
                goto TryAgain;
            }

            // Check Update
            try
            {
                using (WebClient client = new WebClient())
                {
                    client.CachePolicy = new System.Net.Cache.RequestCachePolicy(System.Net.Cache.RequestCacheLevel.NoCacheNoStore);
                    string CheckVersion = client.DownloadString("https://minormole.github.io/Brightness_Controller/update/version");
                    if (CheckVersion.Contains(".") & Application.ProductVersion != CheckVersion)
                    {
                        Updater();
                    }
                }
            } catch (Exception) {}

        }

        private void scanScreen()
        {
            List<BrightnessController> bc_temp = new List<BrightnessController>();
            Screen[] screen = Screen.AllScreens;
            List<IntPtr> screenHandle = new List<IntPtr>();
            for (int i = 0; i < screen.Length; i++)
            {
                Location = screen[i].WorkingArea.Location;
                BrightnessController temp = null;
                try
                {
                    temp = new BrightnessController(Handle);
                }
                catch { }
                if (temp != null && temp.checkMonitorSupport())
                {
                    bc_temp.Add(temp);
                }
            }
            if (bc_temp.Count == 0)
            {
                Location = new Point((Screen.PrimaryScreen.WorkingArea.Width - Width), (Screen.PrimaryScreen.WorkingArea.Height - Height));
                RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("Software\\Brightness Controller");
                if ((string)registryKey.GetValue("CHECK_ERROR_01", "") == "1")
                {
                    DialogResult dialogResult = MessageBox.Show("Your monitor does not support brightness setting!" + Environment.NewLine + "Do you want to check again?", "Brightness Controller", MessageBoxButtons.YesNo);
                    if (dialogResult == DialogResult.Yes)
                    {
                        RegistryKey exampleRegistryKey = Registry.CurrentUser.CreateSubKey("Software\\Brightness Controller");
                        exampleRegistryKey.SetValue("CHECK_ERROR_01", "0");
                        Process.Start(System.Reflection.Assembly.GetEntryAssembly().Location);
                    }
                }
                else
                {
                    RegistryKey exampleRegistryKey = Registry.CurrentUser.CreateSubKey("Software\\Brightness Controller");
                    exampleRegistryKey.SetValue("CHECK_ERROR_01", "1");
                    Process.Start(System.Reflection.Assembly.GetEntryAssembly().Location);
                }
                Process.GetCurrentProcess().Kill();
            }
            else
            {
                RegistryKey exampleRegistryKey = Registry.CurrentUser.CreateSubKey("Software\\Brightness Controller");
                exampleRegistryKey.SetValue("CHECK_ERROR_01", "0");
                bc = bc_temp.ToArray();
            }
        }

        private void Form1_Move(object sender, EventArgs e)
        {
            if (bc != null)
            {
                updateBrightnessValueDisplay();
            }
        }

        private void trackBar_Scroll(object sender, EventArgs e)
        {
            int num = int.Parse(((TrackBar)sender).Name);
            bc[num].SetBrightness(trackBar[num].Value);
            updateBrightnessValueDisplay(num);
            trackBar[num].SendToBack();
        }

        private void updateBrightnessValueDisplay()
        {
            for (int i = 0; i < bc.Length; i++)
            {
                updateBrightnessValueDisplay(i);
            }
        }

        private void updateBrightnessValueDisplay(int screen)
        {

            // Change Notify Icon

            if (trackBar[screen].Value >= 0 && trackBar[screen].Value < 5) { notifyIcon1.Icon = Brightness_Controller.Properties.Resources.Icon000; }
            else if (trackBar[screen].Value >= 5 && trackBar[screen].Value < 15) { notifyIcon1.Icon = Brightness_Controller.Properties.Resources.Icon010; }
            else if (trackBar[screen].Value >= 15 && trackBar[screen].Value < 25) { notifyIcon1.Icon = Brightness_Controller.Properties.Resources.Icon020; }
            else if (trackBar[screen].Value >= 25 && trackBar[screen].Value < 35) { notifyIcon1.Icon = Brightness_Controller.Properties.Resources.Icon030; }
            else if (trackBar[screen].Value >= 35 && trackBar[screen].Value < 45) { notifyIcon1.Icon = Brightness_Controller.Properties.Resources.Icon040; }
            else if (trackBar[screen].Value >= 45 && trackBar[screen].Value < 55) { notifyIcon1.Icon = Brightness_Controller.Properties.Resources.Icon050; }
            else if (trackBar[screen].Value >= 55 && trackBar[screen].Value < 65) { notifyIcon1.Icon = Brightness_Controller.Properties.Resources.Icon060; }
            else if (trackBar[screen].Value >= 65 && trackBar[screen].Value < 75) { notifyIcon1.Icon = Brightness_Controller.Properties.Resources.Icon070; }
            else if (trackBar[screen].Value >= 75 && trackBar[screen].Value < 85) { notifyIcon1.Icon = Brightness_Controller.Properties.Resources.Icon080; }
            else if (trackBar[screen].Value >= 85 && trackBar[screen].Value < 95) { notifyIcon1.Icon = Brightness_Controller.Properties.Resources.Icon090; }
            else if (trackBar[screen].Value >= 95 && trackBar[screen].Value <= 100) { notifyIcon1.Icon = Brightness_Controller.Properties.Resources.Icon100; }

            label[screen].Text = "Monitor " + screen + " - " + trackBar[screen].Value + "%";
            updateTrackBar(trackBar[screen], bc[screen].getBrightnessStat());

            for (int i = 0; i < bc.Length; i++)
            {
                if (i > 0)
                {
                    notifyIcon1.Text += Environment.NewLine + "Monitor " + i + " - " + trackBar[i].Value + "%";
                }
                else
                {
                    notifyIcon1.Text = "Monitor " + i + " - " + trackBar[i].Value + "%";
                }
            }

            GC.Collect();

        }

        private void updateTrackBar(TrackBar tb, uint[] brightness)
        {
            tb.Minimum = (int)brightness[0];
            tb.Maximum = (int)brightness[2];
            tb.Value = (int)brightness[1];
            GC.Collect();
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Show();
            WindowState = FormWindowState.Normal;

            if (GetTaskBarLocation() == TaskBarLocation.TOP)
            {
                Location = new Point(Screen.PrimaryScreen.WorkingArea.Width - Width, ClientSize.Height);
            }
            else if (GetTaskBarLocation() == TaskBarLocation.LEFT)
            {
                Location = new Point(Screen.PrimaryScreen.Bounds.Left + Screen.PrimaryScreen.WorkingArea.Left, Screen.PrimaryScreen.WorkingArea.Height - Height);
            }
            else if (GetTaskBarLocation() == TaskBarLocation.RIGHT)
            {
                Location = new Point(Screen.PrimaryScreen.WorkingArea.Width - ClientSize.Width, Screen.PrimaryScreen.WorkingArea.Height - Height);
            }
            else
            {
                Location = new Point(Screen.PrimaryScreen.WorkingArea.Width - Width, Screen.PrimaryScreen.WorkingArea.Height - Height);
            }

            Activate();
            GC.Collect();
        }

        private void BrightnessControl_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                notifyIcon1.Visible = true;
                Hide();
                e.Cancel = true;
                GC.Collect();
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void BrightnessControl_Resize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                Hide();
                notifyIcon1.Visible = true;
            }
        }

        private void BrightnessControl_Deactivate(object sender, EventArgs e)
        {
            notifyIcon1.Visible = true;
            Hide();
        }

        private void reloadMonitorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start(System.Reflection.Assembly.GetEntryAssembly().Location);
            Application.Exit();
        }

        private void increaseBrightnessToolStripMenuItem_KeyDown(object sender, KeyEventArgs e)
        {
            increaseBrightnessToolStripMenuItem.Text = e.KeyData.ToString();
            increaseHotkeyValue = e.KeyValue;
        }

        private void decreaseBrightnessToolStripMenuItem_KeyDown(object sender, KeyEventArgs e)
        {
            decreaseBrightnessToolStripMenuItem.Text = e.KeyData.ToString();
            decreaseHotkeyValue = e.KeyValue;
        }

        private void confirmToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (increaseBrightnessToolStripMenuItem.Text == decreaseBrightnessToolStripMenuItem.Text)
            {
                MessageBox.Show("Both key need to be the different key!", "Brightness Controller");
            }
            else if (increaseBrightnessToolStripMenuItem.Text != "Increase Brightness" && decreaseBrightnessToolStripMenuItem.Text != "Decrease Brightness")
            {
                RegistryKey exampleRegistryKey = Registry.CurrentUser.CreateSubKey("Software\\Brightness Controller");
                exampleRegistryKey.SetValue("increaseBrightness_HotKey", increaseBrightnessToolStripMenuItem.Text);
                exampleRegistryKey.SetValue("decreaseBrightness_HotKey", decreaseBrightnessToolStripMenuItem.Text);
                exampleRegistryKey.SetValue("increaseBrightness_HotKey_Value", increaseHotkeyValue);
                exampleRegistryKey.SetValue("decreaseBrightness_HotKey_Value", decreaseHotkeyValue);
                exampleRegistryKey.Close();
                Process.Start(System.Reflection.Assembly.GetEntryAssembly().Location);
                Application.Exit();
            }
            else
            {
                MessageBox.Show("Both key need to be set!", "Brightness Controller");
            }
        }

        private void ContextMenuStrip1_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            GC.Collect();
        }

        private void removeHotkeyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RegistryKey exampleRegistryKey = Registry.CurrentUser.CreateSubKey("Software\\Brightness Controller");
            exampleRegistryKey.SetValue("increaseBrightness_HotKey", "");
            exampleRegistryKey.SetValue("decreaseBrightness_HotKey", "");
            exampleRegistryKey.SetValue("increaseBrightness_HotKey_Value", 0);
            exampleRegistryKey.SetValue("decreaseBrightness_HotKey_Value", 0);
            exampleRegistryKey.Close();
            Process.Start(System.Reflection.Assembly.GetEntryAssembly().Location);
            Application.Exit();
        }

        private enum TaskBarLocation { TOP, BOTTOM, LEFT, RIGHT }

        private TaskBarLocation GetTaskBarLocation()
        {
            TaskBarLocation taskBarLocation = TaskBarLocation.BOTTOM;
            bool taskBarOnTopOrBottom = (Screen.PrimaryScreen.WorkingArea.Width == Screen.PrimaryScreen.Bounds.Width);
            if (taskBarOnTopOrBottom)
            {
                if (Screen.PrimaryScreen.WorkingArea.Top > 0) taskBarLocation = TaskBarLocation.TOP;
            }
            else
            {
                if (Screen.PrimaryScreen.WorkingArea.Left > 0)
                {
                    taskBarLocation = TaskBarLocation.LEFT;
                }
                else
                {
                    taskBarLocation = TaskBarLocation.RIGHT;
                }
            }
            return taskBarLocation;
        }

        private void AddStartupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string AppData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Microsoft\\Windows\\Start Menu\\Programs\\Startup\\Brightness Controller.exe";
            if (File.Exists(AppData))
            {
                try { File.Move(AppData, AppData + ".old"); } catch (Exception) { }
                try { File.Move(System.Reflection.Assembly.GetEntryAssembly().Location, AppData); } catch (Exception) { }
                MessageBox.Show("Please restart your computer for the applied changes to take effect.");
                Application.Exit();
            }
            else
            {
                File.Move(System.Reflection.Assembly.GetEntryAssembly().Location, AppData);
                Process.Start(AppData);
                Application.Exit();
            }
        }

        private void Updater()
        {

            string ExeLocation = System.Reflection.Assembly.GetEntryAssembly().Location;

            try
            {
                using (WebClient client = new WebClient())
                {
                    client.CachePolicy = new System.Net.Cache.RequestCachePolicy(System.Net.Cache.RequestCacheLevel.NoCacheNoStore);
                    client.DownloadFile("https://minormole.github.io/Brightness_Controller/update/Brightness%20Controller.exe", ExeLocation + ".new");
                }

                if (File.Exists(ExeLocation + ".new"))
                {
                    File.Move(ExeLocation, ExeLocation + ".old");
                    File.Move(ExeLocation + ".new", ExeLocation);
                    Process.Start(ExeLocation);
                    Application.Exit();
                }
            }
            catch (Exception)
            {
                try
                {
                    File.Delete(ExeLocation + ".new");
                }
                catch (Exception) {}
            }
        }

    }

    public class BrightnessController : IDisposable
    {
        [DllImport("user32.dll", EntryPoint = "MonitorFromWindow")]
        public static extern IntPtr MonitorFromWindow([In] IntPtr hwnd, uint dwFlags);

        [DllImport("dxva2.dll", EntryPoint = "DestroyPhysicalMonitors")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool DestroyPhysicalMonitors(uint dwPhysicalMonitorArraySize, ref PHYSICAL_MONITOR[] pPhysicalMonitorArray);

        [DllImport("dxva2.dll", EntryPoint = "GetNumberOfPhysicalMonitorsFromHMONITOR")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetNumberOfPhysicalMonitorsFromHMONITOR(IntPtr hMonitor, ref uint pdwNumberOfPhysicalMonitors);

        [DllImport("dxva2.dll", EntryPoint = "GetPhysicalMonitorsFromHMONITOR")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetPhysicalMonitorsFromHMONITOR(IntPtr hMonitor, uint dwPhysicalMonitorArraySize, [Out] PHYSICAL_MONITOR[] pPhysicalMonitorArray);

        [DllImport("dxva2.dll", EntryPoint = "GetMonitorBrightness")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetMonitorBrightness(IntPtr handle, ref uint minimumBrightness, ref uint currentBrightness, ref uint maxBrightness);

        [DllImport("dxva2.dll", EntryPoint = "SetMonitorBrightness")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SetMonitorBrightness(IntPtr handle, uint newBrightness);

        private uint _physicalMonitorsCount = 0;
        private PHYSICAL_MONITOR[] _physicalMonitorArray;

        private IntPtr _currentMonitorHandle;

        private uint _minValue = 0;
        private uint _maxValue = 0;
        private uint _currentValue = 0;

        private IntPtr windowHandle_last;

        public bool checkMonitorSupport()
        {
            if (!GetMonitorBrightness(_currentMonitorHandle, ref _minValue, ref _currentValue, ref _maxValue))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public uint[] SetMonitor()
        {
            uint dwFlags = 2u;
            IntPtr ptr = MonitorFromWindow(windowHandle_last, dwFlags);
            if (!GetNumberOfPhysicalMonitorsFromHMONITOR(ptr, ref _physicalMonitorsCount))
            {
                MessageBox.Show("Cannot get monitor count!", "Brightness Controller");
                throw new Exception("Cannot get monitor count!");
            }
            _physicalMonitorArray = new PHYSICAL_MONITOR[_physicalMonitorsCount];

            if (!GetPhysicalMonitorsFromHMONITOR(ptr, _physicalMonitorsCount, _physicalMonitorArray))
            {
                MessageBox.Show("Cannot get physical monitor handle!", "Brightness Controller");
                throw new Exception("Cannot get physical monitor handle!");
            }
            _currentMonitorHandle = _physicalMonitorArray[0].hPhysicalMonitor;

            if (!GetMonitorBrightness(_currentMonitorHandle, ref _minValue, ref _currentValue, ref _maxValue))
            {
                MessageBox.Show("Cannot get monitor brightness!", "Brightness Controller");
                throw new Exception("Cannot get monitor brightness!");
            }
            return getBrightnessStat();
        }

        public uint[] getBrightnessStat()
        {
            GetMonitorBrightness(_currentMonitorHandle, ref _minValue, ref _currentValue, ref _maxValue);
            uint[] brightnessStat = { _minValue, _currentValue, _maxValue };
            return brightnessStat;
        }

        public BrightnessController(IntPtr windowHandle)
        {
            windowHandle_last = windowHandle;
            uint dwFlags = 2u;
            IntPtr ptr = MonitorFromWindow(windowHandle, dwFlags);
            if (!GetNumberOfPhysicalMonitorsFromHMONITOR(ptr, ref _physicalMonitorsCount))
            {
                MessageBox.Show("Cannot get monitor count!", "Brightness Controller");
                throw new Exception("Cannot get monitor count!");
            }
            _physicalMonitorArray = new PHYSICAL_MONITOR[_physicalMonitorsCount];

            if (!GetPhysicalMonitorsFromHMONITOR(ptr, _physicalMonitorsCount, _physicalMonitorArray))
            {
                MessageBox.Show("Cannot get phisical monitor handle!", "Brightness Controller");
                throw new Exception("Cannot get phisical monitor handle!");
            }
            _currentMonitorHandle = _physicalMonitorArray[0].hPhysicalMonitor;

            // Hard Code fix The ERROR_01
            //if (!GetMonitorBrightness(_currentMonitorHandle, ref _minValue, ref _currentValue, ref _maxValue))
            //{
            //    MessageBox.Show("Cannot get monitor brightness!", "Brightness Controller");
            //    throw new Exception("Cannot get monitor brightness!");
            //}
        }

        public void SetBrightness(int newValue)
        {
            newValue = Math.Min(newValue, Math.Max(0, newValue));
            _currentValue = (_maxValue - _minValue) * (uint)newValue / 100u + _minValue;
            SetMonitorBrightness(_currentMonitorHandle, _currentValue);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
            GC.Collect();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_physicalMonitorsCount > 0)
                {
                    DestroyPhysicalMonitors(_physicalMonitorsCount, ref _physicalMonitorArray);
                }
            }
        }
    }
}