using System;
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
        
        public ApplicationUser Owner { get; set; }
        
        public Status Status { get; set; }
        
        public int StatusId { get; set; }
        
        public bool Registered { get; set; }
        
        public string RegisterNumber { get; set; }
        
    }
}