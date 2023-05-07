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
    public partial class ventas : Form
    {
        private CurrentUser user;
        public ventas(CurrentUser usr)
        {
            InitializeComponent();
            user=new CurrentUser { correo=usr.correo, nombre=usr.nombre, pass=usr.pass};
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox1.BackColor = Color.White;
            if (comboBox1.SelectedIndex == 0)
            {
                label4.Text = "Categoria:";
                textBox3.Text = "";
            }
            if (comboBox1.SelectedIndex == 1)
            {
                label4.Text = "ISBN:";
                textBox3.Text = "";
            }
        }

        private void ventas_Load(object sender, EventArgs e)
        {
            comboBox1.SelectedIndex = 0;
        }

        private bool validar_ventas()
        {
            bool valido = true;
            if (textBox3.Text == "")
            {
                errorProvider1.SetError(textBox3, "Este campo no puede estar vacío");
                valido = false;
            }
            else
            {
                errorProvider1.Clear();
            }
            return valido;
        }

        private async void button6_Click(object sender, EventArgs e)
        {
            string txtbox3 = "";
            if (!validar_ventas())
            {
                return;
            }
            dataGridView1.Columns.Clear();

            if (comboBox1.SelectedIndex == 0)
            {
                txtbox3 = textBox3.Text.ToLower();
                string resp = await TokenUtils.get_productos(user.correo, user.pass, txtbox3);
                //MessageBox.Show(resp);
                Respuesta res = JsonConvert.DeserializeObject<Respuesta>(resp);
                //MessageBox.Show(res.data);
                label19.Text = res.code;
                label20.Text = res.message;
                label21.Text = res.status;
                if (res.status != "error")
                {
                    var json = JsonConvert.DeserializeObject<Dictionary<string, string>>(res.data);
                    dataGridView1.Columns.Add("columna1", "ISBN");
                    dataGridView1.Columns.Add("columna2", "Nombre");
                    foreach (string x in json.Keys)
                    {
                        int rowEscribir = dataGridView1.Rows.Count;
                        dataGridView1.Rows.Add(1);
                        dataGridView1.Rows[rowEscribir].Cells[0].Value = x;
                        dataGridView1.Rows[rowEscribir].Cells[1].Value = json[x];
                    }
                }
                else
                {
                    MessageBox.Show("La consulta contiene un error, detalles: \n" + res.message, "Algo salio mal... [" + res.code + "]", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
            if (comboBox1.SelectedIndex == 1)
            {
                txtbox3 = textBox3.Text.ToUpper();
                string rsp = await TokenUtils.get_detalles(user.correo, user.pass, txtbox3);
                RespDetalles respuesta = JsonConvert.DeserializeObject<RespDetalles>(rsp);
                label19.Text = respuesta.code;
                label20.Text = respuesta.message;
                label21.Text = respuesta.status;
                if (respuesta.status != "error")
                {
                    Detalles detalles = JsonConvert.DeserializeObject<Detalles>(respuesta.data);
                    dataGridView1.Columns.Add("columna3", "Propiedad");
                    dataGridView1.Columns.Add("columna4", "Valor");
                    var index = dataGridView1.Rows.Add();
                    dataGridView1.Rows[index].Cells["Columna3"].Value = "Nombre";
                    dataGridView1.Rows[index].Cells["Columna4"].Value = detalles.Nombre;
                    index = dataGridView1.Rows.Add();
                    dataGridView1.Rows[index].Cells["Columna3"].Value = "Autor";
                    dataGridView1.Rows[index].Cells["Columna4"].Value = detalles.Autor;
                    index = dataGridView1.Rows.Add();
                    dataGridView1.Rows[index].Cells["Columna3"].Value = "Descuento";
                    dataGridView1.Rows[index].Cells["Columna4"].Value = detalles.Descuento;
                    index = dataGridView1.Rows.Add();
                    dataGridView1.Rows[index].Cells["Columna3"].Value = "Editorial";
                    dataGridView1.Rows[index].Cells["Columna4"].Value = detalles.Editorial;
                    index = dataGridView1.Rows.Add();
                    dataGridView1.Rows[index].Cells["Columna3"].Value = "Fecha";
                    dataGridView1.Rows[index].Cells["Columna4"].Value = detalles.Fecha;
                    index = dataGridView1.Rows.Add();
                    dataGridView1.Rows[index].Cells["Columna3"].Value = "Precio";
                    dataGridView1.Rows[index].Cells["Columna4"].Value = detalles.Precio;
                }
                else
                {
                    MessageBox.Show("La consulta contiene un error, detalles: \n" + respuesta.message, "Algo salio mal... [" + respuesta.code + "]", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
