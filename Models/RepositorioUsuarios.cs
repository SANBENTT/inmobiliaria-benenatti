using MySql.Data.MySqlClient;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace inmobiliaria_benenatti.Models
{
    public class RepositorioUsuarios
    {
        private readonly string connectionString;
        private readonly IConfiguration configuration;
        public RepositorioUsuarios(IConfiguration configuration)
        {
            this.configuration = configuration;
            connectionString = configuration.GetConnectionString("MySql")
                              ?? "server=localhost;database=inmobiliaria;user=root;password=;";
        }



        public Usuario? ObtenerPorId(int id)
        {
            Usuario? usuario = null;
            using (var connection = new MySqlConnection(connectionString))
            {
                var sql = "SELECT * FROM usuarios WHERE IdUsuario = @id";
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            usuario = new Usuario
                            {
                                IdUsuario = reader.GetInt32("IdUsuario"),
                                Email = reader.GetString("Email"),
                                Clave = reader.GetString("Clave"),
                                Nombre = reader.GetString("Nombre"),
                                Rol = reader.GetInt32("Rol"),
                                Avatar = reader["Avatar"] == DBNull.Value ? null : reader.GetString("Avatar"),
                                CreadoEn = reader.GetDateTime("CreadoEn")
                            };
                        }
                    }
                }
            }
            return usuario;
        }
        public Usuario? ObtenerPorEmail(string email)
        {
            Usuario? usuario = null;
            using (var connection = new MySqlConnection(connectionString))
            {
                var sql = "SELECT * FROM usuarios WHERE Email = @email";
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@email", email);
                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            usuario = new Usuario
                            {
                                IdUsuario = reader.GetInt32("IdUsuario"),
                                Email = reader.GetString("Email"),
                                Clave = reader.GetString("Clave"),
                                Nombre = reader.GetString("Nombre"),
                                Rol = reader.GetInt32("Rol"),
                                Avatar = reader["Avatar"] == DBNull.Value ? null : reader.GetString("Avatar"),
                                CreadoEn = reader.GetDateTime("CreadoEn")
                            };
                        }
                    }
                }
            }
            return usuario;
        }

        public int Alta(Usuario usuario)
        {
            string hashedPassword = HashPassword(usuario.Clave);
            usuario.Clave = hashedPassword;

            int res = -1;
            using (var connection = new MySqlConnection(connectionString))
            {
                var sql = @"INSERT INTO usuarios (Email, Clave, Nombre, Rol, Avatar) 
                            VALUES (@Email, @Clave, @Nombre, @Rol, @Avatar);
                            SELECT LAST_INSERT_ID();";
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@Email", usuario.Email);
                    command.Parameters.AddWithValue("@Clave", usuario.Clave);
                    command.Parameters.AddWithValue("@Nombre", usuario.Nombre);
                    command.Parameters.AddWithValue("@Rol", usuario.Rol);
                    command.Parameters.AddWithValue("@Avatar", (object?)usuario.Avatar ?? DBNull.Value);
                    connection.Open();
                    res = Convert.ToInt32(command.ExecuteScalar());
                    usuario.IdUsuario = res;
                }
            }
            return res;
        }

        private string HashPassword(string password)
        {
            var saltValue = configuration["Salt"];
            return Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: System.Text.Encoding.ASCII.GetBytes(saltValue),
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 5000,
                numBytesRequested: 256 / 8));
        }

        public int ActualizarClave(int idUsuario, string nuevaClave)
        {
            string hashedPassword = HashPassword(nuevaClave);

            using (var connection = new MySqlConnection(connectionString))
            {
                var sql = "UPDATE usuarios SET Clave = @Clave WHERE IdUsuario = @IdUsuario";
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@Clave", hashedPassword);
                    command.Parameters.AddWithValue("@IdUsuario", idUsuario);
                    connection.Open();
                    return command.ExecuteNonQuery();
                }
            }
        }
        public List<Usuario> ObtenerTodos()
        {
            var usuarios = new List<Usuario>();
            using (var connection = new MySqlConnection(connectionString))
            {
                var sql = "SELECT * FROM usuarios";
                using (var command = new MySqlCommand(sql, connection))
                {
                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var usuario = new Usuario
                            {
                                IdUsuario = reader.GetInt32("IdUsuario"),
                                Email = reader.GetString("Email"),
                                Clave = "***", 
                                Nombre = reader.GetString("Nombre"),
                                Rol = reader.GetInt32("Rol"),
                                CreadoEn = reader.GetDateTime("CreadoEn")
                            };
                            if (reader["Avatar"] != DBNull.Value)
                            {
                                usuario.Avatar = reader.GetString("Avatar");
                            }

                            usuarios.Add(usuario);
                        }
                    }
                }
            }
            return usuarios;
        }


        public int Baja(int id)
        {
            int res = -1;
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                var query = "DELETE FROM Usuarios WHERE IdUsuario = @IdUsuario";
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@IdUsuario", id);
                    connection.Open();
                    res = command.ExecuteNonQuery();
                }
            }
            return res;
        }


        public int ActualizarUsuario(Usuario usuario, bool cambiarClave = false)
        {
            int res = -1;
            using (var connection = new MySqlConnection(connectionString))
            {
                var sql = @"UPDATE usuarios SET 
                        Email = @Email, 
                        Nombre = @Nombre, 
                        Rol = @Rol, 
                        Avatar = @Avatar ";

                if (cambiarClave)
                    sql += ", Clave = @Clave ";

                sql += "WHERE IdUsuario = @IdUsuario";

                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@Email", usuario.Email);
                    command.Parameters.AddWithValue("@Nombre", usuario.Nombre);
                    command.Parameters.AddWithValue("@Rol", usuario.Rol);
                    command.Parameters.AddWithValue("@Avatar", (object?)usuario.Avatar ?? DBNull.Value);
                    command.Parameters.AddWithValue("@IdUsuario", usuario.IdUsuario);

                    if (cambiarClave)
                        command.Parameters.AddWithValue("@Clave", HashPassword(usuario.NuevaClave));

                    connection.Open();
                    res = command.ExecuteNonQuery();
                }
            }
            return res;
        }

    }
}