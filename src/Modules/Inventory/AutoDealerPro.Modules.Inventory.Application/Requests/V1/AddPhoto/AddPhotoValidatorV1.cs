using FluentValidation;

namespace AutoDealerPro.Modules.Inventory.Application.Requests.V1.AddPhoto;

public class AddPhotoValidatorV1 : AbstractValidator<AddPhotoRequestV1>
{
    public AddPhotoValidatorV1()
    {
        RuleFor(x => x.PhotoUrl).NotEmpty().Must(url => Uri.TryCreate(url, UriKind.Absolute, out _));
    }
}
