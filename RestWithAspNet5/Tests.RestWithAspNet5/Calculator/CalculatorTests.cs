using Business;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Tests.RestWithAspNet5.Calculator
{
    public class CalculatorTests
    {
        [Fact]
        public void Sum()
        {
            var number = CalculatorBusiness.Sum("2","3");
            Assert.Equal(5, number);
        }
    }
}
