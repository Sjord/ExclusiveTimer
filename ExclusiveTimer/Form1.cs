using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace ExclusiveTimer
{
    public partial class ExclusiveTimer : Form
    {
        private const int RowCount = 10;
        private const int RowHeight = 22;
        private int ActiveTimer = -1;
        private int[] TimerValues;
        private List<Label> Labels;
        private List<TextBox> TextBoxes;
        private string ExportFile;

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            ShowInTaskbar = false;
        }

        public void TrayIconClicked(object sender, EventArgs eventArgs)
        {
            this.Activate();
        }

        public ExclusiveTimer()
        {
            var trayIcon = new NotifyIcon();
            trayIcon.Text = "ExclusiveTimer";
            trayIcon.Icon = new Icon(SystemIcons.Application, 40, 40);
            trayIcon.Visible = true;
            trayIcon.Click += TrayIconClicked;
            trayIcon.DoubleClick += TrayIconClicked;


            ExportFile = @"D:\Logs\ExclusiveTimer-" + DateTime.Now.ToString("yyyy-MM-dd_HHmm") + ".txt";
            TimerValues = new int[RowCount];
            Labels = new List<Label>();
            TextBoxes = new List<TextBox>();
            InitializeComponent();

            for (int i = 0; i < RowCount; i++)
            {
                TimerValues[i] = 0;
                var textbox = new TextBox()
                {
                    Width = 200,
                    Top = i * RowHeight,
                    Left = 0,
                    Height = RowHeight,
                };
                this.Controls.Add(textbox);
                TextBoxes.Add(textbox);
                
                var button = new Button()
                {
                    Text = "Start",
                    Top = i * RowHeight,
                    Left = 200,
                    Width = 100,
                    Height = RowHeight,
                    Tag = i,
                };
                button.Click += OnStartButtonClicked;
                this.Controls.Add(button);

                var label = new Label() {
                    Text = "0:00",
                    Top = i * RowHeight,
                    Left = 350,
                    Height = RowHeight,
                };
                Labels.Add(label);
                this.Controls.Add(label);
                
                this.Height = (1 + RowCount) * RowHeight;
                this.Width = 400;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (ActiveTimer != -1) {
                TimerValues[ActiveTimer]++;
                this.Labels[ActiveTimer].Text = string.Format("{0}:{1:00}", TimerValues[ActiveTimer] / 60, TimerValues[ActiveTimer] % 60);
            }
        }

        private void Export() {
            var writer = new StreamWriter(ExportFile);
            for (int i = 0; i < RowCount; i++) {
                writer.WriteLine(string.Format("{0} {1}:{2:00}", TextBoxes[i].Text.PadRight(40), TimerValues[i] / 60, TimerValues[i] % 60));
            }
            writer.Close();
        }

        protected void OnStartButtonClicked(object sender, EventArgs e)
        {
            var button = (Button) sender;
            var index = (int) button.Tag;
            ActiveTimer = index;
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            Export();
        }
    }
}
