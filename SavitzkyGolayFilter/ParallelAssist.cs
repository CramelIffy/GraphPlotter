namespace ParallelAssist
{
	internal class ParallelAssist
	{
		/// <summary>
		/// A function that divides the process using a For loop into the number of available processors and executes each For loop using multi-thread processing.
		/// Note: In the return value, the minimum value of type double is treated as if it could not be converted.
		/// </summary>
		/// <typeparam name="inType"></typeparam>
		/// <typeparam name="outType"></typeparam>
		/// <param name="array">Source data</param>
		/// <param name="startIndex"></param>
		/// <param name="endIndex"></param>
		/// <param name="actionForLoop">
		/// Processing.
		/// Note: In the return value, the minimum value of type double is treated as if it could not be converted.
		/// </param>
		/// <returns></returns>
		internal ParallelAssist()
		{

		}
		internal static List<outType?> ForMulti<inType, outType>(List<inType> array, int startIndex, int endIndex, Func<inType, int, outType> actionForLoop)
			where outType : IComparable
			where inType : IComparable
		{
			int loopCount = array.Count;
			if (startIndex < 0 || startIndex > loopCount - 1)
			{
				throw new IndexOutOfRangeException("startIndexが配列外にアクセスしています。");
			}
			else if (endIndex < 0 || endIndex > loopCount - 1)
			{
				throw new IndexOutOfRangeException("endIndexが配列外にアクセスしています。");
			}

			loopCount = endIndex - startIndex + 1;

			var result = new List<outType?>(Enumerable.Repeat(default(outType), loopCount));

			int threadCount = Environment.ProcessorCount;
			int chunkSize = loopCount / threadCount;

			var tasks = new List<Task>();

			for (int i = 0; i < threadCount; i++)
			{
				int startIndexForParallel = i * chunkSize + startIndex;
				int endIndexForParallel;
				if (i == threadCount - 1)
				{
					endIndexForParallel = loopCount;
				}
				else endIndexForParallel = startIndexForParallel + chunkSize;

				var task = Task.Run(() =>
				{
					for (int n = startIndexForParallel; n < endIndexForParallel; n++)
						result[n - startIndex] = actionForLoop(array[n], n);
				});
				tasks.Add(task);
			}

			Task.WaitAll(tasks.ToArray());

			return result;
		}
	}
}
