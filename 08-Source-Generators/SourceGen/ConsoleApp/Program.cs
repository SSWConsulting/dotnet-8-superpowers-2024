using Microsoft.Extensions.DependencyInjection;

using static System.Formats.Asn1.AsnWriter;

var services = new ServiceCollection();

// Call the generated method
services.AddWidgetServices(); 

var serviceProvider = services.BuildServiceProvider();

// the most recently registered
var widgetService = serviceProvider.GetRequiredService<IWidgetService>();
widgetService.DoWork();

// all of them
serviceProvider.GetServices<IWidgetService>().ToList().ForEach(s => s.DoWork());

