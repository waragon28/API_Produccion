using InspeccionProduccion.API.Domain;
using InspeccionProduccion.API.Interfaces;
using InspeccionProduccion.API.Services;
using Microsoft.CodeAnalysis;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Produccion.API.Domain;
using Produccion.API.Interfaces;
using Sap.Data.Hana;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Net;

namespace Produccion.API.Dal
{
    public class ParadaMaquinaDAL : IParadaMaquina, IDisposable
    {

        private readonly IConfiguration _configuration;
        private readonly Connection _connection;
        private readonly ServiceLayerSettings _serviceLayerSettings;
        private bool disposedValue;
        private bool disposedValue1;

        public ParadaMaquinaDAL(IConfiguration configuration, IOptions<ServiceLayerSettings> slOptions)
        {
            _configuration = configuration;
            _connection = new(configuration);
            _serviceLayerSettings = slOptions.Value;
        }
#pragma warning disable CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
        public ParadaMaquinaDAL(IConfiguration configuration)
#pragma warning restore CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
        {
            _configuration = configuration;
            _connection = new(configuration);
        }

        public async Task<Response> RegistroParadaMaquina(ParadaMaquinaDomain paradaMaquinaDomain)
        {
            string Respuesta = string.Empty;
            Response rs = new();
            ServiceLayerSAP serviceLayerSAP = new ServiceLayerSAP();
            string Login = _configuration.GetValue<string>("ServiceLayer:Login");
            LoginRequest objLogin = new()
            {
                CompanyDB = _configuration.GetValue<string>("ServiceLayer:CompanyDB"),
                Password = _configuration.GetValue<string>("ServiceLayer:Password"),
                UserName = _configuration.GetValue<string>("ServiceLayer:UserName")
            };
            // Llamada al método que realiza la solicitud
            var response = ServiceLayerSAP.SendLoginRequest(Login, objLogin);

            if (response.IsSuccessStatusCode)
            {
                string responseBody = response.Content.ReadAsStringAsync().Result;
                // Deserializar la respuesta JSON a un objeto dinámico
                dynamic jsonResponse = JsonConvert.DeserializeObject(responseBody);

                // Obtener el valor del token
                string token = jsonResponse.token;

                var Url = _configuration.GetValue<string>("ServiceLayer:Url");
                // Realizar la segunda solicitud utilizando el token
                return rs = serviceLayerSAP.PostDocumentWithToken(Url + "b1s/v1/VIS_PRO_ORPM", token, JsonConvert.SerializeObject(paradaMaquinaDomain).ToString());
            }
            else
            {
                Console.WriteLine("Error: " + response.StatusCode);
                return rs = new Response();
                rs.statusCode = HttpStatusCode.BadRequest;
                rs.data = "Error";

            }
        }


        public Response StockParadaMaquina(string DocEntry)
        {
            Response rs = new Response();
            string Respuesta = string.Empty;
            DateTime fechaActual = DateTime.Now;

            using (HanaConnection connection = _connection.GetConnection())
            {
                connection.Open();

                string strQuery = string.Format("UPDATE {0}.\"@VIS_PRO_ORPM\" SET \"U_FechaFin\"='{1}', \"U_HoraFin\"= '{2}' WHERE \"DocEntry\"='{3}'", 
                    _configuration.GetValue<string>("ServiceLayer:CompanyDB"), fechaActual.ToString("yyyyMMdd"), fechaActual.ToString("HHMM"), DocEntry);

                using (HanaCommand commnad = new HanaCommand(strQuery, connection))
                {
                    using (HanaDataReader reader = (HanaDataReader)commnad.ExecuteReader(System.Data.CommandBehavior.CloseConnection))
                    {

                        Respuesta = "Se detuvo con exito";
                    }
                }
            }
            rs.statusCode = System.Net.HttpStatusCode.OK;
            rs.data = Respuesta;
            return rs;
        }

