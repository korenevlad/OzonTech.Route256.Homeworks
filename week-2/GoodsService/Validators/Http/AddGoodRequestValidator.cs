using FluentValidation;
using GoodsService.Presentation.Controllers.v1.Models;

namespace GoodsService.Validators.Http;
public class AddGoodRequestValidator : AbstractValidator<AddGoodRequest>
{
    public AddGoodRequestValidator()
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
        
        RuleFor(x => x.GoodType)
            .NotEqual(GoodType.Unknown)
            .WithMessage("Нельзя создать товар без выбранного типа товара!");
        
        RuleFor(x => x.NumberStock)
            .GreaterThan(0)
            .WithMessage("Номер склада не может быть отрицательным или равен нулю!");
    }
}