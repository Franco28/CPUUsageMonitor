
using System;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using Org.Mentalis.Utilities;
using System.Threading;
using System.Drawing;

namespace CPUUsageMonitor
{
    public partial class GetCpuChart : Form
    {
        bool iscontinue = true;
        private static CpuUsage cpu;
        public GetCpuChart()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.cpuUsageChart.Series.Clear();
            this.cpuUsageChart.Palette = ChartColorPalette.SeaGreen;

            this.cpuUsageChart.Titles.Add("CPU Usage");

            Series series = this.cpuUsageChart.Series.Add("CPU Usage");
            cpuUsageChart.Series[0].ChartType = SeriesChartType.FastLine;

            series.Points.Add(0);

            cpuUsageChart.Series[0].YAxisType = AxisType.Primary;
            cpuUsageChart.Series[0].YValueType = ChartValueType.Int32;
            cpuUsageChart.Series[0].IsXValueIndexed = false;

            cpuUsageChart.ResetAutoValues();
            cpuUsageChart.ChartAreas[0].AxisY.Maximum = 100;//Max Y 
            cpuUsageChart.ChartAreas[0].AxisY.Minimum = 0;
            cpuUsageChart.ChartAreas[0].AxisX.Enabled = AxisEnabled.False;
            cpuUsageChart.ChartAreas[0].AxisY.Title = "CPU usage %";
            cpuUsageChart.ChartAreas[0].AxisY.IntervalAutoMode = IntervalAutoMode.VariableCount;

            populateCPUInfo();
        }

        private void populateCPUInfo()
        {
            try
            {
                cpu = CpuUsage.Create();
                Thread thread = new Thread(new ThreadStart(delegate ()
                {
                    try
                    {
                        while (iscontinue)
                        {
                            this.Invoke(new MethodInvoker(delegate ()
                            {
                                int process = cpu.Query();
                                proVal.Text = process.ToString() + "%";

                                if (process.Equals(90 <= 100))
                                {
                                    proVal.ForeColor = Color.Red;
                                }
                                else
                                {
                                    proVal.ForeColor = Color.Black;
                                }

                                cpuUsageChart.Series[0].Points.AddY(process);

                                if (cpuUsageChart.Series[0].Points.Count > 40)
                                    cpuUsageChart.Series[0].Points.RemoveAt(0);
                            }));

                            Thread.Sleep(450);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                    }
                }));

                thread.Priority = ThreadPriority.Highest;
                thread.IsBackground = true;
                thread.Start();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            iscontinue = false;
        }
        
    }
}
