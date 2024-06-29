
using WebApplication1.Models;

namespace WebApplication1.ViewModels
{
    public class CreateAttribute_VM
    {
        public Category categories { get; set; }
        public Models.Attribute attribute { get; set; }
        public AttributeSpec attributeSpec { get; set; }
    }
}
