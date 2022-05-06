﻿using Missushi.Clases;
using Missushi.Forms.Gerente;
using Missushi.Funciones;

namespace Missushi.Forms.Administrador {
    public partial class FormHacerReservacionAdministrador : FormDiseño {
        public static int idUsuario = -1;
        public FormHacerReservacionAdministrador() {
            InitializeComponent();
        }

        private void btnElegirUsuario_Click(object sender, EventArgs e) {
            FormElegirUsuario formElegirUsuario = new FormElegirUsuario();
            if(formElegirUsuario.ShowDialog() == DialogResult.OK) {
                Usuario usuario = ConexionBD.consultarUsuario(idUsuario);
                btnElegirUsuario.Text = usuario.Nombres + " " + usuario.Apellidos;
                comprobaciones();
            }
        }

        private void FormHacerReservacionAdministrador_Load(object sender, EventArgs e) {
            comprobarHoraHoy();
            cbHoraInicio.DropDownStyle = ComboBoxStyle.DropDownList;
            cbHoraInicio.DisplayMember = "Text";
            cbHoraInicio.ValueMember = "Value";
            btnHacerReservacion.Enabled = false;
            nudCantidadPersonas.Enabled = false;
            dpFechaInicio.MinDate = DateTime.Today;
            dpFechaInicio.MaxDate = DateTime.Today.AddDays(7);
        }


        private void btnElegirZona_Click(object sender, EventArgs e) {
            FormElegirZona formElegirZona = new FormElegirZona(obtenerFechaInicio());
            if (formElegirZona.ShowDialog() == DialogResult.OK) {
                this.btnElegirZona.Text = "Zona " + Globales.zonaSeleccionada.IdZona;
                this.nudCantidadPersonas.Focus();
                comprobaciones();
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
            fechaFin = fechaFin.AddHours(2);
            return fechaFin;
        }

        private void cbHoraInicio_SelectedIndexChanged(object sender, EventArgs e) {
            comprobarCupo();
        }

        private void dpFechaInicio_ValueChanged(object sender, EventArgs e) {
            comprobarHoraHoy();
            comprobarCupo();
        }

        private bool comprobarCupo() {
            if (Globales.zonaSeleccionada.IdZona == -1) {
                return false;
            }
            int cupoZona = ConexionBD.consultarCupoZona(Globales.zonaSeleccionada.IdZona, obtenerFechaInicio());
            if (cupoZona == 0) {
                btnHacerReservacion.Enabled = false;
                nudCantidadPersonas.Enabled = false;
                MessageBox.Show("No hay cupos en esta zona y horario.\nPuede seleccionar otra zona u horario.");
                return false;
            }
            nudCantidadPersonas.Maximum = cupoZona;
            nudCantidadPersonas.Enabled = true;
            return true;
        }

        private bool comprobarId() {
            if(idUsuario == -1) {
                return false;
            }
            return true;
        }

        private void comprobaciones() {
            if(comprobarCupo() && comprobarId()) {
                btnHacerReservacion.Enabled = true;
            }
        }

        private void comprobarHoraHoy() {
            var horarios = new[] {
                 new { Text = "08:00-10:00", Value = 0 },
                 new { Text = "10:00-12:00", Value = 1 },
                 new { Text = "12:00-14:00", Value = 2 },
                 new { Text = "14:00-16:00", Value = 3 },
                 new { Text = "16:00-18:00", Value = 4 },
                 new { Text = "18:00-20:00", Value = 5},
                 new { Text = "20:00-22:00", Value = 6}
            };
            if (dpFechaInicio.Value.Date != DateTime.Today) {
                cbHoraInicio.DataSource = horarios;
                cbHoraInicio.Enabled = true;
                return;
            }
            TimeSpan horario1 = new TimeSpan(8, 0, 0);
            TimeSpan horario2 = new TimeSpan(10, 0, 0);
            TimeSpan horario3 = new TimeSpan(12, 0, 0);
            TimeSpan horario4 = new TimeSpan(14, 0, 0);
            TimeSpan horario5 = new TimeSpan(16, 0, 0);
            TimeSpan horario6 = new TimeSpan(18, 0, 0);
            TimeSpan horario7 = new TimeSpan(20, 0, 0);

            if (DateTime.Now.TimeOfDay > horario7) {
                horarios = null;
                cbHoraInicio.Enabled = false;
            } else if (DateTime.Now.TimeOfDay > horario6) {
                horarios = new[] {
                    new { Text = "20:00-22:00", Value = 6}
                };
            } else if (DateTime.Now.TimeOfDay > horario5) {
                horarios = new[] {
                     new { Text = "18:00-20:00", Value = 5},
                     new { Text = "20:00-22:00", Value = 6}
                };
            } else if (DateTime.Now.TimeOfDay > horario4) {
                horarios = new[] {
                     new { Text = "16:00-18:00", Value = 4 },
                     new { Text = "18:00-20:00", Value = 5},
                     new { Text = "20:00-22:00", Value = 6}
                };
            } else if (DateTime.Now.TimeOfDay > horario3) {
                horarios = new[] {
                     new { Text = "14:00-16:00", Value = 3 },
                     new { Text = "16:00-18:00", Value = 4 },
                     new { Text = "18:00-20:00", Value = 5},
                     new { Text = "20:00-22:00", Value = 6}
                };
            } else if (DateTime.Now.TimeOfDay > horario2) {
                horarios = new[] {
                     new { Text = "12:00-14:00", Value = 2 },
                     new { Text = "14:00-16:00", Value = 3 },
                     new { Text = "16:00-18:00", Value = 4 },
                     new { Text = "18:00-20:00", Value = 5},
                     new { Text = "20:00-22:00", Value = 6}
            };
            } else if (DateTime.Now.TimeOfDay > horario1) {
                horarios = new[] {
                     new { Text = "10:00-12:00", Value = 1 },
                     new { Text = "12:00-14:00", Value = 2 },
                     new { Text = "14:00-16:00", Value = 3 },
                     new { Text = "16:00-18:00", Value = 4 },
                     new { Text = "18:00-20:00", Value = 5},
                     new { Text = "20:00-22:00", Value = 6}
            };
            } else {
                cbHoraInicio.Enabled = true;
            }
            cbHoraInicio.DataSource = horarios;
        }

        private void btnHacerReservacion_Click(object sender, EventArgs e) {
            try {
                DateTime fechaInicio = obtenerFechaInicio();
                DateTime fechaFin = obtenerFechaFin();
                int cantidadPersonas = (int)nudCantidadPersonas.Value;
                int idZona = Globales.zonaSeleccionada.IdZona;
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
    }
}
