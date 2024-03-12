using Dfinance.Application;
using Dfinance.Core.Views;
using Dfinance.DataModels.Dto;
using Dfinance.Shared.Domain;
using Moq;
using System.Collections;

namespace Dfinance.NUnitTest
{
    public class TransactionAddtionalTest
    {
        private Mock<ITransactionAdditionalsService> _transactionAdditionalsService;
        [SetUp]
        public void Setup()
        {
            _transactionAdditionalsService = new Mock<ITransactionAdditionalsService>();
        }

        [TestCase(34380)]
        [TestCase(1)]
        public void TransactionAddtional_GetTest(int transactionId)
        {
            //Arrange
            _transactionAdditionalsService.Setup(x => x.FillTransactionAdditionals(transactionId)).Returns(new CommonResponse {Exception=null, Data=new SpGetTransactionAdditionals()});

            //Act
            var result = _transactionAdditionalsService.Object.FillTransactionAdditionals(transactionId);
            //Assert
            //Assert.Pass();
            Assert.That(result, Is.Not.Null);
            //Assert.IsNotNull(result);

            Assert.That(result.IsValid,Is.True);

            Assert.That(result.Data,Is.Not.Null);
        }
        [Test]
        public void TransactionAdditionals_SaveTest()
        {
            FiTransactionAdditionalDto transactionAdditionalDto = new FiTransactionAdditionalDto
            {
                AddressLine1 = "test1",
                AddressLine2 = "test2",
                DeliveryNote = "deliveryNote1",
                Attention = "test0",
                TransactionId = 34382,
                MobileNo = "8956234578",
               
            };
            _transactionAdditionalsService.Setup(x => x.SaveTransactionAdditional(transactionAdditionalDto)).Returns(new CommonResponse { Exception = null, Data = new FiTransactionAdditionalDto() });

            //Act
            var result = _transactionAdditionalsService.Object.SaveTransactionAdditional(transactionAdditionalDto);
            Assert.That(result, Is.Not.Null);
            //Assert.IsNotNull(result);

            Assert.That(result.IsValid, Is.True);

            Assert.That(result.Data, Is.Not.Null);
        }
        [Test]
        public void TransactionAdditionals_UpdateTest()
        {
            FiTransactionAdditionalDto transactionAdditionalDto = new FiTransactionAdditionalDto
            {
                AddressLine1 = "test1",
                AddressLine2 = "test2",
                DeliveryNote = "deliveryNote1",
                Attention = "test0",
                TransactionId = 34382,
                MobileNo = "8956234578",

            };
            _transactionAdditionalsService.Setup(x => x.UpdateTransactionAdditional(transactionAdditionalDto)).Returns(new CommonResponse { Exception = null, Data = new FiTransactionAdditionalDto() });

            //Act
            var result = _transactionAdditionalsService.Object.UpdateTransactionAdditional(transactionAdditionalDto);
            Assert.That(result, Is.Not.Null);
            //Assert.IsNotNull(result);

            Assert.That(result.IsValid, Is.True);

            Assert.That(result.Data, Is.Not.Null);
        }
        [TestCase(34382)]
        public void TransactionAddtional_DeleteTest(int transactionId)
        {
            //Arrange
            _transactionAdditionalsService.Setup(x => x.DeleteTransactionAdditional(transactionId)).Returns(new CommonResponse { Exception = null, Data = new FiTransactionAdditionalDto() });

            //Act
            var result = _transactionAdditionalsService.Object.DeleteTransactionAdditional(transactionId);
            //Assert
            //Assert.Pass();
            Assert.That(result, Is.Not.Null);
            //Assert.IsNotNull(result);

            Assert.That(result.IsValid, Is.True);

            Assert.That(result.Data, Is.Not.Null);
        }
    }
}