
using System.ComponentModel.DataAnnotations;

namespace Commander.Models
{
    public class Test
    {
         [Key]
        public int Id { get; set; }
        public string  Name { get; set; }
        public int value { get; set; }
        public User User{get;set;}
    }
}