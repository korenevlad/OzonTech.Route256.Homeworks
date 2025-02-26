using FluentValidation;
using GoodsService.Grps;
using Google.Protobuf.WellKnownTypes;

namespace GoodsService.Validators.Grpc;
public class GetGoodsWithFiltersGrpcValidator : AbstractValidator<GetGoodsWithFiltersRequestProto>
{
    public GetGoodsWithFiltersGrpcValidator()
    {
        RuleFor(x => x.CreationDate)
            .Must(BeValidTimestamp)
            .When(x => x.CreationDate != null)
            .WithMessage("Дата не может быть позже текущего времени!");
            
        RuleFor(x => x.NumberStock)
            .GreaterThan(0)
            .When(x => x.NumberStock.HasValue)
            .WithMessage("Номер склада должен быть больше 0!");
        
        RuleFor(x => x.PageNumber)
            .GreaterThan(0)
            .WithMessage("Количество страниц должно быть больше 0!");
        
        RuleFor(x => x.PageSize)
            .GreaterThan(0)
            .WithMessage("Количество строк должно быть больше 0!");
    }
    
    private bool BeValidTimestamp(Timestamp timestamp) 
        => timestamp.ToDateTime() < DateTime.UtcNow;
}