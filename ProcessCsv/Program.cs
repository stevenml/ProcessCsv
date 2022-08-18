using Autofac;
using Autofac.Extensions.DependencyInjection;
using Autofac.Features.AttributeFilters;
using Microsoft.Extensions.Hosting;
using Cocona;
using Microsoft.Extensions.DependencyInjection;
using ProcessCsv.Commands;

var appBuilder = CoconaApp.CreateBuilder();
appBuilder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

appBuilder.Host.ConfigureContainer<ContainerBuilder>(cb =>
{
    // Register Commands
    cb.RegisterType<PrintAbnormalValuesCommand>()
        .WithAttributeFiltering();
});

var app = appBuilder.Build();

app.AddCommands(typeof(PrintAbnormalValuesCommand));

using (app.Services.CreateScope())
{
    app.Run();
}
