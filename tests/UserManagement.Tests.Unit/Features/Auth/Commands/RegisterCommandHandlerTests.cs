using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Moq;
using UserManagement.Application.Common.Interfaces;
using UserManagement.Application.Features.Auth.Commands.Register;
using UserManagement.Domain.Entities;

public class RegisterCommandHandlerTests
{
    private readonly Mock<UserManager<User>> _userManagerMock;
    private readonly Mock<ITenantContext> _tenantContextMock;
    private readonly Mock<IJwtTokenGenerator> _jwtTokenGeneratorMock;
    private readonly RegisterCommandHandler _handler;

    public RegisterCommandHandlerTests()
    {
        _userManagerMock = new Mock<UserManager<User>>(
            Mock.Of<IUserStore<User>>(),
            null, null, null, null, null, null, null, null);
        _tenantContextMock = new Mock<ITenantContext>();
        _jwtTokenGeneratorMock = new Mock<IJwtTokenGenerator>();

        _handler = new RegisterCommandHandler(
            _userManagerMock.Object,
            _tenantContextMock.Object,
            _jwtTokenGeneratorMock.Object);
    }

    [Fact]
    public async Task Handle_ValidCommand_ReturnsSuccessResult()
    {
        // Arrange
        var command = new RegisterCommand
        {
            Email = "test@example.com",
            Password = "Password123!",
            UserName = "testuser",
            TenantCode = "tenant1"
        };

        _tenantContextMock.Setup(x => x.TenantId).Returns(Guid.NewGuid());
        _userManagerMock.Setup(x => x.CreateAsync(It.IsAny<User>(), command.Password))
            .ReturnsAsync(IdentityResult.Success);
        _jwtTokenGeneratorMock.Setup(x => x.GenerateToken(It.IsAny<User>()))
            .Returns("test-token");

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Success.Should().BeTrue();
        result.Token.Should().Be("test-token");
        result.Errors.Should().BeNull();
    }

    [Fact]
    public async Task Handle_InvalidCommand_ReturnsFailureResult()
    {
        // Arrange
        var command = new RegisterCommand
        {
            Email = "test@example.com",
            Password = "weak",
            UserName = "testuser",
            TenantCode = "tenant1"
        };

        _tenantContextMock.Setup(x => x.TenantId).Returns(Guid.NewGuid());
        _userManagerMock.Setup(x => x.CreateAsync(It.IsAny<User>(), command.Password))
            .ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = "Password too weak" }));

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Success.Should().BeFalse();
        result.Token.Should().BeNull();
        result.Errors.Should().Contain("Password too weak");
    }
}