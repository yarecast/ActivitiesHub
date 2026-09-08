using System;
using EventsHub.Api.Controllers;
using EventsHub.Domain;
using EventsHub.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EventsHub.Api.Controller;

public class EventsController(AppDbContext context) : EventsHubBaseContoller
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<Event>>> GetEventsAsync()
    {
        return await context.Events.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Event>> GetEventDetailAsync(string id)
    {
        var result = await context.Events.FindAsync(id);

        if (result == null)
        {
            return NotFound("The event was not found");
        }

        return result;
    }
}
