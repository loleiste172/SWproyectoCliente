using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using ClienteWS.Properties;
using MD5Hash;

namespace ClienteWS
{
    public partial class login : Form
    {
        bool vis;
        //los de yo ((lao sjsjjs))
        //string endpoint_validacion = "http://localhost:8080/proy_SW/authv2/auth";
        //string endpoint_verificacion = "http://localhost:5053/verification";
        //string endpoint_registro = "";
        public login(bool adver, string texto = "Su sesion ha expirado, ingrese sus credenciales \n nuevamente")
        {
            InitializeComponent();
            comboBox1.SelectedIndex = 0;
            if (adver)
            {
                label10.Text = texto;
            }
            vis = adver;
            panel3.Visible = vis;
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedIndex==1)
            {
                label1.Text = "¡Cree una cuenta para \n disfrutar de los \n beneficios de \n la aplicacion!";
                pictureBox1.Image = new Bitmap(Properties.Resources.register);
            }
            else
            {
                label1.Text = "Ingrese beneficios\nde la aplicacion aqui\n\nPero para la pantalla\nde login";
                pictureBox1.Image = new Bitmap(Properties.Resources.login);
            }
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            string txtactual = textBox5.Text;

        }

        private async void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                errorProvider1.SetError(label2, "Se debe proveer un correo");
                return;
            }
            else
            {
                errorProvider1.Clear();
            }
            if (textBox2.Text == "")
            {
                errorProvider2.SetError(label3, "Se debe proveer una contraseña");
                return;
            }
            else
            {
                errorProvider2.Clear();
            }

            textBox1.Enabled = false;
            textBox2.Enabled = false;
            
            button1.Text = "...";
            button1.Enabled = false;
            
            string token = await TokenUtils.getToken(textBox1.Text, textBox2.Text);
            bool result = await TokenUtils.validateToken(token);
            if (result)
            {
                
                string[] secc = token.Split('-');
                string sec = secc[2];
                CurrentUser user = new CurrentUser { correo = textBox1.Text, pass=textBox2.Text, nombre = secc[0] };
                Settings.Default["sesion"] =textBox2.Text;
                Settings.Default["id"] = textBox1.Text;
                //descomentar esto al final!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                Settings.Default["AuthToken"] = token;
                Settings.Default.Save();
                switch (sec)
                {
                    case "ventas":
                        ventas frmventas = new ventas(user);
                        frmventas.Show();
                        Close();
                        break;
                    case "almacen":
                        almacen frmalmacen = new almacen(user);
                        frmalmacen.Show();
                        Close();
                        break;
                    default:
                        Application.Exit();
                        break;
                }

            }
            else
            {
                button1.Text = "Ingresar";
                panel3.Visible = true;
                textBox1.Enabled = true;
                textBox2.Enabled = true;
                button1.Enabled = true;
                textBox1.Text = "";
                textBox2.Text = "";
                //MessageBox.Show("no");
            }
        }
        //private async Task<string> getToken(string correo, string pass)
        //{
            
        //    using(var client = new HttpClient())
        //    {
                
        //        var strdata = JsonConvert.SerializeObject(new Validationauth { correo=correo, pass = pass });
        //        var strcontent = new StringContent(strdata, Encoding.UTF8, "application/json");
        //        var response = await client.PostAsync(endpoint_validacion, strcontent);
        //        var res = await response.Content.ReadAsStringAsync();
        //        TokenHandler th = JsonConvert.DeserializeObject<TokenHandler>(res);
        //        if (response.IsSuccessStatusCode)
        //        {
        //            return th.Token;
        //        }
        //        return th.message;
        //    }
        //}
        //private async Task<bool> validateToken(string Token)
        //{
        //    string respuesta = "";
        //    using (var client = new HttpClient())
        //    {
        //        var req = new HttpRequestMessage(HttpMethod.Get, endpoint_verificacion);
        //        req.Headers.Add("authorization", Token);
        //        var contenido = await client.SendAsync(req);

        //        respuesta = await contenido.Content.ReadAsStringAsync();

        //        if (contenido.IsSuccessStatusCode)
        //        {
        //            return true;
        //        }
        //        return false;
        //    }
        //}

        private void login_Load(object sender, EventArgs e)
        {
            //validar si el token es valido aun
            
        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void label11_Click(object sender, EventArgs e)
        {
            //hacer invisible el panel TODO jsjsjsjsjs
            panel3.Visible = false;
        }

        private void login_FormClosing(object sender, FormClosingEventArgs e)
        {
            //if (DialogResult == DialogResult.None)
            //{
            //    Application.Exit();
            //}
            
            //
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            if (validarform_reg())
            {
                errorProvider1.Clear();
            }
            else
            {
                errorProvider1.SetError(button2, "LLene todos los campos antes de proceder");
                return;
            }
            //desactivar botones para evitar multiples peticiones
            
            string[] resp = await TokenUtils.post_nvoUser(textBox4.Text, textBox5.Text, textBox3.Text, comboBox1.SelectedItem.ToString().ToLower());
            MessageBox.Show(resp[1], resp[0]);

        }
        private bool validarform_reg()
        {
            if (textBox3.Text == "" || textBox4.Text == "" || textBox5.Text == "")
            {
                return false;
            }
            return true;
        }
    }
    class Validationauth
    {
        public string correo { get; set; }
        public string pass { get; set; }
    }
    class TokenHandler
    {
        public string Token { get; set; }
        public string message { get; set; }
    }

    public class NvoUser
    {
        public string aplicacion { get; set; }
        public string correo { get; set; }
        public string name { get; set; }
        public string pass { get; set; }
    }

}
