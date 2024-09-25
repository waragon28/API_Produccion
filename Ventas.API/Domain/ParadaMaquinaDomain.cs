namespace Produccion.API.Domain
{
    public class ParadaMaquinaDomain
    {

        public string U_Fecha { get; set; }
        public string U_Maquina { get; set; }
        public string U_Area { get; set; }
        public string U_Comentario { get; set; }
        public string U_Estado { get; set; }
        public string U_FechaIni { get; set; }
      //  public string U_FechaFin { get; set; }
        public string U_HoraIni { get; set; }
      //  public string U_HoraFin { get; set; }
        public string U_Usuario { get; set; }
        public int U_MotivoParaMaq { get; set; }
    }

    public class AreaDomain
    {
        public string Code { get; set; }
        public string Name { get; set; }
    }

    public class FiltroListParaMaquiDomain
    {
        public string FechaIni { get; set; }
        public string FechaFin { get; set; }
        public string Estado { get; set; }
    }


    public class MaquinaDomain
    {
        public string Code { get; set; }
        public string Name { get; set; }
    }
    public class MotivoParadaDomain
    {
        public string Code { get; set; }
        public string Name { get; set; }
    }
    public class ListParadaMaquina
    {
        public string DocEntry { get; set; }
        public string Maquina { get; set; }
        public string FechaHoraInicio { get; set; }
        public string FechaHoraFin { get; set; }
        public string Usuario { get; set; }
        public string Area { get; set; }
        public string Comentario { get; set; }
        public string Motivo { get; set; }

    }

}
