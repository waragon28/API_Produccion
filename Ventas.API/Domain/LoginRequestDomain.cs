using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace InspeccionProduccion.API.Domain
{
    public class LoginRequest
    {
        [JsonPropertyName("CompanyDB")]
        public string CompanyDB { get; set; }
        [JsonPropertyName("Password")]
        public string Password { get; set; }
        [JsonPropertyName("UserName")]
        public string UserName { get; set; }
    }

    public class Auth
    {
        public string JwtToen { get; set; } = "";
    }

    public record LoginRequestDto
    {
       // [Required]
        public string User { get; set; }
       // [Required]
        public string PassWord { get; set; }
    }
}
