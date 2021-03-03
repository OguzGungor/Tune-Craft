using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NUnit.Framework;
using TuneAndCraft.v5.Controllers;
using System.Web.Mvc;
using TuneAndCraft.v5.Models;

namespace TuneAndCraft.v5.Test
{
    [TestFixture]
    public class Test
    {

        [Test]
        public void testHomeIndex() {

            var controller = new HomeController();

            var actionResult = controller.Index().Result as ViewResult;

            Assert.That(actionResult.ViewName, Is.EqualTo("Index2"));        
        
        }

    }
}