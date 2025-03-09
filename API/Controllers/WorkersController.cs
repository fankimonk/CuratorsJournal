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
    }
}
