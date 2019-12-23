using System;
using System.Linq;

namespace Cezar
{
    public class CezarCoder
    {
        private int _key;
        private int _counter;

        private readonly int _startKey;
        private readonly int _startCounter;

        private readonly int _limit;
        private readonly int _maxKey;
        private readonly int[] _punctationCodeArr;

        public CezarCoder( int key, int limit = 5, int maxKey = 5 )
        {
            _key = key;
            _startKey = key;
            _startCounter = 1;
            _limit = limit;
            _maxKey = maxKey >= key ? maxKey : key;
            _counter = 1;
            _punctationCodeArr = new int[] { 33, 34, 39, 44, 46, 58, 59 };
        }

        public string CryptStr( string inputStr )
        {
            var result = "";
            _key = _startKey;
            _counter = _startCounter;
            foreach ( var symbol in inputStr )
            {
                result += CryptChar( symbol );
            }
            return result;
        }

        public string EncryptStr( string inputStr )
        {
            _counter = _startCounter;
            _key = _startKey;
            var result = "";
            foreach ( var symbol in inputStr )
            {
                result += EncryptChar( symbol );
            }
            return result;
        }

        private char EncryptChar( char symbol )
        {
            char cryptedChar = ( char )( ( int )symbol - _key );
            UpdateCounter();
            return cryptedChar;
        }

        private char CryptChar( char symbol )
        {
            if ( IsAllowedSymbol( symbol ) )
            {
                char cryptedChar = ( char )( ( int )symbol + _key );
                UpdateCounter();
                return cryptedChar;
            }
            throw new Exception( $"Forbidden symbol: \"{symbol}\"" );
        }

        private bool IsAllowedSymbol( char symbol )
        {
            return !char.IsNumber( symbol ) && !IsRussianLetter( symbol ) && !IsPunctuationMark( symbol ) && ( ( int )symbol != 10 );
        }

        private bool IsRussianLetter( char symbol )
        {
            int code = ( int )symbol;
            return ( code >= 192 && code <= 223 ) || ( code >= 224 && code <= 255 ) || code == 168 || code == 184;
        }

        private bool IsPunctuationMark( char symbol )
        {
            return _punctationCodeArr.Contains( ( int )symbol );
        }

        private void UpdateCounter()
        {
            _counter++;
            if ( _counter > _limit )
            {
                _key++;
                if ( _key > _maxKey )
                {
                    _key = 1;
                }
                _counter = 1;
            }
        }
    }
}
