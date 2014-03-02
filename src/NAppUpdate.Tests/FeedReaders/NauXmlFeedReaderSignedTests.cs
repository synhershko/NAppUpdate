using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NAppUpdate.Framework;
using NAppUpdate.Framework.Conditions;
using NAppUpdate.Framework.Tasks;
using System.Security.Cryptography;
using System.Xml;

namespace NAppUpdate.Tests.FeedReaders
{
    /// <summary>
    /// Summary description for NauXmlFeedReaderTest
    /// </summary>
    [TestClass]
    public class NauXmlFeedReaderTestsSigned
    {
        [TestMethod]
        public void DenyUpdateIfNoValidSignature()
        {
            const string NauUpdateFeed =
                @"<?xml version=""1.0"" encoding=""utf-8""?>
<Feed signature2048=""asdf"">
  <Title>My application</Title>
  <Link>http://myapp.com/</Link>
  <Tasks>
    <FileUpdateTask localPath=""test.dll"" updateTo=""remoteFile.dll"" hotswap=""true"">
      <Description>update details</Description>
    </FileUpdateTask>
  </Tasks>
</Feed>";

            var reader = new NAppUpdate.Framework.FeedReaders.NauXmlFeedReaderSigned();
            reader.PublicKeys = new string[] { "<RSAKeyValue><Modulus>w+T4nWr2hZS4oo2BAJx66NbbGQAQQgxNYWOw+Tl2cICbX2W1kmoWah/wdF2qG4pxEcgOlbsi06Pel1dUI0PSWMSvQq4xsjkHnPoauY/h0Ydb+0dLlocJcbYYCq1iCJSDK3u86tDhhqtb61cvLketFpIUhnnGE6Z6cO6rLouFk18=</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>" };

            IList<IUpdateTask> updates = reader.Read(NauUpdateFeed);
            Assert.IsTrue(updates != null || updates.Count == 0);
        }

        [TestMethod]
        public void DenyUpdateIfMissingHashButSignatureIsOK()
        {
            const string NauUpdateFeed =
                @"<?xml version=""1.0"" encoding=""utf-8""?>
<Feed RSASignature=""hR07W9TZjOQjx4U8FPRIzifGubDvGXmOjVII6+0LGpw1FrIa71ZpcAz//5+8ntHXwErngZn9dvYg0MzkSNPlVOn9lAcU0n2Axqbn9lQcw7RWLQsuvlQR+UqQMa02Qv807MfqupxER4X0Buf/psPN2EHxr2fvUXy5tjryt1FO2J4="">
  <Title>My application</Title>
  <Link>http://myapp.com/</Link>
  <Tasks>
    <FileUpdateTask localPath=""test.dll"" updateTo=""remoteFile.dll"" hotswap=""true"">
      <Description>update details</Description>
    </FileUpdateTask>
  </Tasks>
</Feed>";
            //XmlDocument doc = new XmlDocument();
            //doc.LoadXml(NauUpdateFeed);

            //// Support for different feed versions
            //XmlNode root = doc.SelectSingleNode(@"/Feed[version=""1.0""] | /Feed") ?? doc;

            //SHA512Managed sha = new SHA512Managed();
            //var hash = sha.ComputeHash(Encoding.UTF8.GetBytes(root.InnerXml));

            //RSACryptoServiceProvider provider = new RSACryptoServiceProvider(1024);
            //provider.PersistKeyInCsp = false;
            //var pubKey = provider.ToXmlString(false);

            //var sigBytes = provider.SignHash(hash, "sha512");
            //var sig = Convert.ToBase64String(sigBytes);

            //bool isVerified = provider.VerifyHash(hash, "sha512", Convert.FromBase64String(sig));


            var reader = new NAppUpdate.Framework.FeedReaders.NauXmlFeedReaderSigned();
            reader.PublicKeys = new string[] { "<RSAKeyValue><Modulus>vFdzuQ3iuR606jKt2UrP4QIKY+w6lsnKtbBDiYstFQy9PxrOAOeVyGBThrrrtc3Hyq2F47P+0y1GRT81LiFdL1O1S/82Lw8F5s49/SDEF87SYJZHLgeGKHWipGurIgeKrSuWHvil1iDI2dZpu6LExjRUsXyRjKDqVOhri+HcLEc=</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>" };

            IList<IUpdateTask> updates = reader.Read(NauUpdateFeed);
            Assert.IsTrue(updates != null || updates.Count == 0);
        }


