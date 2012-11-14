using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog.Config;

namespace NLog.Targets.SignalR
{
    [Target("Empty")]
    public class EmptyTarget : TargetWithLayout
    {
        public EmptyTarget()
        {
            this.Host = "localhost";
        }

        [RequiredParameter]
        public string Host { get; set; }


        protected override void Write(Common.AsyncLogEventInfo logEvent)
        {
            base.Write(logEvent);
            var asyncBody = this.Layout.Render(logEvent.LogEvent);
            SendTheMessageToRemoteHost(new[] {asyncBody});
        }

       
        protected override void Write(Common.AsyncLogEventInfo[] logEvents)
        {
            base.Write(logEvents);
            SendTheMessageToRemoteHost(logEvents.Select(x => this.Layout.Render(x.LogEvent)).ToArray());
        }

        protected override void Write(LogEventInfo logEvent)
        {
            base.Write(logEvent);
            SendTheMessageToRemoteHost(new[] {this.Layout.Render(logEvent)});
            
        }

        private void SendTheMessageToRemoteHost(IEnumerable<string> messages)
        {
            Task.Factory.StartNew(() =>
                {
                    var builder = new StringBuilder();
                    foreach (var message in messages)
                    {
                        builder.AppendLine(message);
                    }

                    var allTexts = builder.ToString();

                    System.Diagnostics.Debug.WriteLine(allTexts);
                });
        }
    }
}