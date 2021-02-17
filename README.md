# StormBot Soundpad API 1.0

StormBot Soundpad API was developed only to allow StormBot to handle Soundpad commands when the bot is being hosted remotely on a Raspberry Pi. Since the bot is not running on the same machine as Soundpad (a soundboard available on Steam), there is no way for it to utilize the SoundpadConnector API (made by https://github.com/medokin). SoundpadConnector was only developed to control the soundboard application locally. The StormBot Soundpad API is meant to be ran on same machine where the soundboard is running. It is recommended to run the API in IIS.

There are no plans to continue further development for more in-depth SoundpadConnector support.

The latest version 1.0 utilizes ASP.NET Core 5.0 and .NET 5.0 (latest .NET Core).

## Endpoints!

### GET /Soundpad/status

- Used to get the current connection status. 

### GET /Soundpad/category/{index}

- Used to get a specific category by index.

- Arguments: Index int passed in the route.

### GET /Soundpad/categories

- Used to get all categories.

### POST /Soundpad/addMp3

- Used to download a YouTube video onto the same machine as the Soundpad application.

- Arguments: AddMp3Body object { string source, string videoURL, string soundName } passed in body.

### POST /Soundpad/addSound

- Used to add an .mp3 file to a category in the Soundpad application.

- Arguments: AddSoundBody object { string path, int index, int categoryIndex } passed in body.

### GET /Soundpad/select/{index}

- Used to select a specific sound by index.

- Arguments: Index int passed in the route.

### GET /Soundpad/removeSelected

- Used to remove the current selected sound.

### GET /Soundpad/play/{index}

- Used to play a specific sound by index.

- Arguments: Index int passed in the route.

### GET /Soundpad/pause

- Used to pause the current playing sound.

### GET /Soundpad/stop

- Used to stop the current playing sound.
