using CSESoftware.Core.Entity;

namespace CSESoftware.RepositoryTestProject.Setup
{
    public class Topping : BaseEntity<int>
    {
        public string Name { get; set; }
        public double AdditionalCost { get; set; }
    }
}