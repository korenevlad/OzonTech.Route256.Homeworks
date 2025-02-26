using FluentValidation;
using GoodsService.Presentation.Controllers.v1.Models;

namespace GoodsService.Validators.Http;

public class GetGoodByIdRequestValidator : AbstractValidator<GetGoodByIdRequest>
{
    public GetGoodByIdRequestValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .Must(id => Guid.TryParse(id.ToString(), out _))
            .WithMessage("Некорректный или пустой Id!");
    }
}