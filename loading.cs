using ClienteWS.Properties;
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
    public partial class loading : Form
    {
        login login;
        //los de yo ((lao sjsjjs))
        //const string endpoint_validacion = "http://localhost:8080/proy_SW/authv2/auth";
        //const string endpoint_verificacion = "http://localhost:5053/verification";
        //const string endpoint_registro = "";
        public loading()
        {
            InitializeComponent();

        }

        private async void loading_Load(object sender, EventArgs e)
        {
            bool RedActiva = System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable();
            if (!RedActiva)
            {
                MessageBox.Show("No se detecto una conexion a internet, conectese y vuelva a intentarlo", "No estas conectado a internet", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }
            string currenttoken = (string)Settings.Default["AuthToken"];
            //MessageBox.Show(currenttoken);
            //if (currenttoken.Equals("o-o-o-o"))
            //{
            //    Console.WriteLine("?asdasdasdasd");
            //    //rediriginr a login sin aviso
            //    login = new login(true, texto: "aaaaa");
            //    login.ShowDialog();
            //    //login.Show();

            //    this.Close();
            //    return;
            //}

            bool valid = await TokenUtils.validateToken(currenttoken);
            if (valid)
            {
                string[] secc = currenttoken.Split('-');
                string sec = secc[2];
                string corr = (string)Settings.Default["id"];
                string pass= (string)Settings.Default["sesion"];

                CurrentUser usr = new CurrentUser { nombre = secc[0], correo=corr, pass=pass };
                switch (sec)
                {
                    case "ventas":
                        ventas frmventas = new ventas(usr);
                        frmventas.Show();
                        Close();
                        break;
                    case "almacen":
                        almacen frmalmacen = new almacen(usr);
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
                bool needsadv=true;
                string acv = "Su sesion ha expirado, ingrese sus credenciales \n nuevamente";
                if (currenttoken.Equals("o-o-o-o"))
                {
                    acv = "??";
                    needsadv = false;
                }
                //masndar a login con advertencia
                login = new login(needsadv, texto: acv);
                login.Show();
                Close();
            }
        }
        

        private void button1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void loading_FormClosing(object sender, FormClosingEventArgs e)
        {
            
        }
    }
}
