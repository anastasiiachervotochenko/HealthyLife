using System.Collections.Generic;

namespace HealthyLife.ResponseModel
{
    public class CustomItem<T>
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public List<T> Items { get; set; }
        
    }
}