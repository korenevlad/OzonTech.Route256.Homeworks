using FluentValidation;
using GoodsService.Grps;

namespace GoodsService.Validators.Grpc;
public class GetGoodByIdGrpcValidator : AbstractValidator<GetGoodByIdRequestProto>
{
    public GetGoodByIdGrpcValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .Must(id => Guid.TryParse(id.ToString(), out _))
            .WithMessage("Некорректный или пустой Id!");
    }
}