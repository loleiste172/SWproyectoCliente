using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ClienteWS
{
    internal class TokenUtils
    {
        const string endpoint_validacion = "http://localhost:8080/proy_SW/authv2/auth";
        const string endpoint_verificacion = "http://localhost:5053/verification";
        const string endpoint_registro = "";
        const string carpeta = "2ndtestslim"; //de yo
        //const string carpeta = "WS/p08"; //de zucena

        public static async Task<bool> validateToken(string Token)
        {
            string respuesta = "";
            using (var client = new HttpClient())
            {
                var req = new HttpRequestMessage(HttpMethod.Get, endpoint_verificacion);
                req.Headers.Add("authorization", Token);
                var contenido = await client.SendAsync(req);

                respuesta = await contenido.Content.ReadAsStringAsync();

                if (contenido.IsSuccessStatusCode)
                {
                    return true;
                }
                return false;
            }
        }
        public static async Task<string> getToken(string correo, string pass)
        {
            using (var client = new HttpClient())
            {

                var strdata = JsonConvert.SerializeObject(new Validationauth { correo = correo, pass = pass });
                var strcontent = new StringContent(strdata, Encoding.UTF8, "application/json");
                var response = await client.PostAsync(endpoint_validacion, strcontent);
                var res = await response.Content.ReadAsStringAsync();
                TokenHandler th = JsonConvert.DeserializeObject<TokenHandler>(res);
                if (response.IsSuccessStatusCode)
                {
                    return th.Token;
                }
                return th.message;
            }
        }
        public static async Task<string> get_productos(string user, string pass, string categoria)
        {
            string respuesta = "";
            using (var client = new HttpClient())
            {
                var req = new HttpRequestMessage(HttpMethod.Get, "http://localhost:8080/" + carpeta + "/productos/" + categoria);
                req.Headers.Add("user", user);
                req.Headers.Add("pass", pass);

                var contenido = await client.SendAsync(req);

                respuesta = await contenido.Content.ReadAsStringAsync();
            }
            return respuesta;
        }
        public static async Task<string> get_detalles(string user, string pass, string ISBN)
        {
            string respuesta = "";
            using (var client = new HttpClient())
            {
                var req = new HttpRequestMessage(HttpMethod.Get, "http://localhost:8080/" + carpeta + "/detalles/" + ISBN);
                req.Headers.Add("user", user);
                req.Headers.Add("pass", pass);
                var contenido = await client.SendAsync(req);

                respuesta = await contenido.Content.ReadAsStringAsync();
            }

            return respuesta;
        }

        public static async Task<string> post_producto(string user, string pass, string categoria, string producto)
        {
            string respuesta = "";

            using (var client = new HttpClient())
            {

                HttpRequestMessage request;
                HttpResponseMessage response;

                var endpoint = new Uri("http://localhost:8080/" + carpeta + "/producto");
                request = new HttpRequestMessage(HttpMethod.Post, endpoint);
                var strdata = JsonConvert.SerializeObject(new Body_req { categoria = categoria, producto = producto });
                var strcontent = new StringContent(strdata, Encoding.UTF8, "application/json");
                request.Content = strcontent;
                request.Headers.TryAddWithoutValidation("user", user);
                request.Headers.TryAddWithoutValidation("pass", pass);
                response = await client.SendAsync(request);
                respuesta = await response.Content.ReadAsStringAsync();
            }

            return respuesta;
        }

        public static async Task<string> put_producto(string user, string pass, string clave, string detalles)
        {
            string respuesta = "";
            using (var client = new HttpClient())
            {
                HttpRequestMessage request;
                HttpResponseMessage response;

                var endpoint = new Uri("http://localhost:8080/" + carpeta + "/producto/detalles");
                request = new HttpRequestMessage(HttpMethod.Put, endpoint);
                var strdata = JsonConvert.SerializeObject(new Body_put() { clave = clave, detalles = detalles });
                var strcontent = new StringContent(strdata, Encoding.UTF8, "application/json");
                request.Content = strcontent;
                request.Headers.TryAddWithoutValidation("user", user);
                request.Headers.TryAddWithoutValidation("pass", pass);
                response = await client.SendAsync(request);
                respuesta = await response.Content.ReadAsStringAsync();
            }

            return respuesta;
        }

        public static async Task<string> delete_producto(string user, string pass, string clave)
        {
            string respuesta = "";
            using (var client = new HttpClient())
            {
                HttpRequestMessage request;
                HttpResponseMessage response;

                var endpoint = new Uri("http://localhost:8080/" + carpeta + "/producto");
                request = new HttpRequestMessage(HttpMethod.Delete, endpoint);
                var strdata = JsonConvert.SerializeObject(new Body_delete() { clave = clave });
                var strcontent = new StringContent(strdata, Encoding.UTF8, "application/json");
                request.Content = strcontent;
                //List<NameValueHeaderValue> listheaders = new List<NameValueHeaderValue>();
                //listheaders.Add(new NameValueHeaderValue("user", user));
                //listheaders.Add(new NameValueHeaderValue("pass", pass));
                //foreach (var header in listheaders)
                //{
                //    request.Headers.TryAddWithoutValidation(header.Name, header.Value);
                //}
                request.Headers.TryAddWithoutValidation("user", user);
                request.Headers.TryAddWithoutValidation("pass", pass);
                response = await client.SendAsync(request);
                respuesta = await response.Content.ReadAsStringAsync();
            }

            return respuesta;
        }
        public static bool IsNumeric(object Expression)
        {
            double retNum;

            bool isNum = double.TryParse(Convert.ToString(Expression), System.Globalization.NumberStyles.Any, System.Globalization.NumberFormatInfo.InvariantInfo, out retNum);
            return isNum;
        }
    }
}
