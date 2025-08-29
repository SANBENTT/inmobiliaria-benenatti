using MySql.Data.MySqlClient;

namespace inmobiliaria_benenatti.Models;

public class RepositorioInquilinos
{
    string connectionString = "Server=localhost;User=root;Password=;Database=inmobiliaria;sslmode=none";
    public List<Inquilinos> ObtenerInquilinos()
    {
        List<Inquilinos> inquilinos = new List<Inquilinos>();

        using (MySqlConnection connection = new MySqlConnection(connectionString))

        {
            var query = $@"SELECT {nameof(Inquilinos.id)}, 
                                  {nameof(Inquilinos.dni)}, 
                                  {nameof(Inquilinos.nombre)}, 
                                  {nameof(Inquilinos.telefono)}, 
                                  {nameof(Inquilinos.email)}, 
                                  {nameof(Inquilinos.direccion)} 
                           FROM inquilinos";
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                connection.Open();
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    inquilinos.Add(new Inquilinos
                    {
                        id = reader.GetInt32(nameof(Inquilinos.id)),
                        dni = reader.GetString(nameof(Inquilinos.dni)),
                        nombre = reader.GetString(nameof(Inquilinos.nombre)),
                        telefono = reader.GetString(nameof(Inquilinos.telefono)),
                        email = reader.GetString(nameof(Inquilinos.email)),
                        direccion = reader.GetString(nameof(Inquilinos.direccion))
                    });
                }
                connection.Close();
            }
            return inquilinos;
        }



    }

    public Inquilinos? Obtener(int id)
    {
        Inquilinos? res = null;

        using (MySqlConnection connection = new MySqlConnection(connectionString))

        {
            var query = $@"SELECT {nameof(Inquilinos.id)}, 
                                  {nameof(Inquilinos.dni)}, 
                                  {nameof(Inquilinos.nombre)}, 
                                  {nameof(Inquilinos.telefono)}, 
                                  {nameof(Inquilinos.email)}, 
                                  {nameof(Inquilinos.direccion)} 
                           FROM inquilinos WHERE {nameof(Inquilinos.id)} = @id";
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@id", id);
                connection.Open();
                var reader = command.ExecuteReader();
                if (reader.Read())
                {
                    res = new Inquilinos
                    {
                        id = reader.GetInt32(nameof(Inquilinos.id)),
                        dni = reader.GetString(nameof(Inquilinos.dni)),
                        nombre = reader.GetString(nameof(Inquilinos.nombre)),
                        telefono = reader.GetString(nameof(Inquilinos.telefono)),
                        email = reader.GetString(nameof(Inquilinos.email)),
                        direccion = reader.GetString(nameof(Inquilinos.direccion))
                    };
                }
                connection.Close();
            }
            return res;
        }
    }

    public int Alta(Inquilinos inquilinos)
    {
        int res = -1;
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            var query = $@"INSERT INTO inquilinos 
                          ({nameof(Inquilinos.dni)}, 
                           {nameof(Inquilinos.nombre)}, 
                           {nameof(Inquilinos.telefono)}, 
                           {nameof(Inquilinos.email)}, 
                           {nameof(Inquilinos.direccion)}) 
                           VALUES 
                          (@dni, @nombre, @telefono, @email, @direccion);
                          SELECT LAST_INSERT_ID();";
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@dni", inquilinos.dni);
                command.Parameters.AddWithValue("@nombre", inquilinos.nombre);
                command.Parameters.AddWithValue("@telefono", inquilinos.telefono);
                command.Parameters.AddWithValue("@email", inquilinos.email);
                command.Parameters.AddWithValue("@direccion", inquilinos.direccion);
                connection.Open();
                res = Convert.ToInt32(command.ExecuteScalar());
                connection.Close();
            }


        }
        return res;
    }

    public bool Modificar(Inquilinos inquilinos)
    {
        bool res = false;
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            var query = $@"UPDATE inquilinos SET 
                           {nameof(Inquilinos.dni)} = @dni, 
                           {nameof(Inquilinos.nombre)} = @nombre, 
                           {nameof(Inquilinos.telefono)} = @telefono, 
                           {nameof(Inquilinos.email)} = @email, 
                           {nameof(Inquilinos.direccion)} = @direccion
                           WHERE {nameof(Inquilinos.id)} = @id";
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@id", inquilinos.id);
                command.Parameters.AddWithValue("@dni", inquilinos.dni);
                command.Parameters.AddWithValue("@nombre", inquilinos.nombre);
                command.Parameters.AddWithValue("@telefono", inquilinos.telefono);
                command.Parameters.AddWithValue("@email", inquilinos.email);
                command.Parameters.AddWithValue("@direccion", inquilinos.direccion);
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
            var query = $@"DELETE FROM inquilinos WHERE {nameof(Inquilinos.id)} = @id";
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@id", id);
                connection.Open();
                res = command.ExecuteNonQuery();
                connection.Close();
            }
        }
        return res;
    }
}
