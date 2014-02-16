using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NLog.Targets.SignalR.Tests
{
    [TestClass]
    public class PublisherBaseTests
    {
        [TestMethod]
        public void WriteToQueue_Should_Queue_Functions_To_Execute_If_Client_Is_Not_Connected()
        {
            var publisher = new FakePublisher();
            publisher.SetDisconnected();
            publisher.WriteToQueue(SignalRConnector.LogLevel.Debug, "Some Message");

            Assert.IsTrue(publisher.NumberOfFunctionsToExecute == 1);
        }

        [TestMethod]
        public void WriteToQueue_Should_Not_Queue_Functions_To_Execute_If_Client_Is_Connected()
        {
            var publisher = new FakePublisher();
            publisher.SetConnected();
            publisher.WriteToQueue(SignalRConnector.LogLevel.Debug, "Some Message");

            Assert.IsTrue(publisher.NumberOfFunctionsToExecute == 0);
        }

        [TestMethod]
        public void SendMessageToRemoteHost_Should_Call_SendToSignalR()
        {
            var publisher = new FakePublisher();
            publisher.SetConnected();
            publisher.Execute_SendTheMessageToRemoteHost(SignalRConnector.LogLevel.Debug, new[] {"Some Message"});
            Assert.IsTrue(publisher.HasSignalSent);
        }

        [TestMethod]
        public void StartProcessing_Should_Deque_Functions_When_Queue_Has_Any_Function()
        {
            var publisher = new FakePublisher();
            publisher.SetDisconnected();
            publisher.WriteToQueue(SignalRConnector.LogLevel.Debug, "Some Message");
            publisher.WriteToQueue(SignalRConnector.LogLevel.Error, "Some Message");
            publisher.WriteToQueue(SignalRConnector.LogLevel.Warn, "Some Message");
            publisher.WriteToQueue(SignalRConnector.LogLevel.Info, "Some Message");
            Assert.IsTrue(publisher.NumberOfFunctionsToExecute == 4);

            publisher.Execute_StartProcessing();

            Assert.IsTrue(publisher.NumberOfFunctionsToExecute == 0);
        }
    }
}