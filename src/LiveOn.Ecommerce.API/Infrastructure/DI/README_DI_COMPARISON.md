# Dependency Injection Comparison

This project includes **four** DI implementations for learning purposes.
In production, we use **Autofac** (see `Global.asax.cs`).

---

## ?? Implementations

### 1. SimpleDependencyResolver (Custom)
**File:** `Infrastructure/SimpleDependencyResolver.cs`

**Pros:**
- ? No external dependencies
- ? Full control over registration
- ? Easy to understand
- ? Lightweight

**Cons:**
- ? Manual registration (tedious)
- ? No auto-registration
- ? Limited lifetime management
- ? More code to maintain

**Use when:**
- Learning DI concepts
- Very small projects (< 20 services)
- Don't want NuGet dependencies

---

### 2. Unity (Microsoft)
**File:** `Infrastructure/DI/UnityConfig.cs`

**Pros:**
- ? Microsoft official container
- ? Good documentation
- ? `PerRequestLifetimeManager` built-in
- ? Industry standard in .NET Framework

**Cons:**
- ? Verbose registration
- ? Slower than competitors
- ? Less active development

**Registration Example:**
```csharp
container.RegisterType<IUnitOfWork, UnitOfWork>(new PerRequestLifetimeManager());
```

---

### 3. Autofac ? (CURRENT CHOICE)
**File:** `Infrastructure/DI/AutofacConfig.cs`

**Pros:**
- ? **Most popular** in .NET Framework
- ? **Auto-registration** with assembly scanning
- ? Excellent lifetime management
- ? Powerful features (modules, decorators)
- ? Great documentation

**Cons:**
- ? Slightly complex API
- ? Learning curve

**Why we chose it:**
- Industry standard
- Auto-registration saves time
- Scales well to large projects
- Best balance of features vs complexity

**Registration Example:**
```csharp
// Auto-register all ICommandHandler implementations!
builder.RegisterAssemblyTypes(typeof(ICommandHandler<,>).Assembly)
    .AsClosedTypesOf(typeof(ICommandHandler<,>))
    .InstancePerRequest();
```

---

### 4. Simple Injector
**File:** `Infrastructure/DI/SimpleInjectorConfig.cs`

**Pros:**
- ? **Fastest** performance
- ? **Best error messages**
- ? **Built-in verification** (`container.Verify()`)
- ? Clean, simple API

**Cons:**
- ? Less popular than Autofac
- ? Smaller ecosystem

**Unique feature:**
```csharp
container.Verify();  // Catches configuration errors at startup!
```

---

## ?? Performance Comparison

| Container | Registration Time | Resolution Speed | Memory |
|-----------|------------------|------------------|---------|
| **SimpleDI** | ????? Fast | ????? Fast | ????? Low |
| **Unity** | ??? Medium | ??? Medium | ??? Medium |
| **Autofac** | ???? Fast | ???? Fast | ???? Low |
| **Simple Injector** | ????? Fast | ????? Fastest | ????? Lowest |

---

## ?? How to Switch

### Currently Active: Autofac

To switch, edit `Global.asax.cs`:

```csharp
protected void Application_Start()
{
    GlobalConfiguration.Configure(WebApiConfig.Register);
    
    // Choose ONE:
    // GlobalConfiguration.Configure(UnityConfig.Register);
    GlobalConfiguration.Configure(AutofacConfig.Register);  // ? Current
    // GlobalConfiguration.Configure(SimpleInjectorConfig.Register);
    // GlobalConfiguration.Configuration.DependencyResolver = new SimpleDependencyResolver();
}
```

---

## ?? Key Concepts

### Lifetime Management

**Transient:**
- New instance every time
- Use for stateless services

**Scoped / PerRequest:**
- One instance per HTTP request
- Use for DbContext, UnitOfWork
- **This is what we use!**

**Singleton:**
- One instance for entire application lifetime
- Use for caches, configuration

---

## ?? Learning Takeaways

1. **SimpleDependencyResolver** teaches you how DI works under the hood
2. **Unity** shows Microsoft's approach (verbose but clear)
3. **Autofac** demonstrates production-ready DI with auto-registration
4. **Simple Injector** shows performance-focused, clean API design

Each has its place - we chose **Autofac** for its balance of power and usability.

---

## ?? Next Steps

1. All four implementations are ready to use
2. Currently using: **Autofac** (configured in `Global.asax.cs`)
3. You can switch between them by commenting/uncommenting in `Application_Start()`
4. For new projects: Consider **Autofac** (enterprise) or **Simple Injector** (performance)
