using Newtonsoft.Json;
using VideoLibrary;

namespace StormBotSoundpadApi.Models
{
	public class AddMp3Body
	{
		[JsonProperty("source")]
		public string source;

		[JsonProperty("videoURL")]
		public string videoURL;

		[JsonProperty("soundName")]
		public string soundName;
	}
}
