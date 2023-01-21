using NBomber.Contracts;
using NBomber.CSharp;
using NBomber.Plugins.Http.CSharp;

var httpFactory = HttpClientFactory.Create();

var step = Step.Create("step", httpFactory, async context =>
{
    var response = await context.Client.GetAsync("http://localhost:5000/WeatherForecast", context.CancellationToken);

    return response.IsSuccessStatusCode
        ? Response.Ok(statusCode: (int)response.StatusCode)
        : Response.Fail(statusCode: (int)response.StatusCode);
});

var scenario = ScenarioBuilder
    .CreateScenario("httpScenario", step)
    .WithWarmUpDuration(TimeSpan.FromSeconds(3))
    .WithLoadSimulations(Simulation.InjectPerSec
    (rate: 100, during: TimeSpan.FromSeconds(10)));

NBomberRunner
    .RegisterScenarios(scenario)
    .Run();
