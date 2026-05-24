using LiteBus.Commands.Abstractions;
using LiteBus.Queries.Abstractions;
using LitePress.Application.Read;
using LitePress.Application.Reactions;
using LitePress.Application.Write;
using NetArchTest.Rules;

namespace LitePress.Architecture.Tests;

public sealed class ApplicationLayerTests
{
    [Fact]
    public void QueryHandlers_ShouldNotDependOn_InfrastructureProject()
    {
        var result = Types
            .InAssembly(typeof(ApplicationReadAssemblyMarker).Assembly)
            .ShouldNot()
            .HaveDependencyOn("Infrastructure")
            .GetResult();

        result.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void CommandAndQueryHandlers_ShouldBeInternalSealed()
    {
        var result = Types
            .InAssemblies([
                typeof(ApplicationWriteAssemblyMarker).Assembly,
                typeof(ApplicationReadAssemblyMarker).Assembly
            ])
            .That()
            .ImplementInterface(typeof(ICommandHandler<,>))
            .Or()
            .ImplementInterface(typeof(IQueryHandler<,>))
            .Should()
            .BeSealed()
            .And()
            .NotBePublic()
            .GetResult();

        result.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void ReactionsProject_ShouldNotDependOn_EntityFrameworkCore()
    {
        var result = Types
            .InAssembly(typeof(ApplicationReactionsAssemblyMarker).Assembly)
            .ShouldNot()
            .HaveDependencyOn("Microsoft.EntityFrameworkCore")
            .GetResult();

        result.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void WebApi_ShouldNotContain_ControllerBase()
    {
        var result = Types
            .InAssembly(typeof(Program).Assembly)
            .ShouldNot()
            .Inherit(typeof(Microsoft.AspNetCore.Mvc.ControllerBase))
            .GetResult();

        result.IsSuccessful.Should().BeTrue();
    }
}
