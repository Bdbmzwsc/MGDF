using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Text.RegularExpressions;
using System.Net;
using System.IO;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Web;

namespace MGDF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string oldurl = "";
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            oldurl = URL.Text;
            URL.Text = URL.Text.Substring(URL.Text.Length-(URL.Text.Length-19),URL.Text.Length-19);
            string[] temp = URL.Text.Split("/".ToCharArray());
            string username = temp[0];
            string gitname = temp[1];

            Regex reg = new Regex(temp[0] + "/" + temp[1] + "/tree/(.+)/");
            string t1 = reg.Replace(URL.Text,"");
            string newurl = "https://api.github.com/repos/" + username + "/" + gitname + "/contents/" + t1;
            URL.Text = newurl;
            // MessageBox.Show(newurl);
            string jsonstr = GetHttpResponse(newurl);
          //  MessageBox.Show(jsonstr);
            JArray jar = JArray.Parse(jsonstr);
            string path = AppDomain.CurrentDomain.BaseDirectory + "\\";
            Directory.CreateDirectory(path + "public\\");
            for (int i = 0; i <= jar.Count - 1; i++)
            {
                //MessageBox.Show(jar[i].ToString());
                JObject job = JObject.Parse(jar[i].ToString());
            //   downloadurl.Items.Add(job["download_url"]);
                if (job["type"].ToString() == "file")
                {
                    downloadurl.Items.Add(job["name"].ToString() + " download");
                    string dp = path + job["path"].ToString().Replace("/", "\\");
                    downloadurl.Items.Add(Download_file(job["download_url"].ToString(), dp));
                }
            }
        }
        public static string GetHttpResponse(string url)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

            request.Method = "GET";

            //    request.ContentType = "text/html;charset=UTF-8";
            request.ContentType = "application/json; charset=utf-8";
            request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/95.0.4638.54 Safari/537.36 Edg/95.0.1020.40";
            request.CookieContainer = new CookieContainer();

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            Stream myResponseStream = response.GetResponseStream();

            StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.UTF8);

            string retString = myStreamReader.ReadToEnd();

            myStreamReader.Close();

            myResponseStream.Close();
            return retString;
        }

        private void Test_Click(object sender, RoutedEventArgs e)
        {
            string[] temp = URL.Text.Split("/".ToCharArray());
            MessageBox.Show(temp[temp.Length - 1]);


        }

        private void downloadurl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            download_u.Text = downloadurl.SelectedItem.ToString();
            
        }
        public string Download_file(string url,string path)
        {
            try
            {
                using (var client = new WebClient())
                {
                    client.DownloadFile(url, path);//下载文件
                    return path;
                }
            
            }
            catch(Exception ex)
            {
                return ex.Message;
            }
            

        }
    }
}
