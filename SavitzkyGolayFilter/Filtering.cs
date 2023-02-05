using MathNet.Numerics.LinearAlgebra;

namespace DataProcessing
{
	internal class DataProcessing
	{
		private readonly MainProcess.Form1 form1;

		internal int ignitionTimeIndex;
		internal int burnOutTimeIndex;
		internal double offset;
		internal double thrustMax;
		internal double timeWhenThrustMax;
		internal double impulse;
		internal double averageThrust;
		internal List<double> timeList;
		internal List<double> thrustList;
		internal List<double> filteredThrustList;
		internal List<double> peakProtection;
		internal List<double> peakProtectedthrustList;
		internal string error = "E000";
		private string fileName = "";
		private readonly uint timeColumnNum;
		private readonly uint dataColumnNum;

		internal DataProcessing(MainProcess.Form1 form1, uint timeColumnNum, uint dataColumnNum)
		{
			this.form1 = form1;
			this.timeColumnNum = timeColumnNum;
			this.dataColumnNum = dataColumnNum;

			timeList = new List<double>();
			thrustList = new List<double>();
			filteredThrustList = new List<double>();
			peakProtection = new List<double>();
			peakProtectedthrustList = new List<double>();
		}

		internal void SetFile(string fileName)
		{
			this.fileName = fileName;
			(timeList, thrustList) = ReadCsv(this.fileName, timeColumnNum, dataColumnNum);
		}

		private (List<double>, List<double>) ReadCsv(string fileName, uint timeColumnNum, uint dataColumnNum)
		{
			var rawData = new List<string>();
			var timeList = new List<double>();
			var dataList = new List<double>();
			error = "E000";

			try
			{
				using StreamReader reader = new(fileName);
				string data = reader.ReadToEnd();
				rawData = data.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries).ToList();
			}
			catch (IOException)
			{
				error = "E003";
				return (new List<double>(), new List<double>());
			}
			catch (OutOfMemoryException)
			{
				error = "E005";
				return (new List<double>(), new List<double>());
			}

			int length = rawData.Count;

			string errorTemp = error;

			var locker = new object();

			var multiFor = new ParallelAssist.ParallelAssist();
			Parallel.Invoke(() =>
			{
				timeList = new List<double>(
					ParallelAssist.ParallelAssist.ForMulti(rawData, 0, length - 1, (x, i) =>
					{
						string[] values = x.Split(',');

						if (timeColumnNum > values.Length - 1)
						{
							lock (locker)
								if (errorTemp != "E002") errorTemp = "E002";
							return double.MinValue;
						}
						else
						{
							if (double.TryParse(values[timeColumnNum].Trim(), out double temp))
							{
								return temp;
							}
							else if (errorTemp != "E001")
								lock (locker) errorTemp = "E001";
							return double.MinValue;
						}
					})
				);
			}, () =>
			{
				dataList = new List<double>(
					ParallelAssist.ParallelAssist.ForMulti(rawData, 0, length - 1, (x, i) =>
					{
						string[] values = x.Split(',');

						if (dataColumnNum > values.Length - 1)
						{
							lock (locker)
								if (errorTemp != "E002") errorTemp = "E002";
							return double.MinValue;
						}
						else
						{
							if (double.TryParse(values[dataColumnNum].Trim(), out double temp))
							{
								return temp;
							}
							else if (errorTemp != "E001")
								lock (locker) errorTemp = "E001";
							return double.MinValue;
						}
					})
				);
			});

			int howManyDelete = 0;
			for (int i = 0; i < length - howManyDelete; i++)
			{
				if (timeList[i] == double.MinValue || dataList[i] == double.MinValue)
				{
					timeList.RemoveAt(i - howManyDelete);
					dataList.RemoveAt(i - howManyDelete);
					howManyDelete++;
					i--;
				}
			}

			error = errorTemp;

			if (timeList.Count < 1) error = "E004";

			return (timeList, dataList);
		}

