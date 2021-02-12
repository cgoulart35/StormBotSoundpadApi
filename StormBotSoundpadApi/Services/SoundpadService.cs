using System;
using System.IO;
using System.Threading.Tasks;
using SoundpadConnector;
using SoundpadConnector.Response;
using VideoLibrary;

namespace StormBotSoundpadApi.Services
{
	public class SoundpadService : ISoundpadService
    {
		public Soundpad _soundpad;

        private bool displayedConnectingMessage;
        
        public bool isSoundpadRunning { get; set; }

		public SoundpadService()
		{
			isSoundpadRunning = false;
            displayedConnectingMessage = false;
        }

		public async Task StartService()
		{
			_soundpad = new Soundpad();
			_soundpad.AutoReconnect = true;
			_soundpad.StatusChanged += SoundpadOnStatusChangedAsync;

			await _soundpad.ConnectAsync();
		}

        public void SoundpadOnStatusChangedAsync(object sender, EventArgs e)
        {
            string logStamp = DateTime.Now.ToString("HH:mm:ss ");

            if (_soundpad == null)
            {
                return;
            }
            else if (_soundpad.ConnectionStatus == ConnectionStatus.Connected)
            {
                displayedConnectingMessage = false;
                isSoundpadRunning = true;

                string message = "SOUNDBOARD CONNECTED.";
                Console.WriteLine(logStamp + message.PadLeft(72 - logStamp.Length));
            }
            else if (_soundpad.ConnectionStatus == ConnectionStatus.Disconnected && isSoundpadRunning)
            {
                displayedConnectingMessage = false;

                string message = "SOUNDBOARD DISCONNECTED.";
                Console.WriteLine(logStamp + message.PadLeft(75 - logStamp.Length));
            }
            else if (_soundpad.ConnectionStatus == ConnectionStatus.Connecting && isSoundpadRunning)
            {
                if (!displayedConnectingMessage)
                {
                    displayedConnectingMessage = true;
                    isSoundpadRunning = false;

                    string message = "Listening for the soundboard application...";
                    Console.WriteLine(logStamp + message.PadLeft(94 - logStamp.Length));
                }
            }
        }

        public ConnectionStatus GetConnectionStatus()
        {
            return _soundpad.ConnectionStatus;
        }

        public async Task<CategoryResponse> GetCategory(int index)
        {
            try
            {
                return await _soundpad.GetCategory(index);
            }
            catch
            {
                return null;
            }
        }

        public async Task<CategoryListResponse> GetCategories()
        {
            try
            {
                return await _soundpad.GetCategories(true);
            }
            catch
            {
                return null;
            }
        }

        public bool SaveMP3(string source, YouTubeVideo video, string soundName)
        {
            string fileName = source + soundName + ".mp4";
            File.WriteAllBytes(fileName, video.GetBytes());

            try
            {
                File.Move(fileName, Path.ChangeExtension(fileName, ".mp3"));
            }
            catch (IOException)
            {
                File.Delete(Path.Combine(source, fileName));
                return false;
            }

            return true;
        }

        public async Task<bool> AddSound(string path, int index, int categoryIndex)
        {
            try
            {
                await _soundpad.AddSound(path, index, categoryIndex);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> SelectIndex(int index)
        {
            try
            {
                await _soundpad.SelectIndex(index);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> RemoveSelectedEntries()
        {
            try
            {
                await _soundpad.RemoveSelectedEntries(true);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> PlaySound(int index)
        {
            try
            {
                await _soundpad.PlaySound(index);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> PauseSound()
        {
            try
            {
                await _soundpad.TogglePause();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> StopSound()
        {
            try
            {
                await _soundpad.StopSound();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}