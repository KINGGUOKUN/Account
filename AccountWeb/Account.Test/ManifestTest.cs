using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Account.DAL;
using System.Collections.Generic;
using Account.Entity;

namespace Account.Test
{
    [TestClass]
    public class ManifestTest
    {
        [TestMethod]
        public void GetManifest()
        {
            DateTime begin = DateTime.Now.AddYears(-3),
                end = DateTime.Now;
            ManifestDAL dal = new ManifestDAL();
            IEnumerable<Manifest> result = dal.GetManifest(begin, end);
            Assert.IsNotNull(result);
        }
    }
}
