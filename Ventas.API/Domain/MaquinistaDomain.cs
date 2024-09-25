namespace InspeccionProduccion.API.Domain
{
    public class MaquinistaDomain
    {
        public string ID  { get; set; }
        public string Nonbre  { get; set; }
    }
    public class Lineas
    {
        public string ID { get; set; }
        public string Descripcion { get; set; }
    }

    public class Paletc
    {
        public string Producto { get; set; }
        public string UM { get; set; }
        public string Linea { get; set; }
        public double CantidadPlanificada { get; set; }
    }

    public class ListPalet
    {
        public string OT { get; set; }
        public string U_Fecha { get; set; }
        public string U_Turno { get; set; }
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
        public string U_Usuario { get; set; }
        public string U_Cantidad { get; set; }
        public string U_Maquinista { get; set; }
    }


}
