using Application.CQRS.Users.Handlers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using static Application.CQRS.Users.Handlers.GetById;
using static Application.CQRS.Users.Handlers.Register;
using static Application.CQRS.Users.Handlers.Update;


namespace RestaurantManagement.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController(ISender sender) : ControllerBase
{
    private readonly ISender _sender = sender;

    [HttpGet]
    public async Task<IActionResult> GetById([FromQuery]int id)
    {
        var request = new Query(){Id=id };
        return Ok(await _sender.Send(request));
    }


    [HttpPost]
    public async Task<IActionResult> Register([FromBody] RegisterCommand request)
    {
        return Ok(await _sender.Send(request));
    }


    [HttpDelete]
    public async Task<IActionResult> Remove([FromQuery] int id)
    {
        var request = new Delete.Command() { Id = id };
        return Ok(await _sender.Send(request));
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] UpdateCommand request)
    {
        return Ok(await _sender.Send(request));
    }

}