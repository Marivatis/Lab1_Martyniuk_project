struct Pivot (double value, int colPosition, int rowPosition)
{
	public double value = value;
	public int colPosition = colPosition;
	public int rowPosition = rowPosition;
}

static class MatrixFunctions
{
	public static void PrintMatrix<T>(T[,] matrix, string[] colLabels, string[] rowLabels)
	{
		// Labeling
		System.Console.Write("       ");
		for (int i = 0; i < matrix.GetLength(1); i++)
		{
			System.Console.Write($"{colLabels[i],6} ");
		}
		System.Console.WriteLine();


		for (int i = 0; i < matrix.GetLength(0); i++)
		{
			System.Console.Write($"{rowLabels[i],6} ");
			for (int j = 0; j < matrix.GetLength(1); j++)
			{
				System.Console.Write($"{matrix[i, j],6:F2} ");
			}
			System.Console.WriteLine();
		}
	}
}

static class ArrayFunctions
{
	public static void PrintArray<T>(T[] array)
	{
		for (int i = 0; i < array.Length; i++)
		{
			System.Console.Write($"{array[i],6:F2} ");
		}
	}
}

class Program 
{
	public static double[,] PerformJordanEliminationStep(double[,] matrix, Pivot pivot, ref string[] colLabels, ref string[] rowLabels)
	{		
		double[,] activeMatrix = (double[,]) matrix.Clone();

		// Step 1
		activeMatrix[pivot.colPosition, pivot.rowPosition] = 1;

		// Step 2
		for (int i = 0; i < matrix.GetLength(0); i++)
		{
			if (i == pivot.rowPosition)
				continue;

			activeMatrix[pivot.colPosition, i] = -matrix[pivot.colPosition, i];
		}

		// Step 4
		for (int i = 0; i < matrix.GetLength(0); i++)
		{
			if (i == pivot.colPosition)
				continue;

			for (int j = 0; j < matrix.GetLength(1); j++)
			{
				if (j == pivot.rowPosition)
					continue;

				activeMatrix[i, j] = matrix[i, j] * pivot.value - matrix[i, pivot.rowPosition] * matrix[pivot.colPosition, j];
			}
		}

		// Step 5
		for (int i = 0; i < matrix.GetLength(0); i++)
		{
			for (int j = 0; j < matrix.GetLength(1); j++)
			{
				activeMatrix[i, j] /= pivot.value;
			}
		}

        // Step 6
        (rowLabels[pivot.rowPosition], colLabels[pivot.colPosition]) = (colLabels[pivot.colPosition], rowLabels[pivot.rowPosition]);
        return activeMatrix;
	}
	
