using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace FAQPhone.Helpers
{
    public class __ProgressableStreamContent: MultipartFormDataContent
    {
        public __ProgressableStreamContent(Action<int, long> progress)
        {
            this.progress = progress;
        }
        const int chunkSize = 4096;
        Action<int, long> progress;
        protected override Task SerializeToStreamAsync(Stream stream, TransportContext context)
        {
            return Task.Run(async () =>
            {
                var buffer = new Byte[chunkSize];
                long size;
                TryComputeLength(out size);
                var uploaded = 0;


                using (var sinput = await this.ReadAsStreamAsync())
                {
                    while (true)
                    {
                        var length = sinput.Read(buffer, 0, buffer.Length);
                        if (length <= 0) break;

                        //downloader.Uploaded = uploaded += length;
                        uploaded += length;
                        progress?.Invoke(uploaded, size);

                        //System.Diagnostics.Debug.WriteLine($"Bytes sent {uploaded} of {size}");

                        stream.Write(buffer, 0, length);
                        stream.Flush();
                    }
                }
                stream.Flush();
            });
        }
    }
}
