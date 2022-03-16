
using System.ComponentModel.DataAnnotations;

namespace Commander.Models
{
    public class User
    {
         [Key]
        public int Id { get; set; }
        public string  Name { get; set; }
        public int value { get; set; }
    }
}