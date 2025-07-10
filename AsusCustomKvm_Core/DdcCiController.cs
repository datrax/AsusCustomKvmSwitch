using System.Runtime.InteropServices;

public class DdcCiController
{
    [DllImport("Dxva2.dll", SetLastError = true)]
    private static extern bool GetNumberOfPhysicalMonitorsFromHMONITOR(IntPtr hMonitor, out uint number);

    [DllImport("Dxva2.dll", SetLastError = true)]
    private static extern bool GetPhysicalMonitorsFromHMONITOR(IntPtr hMonitor, uint count, [Out] PHYSICAL_MONITOR[] monitors);

    [DllImport("Dxva2.dll", SetLastError = true)]
    private static extern bool SetVCPFeature(IntPtr handle, byte code, uint value);

    [DllImport("User32.dll", SetLastError = true)]
    private static extern bool EnumDisplayMonitors(IntPtr hdc, IntPtr clip, MonitorEnumDelegate callback, IntPtr data);

    [DllImport("Dxva2.dll", SetLastError = true)]
    private static extern bool DestroyPhysicalMonitor(IntPtr handle);

    private delegate bool MonitorEnumDelegate(IntPtr hMonitor, IntPtr hdcMonitor, ref RECT rect, IntPtr data);

    [StructLayout(LayoutKind.Sequential)]
    private struct PHYSICAL_MONITOR
    {
        public IntPtr hPhysicalMonitor;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
        public string description;
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct RECT
    {
        public int left, top, right, bottom;
    }

    public static void SetVcp(byte vcpCode, uint value)
    {
        EnumDisplayMonitors(IntPtr.Zero, IntPtr.Zero,
            (IntPtr hMonitor, IntPtr hdcMonitor, ref RECT rect, IntPtr data) =>
            {
                if (!GetNumberOfPhysicalMonitorsFromHMONITOR(hMonitor, out uint count))
                    return true;

                PHYSICAL_MONITOR[] monitors = new PHYSICAL_MONITOR[count];
                if (!GetPhysicalMonitorsFromHMONITOR(hMonitor, count, monitors))
                    return true;

                //For simplicity sending to all monitors
                foreach (var monitor in monitors)
                {
                    Console.WriteLine($"➡ {monitor.description} — Set 0x{vcpCode:X2} = {value}");
                    SetVCPFeature(monitor.hPhysicalMonitor, vcpCode, value);
                    DestroyPhysicalMonitor(monitor.hPhysicalMonitor);
                }
                return true;
            },
        IntPtr.Zero);
    }
}
