using MySql.Data.MySqlClient;

namespace inmobiliaria_benenatti.Models
{
    public class RepositorioTiposInmueble
    {
        string connectionString = "Server=localhost;User=root;Password=;Database=inmobiliaria;sslmode=none";

        public List<TipoInmueble> ObtenerTiposInmueble()
        {
            List<TipoInmueble> tipos = new List<TipoInmueble>();

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                var query = @"SELECT IdTipoInmueble, Nombre, Descripcion 
                             FROM tiposinmueble 
                             ORDER BY Nombre";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    connection.Open();
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        tipos.Add(new TipoInmueble
                        {
                            IdTipoInmueble = reader.GetInt32("IdTipoInmueble"),
                            Nombre = reader.GetString("Nombre"),
                            Descripcion = reader.IsDBNull(reader.GetOrdinal("Descripcion")) ? null : reader.GetString("Descripcion")
                        });
                    }
                    connection.Close();
                }
            }
            return tipos;
        }

        public TipoInmueble? ObtenerTipoInmueble(int id)
        {
            TipoInmueble? tipo = null;

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                var query = @"SELECT IdTipoInmueble, Nombre, Descripcion 
                             FROM tiposinmueble 
                             WHERE IdTipoInmueble = @Id";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    connection.Open();
                    var reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        tipo = new TipoInmueble
                        {
                            IdTipoInmueble = reader.GetInt32("IdTipoInmueble"),
                            Nombre = reader.GetString("Nombre"),
                            Descripcion = reader.IsDBNull(reader.GetOrdinal("Descripcion")) ? null : reader.GetString("Descripcion")
                        };
                    }
                    connection.Close();
                }
            }
            return tipo;
        }

        public int Alta(TipoInmueble tipo)
        {
            int res = -1;
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                var query = @"INSERT INTO tiposinmueble (Nombre, Descripcion) 
                             VALUES (@Nombre, @Descripcion);
                             SELECT LAST_INSERT_ID();";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Nombre", tipo.Nombre);
                    command.Parameters.AddWithValue("@Descripcion", (object?)tipo.Descripcion ?? DBNull.Value);
                    connection.Open();
                    res = Convert.ToInt32(command.ExecuteScalar());
                    connection.Close();
                }
            }
            return res;
        }

        public bool Modificar(TipoInmueble tipo)
        {
            bool res = false;
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                var query = @"UPDATE tiposinmueble SET 
                             Nombre = @Nombre, 
                             Descripcion = @Descripcion
                             WHERE IdTipoInmueble = @Id";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Nombre", tipo.Nombre);
                    command.Parameters.AddWithValue("@Descripcion", (object?)tipo.Descripcion ?? DBNull.Value);
                    command.Parameters.AddWithValue("@Id", tipo.IdTipoInmueble);
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
                var query = @"DELETE FROM tiposinmueble WHERE IdTipoInmueble = @Id";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    connection.Open();
                    res = command.ExecuteNonQuery();
                    connection.Close();
                }
            }
            return res;
        }

        public List<TipoInmueble> ObtenerListaTiposInmueble()
        {
            var tipos = new List<TipoInmueble>();
            using (var connection = new MySqlConnection(connectionString))
            {
                var query = @"SELECT IdTipoInmueble, Nombre FROM tiposinmueble ORDER BY Nombre";
                using (var command = new MySqlCommand(query, connection))
                {
                    connection.Open();
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        tipos.Add(new TipoInmueble
                        {
                            IdTipoInmueble = reader.GetInt32("IdTipoInmueble"),
                            Nombre = reader.GetString("Nombre")
                        });
                    }
                }
            }
            return tipos;
        }
    }
}