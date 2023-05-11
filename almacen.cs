using ClienteWS.Properties;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClienteWS
{
    public partial class almacen : Form
    {
        bool editar = false;
        private CurrentUser user;
        public almacen(CurrentUser usr)
        {
            InitializeComponent();
            user = new CurrentUser { correo = usr.correo, nombre = usr.nombre, pass = usr.pass };
            label1.Text = "¡Bienvenido, "+user.nombre+"!";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            upd_table();
        }

        private async void button5_Click(object sender, EventArgs e)
        {
            if (dataGridView2.SelectedRows.Count > 0)
            {

                int index = dataGridView2.SelectedRows[0].Index;
                string isbn = dataGridView2.Rows[index].Cells[0].Value.ToString();
                string rsr_det = await TokenUtils.get_detalles(user.correo, user.pass, isbn);
                RespDetalles resp = JsonConvert.DeserializeObject<RespDetalles>(rsr_det);
                if (resp.status == "error")
                {
                    MessageBox.Show("La consulta contiene un error, detalles: \n" + resp.message, "Algo salio mal... [" + resp.code + "]", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                Detalles det = JsonConvert.DeserializeObject<Detalles>(resp.data);
                textBox11.Text = isbn;
                string cat = isbn.Substring(0, 3);
                switch (cat)
                {
                    case "LIB":
                        {
                            comboBox3.SelectedIndex = 0;
                            break;
                        }
                    case "COM":
                        {
                            comboBox3.SelectedIndex = 1;
                            break;
                        }
                    case "MAN":
                        {
                            comboBox3.SelectedIndex = 2;
                            break;
                        }
                    default:
                        {
                            comboBox3.SelectedIndex = 0;
                            break;
                        }
                }
                textBox5.Text = det.Autor;
                textBox6.Text = det.Nombre;
                textBox7.Text = det.Editorial;
                textBox8.Text = det.Fecha.ToString();
                textBox9.Text = det.Precio.ToString();
                textBox10.Text = det.Descuento.ToString();
                editar = true;
                button2.Text = "Editar";
                button3.Text = "Cancelar edicion";
                comboBox3.Enabled = false;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (editar)
            {
                editar = false;
                button2.Text = "Añadir";
                button3.Text = "Cancelar";
                comboBox3.Enabled = true;
                textBox11.Text = "--Generado automaticamente--";
            }
            comboBox3.SelectedIndex = 0;
            textBox5.Text = "";
            textBox6.Text = "";
            textBox7.Text = "";
            textBox8.Text = "";
            textBox9.Text = "";
            textBox10.Text = "";
        }
        private bool validar_add()
        {
            bool validar = true;
            if (textBox5.Text == "" || textBox6.Text == "" || textBox7.Text == "" || textBox8.Text == "" || textBox9.Text == "" || textBox10.Text == "")
            {
                validar = false;
            }
            if (!TokenUtils.IsNumeric(textBox8.Text))
            {
                validar = false;
                errorProvider1.SetError(textBox8, "No se permiten valores no enteros");
            }
            else
            {
                errorProvider1.Clear();
            }
            if (!TokenUtils.IsNumeric(textBox9.Text))
            {
                validar = false;
                errorProvider2.SetError(textBox9, "No se permiten valores no enteros");
            }
            else
            {
                errorProvider2.Clear();
            }
            if (!TokenUtils.IsNumeric(textBox10.Text))
            {
                errorProvider3.SetError(textBox10, "No se permiten valores no enteros");
                validar = false;
            }
            else
            {
                errorProvider3.Clear();
            }
            return validar;
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            if (!validar_add())
            {
                errorProvider4.SetError(button2, "Algunos campos estan vacios o tienen tipos de datos incorrectos, la solicitud no se puede llevar a cabo");
                
                return;
            }
            else
            {
                errorProvider4.Clear();
            }
            progressBar1.Visible = true;
            disablebuttons();
            Detalles nvo_detalles = new Detalles
            {
                Autor = textBox5.Text,
                Nombre = textBox6.Text,
                Editorial = textBox7.Text,
                Fecha = Convert.ToInt32(textBox8.Text),
                Precio = Convert.ToInt32(textBox9.Text),
                Descuento = Convert.ToInt32(textBox10.Text)
            };
            string producto = JsonConvert.SerializeObject(nvo_detalles);
            if (editar)
            {
                string putresponse = await TokenUtils.put_producto(user.correo, user.pass, textBox11.Text, producto);
                Respuesta resp = JsonConvert.DeserializeObject<Respuesta>(putresponse);
                if (resp.status == "error")
                {
                    progressBar1.Visible = false;
                    enablebuttons();
                    MessageBox.Show("La consulta contiene un error, detalles: \n" + resp.message, "Algo salio mal... [" + resp.code + "]", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                progressBar1.Visible = false;
                enablebuttons();
                MessageBox.Show("Operacion realizada con codigo: " + resp.code + " y con mensaje:\n" + resp.message + " con hora de accion " + resp.data, "Operación exitosa [" + resp.code + "]", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                string rsppost = await TokenUtils.post_producto(user.correo, user.pass, comboBox3.SelectedItem.ToString().ToLower(), producto);
                Respuesta resp = JsonConvert.DeserializeObject<Respuesta>(rsppost);
                if (resp.status == "error")
                {
                    progressBar1.Visible = false;
                    MessageBox.Show("La consulta contiene un error, detalles: \n" + resp.message, "Algo salio mal... [" + resp.code + "]", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                progressBar1.Visible = false;
                MessageBox.Show("Operacion realizada con codigo: " + resp.code + " y con mensaje:\n" + resp.message + " con hora de accion " + resp.data, "Operación exitosa [" + resp.code + "]", MessageBoxButtons.OK, MessageBoxIcon.Information);
                
            }
            comboBox3.SelectedIndex = 0;
            textBox5.Text = "";
            textBox6.Text = "";
            textBox7.Text = "";
            textBox8.Text = "";
            textBox9.Text = "";
            textBox10.Text = "";
            editar = false;
            button2.Text = "Añadir";
            button3.Text = "Cancelar";
            comboBox3.Enabled = true;
            textBox11.Text = "--Generado automaticamente--";
            progressBar1.Visible = false;
            upd_table();
            enablebuttons();
        }

        private async void button4_Click(object sender, EventArgs e)
        {
            if (dataGridView2.SelectedRows.Count > 0)
            {
                
                int index = dataGridView2.SelectedRows[0].Index;
                string isbn = dataGridView2.Rows[index].Cells[0].Value.ToString();
                string nombre = dataGridView2.Rows[index].Cells[1].Value.ToString();
                DialogResult result = MessageBox.Show("¿Esta seguro de querer eliminar el producto con ISBN: " + isbn + " y nombre: " + nombre + "? \n ¡Esta accion NO se puede deshacer!", "Eliminacion de registro", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result == DialogResult.Yes)
                {
                    progressBar1.Visible = true;
                    string delete_resp = await TokenUtils.delete_producto(user.correo, user.pass, isbn);
                    Respuesta resp = JsonConvert.DeserializeObject<Respuesta>(delete_resp);
                    if (resp.status == "error")
                    {
                        MessageBox.Show("La consulta contiene un error, detalles: \n" + resp.message, "Algo salio mal... [" + resp.code + "]", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        progressBar1.Visible = false;
                        return;
                    }
                    progressBar1.Visible = false;
                    MessageBox.Show("Operacion realizada con codigo: " + resp.code + " y con mensaje:\n" + resp.message + " con hora de accion " + resp.data, "Operación exitosa [" + resp.code + "]", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    
                    upd_table();
                }
            }
        }
        private async void upd_table()
        {
            
            if (textBox4.Text == "")
            {
                errorProvider1.SetError(textBox4, "Este campo no puede estar vacio");
                enablebuttons();
                return;
            }
            else
            {
                errorProvider1.Clear();
            }
            pictureBox1.Visible = true;
            disablebuttons();
            dataGridView2.Rows.Clear();
            string rsp = await TokenUtils.get_productos(user.correo, user.pass, textBox4.Text.ToLower());
            Respuesta resp = JsonConvert.DeserializeObject<Respuesta>(rsp);
            if (resp.status == "error")
            {
                pictureBox1.Visible = false;
                enablebuttons();
                MessageBox.Show("La consulta contiene un error, detalles: \n" + resp.message, "Algo salio mal... [" + resp.code + "]", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            var json = JsonConvert.DeserializeObject<Dictionary<string, string>>(resp.data);
            foreach (string x in json.Keys)
            {
                int rowEscribir = dataGridView2.Rows.Count;
                dataGridView2.Rows.Add(1);
                dataGridView2.Rows[rowEscribir].Cells[0].Value = x;
                dataGridView2.Rows[rowEscribir].Cells[1].Value = json[x];
            }
            enablebuttons();
            pictureBox1.Visible = false;
        }

        private void almacen_Load(object sender, EventArgs e)
        {
            comboBox3.SelectedIndex = 0;
            Random rnd = new Random();
            int index = rnd.Next(0, imageList1.Images.Count);
            pictureBox2.Image = imageList1.Images[index];
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Seguro de querer cerrar sesión?", "Cerrar sesion", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
            if (dr == DialogResult.Yes)
            {
                Settings.Default["AuthToken"] = "o-o-o-o";
                Settings.Default["sesion"] = "-";
                Settings.Default["id"] = "-";
                login nlog = new login(false);
                nlog.Show();
                Close();
            }
        }
        private void enablebuttons()
        {
            button1.Enabled = true;
            button2.Enabled = true;
            button3.Enabled = true;
            button4.Enabled = true;
            button5.Enabled = true;
        }
        private void disablebuttons()
        {
            button1.Enabled = false;
            button2.Enabled = false;
            button3.Enabled = false;
            button4.Enabled = false;
            button5.Enabled = false;

        }

        private void almacen_FormClosing(object sender, FormClosingEventArgs e)
        {
            //Application.Exit();
        }
    }
}
