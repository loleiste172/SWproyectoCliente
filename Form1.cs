using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClienteWS
{
    public partial class Form1 : Form
    {
        bool editar = false;
        string carpeta = "2ndtestslim"; //de yo
        //string carpeta = "WS/p08"; //de zucena
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            comboBox1.SelectedIndex = 0;
            comboBox3.SelectedIndex = 0;
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

        private async void button6_Click(object sender, EventArgs e)
        {
            string txtbox3 = "";
            if (!validar_usr_pass())
            {
                return;
            }
            if (!validar_ventas())
            {
                return;
            }
            
            //HttpClient client = new HttpClient();
            dataGridView1.Columns.Clear();

            if (comboBox1.SelectedIndex == 0)
            {
                txtbox3 = textBox3.Text.ToLower();
                string resp = await TokenUtils.get_productos(textBox1.Text, textBox2.Text, txtbox3);
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
                    MessageBox.Show("La consulta contiene un error, detalles: \n"+res.message, "Algo salio mal... ["+res.code+"]", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                
            }
            if (comboBox1.SelectedIndex == 1)
            {
                txtbox3 = textBox3.Text.ToUpper();
                string rsp = await TokenUtils.get_detalles(textBox1.Text, textBox2.Text, txtbox3);
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

        

        private async void button1_Click(object sender, EventArgs e)
        {
            
            if (!validar_usr_pass())
            {
                return;
            }
            if (textBox4.Text == "")
            {
                errorProvider4.SetError(textBox4, "Este campo no puede estar vacio");
                return;
            }
            else
            {
                errorProvider4.Clear();
            }
            dataGridView2.Rows.Clear();
            string rsp = await TokenUtils.get_productos(textBox1.Text, textBox2.Text, textBox4.Text.ToLower());
            Respuesta resp = JsonConvert.DeserializeObject<Respuesta>(rsp);
            if (resp.status == "error")
            {
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

        }

        private async void button5_Click(object sender, EventArgs e)
        {
            if (dataGridView2.SelectedRows.Count > 0)
            {
                int index = dataGridView2.SelectedRows[0].Index;
                string isbn = dataGridView2.Rows[index].Cells[0].Value.ToString();
                string rsr_det =await TokenUtils.get_detalles(textBox1.Text, textBox2.Text, isbn);
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

        private async void button2_Click(object sender, EventArgs e)
        {
            if (!validar_add())
            {
                errorProvider5.SetError(button2, "Algunos campos estan vacios o tienen tipos de datos incorrectos, la solicitud no se puede llevar a cabo");
                return;
            }
            else
            {
                errorProvider5.Clear();
            }
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
                string putresponse = await TokenUtils.put_producto(textBox1.Text, textBox2.Text, textBox11.Text, producto);
                Respuesta resp = JsonConvert.DeserializeObject<Respuesta>(putresponse);
                if (resp.status == "error")
                {
                    MessageBox.Show("La consulta contiene un error, detalles: \n" + resp.message, "Algo salio mal... [" + resp.code + "]", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                MessageBox.Show("Operacion realizada con codigo: " + resp.code + " y con mensaje:\n" + resp.message + " con hora de accion " + resp.data, "Operación exitosa ["+resp.code+"]", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                string rsppost=await TokenUtils.post_producto(textBox1.Text, textBox2.Text, comboBox3.SelectedItem.ToString().ToLower(), producto);
                Respuesta resp = JsonConvert.DeserializeObject<Respuesta>(rsppost);
                if (resp.status == "error")
                {
                    MessageBox.Show("La consulta contiene un error, detalles: \n" + resp.message, "Algo salio mal... [" + resp.code + "]", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                MessageBox.Show("Operacion realizada con codigo: "+resp.code+" y con mensaje:\n"+resp.message+" con hora de accion "+resp.data, "Operación exitosa [" + resp.code + "]", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        }

        private async void button4_Click(object sender, EventArgs e)
        {
            if(dataGridView2.SelectedRows.Count > 0)
            {
                int index = dataGridView2.SelectedRows[0].Index;
                string isbn = dataGridView2.Rows[index].Cells[0].Value.ToString();
                string nombre= dataGridView2.Rows[index].Cells[1].Value.ToString();
                DialogResult result= MessageBox.Show("¿Esta seguro de querer eliminar el producto con ISBN: " + isbn + " y nombre: " + nombre + "? \n ¡Esta accion NO se puede deshacer!", "Eliminacion de registro", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if(result == DialogResult.Yes)
                {
                    string delete_resp =await TokenUtils.delete_producto(textBox1.Text, textBox2.Text, isbn);
                    Respuesta resp = JsonConvert.DeserializeObject<Respuesta>(delete_resp);
                    if (resp.status == "error")
                    {
                        MessageBox.Show("La consulta contiene un error, detalles: \n" + resp.message, "Algo salio mal... [" + resp.code + "]", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    MessageBox.Show("Operacion realizada con codigo: " + resp.code + " y con mensaje:\n" + resp.message + " con hora de accion " + resp.data, "Operación exitosa [" + resp.code + "]", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
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
        private bool validar_usr_pass()
        {
            bool valido = true;
            if (textBox1.Text=="")
            {
                errorProvider2.SetError(textBox1, "El campo usuario no debe estar vacio");
                valido = false;
            }
            else
            {
                errorProvider2.Clear();
            }
            if (textBox2.Text == "")
            {
                errorProvider3.SetError(textBox2, "La contraseña no puede estar vacia");
            }
            else
            {
                errorProvider3.Clear();
            }
            return valido;
        }
        private bool validar_add()
        {
            bool validar = true;
            if (textBox5.Text == "" || textBox6.Text == "" || textBox7.Text == "" || textBox8.Text == "" || textBox9.Text == "" || textBox10.Text == "")
            {
                validar = false;
            }
            if (!IsNumeric(textBox8.Text))
            {
                validar = false;
                errorProvider1.SetError(textBox8, "No se permiten valores no enteros");
            }
            else
            {
                errorProvider1.Clear();
            }
            if (!IsNumeric(textBox9.Text))
            {
                validar = false;
                errorProvider2.SetError(textBox9, "No se permiten valores no enteros");
            }
            else
            {
                errorProvider2.Clear();
            }
            if (!IsNumeric(textBox10.Text))
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
        public static bool IsNumeric(object Expression)
        {
            double retNum;

            bool isNum = double.TryParse(Convert.ToString(Expression), System.Globalization.NumberStyles.Any, System.Globalization.NumberFormatInfo.InvariantInfo, out retNum);
            return isNum;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void label15_Click(object sender, EventArgs e)
        {

        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox3.BackColor = Color.White;
        }

        private void progressBar1_Click(object sender, EventArgs e)
        {

        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {

        }

        private void label17_Click(object sender, EventArgs e)
        {

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
    // RespDetalles detalles = JsonConvert.DeserializeObject<RespDetalles>(myJsonResponse);
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
    public class Body_req
    {
        public string categoria { get; set; }
        public string producto { get; set; }
    }
    public class Body_put
    {
        public string clave { get; set; }
        public string detalles { get; set; }
    }
    class Body_delete
    {
        public string clave { get; set; }
    }
}
