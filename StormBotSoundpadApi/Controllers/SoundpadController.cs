using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SoundpadConnector.Response;
using StormBotSoundpadApi.Models;
using StormBotSoundpadApi.Services;
using VideoLibrary;

namespace StormBotSoundpadApi.Controllers
{
	[Route("[controller]")]
	[ApiController]
	public class SoundpadController : ControllerBase
	{
		private readonly ISoundpadService soundpadService;

		public SoundpadController(ISoundpadService soundpadService)
		{
			this.soundpadService = soundpadService;
		}

		[HttpGet]
		[Route("status")]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SoundpadConnector.ConnectionStatus))]
		public IActionResult GetConnectionStatus()
		{
			return Ok(soundpadService.GetConnectionStatus());
		}

		[HttpGet]
		[Route("category/{index}")]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CategoryResponse))]
		[ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
		[ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
		public async Task<IActionResult> GetCategory(int index)
		{
			if (index < 1)
				return BadRequest("Invalid index.");

			CategoryResponse categoryResponse = await soundpadService.GetCategory(index);

			return categoryResponse == null ? NotFound("Category does not exist.") : Ok(categoryResponse);
		}

		[HttpGet]
		[Route("categories")]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CategoryListResponse))]
		[ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
		public async Task<IActionResult> GetCategories()
		{
			CategoryListResponse categoryListResponse = await soundpadService.GetCategories();

			return categoryListResponse == null ? NotFound("No categories exist.") : Ok(categoryListResponse);
		}

		[HttpPost]
		[Route("addMp3")]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
		[ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
		public IActionResult SaveMP3([FromBody] AddMp3Body addMp3Body)
		{
			if (addMp3Body.source == "" || addMp3Body.source == null)
				return BadRequest("Invalid source.");

			if (addMp3Body.videoURL == "" || addMp3Body.videoURL == null)
				return BadRequest("Invalid video.");

			if (addMp3Body.soundName == "" || addMp3Body.soundName == null)
				return BadRequest("Invalid sound name.");

			YouTubeVideo video = soundpadService.GetYouTubeVideo(addMp3Body.videoURL);
			if (video == null)
				return BadRequest($"Invalid YouTube video URL.");

			bool saved = soundpadService.SaveMP3(addMp3Body.source, video, addMp3Body.soundName);

			return !saved ? BadRequest($"A sound with the name '{addMp3Body.soundName}' already exists in this category.") : Ok(saved);
		}

		[HttpPost]
		[Route("addSound")]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
		[ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
		public async Task <IActionResult> AddSound([FromBody] AddSoundBody addSoundBody)
		{
			if (addSoundBody.path == "" || addSoundBody.path == null)
				return BadRequest("Invalid path.");

			if (addSoundBody.index < 1)
				return BadRequest("Invalid index.");

			if (addSoundBody.categoryIndex < 1)
				return BadRequest("Invalid categoryIndex.");

			bool added = await soundpadService.AddSound(addSoundBody.path, addSoundBody.index, addSoundBody.categoryIndex);

			return !added ? BadRequest("Sound not added.") : Ok(added);
		}

		[HttpGet]
		[Route("select/{index}")]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
		[ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
		[ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
		public async Task<IActionResult> SelectIndex(int index)
		{
			if (index < 1)
				return BadRequest("Invalid index.");

			bool selected = await soundpadService.SelectIndex(index);

			return !selected ? NotFound("Sound not selected.") : Ok(selected);
		}

		[HttpGet]
		[Route("removeSelected")]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
		[ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
		public async Task<IActionResult> RemoveSelectedEntries()
		{
			bool removed = await soundpadService.RemoveSelectedEntries();

			return !removed ? BadRequest("No sound removed.") : Ok(removed);
		}

		[HttpGet]
		[Route("play/{index}")]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
		[ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
		[ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
		public async Task<IActionResult> PlaySound(int index)
		{
			if (index < 1)
				return BadRequest("Invalid index.");

			bool played = await soundpadService.PlaySound(index);

			return !played ? NotFound("Sound not played.") : Ok(played);
		}

		[HttpGet]
		[Route("pause")]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
		[ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
		public async Task<IActionResult> PauseSound()
		{
			bool paused = await soundpadService.PauseSound();

			return !paused ? BadRequest("No sound paused.") : Ok(paused);
		}

		[HttpGet]
		[Route("stop")]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
		[ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
		public async Task<IActionResult> StopSound()
		{
			bool stopped = await soundpadService.StopSound();

			return !stopped ? BadRequest("No sound stopped.") : Ok(stopped);
		}
	}
}
