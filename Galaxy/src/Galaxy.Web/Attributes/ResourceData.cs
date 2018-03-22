using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Galaxy.Web.Attributes
{
    public class ResourceData
    {
        public static Dictionary<string, List<string>> Resources { get; set; }
        static ResourceData()
        {
            Resources = new Dictionary<string, List<string>>();
        }

        public static void AddResource(string name)
        {
            AddResource(name, "");
        }

        public static void AddResource(string name, string action)
        {
            if (string.IsNullOrEmpty(name))
            {
                return;
            }
            if (!Resources.ContainsKey(name))
            {
                Resources.Add(name, new List<string>());
            }
            if (!string.IsNullOrEmpty(action) && !Resources[name].Contains(action))
            {
                Resources[name].Add(action);
            }
        }
    }
}
