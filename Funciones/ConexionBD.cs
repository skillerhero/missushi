﻿using Missushi.Clases;
using MySqlConnector;
using System.Data;

namespace Missushi.Funciones{
    static internal class ConexionBD{
#pragma warning disable CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
        static public MySqlConnection connection;
#pragma warning restore CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
        static public void conectarBD(){
            try{
                MySqlConnectionStringBuilder builder = new MySqlConnectionStringBuilder();
                builder.Server = "localhost";
                builder.UserID = "root";
                builder.Database = "missushi";
                builder.Password = "";
                builder.ApplicationName = "";
                //builder.ApplicationName = "app";
                //builder.Port = 3306;
                connection = new MySqlConnection(builder.ToString());
            }catch (Exception e) {
                MessageBox.Show(e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        static public List<Usuario> consultarUsuarios() {
                List<Usuario> usuarios = new List<Usuario>();
                ConexionBD.connection.Open();
                String query = "SELECT * FROM usuario;";
                MySqlCommand cmd = new MySqlCommand(query, ConexionBD.connection);
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read()) {
                    usuarios.Add(new Usuario(reader.GetInt32(0), reader.GetString(1), reader.GetString(2), reader.GetString(3), reader.GetString(4), reader.GetChar(5)));
                }
                ConexionBD.connection.Close();
                return usuarios;
        }

        static public MySqlDataAdapter consultarUsuariosAdapter() {
            List<Usuario> usuarios = new List<Usuario>();
            ConexionBD.connection.Open();
            String query = "SELECT * FROM usuario;";
            MySqlDataAdapter adapter = new MySqlDataAdapter(query, ConexionBD.connection);
            ConexionBD.connection.Close();
            return adapter;
        }

        static public bool insertarUsuario(string nombres, string apellidos, string contraseña, string correo, char tipo){
            ConexionBD.connection.Open();
            string sql = "INSERT INTO usuario(nombres, apellidos, contrasenia, correo, tipo) VALUES(@0,@1,@2, @3, @4)";
            MySqlCommand cmd = new MySqlCommand(sql, ConexionBD.connection);
            cmd.Parameters.Add("@0", MySqlDbType.VarChar, 80).Value = nombres;
            cmd.Parameters.Add("@1", MySqlDbType.VarChar, 80).Value = apellidos;
            cmd.Parameters.Add("@2", MySqlDbType.VarChar, 50).Value = contraseña;
            cmd.Parameters.Add("@3", MySqlDbType.VarChar, 50).Value = correo;
            cmd.Parameters.Add("@4", MySqlDbType.VarChar, 1).Value = tipo;
            cmd.CommandType = CommandType.Text;
            cmd.ExecuteNonQuery();
            ConexionBD.connection.Close();
            MessageBox.Show("Registrado con éxito,", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return true;
        }

        static public bool login(string correo, string contraseña) {
            bool exito = false;
            string sql = "SELECT idUsuario, contrasenia, tipo FROM usuario where correo = @0";
            ConexionBD.connection.Open();
            MySqlCommand cmd = new MySqlCommand(sql, ConexionBD.connection);
            cmd.Parameters.AddWithValue("@0", correo);
            MySqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read()) {
                if (reader.GetString(1)==contraseña) {
                    exito = true;
                    Usuario.id = reader.GetInt32(0);
                    Usuario.type = reader.GetChar(2);
                }
            }
            ConexionBD.connection.Close();
            return exito;
        }

        static public bool existeCorreo(string correo) {
            bool existe = false;
            string sql = "SELECT * FROM usuario where correo = @0";
            ConexionBD.connection.Open();
            MySqlCommand cmd = new MySqlCommand(sql, ConexionBD.connection);
            cmd.Parameters.AddWithValue("@0", correo);
            MySqlDataReader reader = cmd.ExecuteReader();
            if(reader.Read()) {
                existe = true;
            }
            ConexionBD.connection.Close();
            return existe;
        }

        static public bool existeGerente() {
            bool existe = false;
            string sql = "SELECT * FROM usuario where tipo = @0";
            ConexionBD.connection.Open();
            MySqlCommand cmd = new MySqlCommand(sql, ConexionBD.connection);
            cmd.Parameters.AddWithValue("@0", "G");
            MySqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read()) {
                existe = true;
            }
            ConexionBD.connection.Close();
            return existe;
        }

        static public Restaurante consultarRestaurante() {
            Restaurante restaurante = new Restaurante();
            string sql = "SELECT * FROM restaurante;";
            ConexionBD.connection.Open();
            MySqlCommand cmd = new MySqlCommand(sql, ConexionBD.connection);
            MySqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read()) {
                restaurante.setNombre(reader.GetString(0));
                restaurante.setDescripcion(reader.GetString(1));
                restaurante.setLatitud(reader.GetString(2));
                restaurante.setLongitud(reader.GetString(3));
                restaurante.setFotoPrincipal(reader.GetString(4));
                restaurante.setGerente(reader.GetInt32(5));
            }
            ConexionBD.connection.Close();
            return restaurante;
        }

        static public bool modificarRestaurante(string nombre, string descripcion, string latitud, string longitud, string fotoPrincipal, int idGerente) {
            ConexionBD.connection.Open();
            string sql = "UPDATE usuario set nombre=@0, descripcion=@1, latitud=@2, longitud=@3, fotoPrincipal=@4, idGerente=@5;";
            MySqlCommand cmd = new MySqlCommand(sql, ConexionBD.connection);
            cmd.Parameters.Add("@0", MySqlDbType.VarChar, 50).Value = nombre;
            cmd.Parameters.Add("@1", MySqlDbType.VarChar, 500).Value = descripcion;
            cmd.Parameters.Add("@2", MySqlDbType.VarChar, 100).Value = latitud;
            cmd.Parameters.Add("@3", MySqlDbType.VarChar, 100).Value = longitud;
            cmd.Parameters.Add("@4", MySqlDbType.String).Value = fotoPrincipal;
            cmd.Parameters.Add("@5", MySqlDbType.Int32).Value = idGerente;
            cmd.CommandType = CommandType.Text;
            cmd.ExecuteNonQuery();
            ConexionBD.connection.Close();
            MessageBox.Show("Modificado con éxito,", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return true;
        }
    }  
}
