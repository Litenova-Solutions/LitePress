using LitePress.Domain.Authors;
using LitePress.Domain.Posts;
using LitePress.Domain.Shared;
using LitePress.Domain.Tags;
using NetArchTest.Rules;

namespace LitePress.Architecture.Tests;

public sealed class DomainLayerTests
{
    [Fact]
    public void Domain_ShouldNotReference_EntityFrameworkCore()
    {
        var result = Types
            .InAssembly(typeof(Post).Assembly)
            .ShouldNot()
            .HaveDependencyOn("Microsoft.EntityFrameworkCore")
            .GetResult();

        result.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void Domain_ShouldNotReference_AspNetCore()
    {
        var result = Types
            .InAssembly(typeof(Post).Assembly)
            .ShouldNot()
            .HaveDependencyOn("Microsoft.AspNetCore")
            .GetResult();

        result.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void Domain_ShouldNotReference_LiteBus()
    {
        var result = Types
            .InAssembly(typeof(Post).Assembly)
            .ShouldNot()
            .HaveDependencyOn("LiteBus")
            .GetResult();

        result.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void AggregateRoots_ShouldInheritAggregateRootBase()
    {
        foreach (var aggregateRootType in new[] { typeof(Post), typeof(Tag), typeof(Author) })
        {
            aggregateRootType.BaseType!.GetGenericTypeDefinition().Should().Be(typeof(AggregateRoot<>));
            aggregateRootType.IsSealed.Should().BeTrue();
        }
    }

    [Fact]
    public void DomainEvents_ShouldBeSealedTypesImplementingIDomainEvent()
    {
        var result = Types
            .InAssembly(typeof(Post).Assembly)
            .That()
            .ImplementInterface(typeof(IDomainEvent))
            .Should()
            .BeSealed()
            .GetResult();

        result.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void Domain_ShouldNotContainCommonNamespace()
    {
        var commonTypes = Types
            .InAssembly(typeof(Post).Assembly)
            .That()
            .ResideInNamespace("LitePress.Domain.Common")
            .GetTypes();

        commonTypes.Should().BeEmpty();
    }
}
