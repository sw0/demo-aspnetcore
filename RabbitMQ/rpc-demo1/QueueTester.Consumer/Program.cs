// See https://aka.ms/new-console-template for more information
using EasyNetQ;
using Messages;
using QueueTester.Core;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

Console.Title = "Consumer";
Console.WriteLine("Hello, this is Consumer.");

var factory = new ConnectionFactory();
factory.Uri = new Uri(AppConsts.RabbitConnection);

var connection = factory.CreateConnection();
var channel = connection.CreateModel();

var queueName = "Messages.TextMessage, QueueTester.Core";
queueName = "iot.queue.request.partner1";
var exchangeName = "easy_net_q_rpc";
channel.QueueDeclare(queueName, true, false, false);
channel.QueueBind(queueName, exchangeName, "");

var consumer = new EventingBasicConsumer(channel);
consumer.Received += (sender, args) =>
{
    var body = Encoding.UTF8.GetString(args.Body.ToArray());

    Console.WriteLine(@$"Received:
    MessageId: {args.BasicProperties.MessageId} 
    CorrelationId: {args.BasicProperties.CorrelationId} 
    Expiration: {args.BasicProperties.Expiration} 
    ReplyToAddress: {args.BasicProperties.ReplyToAddress} 
    ClusterId: {args.BasicProperties.ClusterId} 
    DeliveryMode: {args.BasicProperties.DeliveryMode} 
    ReplyTo: {args.BasicProperties.ReplyTo} 
    Timestamp: {args.BasicProperties.Timestamp} 
    UserId: {args.BasicProperties.UserId} 
    ProtocolClassId: {args.BasicProperties.ProtocolClassId} 
    ProtocolClassName: {args.BasicProperties.ProtocolClassName} 
    AppId: {args.BasicProperties.AppId} 
    RoutingKey: {args.RoutingKey}

    Body: {body}");

    if (args.BasicProperties.Headers != null)
        foreach (var item in args.BasicProperties.Headers)
        {
            Console.WriteLine($"\tHeader, {item.Key}:{item.Value}");
        }

    var response = new ResponseMessage() { Response = "Resp: " + body };
    var serializer = new JsonSerializer();
    var bytes = serializer.MessageToBytes(typeof(ResponseMessage), response);
    var properties = channel.CreateBasicProperties();
    properties.CorrelationId = args.BasicProperties.CorrelationId;
    properties.Type = "Messages.ResponseMessage, QueueTester.Core";

    var routingKey = args.BasicProperties.ReplyTo;
    channel.BasicPublish(exchangeName, routingKey, true, properties, bytes);

    Console.WriteLine(@$"Response Sent. CorrelationId: {args.BasicProperties.CorrelationId}, RoutingKey: {routingKey}");

};
channel.BasicConsume(queueName, true, consumer);

Console.WriteLine($"Press any key to exit...");
Console.ReadKey();
channel.Close();
connection.Close();


//using (var bus = RabbitHutch.CreateBus(AppConsts.RabbitConnection))
//{
//    var enableRespond = false;

//    if (enableRespond)
//    {
//        bus.Rpc.Respond<TextMessage, ResponseMessage>(x =>
//        {
//            var response = new ResponseMessage();
//            if (x.Text.StartsWith("a"))
//            {
//                response.Response = "Received:" + x.Text;
//            }
//            else
//            {
//                response.Response = $"Others for {x.Text}";
//            }

//            Console.WriteLine($"Respond: {response.Response}");
//            return Task.FromResult(response);
//        }//, cfg => { cfg.WithQueueName("test3"); }
//        );

//        bus.Rpc.Respond<TextMessage, ResponseMessage>(x =>
//        {
//            var response = new ResponseMessage();
//            if (x.Text.StartsWith("a"))
//            {
//                response.Response = "Received 2 :" + x.Text;
//            }
//            else
//            {
//                response.Response = $"Others 2 for {x.Text}";
//            }

//            Console.WriteLine($"Respond: {response.Response}");
//            return Task.FromResult(response);
//        }//, cfg => { cfg.WithQueueName("test3"); }
//        );
//    }

//    var input = "";
//    Console.WriteLine("Enter a message. 'q' to quit.");
//    while ((input = Console.ReadLine()) != "q")
//    {
//        if (input!.StartsWith("res"))
//        {
//            bus.PubSub.Publish(new ResponseMessage() { Response = $"RESP: {DateTime.Now:MM:ss}" }, options =>
//            {
//                if(input == "res2")
//                {
//                    options.WithTopic("on-or-off");
//                }
//            });
//            Console.WriteLine($"[{DateTime.Now:MM:ss}] TRY Publish Response");
//        }

//        bus.PubSub.Subscribe("", act => { });

//        //var exchange = bus.Advanced.ExchangeDeclare("easy_net_q_rpc", options => { options.AsDurable(true).AsAutoDelete(true); });
//        var exchange = bus.Advanced.ExchangeDeclare("easy_net_q_rpc", ExchangeType.Direct);


//        bus.Advanced.Publish(exchange, "", true, new ResponseMessage() { Response = "" }, )
//    }
//}
