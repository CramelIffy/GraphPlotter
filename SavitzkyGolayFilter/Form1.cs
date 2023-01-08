//#define TIMER

using MathNet.Numerics.LinearAlgebra;
using ScottPlot;
using System.Data;
using System.Diagnostics;
using System.Drawing.Design;
using System.Runtime.InteropServices;
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
        uint countGraphs = 0;
        uint countAnnotation = 0;
        uint countBurnTimes = 0;
        double[] maxTime = new double[2];
        bool checkBoxWarming = false;
        double graphXLeft = 0;
        double graphXRight = 0;

        void ReadCsv(string file, uint timeColumn, uint dataColumn, out List<double> timeList, out List<double> dataList, out string error)
        {
            List<string> rawData = new List<string>();
            timeList = new List<double>();
            dataList = new List<double>();
            error = "E000";

            try
            {
                using (StreamReader reader = new StreamReader(file))
                {
                    while (!reader.EndOfStream)
                    {
                        rawData.Add(reader.ReadLine());
                    }
                }
            }
            catch (IOException)
            {
                error = "E003";
                return;
            }

            var rawDataForLoop = CollectionsMarshal.AsSpan(rawData);

            for (int i = 0; i < rawDataForLoop.Length; i++)
            {
                string[] values = rawDataForLoop[i].Split(',');

                if ((timeColumn > dataColumn ? timeColumn : dataColumn) > values.Length - 1)
                {
                    if (error != "E002") error = "E002";
                }
                else
                {
                    double dataTemp;
                    double timeTemp;
                    if (double.TryParse(values[timeColumn].Trim(), out timeTemp) && double.TryParse(values[dataColumn].Trim(), out dataTemp))
                    {
                        timeList.Add(timeTemp);
                        dataList.Add(dataTemp);
                    }
                    else if (error != "E001") error = "E001";
                }
            }

            if (timeList.Count() < 1) error = "E004";
        }

        double GetImpulse(List<double> timeList, List<double> dataList, int ignitionTimeIndex, int burnOutTimeIndex)
        {
            double totalInpulse = 0;

            for (int i = ignitionTimeIndex; i < burnOutTimeIndex - 1; i++)
                totalInpulse += (dataList[i] + dataList[i + 1]) * (timeList[i + 1] - timeList[i]);

            return totalInpulse / 2;
        }

        List<double> MovAverage(List<double> data, uint range)
        {
            List<double> temp = new List<double>();
            List<double> result = new List<double>();
            temp.AddRange(data);
            result.AddRange(data);

            for (int i = 0; i < range - 1; i++) temp.Insert(0, 0);
            Parallel.For(0, temp.Count() - (int)range - 1, i =>
            {
                double sum = 0;
                for (int j = 0; j < range; j++) sum += temp[i + j];
                result[i] = sum / range;
            });

            return result;
        }

        List<double> HighpassFilter(List<double> data, uint range)
        {
            List<double> temp = MovAverage(data, range);
            List<double> result = new List<double>();

            for (int i = 0; i < data.Count(); i++) result.Add(data[i] - temp[i]);

            return result;
        }

        List<double> PeakProtection(List<double> data, List<double> time, uint range, int ignitionTimeIndex)
        {
            double max;
            List<double> temp = new List<double>();
            temp.AddRange(HighpassFilter(data, range));
            for (int i = 0; i < temp.Count(); i++) temp[i] = Math.Abs(temp[i]);
            max = temp.Max();
            double IntensityValue = max / Convert.ToDouble(peakProtectionIntensity.Value.ToString());

            Parallel.For(0, temp.Count(), i => temp[i] = 1 / (1 + Math.Exp(-(temp[i] - IntensityValue) * 20 / max)));

            Parallel.For(ignitionTimeIndex, temp.Count(), i => temp[i] = temp[i] > 0.5 ? 1.0 : Math.Pow(temp[i] * 2, 4) / 16);
            return temp;
        }

        double GetOffset(List<double> dataList)
        {
            int skip = (int)(dataList.Count() * 0.03);

            double mean = dataList
                .OrderBy(x => x)
                .Skip(skip)
                .Take(skip)
                .Average();

            return mean;
        }

        void GetTimesIndex(List<double> thrustList, List<double> timeList, out List<double> thrustListOut, out List<double> timeListOut, double ignitionThreshold, double burnOutThreshold, bool isOffsetRemoved, out int ignitionTimeIndex, out int burnOutTimeIndex, out string error)
        {
            error = "E010";
            const int ignitionMargin = 500;
            const int burnOutMargin = 1000;
            double offset = 0;
            double thrustMax = thrustList.Max();
            ignitionTimeIndex = 0;
            burnOutTimeIndex = timeList.Count() - 1;
            int trialCount = 0;
            int count1 = 0;
            int count2 = 0;
            int ignitionTimeIndexTemp = 0;
            int burnOutTimeIndexTemp = burnOutTimeIndex;
            var timeListTemp = new List<double>();
            var thrustListTemp = new List<double>();
            timeListOut = new List<double>();
            thrustListOut = new List<double>();
            double autoSkipTime = 0;
            bool[] estimatedResults = new bool[] { false, false };

            timeListTemp.AddRange(timeList);
            thrustListTemp.AddRange(thrustList);

            if (ignitionMargin + burnOutMargin >= thrustListTemp.Count())
            {
                error = "E011";
                return;
            }

            if (!isOffsetRemoved) offset = GetOffset(thrustListTemp);

            while (timeListTemp.Max() - timeListTemp.Min() > autoSkipTime)
            {
                Parallel.Invoke(() =>
                {
                    for (int i = 0; i < thrustListTemp.Count(); i++) //燃焼開始時間推定
                        if (thrustListTemp[i] > (thrustMax - offset) * ignitionThreshold + offset)
                        {
                            count2++;
                            if (count2 > ignitionMargin)
                            {
                                ignitionTimeIndexTemp = i - ignitionMargin;
                                estimatedResults[0] = true;
                                break;
                            }
                        }
                        else
                        {
                            count2 = 0;
                        }
                }, () =>
                {
                    for (int i = thrustListTemp.Count() - 1; i >= 0; i--) //燃焼終了時間推定
                        if (thrustListTemp[i] > (thrustMax - offset) * burnOutThreshold + offset)
                        {
                            count1++;
                            if (count1 > burnOutMargin)
                            {
                                burnOutTimeIndexTemp = i + burnOutMargin;
                                estimatedResults[1] = true;
                                break;
                            }
                        }
                        else
                        {
                            count1 = 0;
                        }
                });
                if (ignitionTimeIndexTemp < burnOutTimeIndexTemp && estimatedResults[0] && estimatedResults[1]) break;
                else
                { //うまく推定できなかった時の処理(推定プログラムの原理上、スラストデータの最大値付近のデータが原因に成りやすい。よって、そこら変のデータをスキップする)
                    if (trialCount == 0)
                    { //最大値手前0.5秒までのデータを削除
                        error = "E012";
                        double beforeMax = Math.Floor((timeListTemp[thrustListTemp.IndexOf(thrustMax)] - timeListTemp.Min() - 0.5) * 10) * 0.1;
                        beforeMax = beforeMax < 0 ? 0 : beforeMax;
                        DataCropping(timeListTemp, thrustListTemp, out timeListTemp, out thrustListTemp, beforeMax, 0.0);
                        autoSkipTime = beforeMax;
                        skipTime.Value = (decimal)autoSkipTime * 1000;
                        skipTime.Refresh();
                        thrustMax = thrustListTemp.Max();
                        count1 = 0;
                        count2 = 0;
                        trialCount++;
                    }
                    else
                    { //上記の処理でもうまくいかなかった場合0.1秒ずつスキップしていく
                        DataCropping(timeListTemp, thrustListTemp, out timeListTemp, out thrustListTemp, 0.1, 0.0);
                        autoSkipTime += 0.1;
                        skipTime.Value = (decimal)autoSkipTime * 1000;
                        skipTime.Refresh();
                        thrustMax = thrustListTemp.Max();
                        count1 = 0;
                        count2 = 0;
                        trialCount++;
                    }
                }
            }
            if (timeListTemp.Max() - timeListTemp.Min() <= autoSkipTime) error = "E013";

            ignitionTimeIndex = ignitionTimeIndexTemp;
            burnOutTimeIndex = burnOutTimeIndexTemp;

            timeListOut.AddRange(timeListTemp);
            thrustListOut.AddRange(thrustListTemp);
        }

        void LeastSquares(out double a, out double b)
        {//F[N] = av[mv] + b <=> y = ax + b
            List<double> x = new List<double>();
            List<double> y = new List<double>();
            string error = "E000";

            a = 0; b = 0;

            ReadCsv(@"./CalibrationData.csv", 0, 1, out y, out x, out error);//1列目のところに載せたおもりのkgを、2列目のところに記録された電圧を書いたcsvファイルを用意する。
            switch (error)
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
                    MessageBox.Show("ファイルが開けません。\nCalibrationData.csvが同一フォルダ内に無い可能性があります。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    toolStripStatus.Text = "ERROR";
                    Application.DoEvents();
                    return;
                case "E004":
                    MessageBox.Show("列指定エラー\n存在しない列を指定しています。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    toolStripStatus.Text = "ERROR";
                    Application.DoEvents();
                    return;
            }
            //重力加速度を乗する(重力加速度は重力値推定計算サービスより)
            y = y.Select(i => i * 9.796685).ToList();
            // 変数 x の平均
            double AverageOfX = (double)x.Average();
            // 変数 y の平均
            double AverageOfY = (double)y.Average();
            // 変数 x と y の積の平均
            double AverageOfXY = (double)x.Select(i => i * y[x.IndexOf(i)]).Average();
            // 変数xの2乗の平均 
            double AverageOfSquareX = (double)x.Select(i => i * i).Average();

            // 単回帰直線の傾き
            a = (AverageOfXY - AverageOfX * AverageOfY) / (AverageOfSquareX - AverageOfX * AverageOfX);
            // 単回帰直線の切片
            b = -a * AverageOfX + AverageOfY;
        }

        List<double> SRegressionLine(List<double> rawData)
        {
            double a, b = 0;
            LeastSquares(out a, out b);

            var result = rawData
                .Select(i => i * a + b)
                .ToList();

            return result;
        }

        void DataCropping(List<double> timeList, List<double> thrustList, out List<double> timeListOut, out List<double> thrustListOut, double skipTime, double cutoffTime)
        {
            timeListOut = new List<double>();
            thrustListOut = new List<double>();
            try
            {
                double maxtime = timeList.Max();
                for (int i = timeList.Count() - 1; maxtime - timeList[i] < cutoffTime; i--)
                {
                    thrustList.RemoveAt(timeList.Count() - 1);
                    timeList.RemoveAt(timeList.Count() - 1);
                }

                int skipTimeIndex = 0;
                double mintime = timeList.Min();
                for (int i = 0; i < timeList.Count(); i++)
                {
                    if (timeList[i] - mintime > skipTime)
                    {
                        skipTimeIndex = i;
                        break;
                    }
                }
                timeList.RemoveRange(0, skipTimeIndex);
                thrustList.RemoveRange(0, skipTimeIndex);
            }
            catch (ArgumentOutOfRangeException)
            {
                MessageBox.Show("SkipもしくはCutoffで異常が発生しました。\nSkipとCutoffの合計がデータ全体の秒数を超えています。\nSkipやCutoffの数値を変更してください。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                toolStripStatus.Text = "ERROR";
                Application.DoEvents();
                Init();
            }
            timeListOut.AddRange(timeList);
            thrustListOut.AddRange(thrustList);
        }

        double GetAverage(List<double> thrustList, int startIndex, int endIndex)
        {
            var result = thrustList
                .Skip(startIndex)
                .Take(endIndex - startIndex)
                .Average();

            return result;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            formsPlot1.Plot.Palette = Palette.OneHalf;
            formsPlot1.Plot.Style(Style.Seaborn);
            formsPlot1.Refresh();

            toolStripStatus.Text = "Standby";
        }

        private void Plot_Click(object sender, EventArgs e)
        {
#if TIMER
            var sw = new Stopwatch();
            sw.Start();
#endif


            string error;
            int ignitionTimeIndex;
            int burnOutTimeIndex;
            double offset;

            if (fileName == "")
            {
                MessageBox.Show("読み込むファイルを選択してください。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            List<double> timeList = new List<double>();
            List<double> thrustList = new List<double>();
            List<double> filteredSignalList = new List<double>();
            List<double> peakProtection = new List<double>();

            toolStripStatus.Text = "File loading";
            Application.DoEvents();

            ReadCsv(fileName, (uint)(timeColumnNum.Value - 1), (uint)(dataColumnNum.Value - 1), out timeList, out thrustList, out error);

            switch (error)
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
                    return;
                case "E004":
                    MessageBox.Show("列指定エラー\n存在しない列を指定しています。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    toolStripStatus.Text = "ERROR";
                    Application.DoEvents();
                    return;
            }

            if (readAsMs.Checked) timeList = timeList.Select(i => i * 0.001).ToList();

            toolStripStatus.Text = "Data cropping";
            Application.DoEvents();

            DataCropping(timeList, thrustList, out timeList, out thrustList, (double)skipTime.Value * 0.001, (double)cutoffTime.Value * 0.001);

            if (readAsVol.Checked) thrustList = SRegressionLine(thrustList);

            filteredSignalList = thrustList;

            toolStripStatus.Text = "Denoised data generating";
            Application.DoEvents();

            for (int j = 2; j >= 1; j--)
            {
                var filteredSignal = new Filtering.SavitzkyGolayFilter(j * 8 + 1, 5).Process(filteredSignalList.ToArray());
                filteredSignalList = filteredSignal.ToList();
            }

            offset = GetOffset(filteredSignalList);

            if (offsetRemoval.Checked)
            {
                toolStripStatus.Text = "Deoffsetting";
                Application.DoEvents();

                Parallel.For(0, thrustList.Count(), i =>
                {
                    thrustList[i] -= offset;
                    filteredSignalList[i] -= offset;
                });

                offset = 0;
            }

            toolStripStatus.Text = "Burn time estimating";
            Application.DoEvents();

            GetTimesIndex(filteredSignalList, timeList, out filteredSignalList, out timeList, igniThreshold.Value * 0.01, burnoutThreshold.Value * 0.01, offsetRemoval.Checked, out ignitionTimeIndex, out burnOutTimeIndex, out error);

            if (filteredSignalList.Count() < thrustList.Count()) thrustList.RemoveRange(0, thrustList.Count() - filteredSignalList.Count());

            switch (error)
            {
                case "E010": break;
                case "E011":
                    MessageBox.Show("燃焼開始前、燃焼終了後のデータのいずれかもしくはその両方が規定より少なすぎます。\n燃焼時間の推定に失敗した恐れがあります。\n燃焼時間の推定に失敗する可能性があります。", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    break;
                case "E012":
                    MessageBox.Show("燃焼時間の推定に失敗したため、データの序盤をスキップしました。\nグラフの描画に失敗した恐れがあります。\n燃焼時間の推定に失敗する場合は、SkipやCut offの値を調整してください。", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    break;
                case "E013":
                    MessageBox.Show("燃焼時間の推定に失敗しました。\nデータの一部に異常が存在する可能性があります。\nSkipやCutoffの数値を調整して異常部分を回避してください。", "警告", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    toolStripStatus.Text = "ERROR";
                    Application.DoEvents();
                    Init();
                    return;
            }

            if (SetIgnitionTimeZero.Checked)
            {
                double ignitionTime = timeList[ignitionTimeIndex];
                Parallel.For(0, timeList.Count(), i => timeList[i] -= ignitionTime);
            }

            if (countGraphs == 0) maxTime[0] = timeList[thrustList.IndexOf(thrustList.Max())];
            else maxTime[1] = timeList[thrustList.IndexOf(thrustList.Max())];

            if (countGraphs > 0 && isAlignMax.Checked)
            {
                toolStripStatus.Text = "Aligning the max";
                Application.DoEvents();

                Parallel.For(0, timeList.Count(), i => timeList[i] += maxTime[0] - maxTime[1]);
            }

            toolStripStatus.Text = "Graph is plotting";
            Application.DoEvents();

            if (isPlotMax.Checked && !showPeakProtectionIntensity.Checked)
            {
                var maxMarker = formsPlot1.Plot.AddMarker(timeList[thrustList.IndexOf(thrustList.Max())], thrustList.Max(), MarkerShape.filledCircle, 5, Color.Red);
                maxMarker.Text = "Max: " + thrustList.Max().ToString("F3") + " N";
                maxMarker.TextFont.Color = Color.Black;
                maxMarker.TextFont.Alignment = Alignment.MiddleLeft;
                maxMarker.TextFont.Size = 28;
            }

            peakProtection = PeakProtection(filteredSignalList, timeList, 80, ignitionTimeIndex);

            if (denoise.Checked)
                Parallel.For(0, thrustList.Count(), i => thrustList[i] = thrustList[i] * peakProtection[i] + filteredSignalList[i] * (1 - peakProtection[i]));

            if (isPlotImpulse.Checked && !showPeakProtectionIntensity.Checked)
            {
                var impulseAno = formsPlot1.Plot.AddAnnotation("Total Impulse of Graph " + countGraphs.ToString() + " = " + GetImpulse(timeList, denoise.Checked ? filteredSignalList : thrustList, ignitionTimeIndex, burnOutTimeIndex).ToString("F3") + " N⋅s", -10, 10 + 50 * countAnnotation);
                impulseAno.Font.Size = 28;
                impulseAno.BackgroundColor = Color.FromArgb(25, Color.Black);
                impulseAno.Shadow = false;

                countAnnotation++;
            }

            if (isPlotBurnTime.Checked && !showPeakProtectionIntensity.Checked)
            {
                double margin = (thrustList.Max() - offset) * 0.06 * countBurnTimes;
                var burnTimeAno = formsPlot1.Plot.AddBracket(timeList[ignitionTimeIndex], -margin, timeList[burnOutTimeIndex], -margin, "Burn Time of Graph " + countGraphs.ToString() + ": " + (timeList[burnOutTimeIndex] - timeList[ignitionTimeIndex]).ToString("F2") + " s");
                burnTimeAno.Font.Size = 22;

                countBurnTimes++;
            }

            var graphColor = Color.Black;

            if (!showPeakProtectionIntensity.Checked)
            {
                var graph = formsPlot1.Plot.AddSignalXY(timeList.ToArray(), thrustList.ToArray(), label: "graph " + countGraphs.ToString());
                graphColor = graph.LineColor;

                var timeListForFill = timeList
                    .Skip(ignitionTimeIndex)
                    .Take(burnOutTimeIndex - ignitionTimeIndex)
                    .ToArray();

                var thrustListForFill = thrustList
                    .Skip(ignitionTimeIndex)
                    .Take(burnOutTimeIndex - ignitionTimeIndex)
                    .ToArray();

                if (burnOutTimeIndex != ignitionTimeIndex)
                    formsPlot1.Plot.AddFill(timeListForFill, thrustListForFill, 0, Color.FromArgb(0x30000000));

                formsPlot1.Plot.YAxis.Label("Thrust [N]");
            }
            else
            {
                var peakProtectionIntensityGraph = formsPlot1.Plot.AddSignalXY(timeList.ToArray(), peakProtection.ToArray(), label: "peak protection intensity of graph " + countGraphs.ToString());
                peakProtectionIntensityGraph.YAxisIndex = 1;
                peakProtectionIntensityGraph.XAxisIndex = 0;

                formsPlot1.Plot.YAxis2.Label("Peak protection intensity");
                formsPlot1.Plot.YAxis2.Ticks(true);
            }

            if (isPlotAverageThrust.Checked && !showPeakProtectionIntensity.Checked)
            {
                double averageThrust = GetAverage(thrustList, ignitionTimeIndex, burnOutTimeIndex);

                var averageThrustLine = formsPlot1.Plot.AddHorizontalLine(averageThrust);
                averageThrustLine.LineWidth = 1;
                averageThrustLine.PositionLabel = true;
                averageThrustLine.Color = graphColor;
                averageThrustLine.LineStyle = LineStyle.Dash;
                averageThrustLine.PositionLabelBackground = graphColor;
            }

            countGraphs++;

            graphXLeft = timeList[ignitionTimeIndex] - (timeList[burnOutTimeIndex] - timeList[ignitionTimeIndex]) * 0.04;
            graphXRight = timeList[burnOutTimeIndex] + (timeList[burnOutTimeIndex] - timeList[ignitionTimeIndex]) * 0.08;

            formsPlot1.Plot.Legend();

            double graphMaxY = thrustList.Max();
            formsPlot1.Plot.SetAxisLimitsX(graphXLeft, graphXRight);
            formsPlot1.Plot.AxisAutoY();
            try
            {
                formsPlot1.Refresh();
            }
            catch (InvalidOperationException)
            {
                MessageBox.Show("時間データが逆行しています。\n時間データは必ず増加し続ける必要があります。", "警告", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Init();
                toolStripStatus.Text = "ERROR";
                Application.DoEvents();
                return;
            }

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
            OpenFileDialog ofd = new OpenFileDialog();

            ofd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            ofd.Filter = "CSVファイル(*.csv)|*.csv|テキストファイル(*.txt)|*.txt|ログファイル(*.log)|*.log";
            ofd.FilterIndex = 1;
            ofd.Title = "開くファイルを選択してください";
            ofd.RestoreDirectory = true;

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                fileName = ofd.FileName;
            }

            ofd.Dispose();

            skipTime.Value = 0;

            toolStripStatus.Text = "Determined files to be read";
            Application.DoEvents();
        }

        private void SaveFigure_Click(object sender, EventArgs e)
        {
            string saveFilePath = "";
            FolderBrowserDialog fbDialog = new FolderBrowserDialog();
            fbDialog.SelectedPath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            fbDialog.ShowNewFolderButton = true;
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
            formsPlot1.Plot.YAxis.Label("");
            formsPlot1.Plot.YAxis2.Label("");
            formsPlot1.Plot.YAxis2.Ticks(false);

            formsPlot1.Plot.Clear();
            formsPlot1.Refresh();

            countGraphs = 0;
            maxTime = new double[2];
            countAnnotation = 0;
            countBurnTimes = 0;
            skipTime.Value = 0;
            cutoffTime.Value = 0;
        }

        private void GraphInit_Click(object sender, EventArgs e)
        {
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
            var startInfo = new ProcessStartInfo("https://github.com/CramelIffy/GraphPlotter");
            startInfo.UseShellExecute = true;
            Process.Start(startInfo);
        }

        private void toolStripStatus_Click(object sender, EventArgs e)
        {

        }
    }
}
namespace Filtering
{
    /// <summary>
    /// <para>Implements a Savitzky-Golay smoothing filter, as found in [1].</para>
    /// <para>[1] Sophocles J.Orfanidis. 1995. Introduction to Signal Processing. Prentice-Hall, Inc., Upper Saddle River, NJ, USA.</para>
    /// </summary>
    internal sealed class SavitzkyGolayFilter
    {
        private readonly int sidePoints;

        private Matrix<double> coefficients;

        internal SavitzkyGolayFilter(int sidePoints, int polynomialOrder)
        {
            this.sidePoints = sidePoints;
            Design(polynomialOrder);
        }

        /// <summary>
        /// Smoothes the input samples.
        /// </summary>
        /// <param name="samples"></param>
        /// <returns></returns>
        internal double[] Process(double[] samples)
        {
            int length = samples.Length;
            double[] output = new double[length];
            int frameSize = (sidePoints << 1) + 1;
            double[] frame = new double[frameSize];

            Array.Copy(samples, frame, frameSize);

            Parallel.Invoke(() =>
            {
                for (int i = 0; i < sidePoints; ++i)
                {
                    output[i] = coefficients.Column(i).DotProduct(Vector<double>.Build.DenseOfArray(frame));
                }
            }, () =>
            {
                for (int n = sidePoints; n < length - sidePoints; ++n)
                {
                    Array.ConstrainedCopy(samples, n - sidePoints, frame, 0, frameSize);
                    output[n] = coefficients.Column(sidePoints).DotProduct(Vector<double>.Build.DenseOfArray(frame));
                }
            });

            Array.ConstrainedCopy(samples, length - frameSize, frame, 0, frameSize);

            for (int i = 0; i < sidePoints; ++i)
            {
                output[length - sidePoints + i] = coefficients.Column(sidePoints + 1 + i).DotProduct(Vector<double>.Build.Dense(frame));
            }

            return output;
        }

        private void Design(int polynomialOrder)
        {
            double[,] a = new double[(sidePoints << 1) + 1, polynomialOrder + 1];

            Parallel.For(-sidePoints, sidePoints + 1, m =>
            {
                for (int i = 0; i <= polynomialOrder; ++i)
                {
                    a[m + sidePoints, i] = Math.Pow(m, i);
                }
            });

            Matrix<double> s = Matrix<double>.Build.DenseOfArray(a);
            coefficients = s.Multiply(s.TransposeThisAndMultiply(s).Inverse()).Multiply(s.Transpose());
        }
    }
}