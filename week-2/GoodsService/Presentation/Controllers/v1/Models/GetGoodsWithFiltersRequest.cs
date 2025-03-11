namespace GoodsService.Presentation.Controllers.v1.Models;

public class GetGoodsWithFiltersRequest
{
    public DateTime CreationDate { get; set; }
    public GoodType GoodType { get; set; }
    public int? NumberStock { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
}