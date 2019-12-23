using System;
using System.Collections.Generic;
using System.Text;

namespace Cezar
{
    public class CezarHacker
    {
        public static int MaxKeyLength = 5;
        public static readonly Encoding Encoding = Encoding.UTF8;
        private readonly static string _alphabet = "\n\r\"\',.:;!-()ABCDEFGHIJKLMNOPQRSTUVWXYZАБВГДЕЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯ";

        public static List<byte []> GetKeys( byte [] encryptedBytes )
        {
            var result = new List<byte []>();
            for ( int keyLength = 1; keyLength <= MaxKeyLength; ++keyLength ) // found all keys
            {
                var keySet = new List<HashSet<byte>>();
                var isKeysFound = true;

                for ( int keyIndex = 0; keyIndex < keyLength; ++keyIndex ) // found key
                {
                    var shiftSet = new HashSet<byte>();
                    for ( int i = keyIndex; i < encryptedBytes.Length; i += keyLength )
                    {
                        if ( i == keyIndex )
                        {
                            shiftSet.UnionWith( GetPossibleShifts( encryptedBytes [ i ] ) );
                        }
                        else
                        {
                            shiftSet.IntersectWith( GetPossibleShifts( encryptedBytes [ i ] ) );
                        }
                    }

                    if ( shiftSet.Count == 0 )
                    {
                        isKeysFound = false;
                        break;
                    }

                    keySet.Add( shiftSet );
                }

                if ( !isKeysFound )
                    continue;

                var keys = new List<byte []>();
                var stack = new Stack<ByteData>();
                stack.Push( new ByteData { Bytes = new byte [ keyLength ], Length = 0 } );

                while ( stack.Count != 0 )
                {
                    ByteData data = stack.Pop();
                    if ( data.Length == keyLength )
                    {
                        keys.Add( data.Bytes );
                    }
                    else
                    {
                        foreach ( byte possibleShift in keySet [ data.Length ] ) // collect all possible keys
                        {
                            byte [] key = new byte [ keyLength ]; // create key
                            Array.Copy( data.Bytes, key, keyLength ); // copy with current key
                            key [ data.Length ] = possibleShift;
                            stack.Push( new ByteData { Bytes = key, Length = data.Length + 1 } );
                        }
                    }
                }

                result.AddRange( keys );
            }

            return result;
        }

        private static HashSet<byte> GetPossibleShifts( byte encrypted )
        {
            var set = new HashSet<byte>();
            var alphabetBytes = Encoding.GetBytes( _alphabet );
            foreach ( char letter in alphabetBytes )
            {
                int key = encrypted - letter;
                if ( key < 0 )
                {
                    key = 256 + key;
                }

                set.Add( ( byte ) key );
            }

            return set;
        }

        private struct ByteData
        {
            public byte [] Bytes;
            public int Length;
        }
    }
}
