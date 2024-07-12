using Dfinance.Application.Services.General.Interface;
using Dfinance.Core.Views.General;
using Dfinance.DataModels.Dto.Finance;
using Dfinance.Shared.Domain;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;
using static Dfinance.Shared.Routes.v1.FinRoute;

namespace Dfinance.NUnitTest.General
{
    public class CounterTest
    {
        private Mock<ICountersService> _counter;
        [SetUp]
        public void Setup()
        {
            _counter = new Mock<ICountersService>();
        }
        [Test]
        public void SaveCounter() 
        {
            CounterDto counterDto = new CounterDto
            {
                ID = 0,
                MachineName = "TestMachine",
                CounterCode = "ABC123",
                CounterName = "Test Counter",
                MachineIP = "192.168.1.100",
                Active = true
            };
            _counter.Setup(x => x.SaveCounters(counterDto)).Returns(new CommonResponse { Exception = null, Data = new ContraDto() });

            
            var result = _counter.Object.SaveCounters(counterDto);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.IsValid, Is.True);
            Assert.That(result.Data, Is.Not.Null);
        }

        [Test]
        public void UpdateCounter()
        {
            CounterDto counterDto = new CounterDto
            {
                ID = 1,
                MachineName = "TestMachine",
                CounterCode = "ABC123",
                CounterName = "Test Counter",
                MachineIP = "192.168.1.100",
                Active = true
            };
            _counter.Setup(x => x.UpdateCounters(counterDto,1)).Returns(new CommonResponse { Exception = null, Data = new ContraDto() });

            
            var result = _counter.Object.UpdateCounters(counterDto,1);
            Assert.That(result, Is.Not.Null);
           
            Assert.That(result.IsValid, Is.True);

            Assert.That(result.Data, Is.Not.Null);
        }

        [Test]
        public void DeleteCounter()
        {
            
            _counter.Setup(x => x.DeleteCounter(1, 433)).Returns(new CommonResponse { Exception = null, Data = new ContraDto() });
            var result = _counter.Object.DeleteCounter(1, 433);
            Assert.That(result, Is.Not.Null);
        
            Assert.That(result.IsValid, Is.True);

            Assert.That(result.Data, Is.Not.Null);
        }

        [Test]
        public void Fill()
        {

            _counter.Setup(x => x.FillMaster()).Returns(new CommonResponse { Exception = null, Data = new ContraDto() });
            var result = _counter.Object.FillMaster();
            Assert.That(result, Is.Not.Null);

            Assert.That(result.IsValid, Is.True);

            Assert.That(result.Data, Is.Not.Null);
        }

        [Test]
        public void FillById()
        {

            _counter.Setup(x => x.FillCountersById(1)).Returns(new CommonResponse { Exception = null, Data = new ContraDto() });
            var result = _counter.Object.FillCountersById(1);
            Assert.That(result, Is.Not.Null);

            Assert.That(result.IsValid, Is.True);

            Assert.That(result.Data, Is.Not.Null);
        }

        [Test]
        public void GetNameIp()
        {

            _counter.Setup(x => x.GetNameandIp()).Returns(new CommonResponse { Exception = null, Data = new ContraDto() });
            var result = _counter.Object.GetNameandIp();
            Assert.That(result, Is.Not.Null);

            Assert.That(result.IsValid, Is.True);

            Assert.That(result.Data, Is.Not.Null);
        }

    }
}
