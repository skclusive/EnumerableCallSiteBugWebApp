
using Microsoft.Extensions.DependencyInjection.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.TryAddEnumerable(ServiceDescriptor.Scoped(typeof(IProcessor<int>), typeof(IntProcessor)));
// Fix: ordering open generic above concretes
builder.Services.TryAddEnumerable(ServiceDescriptor.Scoped(typeof(IProcessor<>), typeof(AnyProcessor<>)));

var app = builder.Build();

// enable this to reproduce the bug in console app
// var processor = scope.ServiceProvider.GetService<IProcessor<int>>();

// bug?: returns "IntProcessor -- IntProcessor" instead of IntProcessor -- AnyProcessor (as in console app)
app.MapGet("/", (IEnumerable<IProcessor<int>> processors) => string.Join(" -- ", processors.Select(p => p.GetType().Name)));

app.Run();

interface IProcessor<T> { }

record AnyProcessor<T> : IProcessor<T>;

record IntProcessor : IProcessor<int>;