		internal void CheckTimeData(bool forceCheck = false)
		{
			if (timeList.Where((x, i) => x < timeList[i - 1 < 0 ? 0 : i - 1]).Any())//時間データが昇順になっていない時の処理
			{
				form1.toolStripStatus.Text = "Retrograde time data";
				Application.DoEvents();

				if (forceCheck)
					if (MessageBox.Show("一部で時間が逆行しています。ソートして読み込みますか？", "注意", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
					{
						form1.graphInit.PerformClick();
						return;
					}

				SortData();
			}
			var seen = new HashSet<double>();//時間データが重複している時の処理
			var duplicates = timeList.Where(x => !seen.Add(x)).ToList();
			if (duplicates.Count > 0)
			{
				form1.toolStripStatus.Text = "Duplicate time data";
				Application.DoEvents();

				if (forceCheck)
					if (MessageBox.Show("同じ時間を指すデータが存在します。このまま読み込みますか？\n\n重複する時間(s): \n" + string.Join("\n", duplicates.Select(x => x.ToString("0.000"))), "注意", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
					{
						form1.graphInit.PerformClick();
						return;
					}
			}
		}

		internal void GetFilterThrustList(int sidePoints, int polynomialOrder)
		{
			filteredThrustList = new List<double>(new SavitzkyGolayFilter(sidePoints, polynomialOrder).Process(thrustList.ToArray()));
		}
		internal void GetImpulse(int startIndex, int endIndex)
		{
			impulse = 0;

			for (int i = startIndex; i < endIndex - 1; i++)
				impulse += (thrustList[i] + thrustList[i + 1]) * (timeList[i + 1] - timeList[i]);

			impulse /= 2;
		}

		internal void GetAverage(int startIndex, int endIndex)
		{
			averageThrust = thrustList
				.Skip(startIndex)
				.Take(endIndex - startIndex)
				.Average();
		}

		internal void GetOffset()
		{
			int skip = (int)(filteredThrustList.Count * 0.03);

			offset = filteredThrustList
				.OrderBy(x => x)
				.Skip(skip)
				.Take(skip)
				.Average();
		}

		internal void GetTimesIndex(double ignitionThreshold, double burnOutThreshold, bool isOffsetRemoved)
		{
			error = "E010";
			const int ignitionMargin = 500;
			const int burnOutMargin = 1000;
			double offset = 0;
			double thrustMax = filteredThrustList.Max();
			double timeMin = this.timeList.Min();
			this.ignitionTimeIndex = 0;
			this.burnOutTimeIndex = this.timeList.Count - 1;
			int trialCount = 0;
			int count1 = 0;
			int count2 = 0;
			double autoSkipTime = 0;
			bool[] estimatedResults = new bool[] { false, false };

			var filteredThrustListTemp = new List<double>(filteredThrustList);
			var timeListTemp = new List<double>(timeList);

			if (ignitionMargin + burnOutMargin >= filteredThrustListTemp.Count)
			{
				error = "E011";
				return;
			}

			if (!isOffsetRemoved) GetOffset();

			while (timeListTemp.Max() - timeListTemp.Min() > autoSkipTime)
			{
				Parallel.Invoke(() =>
				{
					double threshold = (thrustMax - offset) * ignitionThreshold + offset;
					for (int i = 0; i < filteredThrustListTemp.Count; i++) //燃焼開始時間推定
						if (filteredThrustListTemp[i] > threshold)
						{
							count2++;
							if (count2 > ignitionMargin)
							{
								ignitionTimeIndex = i - ignitionMargin;
								estimatedResults[0] = true;
								break;
							}
						}
						else
						{
							if (count2 != 0)
								count2 = 0;
						}
				}, () =>
				{
					double threshold = (thrustMax - offset) * burnOutThreshold + offset;
					for (int i = filteredThrustListTemp.Count - 1; i >= 0; i--) //燃焼終了時間推定
						if (filteredThrustListTemp[i] > threshold)
						{
							count1++;
							if (count1 > burnOutMargin)
							{
								burnOutTimeIndex = i + burnOutMargin;
								estimatedResults[1] = true;
								break;
							}
						}
						else
						{
							if (count1 != 0)
								count1 = 0;
						}
				});
				if (ignitionTimeIndex < burnOutTimeIndex && estimatedResults[0] && estimatedResults[1]) break;
				else
				{ //うまく推定できなかった時の処理(推定プログラムの原理上、スラストデータの最大値付近のデータが原因に成りやすい。よって、そこら変のデータをスキップする)
					double beforeThrustMax = thrustMax;
					int skipTimeIndex = 0;
					while (beforeThrustMax == thrustMax)
					{
						beforeThrustMax = thrustMax;
						if (trialCount == 0)
						{
							error = "E012";
							double floorMaxTime = Math.Floor((timeListTemp[filteredThrustListTemp.IndexOf(thrustMax)] - timeMin) * 100) * 0.01;
							skipTimeIndex = 0;
							for (int i = 0; i < timeListTemp.Count; i++)
							{
								if (timeListTemp[i] > floorMaxTime + timeMin)
								{
									skipTimeIndex = i;
									break;
								}
							}
							autoSkipTime = timeListTemp[skipTimeIndex] - timeMin;
							timeListTemp.RemoveRange(0, skipTimeIndex);
							filteredThrustListTemp = filteredThrustListTemp.Skip(skipTimeIndex).ToList();
							form1.skipTime.Value = (decimal)autoSkipTime * 1000;
							form1.skipTime.Refresh();
							thrustMax = filteredThrustListTemp.Max();
							count1 = 0;
							count2 = 0;
							trialCount++;
						}
						else
						{ //上記の処理でもうまくいかなかった場合0.001*trialCount秒ずつスキップしていく
							skipTimeIndex = 0;
							timeMin = timeListTemp[0];
							for (int i = 0; i < timeListTemp.Count; i++)
							{
								if (timeListTemp[i] > 0.001 * trialCount + timeListTemp[0])
								{
									skipTimeIndex = i;
									break;
								}
							}
							autoSkipTime += timeListTemp[skipTimeIndex] - timeMin;
							timeListTemp.RemoveRange(0, skipTimeIndex);
							filteredThrustListTemp = filteredThrustListTemp.Skip(skipTimeIndex).ToList();
							form1.skipTime.Value = (decimal)autoSkipTime * 1000;
							form1.skipTime.Refresh();
							thrustMax = filteredThrustListTemp.Max();
							count1 = 0;
							count2 = 0;
							trialCount++;
						}
					}
				}
			}
			if (timeListTemp.Max() - timeListTemp.Min() <= autoSkipTime) error = "E013";
		}

		internal void PeakProtectionIntensity(uint range, double threshold)
		{
			peakProtection = new List<double>(HighpassFilter(filteredThrustList, range));
			for (int i = 0; i < peakProtection.Count; i++) peakProtection[i] = Math.Abs(peakProtection[i]);
			double max = peakProtection.Max();
			double IntensityValue = max / threshold;

			peakProtection = ParallelAssist.ParallelAssist.ForMulti(peakProtection, 0, peakProtection.Count - 1, (x, i) =>
			{
				double temp = 1 / (1 + Math.Exp(-(x - IntensityValue) * 20 / max));
				return temp > 0.5 ? 1.0 : Math.Pow(temp * 2, 4) / 16;
			});
		}

		internal void SetPeakProtectedThrustList()
		{
			peakProtectedthrustList = thrustList.Select((x, i) => x * peakProtection[i] + filteredThrustList[i] * (1 - peakProtection[i])).ToList();
			thrustList = peakProtectedthrustList;
		}

		private void SortData()
		{
			var timeAndThrustList = timeList
				.Select((x, i) => new { Time = x, Data = thrustList[i], })
				.OrderBy(x => x.Time).ToList();

			timeList = timeList.Select((x, i) => timeAndThrustList[i].Time).ToList();
			thrustList = thrustList.Select((x, i) => timeAndThrustList[i].Data).ToList();
		}

		/// <summary>
		/// Deletes data for the specified number of seconds.
		/// </summary>
		/// <param name="skipTime"></param>
		/// <param name="cutoffTime"></param>
		/// <returns>If cropping succeeds, true is returned; if it fails, false is returned.</returns>
		internal bool DataCropping(double skipTime, double cutoffTime)
		{
			bool isError = false;
			try
			{
				if (cutoffTime != 0)
				{
					double maxtime = timeList.Max();
					for (int i = timeList.Count - 1; maxtime - timeList[i] < cutoffTime; i--)
					{
						thrustList.RemoveAt(timeList.Count - 1);
						timeList.RemoveAt(timeList.Count - 1);
					}
				}

				if (skipTime != 0)
				{
					int skipTimeIndex = 0;
					double mintime = timeList.Min();
					for (int i = 0; i < timeList.Count; i++)
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
			}
			catch (ArgumentOutOfRangeException)
			{
				isError = true;
			}

			return !isError;
		}

		internal void TimeConvert(double prefix)
		{
			timeList = timeList.Select(i => i * prefix).ToList();
		}

		private void LeastSquares(string calibrationFile, out double a, out double b)
		{//F[N] = av[mv] + b <=> y = ax + b
			var x = new List<double>();
			var y = new List<double>();

			a = 0; b = 0;

			(y, x) = ReadCsv(calibrationFile, 0, 1);//1列目のところに載せたおもりのkgを、2列目のところに記録された電圧を書いたcsvファイルを用意する。
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
					form1.toolStripStatus.Text = "ERROR";
					Application.DoEvents();
					return;
				case "E004":
					MessageBox.Show("列指定エラー\n存在しない列を指定しています。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
					form1.toolStripStatus.Text = "ERROR";
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

		internal void SRegressionLine(string calibrationFile = @"./CalibrationData.csv")
		{
			LeastSquares(calibrationFile, out double a, out double b);

			thrustList = thrustList.Select(i => i * a + b).ToList();
		}

		internal static List<double> MovAverage(List<double> data, uint range)
		{
			var temp = new List<double>(data);
			var result = new List<double>(data);

			for (int i = 0; i < range - 1; i++) temp.Insert(0, 0);
			Parallel.For(0, temp.Count - (int)range - 1, i =>
			{
				double sum = 0;
				for (int j = 0; j < range; j++) sum += temp[i + j];
				result[i] = sum / range;
			});

			return result;
		}

		internal static List<double> HighpassFilter(List<double> data, uint range)
		{
			var temp = MovAverage(data, range);
			return data.Select((x, i) => x - temp[i]).ToList();
		}

	}
	/// <summary>
	/// <para>Implements a Savitzky-Golay smoothing filter, as found in [1].</para>
	/// <para>[1] Sophocles J.Orfanidis. 1995. Introduction to Signal Processing. Prentice-Hall, Inc., Upper Saddle River, NJ, USA.</para>
	/// </summary>
	internal sealed class SavitzkyGolayFilter
	{
		private readonly int sidePoints;

		private readonly Matrix<double> coefficients;

		internal SavitzkyGolayFilter(int sidePoints, int polynomialOrder)
		{
			this.sidePoints = sidePoints;
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

			Parallel.Invoke(() =>
			{
				double[] frame = new double[frameSize];
				Array.Copy(samples, frame, frameSize);

				for (int i = 0; i < sidePoints; ++i)
				{
					output[i] = coefficients.Column(i).DotProduct(Vector<double>.Build.DenseOfArray(frame));
				}
			}, () =>
			{
				ParallelAssist.ParallelAssist.ForMulti(samples.ToList(), sidePoints, length - sidePoints - 1, (x, n) =>
				{
					double[] frame = new double[frameSize];
					Array.ConstrainedCopy(samples, n - sidePoints, frame, 0, frameSize);
					output[n] = coefficients.Column(sidePoints).DotProduct(Vector<double>.Build.DenseOfArray(frame));
					return x;
				});
				/*
				//上記プログラムは下記プログラムを並列処理化したもの
				for (int n = sidePoints; n < length - sidePoints; ++n)
				{
					Array.ConstrainedCopy(samples, n - sidePoints, frame, 0, frameSize);
					output[n] = coefficients.Column(sidePoints).DotProduct(Vector<double>.Build.DenseOfArray(frame));
				}
				*/
			}, () =>
			{
				double[] frame = new double[frameSize];
				Array.ConstrainedCopy(samples, length - frameSize, frame, 0, frameSize);

				for (int i = 0; i < sidePoints; ++i)
				{
					output[length - sidePoints + i] = coefficients.Column(sidePoints + 1 + i).DotProduct(Vector<double>.Build.Dense(frame));
				}
			});

			return output;
		}
	}
}