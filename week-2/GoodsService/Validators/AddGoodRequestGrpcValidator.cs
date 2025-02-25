using FluentValidation;
using GoodsService.Grps;

namespace GoodsService.Validators;
public class AddGoodRequestGrpcValidator : AbstractValidator<AddGoodRequestProto>
{
    public AddGoodRequestGrpcValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Имя не может быть не заполнено!");
        RuleFor(x => x.Price)
            .GreaterThan(0)
            .WithMessage("Цена должна быть больше 0!");
        RuleFor(x => x.Weight)
            .GreaterThan(0)
            .WithMessage("Вес должна быть больше 0!");
        RuleFor(x => x.NumberStock)
            .GreaterThan(0)
            .WithMessage("Номер склада не может быть отрицательным или равен нулю!");
    }
}