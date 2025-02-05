using System.ComponentModel.DataAnnotations;

namespace CrowdCover.Web.Models
{
    public class DynamicDataVariable
    {
        public DynamicDataVariable()
        {
                
        }

        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public string Value { get; set; }
    }
}
