using MathNet.Numerics.LinearAlgebra;

namespace DataProcessing
{
	internal class DataProcessing
	{
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
		private string error = "E000";
		private string fileName = "";
		private readonly uint timeColumnNum;
		private readonly uint dataColumnNum;

		internal DataProcessing(uint timeColumnNum, uint dataColumnNum)
		{
			this.timeColumnNum = timeColumnNum - 1;
			this.dataColumnNum = dataColumnNum - 1;
		}

		internal void SetFile(string fileName)
		{
			this.fileName = fileName;
			ReadCsv();
		}

		private void ReadCsv()
		{
			var rawData = new List<string>();
			timeList = new List<double>();
			thrustList = new List<double>();
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
				return;
			}

			int length = rawData.Count;

			string errorTemp = error;

			var locker = new object();

			var multiFor = new ParallelAssist.ParallelAssist();
			Parallel.Invoke(() =>
			{
				timeList = new List<double>(
					multiFor.ForMulti(rawData, 0, length - 1, (x, i) =>
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
				thrustList = new List<double>(
					multiFor.ForMulti(rawData, 0, length - 1, (x, i) =>
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
				if (timeList[i] == double.MinValue || thrustList[i] == double.MinValue)
				{
					timeList.RemoveAt(i - howManyDelete);
					thrustList.RemoveAt(i - howManyDelete);
					howManyDelete++;
					i--;
				}
			}

			error = errorTemp;

			if (timeList.Count < 1) error = "E004";
		}
		private void GetImpulse(int startIndex, int endIndex)
		{
			impulse = 0;

			for (int i = startIndex; i < endIndex - 1; i++)
				impulse += (thrustList[i] + thrustList[i + 1]) * (timeList[i + 1] - timeList[i]);

			impulse /= 2;
		}

		private void GetAverage(int startIndex, int endIndex)
		{
			averageThrust = thrustList
				.Skip(startIndex)
				.Take(endIndex - startIndex)
				.Average();
		}

		private void PeakProtectionIntensity(uint range, int threshold)
		{
			var peakProtection = new List<double>(HighpassFilter(filteredThrustList, range));
			for (int i = 0; i < peakProtection.Count; i++) peakProtection[i] = Math.Abs(peakProtection[i]);
			double max = peakProtection.Max();
			double IntensityValue = max / threshold;

			peakProtection = new ParallelAssist.ParallelAssist().ForMulti(peakProtection, 0, peakProtection.Count - 1, (x, i) =>
			{
				double temp = 1 / (1 + Math.Exp(-(x - IntensityValue) * 20 / max));
				return temp > 0.5 ? 1.0 : Math.Pow(temp * 2, 4) / 16;
			});
		}

		private void GetOffset()
		{
			int skip = (int)(filteredThrustList.Count * 0.03);

			double offset = filteredThrustList
				.OrderBy(x => x)
				.Skip(skip)
				.Take(skip)
				.Average();
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
		private bool DataCropping(double skipTime, double cutoffTime)
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
				_ = new ParallelAssist.ParallelAssist().ForMulti(samples.ToList(), sidePoints, length - sidePoints - 1, (x, n) =>
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