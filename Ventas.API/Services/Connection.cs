using Sap.Data.Hana;

namespace InspeccionProduccion.API.Services
{
    public class Connection
    {
        private readonly IConfiguration _configuration;

        public Connection(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public HanaConnection GetConnection()
        {
            try
            {
                HanaConnection connection = new HanaConnection(_configuration.GetConnectionString("HanaConnection"));
                return connection;
            }
            catch (HanaException ex)
            {
                ex.Message.ToString();
                throw;
            }
        }
    }
}
