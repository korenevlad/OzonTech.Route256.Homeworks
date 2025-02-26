using GoodsService.BLL;
using GoodsService.Mappers;
using GoodsService.Presentation.Controllers.v1.Models;
using Microsoft.AspNetCore.Mvc;

namespace GoodsService.Presentation.Controllers.v1;
[ApiController]
[Route("v1/[controller]")]
[Produces("application/json")]
public class GoodsServiceController : ControllerBase
{
    private readonly IGoodService _goodService;
    public GoodsServiceController(IGoodService goodService)
    {
        _goodService = goodService;
    }
    
    [HttpPost("[action]")]
    public IActionResult AddGood(AddGoodRequest request)
    {
        var id = _goodService.AddGood(request.Name, request.Price, request.Weight, request.GoodType.ToBll(), request.NumberStock);
        return Ok(id);
    }
    
    [HttpGet("[action]")]
    public IActionResult GetGoodsWithFilters(DateTime? creationDate, GoodType goodType, int? numberStock, int pageNumber, int pageSize)
    {
        var goods = _goodService.GetGoodsWithFilters(
            creationDate, goodType.ToBll(),numberStock, pageNumber, pageSize);
        return Ok(goods);
    }

    [HttpGet("[action]")]
    public IActionResult GetGoodById(Guid id)
    {
        var good = _goodService.GetGoodById(id);
        return Ok(good.ToGetGoodByIdResponse());
    }
}