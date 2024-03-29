﻿using System;
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
        private int ActiveTimer = 0;
        private int[] TimerValues;
        private List<Label> Labels;
        private List<TextBox> TextBoxes;
        private string ExportFile;
        private NotifyIcon trayIcon;

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            //ShowInTaskbar = false;
        }

        public void TrayIconClicked(object sender, EventArgs eventArgs)
        {
            this.Activate();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            var myTrayIcon = trayIcon;
            this.trayIcon = null;
            myTrayIcon.Dispose();
        }

        public ExclusiveTimer()
        {
            ShowTrayIcon();

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
                    Width = 340,
                    Top = i * RowHeight,
                    Left = 0,
                    Height = RowHeight,
                    Tag = i
                };
                textbox.GotFocus += this.OnTextBoxGotFocus;
                this.Controls.Add(textbox);
                TextBoxes.Add(textbox);

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

        private void ShowTrayIcon()
        {
            this.trayIcon = new NotifyIcon();
            this.trayIcon.Text = "ExclusiveTimer";
            this.trayIcon.Icon = new Icon(SystemIcons.Application, 40, 40);
            this.trayIcon.Visible = true;
            this.trayIcon.Click += this.TrayIconClicked;
            this.trayIcon.DoubleClick += this.TrayIconClicked;
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
                writer.WriteLine(string.Format("{0} {1}:{2:00}", TextBoxes[i].Text.PadRight(60), TimerValues[i] / 60, TimerValues[i] % 60));
            }
            writer.Close();
        }

        protected void OnTextBoxGotFocus(object sender, EventArgs e)
        {
            var textBox = (TextBox) sender;
            var index = (int)textBox.Tag;
            ActiveTimer = index;
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            Export();
        }

        private void ExclusiveTimer_Load(object sender, EventArgs e)
        {

        }
    }
}
