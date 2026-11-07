using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using OpenTrophy.Core;
using UnityEngine.Networking;

namespace OpenTrophy
{
    /// <summary>Sends the requests with UnityWebRequest, so it works on every platform Unity builds for.</summary>
    public sealed class UnityTransport : ITransport
    {
        public int TimeoutSeconds = 15;

        public Task<HttpResult> Send(string method, string url, Dictionary<string, string> headers, string jsonBody)
        {
            var done = new TaskCompletionSource<HttpResult>();
            var request = new UnityWebRequest(url, method);
            request.downloadHandler = new DownloadHandlerBuffer();
            if (jsonBody != null)
            {
                request.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(jsonBody));
                request.SetRequestHeader("Content-Type", "application/json");
            }
            if (headers != null) foreach (var h in headers) request.SetRequestHeader(h.Key, h.Value);
            request.timeout = TimeoutSeconds;
            UnityWebRequestAsyncOperation op = request.SendWebRequest();
            op.completed += (_) =>
            {
                // No answer at all (no network, DNS, timeout) is status 0; an HTTP error keeps its status.
                bool unreachable = request.result == UnityWebRequest.Result.ConnectionError || request.responseCode == 0;
                var result = new HttpResult { Status = unreachable ? 0 : (int)request.responseCode, Body = request.downloadHandler != null ? request.downloadHandler.text : null };
                request.Dispose();
                done.TrySetResult(result);
            };
            return done.Task;
        }
    }
}
