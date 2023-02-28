// See https://aka.ms/new-console-template for more information
using CriptografiaAssimetriaRSA;

using System.ComponentModel;
using System.Security.Cryptography;
using System.Text;



internal class Program
{
    private static void Main(string[] args)
    {
        Console.WriteLine("Criando a chave");

        Criptografia.CreateRSAKey();


        Criptografia.GetKeyFromContainer(Criptografia.CONTAINER_PUNTO);

        Console.ReadKey();


    }
}