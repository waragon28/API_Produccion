using InspeccionProduccion.API.Domain;
using InspeccionProduccion.API.Interfaces;
using InspeccionProduccion.API.Services;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.Options;
using Sap.Data.Hana;

namespace InspeccionProduccion.API.Dal
{
    public class Palet : IPalet, IDisposable
    {
        private readonly IConfiguration _configuration;
        private readonly Connection _connection;
        private readonly ServiceLayerSettings _serviceLayerSettings;
        private bool disposedValue;

        public Palet(IConfiguration configuration, IOptions<ServiceLayerSettings> slOptions)
        {
            _configuration = configuration;
            _connection = new(configuration);
            _serviceLayerSettings = slOptions.Value;
        }


#pragma warning disable CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
        public Palet(IConfiguration configuration)
#pragma warning restore CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
        {
            _configuration = configuration;
            _connection = new(configuration);
        }


        public Response GetPaler(string NroOf, string CodBarra)
        {
            Response rs = new Response();
            Paletc APalet = new Paletc();
            using (HanaConnection connection = _connection.GetConnection())
            {
                connection.Open();

                string strQuery =string.Format("SELECT T1.\"ItemName\", T0.\"Uom\", T0.\"PlannedQty\" ,T2.\"ResName\" FROM \"{0}\".OWOR T0 " +
                                  "INNER JOIN \"{0}\".OITM T1 ON T0.\"ItemCode\"=T1.\"ItemCode\" " +
                                  "LEFT JOIN \"{0}\".ORSC T2 ON T2.\"VisResCode\"=T1.\"U_LineMachine\" " +
                                  "WHERE T0.\"U_SYP_OP_ENV\"='{1}' ", _configuration.GetValue<string>("ServiceLayer:CompanyDB").ToString(), CodBarra);

                using (HanaCommand commnad = new HanaCommand(strQuery, connection))
                {
                    using (HanaDataReader reader = (HanaDataReader)commnad.ExecuteReader(System.Data.CommandBehavior.CloseConnection))
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                APalet = new Paletc();
                                APalet.Producto =reader["ItemName"]?.ToString() ?? string.Empty;
                                APalet.UM = reader["Uom"].ToString() ?? string.Empty;
                                APalet.Linea = reader["ResName"].ToString() ?? string.Empty;
                                APalet.CantidadPlanificada =Convert.ToDouble(reader["PlannedQty"].ToString());
                            }
                          }
                    }
                }
            }
            rs.statusCode = System.Net.HttpStatusCode.OK;
            rs.data = APalet;
            return rs;
        }

        public Response ListPalet(string FechaIn,string FechaFin )
        {
            Response rs = new Response();
            ListPalet ListPalet = new ListPalet();
            List<ListPalet> List_Palet = new List<ListPalet>();
            using (HanaConnection connection = _connection.GetConnection())
            {
                connection.Open();

                string strQuery = string.Format("SELECT T0.\"DocEntry\", T0.\"U_Fecha\", T0.\"U_Turno\", T0.\"U_OT\", T0.\"U_Peso_Check\", T0.\"U_Peso_Comment\", T0.\"U_Etiq_Check\", "+
                                                "T0.\"U_Etiq_Comment\", T0.\"U_Lot_Check\", T0.\"U_Lot_Comment\", T0.\"U_Limp_Check\", T0.\"U_Limp_Comment\", T0.\"U_Sell_Check\", "+
                                                "T0.\"U_Sell_Comment\", T0.\"U_Enc_Check\", T0.\"U_Enc_Comment\", T0.\"U_Rotulo_Check\", T0.\"U_Rotulo_Comment\", "+
                                                "T0.\"U_Palet_Check\", T0.\"U_Palet_Comment\", T0.\"U_Conformidad\", T0.\"U_Conformidad_Comment\", "+
                                                "T1.\"lastName\"||' '||T1.\"firstName\"||' '|| T1.\"middleName\" as \"U_Usuario\"," +
                                                "T0.\"U_Cantidad\", T0.\"U_Maquinista\" FROM {0}.\"@VIS_PRO_OIPD\"  T0 "+
                                                "INNER JOIN {0}.OHEM T1 ON TO_NVARCHAR(T0.\"U_Usuario\")=TO_VARCHAR(T1.\"U_VIS_TOKEN\") " +
                                                "WHERE  T0.\"U_Fecha\" BETWEEN  '{1}' AND '{2}' ", _configuration.GetValue<string>("ServiceLayer:CompanyDB"), FechaIn,FechaFin);

                using (HanaCommand commnad = new HanaCommand(strQuery, connection))
                {
                    using (HanaDataReader reader = (HanaDataReader)commnad.ExecuteReader(System.Data.CommandBehavior.CloseConnection))
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                ListPalet = new ListPalet();

                                ListPalet.OT = reader["U_OT"]?.ToString() ?? string.Empty;
                                ListPalet.U_Fecha = reader["U_Fecha"]?.ToString() ?? string.Empty;
                                ListPalet.U_Turno = reader["U_Turno"]?.ToString() ?? string.Empty;
                                ListPalet.U_Peso_Check = reader["U_Peso_Check"]?.ToString() ?? string.Empty;
                                ListPalet.U_Peso_Comment = reader["U_Peso_Comment"]?.ToString() ?? string.Empty;
                                ListPalet.U_Etiq_Check = reader["U_Etiq_Check"]?.ToString() ?? string.Empty;
                                ListPalet.U_Etiq_Comment = reader["U_Etiq_Comment"]?.ToString() ?? string.Empty;
                                ListPalet.U_Lot_Check = reader["U_Lot_Check"]?.ToString() ?? string.Empty;
                                ListPalet.U_Lot_Comment = reader["U_Lot_Comment"]?.ToString() ?? string.Empty;
                                ListPalet.U_Limp_Check = reader["U_Limp_Check"]?.ToString() ?? string.Empty;
                                ListPalet.U_Limp_Comment = reader["U_Limp_Comment"]?.ToString() ?? string.Empty;
                                ListPalet.U_Sell_Check = reader["U_Sell_Check"]?.ToString() ?? string.Empty;
                                ListPalet.U_Sell_Comment = reader["U_Sell_Comment"]?.ToString() ?? string.Empty;
                                ListPalet.U_Enc_Check = reader["U_Enc_Check"]?.ToString() ?? string.Empty;
                                ListPalet.U_Enc_Comment = reader["U_Enc_Comment"]?.ToString() ?? string.Empty;
                                ListPalet.U_Rotulo_Check = reader["U_Rotulo_Check"]?.ToString() ?? string.Empty;
                                ListPalet.U_Rotulo_Comment = reader["U_Rotulo_Comment"]?.ToString() ?? string.Empty;
                                ListPalet.U_Palet_Check = reader["U_Palet_Check"]?.ToString() ?? string.Empty;
                                ListPalet.U_Palet_Comment = reader["U_Palet_Comment"]?.ToString() ?? string.Empty;
                                ListPalet.U_Conformidad = reader["U_Conformidad"]?.ToString() ?? string.Empty;
                                ListPalet.U_Conformidad_Comment = reader["U_Conformidad_Comment"]?.ToString() ?? string.Empty;
                                ListPalet.U_Usuario = reader["U_Usuario"]?.ToString() ?? string.Empty;
                                ListPalet.U_Cantidad = reader["U_Cantidad"]?.ToString() ?? string.Empty;
                                ListPalet.U_Maquinista = reader["U_Maquinista"]?.ToString() ?? string.Empty;

                                List_Palet.Add(ListPalet);
                            }
                        }
                    }
                }
            }
            rs.statusCode = System.Net.HttpStatusCode.OK;
            rs.data = List_Palet;
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
        // ~Palet()
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
