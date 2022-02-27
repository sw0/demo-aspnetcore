// See https://aka.ms/new-console-template for more information
using EasyNetQ;
using Messages;
using QueueTester.Core;

Console.WriteLine("Hello, this is Publiser.");
Console.Title = "Publisher";

using (var bus = RabbitHutch.CreateBus(AppConsts.RabbitConnection))
{
    //test publisher
    var input = "";
    Console.WriteLine("Enter a message. Or 'rpc' to make RPC call, 'q' to quit.");
    while ((input = Console.ReadLine()) != "q")
    {
        if (input == "rpc")
        {
            try
            {
                var request = new TextMessage()
                {
                    Text = $"at-{DateTime.Now:mm:ss} [RPC Request]"
                };
                Console.WriteLine($"[{DateTime.Now:mm:ss}] Sent RPC request: {request.Text}");

                var resposne = bus.Rpc.Request<TextMessage, ResponseMessage>(request,
                    options =>
                    {
                        options
                        //.WithQueueName("iot.queue.request") //Commented out, because it doesn't work.
                        .WithExpiration(TimeSpan.FromSeconds(50))
                        ;
                    });

                Console.WriteLine($"[{DateTime.Now:mm:ss}] Got RPC response: {resposne.Response}");
            }
            catch (TaskCanceledException tce)
            {
                Console.WriteLine($"[{DateTime.Now:mm:ss}] Task Cancelled. Timeout. {tce.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[{DateTime.Now:mm:ss}] Exception occurred: {ex.Message}");
            }
        }
        else
        {
            bus.PubSub.Publish(new TextMessage
            {
                Text = input ?? "empty string"
            });
            Console.WriteLine($"[{DateTime.Now:mm:ss}] Publish to queue: {input}");
        }

        Console.WriteLine("Enter a message. Or 'rpc' to make RPC call, 'q' to quit.");
    }
}