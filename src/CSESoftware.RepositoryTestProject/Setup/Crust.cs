using CSESoftware.Core.Entity;

namespace CSESoftware.RepositoryTestProject.Setup
{
    public class Crust : BaseEntity<int>
    {
        public string Name { get; set; }
        public double AdditionalCharge { get; set; }
    }
}