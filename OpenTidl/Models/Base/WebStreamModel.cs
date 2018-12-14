using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTidl;
using System.IO;
using System.Net;
using System.Net.Http;

namespace OpenTidl.Models.Base
{
    public class WebStreamModel
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
                }
            }
            catch { }

            return null;
        }

        private WebStreamModel()
        {
            
        }

        #endregion
    }
}
