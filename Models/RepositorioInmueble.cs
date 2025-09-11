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
            var query = @$"SELECT {nameof(Inmueble.IdInmueble)}, 
                                {nameof(Inmueble.Direccion)}, 
                                {nameof(Inmueble.Ambientes)},
                                {nameof(Inmueble.Superficie)}, 
                                {nameof(Inmueble.Latitud)}, 
                                {nameof(Inmueble.Longitud)}, 
                                {nameof(Inmueble.PropietarioId)}
                             FROM inmuebles";

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
            var query = @$"SELECT {nameof(Inmueble.IdInmueble)}, 
                                {nameof(Inmueble.Direccion)}, 
                                {nameof(Inmueble.Ambientes)},
                                {nameof(Inmueble.Superficie)}, 
                                {nameof(Inmueble.Latitud)}, 
                                {nameof(Inmueble.Longitud)}, 
                                {nameof(Inmueble.PropietarioId)}
                             FROM inmuebles WHERE {nameof(Inmueble.IdInmueble)} = @IdInmueble";
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
                     };
                }
                connection.Close();
            }
            return res;
        }
    }

    public int Alta(Inmueble inmuebles)
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
                              {nameof(Inmueble.PropietarioId)}) 
                             VALUES (@direccion, @ambientes, @superficie, @latitud, @longitud, @propietarioId);
                             SELECT LAST_INSERT_ID();";
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@direccion", inmuebles.Direccion);
                    command.Parameters.AddWithValue("@ambientes", inmuebles.Ambientes);
                    command.Parameters.AddWithValue("@superficie", inmuebles.Superficie);
                    command.Parameters.AddWithValue("@latitud", inmuebles.Latitud);
                    command.Parameters.AddWithValue("@longitud", inmuebles.Longitud);
                    command.Parameters.AddWithValue("@propietarioId", inmuebles.PropietarioId);
                connection.Open();
                res = Convert.ToInt32(command.ExecuteScalar());
                connection.Close();
            }


        }
        return res;
    }

    public bool Modificar(Inmueble inmuebles)
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
                            {nameof(Inmueble.PropietarioId)}=@propietarioId
                             WHERE {nameof(Inmueble.IdInmueble)}=@IdInmueble;";
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@direccion", inmuebles.Direccion);
                command.Parameters.AddWithValue("@ambientes", inmuebles.Ambientes);
                command.Parameters.AddWithValue("@superficie", inmuebles.Superficie);
                command.Parameters.AddWithValue("@latitud", inmuebles.Latitud);
                command.Parameters.AddWithValue("@longitud", inmuebles.Longitud);
                command.Parameters.AddWithValue("@propietarioId", inmuebles.PropietarioId);
                command.Parameters.AddWithValue("@IdInmueble", inmuebles.IdInmueble);
                connection.Open();
                res = command.ExecuteNonQuery() > 0;
                connection.Close();
            }
        }
        return res;
    }
    
    public int Baja (int id)
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
}
