using MySql.Data.MySqlClient;

namespace inmobiliaria_benenatti.Models;

public class RepositorioPagos
{
    string connectionString = "Server=localhost;User=root;Password=;Database=inmobiliaria;sslmode=none";

   public List<Pago> ObtenerPagos()
{
    List<Pago> pagos = new List<Pago>();

    using (MySqlConnection connection = new MySqlConnection(connectionString))
    {
        var query = @$"SELECT p.{nameof(Pago.IdPago)}, 
                            p.{nameof(Pago.ContratoId)}, 
                            p.{nameof(Pago.FechaPago)},
                            p.{nameof(Pago.Monto)}, 
                            p.{nameof(Pago.Observacion)},
                            p.{nameof(Pago.UsuarioCreadorId)},
                            p.{nameof(Pago.UsuarioAnuladorId)},
                            p.{nameof(Pago.FechaCreacion)},
                            p.{nameof(Pago.FechaAnulacion)},
                            p.{nameof(Pago.Anulado)},
                            c.{nameof(Contrato.IdContrato)},
                            i.nombre AS InquilinoNombre,
                            inm.Direccion AS InmuebleDireccion,
                            uc.Nombre AS UsuarioCreadorNombre,
                            ua.Nombre AS UsuarioAnuladorNombre
                     FROM pagos p
                     INNER JOIN contratos c ON p.{nameof(Pago.ContratoId)} = c.{nameof(Contrato.IdContrato)}
                     INNER JOIN inquilinos i ON c.InquilinoId = i.id
                     INNER JOIN inmuebles inm ON c.InmuebleId = inm.IdInmueble
                     LEFT JOIN usuarios uc ON p.UsuarioCreadorId = uc.IdUsuario
                     LEFT JOIN usuarios ua ON p.UsuarioAnuladorId = ua.IdUsuario";

        using (MySqlCommand command = new MySqlCommand(query, connection))
        {
            connection.Open();
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                pagos.Add(new Pago
                {
                    IdPago = reader.GetInt32(nameof(Pago.IdPago)),
                    ContratoId = reader.GetInt32(nameof(Pago.ContratoId)),
                    FechaPago = reader.GetDateTime(nameof(Pago.FechaPago)),
                    Monto = reader.GetDecimal(nameof(Pago.Monto)),
                    Observacion = reader.IsDBNull(4) ? null : reader.GetString(4),
                    UsuarioCreadorId = reader.IsDBNull(5) ? null : reader.GetInt32(5),
                    UsuarioAnuladorId = reader.IsDBNull(6) ? null : reader.GetInt32(6),
                    FechaCreacion = reader.IsDBNull(7) ? null : reader.GetDateTime(7),
                    FechaAnulacion = reader.IsDBNull(8) ? null : reader.GetDateTime(8),
                    Anulado = reader.GetBoolean(nameof(Pago.Anulado)),
                    Contrato = new Contrato
                    {
                        IdContrato = reader.GetInt32(nameof(Contrato.IdContrato)),
                        Inquilino = new Inquilinos { nombre = reader.GetString("InquilinoNombre") },
                        Inmueble = new Inmueble { Direccion = reader.GetString("InmuebleDireccion") }
                    },
                    UsuarioCreador = reader.IsDBNull(13) ? null : new Usuario { 
                        Nombre = reader.GetString("UsuarioCreadorNombre") 
                    },
                    UsuarioAnulador = reader.IsDBNull(14) ? null : new Usuario { 
                        Nombre = reader.GetString("UsuarioAnuladorNombre") 
                    }
                });
            }
            connection.Close();
        }
        return pagos;
    }
}

    public Pago? Obtener(int id)
    {
        Pago? res = null;

        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            var query = @$"SELECT 
                            p.{nameof(Pago.IdPago)}, 
                            p.{nameof(Pago.ContratoId)}, 
                            p.{nameof(Pago.FechaPago)},
                            p.{nameof(Pago.Monto)}, 
                            p.{nameof(Pago.Observacion)},
                            p.{nameof(Pago.UsuarioCreadorId)},
                            p.{nameof(Pago.UsuarioAnuladorId)},
                            p.{nameof(Pago.FechaCreacion)},
                            p.{nameof(Pago.FechaAnulacion)},
                            p.{nameof(Pago.Anulado)},
                            c.{nameof(Contrato.IdContrato)},
                            i.nombre AS InquilinoNombre,
                            inm.Direccion AS InmuebleDireccion,
                            uc.Nombre AS UsuarioCreadorNombre,
                            ua.Nombre AS UsuarioAnuladorNombre
                     FROM pagos p
                     INNER JOIN contratos c ON p.{nameof(Pago.ContratoId)} = c.{nameof(Contrato.IdContrato)}
                     INNER JOIN inquilinos i ON c.InquilinoId = i.id
                     INNER JOIN inmuebles inm ON c.InmuebleId = inm.IdInmueble
                     LEFT JOIN usuarios uc ON p.UsuarioCreadorId = uc.IdUsuario
                     LEFT JOIN usuarios ua ON p.UsuarioAnuladorId = ua.IdUsuario
                     WHERE p.{nameof(Pago.IdPago)} = @IdPago";

            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@IdPago", id);
                connection.Open();
                var reader = command.ExecuteReader();
                if (reader.Read())
                {
                    res = new Pago
                    {
                        IdPago = reader.GetInt32(0),
                        ContratoId = reader.GetInt32(1),
                        FechaPago = reader.GetDateTime(2),
                        Monto = reader.GetDecimal(3),
                        Observacion = reader.IsDBNull(4) ? null : reader.GetString(4),
                        UsuarioCreadorId = reader.IsDBNull(5) ? null : reader.GetInt32(5),
                        UsuarioAnuladorId = reader.IsDBNull(6) ? null : reader.GetInt32(6),
                        FechaCreacion = reader.IsDBNull(7) ? null : reader.GetDateTime(7),
                        FechaAnulacion = reader.IsDBNull(8) ? null : reader.GetDateTime(8),
                        Anulado = reader.GetBoolean(9),
                        Contrato = new Contrato
                        {
                            IdContrato = reader.GetInt32(10),
                            Inquilino = new Inquilinos { nombre = reader.GetString(11) },
                            Inmueble = new Inmueble { Direccion = reader.GetString(12) }
                        },
                        UsuarioCreador = reader.IsDBNull(13) ? null : new Usuario
                        {
                            Nombre = reader.GetString(13)
                        },
                        UsuarioAnulador = reader.IsDBNull(14) ? null : new Usuario
                        {
                            Nombre = reader.GetString(14)
                        }
                    };
                }
                connection.Close();
            }
        }
        return res;
    }

