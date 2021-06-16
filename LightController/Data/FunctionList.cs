using System.ComponentModel.DataAnnotations.Schema;

namespace LightController.Data
{

    public class FunctionList
    {
        public int Id { get; set; }
        public Function Function { get; set; }
        public Device Device { get; set; }
    }
}
