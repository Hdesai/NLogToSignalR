using System.Collections.Generic;
using System.Threading.Tasks;

namespace NLog.Targets.SignalR.Tests
{
    public class FakePublisher : PublisherBase
    {

        public bool HasSignalSent = false;
        public bool HasProcessingStarted = false;
        public bool HasMessageToRemoteHostSent = false;

        public void SetConnected()
        {
            this.IsConnected = true;
        }

        public void SetDisconnected()
        {
            this.IsConnected = false;
        }

        public int NumberOfFunctionsToExecute
        {
            get { return this.FunctionsToExecute.Count; }
        }

        protected override void SendToSignalR(Message message)
        {
            HasSignalSent = true;

        }

        protected override void StartProcessing()
        {
            base.StartProcessing();
            HasProcessingStarted = true;
        }

        protected override void SendTheMessageToRemoteHost(LogLevel level, System.Collections.Generic.IEnumerable<string> messages)
        {
            base.SendTheMessageToRemoteHost(level, messages);
            HasMessageToRemoteHostSent = true;

        }

        public void Execute_SendTheMessageToRemoteHost(LogLevel level,
                                                       IEnumerable<string> messages)
        {
            var task=this.GetSendTheMessageToRemoteHostTask(level,messages);
            task.Start();
            task.Wait();
        }

        public void Execute_StartProcessing()
        {
            this.StartProcessing();
        }

    }
}