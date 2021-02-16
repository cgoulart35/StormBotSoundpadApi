
using Newtonsoft.Json;

namespace StormBotSoundpadApi.Models
{
	public class AddSoundBody
	{
		[JsonProperty("path")]
		public string path;

		[JsonProperty("index")]
		public int index;

		[JsonProperty("categoryIndex")]
		public int categoryIndex;
	}
}
