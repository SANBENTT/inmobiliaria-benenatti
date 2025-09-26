using MySql.Data.MySqlClient;
using System.Data;

namespace inmobiliaria_benenatti.Models;

public class RepositorioContratos
{
    string connectionString = "Server=localhost;User=root;Password=;Database=inmobiliaria;sslmode=none";

    public List<Contrato> ObtenerContratos()
{
    List<Contrato> contratos = new List<Contrato>();

    using (MySqlConnection connection = new MySqlConnection(connectionString))
    {
        var query = @$"SELECT c.{nameof(Contrato.IdContrato)}, 
                            c.{nameof(Contrato.InquilinoId)}, 
                            c.{nameof(Contrato.InmuebleId)},
                            c.{nameof(Contrato.FechaInicio)}, 
                            c.{nameof(Contrato.FechaFin)}, 
                            c.{nameof(Contrato.Monto)},
                            c.{nameof(Contrato.UsuarioCreadorId)},
                            c.{nameof(Contrato.UsuarioTerminadorId)},
                            c.{nameof(Contrato.FechaCreacion)},
                            c.{nameof(Contrato.FechaTerminacion)},
                            c.{nameof(Contrato.Terminado)},
                            i.nombre AS InquilinoNombre,
                            inm.Direccion AS InmuebleDireccion,
                            uc.Nombre AS UsuarioCreadorNombre,
                            ut.Nombre AS UsuarioTerminadorNombre
                     FROM contratos c
                     INNER JOIN inquilinos i ON c.InquilinoId = i.id
                     INNER JOIN inmuebles inm ON c.InmuebleId = inm.IdInmueble
                     LEFT JOIN usuarios uc ON c.UsuarioCreadorId = uc.IdUsuario
                     LEFT JOIN usuarios ut ON c.UsuarioTerminadorId = ut.IdUsuario";

        using (MySqlCommand command = new MySqlCommand(query, connection))
        {
            connection.Open();
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                contratos.Add(new Contrato
                {
                    IdContrato = reader.GetInt32(nameof(Contrato.IdContrato)),
                    InquilinoId = reader.GetInt32(nameof(Contrato.InquilinoId)),
                    InmuebleId = reader.GetInt32(nameof(Contrato.InmuebleId)),
                    FechaInicio = reader.GetDateTime(nameof(Contrato.FechaInicio)),
                    FechaFin = reader.GetDateTime(nameof(Contrato.FechaFin)),
                    Monto = reader.GetDecimal(nameof(Contrato.Monto)),
                    UsuarioCreadorId = reader.IsDBNull(6) ? null : reader.GetInt32(6),
                    UsuarioTerminadorId = reader.IsDBNull(7) ? null : reader.GetInt32(7),
                    FechaCreacion = reader.IsDBNull(8) ? null : reader.GetDateTime(8),
                    FechaTerminacion = reader.IsDBNull(9) ? null : reader.GetDateTime(9),
                    Terminado = reader.GetBoolean(nameof(Contrato.Terminado)),
                    Inquilino = new Inquilinos { id = reader.GetInt32(nameof(Contrato.InquilinoId)), nombre = reader.GetString("InquilinoNombre") },
                    Inmueble = new Inmueble { IdInmueble = reader.GetInt32(nameof(Contrato.InmuebleId)), Direccion = reader.GetString("InmuebleDireccion") },
                    UsuarioCreador = reader.IsDBNull(13) ? null : new Usuario { Nombre = reader.GetString("UsuarioCreadorNombre") },
                    UsuarioTerminador = reader.IsDBNull(14) ? null : new Usuario { Nombre = reader.GetString("UsuarioTerminadorNombre") }
                });
            }
            connection.Close();
        }
        return contratos;
    }
}

    public Contrato? Obtener(int id)
{
    Contrato? res = null;

    using (MySqlConnection connection = new MySqlConnection(connectionString))
    {
        var query = @$"SELECT c.{nameof(Contrato.IdContrato)}, 
                            c.{nameof(Contrato.InquilinoId)}, 
                            c.{nameof(Contrato.InmuebleId)},
                            c.{nameof(Contrato.FechaInicio)}, 
                            c.{nameof(Contrato.FechaFin)}, 
                            c.{nameof(Contrato.Monto)},
                            c.{nameof(Contrato.UsuarioCreadorId)},
                            c.{nameof(Contrato.UsuarioTerminadorId)},
                            c.{nameof(Contrato.FechaCreacion)},
                            c.{nameof(Contrato.FechaTerminacion)},
                            c.{nameof(Contrato.Terminado)},
                            i.nombre AS InquilinoNombre,
                            inm.Direccion AS InmuebleDireccion,
                            uc.Nombre AS UsuarioCreadorNombre,
                            ut.Nombre AS UsuarioTerminadorNombre
                     FROM contratos c
                     INNER JOIN inquilinos i ON c.InquilinoId = i.id
                     INNER JOIN inmuebles inm ON c.InmuebleId = inm.IdInmueble
                     LEFT JOIN usuarios uc ON c.UsuarioCreadorId = uc.IdUsuario
                     LEFT JOIN usuarios ut ON c.UsuarioTerminadorId = ut.IdUsuario
                     WHERE c.{nameof(Contrato.IdContrato)} = @IdContrato";

        using (MySqlCommand command = new MySqlCommand(query, connection))
        {
            command.Parameters.AddWithValue("@IdContrato", id);
            connection.Open();
            var reader = command.ExecuteReader();
            if (reader.Read())
            {
                res = new Contrato
                {
                    IdContrato = reader.GetInt32(nameof(Contrato.IdContrato)),
                    InquilinoId = reader.GetInt32(nameof(Contrato.InquilinoId)),
                    InmuebleId = reader.GetInt32(nameof(Contrato.InmuebleId)),
                    FechaInicio = reader.GetDateTime(nameof(Contrato.FechaInicio)),
                    FechaFin = reader.GetDateTime(nameof(Contrato.FechaFin)),
                    Monto = reader.GetDecimal(nameof(Contrato.Monto)),
                    UsuarioCreadorId = reader.IsDBNull(6) ? null : reader.GetInt32(6),
                    UsuarioTerminadorId = reader.IsDBNull(7) ? null : reader.GetInt32(7),
                    FechaCreacion = reader.IsDBNull(8) ? null : reader.GetDateTime(8),
                    FechaTerminacion = reader.IsDBNull(9) ? null : reader.GetDateTime(9),
                    Terminado = reader.GetBoolean(nameof(Contrato.Terminado)),
                    Inquilino = new Inquilinos { 
                        id = reader.GetInt32(nameof(Contrato.InquilinoId)), 
                        nombre = reader.GetString("InquilinoNombre") 
                    },
                    Inmueble = new Inmueble { 
                        IdInmueble = reader.GetInt32(nameof(Contrato.InmuebleId)), 
                        Direccion = reader.GetString("InmuebleDireccion") 
                    },
                    UsuarioCreador = reader.IsDBNull(13) ? null : new Usuario { 
                        Nombre = reader.GetString("UsuarioCreadorNombre") 
                    },
                    UsuarioTerminador = reader.IsDBNull(14) ? null : new Usuario { 
                        Nombre = reader.GetString("UsuarioTerminadorNombre") 
                    }
                };
            }
            connection.Close();
        }
        return res;
    }
}

        public int Alta(Contrato contrato)
    {
        int res = -1;
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            var query = @$"INSERT INTO contratos 
                            ({nameof(Contrato.InquilinoId)}, 
                            {nameof(Contrato.InmuebleId)}, 
                            {nameof(Contrato.FechaInicio)}, 
                            {nameof(Contrato.FechaFin)}, 
                            {nameof(Contrato.Monto)},
                            {nameof(Contrato.UsuarioCreadorId)},
                            {nameof(Contrato.FechaCreacion)})
                            VALUES (@inquilinoId, @inmuebleId, @fechaInicio, @fechaFin, @monto, @usuarioCreadorId, @fechaCreacion);
                            SELECT LAST_INSERT_ID();";
            
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@inquilinoId", contrato.InquilinoId);
                command.Parameters.AddWithValue("@inmuebleId", contrato.InmuebleId);
                command.Parameters.AddWithValue("@fechaInicio", contrato.FechaInicio);
                command.Parameters.AddWithValue("@fechaFin", contrato.FechaFin);
                command.Parameters.AddWithValue("@monto", contrato.Monto);
                command.Parameters.AddWithValue("@usuarioCreadorId", contrato.UsuarioCreadorId);
                command.Parameters.AddWithValue("@fechaCreacion", DateTime.Now);
                
                connection.Open();
                res = Convert.ToInt32(command.ExecuteScalar());
                connection.Close();
            }
        }
        return res;
    }
    public bool TerminarContrato(int contratoId, int usuarioTerminadorId)
    {
        bool res = false;
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            var query = $@"UPDATE contratos SET 
                            {nameof(Contrato.Terminado)} = 1,
                            {nameof(Contrato.UsuarioTerminadorId)} = @usuarioTerminadorId,
                            {nameof(Contrato.FechaTerminacion)} = @fechaTerminacion
                            WHERE {nameof(Contrato.IdContrato)} = @IdContrato;";
            
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@usuarioTerminadorId", usuarioTerminadorId);
                command.Parameters.AddWithValue("@fechaTerminacion", DateTime.Now);
                command.Parameters.AddWithValue("@IdContrato", contratoId);
                
                connection.Open();
                res = command.ExecuteNonQuery() > 0;
                connection.Close();
            }
        }
        return res;
    }

    public bool Modificar(Contrato contrato)
    {
        bool res = false;
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            var query = $@"UPDATE contratos SET 
                            {nameof(Contrato.InquilinoId)}=@inquilinoId, 
                            {nameof(Contrato.InmuebleId)}=@inmuebleId, 
                            {nameof(Contrato.FechaInicio)}=@fechaInicio, 
                            {nameof(Contrato.FechaFin)}=@fechaFin, 
                            {nameof(Contrato.Monto)}=@monto
                             WHERE {nameof(Contrato.IdContrato)}=@IdContrato;";

            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@inquilinoId", contrato.InquilinoId);
                command.Parameters.AddWithValue("@inmuebleId", contrato.InmuebleId);
                command.Parameters.AddWithValue("@fechaInicio", contrato.FechaInicio);
                command.Parameters.AddWithValue("@fechaFin", contrato.FechaFin);
                command.Parameters.AddWithValue("@monto", contrato.Monto);
                command.Parameters.AddWithValue("@IdContrato", contrato.IdContrato);

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
            var query = $@"DELETE FROM contratos WHERE {nameof(Contrato.IdContrato)} = @IdContrato";

            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@IdContrato", id);
                connection.Open();
                res = command.ExecuteNonQuery();
                connection.Close();
            }
        }
        return res;
    }


    public List<Contrato> ObtenerPorInmueble(int inmuebleId)
    {
        List<Contrato> contratos = new List<Contrato>();

        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            var query = @$"SELECT {nameof(Contrato.IdContrato)}, 
                                {nameof(Contrato.InquilinoId)}, 
                                {nameof(Contrato.InmuebleId)},
                                {nameof(Contrato.FechaInicio)}, 
                                {nameof(Contrato.FechaFin)}, 
                                {nameof(Contrato.Monto)}
                             FROM contratos
                             WHERE {nameof(Contrato.InmuebleId)} = @InmuebleId";

            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@InmuebleId", inmuebleId);
                connection.Open();
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    contratos.Add(new Contrato
                    {
                        IdContrato = reader.GetInt32(nameof(Contrato.IdContrato)),
                        InquilinoId = reader.GetInt32(nameof(Contrato.InquilinoId)),
                        InmuebleId = reader.GetInt32(nameof(Contrato.InmuebleId)),
                        FechaInicio = reader.GetDateTime(nameof(Contrato.FechaInicio)),
                        FechaFin = reader.GetDateTime(nameof(Contrato.FechaFin)),
                        Monto = reader.GetDecimal(nameof(Contrato.Monto))
                    });
                }
                connection.Close();
            }
            return contratos;
        }
    }


    public List<Contrato> ObtenerPorInquilino(int inquilinoId)
    {
        List<Contrato> contratos = new List<Contrato>();

        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            var query = @$"SELECT {nameof(Contrato.IdContrato)}, 
                                {nameof(Contrato.InquilinoId)}, 
                                {nameof(Contrato.InmuebleId)},
                                {nameof(Contrato.FechaInicio)}, 
                                {nameof(Contrato.FechaFin)}, 
                                {nameof(Contrato.Monto)}
                             FROM contratos
                             WHERE {nameof(Contrato.InquilinoId)} = @InquilinoId";

            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@InquilinoId", inquilinoId);
                connection.Open();
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    contratos.Add(new Contrato
                    {
                        IdContrato = reader.GetInt32(nameof(Contrato.IdContrato)),
                        InquilinoId = reader.GetInt32(nameof(Contrato.InquilinoId)),
                        InmuebleId = reader.GetInt32(nameof(Contrato.InmuebleId)),
                        FechaInicio = reader.GetDateTime(nameof(Contrato.FechaInicio)),
                        FechaFin = reader.GetDateTime(nameof(Contrato.FechaFin)),
                        Monto = reader.GetDecimal(nameof(Contrato.Monto))
                    });
                }
                connection.Close();
            }
            return contratos;
        }
    }
    


            public bool ExisteSuperposicionContrato(int inmuebleId, DateTime fechaInicio, DateTime fechaFin, int contratoIdExcluir = 0)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                var query = @"SELECT COUNT(*) FROM contratos 
                            WHERE InmuebleId = @inmuebleId 
                            AND IdContrato != @contratoIdExcluir
                            AND ((FechaInicio BETWEEN @fechaInicio AND @fechaFin) 
                                OR (FechaFin BETWEEN @fechaInicio AND @fechaFin)
                                OR (@fechaInicio BETWEEN FechaInicio AND FechaFin)
                                OR (@fechaFin BETWEEN FechaInicio AND FechaFin))";
                
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@inmuebleId", inmuebleId);
                    command.Parameters.AddWithValue("@fechaInicio", fechaInicio);
                    command.Parameters.AddWithValue("@fechaFin", fechaFin);
                    command.Parameters.AddWithValue("@contratoIdExcluir", contratoIdExcluir);
                    
                    connection.Open();
                    int count = Convert.ToInt32(command.ExecuteScalar());
                    connection.Close();
                    
                    return count > 0;
                }
            }
        }
}