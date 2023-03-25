using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClienteWS
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            comboBox1.SelectedIndex = 0;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
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

        private void button6_Click(object sender, EventArgs e)
        {
            //HttpClient client = new HttpClient();
            dataGridView1.Columns.Clear();

            if (comboBox1.SelectedIndex == 0)
            {
                string resp = get_productos(textBox1.Text, textBox2.Text, textBox3.Text);
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
                        int rowEscribir = dataGridView1.Rows.Count - 1;
                        dataGridView1.Rows.Add(1);
                        dataGridView1.Rows[rowEscribir].Cells[0].Value = x;
                        dataGridView1.Rows[rowEscribir].Cells[1].Value = json[x];
                    }
                }
                else
                {
                    MessageBox.Show("La consulta contiene un error, detalles: \n"+res.message, "Algo salio mal... ["+res.code+"]", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                
            }
            if (comboBox1.SelectedIndex == 1)
            {
                string rsp = get_detalles(textBox1.Text, textBox2.Text, textBox3.Text);
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

        private string get_productos(string user, string pass, string categoria)
        {
            
            string respuesta = "";
            using(var client=new HttpClient())
            {
                var req = new HttpRequestMessage(HttpMethod.Get, "http://localhost:8080/2ndtestslim/productos/" + categoria);
                req.Headers.Add("user", user);
                req.Headers.Add("pass", pass);
                var contenido = client.SendAsync(req).Result;

                respuesta = contenido.Content.ReadAsStringAsync().Result;
            }
            return respuesta;
        }

        private string get_detalles(string user, string pass, string ISBN)
        {
            string respuesta = "";
            using(var client=new HttpClient())
            {
                var req = new HttpRequestMessage(HttpMethod.Get, "http://localhost:8080/2ndtestslim/detalles/" + ISBN);
                req.Headers.Add("user", user);
                req.Headers.Add("pass", pass);
                var contenido = client.SendAsync(req).Result;

                respuesta = contenido.Content.ReadAsStringAsync().Result;
            }

            return respuesta;
        }

        private string post_producto(string user, string pass, string categoria, string producto)
        {
            string respuesta = "";


            return respuesta;
        }

        private string put_producto(string user, string pass, string clave, string detalles)
        {
            string respuesta = "";


            return respuesta;
        }

        private string delete_producto(string user, string pass, string clave)
        {
            string respuesta = "";


            return respuesta;
        }
    }
    // Respuesta resp = JsonConvert.DeserializeObject<Respuesta>(myJsonResponse);
    public class Respuesta
    {
        public string code { get; set; }
        public string message { get; set; }
        public string data { get; set; }
        public string status { get; set; }
    }
    // Detalles detalles = JsonConvert.DeserializeObject<Detalles>(myJsonResponse);
    public class RespDetalles
    {
        public string code { get; set; }
        public string message { get; set; }
        public string data { get; set; }
        public string status { get; set; }
        public bool oferta { get; set; }
    }
    // Detalles det = JsonConvert.DeserializeObject<Detalles>(myJsonResponse);
    public class Detalles
    {
        public string Autor { get; set; }
        public int Descuento { get; set; }
        public string Editorial { get; set; }
        public int Fecha { get; set; }
        public string Nombre { get; set; }
        public int Precio { get; set; }
    }

}
