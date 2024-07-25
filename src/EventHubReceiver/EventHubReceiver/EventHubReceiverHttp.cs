using System.Collections.Generic;
using System.Net;
using EventHubReceiver.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace EventHubReceiver;

public class EventHubReceiverHttp
{
    private readonly IEventHubConsumerService _eventHubConsumerService;
    private readonly ILogger _logger;

    public EventHubReceiverHttp(IEventHubConsumerService eventHubConsumerService, ILoggerFactory loggerFactory)
    {
        _eventHubConsumerService = eventHubConsumerService;
        _logger = loggerFactory.CreateLogger<EventHubReceiverHttp>();
    }

    [Function("EventHubReceiverHttp")]
    public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequestData req,
        FunctionContext executionContext)
    {
        _logger.LogInformation("C# HTTP trigger function processed a request.");

        using CancellationTokenSource cancellationSource = new();
        var events = await _eventHubConsumerService.ReceiveEvents(cancellationSource);

        return req.CreateResponse(HttpStatusCode.OK);
    }
}