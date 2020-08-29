
using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace CPUUsageMonitor
{
    public partial class GetCpuChart : Form
    {
        private SettingsMng oConfigMng = new SettingsMng();
        int usage;
        int usage2;
        int x = 1;
        protected PerformanceCounter countCpu = new PerformanceCounter("Processor", "% Processor Time", "_Total");
        protected PerformanceCounter countRAM = new PerformanceCounter("Memory", "Available MBytes");

        public GetCpuChart()
        {
            InitializeComponent();

            oConfigMng.LoadConfig();
            if (oConfigMng.Config.ToolTheme == "dark")
            {
                this.BackColor = Color.FromArgb(38, 38, 38);
                this.ForeColor = Color.FromArgb(250, 232, 232);
                cpuUsageChart.BackColor = Color.FromArgb(38, 38, 38);
                cpuUsageChart.ForeColor = Color.FromArgb(250, 232, 232);
            }

            timerMain.Tick += new EventHandler(timerMain_Tick);
            timerMain.Start();

            this.cpuUsageChart.Series.Clear();
            this.cpuUsageChart.Palette = ChartColorPalette.SeaGreen;
            this.cpuUsageChart.Titles.Add("CPU Usage and RAM Usage");

            // CPU
            Series series = this.cpuUsageChart.Series.Add("CPU Usage");
            cpuUsageChart.Series[0].ChartType = SeriesChartType.FastLine;

            series.Points.Add(0);

            cpuUsageChart.Series[0].YAxisType = AxisType.Primary;
            cpuUsageChart.Series[0].YValueType = ChartValueType.Int32;
            cpuUsageChart.Series[0].IsXValueIndexed = false;

            cpuUsageChart.ResetAutoValues();

            cpuUsageChart.ChartAreas[0].AxisY.Maximum = 100; // Max Y 
            cpuUsageChart.ChartAreas[0].AxisY.Minimum = 0;
            cpuUsageChart.ChartAreas[0].AxisX.Enabled = AxisEnabled.False;
            cpuUsageChart.ChartAreas[0].AxisY.Title = "Moto Boot Logo Maker Process";
            cpuUsageChart.ChartAreas[0].AxisY.IntervalAutoMode = IntervalAutoMode.VariableCount;

            // RAM
            Series series2 = this.cpuUsageChart.Series.Add("RAM Usage");
            series2.Points.Add(0);
            cpuUsageChart.Series["RAM Usage"].ChartType = SeriesChartType.FastLine;
            cpuUsageChart.Series["RAM Usage"].YAxisType = AxisType.Secondary;
            cpuUsageChart.Series["RAM Usage"].YValueType = ChartValueType.Int32;
            cpuUsageChart.Series["RAM Usage"].IsXValueIndexed = false;

            if (oConfigMng.Config.ToolTheme == "dark")
            {
                this.cpuUsageChart.ChartAreas[0].AxisY.LabelStyle.ForeColor = Color.FromArgb(250, 232, 232);
                this.cpuUsageChart.ChartAreas[0].AxisY.LineColor = Color.FromArgb(250, 232, 232);
                this.cpuUsageChart.ChartAreas[0].AxisY.LabelStyle.ForeColor = Color.FromArgb(250, 232, 232);

                cpuUsageChart.Legends[0].ForeColor = Color.FromArgb(250, 232, 232);
                cpuUsageChart.Legends[0].BackColor = Color.FromArgb(38, 38, 38);

                cpuUsageChart.Series[0].BackSecondaryColor = Color.FromArgb(250, 232, 232);

                cpuUsageChart.ChartAreas[0].BackColor = Color.FromArgb(38, 38, 38);
                cpuUsageChart.ChartAreas[0].AxisX.MajorGrid.LineColor = Color.FromArgb(250, 232, 232);
                cpuUsageChart.ChartAreas[0].AxisY.MajorGrid.LineColor = Color.FromArgb(250, 232, 232);

                this.cpuUsageChart.ChartAreas[0].AxisX.LabelStyle.ForeColor = Color.FromArgb(250, 232, 232);
                this.cpuUsageChart.ChartAreas[0].AxisX.LineColor = Color.FromArgb(250, 232, 232);
                this.cpuUsageChart.ChartAreas[0].AxisX.LabelStyle.ForeColor = Color.FromArgb(250, 232, 232);
            }
        }

        static ulong GetTotalMemoryInBytes()
        {
            return new Microsoft.VisualBasic.Devices.ComputerInfo().TotalPhysicalMemory;
        }

        public ulong InstalledRam { get; set; }
        
        private void timerMain_Tick(object sender, EventArgs e)
        {
            usage = Convert.ToInt32(countCpu.NextValue());
            usage2 = Convert.ToInt32(countRAM.NextValue());
            cpuUsageChart.Series["CPU Usage"].Points.AddXY(x, usage);
            cpuUsageChart.Series["RAM Usage"].Points.AddXY(x, usage2);
            proVal.Text = Convert.ToString(usage.ToString()) + "%";
            InstalledRam = GetTotalMemoryInBytes();
            var totalram = Convert.ToInt32(InstalledRam / (1024 * 1024 * 1024));
            labelRAM.Text = Convert.ToString(usage2.ToString()) + "MB" + " of " + totalram + "GB";
            x++;

            if (cpuUsageChart.ChartAreas[0].AxisX.Maximum > cpuUsageChart.ChartAreas[0].AxisX.ScaleView.Size)
            {
                cpuUsageChart.ChartAreas[0].AxisX.ScaleView.Scroll(cpuUsageChart.ChartAreas[0].AxisX.Maximum);
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            timerMain.Stop();
        }
    }
}
