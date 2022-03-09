// See https://aka.ms/new-console-template for more information
using EasyNetQ;
using Messages;
using QueueTester.Core;

Console.Title = "Publisher";

Console.WriteLine("Hello, this is Publiser. Please input vhost:1 or 2");
Console.WriteLine("In this demo, we requires two vhosts, and using RPC calls running on multiple consoles, without affecting each other.");
var vhost = Console.ReadLine();
var connectionString = AppConsts.RabbitConnection;

if (vhost == "2")
{
    Console.Title = "Publisher[2]";
    connectionString = AppConsts.RabbitConnection2;
}

Console.WriteLine("Now we're using " + connectionString + Environment.NewLine);

using (var bus = RabbitHutch.CreateBus(connectionString))
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
                    Text = $"at-{DateTime.Now:mm:ss} [RPC Request({vhost})]"
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

                Console.WriteLine($"[{DateTime.Now:mm:ss}] Got RPC response: {resposne.Response}({vhost})");
            }
            catch (TaskCanceledException tce)
            {
                Console.WriteLine($"[{DateTime.Now:mm:ss}] Task Cancelled. Timeout. {tce.Message}({vhost})");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[{DateTime.Now:mm:ss}] Exception occurred: {ex.Message}({vhost})");
            }
        }
        else
        {
            bus.PubSub.Publish(new TextMessage
            {
                Text = input ?? "empty string"
            });
            Console.WriteLine($"[{DateTime.Now:mm:ss}] Publish to queue: {input}({vhost})");
        }

        Console.WriteLine("Enter a message. Or 'rpc' to make RPC call, 'q' to quit.");
    }
}