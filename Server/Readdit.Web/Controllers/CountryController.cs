using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Readdit.Services.Data.Countries;
using Readdit.Services.Data.Countries.Models;

namespace Readdit.Web.Controllers;

public class CountriesController : ApiController
{
    private readonly ICountryService _countryService;

    public CountriesController(ICountryService countryService)
        => _countryService = countryService;

    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType((int) HttpStatusCode.OK, Type = typeof(IEnumerable<CountryModel>))]
    public async Task<IActionResult> All()
        => Ok(await _countryService.GetAllAsync<CountryModel>());
}