        public Response GetArea()
        {
            Response rs = new Response();
            List<AreaDomain> lAreaDomain = new List<AreaDomain>();
            using (HanaConnection connection = _connection.GetConnection())
            {
                connection.Open();

                string strQuery = string.Format("SELECT T0.\"Code\", T0.\"Name\" FROM {0}.OUDP T0 WHERE T0.\"U_VIS_VisibleParaMaq\"='Y'", _configuration.GetValue<string>("ServiceLayer:CompanyDB"));

                using (HanaCommand commnad = new HanaCommand(strQuery, connection))
                {
                    using (HanaDataReader reader = (HanaDataReader)commnad.ExecuteReader(System.Data.CommandBehavior.CloseConnection))
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                AreaDomain areaDomain = new AreaDomain();
                                areaDomain.Code = reader["Code"]?.ToString() ?? string.Empty;
                                areaDomain.Name = reader["Name"].ToString() ?? string.Empty;
                                lAreaDomain.Add(areaDomain);
                            }
                        }
                    }
                }
            }
            rs.statusCode = System.Net.HttpStatusCode.OK;
            rs.data = lAreaDomain;
            return rs;
        }

        public Response GetMaquina()
        {
            Response rs = new Response();
            List<MaquinaDomain> lMaquinaDomain = new List<MaquinaDomain>();
            using (HanaConnection connection = _connection.GetConnection())
            {
                connection.Open();

                string strQuery = string.Format("SELECT T0.\"VisResCode\",T0.\"ResName\" FROM {0}.ORSC T0"/* WHERE T0.\"U_VIS_ListParaMaq\" ='Y'"*/, _configuration.GetValue<string>("ServiceLayer:CompanyDB"));

                using (HanaCommand commnad = new HanaCommand(strQuery, connection))
                {
                    using (HanaDataReader reader = (HanaDataReader)commnad.ExecuteReader(System.Data.CommandBehavior.CloseConnection))
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                MaquinaDomain maquinaDomain = new MaquinaDomain();
                                maquinaDomain.Code = reader["VisResCode"]?.ToString() ?? string.Empty;
                                maquinaDomain.Name = reader["ResName"].ToString() ?? string.Empty;
                                lMaquinaDomain.Add(maquinaDomain);
                            }
                        }
                    }
                }
            }
            rs.statusCode = System.Net.HttpStatusCode.OK;
            rs.data = lMaquinaDomain;
            return rs;
        }
        public Response GetMotivoParada(string Area)
        {
            Response rs = new Response();
            List<MotivoParadaDomain> lMotivoParadaDomain = new List<MotivoParadaDomain>();
            using (HanaConnection connection = _connection.GetConnection())
            {
                connection.Open();

                string strQuery = string.Format("SELECT T0.\"DocEntry\", T0.\"U_Motivo\" FROM {0}.\"@VIS_PRO_OMPM\"  T0 WHERE T0.\"U_Area\" ='{1}'", _configuration.GetValue<string>("ServiceLayer:CompanyDB"), Area /*_configuration.GetValue<string>("ServiceLayer:CompanyDB").ToString()*/);

                using (HanaCommand commnad = new HanaCommand(strQuery, connection))
                {
                    using (HanaDataReader reader = (HanaDataReader)commnad.ExecuteReader(System.Data.CommandBehavior.CloseConnection))
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                MotivoParadaDomain motivoParadaDomain = new MotivoParadaDomain();
                                motivoParadaDomain.Code = reader["DocEntry"]?.ToString() ?? string.Empty;
                                motivoParadaDomain.Name = reader["U_Motivo"].ToString() ?? string.Empty;
                                lMotivoParadaDomain.Add(motivoParadaDomain);
                            }
                        }
                    }
                }
            }
            rs.statusCode = System.Net.HttpStatusCode.OK;
            rs.data = lMotivoParadaDomain;
            return rs;
        }


        public Response GetListaParadaMaquina(FiltroListParaMaquiDomain filtroListParaMaquiDomain)
        {
            Response rs = new Response();
            List<ListParadaMaquina> lListParadaMaquina = new List<ListParadaMaquina>();
            using (HanaConnection connection = _connection.GetConnection())
            {
                connection.Open();
                
                string QueryEstado = string.Empty;

                switch (filtroListParaMaquiDomain.Estado)
                {
                    case "F":
                        QueryEstado = " AND IFNULL(T0.\"U_FechaFin\",'')<>'' "; 
                        break;
                    case "I":
                        QueryEstado = " AND IFNULL(T0.\"U_FechaFin\",'')='' ";
                        break;
                }

                string strQuery = string.Format("SELECT T0.\"DocEntry\", T1.\"ResName\", TO_NVARCHAR(T0.\"U_FechaIni\",'DD-MM-YYYY') ||'   '|| {0}.\"FX_HORATEXT\"(T0.\"U_HoraIni\") AS \"FechaHoraInicio\", "+
                    "TO_NVARCHAR(T0.\"U_FechaFin\",'DD-MM-YYYY') ||'   '|| {0}.\"FX_HORATEXT\"(T0.\"U_HoraFin\")  AS \"FechaHoraFin\",T2.\"Name\" as \"Area\"," +
                    "T5.\"lastName\"||' '||T5.\"firstName\"||' '|| T5.\"middleName\" as \"U_Usuario\", " +
                    "\"U_Comentario\",T3.\"U_Motivo\" FROM {0}.\"@VIS_PRO_ORPM\" " +
                    " T0 INNER JOIN  {0}.ORSC T1 ON T1.\"VisResCode\"=T0.\"U_Maquina\" "+
                    "INNER JOIN  {0}.OUDP T2 ON T2.\"Code\"=T0.\"U_Area\" "+
                    "LEFT JOIN  {0}.\"@VIS_PRO_OMPM\"  T3 ON T3.\"DocEntry\"=T0.\"U_MotivoParaMaq\" " +
                    "LEFT JOIN {0}.OHEM T5 ON TO_NVARCHAR(T0.\"U_Usuario\")=TO_VARCHAR(T5.\"U_VIS_TOKEN\") " +
                    "WHERE T0.\"U_Fecha\">='{1}' AND T0.\"U_Fecha\"<='{2}' " + QueryEstado+
                                                "ORDER BY T0.\"DocEntry\"", _configuration.GetValue<string>("ServiceLayer:CompanyDB"), filtroListParaMaquiDomain.FechaIni, filtroListParaMaquiDomain.FechaFin);

                using (HanaCommand commnad = new HanaCommand(strQuery, connection))
                {
                    using (HanaDataReader reader = (HanaDataReader)commnad.ExecuteReader(System.Data.CommandBehavior.CloseConnection))
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                ListParadaMaquina listParadaMaquina = new ListParadaMaquina();
                                listParadaMaquina.DocEntry = reader["DocEntry"]?.ToString() ?? string.Empty;
                                listParadaMaquina.Maquina = reader["ResName"].ToString() ?? string.Empty;
                                listParadaMaquina.FechaHoraInicio = reader["FechaHoraInicio"].ToString() ?? string.Empty;
                                listParadaMaquina.FechaHoraFin = reader["FechaHoraFin"].ToString() ?? string.Empty;
                                listParadaMaquina.Usuario = reader["U_Usuario"].ToString() ?? string.Empty;

                                listParadaMaquina.Area = reader["Area"].ToString() ?? string.Empty;
                                listParadaMaquina.Comentario = reader["U_Comentario"].ToString() ?? string.Empty;
                                listParadaMaquina.Motivo = reader["U_Motivo"].ToString() ?? string.Empty;

                                lListParadaMaquina.Add(listParadaMaquina);
                            }
                        }
                    }
                }
            }
            rs.statusCode = System.Net.HttpStatusCode.OK;
            rs.data = lListParadaMaquina;
            return rs;
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
        // ~Evaluacion()
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
