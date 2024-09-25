namespace InspeccionProduccion.API.Domain
{
    public class ServiceLayerSettings
    {
        public const string SectionName = "ServiceLayer";
        public string PathUri { get; init; } = null!;
        public int MinuteCacheServer { get; init; }
        public string UserName { get; init; } = null!;
        public string Password { get; init; } = null!;
        public string CompanyDB { get;init; } = null!;
        public string Login { get;init; } = null!;
        public string Url { get;init; } = null!;
        
    }
}
