namespace PatientAPI.NHSNumber
{
	public static class NHSNumberChecker
	{
		/// <summary>
		/// Valididate NHS number  
		/// </summary>
		/// <param name="nhsNumber"></param>
		/// <returns></returns>
		public static bool IsValidNHSNumber(string nhsNumber)
		{
			// Ensure the NHS number is exactly 10 digits
			if (nhsNumber.Length != 10 || !long.TryParse(nhsNumber, out _))
			{
				return false;
			}

			// The first 9 digits for the checksum calculation
			string firstNineDigits = nhsNumber.Substring(0, 9);

			// Weights for each digit
			int[] weights = { 10, 9, 8, 7, 6, 5, 4, 3, 2 };

			int sum = 0;

			// Calculate weighted sum of first 9 digits
			for (int i = 0; i < 9; i++)
			{
				sum += (firstNineDigits[i] - '0') * weights[i];
			}

			// The 10th digit is the checksum, which should be (10 - (sum % 11)) % 10
			int checksum = (10 - (sum % 11)) % 10 + 1;

			// Compare the calculated checksum with the 10th digit
			return nhsNumber[9] == checksum.ToString()[0];
		}

	}
}
