using System.Text.RegularExpressions;

namespace PatientAPI.Validations
{
	public static class PostCodeChecker
	{
		//The postcode can be split into two parts: the outward code(A1A) and the inward code(1AA).
		//The outward code starts with one or two uppercase letters, followed by a digit, optionally another alphanumeric character.
		//The inward code consists of a digit, followed by two uppercase letters.
		//The regex also accounts for the space between the outward and inward codes.

		/// <summary>
		/// The regular expression @"^([A-Z]{1,2}\d[A-Z\d]? \d[A-Z]{2})$" captures UK postcode formats
		/// </summary>
		/// <param name="postcode"></param>
		/// <returns></returns>
		public static bool IsValidUKPostcode(string postcode)
		{
			// Define the regex pattern for UK postcodes
			string pattern = @"^([A-Z]{1,2}\d[A-Z\d]? \d[A-Z]{2})$";

			// Check if the postcode matches the regex pattern (ignoring case)
			Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);

			return regex.IsMatch(postcode);
		}

	}
}
