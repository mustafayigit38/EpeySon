using WebApplication1.Models;

namespace WebApplication1.ViewModels
{
    public class ComparisonResult
    {
        public string AttributeName { get; set; }
        public string AttributeSpecName { get; set; }
        public List<ProductAttributeValue> ProductValues { get; set; }
    }
}


