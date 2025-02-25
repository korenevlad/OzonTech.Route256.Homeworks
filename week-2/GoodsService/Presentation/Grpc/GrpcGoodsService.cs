using GoodsService.BLL;
using GoodsService.Grps;
using GoodsService.Mappers;
using GoodsService.Mappers.Grpc;
using Grpc.Core;

namespace GoodsService.Presentation.Grpc;
public class GrpcGoodsService : GoodsServiceGrps.GoodsServiceGrpsBase
{
    private readonly IGoodService _goodService;
    public GrpcGoodsService(IGoodService goodService)
    {
        _goodService = goodService;
    }

    public override Task<AddGoodResponseProto> V1AddGood(AddGoodRequestProto request, ServerCallContext context)
    {
        var goodId = _goodService.AddGood(request.Price, request.Weight, request.GoodType.ToBLL(), request.NumberStock);
        return Task.FromResult(new AddGoodResponseProto { Id = goodId.ToString()});
    }

    public override Task<GetGoodsWithFiltersResponseProto> V1GetGoodsWithFilters(GetGoodsWithFiltersRequestProto request, ServerCallContext context)
    {
        return base.V1GetGoodsWithFilters(request, context);
    }

    public override Task<GetGoodByIdResponseProto> V1GetGoodById(GetGoodByIdRequestProto request, ServerCallContext context)
    {
        var good = _goodService.GetGoodById(Guid.Parse(request.Id));
        return Task.FromResult(new GetGoodByIdResponseProto(good.ToGetGoodByIdResponseProto()));
    }
}