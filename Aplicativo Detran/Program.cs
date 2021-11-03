using Microsoft.VisualStudio.Services.OAuth;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace HelloWorldApplication
{
    class HelloWorld
    {
        class Program
        {
            private static async Task Main(string[] args)
            {
                Uri urlConsulta = new Uri("https://www.detran.mg.gov.br/habilitacao/prontuario/consultar-pontuacao-cnh/");


                var csrfToken = "";
                var cakephp = "";
                var SERVERIDDETRAN = "";


                using (WebClient client = new WebClient())
                {


                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                    var responseData = client.DownloadData(urlConsulta.ToString());
                    WebHeaderCollection myWebHeaderCollection = client.ResponseHeaders;

                    for (int i = 0; i < myWebHeaderCollection.Count; i++)
                        if (myWebHeaderCollection.GetKey(i).Trim() == "Set-Cookie")
                        {
                            csrfToken = myWebHeaderCollection.Get(i).Substring(myWebHeaderCollection.Get(i).IndexOf("csrfToken") + 10, 128);
                            cakephp = myWebHeaderCollection.Get(i).Substring(myWebHeaderCollection.Get(i).IndexOf("CAKEPHP") + 8, 26);
                            SERVERIDDETRAN = myWebHeaderCollection.Get(i).Substring(myWebHeaderCollection.Get(i).IndexOf("SERVERIDDETRAN") + 15, 14);
                        }


                    var responseString = Encoding.Default.GetString(responseData);
                    var redirectPostToken = responseString.Substring(responseString.IndexOf("Pesquisar</button><div style=\"display:none;\"><input type=\"hidden\" name=\"_Token[fields]\" class=\"form-control\"  autocomplete=\"off\" ") + -1038, 172);
                    var tokenFields = responseString.Substring(responseString.IndexOf("Pesquisar</button><div style=\"display:none;\"><input type=\"hidden\" name=\"_Token[fields]\" class=\"form-control\"  autocomplete=\"off\" ") + 136, 61);

                    var client1 = new RestClient("https://www.detran.mg.gov.br/habilitacao/prontuario/consultar-pontuacao-cnh/exibir-pontuacao-cnh");
                    client1.Timeout = -1;
                    client1.FollowRedirects = false;
                    client1.MaxRedirects = 1000;



                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                    ServicePointManager.SetTcpKeepAlive(true, int.MaxValue, int.MaxValue);

                    var request1 = new RestRequest(Method.POST);

                    request1.AddHeader("Content-Type", " application/x-www-form-urlencoded");
                    request1.AddHeader("Cookie", "csrfToken=" + String.Concat(csrfToken) + "; SERVERIDDETRAN=" + String.Concat(SERVERIDDETRAN) + "; CAKEPHP=" + String.Concat(cakephp, ";"));
                   
                    request1.AddParameter("_method", "POST");
                    request1.AddParameter("_csrfToken", String.Concat(csrfToken));
                    request1.AddParameter("_redirectPostToken", String.Concat(redirectPostToken));
                    request1.AddParameter("cpf", "cpf com '.' e '-'");
                    request1.AddParameter("dataNascimento", "data de nascimento com '/'");
                    request1.AddParameter("dataPrimeiraHabilitacao", "data de nascimento com '/'");
                    request1.AddParameter("_Token[fields]", String.Concat(tokenFields));
                    request1.AddParameter("_Token[unlocked]", "");


                    IRestResponse response1 = client1.Execute(request1);
                    Console.WriteLine(response1.StatusCode);



                }
            }
        }

    }

}
