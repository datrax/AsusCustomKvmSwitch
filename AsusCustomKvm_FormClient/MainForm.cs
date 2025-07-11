﻿using Microsoft.Win32;
using System.Diagnostics;
using System.Globalization;
using System.Management;
using System.Text.RegularExpressions;

namespace AsusCustomKvm_FormClient;

public partial class MainForm : Form
{
    private const string UserRegistryPath = "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run";
    private const string AutostartRegisterKeyName = "ASUS_KVM_client_autostart";

    private static readonly string[] HexOptions = Enumerable.Range(0, 0x14)
        .Select(i => $"0x{i:X2}")
        .ToArray();

    private static readonly HashSet<string> RecentlyHandledDisconnectedDeviceIds = new();
    private static readonly HashSet<string> RecentlyHandledConnectedDeviceIds = new();

    private static AppSettings Settings { get; set; } = AppSettings.ReadFromFile();
    private static bool DetectingHub { get; set; }

    public MainForm()
    {
        InitializeComponent();

        InitializeAutostartCheckbox();
        InitializeUsbWatchers();
        InitializeDeviceComboBoxes();
        InitializeInputValues();
    }

    private void InitializeAutostartCheckbox()
    {
        var rk = GetAutostartRegistryKey();
        autostartCheckbox.Checked = rk?.GetValue(AutostartRegisterKeyName) != null;
    }

    private void InitializeUsbWatchers()
    {
        var creationQuery = new WqlEventQuery(
            "SELECT * FROM __InstanceCreationEvent WITHIN 1 WHERE TargetInstance ISA 'Win32_PnPEntity'");
        var deletionQuery = new WqlEventQuery(
            "SELECT * FROM __InstanceDeletionEvent WITHIN 1 WHERE TargetInstance ISA 'Win32_PnPEntity'");

        var creationWatcher = new ManagementEventWatcher(creationQuery);
        var deletionWatcher = new ManagementEventWatcher(deletionQuery);

        creationWatcher.EventArrived += OnUsbDeviceConnected;
        deletionWatcher.EventArrived += OnUsbDeviceDisconnected;

        creationWatcher.Start();
        deletionWatcher.Start();
    }

    private void InitializeDeviceComboBoxes()
    {
        onConnectedInput.Items.AddRange(HexOptions);
        onDisconnectedInput.Items.AddRange(HexOptions);
    }

    private void InitializeInputValues()
    {
        if (Settings.HubDevice == null)
        {
            Show();
        }

        usbHubTextField.Text = Settings.HubDevice?.Key ?? "";
        onConnectedInput.Text = $"0x{Settings.OnConnectedVPCCode:X2}";
        onDisconnectedInput.Text = $"0x{Settings.OnDisconnectedVPCCode:X2}";
    }

    private static void OnUsbDeviceConnected(object sender, EventArrivedEventArgs e)
    {
        Debug.WriteLine($"connected {(RecentlyHandledConnectedDeviceIds.Count > 0
            ? string.Join(", ", RecentlyHandledConnectedDeviceIds)
            : "empty")}");
        var instance = (ManagementBaseObject)e.NewEvent["TargetInstance"];
        string deviceId = instance["DeviceID"]?.ToString();
        if (string.IsNullOrEmpty(deviceId)) return;

        var device = ExtractDeviceKeyFromId(deviceId);
        if (string.IsNullOrEmpty(device?.Key)) return;

        if (RecentlyHandledConnectedDeviceIds.Add(device.Key))
        {
            ScheduleDeviceRemoval(device.Key, RecentlyHandledConnectedDeviceIds);
        }
        else
        {
            return;
        }

        if (DetectingHub)
        {
            Settings.HubDevice = new Device
            {
                Name = device.Name,
                Pid = device.Pid,
                Vid = device.Vid,
            };

            Settings.SaveToFile();
            DetectingHub = false;
            return;
        }

