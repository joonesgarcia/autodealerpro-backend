using AutoDealerPro.Modules.Leads.Core.Entities;
using AutoDealerPro.Modules.Leads.Core.Enums;
using AutoDealerPro.Modules.Leads.Infrastructure.Persistence;
using AutoDealerPro.Modules.Leads.Infrastructure.Repositories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace AutoDealerPro.Modules.Leads.Infrastructure.Tests;

public class LeadRepositoryTests : IDisposable
{
    private readonly LeadsDbContext _context;
    private readonly LeadRepository _repository;

    public LeadRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<LeadsDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        _context = new LeadsDbContext(options);
        _repository = new LeadRepository(_context);
    }

    [Fact]
    public async Task AddAsync_WithValidLead_InsertsAndReturnsLead()
    {
        // Arrange
        var lead = Lead.Create("John", "Doe", "john@example.com", "(12) 98183-7450", 
            Guid.NewGuid(), LeadType.GeneralInquiry, "Message");

        // Act
        var result = await _repository.AddAsync(lead);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().NotBeEmpty();
        result.Id.Should().Be(lead.Id);
    }

    [Fact]
    public async Task GetByIdAsync_WithExistingLead_ReturnsLead()
    {
        // Arrange
        var lead = Lead.Create("John", "Doe", "john@example.com", "(12) 98183-7450", 
            Guid.NewGuid(), LeadType.GeneralInquiry, "Message");
        await _repository.AddAsync(lead);

        // Act
        var result = await _repository.GetByIdAsync(lead.Id);

        // Assert
        result.Should().NotBeNull();
        result!.FirstName.Should().Be("John");
        result.LastName.Should().Be("Doe");
    }

    [Fact]
    public async Task GetByIdAsync_WithNonExistentLead_ReturnsNull()
    {
        // Arrange
        var leadId = Guid.NewGuid();

        // Act
        var result = await _repository.GetByIdAsync(leadId);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetByEmailAsync_WithExistingEmail_ReturnsLead()
    {
        // Arrange
        var email = "john@example.com";
        var lead = Lead.Create("John", "Doe", email, "(555) 123-4567", 
            Guid.NewGuid(), LeadType.GeneralInquiry, "Message");
        await _repository.AddAsync(lead);

        // Act
        var result = await _repository.GetByEmailAsync(email);

        // Assert
        result.Should().NotBeNull();
        result!.Email.Should().Be(email.ToLowerInvariant());
    }

    [Fact]
    public async Task GetByEmailAsync_WithNonExistentEmail_ReturnsNull()
    {
        // Arrange
        // Act
        var result = await _repository.GetByEmailAsync("nonexistent@example.com");

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetByEmailAsync_IsCaseInsensitive()
    {
        // Arrange
        var email = "john@example.com";
        var lead = Lead.Create("John", "Doe", email.ToUpper(), "(555) 123-4567", 
            Guid.NewGuid(), LeadType.GeneralInquiry, "Message");
        await _repository.AddAsync(lead);

        // Act
        var result = await _repository.GetByEmailAsync(email.ToLower());

        // Assert
        result.Should().NotBeNull();
    }

    [Fact]
    public async Task GetAllAsync_WithMultipleLeads_ReturnsPaginatedResults()
    {
        // Arrange
        var leads = Enumerable.Range(0, 15)
            .Select(i => Lead.Create($"First{i}", $"Last{i}", $"email{i}@example.com", 
                "(12) 98183-7450", Guid.NewGuid(), LeadType.GeneralInquiry, "Message"))
            .ToList();

        foreach (var lead in leads)
            await _repository.AddAsync(lead);

        // Act
        var result = await _repository.GetAllAsync(page: 1, pageSize: 10);

        // Assert
        result.Should().HaveCount(10);
    }

    [Fact]
    public async Task GetAllAsync_WithPagination_ReturnsCorrectPage()
    {
        // Arrange
        var leads = Enumerable.Range(0, 15)
            .Select(i => Lead.Create($"First{i}", $"Last{i}", $"email{i}@example.com", 
                "(12) 98183-7450", Guid.NewGuid(), LeadType.GeneralInquiry, "Message"))
            .ToList();

        foreach (var lead in leads)
            await _repository.AddAsync(lead);

        // Act
        var page1 = await _repository.GetAllAsync(page: 1, pageSize: 10);
        var page2 = await _repository.GetAllAsync(page: 2, pageSize: 10);

        // Assert
        page1.Should().HaveCount(10);
        page2.Should().HaveCount(5);
    }

    [Fact]
    public async Task GetByStatusAsync_WithExistingStatus_ReturnsFilteredLeads()
    {
        // Arrange
        var newLead = Lead.Create("John", "Doe", "john@example.com", "(12) 98183-7450", 
            Guid.NewGuid(), LeadType.GeneralInquiry, "Message");
        var assignedLead = Lead.Create("Jane", "Smith", "jane@example.com", "(11) 99876-5432", 
            Guid.NewGuid(), LeadType.TestDrive, "Message");
        assignedLead.AssignToStaff(Guid.NewGuid());

        await _repository.AddAsync(newLead);
        await _repository.AddAsync(assignedLead);

        // Act
        var result = await _repository.GetByStatusAsync(LeadStatus.New);

        // Assert
        result.Should().HaveCount(1);
        result.First().FirstName.Should().Be("John");
    }

    [Fact]
    public async Task UpdateAsync_WithExistingLead_UpdatesLead()
    {
        // Arrange
        var lead = Lead.Create("John", "Doe", "john@example.com", "(12) 98183-7450", 
            Guid.NewGuid(), LeadType.GeneralInquiry, "Message");
        await _repository.AddAsync(lead);

        var staffId = Guid.NewGuid();
        lead.AssignToStaff(staffId);

        // Act
        await _repository.UpdateAsync(lead);
        var updated = await _repository.GetByIdAsync(lead.Id);

        // Assert
        updated.Should().NotBeNull();
        updated!.AssignedToStaffId.Should().Be(staffId);
        updated.Status.Should().Be(LeadStatus.Assigned);
    }

    [Fact]
    public async Task DeleteAsync_WithExistingLead_RemovesLead()
    {
        // Arrange
        var lead = Lead.Create("John", "Doe", "john@example.com", "(12) 98183-7450", 
            Guid.NewGuid(), LeadType.GeneralInquiry, "Message");
        await _repository.AddAsync(lead);

        // Act
        await _repository.DeleteAsync(lead.Id);
        var result = await _repository.GetByIdAsync(lead.Id);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetAssignedToStaffAsync_WithExistingAssignments_ReturnsLeads()
    {
        // Arrange
        var staffId = Guid.NewGuid();
        var lead1 = Lead.Create("John", "Doe", "john@example.com", "(12) 98183-7450", 
            Guid.NewGuid(), LeadType.GeneralInquiry, "Message");
        lead1.AssignToStaff(staffId);
        var lead2 = Lead.Create("Jane", "Smith", "jane@example.com", "(11) 99876-5432", 
            Guid.NewGuid(), LeadType.TestDrive, "Message");
        lead2.AssignToStaff(staffId);

        await _repository.AddAsync(lead1);
        await _repository.AddAsync(lead2);

        // Act
        var result = await _repository.GetAssignedToStaffAsync(staffId);

        // Assert
        result.Should().HaveCount(2);
    }

    [Fact]
    public async Task GetByVehicleIdAsync_WithExistingVehicle_ReturnsLeads()
    {
        // Arrange
        var vehicleId = Guid.NewGuid();
        var lead1 = Lead.Create("John", "Doe", "john@example.com", "(12) 98183-7450", 
            vehicleId, LeadType.GeneralInquiry, "Message");
        var lead2 = Lead.Create("Jane", "Smith", "jane@example.com", "(11) 99876-5432", 
            vehicleId, LeadType.TestDrive, "Message");

        await _repository.AddAsync(lead1);
        await _repository.AddAsync(lead2);

        // Act
        var result = await _repository.GetByVehicleIdAsync(vehicleId);

        // Assert
        result.Should().HaveCount(2);
    }

    [Fact]
    public async Task GetCountAsync_WithMultipleLeads_ReturnsCorrectCount()
    {
        // Arrange
        var leads = Enumerable.Range(0, 5)
            .Select(i => Lead.Create($"First{i}", $"Last{i}", $"email{i}@example.com", 
                "(12) 98183-7450", Guid.NewGuid(), LeadType.GeneralInquiry, "Message"))
            .ToList();

        foreach (var lead in leads)
            await _repository.AddAsync(lead);

        // Act
        var count = await _repository.GetCountAsync();

        // Assert
        count.Should().Be(5);
    }

    public void Dispose()
    {
        _context?.Dispose();
    }
}
