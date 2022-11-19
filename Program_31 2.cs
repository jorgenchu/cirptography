using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace projpractica2
{
    class Program
    {
        static void Main(string[] args)
        {
            const int longClave = 16;
            const int longTextoPlano = 16;
            const int iniVector = 16;

            byte[] clave = iniVec(longClave, 0x00);
            byte[] vI = iniVec(iniVector, 0xA0);
            byte[] textoPlano = iniVec(longTextoPlano, 0xFF);

            using (AesCryptoServiceProvider bloqueAes = new AesCryptoServiceProvider())
            {
                Console.WriteLine("BlockSize: {0} ", bloqueAes.BlockSize);
                Console.WriteLine("KeySize: {0}", bloqueAes.KeySize);
                Console.WriteLine("Padding: {0}", bloqueAes.Padding);
                Console.WriteLine("Mode: {0}", bloqueAes.Mode);

                bloqueAes.GenerateKey();
                Console.WriteLine("Clave aleatoria" + BitConverter.ToString(bloqueAes.Key));//Clave aleatoria

                bloqueAes.KeySize = 128;
                bloqueAes.Key = clave;
                Console.WriteLine("Clave Inicio:" + BitConverter.ToString(bloqueAes.Key));//Clave generada al principio
                Console.WriteLine("Clave creada al principio : " + BitConverter.ToString(clave) + " Comprobacion");
                //Generar IV

                bloqueAes.GenerateIV();
                Console.WriteLine("Vector IV: " + BitConverter.ToString(bloqueAes.IV));

                bloqueAes.IV = vI;
                Console.WriteLine("Vector IV: " + BitConverter.ToString(bloqueAes.IV) + " Creado inicio");
                Console.WriteLine("Vector IV: " + BitConverter.ToString(vI) + " Comprobación");



                using (FileStream outputFile = new FileStream("UO285762_cifrado.bin", FileMode.Create))
                {
                    using (ICryptoTransform iC = bloqueAes.CreateEncryptor())
                    {
                        using (CryptoStream csStream = new CryptoStream(outputFile, iC , CryptoStreamMode.Write))
                        {
                            csStream.Write(textoPlano, 0, textoPlano.Length);
                            csStream.Flush();
                        }
                    }
                }

                byte[] textoDescifrar = new byte[16];
                using (FileStream inputFile = new FileStream("UO285762_cifrado.bin", FileMode.Open))
                {
                    using (ICryptoTransform iCdes = bloqueAes.CreateDecryptor())
                    {
                        CryptoStream cStream = new CryptoStream(inputFile, iCdes, CryptoStreamMode.Read);
                        cStream.Read(textoDescifrar, 0, textoDescifrar.Length);
                        Console.WriteLine("Mensaje descifrado: " + BitConverter.ToString(textoDescifrar));
                    }
                }
            }

            //3.2

           



        }
        static byte[] iniVec(int longitud, byte hexadecimal)//
        {
            byte[] sal = new byte[longitud];

            for (int i = 0; i < longitud; i++)
                sal[i] = (byte)(hexadecimal + i);

            return sal;
        }
    }
}

    