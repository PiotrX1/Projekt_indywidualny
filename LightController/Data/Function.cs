using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection.Metadata;

namespace LightController.Data
{
    public class Function
    {
        public int Id { get; set; }
        
        [DataType("varchar(100)")]
        public string Name { get; set; }
        
        public Byte[] Driver { get; set; }
    }
}