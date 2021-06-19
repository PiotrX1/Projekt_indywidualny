using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace LightController.Data
{
    public class ApplicationUser : IdentityUser<int>
    {
        public List<Device> Devices { get; set; }
        public string ActivationToken { get; set; }
        
    }
}