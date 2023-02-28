using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection.Metadata;
using System.Runtime.InteropServices.Marshalling;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CriptografiaAssimetriaRSA
{
    public class Criptografia
    {
        public const string CONTAINER_PUNTO = "ContainerChavePunto";

        public static byte[] RSACifra(byte[] dadosEmClaro, RSAParameters rsaInfo, bool isOAEP)
        {
            try
            {
                byte[] dadosCifrados;

                //cria uma nova instancia do RSA provider 
                using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
                {
                    //importa a info da chave RSA
                    //feito apenas para incluir a info da chave publica 
                    rsa.ImportParameters(rsaInfo);

                    //cria o array de bytes e especifica o preenchimento do OAEP
                    dadosCifrados = rsa.Encrypt(dadosEmClaro, isOAEP);
                }

                return dadosCifrados;
            }
            catch (Exception e)
            {

                throw;
            }
        }

        public static byte[] RSADecifra(byte[] dadosCifrados, RSAParameters rsaInfo, bool isOAEP)
        {
            try
            {
                byte[] dadosEmClaro;

                using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
                {
                    rsa.ImportParameters(rsaInfo);

                    dadosEmClaro = rsa.Decrypt(dadosCifrados, isOAEP);
                }

                return dadosEmClaro;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public static void CreateRSAKey()
        {
            var parameters = new CspParameters()
            {
                KeyContainerName = CONTAINER_PUNTO
                
            };

            using var rsa = new RSACryptoServiceProvider(parameters);
            
            Console.WriteLine($"Key adicionada ao container: \n {rsa.ToXmlString(true)}");

            if (!Directory.Exists(@"c:\temp\Criptografia"))
                Directory.CreateDirectory(@"C:\temp\Criptografia");

            var chavePrivada = rsa.ExportRSAPublicKeyPem();
            File.WriteAllText(@"c:\temp\Criptografia\PrivateKeyPunto.pem", chavePrivada);

            var chavePublica = rsa.ExportRSAPrivateKeyPem();
            File.WriteAllText(@"c:\temp\Criptografia\PublicKeyPunto.pem", chavePublica);

        }

        public static void GetKeyFromContainer(string containerName)
        {
            var parameters = new CspParameters
            {
                KeyContainerName = containerName
            };

            using var rsa = new RSACryptoServiceProvider(parameters);

            Console.WriteLine($"Key {rsa.ToXmlString(true)} recuperada do container: {containerName}");
        }

        public static void DeleteKeyFromContainer(string containerName)
        {
            var parameters = new CspParameters()
            {
                KeyContainerName = containerName
            };

            using var rsa = new RSACryptoServiceProvider(parameters)
            {
                // Delete the key entry in the container.
                PersistKeyInCsp = false
            };

            rsa.Clear();

            Console.WriteLine("Key foi apagada");
        }
    }
}
