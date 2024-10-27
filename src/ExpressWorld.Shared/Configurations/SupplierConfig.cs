namespace ExpressWorld.Shared.Configurations
{
    public class SupplierConfig
    {
        public string Name { get; set; }
        public string DataSourceType { get; set; }
        public string SourcePath { get; set; }
        public string DtoType { get; set; }
        public string RootPropertyName { get; set; }
        public Type DtoTypeResolved => Type.GetType(DtoType);
    }
}
