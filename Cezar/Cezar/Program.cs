using System;
using System.IO;
using System.Linq;

namespace Cezar
{
    class Program
    {
        static void Main( string [] args )
        {
            try
            {
                CezarCoder coder = new CezarCoder( 5 );
                string str = "qwertyqwertyqwerty";
                Console.WriteLine( $"Start str: {str}" );
                string cryptedStr = coder.CryptStr( str );
                Console.WriteLine( $"Crypted str: {cryptedStr}" );
                string encryptedStr = coder.EncryptStr( cryptedStr );
                Console.WriteLine( $"Encrypted str: {encryptedStr}" );

                Hack( CezarHacker.Encoding.GetBytes( cryptedStr ) );
            }
            catch ( Exception e )
            {
                Console.WriteLine( e.Message );
            }
        }

        static void Hack( byte [] str )
        {
            Console.WriteLine( "Hacking..." );
            byte [] input = str;
            var keys = CezarHacker.GetKeys( input );
            if ( keys.Any() )
            {
                Console.WriteLine( $"Keys found" );
                using ( var fileStream = new FileStream( "output.txt", FileMode.Create ) )
                using ( var streamWriter = new StreamWriter( fileStream, CezarHacker.Encoding ) )
                {
                    var list = keys;
                    for ( int i = 0; i < list.Count; i++ )
                    {
                        string key = CezarHacker.Encoding.GetString( list [ i ] );
                        streamWriter.WriteLine( key );
                    }
                }
            }
            else
            {
                Console.WriteLine( "No keys found!" );
            }
        }
    }
}
