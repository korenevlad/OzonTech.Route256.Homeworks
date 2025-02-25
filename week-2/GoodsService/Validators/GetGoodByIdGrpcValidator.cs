using FluentValidation;
using GoodsService.Grps;

namespace GoodsService.Validators;
public class GetGoodByIdGrpcValidator : AbstractValidator<GetGoodByIdRequestProto>
{
    public GetGoodByIdGrpcValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id не должен быть пустым!");
    }
}