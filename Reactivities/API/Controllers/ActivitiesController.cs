using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Activities;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActivitiesController : ControllerBase
    {
        public ActivitiesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        private readonly IMediator _mediator;

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public async Task<Unit> Delete(Guid id)
        {
            return await _mediator.Send(
                new Delete.Command
                {
                    Id = id
                });
        }

        // GET: api/Activities
        [HttpGet]
        public async Task<IEnumerable<Activity>> Get()
        {
            return await _mediator.Send(new List.Query());
        }

        // GET: api/Activities/5
        [HttpGet("{id}", Name = "Get")]
        public async Task<Activity> Get(Guid id)
        {
            return await _mediator.Send(new Details.Query(id));
        }

        // POST: api/Activities
        [HttpPost]
        public async Task<ActionResult<Unit>> Post([FromBody] Activity activity)
        {
            return await _mediator.Send(new Create.Command(activity));
        }

        // PUT: api/Activities/5
        [HttpPut("{id}")]
        public async Task<ActionResult<Unit>> Put(Guid id, [FromBody] Activity activity)
        {
            return await _mediator.Send(new Edit.Command(id, activity));
        }
    }
}