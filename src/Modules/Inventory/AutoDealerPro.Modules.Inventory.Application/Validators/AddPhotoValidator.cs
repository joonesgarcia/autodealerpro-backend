using FluentValidation;

namespace AutoDealerPro.Modules.Inventory.Application.Validators;

public class AddPhotoValidator : AbstractValidator<Requests.AddPhotoRequest>
{
    public AddPhotoValidator()
    {
        RuleFor(x => x.PhotoUrl).NotEmpty().Must(url => Uri.TryCreate(url, UriKind.Absolute, out _));
    }
}
