using FluentValidation;
using GoodsService.Grps;

namespace GoodsService.Validators;
public class AddGoodRequestGrpcValidator : AbstractValidator<AddGoodRequestProto>
{
    public AddGoodRequestGrpcValidator()
    {
        RuleFor(x => x.Price)
            .GreaterThan(0)
            .WithMessage("Цена должна быть больше 0!");
    }
}