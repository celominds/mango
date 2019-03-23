using Mango.Controllers;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;

namespace Mango.UnitTests
{
    [TestFixture]
    public class MangoUnitTest
    {
       [Test]
        public void RedirectsToIndex_True()
        {
            
            MangoController mangoController = new MangoController();

            var actionResult = mangoController.Index() as ViewResult;

            Assert.AreEqual("Index", actionResult.ViewName);
        }
    }
}