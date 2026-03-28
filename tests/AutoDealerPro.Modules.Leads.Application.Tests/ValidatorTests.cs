using AutoDealerPro.Modules.Leads.Application.Requests;
using AutoDealerPro.Modules.Leads.Application.Validators;
using FluentAssertions;
using Xunit;

namespace AutoDealerPro.Modules.Leads.Application.Tests;

public class ValidatorTests
{
    private readonly CreateLeadValidator _createLeadValidator = new();
    private readonly AssignLeadValidator _assignLeadValidator = new();
    private readonly MarkAsContactedValidator _markAsContactedValidator = new();
    private readonly AddFollowUpValidator _addFollowUpValidator = new();

    [Fact]
    public async Task CreateLeadValidator_WithValidRequest_PassesValidation()
    {
        // Arrange
        var request = new CreateLeadRequest(
            FirstName: "John",
            LastName: "Doe",
            Email: "john@example.com",
            Phone: "(12) 98183-7450",
            VehicleId: Guid.NewGuid(),
            Type: "GeneralInquiry",
            Message: "Interested");

        // Act
        var result = await _createLeadValidator.ValidateAsync(request);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public async Task CreateLeadValidator_WithEmptyFirstName_FailsValidation()
    {
        // Arrange
        var request = new CreateLeadRequest(
            FirstName: "",
            LastName: "Doe",
            Email: "john@example.com",
            Phone: "(555) 123-4567",
            VehicleId: Guid.NewGuid(),
            Type: "GeneralInquiry",
            Message: "Interested");

        // Act
        var result = await _createLeadValidator.ValidateAsync(request);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle(e => e.PropertyName == "FirstName");
    }

    [Fact]
    public async Task CreateLeadValidator_WithFirstNameOver100Chars_FailsValidation()
    {
        // Arrange
        var request = new CreateLeadRequest(
            FirstName: new string('a', 101),
            LastName: "Doe",
            Email: "john@example.com",
            Phone: "(555) 123-4567",
            VehicleId: Guid.NewGuid(),
            Type: "GeneralInquiry",
            Message: "Interested");

        // Act
        var result = await _createLeadValidator.ValidateAsync(request);

        // Assert
        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public async Task CreateLeadValidator_WithInvalidEmail_FailsValidation()
    {
        // Arrange
        var request = new CreateLeadRequest(
            FirstName: "John",
            LastName: "Doe",
            Email: "not-an-email",
            Phone: "(555) 123-4567",
            VehicleId: Guid.NewGuid(),
            Type: "GeneralInquiry",
            Message: "Interested");

        // Act
        var result = await _createLeadValidator.ValidateAsync(request);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle(e => e.PropertyName == "Email");
    }

    [Fact]
    public async Task CreateLeadValidator_WithInvalidPhone_FailsValidation()
    {
        // Arrange
        var request = new CreateLeadRequest(
            FirstName: "John",
            LastName: "Doe",
            Email: "john@example.com",
            Phone: "123",
            VehicleId: Guid.NewGuid(),
            Type: "GeneralInquiry",
            Message: "Interested");

        // Act
        var result = await _createLeadValidator.ValidateAsync(request);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle(e => e.PropertyName == "Phone");
    }

    [Fact]
    public async Task CreateLeadValidator_WithInvalidLeadType_FailsValidation()
    {
        // Arrange
        var request = new CreateLeadRequest(
            FirstName: "John",
            LastName: "Doe",
            Email: "john@example.com",
            Phone: "(555) 123-4567",
            VehicleId: Guid.NewGuid(),
            Type: "InvalidType",
            Message: "Interested");

        // Act
        var result = await _createLeadValidator.ValidateAsync(request);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle(e => e.PropertyName == "Type");
    }

    [Fact]
    public async Task CreateLeadValidator_WithEmptyMessage_FailsValidation()
    {
        // Arrange
        var request = new CreateLeadRequest(
            FirstName: "John",
            LastName: "Doe",
            Email: "john@example.com",
            Phone: "(555) 123-4567",
            VehicleId: Guid.NewGuid(),
            Type: "GeneralInquiry",
            Message: "");

        // Act
        var result = await _createLeadValidator.ValidateAsync(request);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle(e => e.PropertyName == "Message");
    }

    [Fact]
    public async Task CreateLeadValidator_WithMessageOver1000Chars_FailsValidation()
    {
        // Arrange
        var request = new CreateLeadRequest(
            FirstName: "John",
            LastName: "Doe",
            Email: "john@example.com",
            Phone: "(555) 123-4567",
            VehicleId: Guid.NewGuid(),
            Type: "GeneralInquiry",
            Message: new string('a', 1001));

        // Act
        var result = await _createLeadValidator.ValidateAsync(request);

        // Assert
        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public async Task CreateLeadValidator_WithTradeInYearBefore1900_FailsValidation()
    {
        // Arrange
        var request = new CreateLeadRequest(
            FirstName: "John",
            LastName: "Doe",
            Email: "john@example.com",
            Phone: "(555) 123-4567",
            VehicleId: Guid.NewGuid(),
            Type: "TradeIn",
            Message: "Message",
            TradeInYear: 1899);

        // Act
        var result = await _createLeadValidator.ValidateAsync(request);

        // Assert
        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public async Task CreateLeadValidator_WithNegativeMileage_FailsValidation()
    {
        // Arrange
        var request = new CreateLeadRequest(
            FirstName: "John",
            LastName: "Doe",
            Email: "john@example.com",
            Phone: "(555) 123-4567",
            VehicleId: Guid.NewGuid(),
            Type: "TradeIn",
            Message: "Message",
            TradeInMileage: -100);

        // Act
        var result = await _createLeadValidator.ValidateAsync(request);

        // Assert
        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public async Task AssignLeadValidator_WithValidRequest_PassesValidation()
    {
        // Arrange
        var request = new AssignLeadRequest(Guid.NewGuid());

        // Act
        var result = await _assignLeadValidator.ValidateAsync(request);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public async Task AssignLeadValidator_WithEmptyGuid_FailsValidation()
    {
        // Arrange
        var request = new AssignLeadRequest(Guid.Empty);

        // Act
        var result = await _assignLeadValidator.ValidateAsync(request);

        // Assert
        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public async Task MarkAsContactedValidator_WithValidRequest_PassesValidation()
    {
        // Arrange
        var request = new MarkAsContactedRequest("Customer very interested");

        // Act
        var result = await _markAsContactedValidator.ValidateAsync(request);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public async Task MarkAsContactedValidator_WithEmptyNotes_FailsValidation()
    {
        // Arrange
        var request = new MarkAsContactedRequest("");

        // Act
        var result = await _markAsContactedValidator.ValidateAsync(request);

        // Assert
        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public async Task MarkAsContactedValidator_WithNotesOver500Chars_FailsValidation()
    {
        // Arrange
        var request = new MarkAsContactedRequest(new string('a', 501));

        // Act
        var result = await _markAsContactedValidator.ValidateAsync(request);

        // Assert
        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public async Task AddFollowUpValidator_WithValidRequest_PassesValidation()
    {
        // Arrange
        var request = new AddFollowUpRequest(
            Notes: "Follow up next week",
            NextFollowUpDate: DateTime.UtcNow.AddDays(7));

        // Act
        var result = await _addFollowUpValidator.ValidateAsync(request);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public async Task AddFollowUpValidator_WithEmptyNotes_FailsValidation()
    {
        // Arrange
        var request = new AddFollowUpRequest(
            Notes: "",
            NextFollowUpDate: DateTime.UtcNow.AddDays(7));

        // Act
        var result = await _addFollowUpValidator.ValidateAsync(request);

        // Assert
        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public async Task AddFollowUpValidator_WithPastFollowUpDate_FailsValidation()
    {
        // Arrange
        var request = new AddFollowUpRequest(
            Notes: "Follow up",
            NextFollowUpDate: DateTime.UtcNow.AddDays(-1));

        // Act
        var result = await _addFollowUpValidator.ValidateAsync(request);

        // Assert
        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public async Task AddFollowUpValidator_WithNullFollowUpDate_PassesValidation()
    {
        // Arrange
        var request = new AddFollowUpRequest(
            Notes: "Follow up",
            NextFollowUpDate: null);

        // Act
        var result = await _addFollowUpValidator.ValidateAsync(request);

        // Assert
        result.IsValid.Should().BeTrue();
    }
}