	public static void FindInverseMatrix(double[,] matrix, string[] colLabels, string[] rowLabels)
	{
		System.Console.WriteLine("Given matrix: ");
		MatrixFunctions.PrintMatrix<double>(matrix, colLabels, rowLabels);

		for (int i = 0; i < matrix.GetLength(0); i++)
		{		
			if (matrix[i, i] == 0)
			{
				System.Console.Error.WriteLine("[Error] Pivot value can't be 0.");
				return;
			}	

			Pivot pivot = new(matrix[i, i], i, i);

			matrix = PerformJordanEliminationStep(matrix, pivot, ref colLabels, ref rowLabels);

			System.Console.WriteLine($"\nStep: {i + 1}");
			System.Console.WriteLine($"Pivot: {pivot.value,6:F2}");

			System.Console.WriteLine(i != matrix.GetLength(0) - 1 ? "Active matrix: " : "\nInverse matrix: ");

			MatrixFunctions.PrintMatrix<double>(matrix, colLabels, rowLabels);
		}
	}
	public static void FindMatrixRank(double[,] matrix, string[] colLabels, string[] rowLabels)
	{
		System.Console.WriteLine("Given matrix: ");
		MatrixFunctions.PrintMatrix<double>(matrix, colLabels, rowLabels);

		int rank = 0;

		for (int i = 0; i < matrix.GetLength(0); i++)
		{		
			if (matrix[i, i] == 0)
			{
				System.Console.Error.WriteLine("[Error] Pivot value can't be 0.");
				return;
			}				
			
			Pivot pivot = new(matrix[i, i], i, i);
			
			matrix = PerformJordanEliminationStep(matrix, pivot, ref colLabels, ref rowLabels);

			rank++;

			System.Console.WriteLine($"\nStep: {i + 1}");
			System.Console.WriteLine($"Pivot: {pivot.value,6:F2}");
			System.Console.WriteLine($"Active matrix: ");			

			MatrixFunctions.PrintMatrix<double>(matrix, colLabels, rowLabels);
		}

		System.Console.WriteLine($"Matrix rank: {rank}");
	}
	public static void InverseMatrixSolveSLAE(double[,] matrix, double[] vector, string[] colLabels, string[] rowLabels)
	{
		System.Console.WriteLine("Given matrix: ");
		MatrixFunctions.PrintMatrix<double>(matrix, colLabels, rowLabels);
		System.Console.WriteLine("Given vector: ");
		ArrayFunctions.PrintArray<double>(vector);
		System.Console.WriteLine();

		// Finding the inverse matrix 
		for (int i = 0; i < matrix.GetLength(0); i++)
		{
			if (matrix[i, i] == 0)
			{
				System.Console.Error.WriteLine("[Error] Pivot value can't be 0.");
				return;
			}	
			
			Pivot pivot = new(matrix[i, i], i, i);
	
			matrix = PerformJordanEliminationStep(matrix, pivot, ref colLabels, ref rowLabels);

			System.Console.WriteLine($"\nStep: {i + 1}");
			System.Console.WriteLine($"Pivot: {pivot.value,6:F2}");

			System.Console.WriteLine(i != matrix.GetLength(0) - 1 ? "Active matrix: " : "\nInverse matrix: ");

			MatrixFunctions.PrintMatrix<double>(matrix, colLabels, rowLabels);
		}

		// Soling SLAE using the inverse matrix
		System.Console.WriteLine("\nSolution: ");

		for (int i = 0; i < matrix.GetLength(0); i++)
		{
			double solution = 0;
			string[] solutionLog = new string[vector.Length];

			for (int j = 0; j < vector.Length; j++)
			{
				 solution += vector[j]*matrix[i, j];
				 solutionLog[j] = $"{vector[j],6:F2} *{matrix[i, j],6:F2}";
			}

			System.Console.WriteLine($"X{i+1} ={string.Join(" +", solutionLog)} ={solution,6:F2}");
		}
	}

    public static void Main() 
    {
		// Given data
        double[,] givenMatrix = new double[,]
		{
			{ -1, -2, 1 },
			{ 2, -3, 4 },
			{ -1, 3, 5 }
		};
		double[] givenVector = [4, 5 ,3];

		// Labels
		string[] colLabels = new string[givenMatrix.GetLength(0)];
		string[] rowLabels = new string[givenMatrix.GetLength(1)];
		for (int i = 0; i < givenMatrix.GetLength(0); i++)
		{
			colLabels[i] = $"x{i + 1}";
		}
		for (int i = 0; i < givenMatrix.GetLength(1); i++)
		{
			rowLabels[i] = $"b{i + 1}";
		}
		
		// Main program loop
		while (true)
		{
			System.Console.WriteLine("\nMenu:");
			System.Console.WriteLine("1 - Find the inverse matrix.");
			System.Console.WriteLine("2 - Find the rank of the matrix.");
			System.Console.WriteLine("3 - Solve SLAE using the inverse matrix.");
			System.Console.WriteLine("0 - Exit.");
			System.Console.Write	("Enter the option: ");
			
			string optionStr = System.Console.ReadLine() ?? string.Empty;

			switch (optionStr)
			{
				case "0":
					System.Console.WriteLine("\n[Exit] Exiting the program.");
					return;
				case "1":
					System.Console.WriteLine("\n[Running] Finding the inverse matrix.");

					FindInverseMatrix(givenMatrix, colLabels, rowLabels);
					break;
				case "2":
					System.Console.WriteLine("\n[Running] Finding the rank of the matrix.");

					FindMatrixRank(givenMatrix, colLabels, rowLabels);
					break;
				case "3":
					System.Console.WriteLine("\n[Running] Soling SLAE using the inverse matrix.");

					for (int i = 0; i < givenMatrix.GetLength(1); i++)
					{
						rowLabels[i] = givenVector[i].ToString();
					}

					InverseMatrixSolveSLAE(givenMatrix, givenVector, colLabels, rowLabels);
					break;
				default:
					System.Console.Error.WriteLine("[Error] Invalid option selected.");
					break;
			}
		}
    }
}
