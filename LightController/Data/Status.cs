using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LightController.Data
{
    public class Status
    {
        public int Id { get; set; }
        
        [DataType("varchar(100)")]
        public string Name { get; set; }
        
        [DataType("varchar(100)")]
        public string Description { get; set; }
    }
}