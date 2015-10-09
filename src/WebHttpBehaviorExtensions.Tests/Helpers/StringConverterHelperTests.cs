using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebHttpBehaviorExtensions.Helpers;

namespace WebHttpBehaviorExtensions.Tests.Helpers
{
    [TestFixture]
    public class StringConverterHelperTests
    {
        [Test]
        public void NumericString_ConvertToInt_Valid()
        {
            long numericTestValue = 29378932;

            var result = numericTestValue.ToString().To<Int32>();
            Console.Write(result.GetType());

            Assert.AreEqual(numericTestValue, result);
        }

        [Test]
        public void NumericString_ConvertValueNullableInteger_Valid()
        {
            int? nullableTestValue = 12323;

            var result = nullableTestValue.Value.ToString().To<Nullable<Int32>>();

            Assert.AreEqual(nullableTestValue, result);
            Assert.IsInstanceOf<Nullable<Int32>>(result);
        }

        [Test]
        public void GuidString_ConvertToGuidType_Valid()
        {
            var guid = Guid.NewGuid();

            var result = guid.ToString().To<Guid>();

            Assert.AreEqual(guid, result);
        }

        [Test]
        public void BooleanString_ConvertToBool_Valid()
        {
            bool expectedValue = false;
            string testInput = "false";

            var result = testInput.To<Boolean>();

            Assert.AreEqual(expectedValue, result);
        }

        [Test]
        public void EnumReresentedByString_ConvertToEnum_Valid()
        {
            TestState expectedValue = TestState.Success;
            string testInput = "success";

            var result = testInput.To<TestState>();

            Assert.AreEqual(expectedValue, result);
        }

        [Test]
        public void DateReresentedByString_ConvertToDateTimeObject_Valid()
        {
            string testInput = "08/01/2015";

            var result = testInput.To<DateTime>();

            Assert.AreEqual(testInput, result.ToString("MM/dd/yyyy"));
        }
    }

    internal static class TestHelperExtensions
    {
        public static T To<T>(this object source)
        {
            object result = source;

            source.TryConvertTo(typeof(T), out result);

            return (T)result;
        }
    }
}
