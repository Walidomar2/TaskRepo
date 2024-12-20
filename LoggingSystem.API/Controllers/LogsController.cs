﻿using AutoMapper;
using LoggingSystem.API.Action_Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LoggingSystem.API.Controllers
{
    [Route("v1/logs")]
    [ApiController]
    [Authorize(Roles ="Admin")]
    public class LogsController : ControllerBase
    {
        private readonly ILogRepository _logRepository;
        private readonly IMapper _mapper;
        private readonly ILogStorage _logStorage;

        public LogsController(ILogRepository logRepository
                              , IMapper mapper
                              , ILogStorage logStorage)
        {
            _logRepository = logRepository;
            _mapper = mapper;
            _logStorage = logStorage;
        }

        [HttpPost]
        [ValidateModel]
        public async Task<IActionResult> Create([FromBody] CreateLogDto createModel)
        {
            var logDomain = _mapper.Map<Log>(createModel);
            var result = await _logRepository.CreateAsync(logDomain);

            // it will store the log into the local database 

           // await _logStorage.SaveLogAsync(logDomain);

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
        public async Task<IActionResult> GetAll([FromQuery] string? service, [FromQuery] string? level,
            [FromQuery] DateTime? start_time, [FromQuery] DateTime? end_time,
             [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 1000)
        {
            var logsDomain = await _logRepository.GetAllAsync(service, level, start_time
                                                    , end_time,pageNumber, pageSize);

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
        [ValidateModel]
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
