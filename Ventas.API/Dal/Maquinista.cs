using InspeccionProduccion.API.Domain;
using InspeccionProduccion.API.Interfaces;
using InspeccionProduccion.API.Services;
using Microsoft.Extensions.Options;
using Sap.Data.Hana;

namespace InspeccionProduccion.API.Dal
{
    public class Maquinista : IMaquinista, IDisposable
    {
        private readonly IConfiguration _configuration;
        private readonly Connection _connection;
        private readonly ServiceLayerSettings _serviceLayerSettings;
        private bool disposedValue;

        public Maquinista(IConfiguration configuration, IOptions<ServiceLayerSettings> slOptions)
        {
            _configuration = configuration;
            _connection = new(configuration);
            _serviceLayerSettings = slOptions.Value;
        }


        public Response GetLineas()
        {
            Response rs = new Response();
            List<Lineas> lLineas = new List<Lineas>();
            using (HanaConnection connection = _connection.GetConnection())
            {
                connection.Open();

                string strQuery =string.Format("SELECT T0.\"VisResCode\" as \"ID\", T0.\"ResName\" as \"Descripcion\" FROM \"{0}\".ORSC T0 WHERE \"VisResCode\" IN ("+
                    "'ENS-001','ENV-001','ENV-003','ENV-005','ENV-006','ENV-007','ENV-008','ENV-009','ENV-011','ENV-012','ENV-013','ENV-014','ENV-015','ENV-017','ENV-018','ENV-020','ENV-026','ENV-027','ENV-028',"+
                    "'ENV-029','ENV-030','ENV-002','ENV-022') ", _configuration.GetValue<string>("ServiceLayer:CompanyDB"));
                /*CANBIO SOLICITADO POR YELINA*/
                using (HanaCommand commnad = new HanaCommand(strQuery, connection))
                {
                    using (HanaDataReader reader = (HanaDataReader)commnad.ExecuteReader(System.Data.CommandBehavior.CloseConnection))
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                Lineas Lineas = new Lineas();
                                Lineas.ID = reader["ID"]?.ToString() ?? string.Empty;
                                Lineas.Descripcion = reader["Descripcion"].ToString() ?? string.Empty;
                                lLineas.Add(Lineas);
                            }
                        }
                    }
                }
            }
            rs.statusCode = System.Net.HttpStatusCode.OK;
            rs.data = lLineas;
            return rs;
        }
        public Response GetMaquinista()
        {
            Response rs = new Response();
            List<MaquinistaDomain> lMaquinistaDomain = new List<MaquinistaDomain>();
            using (HanaConnection connection = _connection.GetConnection())
            {
                connection.Open();

                string strQuery =string.Format("SELECT T0.\"empID\", IFNULL(T0.\"lastName\",'')||' '||" +
                    "IFNULL(T0.\"firstName\",'')||' '||IFNULL(T0.\"middleName\",'') AS \"Nombre\" " +
                    "FROM {0}.OHEM T0  INNER JOIN {0}.HEM6 T1 ON T0.\"empID\" = T1.\"empID\" " +
                    "WHERE T1.\"roleID\" ='13'", _configuration.GetValue<string>("ServiceLayer:CompanyDB"));

                using (HanaCommand commnad = new HanaCommand(strQuery, connection))
                {
                    using (HanaDataReader reader = (HanaDataReader)commnad.ExecuteReader(System.Data.CommandBehavior.CloseConnection))
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                MaquinistaDomain Maquinista = new MaquinistaDomain();
                                Maquinista.ID = reader["empID"]?.ToString() ?? string.Empty;
                                Maquinista.Nonbre = reader["Nombre"].ToString() ?? string.Empty;
                                lMaquinistaDomain.Add(Maquinista);
                            }
                        }
                    }
                }
            }
            rs.statusCode = System.Net.HttpStatusCode.OK;
            rs.data = lMaquinistaDomain;
            return rs;
        }



#pragma warning disable CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
        public Maquinista(IConfiguration configuration)
        #pragma warning restore CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
        {
            _configuration = configuration;
            _connection = new(configuration);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: eliminar el estado administrado (objetos administrados)
                }

                // TODO: liberar los recursos no administrados (objetos no administrados) y reemplazar el finalizador
                // TODO: establecer los campos grandes como NULL
                disposedValue = true;
            }
        }

        // // TODO: reemplazar el finalizador solo si "Dispose(bool disposing)" tiene código para liberar los recursos no administrados
        // ~Maquinista()
        // {
        //     // No cambie este código. Coloque el código de limpieza en el método "Dispose(bool disposing)".
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // No cambie este código. Coloque el código de limpieza en el método "Dispose(bool disposing)".
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

    }
}
