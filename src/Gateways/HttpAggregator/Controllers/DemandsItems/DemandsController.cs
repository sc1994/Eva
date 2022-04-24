﻿using Eva.HttpAggregator.ServiceInterfaces;
using Microsoft.AspNetCore.Mvc;

namespace Eva.HttpAggregator.Controllers.DemandsItems;

[ApiController]
[Route("aggr/v1/[controller]")]
public class DemandsController : ControllerBase
{
    private readonly IDemandsService _demandsService;

    public DemandsController(IDemandsService demandsService)
    {
        _demandsService = demandsService;
    }

    [HttpGet]
    public async Task<string> Test()
    {
        return await _demandsService.CreateAsync(new {Name = "aaa"});
    }
}