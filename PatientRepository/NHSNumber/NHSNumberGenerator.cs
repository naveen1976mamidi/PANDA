using static System.Runtime.InteropServices.JavaScript.JSType;

namespace PatientAPI.NHSNumber
{

	public static class NHSNumberGenerator
	{
		private static readonly int[] Weights = new int[] { 10, 9, 8, 7, 6, 5, 4, 3, 2 }; // Weights for the first 9 digits

		/// <summary>
		///  calculate the checksum digit
		/// </summary>
		/// <param name="nhsPrefix"></param>
		/// <returns></returns>
		private static int CalculateChecksum(string nhsPrefix)
		{
			int sum = 0;


			// Sum the products of each digit and its corresponding weight
			for (int i = 0; i < nhsPrefix.Length; i++)
			{
				sum += (nhsPrefix[i] - '0') * Weights[i]; // Convert character to digit
			}

			// Calculate the remainder and checksum digit
			int remainder = sum % 11;
			int checksum = 11 - remainder;

			// Special handling for checksum values of 10 and 11
			if (checksum == 10)
				checksum = 1;
			if (checksum == 11)
				checksum = 0;

			return checksum;
		}

		/// <summary>
		/// Generate new NHS Number
		/// </summary>
		/// <returns></returns>
		public static string GenerateNHSNumber()
		{
			// Generate 9 random digits.
			Random random = new Random();
			string digits = new string(Enumerable.Range(0, 9).Select(i => random.Next(0, 10).ToString()[0]).ToArray());

			// Apply the Luhn algorithm (variation for NHS numbers).
			int[] weights = { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
			int sum = 0;

			for (int i = 0; i < 9; i++)
			{
				sum += (digits[i] - '0') * weights[i]; // Convert char to int and multiply
			}

			int remainder = sum % 11;
			int checkDigit = (11 - remainder) % 10; // Modulo 10 to get the check digit

			//  Return the 10-digit NHS number.
			return digits + checkDigit.ToString();

		}


	}
}
