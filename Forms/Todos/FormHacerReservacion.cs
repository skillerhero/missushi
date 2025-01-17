﻿using Missushi.Clases;
using Missushi.Forms.Administrador;
using Missushi.Forms.Gerente;
using Missushi.Funciones;
using QRCoder;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;

namespace Missushi.Forms.Cliente {
    public partial class FormHacerReservacion : FormDiseño {
        public FormHacerReservacion() {
            InitializeComponent();
            cargarBarraUsuario();
        }

        private void FormReservacionCliente_Load(object sender, EventArgs e) {
            comprobarHoraHoy();
            cbHoraInicio.DropDownStyle = ComboBoxStyle.DropDownList;
            cbHoraInicio.DisplayMember = "Text";
            cbHoraInicio.ValueMember = "Value";
            btnHacerReservacion.Enabled = false;
            nudCantidadPersonas.Enabled = false;
            dpFechaInicio.MinDate = DateTime.Today;
            dpFechaInicio.MaxDate = DateTime.Today.AddDays(7);
            if (Globales.usuarioActual.Tipo == 'C') {
                Globales.usuarioSeleccionado = Globales.usuarioActual;
                btnElegirUsuario.Text = Globales.usuarioSeleccionado.Nombres + " " + Globales.usuarioSeleccionado.Apellidos;
                btnElegirUsuario.Click -= new EventHandler(btnElegirUsuario_Click);
                btnElegirUsuario.Cursor = Cursors.Default;
            } else {
                Globales.usuarioSeleccionado = new Usuario();
            }
        }

