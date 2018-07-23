using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ServiceModel.Test
{
    public class BaseTestClient
    {
        HttpClient _client;
        Uri _clientUri;

        public void SetupClient()
        {
            _client = new HttpClient();
            _clientUri = new Uri("http://businesssupport.bamnuttall.co.uk/api/V3/Projection/");
        }
    }
}