public int Alta(Pago pago)
{
    int res = -1;
    using (MySqlConnection connection = new MySqlConnection(connectionString))
    {
        var query = @$"INSERT INTO pagos 
                         ({nameof(Pago.ContratoId)}, 
                          {nameof(Pago.FechaPago)}, 
                          {nameof(Pago.Monto)}, 
                          {nameof(Pago.Observacion)},
                          {nameof(Pago.UsuarioCreadorId)},
                          {nameof(Pago.FechaCreacion)})
                         VALUES (@contratoId, @fechaPago, @monto, @observacion, @usuarioCreadorId, @fechaCreacion);
                         SELECT LAST_INSERT_ID();";

        using (MySqlCommand command = new MySqlCommand(query, connection))
        {
            command.Parameters.AddWithValue("@contratoId", pago.ContratoId);
            command.Parameters.AddWithValue("@fechaPago", pago.FechaPago);
            command.Parameters.AddWithValue("@monto", pago.Monto);
            command.Parameters.AddWithValue("@observacion", (object?)pago.Observacion ?? DBNull.Value);
            command.Parameters.AddWithValue("@usuarioCreadorId", pago.UsuarioCreadorId);
            command.Parameters.AddWithValue("@fechaCreacion", DateTime.Now);
            
            connection.Open();
            res = Convert.ToInt32(command.ExecuteScalar());
            connection.Close();
        }
    }
    return res;
}

