﻿using Missushi.Clases;
using Missushi.Funciones;

namespace Missushi.Forms.Cliente {
    public partial class FormReservacionCliente : Form {
        public FormReservacionCliente() {
            InitializeComponent();
        }

        private void btnHacerReservacion_Click(object sender, EventArgs e) {
            try {
                DateTime fechaInicio = obtenerFechaInicio();
                DateTime fechaFin = obtenerFechaFin();
                int cantidadPersonas = (int)nudCantidadPersonas.Value;
                int idUsuario = Usuario.id;
                int idZona = Zona.id;
                string estado = "En espera";

                if (ConexionBD.usuarioTieneReservacionesEnEspera(idUsuario)) {
                    MessageBox.Show("Tiene una reservación en espera. Cáncelela o asista a la reservación.");
                    this.DialogResult = DialogResult.Cancel;
                    return;
                }
                ConexionBD.agregarReservacion(fechaInicio, fechaFin, cantidadPersonas, idUsuario, idZona, estado);

                MessageBox.Show("Reservacion creada");
                this.DialogResult = DialogResult.OK;
            } catch (Exception ex) {
                MessageBox.Show(ex.Message);
            }
        }

        private void FormReservacionCliente_Load(object sender, EventArgs e) {
            comprobarHoraHoy();
            cbHoraInicio.DropDownStyle = ComboBoxStyle.DropDownList;
            cbHoraInicio.DisplayMember = "Text";
            cbHoraInicio.ValueMember = "Value";
            btnHacerReservacion.Enabled = false;
            nudCantidadPersonas.Enabled = false;
            dpFechaInicio.MinDate = DateTime.Today;
        }
        private void btnElegirZona_Click(object sender, EventArgs e) {
            FormElegirZona formElegirZona = new FormElegirZona(obtenerFechaInicio());
            if(formElegirZona.ShowDialog() == DialogResult.OK) {
                this.btnElegirZona.Text = "Zona " + Zona.id;
                this.nudCantidadPersonas.Focus();
                comprobarCupo();
            }
        }

        private DateTime obtenerFechaInicio() {
            DateTime fechaInicio;
            fechaInicio = dpFechaInicio.Value.Date;
            ;
            switch (cbHoraInicio.SelectedValue) {
                case 0:
                    fechaInicio = fechaInicio.AddHours(8);
                    break;
                case 1:
                    fechaInicio = fechaInicio.AddHours(11);
                    break;
                case 2:
                    fechaInicio = fechaInicio.AddHours(14);
                    break;
                case 3:
                    fechaInicio = fechaInicio.AddHours(17);
                    break;
                case 4:
                    fechaInicio = fechaInicio.AddHours(20);
                    break;
            }
            return fechaInicio;
        }
        private DateTime obtenerFechaFin() {
            DateTime fechaInicio = obtenerFechaInicio();
            DateTime fechaFin = fechaInicio;
            fechaFin = fechaFin.AddHours(3);
            return fechaFin;
        }

        private void cbHoraInicio_SelectedIndexChanged(object sender, EventArgs e) {
            comprobarCupo();
        }

        private void dpFechaInicio_ValueChanged(object sender, EventArgs e) {
            comprobarHoraHoy();
            comprobarCupo();
        }

        private void comprobarCupo() {
            if (Zona.id == -1) {
                return;
            }
            int cupoZona = ConexionBD.consultarCupoZona(Zona.id, obtenerFechaInicio());
            if (cupoZona == 0) {
                btnHacerReservacion.Enabled = false;
                nudCantidadPersonas.Enabled = false;
                MessageBox.Show("No hay cupos en esta zona y horario.\nPuede seleccionar otra zona u horario.");
                return;
            }
            nudCantidadPersonas.Maximum = cupoZona;
            btnHacerReservacion.Enabled = true;
            nudCantidadPersonas.Enabled = true;
        }

        private void comprobarHoraHoy() {
            var horarios = new[] {
                 new { Text = "08:00-11:00", Value = 0 },
                 new { Text = "11:00-14:00", Value = 1 },
                 new { Text = "14:00-17:00", Value = 2 },
                 new { Text = "17:00-20:00", Value = 3 },
                 new { Text = "20:00-23:00", Value = 4 }
            };
            if (dpFechaInicio.Value.Date != DateTime.Today) {
                cbHoraInicio.DataSource = horarios;
                return;
            }
            TimeSpan horario1 = new TimeSpan(8, 0, 0);
            TimeSpan horario2 = new TimeSpan(11, 0, 0);
            TimeSpan horario3 = new TimeSpan(14, 0, 0);
            TimeSpan horario4 = new TimeSpan(17, 0, 0);
            TimeSpan horario5 = new TimeSpan(20, 0, 0);

            
            if(DateTime.Now.TimeOfDay > horario1) {
                horarios = new[] {
                 new { Text = "11:00-14:00", Value = 1 },
                 new { Text = "14:00-17:00", Value = 2 },
                 new { Text = "17:00-20:00", Value = 3 },
                 new { Text = "20:00-23:00", Value = 4 }
            };
            }
            if (DateTime.Now.TimeOfDay > horario2) {
                horarios = new[] {
                 new { Text = "14:00-17:00", Value = 2 },
                 new { Text = "17:00-20:00", Value = 3 },
                 new { Text = "20:00-23:00", Value = 4 }
            };
            }
            if (DateTime.Now.TimeOfDay > horario3) {
                horarios = new[] {
                 new { Text = "17:00-20:00", Value = 3 },
                 new { Text = "20:00-23:00", Value = 4 }
            };
            }
            if (DateTime.Now.TimeOfDay > horario4) {
                horarios = new[] {
                 new { Text = "20:00-23:00", Value = 4 }
                };
            }
            if (DateTime.Now.TimeOfDay > horario5) {
                horarios = null;
                cbHoraInicio.Enabled = false;
            } else {
                cbHoraInicio.Enabled = true;
            }
            cbHoraInicio.DataSource = horarios;
        }
    }
}
