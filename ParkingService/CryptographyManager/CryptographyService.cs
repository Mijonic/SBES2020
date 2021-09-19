using SecurityManager;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using CryptographyManager.DigitalSignatureFolder;
using System.ServiceModel;
using HashAlgorithm = CryptographyManager.DigitalSignatureFolder.HashAlgorithm;

namespace CryptographyManager
{
	public class CryptographyService<T>
	{
		private static string path1 = @"..\..\..\CryptographyManager\Key\SecretKey.txt";
		private static string fullPath = Path.GetFullPath(path1);

		public static byte[] Encrypt(List<T> data)
		{
			byte[] body = ByteArraySerializer<List<T>>.Serialize(data);

            string eSecretKeyDes = SecretKey.GenerateKey(AlgorithmType.DES);
			SecretKey.StoreKey(eSecretKeyDes, fullPath);
			return DesAlgorithm.EncryptData(body, eSecretKeyDes, CipherMode.CBC);
		}

		public static List<T> Decrypt(KeyValuePair<byte[], byte[]> data)
		{
			string secretKey = SecretKey.LoadKey(fullPath);
            byte[] decrypted = DesAlgorithm.DecryptData(data.Value, secretKey, CipherMode.CBC);

            List<T> lista = ByteArraySerializer<List<T>>.Deserialize(decrypted);

            if (!ValidateData(lista, data.Key))
                return null;

            return lista;
        }

        public static byte[] SignData(List<T> data)
        {
            string signCertCN = Formatter.ParseName(WindowsIdentity.GetCurrent().Name) + "_sign";
            X509Certificate2 certificateSign = ManagerHelper.GetCertificateFromStorage(StoreName.My, StoreLocation.LocalMachine, signCertCN);

            byte[] body = ByteArraySerializer<List<T>>.Serialize(data);

            return DigitalSignature.Sign(body, HashAlgorithm.SHA1, certificateSign);
        }


        public static bool ValidateData(List<T> data, byte[] sign)
        {
            string clienName = Formatter.ParseName(WindowsIdentity.GetCurrent().Name);
            string srvCertCN = "";

            if (clienName.Equals("primary"))
            {
                srvCertCN = "secondary";
            }
            else
            {
                srvCertCN = "primary";
            }

            string clientNameSign = srvCertCN + "_sign";
            X509Certificate2 certificate = ManagerHelper.GetCertificateFromStorage(StoreName.TrustedPeople, StoreLocation.LocalMachine, clientNameSign);

            byte[] body = ByteArraySerializer<List<T>>.Serialize(data);

            if (DigitalSignature.Verify(body, HashAlgorithm.SHA1, sign, certificate))
            {
                Console.WriteLine("Sign is valid");
                return true;
            }
            else
            {
                Console.WriteLine("Sign is invalid");
                return false;
            }

        }
    }
}
