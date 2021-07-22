/*
    Copyright (C) 2014 Omega software d.o.o.

    This file is part of Rhetos.

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU Affero General Public License as
    published by the Free Software Foundation, either version 3 of the
    License, or (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU Affero General Public License for more details.

    You should have received a copy of the GNU Affero General Public License
    along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/

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
