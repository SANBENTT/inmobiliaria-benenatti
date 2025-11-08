using MySql.Data.MySqlClient;

namespace inmobiliaria_benenatti.Models;

public class RepositorioPropietarios
{
    string connectionString = "Server=localhost;User=root;Password=;Database=inmobiliaria;sslmode=none";
    public List<Propietarios> obtenerPropietarios()
    {
        List<Propietarios> propietarios = new List<Propietarios>();

        using (MySqlConnection connection = new MySqlConnection(connectionString))

        {
            var query = $@"SELECT {nameof(Propietarios.id)}, 
                                  {nameof(Propietarios.dni)}, 
                                  {nameof(Propietarios.nombre)}, 
                                  {nameof(Propietarios.telefono)}, 
                                  {nameof(Propietarios.email)}, 
                                  {nameof(Propietarios.direccion)} 
                           FROM propietarios";
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                connection.Open();
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    propietarios.Add(new Propietarios
                    {
                        id = reader.GetInt32(nameof(Propietarios.id)),
                        dni = reader.GetString(nameof(Propietarios.dni)),
                        nombre = reader.GetString(nameof(Propietarios.nombre)),
                        telefono = reader.GetString(nameof(Propietarios.telefono)),
                        email = reader.GetString(nameof(Propietarios.email)),
                        direccion = reader.GetString(nameof(Propietarios.direccion))
                    });
                }
                connection.Close();
            }
            return propietarios;
        }



    }
    public Propietarios? Obtener(int id)
    {
        Propietarios? res = null;

        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            var query = $@"
            SELECT 
                {nameof(Propietarios.id)}, 
                {nameof(Propietarios.dni)}, 
                {nameof(Propietarios.nombre)}, 
                {nameof(Propietarios.telefono)}, 
                {nameof(Propietarios.email)}, 
                {nameof(Propietarios.direccion)}, 
                {nameof(Propietarios.clave)}  
            FROM propietarios 
            WHERE {nameof(Propietarios.id)} = @id";

            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@id", id);
                connection.Open();
                var reader = command.ExecuteReader();

                if (reader.Read())
                {
                    res = new Propietarios
                    {
                        id = reader.GetInt32(nameof(Propietarios.id)),
                        dni = reader.GetString(nameof(Propietarios.dni)),
                        nombre = reader.GetString(nameof(Propietarios.nombre)),
                        telefono = reader.GetString(nameof(Propietarios.telefono)),
                        email = reader.GetString(nameof(Propietarios.email)),
                        direccion = reader.GetString(nameof(Propietarios.direccion)),
                        clave = reader.GetString(nameof(Propietarios.clave)) 
                    };
                }

                connection.Close();
            }
            return res;
        }
    }


    public int Alta(Propietarios propietarios)
    {
        int res = -1;
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            var query = $@"INSERT INTO propietarios 
                          ({nameof(Propietarios.dni)}, 
                           {nameof(Propietarios.nombre)}, 
                           {nameof(Propietarios.telefono)}, 
                           {nameof(Propietarios.email)}, 
                           {nameof(Propietarios.direccion)}) 
                           VALUES 
                          (@dni, @nombre, @telefono, @email, @direccion);
                          SELECT LAST_INSERT_ID();";
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@dni", propietarios.dni);
                command.Parameters.AddWithValue("@nombre", propietarios.nombre);
                command.Parameters.AddWithValue("@telefono", propietarios.telefono);
                command.Parameters.AddWithValue("@email", propietarios.email);
                command.Parameters.AddWithValue("@direccion", propietarios.direccion);
                connection.Open();
                res = Convert.ToInt32(command.ExecuteScalar());
                connection.Close();
            }


        }
        return res;
    }

    public bool Modificar(Propietarios propietarios)
    {
        bool res = false;
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            var query = $@"
            UPDATE propietarios SET 
                {nameof(Propietarios.dni)} = @dni, 
                {nameof(Propietarios.nombre)} = @nombre, 
                {nameof(Propietarios.telefono)} = @telefono, 
                {nameof(Propietarios.email)} = @email, 
                {nameof(Propietarios.direccion)} = @direccion,
                {nameof(Propietarios.clave)} = @clave 
            WHERE {nameof(Propietarios.id)} = @id";

            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@id", propietarios.id);
                command.Parameters.AddWithValue("@dni", propietarios.dni);
                command.Parameters.AddWithValue("@nombre", propietarios.nombre);
                command.Parameters.AddWithValue("@telefono", propietarios.telefono);
                command.Parameters.AddWithValue("@email", propietarios.email);
                command.Parameters.AddWithValue("@direccion", propietarios.direccion);
                command.Parameters.AddWithValue("@clave", propietarios.clave); 

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
            var query = $@"DELETE FROM propietarios WHERE {nameof(Propietarios.id)} = @id";
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

    public bool ExisteEmail(string email, int? id = null)
    {
        bool existe = false;
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            var query = $@"SELECT COUNT(*) FROM propietarios 
                       WHERE {nameof(Propietarios.email)} = @mail
                       {(id.HasValue ? "AND id <> @id" : "")}";
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@mail", email);
                if (id.HasValue)
                    command.Parameters.AddWithValue("@id", id.Value);

                connection.Open();
                existe = Convert.ToInt32(command.ExecuteScalar()) > 0;
                connection.Close();
            }
        }
        return existe;
    }

    public bool ExisteDni(string dni, int id = 0)
    {
        bool existe = false;
        using (var connection = new MySqlConnection(connectionString))
        {
            var query = @"SELECT COUNT(*) 
                            FROM propietarios 
                            WHERE dni = @dni AND id != @id";

            using (var command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@dni", dni);
                command.Parameters.AddWithValue("@id", id);
                connection.Open();
                var result = Convert.ToInt32(command.ExecuteScalar());
                existe = result > 0;
            }
        }
        return existe;
    }

    public List<Propietarios> ObtenerListaPropietarios()
    {
        List<Propietarios> propietarios = new List<Propietarios>();

        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            var query = @$"SELECT {nameof(Propietarios.id)}, 
                              {nameof(Propietarios.nombre)} 
                       FROM propietarios";

            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                connection.Open();
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    propietarios.Add(new Propietarios
                    {
                        id = reader.GetInt32(nameof(Propietarios.id)),
                        nombre = reader.GetString(nameof(Propietarios.nombre))
                    });
                }
                connection.Close();
            }
        }

        return propietarios;
    }


    public Propietarios? ObtenerPorEmail(string email)
    {
        Propietarios? propietario = null;

        using (var connection = new MySqlConnection(connectionString))
        {
            var query = $@"SELECT {nameof(Propietarios.id)},
                              {nameof(Propietarios.dni)},
                              {nameof(Propietarios.nombre)},
                              {nameof(Propietarios.telefono)},
                              {nameof(Propietarios.email)},
                              {nameof(Propietarios.direccion)},
                              {nameof(Propietarios.clave)}
                       FROM propietarios
                       WHERE {nameof(Propietarios.email)} = @email";

            using (var command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@email", email);
                connection.Open();
                var reader = command.ExecuteReader();

                if (reader.Read())
                {
                    propietario = new Propietarios
                    {
                        id = reader.GetInt32(nameof(Propietarios.id)),
                        dni = reader.GetString(nameof(Propietarios.dni)),
                        nombre = reader.GetString(nameof(Propietarios.nombre)),
                        telefono = reader.GetString(nameof(Propietarios.telefono)),
                        email = reader.GetString(nameof(Propietarios.email)),
                        direccion = reader.GetString(nameof(Propietarios.direccion)),
                        clave = reader.GetString(nameof(Propietarios.clave))
                    };
                }

                connection.Close();
            }
        }

        return propietario;
    }



}