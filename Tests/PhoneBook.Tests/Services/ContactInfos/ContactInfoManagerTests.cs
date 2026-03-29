using FluentAssertions;
using Moq;
using PhoneBook.Domain.Entities;
using PhoneBook.Domain.Enums;
using PhoneBook.Infrastructure.Repositories;
using PhoneBook.Services.ContactInfos;

namespace PhoneBook.Tests.Services.ContactInfos;

public class ContactInfoManagerTests
{
    private readonly Mock<IContactInfoRepository> _contactInfoRepositoryMock;
    private readonly ContactInfoManager _contactInfoManager;

    public ContactInfoManagerTests()
    {
        _contactInfoRepositoryMock = new Mock<IContactInfoRepository>();
        _contactInfoManager = new ContactInfoManager(_contactInfoRepositoryMock.Object);
    }

    [Fact]
    public async Task GetByContactIdAsync_Should_Return_ContactInfos()
    {
        // Arrange
        var contactId = Guid.NewGuid();

        var contactInfos = new List<ContactInfo>
        {
            new ContactInfo(contactId, ContactInfoType.Phone, "05551234567"),
            new ContactInfo(contactId, ContactInfoType.Email, "ahmet@example.com")
        };

        _contactInfoRepositoryMock
            .Setup(x => x.GetByContactIdAsync(contactId))
            .ReturnsAsync(contactInfos);

        // Act
        var result = await _contactInfoManager.GetByContactIdAsync(contactId);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(contactInfos.Count);
        result.Should().BeEquivalentTo(contactInfos);
        _contactInfoRepositoryMock.Verify(x => x.GetByContactIdAsync(contactId), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_Should_Add_ContactInfo_And_Return_It_When_Valid_Entity_Is_Provided()
    {
        // Arrange
        var contactId = Guid.NewGuid();
        var contactInfo = new ContactInfo(contactId, ContactInfoType.Phone, "05551234567");

        _contactInfoRepositoryMock
            .Setup(x => x.AddAsync(contactInfo))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _contactInfoManager.CreateAsync(contactInfo);

        // Assert
        result.Should().Be(contactInfo);
        _contactInfoRepositoryMock.Verify(x => x.AddAsync(contactInfo), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_Should_Throw_ArgumentNullException_When_Entity_Is_Null()
    {
        // Act
        Func<Task> act = async () => await _contactInfoManager.CreateAsync(null!);

        // Assert
        await act.Should().ThrowAsync<ArgumentNullException>()
            .WithParameterName("contactInfo");

        _contactInfoRepositoryMock.Verify(x => x.AddAsync(It.IsAny<ContactInfo>()), Times.Never);
    }

    [Fact]
    public async Task DeleteAsync_Should_Delete_ContactInfo_When_Entity_Exists()
    {
        // Arrange
        var id = Guid.NewGuid();
        var entity = new ContactInfo(Guid.NewGuid(), ContactInfoType.Phone, "05551234567");

        _contactInfoRepositoryMock
            .Setup(x => x.GetByIdAsync(id))
            .ReturnsAsync(entity);

        _contactInfoRepositoryMock
            .Setup(x => x.DeleteAsync(id))
            .Returns(Task.CompletedTask);

        // Act
        await _contactInfoManager.DeleteAsync(id);

        // Assert
        _contactInfoRepositoryMock.Verify(x => x.GetByIdAsync(id), Times.Once);
        _contactInfoRepositoryMock.Verify(x => x.DeleteAsync(id), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_Should_Throw_KeyNotFoundException_When_Entity_Does_Not_Exist()
    {
        // Arrange
        var id = Guid.NewGuid();

        _contactInfoRepositoryMock
            .Setup(x => x.GetByIdAsync(id))
            .ReturnsAsync((ContactInfo?)null);

        // Act
        Func<Task> act = async () => await _contactInfoManager.DeleteAsync(id);

        // Assert
        await act.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage($"Contact info not found. Id: {id}");

        _contactInfoRepositoryMock.Verify(x => x.GetByIdAsync(id), Times.Once);
        _contactInfoRepositoryMock.Verify(x => x.DeleteAsync(It.IsAny<Guid>()), Times.Never);
    }

    [Fact]
    public async Task SoftDeleteAsync_Should_SoftDelete_ContactInfo_When_Entity_Exists()
    {
        // Arrange
        var id = Guid.NewGuid();
        var entity = new ContactInfo(Guid.NewGuid(), ContactInfoType.Phone, "05551234567");

        _contactInfoRepositoryMock
            .Setup(x => x.GetByIdAsync(id))
            .ReturnsAsync(entity);

        _contactInfoRepositoryMock
            .Setup(x => x.SoftDeleteAsync(id))
            .Returns(Task.CompletedTask);

        // Act
        await _contactInfoManager.SoftDeleteAsync(id);

        // Assert
        _contactInfoRepositoryMock.Verify(x => x.GetByIdAsync(id), Times.Once);
        _contactInfoRepositoryMock.Verify(x => x.SoftDeleteAsync(id), Times.Once);
    }

    [Fact]
    public async Task SoftDeleteAsync_Should_Throw_KeyNotFoundException_When_Entity_Does_Not_Exist()
    {
        // Arrange
        var id = Guid.NewGuid();

        _contactInfoRepositoryMock
            .Setup(x => x.GetByIdAsync(id))
            .ReturnsAsync((ContactInfo?)null);

        // Act
        Func<Task> act = async () => await _contactInfoManager.SoftDeleteAsync(id);

        // Assert
        await act.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage($"Contact info not found. Id: {id}");

        _contactInfoRepositoryMock.Verify(x => x.GetByIdAsync(id), Times.Once);
        _contactInfoRepositoryMock.Verify(x => x.SoftDeleteAsync(It.IsAny<Guid>()), Times.Never);
    }

    [Fact]
    public async Task SoftDeleteByContactIdAsync_Should_Call_Repository_Method()
    {
        // Arrange
        var contactId = Guid.NewGuid();

        _contactInfoRepositoryMock
            .Setup(x => x.SoftDeleteByContactIdAsync(contactId))
            .Returns(Task.CompletedTask);

        // Act
        await _contactInfoManager.SoftDeleteByContactIdAsync(contactId);

        // Assert
        _contactInfoRepositoryMock.Verify(x => x.SoftDeleteByContactIdAsync(contactId), Times.Once);
    }
}