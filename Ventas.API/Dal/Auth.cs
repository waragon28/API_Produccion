using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Sap.Data.Hana;
using System.Data.Common;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using InspeccionProduccion.API.Domain;
using InspeccionProduccion.API.Interfaces;
using InspeccionProduccion.API.Services;

namespace InspeccionProduccion.API.Dal
{
    public class Auth : IAuth, IDisposable
    {

        private readonly IConfiguration _configuration;
        private readonly Connection _connection;
        private readonly ServiceLayerSettings _serviceLayerSettings;
        private bool disposedValue;

        public Auth(IConfiguration configuration, IOptions<ServiceLayerSettings> slOptions)
        {
            _configuration = configuration;
            _connection = new(configuration);
            _serviceLayerSettings = slOptions.Value;
        }

#pragma warning disable CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
        public Auth(IConfiguration configuration)
#pragma warning restore CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
        {
            _configuration = configuration;
            _connection = new(configuration);
        }


        public async Task<Response> Authenticate(string Usuario,string Password)
        {
            
            string Respuesta = string.Empty;
            using (HanaConnection connection = _connection.GetConnection())
            {
                connection.Open();

                string strSQL = string.Format("CALL \"{0}\".API_SELECTPASSWORD ('{1}','{2}','{3}')", _configuration.GetValue<string>("ServiceLayer:CompanyDB")/*_serviceLayerSettings.CompanyDB*/, Usuario, Password,
                     _configuration.GetValue<string>("settings:secretKey"));
                using (HanaCommand commnad = new HanaCommand(strSQL, connection))
                {
                    using (HanaDataReader reader = (HanaDataReader)commnad.ExecuteReader(System.Data.CommandBehavior.CloseConnection))
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                if ((reader["HASH"].ToString() ?? "Error").Equals((reader["U_VIS_CLAVE"].ToString() ?? string.Empty)))
                                {
                                    // Generar el token JWT
                                    #pragma warning disable CS8602 // Desreferencia de una referencia posiblemente NULL.
                                    Respuesta = "OK";// JwtToken.GenerateJwtToken(Usuario, _configuration.GetValue<string>("settings:secretKey").ToString(), 60);
                                    #pragma warning restore CS8602 // Desreferencia de una referencia posiblemente NULL.

                                }
                                else
                                {
                                    Respuesta = "Usuario y contraseña invalido";
                                }
                            }
                        }
                    }
                    //Respuesta = "Usuario y contraseña invalido";
                }
            }

            return new Response
            {
                statusCode = HttpStatusCode.OK,
                data = Respuesta
            };
           
        }




        #region disposable
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
        // ~Auth()
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
        #endregion

    }
}
