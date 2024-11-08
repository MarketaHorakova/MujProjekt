using NUnit.Framework;
using StringCalculatorNamespace;

namespace StringCalculatorTestsNamespace
{
    [TestFixture]
    public class StringCalculatorTests
    {
        private StringCalculator calculator;

        [SetUp]
        public void SetUp()
        {
            calculator = new StringCalculator();
        }

        [Test]
        public void Add_EmptyString_ReturnsZero()
        {
            // Arrange
            var input = "";

            // Act
            var result = calculator.Add(input);

            // Assert
            Assert.AreEqual(0, result);
        }

        
    }
}
