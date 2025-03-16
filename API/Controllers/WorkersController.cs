using API.Mappers;
using Contracts.Workers;
using DataAccess.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/worker")]
    [ApiController]
    public class WorkersController : ControllerBase
    {
        private readonly IWorkersRepository _workersRepository;

        public WorkersController(IWorkersRepository workersRepository)
        {
            _workersRepository = workersRepository;
        }

        [HttpGet]
        public async Task<ActionResult<List<WorkerResponse>>> GetAll()
        {
            var workers = await _workersRepository.GetAllAsync();

            var response = workers.Select(w => w.ToResponse());
            return Ok(response);
        }

        [HttpGet("{workerId}")]
        public async Task<ActionResult<Worker>> GetById([FromRoute] int workerId)
        {
            var worker = await _workersRepository.GetById(workerId);
            if (worker == null) return NotFound();

            var response = worker.ToResponse();
            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult<WorkerResponse>> Create([FromBody] CreateWorkerRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var created = await _workersRepository.CreateAsync(request.ToEntity());
            if (created == null) return BadRequest();

            var response = created.ToResponse();

            return CreatedAtAction(nameof(Create), response);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<WorkerResponse>> Update([FromRoute] int id, [FromBody] UpdateWorkerRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var updated = await _workersRepository.UpdateAsync(id, request.ToEntity());
            if (updated == null) return BadRequest();

            var response = updated.ToResponse();

            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete([FromRoute] int id)
        {
            if (!await _workersRepository.DeleteAsync(id)) return NotFound();

            return NoContent();
        }
    }
}
