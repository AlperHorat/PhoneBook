using System.Timers;
using FluentAssertions;
using Moq;
using PhoneBook.Domain.Entities;
using PhoneBook.Infrastructure.Repositories;
using PhoneBook.Services.Contacts;

namespace PhoneBook.Tests.Services.Contacts;

public class ContactManagerTests
{
    private readonly Mock<IContactRepository> _contactRepositoryMock;
    private readonly ContactManager _contactManager;

    public ContactManagerTests()
    {
        _contactRepositoryMock = new Mock<IContactRepository>();
        _contactManager = new ContactManager(_contactRepositoryMock.Object);
    }

    [Fact]
    public async Task GetAllAsync_Should_Return_Contacts()
    {
        // Arrange
        var contacts = new List<Contact>
        {
            new Contact("Ahmet", "Yılmaz", "Aselsan"),
            new Contact("Ayşe", "Demir", "Havelsan"),
            new Contact("Mehmet", "Doğan", "Roketsan")
        };

        _contactRepositoryMock
            .Setup(x => x.GetAllAsync())
            .ReturnsAsync(contacts);

        // Act
        var result = await _contactManager.GetAllAsync();

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(contacts.Count);
        result.Should().BeEquivalentTo(contacts);
        _contactRepositoryMock.Verify(x => x.GetAllAsync(), Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_Should_Return_Contact_When_Contact_Exists()
    {
        // Arrange
        var contactId = Guid.NewGuid();
        var contact = new Contact("Ahmet", "Yılmaz", "Aselsan");

        _contactRepositoryMock
            .Setup(x => x.GetByIdAsync(contactId))
            .ReturnsAsync(contact);

        // Act
        var result = await _contactManager.GetByIdAsync(contactId);

        // Assert
        result.Should().NotBeNull();
        result.Should().Be(contact);
        _contactRepositoryMock.Verify(x => x.GetByIdAsync(contactId), Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_Should_Return_Null_When_Contact_Does_Not_Exist()
    {
        // Arrange
        var contactId = Guid.NewGuid();

        _contactRepositoryMock
            .Setup(x => x.GetByIdAsync(contactId))
            .ReturnsAsync((Contact?)null);

        // Act
        var result = await _contactManager.GetByIdAsync(contactId);

        // Assert
        result.Should().BeNull();
        _contactRepositoryMock.Verify(x => x.GetByIdAsync(contactId), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_Should_Add_Contact_And_Return_It_When_Valid_Contact_Is_Provided()
    {
        // Arrange
        var contact = new Contact("Ahmet", "Yılmaz", "Aselsan");

        _contactRepositoryMock
            .Setup(x => x.AddAsync(contact))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _contactManager.CreateAsync(contact);

        // Assert
        result.Should().Be(contact);
        _contactRepositoryMock.Verify(x => x.AddAsync(contact), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_Should_Throw_ArgumentNullException_When_Contact_Is_Null()
    {
        // Act
        Func<Task> act = async () => await _contactManager.CreateAsync(null!);

        // Assert
        await act.Should().ThrowAsync<ArgumentNullException>()
            .WithParameterName("contact");

        _contactRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Contact>()), Times.Never);
    }

    [Fact]
    public async Task DeleteAsync_Should_Delete_Contact_When_Contact_Exists()
    {
        // Arrange
        var contactId = Guid.NewGuid();
        var contact = new Contact("Ahmet", "Yılmaz", "Aselsan");

        _contactRepositoryMock
            .Setup(x => x.GetByIdAsync(contactId))
            .ReturnsAsync(contact);

        _contactRepositoryMock
            .Setup(x => x.DeleteAsync(contactId))
            .Returns(Task.CompletedTask);

        // Act
        await _contactManager.DeleteAsync(contactId);

        // Assert
        _contactRepositoryMock.Verify(x => x.GetByIdAsync(contactId), Times.Once);
        _contactRepositoryMock.Verify(x => x.DeleteAsync(contactId), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_Should_Throw_KeyNotFoundException_When_Contact_Does_Not_Exist()
    {
        // Arrange
        var contactId = Guid.NewGuid();

        _contactRepositoryMock
            .Setup(x => x.GetByIdAsync(contactId))
            .ReturnsAsync((Contact?)null);

        // Act
        Func<Task> act = async () => await _contactManager.DeleteAsync(contactId);

        // Assert
        await act.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage($"Contact not found. Id: {contactId}");

        _contactRepositoryMock.Verify(x => x.GetByIdAsync(contactId), Times.Once);
        _contactRepositoryMock.Verify(x => x.DeleteAsync(It.IsAny<Guid>()), Times.Never);
    }

    [Fact]
    public async Task SoftDeleteAsync_Should_SoftDelete_Contact_When_Contact_Exists()
    {
        // Arrange
        var contactId = Guid.NewGuid();
        var contact = new Contact("Ahmet", "Yılmaz", "Aselsan");

        _contactRepositoryMock
            .Setup(x => x.GetByIdAsync(contactId))
            .ReturnsAsync(contact);

        _contactRepositoryMock
            .Setup(x => x.SoftDeleteAsync(contactId))
            .Returns(Task.CompletedTask);

        // Act
        await _contactManager.SoftDeleteAsync(contactId);

        // Assert
        _contactRepositoryMock.Verify(x => x.GetByIdAsync(contactId), Times.Once);
        _contactRepositoryMock.Verify(x => x.SoftDeleteAsync(contactId), Times.Once);
    }

    [Fact]
    public async Task SoftDeleteAsync_Should_Throw_KeyNotFoundException_When_Contact_Does_Not_Exist()
    {
        // Arrange
        var contactId = Guid.NewGuid();

        _contactRepositoryMock
            .Setup(x => x.GetByIdAsync(contactId))
            .ReturnsAsync((Contact?)null);

        // Act
        Func<Task> act = async () => await _contactManager.SoftDeleteAsync(contactId);

        // Assert
        await act.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage($"Contact not found. Id: {contactId}");

        _contactRepositoryMock.Verify(x => x.GetByIdAsync(contactId), Times.Once);
        _contactRepositoryMock.Verify(x => x.SoftDeleteAsync(It.IsAny<Guid>()), Times.Never);
    }
}