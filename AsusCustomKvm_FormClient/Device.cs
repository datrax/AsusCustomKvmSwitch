namespace AsusCustomKvm_FormClient;

public class Device
{
    public string Name { get; set; }
    public string Vid { get; set; }
    public string Pid { get; set; }

    public string Key => $"{Vid}&{Pid}";
}
