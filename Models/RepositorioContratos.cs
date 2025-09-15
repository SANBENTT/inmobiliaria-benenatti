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
                                i.nombre AS InquilinoNombre,
                              inm.Direccion AS InmuebleDireccion
                       FROM contratos c
                       INNER JOIN inquilinos i ON c.InquilinoId = i.id
                       INNER JOIN inmuebles inm ON c.InmuebleId = inm.IdInmueble";

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
                    Inquilino = new Inquilinos { id = reader.GetInt32(nameof(Contrato.InquilinoId)), nombre = reader.GetString("InquilinoNombre") },
                    Inmueble = new Inmueble { IdInmueble = reader.GetInt32(nameof(Contrato.InmuebleId)), Direccion = reader.GetString("InmuebleDireccion") }
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
                                i.nombre AS InquilinoNombre,
                              inm.Direccion AS InmuebleDireccion
                       FROM contratos c
                       INNER JOIN inquilinos i ON c.InquilinoId = i.id
                       INNER JOIN inmuebles inm ON c.InmuebleId = inm.IdInmueble
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
                    Inquilino = new Inquilinos { id = reader.GetInt32(nameof(Contrato.InquilinoId)), nombre = reader.GetString("InquilinoNombre") },
                    Inmueble = new Inmueble { IdInmueble = reader.GetInt32(nameof(Contrato.InmuebleId)), Direccion = reader.GetString("InmuebleDireccion") }
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
                              {nameof(Contrato.Monto)}) 
                             VALUES (@inquilinoId, @inmuebleId, @fechaInicio, @fechaFin, @monto);
                             SELECT LAST_INSERT_ID();";
            
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@inquilinoId", contrato.InquilinoId);
                command.Parameters.AddWithValue("@inmuebleId", contrato.InmuebleId);
                command.Parameters.AddWithValue("@fechaInicio", contrato.FechaInicio);
                command.Parameters.AddWithValue("@fechaFin", contrato.FechaFin);
                command.Parameters.AddWithValue("@monto", contrato.Monto);
                
                connection.Open();
                res = Convert.ToInt32(command.ExecuteScalar());
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
}