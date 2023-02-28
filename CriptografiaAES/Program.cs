using System.Security.Cryptography;
using System.IO;
using System.Text;
using System.Net.Mail;
using System.Runtime.CompilerServices;
using System.Text.Json;

namespace CriptografiaAES
{
    internal class Program
    {
        private static string ivBase64 = string.Empty;
        private static string _keyBase64 = "1f0pMyKV4gWvA9SNi3IqYd6KnZ4uIeeORPcB4o/nnLE=";
        private static string clearText = string.Empty;

        static void Main(string[] args)
        {


            do
            {
                Console.WriteLine("Digite o valor a ser criptografado");
                clearText = Console.ReadLine();

            } while (string.IsNullOrWhiteSpace(clearText));

            Console.WriteLine($"Criptografando valor {clearText}");

            var resultado = EncryptAes(clearText, _keyBase64, out ivBase64);
            Console.WriteLine($"Resultado a criptografia: {resultado}");
            Console.WriteLine();

            var resultadoDescriptografia = Decrypt(resultado, _keyBase64, ivBase64);
            Console.WriteLine($"Resultado a descriptografia: {resultadoDescriptografia}");
            Console.WriteLine();


        }

        public static string EncryptAes(string raw, string keyBase64, out string vectorBase64)
        {
            byte[] resultado;

            try
            {
                using (var aes = Aes.Create())
                {
                    aes.Key = Convert.FromBase64String(keyBase64);

                    vectorBase64 = Convert.ToBase64String(aes.IV);

                    //view properties
                    Console.WriteLine($"Criptografando usando o vector: {Convert.ToBase64String(aes.IV)}");
                    Console.WriteLine($"Criptografando usando o mode: {aes.Mode}");
                    Console.WriteLine($"Criptografando usando o padding:{aes.Padding}");
                    Console.WriteLine($"Criptografando usando a KeySize: {aes.KeySize}");
                    Console.WriteLine($"Criptografando usando o BlockSize:{aes.BlockSize}");


                    ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                    using (var ms = new MemoryStream())
                    {
                        using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                        {
                            using (var sw = new StreamWriter(cs))
                            {
                                sw.Write(raw);
                            }

                        }

                        resultado = ms.ToArray();
                    }

                    return Convert.ToBase64String(resultado);
                }
            }
            catch (Exception e)
            {

                throw;
            }
        }


        public static string Decrypt(string cipherText, string keyBase64, string vectorBase64)
        {
            string resultado;


            using (var aes = Aes.Create())
            {
                aes.Key = Convert.FromBase64String(keyBase64);
                aes.IV = Convert.FromBase64String(vectorBase64);


                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                Console.WriteLine($"Descriptografando usando o vector: {Convert.ToBase64String(aes.IV)}");
                Console.WriteLine($"Descriptografando usando o mode: {aes.Mode}");
                Console.WriteLine($"Descriptografando usando o padding:{aes.Padding}");
                Console.WriteLine($"Descriptografando usando a KeySize: {aes.KeySize}");
                Console.WriteLine($"Descriptografando usando o BlockSize:{aes.BlockSize}");

                byte[] cipher = Convert.FromBase64String(cipherText);

                using (MemoryStream ms = new MemoryStream(cipher))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {

                            // Read the decrypted bytes from the decrypting stream
                            // and place them in a string.
                            resultado = srDecrypt.ReadToEnd();
                        }
                    }
                }

                return resultado;
            }
        }
    }

}