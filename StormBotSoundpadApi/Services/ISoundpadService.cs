using System;
using System.Threading.Tasks;
using SoundpadConnector;
using SoundpadConnector.Response;
using VideoLibrary;

namespace StormBotSoundpadApi.Services
{
	public interface ISoundpadService
	{
        public Task StartService();

        public void SoundpadOnStatusChangedAsync(object sender, EventArgs e);

        public ConnectionStatus GetConnectionStatus();

        public Task<CategoryResponse> GetCategory(int index);

        public Task<CategoryListResponse> GetCategories();

        public bool SaveMP3(string source, YouTubeVideo video, string soundName);

        public Task<bool> AddSound(string path, int index, int categoryIndex);

        public Task<bool> SelectIndex(int index);

        public Task<bool> RemoveSelectedEntries();

        public Task<bool> PlaySound(int index);

        public Task<bool> PauseSound();

        public Task<bool> StopSound();
    }
}