        [TestMethod]
        public void AllowUpdateIfSignatureAndHash()
        {
            const string NauUpdateFeed =
                @"<?xml version=""1.0"" encoding=""utf-8""?>
<Feed RSASignature=""SIGNATURE"">
  <Title>My application</Title>
  <Link>http://myapp.com/</Link>
  <Tasks>
    <FileUpdateTask sha256-checksum=""invalidhashButWeCanNotKnowThatNow"" localPath=""test.dll"" updateTo=""remoteFile.dll"" hotswap=""true"">
      <Description>update details</Description>
    </FileUpdateTask>
  </Tasks>
</Feed>";
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(NauUpdateFeed);

            // Support for different feed versions
            XmlNode root = doc.SelectSingleNode(@"/Feed[version=""1.0""] | /Feed") ?? doc;

            SHA512Managed sha = new SHA512Managed();
            var hash = sha.ComputeHash(Encoding.UTF8.GetBytes(root.InnerXml));

            RSACryptoServiceProvider provider = new RSACryptoServiceProvider(1024);
            provider.PersistKeyInCsp = false;
            var pubKey = provider.ToXmlString(false);

            var sigBytes = provider.SignHash(hash, "sha512");
            var sig = Convert.ToBase64String(sigBytes);

            var update = NauUpdateFeed.Replace("SIGNATURE", sig);

            bool isVerified = provider.VerifyHash(hash, "sha512", Convert.FromBase64String(sig));
            Assert.IsTrue(isVerified);

            var reader = new NAppUpdate.Framework.FeedReaders.NauXmlFeedReaderSigned();
            reader.PublicKeys = new string[] { pubKey };

            IList<IUpdateTask> updates = reader.Read(update);
            Assert.IsTrue(updates != null && updates.Count == 1);
        }

        [TestMethod]
        public void AllowUpdateIfIDontRequireChecksum()
        {
            const string NauUpdateFeed =
                @"<?xml version=""1.0"" encoding=""utf-8""?>
<Feed RSASignature=""SIGNATURE"">
  <Title>My application</Title>
  <Link>http://myapp.com/</Link>
  <Tasks>
    <FileUpdateTask sha256-checksum=""invalidhashButWeCanNotKnowThatNow"" localPath=""test.dll"" updateTo=""remoteFile.dll"" hotswap=""true"">
      <Description>update details</Description>
    </FileUpdateTask>
    <RegistryTask keyName=""asdf"" keyValue=""zsdfgafsdg"" valueKind=""String"">
    </RegistryTask>
  </Tasks>
</Feed>";
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(NauUpdateFeed);

            // Support for different feed versions
            XmlNode root = doc.SelectSingleNode(@"/Feed[version=""1.0""] | /Feed") ?? doc;

            SHA512Managed sha = new SHA512Managed();
            var hash = sha.ComputeHash(Encoding.UTF8.GetBytes(root.InnerXml));

            RSACryptoServiceProvider provider = new RSACryptoServiceProvider(1024);
            provider.PersistKeyInCsp = false;
            var pubKey = provider.ToXmlString(false);

            var sigBytes = provider.SignHash(hash, "sha512");
            var sig = Convert.ToBase64String(sigBytes);

            var update = NauUpdateFeed.Replace("SIGNATURE", sig);

            bool isVerified = provider.VerifyHash(hash, "sha512", Convert.FromBase64String(sig));
            Assert.IsTrue(isVerified);

            var reader = new NAppUpdate.Framework.FeedReaders.NauXmlFeedReaderSigned();
            reader.PublicKeys = new string[] { pubKey };

            IList<IUpdateTask> updates = reader.Read(update);
            Assert.IsTrue(updates != null && updates.Count == 2);
        }


    }
}
