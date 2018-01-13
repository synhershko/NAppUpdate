using System;
using System.Collections.Generic;
using System.Xml;

using NAppUpdate.Framework.Tasks;
using NAppUpdate.Framework.Conditions;
using System.Security.Cryptography;
using System.Text;

namespace NAppUpdate.Framework.FeedReaders
{
    public class NauXmlFeedReaderSigned : NauXmlFeedReader
    {
        /// <summary>
        /// The public keys that should be used to verify signature of the feed. More than one key is allowed in order to support backup keys.
        /// </summary>
        public string[] PublicKeys { get; set; }

        /// <summary>
        /// Feed should have an attribute with this name containing the signature
        /// </summary>
        private const string sigName = "RSASignature";


        public new IList<IUpdateTask> Read(string feed)
        {
            var nothing = new List<IUpdateTask>();
            var ret = base.Read(feed);
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(feed);

            // Support for different feed versions
            XmlNode root = doc.SelectSingleNode(@"/Feed[version=""1.0""] | /Feed") ?? doc;


            if (PublicKeys != null && PublicKeys.Length > 0)
            {
                if (!IsValidSignature(root, PublicKeys))
                {
                    return nothing;
                }
                foreach (var t in ret)
                {
                    if (!((t is IDontRequireChecksum) ||
                        ((t is IHaveSha256Checksum || t is IHaveSha512Checksum) &&
                        ( t is IHaveSha256Checksum && !string.IsNullOrEmpty((t as IHaveSha256Checksum).Sha256Checksum)) ||
                        (t is IHaveSha512Checksum && !string.IsNullOrEmpty((t as IHaveSha512Checksum).Sha512Checksum))
                        )
                        ))
                        return nothing;
                }
            }
            return ret;
        }

        private bool IsValidSignature(XmlNode root, string[] publicKey)
        {
            if (publicKey == null || publicKey.Length == 0 || root == null || string.IsNullOrEmpty(root.InnerXml))
            {
                return false;
            }
            if (root.Attributes[sigName] == null || string.IsNullOrEmpty(root.Attributes[sigName].Value))
            {
                return false;
            }
            SHA512Managed sha = new SHA512Managed();
            var hash = sha.ComputeHash(Encoding.UTF8.GetBytes(root.InnerXml));

            foreach (var pk in publicKey)
            {
                string sig = root.Attributes[sigName].Value;
                if (!string.IsNullOrEmpty(pk))
                {
                    bool isVerified = VerifyHash(hash, pk, sig);
                    if (!isVerified)
                    {
                        continue;
                    }
                    else
                    {
                        return true;
                    }
                }
                else
                {
                    return false;
                }
            }
            return false;
        }

        private static bool VerifyHash(Byte[] hash, string publicKey, string sig)
        {
            RSACryptoServiceProvider provider = new RSACryptoServiceProvider();
            provider.PersistKeyInCsp = false;
            try
            {
                provider.FromXmlString(publicKey);
            }
            catch (Exception)
            {
                throw new FeedReaderException("Could not read public key. It needs to have an xml format, as required by RSACryptoServiceProvider");
            }

            if (!provider.PublicOnly)
            {
                throw new FeedReaderException("The given public key contains both private and public parts. Please dont publish your private keys publicly.");
            }
            return provider.VerifyHash(hash, "sha512", Convert.FromBase64String(sig));
        }
    }
}
