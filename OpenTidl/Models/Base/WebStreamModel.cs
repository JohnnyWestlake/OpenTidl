using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTidl;
using System.IO;
using System.Net;
using System.Net.Http;
using OpenTidl.Transport;

namespace OpenTidl.Models.Base
{
    public class WebStreamModel : IDisposable
    {
        #region properties

        public Stream Stream { get; private set; }
        public Int64 ContentLength { get; private set; }

        #endregion


        #region methods

        public Byte[] ToArray()
        {
            return this.Stream.ToArray();
        }

        #endregion


        #region construction

        static internal async Task<WebStreamModel> CreateAsync(HttpResponseMessage response)
        {
            try
            {
                if (response != null)
                {
                    var model = new WebStreamModel();
                    model.ContentLength = response.Content.Headers.ContentLength.Value;
                    model.Stream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
                    return model;
                }
            }
            catch { }

            return null;
        }

        static internal WebStreamModel Create(StreamResponse response)
        {
            try
            {
                if (response != null)
                {
                    return new WebStreamModel
                    {
                        ContentLength = response.Stream.Length,
                        Stream = response.Stream
                    };
                }
            }
            catch { }

            return null;
        }

        private WebStreamModel()
        {
            
        }

        #endregion




        public void Dispose()
        {
            this.Stream?.Dispose();
        }
    }
}
