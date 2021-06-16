using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.InteropServices;
using Microsoft.AspNetCore.Identity;

namespace LightController.Data
{
    public class Device
    {
        public int Id { get; set; }
        
        [DataType("varchar(100")] 
        public string Name { get; set; }
        
        public IdentityUser Owner { get; set; }
        
        public Status Status { get; set; }
        
        public bool Registered { get; set; }
        
    }
}