        if (device.Key == Settings.HubDevice?.Key)
        {
            SetVcp(Settings.OnConnectedVPCCode);
        }
    }

    private static void OnUsbDeviceDisconnected(object sender, EventArrivedEventArgs e)
    {
        Debug.WriteLine($"connected {(RecentlyHandledDisconnectedDeviceIds.Count > 0
            ? string.Join(", ", RecentlyHandledDisconnectedDeviceIds)
            : "empty")}");
        var instance = (ManagementBaseObject)e.NewEvent["TargetInstance"];
        string deviceId = instance["DeviceID"]?.ToString();

        if (string.IsNullOrEmpty(deviceId)) return;

        var device = ExtractDeviceKeyFromId(deviceId);

        if (string.IsNullOrEmpty(device?.Key)) return;

        if (RecentlyHandledDisconnectedDeviceIds.Add(device.Key))
        {
            ScheduleDeviceRemoval(device.Key, RecentlyHandledDisconnectedDeviceIds);
        }
        else
        {
            return;
        }

        if (device.Key == Settings.HubDevice?.Key)
        {
            SetVcp(Settings.OnDisconnectedVPCCode);
        }
    }

    private static void SetVcp(uint code)
    {
        DdcCiController.SetVcp(0x60, code);
        Debug.WriteLine($"switch {code}");
    }

    private static async void ScheduleDeviceRemoval(string key, HashSet<string> set)
    {
        await Task.Delay(2000);
        set.Remove(key);

        Debug.WriteLine($"RecentlyHandledDisconnectedDeviceIds {RecentlyHandledDisconnectedDeviceIds.Count}");
        Debug.WriteLine($"RecentlyHandledConnectedDeviceIds {RecentlyHandledConnectedDeviceIds.Count}");
    }

    private static Device? ExtractDeviceKeyFromId(string deviceId)
    {
        var vidMatch = Regex.Match(deviceId, @"VID_[0-9A-Fa-f]{4}");
        var pidMatch = Regex.Match(deviceId, @"PID_[0-9A-Fa-f]{4}");

        if (vidMatch.Success && pidMatch.Success)
        {
            return new Device
            {
                Pid = pidMatch.Value,
                Vid = vidMatch.Value,
            };
        }

        return null;
    }

    private async void detectButton_Click(object sender, EventArgs e)
    {
        Settings.HubDevice = null;
        usbHubTextField.Text = "Detecting - Reconnect your hub!";
        ((Button)sender).Enabled = false;
        DetectingHub = true;

        var detectTask = Task.Run(() =>
        {
            while (DetectingHub) Thread.Sleep(1000);
        });

        var timeoutTask = Task.Delay(30000);
        var finishedTask = await Task.WhenAny(detectTask, timeoutTask);

        usbHubTextField.Text = finishedTask == timeoutTask
            ? "Try again"
            : Settings.HubDevice?.Key ?? "Not found";

        ((Button)sender).Enabled = true;
    }

    private void onConnectedBox_TextChanged(object sender, EventArgs e)
    {
        if (TryParseHex(onConnectedInput.Text, out uint value))
        {
            Settings.OnConnectedVPCCode = value;
            Settings.SaveToFile();
        }
    }

    private void onDisconnectedInput_TextChanged(object sender, EventArgs e)
    {
        if (TryParseHex(onDisconnectedInput.Text, out uint value))
        {
            Settings.OnDisconnectedVPCCode = value;
            Settings.SaveToFile();
        }
    }

    private bool TryParseHex(string text, out uint value)
    {
        value = 0;
        if (text.StartsWith("0x") && text.Length > 2)
        {
            return uint.TryParse(text.Substring(2), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out value);
        }
        return false;
    }

    private void testButton_Click(object sender, EventArgs e)
    {
        var button = (Button)sender;
        var code = button.Name.Contains("Disconnected")
            ? Settings.OnDisconnectedVPCCode
            : Settings.OnConnectedVPCCode;

        SetVcp(code);
    }

    private RegistryKey? GetAutostartRegistryKey()
    {
        return Registry.CurrentUser.OpenSubKey(UserRegistryPath, writable: true);
    }

    private void autostartCheckbox_CheckedChanged(object sender, EventArgs e)
    {
        using var rk = GetAutostartRegistryKey();
        if (autostartCheckbox.Checked)
        {
            rk?.SetValue(AutostartRegisterKeyName, $"\"{Application.ExecutablePath}\"");
        }
        else
        {
            rk?.DeleteValue(AutostartRegisterKeyName, false);
        }

        Settings?.SaveToFile();
    }

    private void MainFormClosing(object sender, FormClosingEventArgs e)
    {
        if (e.CloseReason == CloseReason.UserClosing)
        {
            e.Cancel = true;
            Hide();
        }
    }

    private void trayIcon_MouseDoubleClick(object sender, MouseEventArgs e)
    {
        Show();
        WindowState = FormWindowState.Normal;
    }

    private void exitToolStripMenuItem_Click(object sender, EventArgs e)
    {
        Close();
        Application.Exit();
    }

    private void MainFormResized(object sender, EventArgs e)
    {
        if (WindowState == FormWindowState.Minimized)
        {
            Hide();
        }
    }
}
