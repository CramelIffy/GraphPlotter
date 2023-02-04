//#define TIMER

using GraphPlotter;
using ScottPlot;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace MainProcess
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        string fileName = "";
        string previousFileName = "";
        uint countGraphs = 0;
        uint countAnnotation = 0;
        uint countBurnTimes = 0;
        double[] maxTime = new double[2];
        bool checkBoxWarming = false;
        double graphXLeft = 0;
        double graphXRight = 0;
        private DataProcessing.DataProcessing data;
        bool isFileLoaded = false;

        private void Form1_Load(object sender, EventArgs e)
        {
            formsPlot1.Plot.Palette = Palette.ColorblindFriendly;
            formsPlot1.Plot.Style(Style.Monospace);
            formsPlot1.Plot.XAxis.ManualTickSpacing(0.1);
            formsPlot1.Plot.XAxis.MajorGrid(true, lineWidth: 2);
            formsPlot1.Plot.XAxis.MinorGrid(true);
            formsPlot1.Plot.YAxis.MajorGrid(true, lineWidth: 2);

            var impulseAno = formsPlot1.Plot.AddAnnotation("Total Impulse", -10, 10 + 50 * countAnnotation);
            impulseAno.Font.Size = 28;
            impulseAno.BackgroundColor = Color.FromArgb(25, Color.Black);
            impulseAno.Shadow = false;

            countAnnotation++;

            formsPlot1.Refresh();

            toolStripStatus.Text = "Standby";
        }

        private void Plot_Click(object sender, EventArgs e)
        {
#if TIMER
            var sw = new Stopwatch();
            sw.Start();
#endif

            if (fileName == "")
            {
                if (MessageBox.Show("ファイルが読み込まれていません。\n読み込みますか？", "注意", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    selectFile.PerformClick();
                }
                else
                {
                    return;
                }
            }

            toolStripStatus.Text = "Determined the file to be read";
            Application.DoEvents();

            if (isFileLoaded)
            {
                data = new DataProcessing.DataProcessing(this, (uint)(timeColumnNum.Value - 1), (uint)(dataColumnNum.Value - 1));
                data.SetFile(fileName);

                switch (data.error)
                {
                    case "E000":
                        break;
                    case "E001":
                        MessageBox.Show("数値以外のデータが含まれています。\n正しく読み込めない場合があります。", "注意", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        Application.DoEvents();
                        break;
                    case "E002":
                        MessageBox.Show("読み込んだファイルの行の数とデータの要素数が一致しません。\n正しく読み込めない場合があります。", "注意", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        break;
                    case "E003":
                        MessageBox.Show("ファイルが開けません。別のプロセスによって使用されている可能性があります。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        toolStripStatus.Text = "ERROR";
                        Application.DoEvents();
                        graphInit.PerformClick();
                        return;
                    case "E004":
                        MessageBox.Show("列指定エラー\n存在しない列を指定しています。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        toolStripStatus.Text = "ERROR";
                        Application.DoEvents();
                        graphInit.PerformClick();
                        return;
                    case "E005":
                        MessageBox.Show("ファイルが開けません。ファイルサイズが大きすぎます。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        toolStripStatus.Text = "ERROR";
                        Application.DoEvents();
                        graphInit.PerformClick();
                        return;
                }

                toolStripStatus.Text = "The file has been loaded.";
                Application.DoEvents();
            }
            else isFileLoaded = true;

            if (readAsMs.Checked) data.TimeConvert(0.001);

            data.DataCropping((double)skipTime.Value * 0.001, (double)cutoffTime.Value * 0.001);

            if (readAsVol.Checked) data.SRegressionLine(@"./CalibrationData.csv");
            toolStripStatus.Text = "Data cropping";
            Application.DoEvents();

            data.CheckTimeData(fileName != previousFileName);

            do
            {
                toolStripStatus.Text = "Denoised data generating";
                Application.DoEvents();

                data.GetFilterThrustList(21, 4);

                data.GetOffset();

                if (offsetRemoval.Checked)
                {
                    toolStripStatus.Text = "Deoffsetting";
                    Application.DoEvents();

                    data.thrustList = data.thrustList.Select(i => i - data.offset).ToList();
                    data.filteredThrustList = data.filteredThrustList.Select(i => i - data.offset).ToList();

                    data.offset = 0;
                }


                toolStripStatus.Text = "Burn time estimating";
                Application.DoEvents();

                data.GetTimesIndex(igniThreshold.Value * 0.01, burnoutThreshold.Value * 0.01, offsetRemoval.Checked);

                switch (data.error)
                {
                    case "E010": break;
                    case "E011":
                        MessageBox.Show("燃焼開始前、燃焼終了後のデータのいずれかもしくはその両方が規定より少なすぎます。\n燃焼時間の推定に失敗した恐れがあります。\n燃焼時間の推定に失敗する可能性があります。", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        break;
                    case "E012":
                        toolStripStatus.Text = "Data skip time estimation complete. Rerun.";
                        Application.DoEvents();
                        MessageBox.Show("燃焼時間の推定に失敗しました。\nSkipする時間を自動調整して再挑戦します。", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        data.DataCropping((double)skipTime.Value * 0.001, 0.0);
                        break;
                    case "E013":
                        MessageBox.Show("燃焼時間の推定に失敗しました。\nデータの一部に異常が存在する可能性があります。\nSkipやCutoffの数値を調整して異常部分を回避してください。", "警告", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        toolStripStatus.Text = "ERROR";
                        Application.DoEvents();
                        graphInit.PerformClick();
                        return;
                }
            } while (data.error == "E012");

            if (countGraphs == 0) maxTime[0] = data.timeList[data.thrustList.IndexOf(data.thrustList.Max())];
            else maxTime[1] = data.timeList[data.thrustList.IndexOf(data.thrustList.Max())];

            if (SetIgnitionTimeZero.Checked)
            {
                double ignitionTime = data.timeList[data.ignitionTimeIndex];
                data.timeList = data.timeList.Select(i => i - ignitionTime).ToList();
            }

            if (countGraphs > 0 && isAlignMax.Checked)
            {
                data.timeList = data.timeList.Select(i => i + maxTime[0] - maxTime[1]).ToList();
            }

            data.thrustMax = data.thrustList.Max();
            data.timeWhenThrustMax = data.timeList[data.thrustList.IndexOf(data.thrustMax)];

            Parallel.Invoke(
                () => data.PeakProtectionIntensity(80, peakProtectionIntensity.Value),
                () => data.GetImpulse(data.ignitionTimeIndex, data.burnOutTimeIndex),
                () => data.GetAverage(data.ignitionTimeIndex, data.burnOutTimeIndex)
                );

            var unfilteredThrustList = data.thrustList;

            if (denoise.Checked || DenoisedVsRaw.Checked)
                data.thrustList = data.thrustList.Select((x, i) => x * data.peakProtection[i] + data.filteredThrustList[i] * (1 - data.peakProtection[i])).ToList();

            ////////////////////////////////////////////////////////////////データ加工終了。以下描画処理////////////////////////////////////////////////////////////////

            toolStripStatus.Text = "Graph is plotting";
            Application.DoEvents();

            string graphTitle = "";

            if (previousFileName != fileName)
            {
                var fs = new GraphName();
                while (graphTitle == "")
                {
                    DialogResult dr = fs.ShowDialog();
                    graphTitle = fs.value;
                    if (graphTitle == "")
                        MessageBox.Show("グラフの名前を入力してください。", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                fs.Dispose();

                if (isPlotMax.Checked && !showPeakProtectionIntensity.Checked)
                {
                    var maxMarker = formsPlot1.Plot.AddMarker(data.timeWhenThrustMax, data.thrustMax, MarkerShape.filledCircle, 5, Color.Red);
                    maxMarker.Text = "Max(" + graphTitle + "): " + data.thrustMax.ToString("F3") + " N";
                    maxMarker.TextFont.Color = Color.Black;
                    maxMarker.TextFont.Alignment = Alignment.MiddleLeft;
                    maxMarker.TextFont.Size = 28;
                }

                if (isPlotImpulse.Checked && !showPeakProtectionIntensity.Checked)
                {
                    var impulseAno = formsPlot1.Plot.AddAnnotation(graphTitle + ": " + data.impulse.ToString("F3") + " N⋅s", -10, 10 + 50 * countAnnotation);
                    impulseAno.Font.Size = 28;
                    impulseAno.BackgroundColor = Color.FromArgb(25, Color.Black);
                    impulseAno.Shadow = false;

                    countAnnotation++;
                }
            }

            var graphColor = Color.FromArgb(255, (int)(15 + countGraphs * 15), (int)(20 + countGraphs * 20), (int)(25 + countGraphs * 25));

            if (!showPeakProtectionIntensity.Checked)
            {
                if (DenoisedVsRaw.Checked)
                {
                    var graphDenoisedVsRaw = formsPlot1.Plot.AddSignalXY(data.timeList.ToArray(), unfilteredThrustList.ToArray(), label: graphTitle + "(original)");
                    graphDenoisedVsRaw.LineWidth = 4;
                    graphDenoisedVsRaw.LineColor = Color.FromArgb(170, graphColor.R + 100, graphColor.G + 100, graphColor.B + 100);
                }
                var graph = formsPlot1.Plot.AddSignalXY(data.timeList.ToArray(), data.thrustList.ToArray(), label: graphTitle + (DenoisedVsRaw.Checked ? "(denoised)" : ""));
                graph.LineWidth = 3;
                graph.LineColor = graphColor;

                formsPlot1.Plot.YAxis.Label("Thrust [N]");
                formsPlot1.Plot.XAxis.Label("Time [s]");
            }
            else
            {
                var peakProtectionIntensityGraph = formsPlot1.Plot.AddSignalXY(data.timeList.ToArray(), data.peakProtection.ToArray(), label: "Peak protection intensity of the " + graphTitle);
                peakProtectionIntensityGraph.YAxisIndex = 1;
                peakProtectionIntensityGraph.XAxisIndex = 0;

                formsPlot1.Plot.YAxis2.Label("Peak protection intensity");
                formsPlot1.Plot.XAxis.Label("Time [s]");
                formsPlot1.Plot.YAxis2.Ticks(true);
            }

            if (previousFileName != fileName)
            {
                if (isPlotAverageThrust.Checked && !showPeakProtectionIntensity.Checked)
                {

                    var averageThrustLine = formsPlot1.Plot.AddHorizontalLine(data.averageThrust);
                    averageThrustLine.LineWidth = 1;
                    averageThrustLine.PositionLabel = true;
                    averageThrustLine.Color = graphColor;
                    averageThrustLine.LineStyle = LineStyle.Dash;
                    averageThrustLine.PositionLabelBackground = graphColor;
                }

                if (isPlotBurnTime.Checked && !showPeakProtectionIntensity.Checked)
                {
                    double margin = (data.thrustList.Max() - data.offset) * 0.06 * countBurnTimes;
                    var burnTimeAno = formsPlot1.Plot.AddBracket(data.timeList[data.ignitionTimeIndex], -margin, data.timeList[data.burnOutTimeIndex], -margin, graphTitle + ": " + (data.timeList[data.burnOutTimeIndex] - data.timeList[data.ignitionTimeIndex]).ToString("F2") + " s");
                    burnTimeAno.Font.Size = 22;

                    if (countGraphs == 0)
                    {
                        var timeListForFill = data.timeList
                            .Skip(data.ignitionTimeIndex)
                            .Take(data.burnOutTimeIndex - data.ignitionTimeIndex)
                            .ToArray();

                        var thrustListForFill = data.thrustList
                            .Skip(data.ignitionTimeIndex)
                            .Take(data.burnOutTimeIndex - data.ignitionTimeIndex)
                            .ToArray();

                        if (data.burnOutTimeIndex != data.ignitionTimeIndex)
                            formsPlot1.Plot.AddFill(timeListForFill, thrustListForFill, 0, Color.FromArgb(0x30000000));
                    }

                    countBurnTimes++;
                }
            }

            countGraphs++;

            graphXLeft = data.timeList[data.ignitionTimeIndex] - (data.timeList[data.burnOutTimeIndex] - data.timeList[data.ignitionTimeIndex]) * 0.04;
            graphXRight = data.timeList[data.burnOutTimeIndex] + (data.timeList[data.burnOutTimeIndex] - data.timeList[data.ignitionTimeIndex]) * 0.08;

            formsPlot1.Plot.Legend();

            double graphMaxY = data.thrustList.Max();
            formsPlot1.Plot.SetAxisLimitsX(graphXLeft, graphXRight);
            formsPlot1.Plot.AxisAutoY();
            try
            {
                formsPlot1.Refresh();
            }
            catch (InvalidOperationException)
            {
                MessageBox.Show("原因不明なエラーによりグラフ描画に失敗しました。", "警告", MessageBoxButtons.OK, MessageBoxIcon.Error);
                graphInit.PerformClick();
                toolStripStatus.Text = "ERROR";
                Application.DoEvents();
                return;
            }

            previousFileName = fileName;

            toolStripStatus.Text = "Done";

#if TIMER
            sw.Stop();
            MessageBox.Show(((double)sw.ElapsedMilliseconds / 1000).ToString("F3"));
#endif
        }

        private void Threshold_Scroll(object sender, EventArgs e)
        {
            label2.Text = (peakProtectionIntensity.Value / 2.0).ToString("F1");
        }

        private void IgniThreshold_Scroll(object sender, EventArgs e)
        {
            label3.Text = (igniThreshold.Value * 0.01).ToString("F2");
        }

        private void BurnOutThreshold_Scroll(object sender, EventArgs e)
        {
            label5.Text = (burnoutThreshold.Value * 0.01).ToString("F2");
        }

        private void OpenFile_Click(object sender, EventArgs e)
        {

            var ofd = new OpenFileDialog
            {
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory),
                Filter = "CSVファイル(*.csv)|*.csv|テキストファイル(*.txt)|*.txt|ログファイル(*.log)|*.log",
                FilterIndex = 1,
                Title = "開くファイルを選択してください",
                RestoreDirectory = true
            };

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                fileName = ofd.FileName;

                data = new DataProcessing.DataProcessing(this, (uint)(timeColumnNum.Value - 1), (uint)(dataColumnNum.Value - 1));
                data.SetFile(fileName);

                switch (data.error)
                {
                    case "E000":
                        break;
                    case "E001":
                        MessageBox.Show("数値以外のデータが含まれています。\n正しく読み込めない場合があります。", "注意", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        Application.DoEvents();
                        break;
                    case "E002":
                        MessageBox.Show("読み込んだファイルの行の数とデータの要素数が一致しません。\n正しく読み込めない場合があります。", "注意", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        break;
                    case "E003":
                        MessageBox.Show("ファイルが開けません。別のプロセスによって使用されている可能性があります。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        toolStripStatus.Text = "ERROR";
                        Application.DoEvents();
                        graphInit.PerformClick();
                        return;
                    case "E004":
                        MessageBox.Show("列指定エラー\n存在しない列を指定しています。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        toolStripStatus.Text = "ERROR";
                        Application.DoEvents();
                        graphInit.PerformClick();
                        return;
                    case "E005":
                        MessageBox.Show("ファイルが開けません。ファイルサイズが大きすぎます。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        toolStripStatus.Text = "ERROR";
                        Application.DoEvents();
                        graphInit.PerformClick();
                        return;
                }

                toolStripStatus.Text = "The file has been loaded.";
                Application.DoEvents();

                skipTime.Value = 0;
            }

            ofd.Dispose();

            isFileLoaded = false;
        }

        private void SaveFigure_Click(object sender, EventArgs e)
        {
            string saveFilePath = "";
            var fbDialog = new FolderBrowserDialog
            {
                SelectedPath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory),
                ShowNewFolderButton = true
            };
            if (fbDialog.ShowDialog() == DialogResult.OK)
            {
                saveFilePath = fbDialog.SelectedPath;
            }
            else
            {
                fbDialog.Dispose();
                return;
            }

            fbDialog.Dispose();

            var di = new DirectoryInfo(saveFilePath);
            var tagName = "figure";
            var max = di.GetFiles(tagName + "_???.png")                         // パターンに一致するファイルを取得する
                .Select(fi => Regex.Match(fi.Name, @"(?i)_(\d{3})\.png$"))      // ファイルの中で数値のものを探す
                .Where(m => m.Success)                                          // 該当するファイルだけに絞り込む
                .Select(m => Int32.Parse(m.Groups[1].Value))                    // 数値を取得する
                .DefaultIfEmpty(0)                                              // １つも該当しなかった場合は 0 とする
                .Max();                                                         // 最大値を取得する
            var fileName = String.Format("{0}_{1:d3}.png", tagName, max + 1);

            formsPlot1.Plot.SaveFig(saveFilePath + @"\" + fileName);
            Process.Start("Explorer.exe", saveFilePath);
        }

        private void Init()
        {
            countGraphs = 0;
            maxTime = new double[2];
            countAnnotation = 1;
            countBurnTimes = 0;
            skipTime.Value = 0;
            cutoffTime.Value = 0;
            previousFileName = "";
            isFileLoaded = false;
            data = new DataProcessing.DataProcessing(this, (uint)(timeColumnNum.Value - 1), (uint)(dataColumnNum.Value - 1));
            if (fileName != "") data.SetFile(fileName);
        }

        private void GraphInit_Click(object sender, EventArgs e)
        {
            formsPlot1.Plot.YAxis.Label("");
            formsPlot1.Plot.YAxis2.Label("");
            formsPlot1.Plot.YAxis2.Ticks(false);

            formsPlot1.Plot.Clear();

            var impulseAno = formsPlot1.Plot.AddAnnotation("Total Impulse", -10, 10);
            impulseAno.Font.Size = 28;
            impulseAno.BackgroundColor = Color.FromArgb(25, Color.Black);
            impulseAno.Shadow = false;

            formsPlot1.Refresh();

            Init();
        }

        private void GraphScaleInit_Click(object sender, EventArgs e)
        {
            formsPlot1.Plot.AxisAutoY();
            if (graphXLeft == graphXRight && graphXLeft == 0) formsPlot1.Plot.AxisAutoX();
            else formsPlot1.Plot.SetAxisLimitsX(graphXLeft, graphXRight);
            formsPlot1.Refresh();
        }

        void ShowCheckBoxWarming()
        {
            if (!checkBoxWarming)
            {
                MessageBox.Show("チャックボックスの変更は今まで描画されたグラフには適応されません。\n" +
                 "変更したい場合は初期化してください。", "注意", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                checkBoxWarming = true;
            }

        }

        private void IsAlignMax_CheckedChanged(object sender, EventArgs e)
        {
            ShowCheckBoxWarming();
            if (SetIgnitionTimeZero.Checked) SetIgnitionTimeZero.Checked = false;
        }

        private void SetIgnitionTimeZero_CheckedChanged(object sender, EventArgs e)
        {
            ShowCheckBoxWarming();
            if (isAlignMax.Checked) isAlignMax.Checked = false;
        }

        private void IsPlotMax_CheckedChanged(object sender, EventArgs e)
        {
            ShowCheckBoxWarming();
        }

        private void Denoise_CheckedChanged(object sender, EventArgs e)
        {
            ShowCheckBoxWarming();
        }

        private void ShowPeakProtectionIntensity_CheckedChanged(object sender, EventArgs e)
        {
            ShowCheckBoxWarming();
        }

        private void Deoffset_CheckedChanged(object sender, EventArgs e)
        {
            ShowCheckBoxWarming();
        }

        private void IsPlotImpulse_CheckedChanged(object sender, EventArgs e)
        {
            ShowCheckBoxWarming();
        }

        private void RreadAsVol_CheckedChanged(object sender, EventArgs e)
        {
            ShowCheckBoxWarming();
        }

        private void ReadAsMs_CheckedChanged(object sender, EventArgs e)
        {
            ShowCheckBoxWarming();
        }

        private void IsPlotAverageThrust_CheckedChanged(object sender, EventArgs e)
        {
            ShowCheckBoxWarming();
        }

        private void ShowReadme_Click(object sender, EventArgs e)
        {
            var startInfo = new ProcessStartInfo("https://github.com/CramelIffy/GraphPlotter")
            {
                UseShellExecute = true
            };
            Process.Start(startInfo);
        }

        private void ToolStripStatus_Click(object sender, EventArgs e)
        {

        }
    }
}