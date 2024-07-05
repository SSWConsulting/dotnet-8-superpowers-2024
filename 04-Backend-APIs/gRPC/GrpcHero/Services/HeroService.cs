using Grpc.Core;
using GrpcHero;

namespace GrpcHero.Services
{
    /// <summary>
    /// Hero Greeting Service
    /// </summary>
    /// <param name="logger">Injected Logger</param>
    public class HeroService(ILogger<HeroService> logger) : Hero.HeroBase
    {
        /// <summary>
        /// Endpoint that responds with a Hero's Greeting
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
        {
            return Task.FromResult(new HelloReply
            {
                Message = $"Hello {request.Name}! I am your GRPC Hero for today!"
            });
        }

        /// <summary>
        /// Endpoint that responds with a stream of Hero's Greetings
        /// </summary>
        /// <param name="request"></param>
        /// <param name="responseStream"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        /// <exception cref="RpcException"></exception>
        public override async Task SayHelloStream(
            HelloStreamRequest request, 
            IServerStreamWriter<HelloReply> responseStream, 
            ServerCallContext context)
        {
            if (request.Count <= 0)
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Count must be greater than zero."));
            }

            logger.LogInformation($"Sending {request.Count} hellos to {request.Name}");

            for (var i = 0; i < request.Count; i++)
            {
                await responseStream.WriteAsync(new HelloReply { Message = $"Hello {request.Name} {i + 1}" });
                await Task.Delay(TimeSpan.FromSeconds(1));
            }
        }
    }
}
