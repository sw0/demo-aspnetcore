using EasyNetQ;

namespace Messages
{
    [Queue("requestqueue", ExchangeName = "my-easynetq")]
    public class TextMessage
    {
        public string Text { get; set; }
    }

    public class ResponseMessage
    {
        public string Response { get; set; }
    }
}
