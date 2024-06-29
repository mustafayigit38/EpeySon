namespace WebApplication1.ViewModels
{
    public class CreateProductViewModel
    {
        public string ProductName { get; set; }
        public int ProductCategory { get; set; }
        public IFormFile ProductImage { get; set; }
        public List<AttributeValueViewModel> Values { get; set; }
    }

    public class AttributeValueViewModel
    {
        public int AttributeID { get; set; }
        public string AttributeName { get; set; }
        public string AttributeType { get; set; }
        public string ValueString { get; set; }
        public int? ValueInteger { get; set; }
        public double? ValueFloat { get; set; }
        public DateOnly? ValueDate { get; set; }
        public int? ValueBoolean { get; set; }

    }
}
