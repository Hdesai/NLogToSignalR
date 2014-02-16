using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR.Client.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using SignalRConnector;

namespace NLog.Targets.SignalR.Tests
{
    [TestClass]
    public class PersistentConnectionPublisherTests
    {
        private IConnectionProxy _connectionProxyMock;
        private IHttpClient _httpClientMock;
        private IResponse _responseMock;

        [TestInitialize]
        public void StartTest()
        {
            _connectionProxyMock = Substitute.For<IConnectionProxy>();
            _httpClientMock = Substitute.For<IHttpClient>();
            _responseMock = Substitute.For<IResponse>();
        }

        [TestMethod]
        public void Connect_Should_Set_IsConnected_To_True_When_Start_Task_Returns_Success()
        {
            var persistentConnectionPublisher = new PersistentConnectionPublisher(_connectionProxyMock);

            _connectionProxyMock.StartConnection(_httpClientMock).Returns(info => Task.FromResult(true));

            persistentConnectionPublisher.Connect(_httpClientMock);

            Assert.IsTrue(persistentConnectionPublisher.IsConnected);
        }

        
        [TestMethod]
        public void Connect_Should_Not_Set_IsConnected_To_True_When_Start_Task_Returns_Failure()
         {
             var persistentConnectionPublisher = new PersistentConnectionPublisher(_connectionProxyMock);

             _connectionProxyMock.StartConnection(_httpClientMock).Returns(info => {throw new Exception("test");});

             try
             {
                 persistentConnectionPublisher.Connect(_httpClientMock);
             }
             catch
             {
                 
             }

             Assert.IsFalse(persistentConnectionPublisher.IsConnected);
         }

        [TestMethod]
        public void SendToSignalR_Should_Set_SentToSignalR_Flag_To_True_On_Success()
        {
            //Arrange
            var publisher = new FakePersistentConnectionPublisher(_connectionProxyMock);
            _connectionProxyMock.StartConnection(_httpClientMock).Returns(info => Task.FromResult(true));

            //Act
            publisher.Execute_SendToSignalR(new Message());

            Assert.IsTrue(publisher.SentToSignalR);
        }

        
    }

    public class FakePersistentConnectionPublisher:PersistentConnectionPublisher
    {
        public FakePersistentConnectionPublisher(IConnectionProxy connectionProxy) : base(connectionProxy)
        {
        }

        public void Execute_SendToSignalR(Message message)
        {
            this.SendToSignalR(message);
        }
    }
}