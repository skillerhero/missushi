﻿using Missushi.Clases;
using Missushi.Funciones;

namespace Missushi.Forms{
    public partial class FormLogin : FormDiseño {
        public FormLogin(){
            InitializeComponent();
            this.cargarPantallaIngresar();
            pbLogin.Cursor = Cursors.Default;
        }

        private void entrar() {
            if (Globales.usuarioActual.Tipo == 'C' || Globales.usuarioActual.Tipo == 'A' || Globales.usuarioActual.Tipo == 'G') {
                FormMain form = new FormMain();
                form.Show();
            }
            Globales.transition();
            Close();
        }

        private void btnIngresar_Click(object sender, EventArgs e) {
            try {
                string correo = txtCorreo.Text.Trim();
                string contraseña = txtContraseña.Text.Trim();
                if (contraseña.Length > 200) {
                    contraseña = contraseña[..200];
                }
                if (ConexionBD.usuarioSuspendido(correo, contraseña)) {
                    MessageBox.Show("Este usuario ha sido suspendido.");
                    Close();
                }else if(ConexionBD.usuarioEnEspera(correo, contraseña)) {
                    MessageBox.Show("Confirme su cuenta desde el correo que le mandamos.");
                    Close();
                } else if (ConexionBD.login(correo, contraseña)) {
                    MessageBox.Show("Bienvenido/a");
                    entrar();

                } else {
                    MessageBox.Show("Correo o contraseña incorrectos.");
                }
            } catch (Exception ex) { 
                MessageBox.Show(ex.Message);
            }
        }

        private void clienteToolStripMenuItem_Click(object sender, EventArgs e) {
            if (ConexionBD.consultarPrimerUsuario('C')) {
                entrar();
            } else {
                MessageBox.Show("No hay clientes registrados.");
            }
        }

        private void adminToolStripMenuItem_Click(object sender, EventArgs e) {
            if (ConexionBD.consultarPrimerUsuario('A')) {
                entrar();
            } else {
                MessageBox.Show("No hay administradores registrados.");
            }
        }

        private void gerenteToolStripMenuItem_Click(object sender, EventArgs e) {
            if (ConexionBD.consultarPrimerUsuario('G')) {
                entrar();
            } else {
                MessageBox.Show("No hay gerentes registrados.");
            }
        }
    }
}