        private void btnHacerReservacion_Click(object sender, EventArgs e) {
            try {
                DateTime fechaInicio = obtenerFechaInicio();
                DateTime fechaFin = obtenerFechaFin();
                int cantidadPersonas = (int)nudCantidadPersonas.Value;
                int idUsuario = Globales.usuarioSeleccionado.IdUsuario;
                int idZona = Globales.zonaSeleccionada.IdZona;
                string estado = "En espera";

                if (ConexionBD.usuarioTieneReservacionesEnEspera(Globales.usuarioSeleccionado.IdUsuario)) {
                    MessageBox.Show("Tiene una reservación en espera. Cáncelela o asista a la reservación.");
                    Close();
                    return;
                }
                if (!comprobaciones()) {
                    MessageBox.Show("Se han actualizado los cupos, por favor vuelva a seleccionar los datos de la reservación.");
                    return;
                }
                ConexionBD.agregarReservacion(fechaInicio, fechaFin, cantidadPersonas, Globales.usuarioSeleccionado.IdUsuario, idZona, estado);
                mandarCorreo();
                MessageBox.Show("Reservacion creada");
                Close();
            } catch (Exception ex) {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnElegirUsuario_Click(object sender, EventArgs e) {
            FormElegirUsuario formElegirUsuario = new FormElegirUsuario();
            if (formElegirUsuario.ShowDialog() == DialogResult.OK) {
                Globales.usuarioSeleccionado = ConexionBD.consultarUsuario(Globales.usuarioSeleccionado.IdUsuario);
                btnElegirUsuario.Text = Globales.usuarioSeleccionado.Nombres + " " + Globales.usuarioSeleccionado.Apellidos;
            }
        }

        private void btnElegirZona_Click(object sender, EventArgs e) {
            FormElegirZona formElegirZona = new FormElegirZona(obtenerFechaInicio());
            if (formElegirZona.ShowDialog() == DialogResult.OK) {
                this.btnElegirZona.Text = "Zona " + Globales.zonaSeleccionada.IdZona;
                this.nudCantidadPersonas.Focus();
                comprobaciones();
            }
        }

        private bool comprobaciones() {
            if (comprobarCupo() > 0 && comprobarId() && DateTime.Now < obtenerFechaInicio() && Globales.usuarioSeleccionado.IdUsuario != -1) {
                btnHacerReservacion.Enabled = true;
                return true;
            } else {
                btnHacerReservacion.Enabled = false;
                return false;
            }
        }
        private bool comprobarId() {
            if (Globales.usuarioSeleccionado.IdUsuario == -1) {
                return false;
            }
            return true;
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
                    fechaInicio = fechaInicio.AddHours(10);
                    break;
                case 2:
                    fechaInicio = fechaInicio.AddHours(12);
                    break;
                case 3:
                    fechaInicio = fechaInicio.AddHours(14);
                    break;
                case 4:
                    fechaInicio = fechaInicio.AddHours(16);
                    break;
                case 5:
                    fechaInicio = fechaInicio.AddHours(18);
                    break;
                case 6:
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

        private int comprobarCupo() {
            if (Globales.zonaSeleccionada.IdZona == -1) {
                return 0;
            }
            int cupoZona = ConexionBD.consultarCupoZona(Globales.zonaSeleccionada.IdZona, obtenerFechaInicio());
            if (cupoZona == 0) {
                btnHacerReservacion.Enabled = false;
                nudCantidadPersonas.Enabled = false;
                MessageBox.Show("No hay cupos en esta zona y horario.\nPuede seleccionar otra zona u horario.");
                return 0;
            }
            if (cbHoraInicio.Enabled == true) {
                btnHacerReservacion.Enabled = true;
                nudCantidadPersonas.Enabled = true;
            }
            return cupoZona;
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
                btnElegirZona.Enabled = true;
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
                btnElegirZona.Enabled = false;
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
                btnElegirZona.Enabled = true;
            }
            cbHoraInicio.DataSource = horarios;
        }

        private void nudCantidadPersonas_ValueChanged(object sender, EventArgs e) {
            int cupo = comprobarCupo();
            if (nudCantidadPersonas.Value > cupo) {
                MessageBox.Show("Cupo máximo: " + cupo + " personas.");
                nudCantidadPersonas.Value = cupo;
            }
        }

        private void mandarCorreo() {
            try {
                Usuario usuario = Globales.usuarioSeleccionado;
                string remitente = "missushi.contacto@gmail.com";
                string destinatario = usuario.Correo;
                //string copiaA = "danna.medina2869@alumnos.udg.mx";


                var dirRemitente = new MailAddress(remitente, "Missushi");
                var dirDestinatario = new MailAddress(destinatario, usuario.Nombres);
                const string contra = "frribGLDb7D2mf";
                const string asunto = "Reservación";
                string body = "";

                var smtp = new SmtpClient {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(dirRemitente.Address, contra)
                };
                var message = new MailMessage(dirRemitente, dirDestinatario);
                //message.Bcc.Add(copiaA);
                message.AlternateViews.Add(GetEmbeddedImage());
                message.Subject = asunto;
                message.Body = body;
                message.IsBodyHtml = true;
                smtp.Send(message);
            } catch (Exception ex) {
                MessageBox.Show(ex.Message);
            }
        }
        private AlternateView GetEmbeddedImage() {
            Reservacion reservacion = ConexionBD.consultarReservacion(Globales.usuarioSeleccionado.IdUsuario);
            Usuario usuario = Globales.usuarioSeleccionado;
            QRCodeGenerator qr = new QRCodeGenerator();
            string url = "http://" + ConexionBD.ipServidor + "/login/login.php?idReservacion=" + reservacion.IdReservacion;
            QRCodeData data = qr.CreateQrCode(url, QRCodeGenerator.ECCLevel.Q);
            QRCode code = new QRCode(data);

            Bitmap b = code.GetGraphic(5);
            ImageConverter ic = new ImageConverter();
            Byte[]? ba = ic.ConvertTo(b, typeof(Byte[])) as Byte[];
            MemoryStream memoryStream = new MemoryStream(ba);

            LinkedResource res = new LinkedResource(memoryStream);
            res.ContentId = Guid.NewGuid().ToString();
            res.ContentType.Name = "reservacion.jpg";
            string htmlBody =
            "<font face='Century Gothic' align='center'>"+
            "<div align = 'center'>"+@"<img src = 'cid:" + res.ContentId + @"'/></div>"+
            "<h2 style = 'color: #61784f'> Tu reservacion es... </h2>"+ 
            "<p><b style = 'color: #8f443c'> Nombre:</b> "+ usuario.Nombres + " " + usuario.Apellidos+"</p>" +
            "<p><b style = 'color: #8f443c'> Personas:</b> "+ reservacion.CantidadPersonas +" </p>" +
            "<p><b style = 'color: #8f443c'> Fecha y hora de inicio:</b> "+ reservacion.FechaHoraInicio+ "</p>" +
            "<p><b style = 'color: #8f443c'> Fecha y hora de fin:</b> "+ reservacion.FechaHoraFin+"</p>"+
            "<p><b style = 'color: #8f443c'> Zona:</b> "+ reservacion.IdZona + " </p>" +
            "</font>";
            AlternateView alternateView = AlternateView.CreateAlternateViewFromString(htmlBody, null, MediaTypeNames.Text.Html);
            alternateView.LinkedResources.Add(res);
            return alternateView;
        }
    }
}
