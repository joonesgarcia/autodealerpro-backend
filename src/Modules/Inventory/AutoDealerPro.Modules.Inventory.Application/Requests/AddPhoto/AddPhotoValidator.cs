using FluentValidation;

namespace AutoDealerPro.Modules.Inventory.Application.Requests.AddPhoto;

public class AddPhotoValidator : AbstractValidator<AddPhotoRequest>
{
    public AddPhotoValidator()
    {
        RuleFor(x => x.PhotoUrl).NotEmpty().Must(url => Uri.TryCreate(url, UriKind.Absolute, out _));
    }
}
