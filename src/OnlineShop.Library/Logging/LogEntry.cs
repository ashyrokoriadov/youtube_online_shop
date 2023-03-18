using Newtonsoft.Json;

namespace OnlineShop.Library.Logging
{
    public class LogEntry
    {
        public string Class { get; set; }
        public string Method { get; set; }
        public string Comment { get; set; }  
        public string Operation { get; set; }
        public string Parameters { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
