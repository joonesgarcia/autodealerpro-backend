using AutoDealerPro.Modules.Leads.Application.Exceptions;
using AutoDealerPro.Modules.Leads.Application.Interfaces;
using AutoDealerPro.Modules.Leads.Application.Requests;
using AutoDealerPro.Modules.Leads.Application.Services;
using AutoDealerPro.Modules.Leads.Core.Entities;
using AutoDealerPro.Modules.Leads.Core.Enums;
using AutoDealerPro.Modules.Leads.Core.Repositories;
using FluentAssertions;
using Moq;
using Xunit;

namespace AutoDealerPro.Modules.Leads.Application.Tests;

public class LeadsServiceTests
{
    private readonly Mock<ILeadRepository> _mockRepository = new();
    private readonly ILeadsService _service;

    public LeadsServiceTests()
    {
        _service = new LeadsService(_mockRepository.Object);
    }

    [Fact]
    public async Task CreateLeadAsync_WithValidRequest_ReturnsLeadDetailDto()
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

        _mockRepository.Setup(r => r.GetByEmailAsync(It.IsAny<string>()))
            .ReturnsAsync((Lead?)null);
        _mockRepository.Setup(r => r.AddAsync(It.IsAny<Lead>()))
            .ReturnsAsync((Lead lead) => lead);

        // Act
        var result = await _service.CreateLeadAsync(request);

