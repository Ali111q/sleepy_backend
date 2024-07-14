
using BackEndStructuer.DATA.DTOs;
using BackEndStructuer.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BackEndStructuer.DATA.DTOs;
using BackEndStructuer.Entities;
using System.Threading.Tasks;
using GaragesStructure.Controllers;
using GaragesStructure.Helpers;

namespace BackEndStructuer.Controllers
{
[ServiceFilter(typeof(AuthorizeActionFilter))]
    public class MusicsController : BaseController
    {
        private readonly IMusicServices _musicServices;

        public MusicsController(IMusicServices musicServices)
        {
            _musicServices = musicServices;
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<List<MusicDto>>> GetAll([FromQuery] MusicFilter filter) => Ok(await _musicServices.GetAll(filter) , filter.PageNumber , filter.PageSize);

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<Music>> Create([FromBody] MusicForm musicForm) => Ok(await _musicServices.Create(musicForm));

        [Authorize]
        [HttpPut("{id}")]
        public async Task<ActionResult<Music>> Update([FromBody] MusicUpdate musicUpdate, Guid id) => Ok(await _musicServices.Update(id , musicUpdate));

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<ActionResult<Music>> Delete(Guid id) =>  Ok( await _musicServices.Delete(id));
        
    }
}
