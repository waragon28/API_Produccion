namespace InspeccionProduccion.API.Domain
{
    public record BarraDomain
    {
        public string Barra { get; set; }
    }

    public record FiltroFecha
    {
        public string FechaIni { get; set; }
        public string FechaFin { get; set; }
    }


    public record EvaluacionDomain
    {
        public string U_Fecha { get; set; }
        public string U_Turno { get; set; }
        public string U_OT { get; set; }
        public string U_Peso_Check { get; set; }
        public string U_Peso_Comment { get; set; }
        public string U_Etiq_Check { get; set; }
        public string U_Etiq_Comment { get; set; }
        public string U_Lot_Check { get; set; }
        public string U_Lot_Comment { get; set; }
        public string U_Limp_Check { get; set; }
        public string U_Limp_Comment { get; set; }
        public string U_Sell_Check { get; set; }
        public string U_Sell_Comment { get; set; }
        public string U_Enc_Check { get; set; }
        public string U_Enc_Comment { get; set; }
        public string U_Rotulo_Check { get; set; }
        public string U_Rotulo_Comment { get; set; }
        public string U_Palet_Check { get; set; }
        public string U_Palet_Comment { get; set; }
        public string U_Conformidad { get; set; }
        public string U_Conformidad_Comment { get; set; }
        //public string U_Maquinista { get; set; }
        public string U_Usuario { get; set; }
        public int U_Cantidad { get; set; }

    }
}
