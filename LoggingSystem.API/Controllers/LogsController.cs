using AutoMapper;
using LoggingSystem.API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace LoggingSystem.API.Controllers
{
    [Route("v1/logs")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class LogsController : ControllerBase
    {
        private readonly ILogRepository _logRepository;
        private readonly IMapper _mapper;

        public LogsController(ILogRepository logRepository, IMapper mapper)
        {
            _logRepository = logRepository;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateLogDto createModel)
        {
            var logDomain = _mapper.Map<Log>(createModel);
            var result = await _logRepository.CreateAsync(logDomain);

            if (result is null)
                return BadRequest(ModelState);

            
            return CreatedAtAction(nameof(GetById)
                                    , new { id = logDomain.Id }
                                    , _mapper.Map<LogDto>(result));
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var logDomain = await _logRepository.GetAsync(id);

            if (logDomain is null)
            {
                return NotFound();  
            }

            return Ok(_mapper.Map<LogDto>(logDomain));
        }


        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string? service, [FromQuery] string? level, [FromQuery] DateTime? start_time, [FromQuery] DateTime? end_time)
        {
            var logsDomain = await _logRepository.GetAllAsync(service, level, start_time, end_time);

            return Ok(_mapper.Map<List<LogDto>>(logsDomain));
        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var logDomain = await _logRepository.DeleteAsync(id);

            if (logDomain is null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<LogDto>(logDomain));
        }

        [HttpPut]
        [Route("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateLogDto updateModel)
        {
            var logDomain = _mapper.Map<Log>(updateModel);

            logDomain = await _logRepository.UpdateAsync(id, logDomain);
            if (logDomain is null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<LogDto>(logDomain));
        }

    }
}
