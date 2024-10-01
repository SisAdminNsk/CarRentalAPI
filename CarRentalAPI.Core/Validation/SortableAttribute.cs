namespace CarRentalAPI.Core.Validation
{
    [AttributeUsage(AttributeTargets.Property)]
    public class SortableAttribute : Attribute
    {
        public SortableAttribute() { }
    }
}