public bool AnularPago(int pagoId, int usuarioAnuladorId)
{
    bool res = false;
    using (MySqlConnection connection = new MySqlConnection(connectionString))
    {
        var query = $@"UPDATE pagos SET 
                        {nameof(Pago.Anulado)} = 1,
                        {nameof(Pago.UsuarioAnuladorId)} = @usuarioAnuladorId,
                        {nameof(Pago.FechaAnulacion)} = @fechaAnulacion
                         WHERE {nameof(Pago.IdPago)} = @IdPago;";

        using (MySqlCommand command = new MySqlCommand(query, connection))
        {
            command.Parameters.AddWithValue("@usuarioAnuladorId", usuarioAnuladorId);
            command.Parameters.AddWithValue("@fechaAnulacion", DateTime.Now);
            command.Parameters.AddWithValue("@IdPago", pagoId);
            
            connection.Open();
            res = command.ExecuteNonQuery() > 0;
            connection.Close();
        }
    }
    return res;
}

    public bool Modificar(Pago pago)
{
    bool res = false;
    using (MySqlConnection connection = new MySqlConnection(connectionString))
    {
        var query = $@"UPDATE pagos SET 
                        {nameof(Pago.ContratoId)}=@contratoId, 
                        {nameof(Pago.FechaPago)}=@fechaPago, 
                        {nameof(Pago.Monto)}=@monto, 
                        {nameof(Pago.Observacion)}=@observacion,
                        {nameof(Pago.UsuarioCreadorId)}=@usuarioCreadorId
                         WHERE {nameof(Pago.IdPago)}=@IdPago;";

        using (MySqlCommand command = new MySqlCommand(query, connection))
        {
            command.Parameters.AddWithValue("@contratoId", pago.ContratoId);
            command.Parameters.AddWithValue("@fechaPago", pago.FechaPago);
            command.Parameters.AddWithValue("@monto", pago.Monto);
            command.Parameters.AddWithValue("@observacion", (object?)pago.Observacion ?? DBNull.Value);
            command.Parameters.AddWithValue("@usuarioCreadorId", pago.UsuarioCreadorId);
            command.Parameters.AddWithValue("@IdPago", pago.IdPago);
            
            connection.Open();
            res = command.ExecuteNonQuery() > 0;
            connection.Close();
        }
    }
    return res;
}

    public int Baja(int id)
    {
        int res = -1;
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            var query = $@"DELETE FROM pagos WHERE {nameof(Pago.IdPago)} = @IdPago";

            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@IdPago", id);
                connection.Open();
                res = command.ExecuteNonQuery();
                connection.Close();
            }
        }
        return res;
    }

    public List<Pago> ObtenerPorContrato(int contratoId)
    {
        List<Pago> pagos = new List<Pago>();

        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            var query = @$"SELECT {nameof(Pago.IdPago)}, 
                                {nameof(Pago.ContratoId)}, 
                                {nameof(Pago.FechaPago)},
                                {nameof(Pago.Monto)}, 
                                {nameof(Pago.Observacion)}
                             FROM pagos
                             WHERE {nameof(Pago.ContratoId)} = @ContratoId";

            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@ContratoId", contratoId);
                connection.Open();
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    pagos.Add(new Pago
                    {
                        IdPago = reader.GetInt32(nameof(Pago.IdPago)),
                        ContratoId = reader.GetInt32(nameof(Pago.ContratoId)),
                        FechaPago = reader.GetDateTime(nameof(Pago.FechaPago)),
                        Monto = reader.GetDecimal(nameof(Pago.Monto)),
                        Observacion = reader.IsDBNull(4) ? null : reader.GetString(4)
                    });
                }
                connection.Close();
            }
            return pagos;
        }
    }
}