#related to [issue](https://github.com/dotnet/efcore/issues/19470)


I have following interfaces (located in nuget package, but in reproduction it is alongside other projects in single solution)

```csharp
public interface IUnitOfWork
{
    Task<int> SaveAsync(CancellationToken cancellationToken = default (CancellationToken));
}

public interface IRepository<TAggregate, TKey> where TAggregate : class, IAggregateRoot<TKey>
{
    IUnitOfWork UnitOfWork { get; }
}

public interface IEfRepository<TAggregate, TKey> : IRepository<TAggregate, TKey>
        where TAggregate : class, IAggregateRoot<TKey>
{
    Task SomeMethod(CancellationToken token = default);
}

public interface IAggregateRoot<TKey>
{
    TKey Id { get; set; }
}
```

csproj of a nuget package looks like that:

```csharp
<Project Sdk="Microsoft.NET.Sdk">

    <PropertyGroup>
        <TargetFramework>netstandard2.1</TargetFramework>
        <GeneratePackageOnBuild>true</GeneratePackageOnBuild>
        <Version>2.0.0</Version>
        <AssemblyVersion>2.0.0</AssemblyVersion>
        <DebugType>Full</DebugType>

        <Nullable>enable</Nullable>
        <LangVersion>8</LangVersion>
        <TreatWarningsAsErrors>true</TreatWarningsAsErrors>
        <DebugType>Full</DebugType>
    </PropertyGroup>

    <ItemGroup>
        <PackageReference Include="Microsoft.EntityFrameworkCore" Version="3.1.0" />
    </ItemGroup>
</Project>

```


and in my Infrastructure project (which is referenced by Service project) I have following interface and implementation:

```csharp
public interface IDocumentRepository : IEfRepository<Document, int>
{
        
}

public class DocumentRepository : Repository<Document, int>, IDocumentRepository
{
    public DocumentRepository(
        WriteDocumentsContext writeDocumentsContext
    )
        : base(writeDocumentsContext)
    {

    }

    public IUnitOfWork UnitOfWork { get; }
    public Task SomeMethod(CancellationToken token = default)
    {
        return Task.CompletedTask;
    }
}

public class Document : IAggregateRoot<int>
{
    public int Id { get; set; }
}

public class Repository<T, T1>
{
    protected Repository(WriteDocumentsContext writeDocumentsContext)
    {
        
    }
}

public class WriteDocumentsContext : DbContext
{
}
```

When I try to invoke `UnitOfWork` like that:

```csharp
public class D 
{
    private readonly IDocumentRepository _documentsRepository;
    public D(IDocumentRepository docRepo, IDocumentRepository documentsRepository)
    {
        _documentsRepository = documentsRepository;
    }

    public async Task SomeMethod(CancellationToken token = default)
    {
        await _documentsRepository.UnitOfWork.SaveAsync(token);
    }
}
```

and csproj of my Infrastructure project looks like that:

```csharp
<Project Sdk="Microsoft.NET.Sdk">

    <PropertyGroup>
        <TargetFramework>netcoreapp3.1</TargetFramework>
        <DebugType>Full</DebugType>
        <IsPackable>false</IsPackable>
    </PropertyGroup>

    <ItemGroup>
      <PackageReference Include="MyNugetPackage" Version="2.0.0" />
    </ItemGroup> 

</Project>
```

and csproj of my Service project looks like that:

```csharp
<Project Sdk="Microsoft.NET.Sdk">

    <PropertyGroup>
        <TargetFramework>netcoreapp3.1</TargetFramework>
        <DebugType>Full</DebugType>
        <IsPackable>false</IsPackable>
    </PropertyGroup>

    <ItemGroup>
      <ProjectReference Include="..\Infrastructure\Infrastructure.csproj" />
    </ItemGroup> 

</Project>
```

I get following error:

```
DeleteDocumentCommandHandler.cs(58, 48): [CS0229] Ambiguity between 'IRepository<Document, int>.UnitOfWork' and 'IRepository<Document, int>.UnitOfWork'
```

But when:
    
 * I move `DocumentRepository` and `IDocumentRepository` files to the service project, everything works just fine.
 * When I inject `IEfRepository<Document, int>` instead of `IDocumentRepository` also everything works just fine.
 * When I add generic constraints on `IDocumentRepository` it still doesn't work.
 * I add to the Infrastructure project csproj following lines 

    ```csharp
        <Nullable>enable</Nullable>
        <LangVersion>8</LangVersion>
    ```

    It works...

I'm using .net core 3.1 and try to compile project by `dotnet build`, in Rider IDE and Visual Studio 2019.


Side note:
When I remove the `nullable = enable` from a nuget package, also everything works just fine.

EF Core version: 3.1
Database provider: Microsoft.EntityFrameworkCore.SqlServer
Target framework: NET Core 3.1
Operating system: Windows 10 Pro/Home
IDE: dotnet console, Jetbrains Rider, Visual Studio 2019