        // Assert
        result.Should().NotBeNull();
        result.FirstName.Should().Be(request.FirstName);
        result.LastName.Should().Be(request.LastName);
        result.Email.Should().Be(request.Email.ToLowerInvariant());
        _mockRepository.Verify(r => r.GetByEmailAsync(It.IsAny<string>()), Times.Once);
        _mockRepository.Verify(r => r.AddAsync(It.IsAny<Lead>()), Times.Once);
    }

    [Fact]
    public async Task CreateLeadAsync_WithDuplicateEmail_ThrowsDuplicateLeadException()
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

        var existingLead = Lead.Create("Jane", "Doe", "john@example.com", "(12) 98765-4321",
            Guid.NewGuid(), LeadType.GeneralInquiry, "Message");

        _mockRepository.Setup(r => r.GetByEmailAsync(It.IsAny<string>()))
            .ReturnsAsync(existingLead);

        // Act & Assert
        await _service.Invoking(s => s.CreateLeadAsync(request))
            .Should().ThrowAsync<DuplicateLeadException>();
    }

    [Fact]
    public async Task CreateLeadAsync_WithInvalidLeadType_ThrowsArgumentException()
    {
        // Arrange
        var request = new CreateLeadRequest(
            FirstName: "John",
            LastName: "Doe",
            Email: "john@example.com",
            Phone: "(12) 98183-7450",
            VehicleId: Guid.NewGuid(),
            Type: "InvalidType",
            Message: "Interested");

        _mockRepository.Setup(r => r.GetByEmailAsync(It.IsAny<string>()))
            .ReturnsAsync((Lead?)null);

        // Act & Assert
        await _service.Invoking(s => s.CreateLeadAsync(request))
            .Should().ThrowAsync<ArgumentException>();
    }

    [Fact]
    public async Task GetLeadByIdAsync_WithExistingLead_ReturnsLeadDetailDto()
    {
        // Arrange
        var leadId = Guid.NewGuid();
        var lead = Lead.Create("John", "Doe", "john@example.com", "(12) 98183-7450", 
            Guid.NewGuid(), LeadType.GeneralInquiry, "Message");

        _mockRepository.Setup(r => r.GetByIdAsync(leadId))
            .ReturnsAsync(lead);

        // Act
        var result = await _service.GetLeadByIdAsync(leadId);

        // Assert
        result.Should().NotBeNull();
        result.FirstName.Should().Be(lead.FirstName);
        result.LastName.Should().Be(lead.LastName);
    }

    [Fact]
    public async Task GetLeadByIdAsync_WithNonExistentLead_ThrowsLeadNotFoundException()
    {
        // Arrange
        var leadId = Guid.NewGuid();

        _mockRepository.Setup(r => r.GetByIdAsync(leadId))
            .ReturnsAsync((Lead?)null);

        // Act & Assert
        await _service.Invoking(s => s.GetLeadByIdAsync(leadId))
            .Should().ThrowAsync<LeadNotFoundException>();
    }

    [Fact]
    public async Task GetAllLeadsAsync_WithLeads_ReturnsLeadListDtos()
    {
        // Arrange
        var leads = new List<Lead>
        {
            Lead.Create("John", "Doe", "john@example.com", "(12) 98183-7450", 
                Guid.NewGuid(), LeadType.GeneralInquiry, "Message"),
            Lead.Create("Jane", "Smith", "jane@example.com", "(11) 99876-5432", 
                Guid.NewGuid(), LeadType.TestDrive, "Message")
        };

        _mockRepository.Setup(r => r.GetAllAsync(1, 10))
            .ReturnsAsync(leads);

        // Act
        var result = await _service.GetAllLeadsAsync(1, 10);

        // Assert
        result.Should().HaveCount(2);
        result.First().FirstName.Should().Be("John");
        result.Last().FirstName.Should().Be("Jane");
    }

    [Fact]
    public async Task GetLeadsByStatusAsync_WithValidStatus_ReturnsFilteredLeads()
    {
        // Arrange
        var leads = new List<Lead>
        {
            Lead.Create("John", "Doe", "john@example.com", "(12) 98183-7450", 
                Guid.NewGuid(), LeadType.GeneralInquiry, "Message")
        };

        _mockRepository.Setup(r => r.GetByStatusAsync(LeadStatus.New, 1, 10))
            .ReturnsAsync(leads);

        // Act
        var result = await _service.GetLeadsByStatusAsync("New", 1, 10);

        // Assert
        result.Should().HaveCount(1);
    }

    [Fact]
    public async Task GetLeadsByStatusAsync_WithInvalidStatus_ThrowsArgumentException()
    {
        // Arrange
        // Act & Assert
        await _service.Invoking(s => s.GetLeadsByStatusAsync("InvalidStatus", 1, 10))
            .Should().ThrowAsync<ArgumentException>();
    }

    [Fact]
    public async Task GetLeadsByTypeAsync_WithValidType_ReturnsFilteredLeads()
    {
        // Arrange
        var leads = new List<Lead>
        {
            Lead.Create("John", "Doe", "john@example.com", "(12) 98183-7450", 
                Guid.NewGuid(), LeadType.TradeIn, "Message")
        };

        _mockRepository.Setup(r => r.GetByTypeAsync(LeadType.TradeIn, 1, 10))
            .ReturnsAsync(leads);

        // Act
        var result = await _service.GetLeadsByTypeAsync("TradeIn", 1, 10);

        // Assert
        result.Should().HaveCount(1);
    }

    [Fact]
    public async Task GetLeadsByTypeAsync_WithInvalidType_ThrowsArgumentException()
    {
        // Arrange
        // Act & Assert
        await _service.Invoking(s => s.GetLeadsByTypeAsync("InvalidType", 1, 10))
            .Should().ThrowAsync<ArgumentException>();
    }

    [Fact]
    public async Task AssignLeadToStaffAsync_WithNonExistentLead_ThrowsLeadNotFoundException()
    {
        // Arrange
        var leadId = Guid.NewGuid();
        var request = new AssignLeadRequest(Guid.NewGuid());

        _mockRepository.Setup(r => r.GetByIdAsync(leadId))
            .ReturnsAsync((Lead?)null);

        // Act & Assert
        await _service.Invoking(s => s.AssignLeadToStaffAsync(leadId, request))
            .Should().ThrowAsync<LeadNotFoundException>();
    }

    [Fact]
    public async Task AssignLeadToStaffAsync_WithExistingLead_AssignsAndUpdates()
    {
        // Arrange
        var leadId = Guid.NewGuid();
        var staffId = Guid.NewGuid();
        var lead = Lead.Create("John", "Doe", "john@example.com", "(12) 98183-7450", 
            Guid.NewGuid(), LeadType.GeneralInquiry, "Message");
        var request = new AssignLeadRequest(staffId);

        _mockRepository.Setup(r => r.GetByIdAsync(leadId))
            .ReturnsAsync(lead);
        _mockRepository.Setup(r => r.UpdateAsync(It.IsAny<Lead>()))
            .Returns(Task.CompletedTask);

        // Act
        await _service.AssignLeadToStaffAsync(leadId, request);

        // Assert
        lead.AssignedToStaffId.Should().Be(staffId);
        _mockRepository.Verify(r => r.UpdateAsync(lead), Times.Once);
    }

    [Fact]
    public async Task MarkLeadAsContactedAsync_WithNonExistentLead_ThrowsLeadNotFoundException()
    {
        // Arrange
        var leadId = Guid.NewGuid();
        var request = new MarkAsContactedRequest("Notes");

        _mockRepository.Setup(r => r.GetByIdAsync(leadId))
            .ReturnsAsync((Lead?)null);

        // Act & Assert
        await _service.Invoking(s => s.MarkLeadAsContactedAsync(leadId, request))
            .Should().ThrowAsync<LeadNotFoundException>();
    }

    [Fact]
    public async Task MarkLeadAsContactedAsync_WithExistingLead_MarkesAsContacted()
    {
        // Arrange
        var leadId = Guid.NewGuid();
        var notes = "Customer interested in financing";
        var lead = Lead.Create("John", "Doe", "john@example.com", "(12) 98183-7450", 
            Guid.NewGuid(), LeadType.GeneralInquiry, "Message");
        var request = new MarkAsContactedRequest(notes);

        _mockRepository.Setup(r => r.GetByIdAsync(leadId))
            .ReturnsAsync(lead);
        _mockRepository.Setup(r => r.UpdateAsync(It.IsAny<Lead>()))
            .Returns(Task.CompletedTask);

        // Act
        await _service.MarkLeadAsContactedAsync(leadId, request);

        // Assert
        lead.Status.Should().Be(LeadStatus.Contacted);
        lead.StaffNotes.Should().Be(notes);
        _mockRepository.Verify(r => r.UpdateAsync(lead), Times.Once);
    }

    [Fact]
    public async Task CloseLeadAsync_WithNonExistentLead_ThrowsLeadNotFoundException()
    {
        // Arrange
        var leadId = Guid.NewGuid();
        var request = new CloseLeadRequest(true);

        _mockRepository.Setup(r => r.GetByIdAsync(leadId))
            .ReturnsAsync((Lead?)null);

        // Act & Assert
        await _service.Invoking(s => s.CloseLeadAsync(leadId, request))
            .Should().ThrowAsync<LeadNotFoundException>();
    }

    [Fact]
    public async Task CloseLeadAsync_WithConvertedTrue_ClosesAsConverted()
    {
        // Arrange
        var leadId = Guid.NewGuid();
        var lead = Lead.Create("John", "Doe", "john@example.com", "(12) 98183-7450", 
            Guid.NewGuid(), LeadType.GeneralInquiry, "Message");
        var request = new CloseLeadRequest(true);

        _mockRepository.Setup(r => r.GetByIdAsync(leadId))
            .ReturnsAsync(lead);
        _mockRepository.Setup(r => r.UpdateAsync(It.IsAny<Lead>()))
            .Returns(Task.CompletedTask);

        // Act
        await _service.CloseLeadAsync(leadId, request);

        // Assert
        lead.Status.Should().Be(LeadStatus.Converted);
    }

    [Fact]
    public async Task CloseLeadAsync_WithConvertedFalse_ClosesAsLost()
    {
        // Arrange
        var leadId = Guid.NewGuid();
        var lead = Lead.Create("John", "Doe", "john@example.com", "(12) 98183-7450", 
            Guid.NewGuid(), LeadType.GeneralInquiry, "Message");
        var request = new CloseLeadRequest(false);

        _mockRepository.Setup(r => r.GetByIdAsync(leadId))
            .ReturnsAsync(lead);
        _mockRepository.Setup(r => r.UpdateAsync(It.IsAny<Lead>()))
            .Returns(Task.CompletedTask);

        // Act
        await _service.CloseLeadAsync(leadId, request);

        // Assert
        lead.Status.Should().Be(LeadStatus.Lost);
    }
}
