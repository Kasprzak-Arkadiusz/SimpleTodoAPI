﻿using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BaseController : ControllerBase
{
    protected readonly IMediator Mediator;
    
    public BaseController(IMediator mediator)
    {
        Mediator = mediator;
    }

    public BaseController()
    {
        
    }
}