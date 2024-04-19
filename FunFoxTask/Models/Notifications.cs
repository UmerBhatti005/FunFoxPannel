using Newtonsoft.Json;

namespace FunFoxTask.Models
{
    public class Notification
    {
        public Notification(string type, string description)
        {
            Type = type;
            Description = description;
        }

        public string Type { get; set; }

        public string Description { get; set; }

        public static string SerializeNotifications(List<Notification> notifications)
        {
            return JsonConvert.SerializeObject(notifications);
        }
    }
}
