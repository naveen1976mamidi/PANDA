namespace PatientAPI.Filehandlers
{
	using Newtonsoft.Json;
	using System.IO;
	using System.Threading.Tasks;

	public class JsonFileHandler
	{
		private readonly string _filePath;
		
		private static readonly JsonSerializerSettings _options = new() { NullValueHandling = NullValueHandling.Ignore };

		public JsonFileHandler(string filePath)
		{
			_filePath = filePath;
		}

		public async Task<T> ReadAsync<T>()
		{
			using (var stream = new FileStream(_filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
			{
				return await System.Text.Json.JsonSerializer.DeserializeAsync<T>(stream);

			}
		}

		public async Task WriteAsync<T>(T data)
		{
			using (var stream = new FileStream(_filePath, FileMode.Create, FileAccess.Write, FileShare.None))
			{
				await System.Text.Json.JsonSerializer.SerializeAsync(stream, data);
			}
		}


	

		public async  Task  WriteToJSONFile(object obj, string fileName)
		{
			var jsonString = JsonConvert.SerializeObject(obj, _options);
			File.WriteAllText(fileName, jsonString);
		
		}
	}

	
}
