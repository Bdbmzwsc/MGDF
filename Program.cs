using System;
using System.Text;
using System.Net;
using System.Web;
using System.IO;
using System.Security.Cryptography;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Update_MGDF
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("key:");
            try
            {
                string key = ED.Md5Decrypt(Console.ReadLine());
                if (key != "2021/11/11 13:38") return;
            }
            catch
            {
                Console.WriteLine("Error");
                return;
            }
            string an=Console.ReadLine();//App Number
            string jsonstr = GetHttpResponse("https://raw.githubusercontent.com/Bdbmzwsc/MGDF/Application/msg.txt");
            JObject job = JObject.Parse(jsonstr);
            if (an == job["AppNum"].ToString())
            {
                Console.WriteLine("Last Application");//This is the new Application
                return;
            }
            Console.WriteLine("New Application");//Can Update
            string temp=Console.ReadLine();
            if (temp == "N") return;//Input N
            string Path=Console.ReadLine();//Path
            Console.WriteLine(Download_file(job["download_url"].ToString(), Path));
            
         //   string url = Console.ReadLine();
         //   Console.WriteLine(GetHttpResponse(url));
        }
        public static string GetHttpResponse(string url)//Send GET
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
        public static string Download_file(string url, string path)//Download File
        {
            try
            {
                using (var client = new WebClient())
                {
                    client.DownloadFile(url, path);//下载文件
                    return path;
                }

            }
            catch (Exception ex)
            {
                return ex.Message;
            }


        }
    }
}
