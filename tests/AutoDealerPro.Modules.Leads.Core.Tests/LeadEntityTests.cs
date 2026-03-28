using AutoDealerPro.Modules.Leads.Core.Entities;
using AutoDealerPro.Modules.Leads.Core.Enums;
using FluentAssertions;
using Xunit;

namespace AutoDealerPro.Modules.Leads.Core.Tests;

public class LeadEntityTests
{
    [Fact]
    public void Create_WithValidData_ReturnsLeadWithCorrectInitialState()
    {
        // Arrange
        var firstName = "John";
        var lastName = "Doe";
        var email = "JOHN@EXAMPLE.COM";
        var phone = "(12) 98183-7450";
        var vehicleId = Guid.NewGuid();
        var leadType = LeadType.GeneralInquiry;
        var message = "Interested in the vehicle";

        // Act
        var lead = Lead.Create(firstName, lastName, email, phone, vehicleId, leadType, message);

        // Assert
        lead.FirstName.Should().Be(firstName);
        lead.LastName.Should().Be(lastName);
        lead.Email.Should().Be(email.ToLowerInvariant());
        lead.Phone.Should().Be(phone);
        lead.VehicleId.Should().Be(vehicleId);
        lead.Type.Should().Be(leadType);
        lead.Message.Should().Be(message);
        lead.Status.Should().Be(LeadStatus.New);
        lead.AssignedToStaffId.Should().BeNull();
        lead.ContactedAt.Should().BeNull();
        lead.Id.Should().NotBeEmpty();
    }

    [Fact]
    public void Create_WithTradeInData_StoresTradeInInfo()
    {
        // Arrange
        var tradeInMake = "Honda";
        var tradeInModel = "Civic";
        var tradeInYear = 2020;
        var tradeInMileage = 45000;

        // Act
        var lead = Lead.Create(
            "John", "Doe", "john@example.com", "(12) 98183-7450",
            Guid.NewGuid(), LeadType.TradeIn, "Message",
            tradeInMake, tradeInModel, tradeInYear, tradeInMileage);

        // Assert
        lead.TradeInMake.Should().Be(tradeInMake);
        lead.TradeInModel.Should().Be(tradeInModel);
        lead.TradeInYear.Should().Be(tradeInYear);
        lead.TradeInMileage.Should().Be(tradeInMileage);
    }

    [Fact]
    public void AssignToStaff_WithValidStaffId_UpdatesStatusAndStaffId()
    {
        // Arrange
        var lead = Lead.Create("John", "Doe", "john@example.com", "(12) 98183-7450",
            Guid.NewGuid(), LeadType.GeneralInquiry, "Message");
        var staffId = Guid.NewGuid();

        // Act
        lead.AssignToStaff(staffId);

        // Assert
        lead.AssignedToStaffId.Should().Be(staffId);
        lead.Status.Should().Be(LeadStatus.Assigned);
        lead.UpdatedAt.Should().NotBeNull();
    }

    [Fact]
    public void MarkAsContacted_WithNotes_UpdatesStatusAndNotes()
    {
        // Arrange
        var lead = Lead.Create("John", "Doe", "john@example.com", "(12) 98183-7450",
            Guid.NewGuid(), LeadType.GeneralInquiry, "Message");
        var notes = "Customer interested in financing options";

        // Act
        lead.MarkAsContacted(notes);

        // Assert
        lead.Status.Should().Be(LeadStatus.Contacted);
        lead.StaffNotes.Should().Be(notes);
        lead.ContactedAt.Should().NotBeNull();
        lead.UpdatedAt.Should().NotBeNull();
    }

    [Fact]
    public void AddFollowUp_WithNotes_AddsFollowUpToCollection()
    {
        // Arrange
        var lead = Lead.Create("John", "Doe", "john@example.com", "(12) 98183-7450",
            Guid.NewGuid(), LeadType.GeneralInquiry, "Message");
        var notes = "Follow up in 3 days";
        var nextFollowUpDate = DateTime.UtcNow.AddDays(3);

        // Act
        lead.AddFollowUp(notes, nextFollowUpDate);

        // Assert
        lead.FollowUps.Should().HaveCount(1);
        lead.FollowUps[0].Notes.Should().Be(notes);
        lead.FollowUps[0].NextFollowUpDate.Should().Be(nextFollowUpDate);
        lead.UpdatedAt.Should().NotBeNull();
    }

    [Fact]
    public void MarkAsClosed_WithConvertedTrue_SetStatusToConverted()
    {
        // Arrange
        var lead = Lead.Create("John", "Doe", "john@example.com", "(12) 98183-7450",
            Guid.NewGuid(), LeadType.GeneralInquiry, "Message");

        // Act
        lead.MarkAsClosed(converted: true);

        // Assert
        lead.Status.Should().Be(LeadStatus.Converted);
        lead.UpdatedAt.Should().NotBeNull();
    }

    [Fact]
    public void MarkAsClosed_WithConvertedFalse_SetStatusToLost()
    {
        // Arrange
        var lead = Lead.Create("John", "Doe", "john@example.com", "(12) 98183-7450",
            Guid.NewGuid(), LeadType.GeneralInquiry, "Message");

        // Act
        lead.MarkAsClosed(converted: false);

        // Assert
        lead.Status.Should().Be(LeadStatus.Lost);
    }
}
