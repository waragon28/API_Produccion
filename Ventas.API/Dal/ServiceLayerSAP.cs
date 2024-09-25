using InspeccionProduccion.API.Domain;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Text;

namespace Produccion.API.Dal
{
    public class ServiceLayerSAP
    {
        public static HttpResponseMessage SendLogoutRequest(string url)
        {
            // Configurar el handler con la validación de certificado personalizada
            HttpClientHandler clientHandler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true
            };

            // Crear HttpClient con el handler configurado
            using (HttpClient client = new HttpClient(clientHandler))
            {
                // Serializar el objeto a JSON
                StringContent content = new StringContent("", Encoding.UTF8, "application/json");

                // Enviar la solicitud POST
                HttpResponseMessage response = client.PostAsync(url, content).Result;
                return response;
            }
        }

        public static HttpResponseMessage SendLoginRequest(string url, LoginRequest loginRequest)
        {
            // Configurar el handler con la validación de certificado personalizada
            HttpClientHandler clientHandler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true
            };

            // Crear HttpClient con el handler configurado
            using (HttpClient client = new HttpClient(clientHandler))
            {
                // Serializar el objeto a JSON
                string json = JsonConvert.SerializeObject(loginRequest);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

                // Enviar la solicitud POST
                HttpResponseMessage response = client.PostAsync(url, content).Result;
                return response;
            }
        }


        public Response PostDocumentWithToken(string url, string token, string Json)
        {
            Response rs = new();
            string Respuesta = string.Empty;
            var baseAddress = new Uri(url);

            var cookieContainer = new CookieContainer();
            HttpClientHandler clientHandler = new HttpClientHandler
            {
                CookieContainer = cookieContainer,
                ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true
            };

            HttpClient client = new HttpClient(clientHandler)
            {
                BaseAddress = baseAddress
            };
            client.DefaultRequestHeaders.Add("Prefer", "odata.maxpagesize=2000");
            cookieContainer.Add(baseAddress, new Cookie("B1SESSION", token));

            StringContent content = new StringContent(Json, Encoding.UTF8, "application/json");

            // Enviar la solicitud POST
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            // Manejo de la respuesta
            if (response.IsSuccessStatusCode)
            {
                string responseBody = response.Content.ReadAsStringAsync().Result;
                JObject jsonObject = JObject.Parse(responseBody);
                rs.statusCode = response.StatusCode;
                int docNum = jsonObject["DocNum"].Value<int>();
                rs.data = "Se Ingreso la Evaluacion Nº " + Convert.ToString(docNum);
            }
            else
            {
                string responseBody = response.Content.ReadAsStringAsync().Result;
                rs.statusCode = response.StatusCode;
                var jsonResponse = JsonConvert.SerializeObject(responseBody);
                // Deserializar el JSON a un objeto dinámico
                var jsonObject = JsonConvert.DeserializeObject<dynamic>(jsonResponse);
                // Deserializar el JSON a un objeto dinámico
                dynamic jsonObject1 = JsonConvert.DeserializeObject<dynamic>(jsonObject);

                // Acceder al valor de la propiedad "value"
                string value = jsonObject1.error.message.value;

                // Deserializar la respuesta JSON a un objeto dinámico
                rs.statusCode = HttpStatusCode.BadRequest;
                rs.data = value;

            }
            return rs;
        }



    }
}
