namespace AsusCustomKvm_FormClient;
partial class MainForm
{
    /// <summary>
    ///  Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    ///  Clean up any resources being used.
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
    ///  Required method for Designer support - do not modify
    ///  the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
        autostartCheckbox = new CheckBox();
        detectButton = new Button();
        label1 = new Label();
        usbHubTextField = new TextBox();
        contextMenuStrip1 = new ContextMenuStrip(components);
        exitToolStripMenuItem = new ToolStripMenuItem();
        trayIcon = new NotifyIcon(components);
        onConnectedInput = new ComboBox();
        onDisconnectedInput = new ComboBox();
        testConnectedButton = new Button();
        testDisconnectedButton = new Button();
        label2 = new Label();
        label3 = new Label();
        contextMenuStrip1.SuspendLayout();
        SuspendLayout();
        // 
        // autostartCheckbox
        // 
        autostartCheckbox.AutoSize = true;
        autostartCheckbox.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
        autostartCheckbox.Location = new Point(319, 12);
        autostartCheckbox.Name = "autostartCheckbox";
        autostartCheckbox.Size = new Size(93, 25);
        autostartCheckbox.TabIndex = 0;
        autostartCheckbox.Text = "Autostart";
        autostartCheckbox.UseVisualStyleBackColor = true;
        autostartCheckbox.CheckedChanged += autostartCheckbox_CheckedChanged;
        // 
        // detectButton
        // 
        detectButton.Location = new Point(319, 51);
        detectButton.Name = "detectButton";
        detectButton.Size = new Size(93, 35);
        detectButton.TabIndex = 2;
        detectButton.Text = "Detect";
        detectButton.UseVisualStyleBackColor = true;
        detectButton.Click += detectButton_Click;
        // 
        // label1
        // 
        label1.AutoSize = true;
        label1.Font = new Font("Segoe UI", 15.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
        label1.Location = new Point(12, 6);
        label1.Name = "label1";
        label1.Size = new Size(99, 30);
        label1.TabIndex = 3;
        label1.Text = "Your Hub";
        // 
        // usbHubTextField
        // 
        usbHubTextField.Enabled = false;
        usbHubTextField.Font = new Font("Segoe UI", 15.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
        usbHubTextField.Location = new Point(12, 51);
        usbHubTextField.Name = "usbHubTextField";
        usbHubTextField.Size = new Size(288, 35);
        usbHubTextField.TabIndex = 4;
        // 
        // contextMenuStrip1
        // 
        contextMenuStrip1.Items.AddRange(new ToolStripItem[] { exitToolStripMenuItem });
        contextMenuStrip1.Name = "contextMenuStrip1";
        contextMenuStrip1.Size = new Size(103, 28);
        // 
        // exitToolStripMenuItem
        // 
        exitToolStripMenuItem.Name = "exitToolStripMenuItem";
        exitToolStripMenuItem.Size = new Size(102, 24);
        exitToolStripMenuItem.Text = "Exit";
        exitToolStripMenuItem.Click += exitToolStripMenuItem_Click;
        // 
        // trayIcon
        // 
        trayIcon.ContextMenuStrip = contextMenuStrip1;
        trayIcon.Icon = (Icon)resources.GetObject("trayIcon.Icon");
        trayIcon.Text = "notifyIcon1";
        trayIcon.Visible = true;
        trayIcon.MouseDoubleClick += trayIcon_MouseDoubleClick;
        // 
        // onConnectedInput
        // 
        onConnectedInput.FormattingEnabled = true;
        onConnectedInput.Location = new Point(12, 121);
        onConnectedInput.Name = "onConnectedInput";
        onConnectedInput.Size = new Size(121, 28);
        onConnectedInput.TabIndex = 6;
        onConnectedInput.TextChanged += onConnectedBox_TextChanged;
        // 
        // onDisconnectedInput
        // 
        onDisconnectedInput.FormattingEnabled = true;
        onDisconnectedInput.Location = new Point(179, 129);
        onDisconnectedInput.Name = "onDisconnectedInput";
        onDisconnectedInput.Size = new Size(121, 28);
        onDisconnectedInput.TabIndex = 7;
        onDisconnectedInput.TextChanged += onDisconnectedInput_TextChanged;
        // 
        // testConnectedButton
        // 
        testConnectedButton.Location = new Point(12, 163);
        testConnectedButton.Name = "testConnectedButton";
        testConnectedButton.Size = new Size(121, 29);
        testConnectedButton.TabIndex = 8;
        testConnectedButton.Text = "Test";
        testConnectedButton.UseVisualStyleBackColor = true;
        testConnectedButton.Click += testButton_Click;
        // 
        // testDisconnectedButton
        // 
        testDisconnectedButton.Location = new Point(179, 163);
        testDisconnectedButton.Name = "testDisconnectedButton";
        testDisconnectedButton.Size = new Size(121, 29);
        testDisconnectedButton.TabIndex = 9;
        testDisconnectedButton.Text = "Test";
        testDisconnectedButton.UseVisualStyleBackColor = true;
        testDisconnectedButton.Click += testButton_Click;
        // 
        // label2
        // 
        label2.AutoSize = true;
        label2.Location = new Point(179, 98);
        label2.Name = "label2";
        label2.Size = new Size(129, 20);
        label2.TabIndex = 10;
        label2.Text = "Hub disconnected";
        // 
        // label3
        // 
        label3.AutoSize = true;
        label3.Location = new Point(12, 98);
        label3.Name = "label3";
        label3.Size = new Size(110, 20);
        label3.TabIndex = 11;
        label3.Text = "Hub connected";
        // 
        // MainForm
        // 
        AutoScaleDimensions = new SizeF(8F, 20F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(433, 205);
        Controls.Add(label3);
        Controls.Add(label2);
        Controls.Add(testDisconnectedButton);
        Controls.Add(testConnectedButton);
        Controls.Add(onDisconnectedInput);
        Controls.Add(onConnectedInput);
        Controls.Add(usbHubTextField);
        Controls.Add(label1);
        Controls.Add(detectButton);
        Controls.Add(autostartCheckbox);
        Icon = (Icon)resources.GetObject("$this.Icon");
        Name = "MainForm";
        Text = "Form1";
        FormClosing += MainFormClosing;
        Resize += MainFormResized;
        contextMenuStrip1.ResumeLayout(false);
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private CheckBox autostartCheckbox;
    private Button detectButton;
    private Label label1;
    private TextBox usbHubTextField;
    private ContextMenuStrip contextMenuStrip1;
    private NotifyIcon trayIcon;
    private ComboBox onConnectedInput;
    private ComboBox onDisconnectedInput;
    private Button testConnectedButton;
    private Button testDisconnectedButton;
    private ToolStripMenuItem exitToolStripMenuItem;
    private Label label2;
    private Label label3;
}
