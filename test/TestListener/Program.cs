using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace TestListener
{
    static class Program
    {
        static void Main()
        {
            var listener = new HttpListener();
            listener.Prefixes.Add("http://localhost:52222/");

            listener.Start();
            Console.WriteLine("Listening...");

            int i = 0;

            while (true)
            {
                var context = listener.GetContext(); // The GetContext method blocks while waiting for a request.

                // Display request info:

                string documentContents;
                using (Stream inputStream = context.Request.InputStream)
                    using (var reader = new StreamReader(inputStream, Encoding.UTF8))
                        documentContents = reader.ReadToEnd();

                Console.WriteLine();
                Console.WriteLine($"{++i}.");
                Console.WriteLine(DateTime.Now.ToString("O"));
                Console.WriteLine($"{context.Request.HttpMethod} {context.Request.Url}");
                if (!string.IsNullOrWhiteSpace(documentContents))
                    Console.WriteLine(documentContents);

                // Return response:

                string responseString = "<HTML><BODY>All OK.</BODY></HTML>";
                byte[] buffer = Encoding.UTF8.GetBytes(responseString);
                context.Response.ContentLength64 = buffer.Length;
                Stream output = context.Response.OutputStream;
                output.Write(buffer, 0, buffer.Length);
                output.Close();
            }

            //listener.Stop();
        }
    }
}
