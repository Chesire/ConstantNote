using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;

namespace ConstantNote.Classes.Controller
{
    static public class Serializer
    {
        static public void Serialize(string fileName, object objectToSerialize)
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();

            byte[] key = { 1, 6, 11, 12, 21, 1, 25, 5 };
            byte[] iv = { 32, 9, 5, 30, 1, 15, 20, 26 };

            using (Stream stream = File.Open(fileName, FileMode.Create))
            {
                var cryptoStream = new CryptoStream(stream, des.CreateEncryptor(key, iv), CryptoStreamMode.Write);

                binaryFormatter.Serialize(cryptoStream, objectToSerialize);
                cryptoStream.Close();
            }
        }

        static public object Deserialize(string fileName)
        {
            if (!File.Exists(fileName))
            {
                return null;
            }

            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            BinaryFormatter binaryFormatter = new BinaryFormatter();

            object objectName;
            byte[] key = { 1, 6, 11, 12, 21, 1, 25, 5 };
            byte[] iv = { 32, 9, 5, 30, 1, 15, 20, 26 };

            using (Stream stream = File.Open(fileName, FileMode.Open))
            {
                var cryptoStream = new CryptoStream(stream, des.CreateDecryptor(key, iv), CryptoStreamMode.Read);
                objectName = binaryFormatter.Deserialize(cryptoStream);

                cryptoStream.Close();
            }

            return objectName;
        }
    }
}
