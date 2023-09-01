//#define TIMER

using GraphPlotter;
using ScottPlot;
using ScottPlot.Plottable;
using System.Data;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace MainProcess
{
    public partial class Form1 : Form
    {
        private enum GraphType : byte
        {
            maxMarker,
            impulseAno,
            graphDenoisedVsRaw,
            graph,
            peakProtectionIntensityGraph,
            averageThrustLine,
            burnTimeAno,
            fill
        }

        private string fileName;
        private string previousFileName;
        private uint countGraphs;
        private uint countAnnotation;
        private uint countBurnTimes;
        private double[] maxTime;
        private bool checkBoxWarming;
        private double graphXLeft;
        private double graphXRight;
        private DataProcessing.DataProcessing data;
        private bool isFileLoaded;
        private bool isGraphDrawn;
        private string graphTitle;
        private readonly bool[] isDeleteGraphs;
        private readonly byte howManyGraphTypes = (byte)Enum.GetValues(typeof(GraphType)).Length;
        private readonly List<object> graphs;

        public Form1()
        {
            InitializeComponent();
            fileName = "";
            countGraphs = 0;
            maxTime = new double[2];
            countBurnTimes = 0;
            skipTime.Value = 0;
            cutoffTime.Value = 0;
            previousFileName = "";
            isFileLoaded = false;
            isGraphDrawn = false;
            data = new DataProcessing.DataProcessing(this, (uint)(timeColumnNum.Value - 1), (uint)(dataColumnNum.Value - 1));
            countAnnotation = 0;
            checkBoxWarming = false;
            graphXLeft = 0;
            graphXRight = 0;
            graphTitle = "";
            isDeleteGraphs = new bool[howManyGraphTypes];
            for (int i = 0; i < howManyGraphTypes; i++)
            {
                isDeleteGraphs[i] = false;
            }
            graphs = new List<object>()
            {
                new Stack<MarkerPlot?>(),
                new Stack<Annotation?>(),
                new Stack<SignalPlotXY?>(),
                new Stack<SignalPlotXY?>(),
                new Stack<SignalPlotXY?>(),
                new Stack<HLine?>(),
                new Stack<Bracket?>(),
                new Stack<Polygon?>()
            };
        }

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

            for (int i = 0; i < howManyGraphTypes; i++)
            {
                isDeleteGraphs[i] = false;
            }
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
                    case "E006":
                        MessageBox.Show("データエラー\n極端に大きい、または負の値が存在します。\nこれらの値は自動で削除されます。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        toolStripStatus.Text = "ERROR";
                        Application.DoEvents();
                        break;
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
            switch (data.error)
            {
                case "E007":
                    MessageBox.Show("データエラー\n負の時間データが存在します。\nこれらの値は自動で削除されます。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    Application.DoEvents();
                    break;
            }

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
                () => data.PeakProtectionIntensity(80, peakProtectionIntensity.Value / 4.0),
                () => data.GetImpulse(data.ignitionTimeIndex, data.burnOutTimeIndex),
                () => data.GetAverage(data.ignitionTimeIndex, data.burnOutTimeIndex)
                );

            var unfilteredThrustList = data.thrustList;

            if (denoise.Checked || DenoisedVsRaw.Checked)
                data.SetPeakProtectedThrustList();

            ////////////////////////////////////////////////////////////////データ加工終了。以下描画処理////////////////////////////////////////////////////////////////

            toolStripStatus.Text = "Graph is plotting";
            Application.DoEvents();

            bool tempIsGraphDrawn = isGraphDrawn;

            var fs = new GraphName();
            if (previousFileName != fileName)
                do
                {
                    DialogResult dr = fs.ShowDialog();
                    graphTitle = fs.value;
                    if (graphTitle == "")
                        MessageBox.Show("グラフの名前を入力してください。", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                } while (graphTitle == "" && previousFileName != fileName);

            fs.Dispose();

            if (isPlotMax.Checked && !showPeakProtectionIntensity.Checked && !isGraphDrawn)
            {
                var maxMarker = formsPlot1.Plot.AddMarker(data.timeWhenThrustMax, data.thrustMax, MarkerShape.filledCircle, 5, Color.Red);
                maxMarker.Text = "Max(" + graphTitle + "): " + data.thrustMax.ToString("F3") + " N";
                maxMarker.TextFont.Color = Color.Black;
                maxMarker.TextFont.Alignment = Alignment.MiddleLeft;
                maxMarker.TextFont.Size = 28;

                ((Stack<MarkerPlot?>)graphs[(byte)GraphType.maxMarker]).Push(maxMarker);
            }
            else
            {
                ((Stack<MarkerPlot?>)graphs[(byte)GraphType.maxMarker]).Push(null);
            }

            if (isPlotImpulse.Checked && !showPeakProtectionIntensity.Checked && !isGraphDrawn)
            {
                var impulseAno = formsPlot1.Plot.AddAnnotation(graphTitle + ": " + data.impulse.ToString("F3") + " N⋅s", -10, 10 + 50 * countAnnotation);
                impulseAno.Font.Size = 28;
                impulseAno.BackgroundColor = Color.FromArgb(25, Color.Black);
                impulseAno.Shadow = false;

                ((Stack<Annotation?>)graphs[(byte)GraphType.impulseAno]).Push(impulseAno);

                countAnnotation++;
            }
            else
            {
                ((Stack<Annotation?>)graphs[(byte)GraphType.impulseAno]).Push(null);
            }

            var graphColor = Color.FromArgb(255, Math.Abs((int)(30 + countGraphs * 120) % 192 - 64), Math.Abs((int)(40 + countGraphs * 160) % 192 - 64), Math.Abs((int)(50 + countGraphs * 200) % 192 - 64));

            if (!showPeakProtectionIntensity.Checked)
            {
                if (DenoisedVsRaw.Checked)
                {
                    foreach (var item in (Stack<SignalPlotXY?>)graphs[(byte)GraphType.graphDenoisedVsRaw])
                        if (item != null) formsPlot1.Plot.Remove(item);

                    var graphDenoisedVsRaw = formsPlot1.Plot.AddSignalXY(data.timeList.ToArray(), unfilteredThrustList.ToArray(), label: graphTitle + "(original)");
                    graphDenoisedVsRaw.LineWidth = 3;
                    graphDenoisedVsRaw.LineColor = Color.FromArgb(90, graphColor);
                    graphDenoisedVsRaw.MarkerColor = Color.FromArgb(90, graphColor);

                    ((Stack<SignalPlotXY?>)graphs[(byte)GraphType.graphDenoisedVsRaw]).Push(graphDenoisedVsRaw);
                }
                else
                {
                    ((Stack<SignalPlotXY?>)graphs[(byte)GraphType.graphDenoisedVsRaw]).Push(null);
                }
                var graph = formsPlot1.Plot.AddSignalXY(data.timeList.ToArray(), data.thrustList.ToArray(), label: graphTitle + (DenoisedVsRaw.Checked ? "(denoised)" : ""));
                graph.LineWidth = 2;
                graph.LineColor = graphColor;
                graph.MarkerColor = graphColor;

                ((Stack<SignalPlotXY?>)graphs[(byte)GraphType.graph]).Push(graph);
                ((Stack<SignalPlotXY?>)graphs[(byte)GraphType.peakProtectionIntensityGraph]).Push(null);

                formsPlot1.Plot.YAxis.Label("Thrust [N]");
                formsPlot1.Plot.XAxis.Label("Time [s]");

                tempIsGraphDrawn = true;
                countGraphs++;
            }
            else
            {
                var peakProtectionIntensityGraph = formsPlot1.Plot.AddSignalXY(data.timeList.ToArray(), data.peakProtection.ToArray(), label: "Peak protection intensity of the " + graphTitle);
                peakProtectionIntensityGraph.YAxisIndex = 1;
                peakProtectionIntensityGraph.XAxisIndex = 0;

                ((Stack<SignalPlotXY?>)graphs[(byte)GraphType.graphDenoisedVsRaw]).Push(null);
                ((Stack<SignalPlotXY?>)graphs[(byte)GraphType.graph]).Push(null);
                ((Stack<SignalPlotXY?>)graphs[(byte)GraphType.peakProtectionIntensityGraph]).Push(peakProtectionIntensityGraph);

                formsPlot1.Plot.YAxis2.Label("Peak protection intensity");
                formsPlot1.Plot.XAxis.Label("Time [s]");
                formsPlot1.Plot.YAxis2.Ticks(true);
            }

            if (isPlotAverageThrust.Checked && !showPeakProtectionIntensity.Checked && !isGraphDrawn)
            {
                var averageThrustLine = formsPlot1.Plot.AddHorizontalLine(data.averageThrust);
                averageThrustLine.LineWidth = 1;
                averageThrustLine.PositionLabel = true;
                averageThrustLine.Color = graphColor;
                averageThrustLine.LineStyle = LineStyle.Dash;
                averageThrustLine.PositionLabelBackground = graphColor;

                ((Stack<HLine?>)graphs[(byte)GraphType.averageThrustLine]).Push(averageThrustLine);
            }
            else
            {
                ((Stack<HLine?>)graphs[(byte)GraphType.averageThrustLine]).Push(null);
            }

            if (isPlotBurnTime.Checked && !showPeakProtectionIntensity.Checked && !isGraphDrawn)
            {
                double margin = (data.thrustList.Max() - (offsetRemoval.Checked ? 0 : data.offset)) * 0.06 * countBurnTimes;

                var burnTimeAno = formsPlot1.Plot.AddBracket(data.timeList[data.ignitionTimeIndex], -margin, data.timeList[data.burnOutTimeIndex], -margin, graphTitle + ": " + (data.timeList[data.burnOutTimeIndex] - data.timeList[data.ignitionTimeIndex]).ToString("F2") + " s");
                burnTimeAno.Font.Size = 22;

                ((Stack<Bracket?>)graphs[(byte)GraphType.burnTimeAno]).Push(burnTimeAno);

                if (data.burnOutTimeIndex != data.ignitionTimeIndex && countGraphs == 1)
                    ((Stack<Polygon?>)graphs[(byte)GraphType.fill]).Push
                        (formsPlot1.Plot.AddFill
                            (data.timeList.Skip(data.ignitionTimeIndex).Take(data.burnOutTimeIndex - data.ignitionTimeIndex).ToArray(),
                            data.thrustList.Skip(data.ignitionTimeIndex).Take(data.burnOutTimeIndex - data.ignitionTimeIndex).ToArray(),
                            0, Color.FromArgb(0x30000000)));
                else ((Stack<Polygon?>)graphs[(byte)GraphType.fill]).Push(null);

                countBurnTimes++;
            }
            else
            {
                ((Stack<Bracket?>)graphs[(byte)GraphType.burnTimeAno]).Push(null);
                ((Stack<Polygon?>)graphs[(byte)GraphType.fill]).Push(null);
            }

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
            isGraphDrawn = tempIsGraphDrawn;

            toolStripStatus.Text = "Done";

#if TIMER
            sw.Stop();
            MessageBox.Show(((double)sw.ElapsedMilliseconds / 1000).ToString("F3"));
#endif
        }

        private void Threshold_Scroll(object sender, EventArgs e)
        {
            label2.Text = (peakProtectionIntensity.Value / 4.0).ToString("F2");
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
                    case "E006":
                        MessageBox.Show("データエラー\n負の時間が発生しています。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        toolStripStatus.Text = "ERROR";
                        Application.DoEvents();
                        return;
                }

                toolStripStatus.Text = "The file has been loaded.";
                Application.DoEvents();

                skipTime.Value = 0;

                isGraphDrawn = false;
                isFileLoaded = false;
            }

            ofd.Dispose();
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
            isGraphDrawn = false;
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

        private void DenoisedVsRaw_CheckedChanged(object sender, EventArgs e)
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

        private void UndoGraph_Click(object sender, EventArgs e)
        {
            if (((Stack<SignalPlotXY?>)graphs[(byte)GraphType.graph]).Count > 0)
            {
                var PlotData = formsPlot1.Plot;

                PlotData.Remove(((Stack<MarkerPlot?>)graphs[(byte)GraphType.maxMarker]).Pop());
                PlotData.Remove(((Stack<Annotation?>)graphs[(byte)GraphType.impulseAno]).Pop());
                PlotData.Remove(((Stack<SignalPlotXY?>)graphs[(byte)GraphType.graphDenoisedVsRaw]).Pop());
                PlotData.Remove(((Stack<SignalPlotXY?>)graphs[(byte)GraphType.graph]).Pop());
                PlotData.Remove(((Stack<SignalPlotXY?>)graphs[(byte)GraphType.peakProtectionIntensityGraph]).Pop());
                PlotData.Remove(((Stack<HLine?>)graphs[(byte)GraphType.averageThrustLine]).Pop());
                PlotData.Remove(((Stack<Bracket?>)graphs[(byte)GraphType.burnTimeAno]).Pop());
                PlotData.Remove(((Stack<Polygon?>)graphs[(byte)GraphType.fill]).Pop());

                countAnnotation = (uint)((Stack<Annotation?>)graphs[(byte)GraphType.impulseAno]).Where(x => x != null).Count() + 1;
                countGraphs = (uint)((Stack<SignalPlotXY?>)graphs[(byte)GraphType.graph]).Where(x => x != null).Count();
                if (countGraphs == 0) maxTime[1] = 0;
                countBurnTimes = (uint)((Stack<Bracket?>)graphs[(byte)GraphType.burnTimeAno]).Where(x => x != null).Count();

                try
                {
                    PlotData.Add(((Stack<SignalPlotXY?>)graphs[(byte)GraphType.graphDenoisedVsRaw]).Peek());
                }
                catch (InvalidOperationException)
                {
                    //グラフを削除した結果全グラフがなくなったときに実行されるコード
                    //グラフが存在しないのにStackでPeek()を実行するとエラーが発生するのを利用している
                    graphInit.PerformClick();
                }

                formsPlot1.Refresh();

                fileName = "";
            }
            else
            {
                MessageBox.Show("これ以上Undoできません。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                toolStripStatus.Text = "ERROR";
                Application.DoEvents();
            }
        }
    }
}