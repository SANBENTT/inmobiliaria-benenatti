using MySql.Data.MySqlClient;

namespace inmobiliaria_benenatti.Models;

public class RepositorioInmuebles
{
    string connectionString = "Server=localhost;User=root;Password=;Database=inmobiliaria;sslmode=none";

    public List<Inmueble> ObtenerInmuebles()
    {
        List<Inmueble> inmuebles = new List<Inmueble>();

        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            var query = @$"SELECT i.{nameof(Inmueble.IdInmueble)}, 
                                i.{nameof(Inmueble.Direccion)}, 
                                i.{nameof(Inmueble.Ambientes)},
                                i.{nameof(Inmueble.Superficie)}, 
                                i.{nameof(Inmueble.Latitud)}, 
                                i.{nameof(Inmueble.Longitud)}, 
                                i.{nameof(Inmueble.PropietarioId)},
                                i.{nameof(Inmueble.TipoInmuebleId)},
                                i.{nameof(Inmueble.Uso)},
                                i.{nameof(Inmueble.Disponible)},
                                p.{nameof(Propietarios.nombre)},
                                t.Nombre AS TipoNombre,
                                t.Descripcion AS TipoDescripcion
                         FROM inmuebles i
                         INNER JOIN propietarios p ON i.{nameof(Inmueble.PropietarioId)} = p.{nameof(Propietarios.id)}
                         LEFT JOIN tiposinmueble t ON i.{nameof(Inmueble.TipoInmuebleId)} = t.IdTipoInmueble";

            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                connection.Open();
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    inmuebles.Add(new Inmueble
                    {
                        IdInmueble = reader.GetInt32(nameof(Inmueble.IdInmueble)),
                        Direccion = reader.GetString(nameof(Inmueble.Direccion)),
                        Ambientes = reader.GetInt32(nameof(Inmueble.Ambientes)),
                        Superficie = reader.GetDecimal(nameof(Inmueble.Superficie)),
                        Latitud = reader.GetDecimal(nameof(Inmueble.Latitud)),
                        Longitud = reader.GetDecimal(nameof(Inmueble.Longitud)),
                        PropietarioId = reader.GetInt32(nameof(Inmueble.PropietarioId)),
                        TipoInmuebleId = reader.GetInt32(nameof(Inmueble.TipoInmuebleId)),
                        Uso = (UsoInmueble)reader.GetInt32(nameof(Inmueble.Uso)),
                        Disponible = reader.GetBoolean(nameof(Inmueble.Disponible)),
                        Propietario = new Propietarios
                        {
                            nombre = reader.GetString(nameof(Propietarios.nombre))
                        },
                        TipoInmueble = new TipoInmueble
                        {
                            IdTipoInmueble = reader.GetInt32(nameof(Inmueble.TipoInmuebleId)),
                            Nombre = reader.GetString("TipoNombre"),
                            Descripcion = reader.IsDBNull(reader.GetOrdinal("TipoDescripcion")) ? null : reader.GetString("TipoDescripcion")
                        }
                    });
                }
                connection.Close();
            }
            return inmuebles;
        }
    }

    public Inmueble? Obtener(int id)
    {
        Inmueble? res = null;

        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            var query = @$"SELECT i.{nameof(Inmueble.IdInmueble)}, 
                                i.{nameof(Inmueble.Direccion)}, 
                                i.{nameof(Inmueble.Ambientes)},
                                i.{nameof(Inmueble.Superficie)}, 
                                i.{nameof(Inmueble.Latitud)}, 
                                i.{nameof(Inmueble.Longitud)}, 
                                i.{nameof(Inmueble.PropietarioId)},
                                i.{nameof(Inmueble.TipoInmuebleId)},
                                i.{nameof(Inmueble.Uso)},
                                i.{nameof(Inmueble.Disponible)},
                                p.{nameof(Propietarios.nombre)},
                                t.Nombre AS TipoNombre,
                                t.Descripcion AS TipoDescripcion
                         FROM inmuebles i
                         INNER JOIN propietarios p ON i.{nameof(Inmueble.PropietarioId)} = p.{nameof(Propietarios.id)}
                         LEFT JOIN tiposinmueble t ON i.{nameof(Inmueble.TipoInmuebleId)} = t.IdTipoInmueble
                         WHERE i.{nameof(Inmueble.IdInmueble)} = @IdInmueble";

            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@IdInmueble", id);
                connection.Open();
                var reader = command.ExecuteReader();
                if (reader.Read())
                {
                    res = new Inmueble
                    {
                        IdInmueble = reader.GetInt32(nameof(Inmueble.IdInmueble)),
                        Direccion = reader.GetString(nameof(Inmueble.Direccion)),
                        Ambientes = reader.GetInt32(nameof(Inmueble.Ambientes)),
                        Superficie = reader.GetDecimal(nameof(Inmueble.Superficie)),
                        Latitud = reader.GetDecimal(nameof(Inmueble.Latitud)),
                        Longitud = reader.GetDecimal(nameof(Inmueble.Longitud)),
                        PropietarioId = reader.GetInt32(nameof(Inmueble.PropietarioId)),
                        TipoInmuebleId = reader.GetInt32(nameof(Inmueble.TipoInmuebleId)),
                        Uso = (UsoInmueble)reader.GetInt32(nameof(Inmueble.Uso)),
                        Disponible = reader.GetBoolean(nameof(Inmueble.Disponible)),
                        Propietario = new Propietarios
                        {
                            nombre = reader.GetString(nameof(Propietarios.nombre))
                        },
                        TipoInmueble = new TipoInmueble
                        {
                            IdTipoInmueble = reader.GetInt32(nameof(Inmueble.TipoInmuebleId)),
                            Nombre = reader.GetString("TipoNombre"),
                            Descripcion = reader.IsDBNull(reader.GetOrdinal("TipoDescripcion")) ? null : reader.GetString("TipoDescripcion")
                        }
                    };
                }
                connection.Close();
            }
            return res;
        }
    }

    public int Alta(Inmueble inmueble)
    {
        int res = -1;
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            var query = @$"INSERT INTO inmuebles 
                        ({nameof(Inmueble.Direccion)}, 
                            {nameof(Inmueble.Ambientes)}, 
                            {nameof(Inmueble.Superficie)}, 
                            {nameof(Inmueble.Latitud)}, 
                            {nameof(Inmueble.Longitud)}, 
                            {nameof(Inmueble.PropietarioId)},
                            {nameof(Inmueble.TipoInmuebleId)},
                            {nameof(Inmueble.Uso)},
                            {nameof(Inmueble.Disponible)},
                            {nameof(Inmueble.Foto)}) 
                        VALUES (@direccion, @ambientes, @superficie, @latitud, @longitud, @propietarioId, @tipoInmuebleId, @uso, @disponible, @foto);
                        SELECT LAST_INSERT_ID();";


            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@direccion", inmueble.Direccion);
                command.Parameters.AddWithValue("@ambientes", inmueble.Ambientes);
                command.Parameters.AddWithValue("@superficie", inmueble.Superficie);
                command.Parameters.AddWithValue("@latitud", inmueble.Latitud);
                command.Parameters.AddWithValue("@longitud", inmueble.Longitud);
                command.Parameters.AddWithValue("@propietarioId", inmueble.PropietarioId);
                command.Parameters.AddWithValue("@tipoInmuebleId", inmueble.TipoInmuebleId);
                command.Parameters.AddWithValue("@uso", (int)inmueble.Uso);
                command.Parameters.AddWithValue("@disponible", inmueble.Disponible);
                command.Parameters.AddWithValue("@foto", inmueble.Foto ?? (object)DBNull.Value);
                connection.Open();
                res = Convert.ToInt32(command.ExecuteScalar());
                connection.Close();
            }
        }
        return res;
    }

    public bool Modificar(Inmueble inmueble)
    {
        bool res = false;
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            var query = $@"UPDATE inmuebles SET 
                        {nameof(Inmueble.Direccion)}=@direccion, 
                        {nameof(Inmueble.Ambientes)}=@ambientes, 
                        {nameof(Inmueble.Superficie)}=@superficie, 
                        {nameof(Inmueble.Latitud)}=@latitud, 
                        {nameof(Inmueble.Longitud)}=@longitud, 
                        {nameof(Inmueble.PropietarioId)}=@propietarioId,
                        {nameof(Inmueble.TipoInmuebleId)}=@tipoInmuebleId,
                        {nameof(Inmueble.Uso)}=@uso,
                        {nameof(Inmueble.Disponible)}=@disponible
                         WHERE {nameof(Inmueble.IdInmueble)}=@IdInmueble;";

            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@direccion", inmueble.Direccion);
                command.Parameters.AddWithValue("@ambientes", inmueble.Ambientes);
                command.Parameters.AddWithValue("@superficie", inmueble.Superficie);
                command.Parameters.AddWithValue("@latitud", inmueble.Latitud);
                command.Parameters.AddWithValue("@longitud", inmueble.Longitud);
                command.Parameters.AddWithValue("@propietarioId", inmueble.PropietarioId);
                command.Parameters.AddWithValue("@tipoInmuebleId", inmueble.TipoInmuebleId);
                command.Parameters.AddWithValue("@uso", (int)inmueble.Uso);
                command.Parameters.AddWithValue("@disponible", inmueble.Disponible);
                command.Parameters.AddWithValue("@IdInmueble", inmueble.IdInmueble);
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
            var query = $@"DELETE FROM inmuebles WHERE {nameof(Inmueble.IdInmueble)} = @IdInmueble";
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@IdInmueble", id);
                connection.Open();
                res = command.ExecuteNonQuery();
                connection.Close();
            }
        }
        return res;
    }


    public List<Inmueble> ObtenerListaInmuebles()
    {
        var inmuebles = new List<Inmueble>();
        using (var connection = new MySqlConnection(connectionString))
        {
            var query = @$"SELECT {nameof(Inmueble.IdInmueble)}, {nameof(Inmueble.Direccion)}
                            FROM Inmuebles";
            using (var command = new MySqlCommand(query, connection))
            {
                connection.Open();
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    inmuebles.Add(new Inmueble
                    {
                        IdInmueble = reader.GetInt32(nameof(Inmueble.IdInmueble)),
                        Direccion = reader.GetString(nameof(Inmueble.Direccion))
                    });
                }
            }
        }
        return inmuebles;
    }

    public List<Inmueble> ObtenerInmueblesDisponibles()
    {
        List<Inmueble> inmuebles = new List<Inmueble>();

        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            var query = @$"SELECT i.{nameof(Inmueble.IdInmueble)}, 
                                i.{nameof(Inmueble.Direccion)}, 
                                i.{nameof(Inmueble.Ambientes)},
                                i.{nameof(Inmueble.Superficie)}, 
                                i.{nameof(Inmueble.Latitud)}, 
                                i.{nameof(Inmueble.Longitud)}, 
                                i.{nameof(Inmueble.PropietarioId)},
                                i.{nameof(Inmueble.TipoInmuebleId)},
                                i.{nameof(Inmueble.Uso)},
                                i.{nameof(Inmueble.Disponible)},
                                p.{nameof(Propietarios.nombre)},
                                t.Nombre AS TipoNombre,
                                t.Descripcion AS TipoDescripcion
                         FROM inmuebles i
                         INNER JOIN propietarios p ON i.{nameof(Inmueble.PropietarioId)} = p.{nameof(Propietarios.id)}
                         LEFT JOIN tiposinmueble t ON i.{nameof(Inmueble.TipoInmuebleId)} = t.IdTipoInmueble
                         WHERE i.{nameof(Inmueble.Disponible)} = 1";

            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                connection.Open();
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    inmuebles.Add(new Inmueble
                    {
                        IdInmueble = reader.GetInt32(nameof(Inmueble.IdInmueble)),
                        Direccion = reader.GetString(nameof(Inmueble.Direccion)),
                        Ambientes = reader.GetInt32(nameof(Inmueble.Ambientes)),
                        Superficie = reader.GetDecimal(nameof(Inmueble.Superficie)),
                        Latitud = reader.GetDecimal(nameof(Inmueble.Latitud)),
                        Longitud = reader.GetDecimal(nameof(Inmueble.Longitud)),
                        PropietarioId = reader.GetInt32(nameof(Inmueble.PropietarioId)),
                        TipoInmuebleId = reader.GetInt32(nameof(Inmueble.TipoInmuebleId)),
                        Uso = (UsoInmueble)reader.GetInt32(nameof(Inmueble.Uso)),
                        Disponible = reader.GetBoolean(nameof(Inmueble.Disponible)),
                        Propietario = new Propietarios
                        {
                            nombre = reader.GetString(nameof(Propietarios.nombre))
                        },
                        TipoInmueble = new TipoInmueble
                        {
                            IdTipoInmueble = reader.GetInt32(nameof(Inmueble.TipoInmuebleId)),
                            Nombre = reader.GetString("TipoNombre"),
                            Descripcion = reader.IsDBNull(reader.GetOrdinal("TipoDescripcion")) ? null : reader.GetString("TipoDescripcion")
                        }
                    });
                }
                connection.Close();
            }
            return inmuebles;
        }
    }



    public List<Inmueble> ObtenerInmueblesDisponiblesPorFecha(DateTime fechaInicio, DateTime fechaFin)
    {
        List<Inmueble> inmuebles = new List<Inmueble>();

        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            var query = @$"SELECT DISTINCT i.{nameof(Inmueble.IdInmueble)}, 
                                i.{nameof(Inmueble.Direccion)}, 
                                i.{nameof(Inmueble.Ambientes)},
                                i.{nameof(Inmueble.Superficie)}, 
                                i.{nameof(Inmueble.Latitud)}, 
                                i.{nameof(Inmueble.Longitud)}, 
                                i.{nameof(Inmueble.PropietarioId)},
                                i.{nameof(Inmueble.TipoInmuebleId)},
                                i.{nameof(Inmueble.Uso)},
                                i.{nameof(Inmueble.Disponible)},
                                p.{nameof(Propietarios.nombre)},
                                t.Nombre AS TipoNombre,
                                t.Descripcion AS TipoDescripcion
                         FROM inmuebles i
                         INNER JOIN propietarios p ON i.{nameof(Inmueble.PropietarioId)} = p.{nameof(Propietarios.id)}
                         LEFT JOIN tiposinmueble t ON i.{nameof(Inmueble.TipoInmuebleId)} = t.IdTipoInmueble
                         WHERE i.{nameof(Inmueble.Disponible)} = 1
                         AND i.{nameof(Inmueble.IdInmueble)} NOT IN (
                             SELECT c.{nameof(Contrato.InmuebleId)}
                             FROM contratos c
                             WHERE (c.{nameof(Contrato.FechaInicio)} <= @fechaFin 
                                 AND c.{nameof(Contrato.FechaFin)} >= @fechaInicio)
                         )";

            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@fechaInicio", fechaInicio);
                command.Parameters.AddWithValue("@fechaFin", fechaFin);

                connection.Open();
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    inmuebles.Add(new Inmueble
                    {
                        IdInmueble = reader.GetInt32(nameof(Inmueble.IdInmueble)),
                        Direccion = reader.GetString(nameof(Inmueble.Direccion)),
                        Ambientes = reader.GetInt32(nameof(Inmueble.Ambientes)),
                        Superficie = reader.GetDecimal(nameof(Inmueble.Superficie)),
                        Latitud = reader.GetDecimal(nameof(Inmueble.Latitud)),
                        Longitud = reader.GetDecimal(nameof(Inmueble.Longitud)),
                        PropietarioId = reader.GetInt32(nameof(Inmueble.PropietarioId)),
                        TipoInmuebleId = reader.GetInt32(nameof(Inmueble.TipoInmuebleId)),
                        Uso = (UsoInmueble)reader.GetInt32(nameof(Inmueble.Uso)),
                        Disponible = reader.GetBoolean(nameof(Inmueble.Disponible)),
                        Propietario = new Propietarios
                        {
                            nombre = reader.GetString(nameof(Propietarios.nombre))
                        },
                        TipoInmueble = new TipoInmueble
                        {
                            IdTipoInmueble = reader.GetInt32(nameof(Inmueble.TipoInmuebleId)),
                            Nombre = reader.GetString("TipoNombre"),
                            Descripcion = reader.IsDBNull(reader.GetOrdinal("TipoDescripcion")) ? null : reader.GetString("TipoDescripcion")
                        }
                    });
                }
                connection.Close();
            }
            return inmuebles;
        }
    }

    public bool TieneContratosAsociados(int inmuebleId)
    {
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            var query = @"SELECT COUNT(*) FROM contratos WHERE InmuebleId = @inmuebleId";

            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@inmuebleId", inmuebleId);
                connection.Open();
                int count = Convert.ToInt32(command.ExecuteScalar());
                connection.Close();

                return count > 0;
            }
        }
    }


    public int CantidadContratosAsociados(int inmuebleId)
    {
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            var query = @"SELECT COUNT(*) FROM contratos WHERE InmuebleId = @inmuebleId";

            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@inmuebleId", inmuebleId);
                connection.Open();
                int count = Convert.ToInt32(command.ExecuteScalar());
                connection.Close();

                return count;
            }
        }
    }



    
}
