using InspeccionProduccion.API.Domain;
using InspeccionProduccion.API.Interfaces;
using InspeccionProduccion.API.Services;
using Microsoft.CodeAnalysis;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Produccion.API.Dal;
using Sap.Data.Hana;
using System.Net;
using System.Text;

namespace InspeccionProduccion.API.Dal
{
    public class Evaluacion : IEvaluacion, IDisposable
    {
        private readonly IConfiguration _configuration;
        private readonly Connection _connection;
        private readonly ServiceLayerSettings _serviceLayerSettings;
        private bool disposedValue;

        public Evaluacion(IConfiguration configuration, IOptions<ServiceLayerSettings> slOptions)
        {
            _configuration = configuration;
            _connection = new(configuration);
            _serviceLayerSettings = slOptions.Value;
        }

            #pragma warning disable CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
        public Evaluacion(IConfiguration configuration)
            #pragma warning restore CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
        {
            _configuration = configuration;
            _connection = new(configuration);
        }

      
        public async Task<Response> PostEvaluacion(EvaluacionDomain evaluacionDomain)
        {
            string Respuesta = string.Empty;
            Response rs = new();
            ServiceLayerSAP serviceLayerSAP  = new ServiceLayerSAP();
            string Login = _configuration.GetValue<string>("ServiceLayer:Url") + "b1s/v1/Login";
            string Logout = _configuration.GetValue<string>("ServiceLayer:Url") + "b1s/v1/Logout";
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
                string token = jsonResponse.SessionId;

                 var Url = _configuration.GetValue<string>("ServiceLayer:Url");
                // Realizar la segunda solicitud utilizando el token
                return rs = serviceLayerSAP.PostDocumentWithToken(Url + "b1s/v1/VIS_PRO_OIPD", token, JsonConvert.SerializeObject(evaluacionDomain).ToString());

            }
            else
            {
                Console.WriteLine("Error: " + response.StatusCode);
                return rs=new Response();
                rs.statusCode=HttpStatusCode.BadRequest;
                rs.data = "Error";

            }
            var Rs = ServiceLayerSAP.SendLogoutRequest(Logout);

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
