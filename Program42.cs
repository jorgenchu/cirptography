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
            const int longTextoPlano = 32;
            const int iniVector = 16;

            byte[] clave = iniVec(longClave, 0x00);
            byte[] vI = iniVec(iniVector, 0xA0);

            string nomFichero = nombreFich();

            getClave(clave, longClave);

            using (AesCryptoServiceProvider bloqueAes = new AesCryptoServiceProvider())
            {
                bloqueAes.GenerateKey();

                
                bloqueAes.KeySize = longClave;
                bloqueAes.Key = clave;


                //Generar IV
                bloqueAes.GenerateIV();
                bloqueAes.IV = vI;

                //Modo de relleno
                int caseSwitch = 1;

                switch (caseSwitch)
                {
                    case 1:
                        Console.WriteLine("1 : ANSIX923");
                        bloqueAes.Padding = PaddingMode.ANSIX923;
                        break;
                    case 2:
                        Console.WriteLine("2 : ISO10126");
                        bloqueAes.Padding = PaddingMode.ISO10126;
                        break;
                    case 3:
                        Console.WriteLine("3 : None");
                        bloqueAes.Padding = PaddingMode.None;
                        break;
                    case 4:
                        Console.WriteLine("4 : PKCS7");
                        bloqueAes.Padding = PaddingMode.PKCS7;
                        break;
                    case 5:
                        Console.WriteLine("5 : Zeros");
                        bloqueAes.Padding = PaddingMode.Zeros;
                        break;
                    default:
                        Console.WriteLine("Elige uno valido");
                        break;
                }

                //Modo de cifrado
                string modoCi = modoCifrado();
                if (modoCi.ToLower() == "ecb")
                    bloqueAes.Mode = CipherMode.ECB;

                else if (modoCi.ToLower() == "cbc")
                    bloqueAes.Mode = CipherMode.CBC;
                else
                    Console.WriteLine("Modo de cifrado no valido");

                //Lectura
                byte[] textoDescifrar = new byte[longTextoPlano];
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
        }


        //Perdir Datos
        private static string nombreFich()
        {
            Console.WriteLine("Introduzca el nombre del fichero (con extensión): ");
            string nombreFich = Console.ReadLine();
            return nombreFich;
        }

        static string modoCifrado()
        {
            Console.WriteLine("Introduce modo de cifrado: ");
            string modo = Console.ReadLine();
            return modo;
        }




        static void getClave(byte[] clave, int longClave)
        {
            Console.WriteLine(" Introduce la longitud de la clave (bytes): ");
            longClave = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine(" Introduce la clave: ");
            string claveStr = Console.ReadLine();
            clave = Encoding.UTF8.GetBytes(claveStr); 
           


        }



        static byte[] iniVec(int longitud, byte hexadecimal)
        {
            byte[] sal = new byte[longitud];

            for (int i = 0; i < longitud; i++)
                sal[i] = (byte)(hexadecimal + i);


            return sal;
        }


    }   
}

