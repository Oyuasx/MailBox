using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace mail
{
    static class İmageToRtf
    {
        private struct RtfFontFamilyDef
        {
            public const string Unknown = @"\fnil";
            public const string Roman = @"\froman";
            public const string Swiss = @"\fswiss";
            public const string Modern = @"\fmodern";
            public const string Script = @"\fscript";
            public const string Decor = @"\fdecor";
            public const string Technical = @"\ftech";
            public const string BiDirect = @"\fbidi";
        }
        private enum EmfToWmfBitsFlags
        {

            // Use the default conversion
            EmfToWmfBitsFlagsDefault = 0x00000000,

            // Embedded the source of the EMF metafiel within the resulting WMF
            // metafile
            EmfToWmfBitsFlagsEmbedEmf = 0x00000001,

            // Place a 22-byte header in the resulting WMF file.  The header is
            // required for the metafile to be considered placeable.
            EmfToWmfBitsFlagsIncludePlaceable = 0x00000002,

            // Don't simulate clipping by using the XOR operator.
            EmfToWmfBitsFlagsNoXORClip = 0x00000004
        };

        public static void InsertImage(this RichTextBox thiss, System.Drawing.Image _image)
        {

            StringBuilder _rtf = new StringBuilder();

            Graphics _graphics = thiss.CreateGraphics();

            var xDpi = _graphics.DpiX;
            var yDpi = _graphics.DpiY;

            _rtf.Append(@"{\rtf1\ansi\ansicpg1252\deff0\deflang1033{\fonttbl{\f0\fnil\fcharset0 Microsoft Sans Serif;}}
\viewkind4\uc1\pard\f0\fs17");

            _rtf.Append(GetImagePrefix(_image, xDpi, yDpi));

            _rtf.Append(GetRtfImage(_image, thiss));

            _rtf.Append(@"}\par}");

            thiss.Select(thiss.TextLength, 0);
            thiss.SelectedRtf = _rtf.ToString();
        }


        private static string GetImagePrefix(System.Drawing.Image _image, float xDpi, float yDpi)
        {

            StringBuilder _rtf = new StringBuilder();

            int picw = (int)Math.Round((_image.Width / xDpi) * 2540);

            int pich = (int)Math.Round((_image.Height / yDpi) * 2540);

            int picwgoal = (int)Math.Round((_image.Width / xDpi) * 1440);

            int pichgoal = (int)Math.Round((_image.Height / yDpi) * 1440);

            _rtf.Append(@"{\pict\wmetafile8");
            _rtf.Append(@"\picw");
            _rtf.Append(picw);
            _rtf.Append(@"\pich");
            _rtf.Append(pich);
            _rtf.Append(@"\picwgoal");
            _rtf.Append(picwgoal);
            _rtf.Append(@"\pichgoal");
            _rtf.Append(pichgoal);
            _rtf.Append(" ");

            return _rtf.ToString();
        }

        [DllImportAttribute("gdiplus.dll")]
        private static extern uint GdipEmfToWmfBits(IntPtr _hEmf, uint _bufferSize,
            byte[] _buffer, int _mappingMode, EmfToWmfBitsFlags _flags);


        private static string GetRtfImage(System.Drawing.Image _image, RichTextBox thiss)
        {

            StringBuilder _rtf = null;

            MemoryStream _stream = null;

            Graphics _graphics = null;

            Metafile _metaFile = null;

            IntPtr _hdc;

            try
            {
                _rtf = new StringBuilder();
                _stream = new MemoryStream();

                using (_graphics = thiss.CreateGraphics())
                {

                    _hdc = _graphics.GetHdc();

                    _metaFile = new Metafile(_stream, _hdc);


                    _graphics.ReleaseHdc(_hdc);
                }

                using (_graphics = Graphics.FromImage(_metaFile))
                {

                    _graphics.DrawImage(_image, new System.Drawing.Rectangle(0, 0, _image.Width, _image.Height));

                }


                IntPtr _hEmf = _metaFile.GetHenhmetafile();


                uint _bufferSize = GdipEmfToWmfBits(_hEmf, 0, null, 8,
                    EmfToWmfBitsFlags.EmfToWmfBitsFlagsDefault);

                byte[] _buffer = new byte[_bufferSize];


                uint _convertedSize = GdipEmfToWmfBits(_hEmf, _bufferSize, _buffer, 8,
                    EmfToWmfBitsFlags.EmfToWmfBitsFlagsDefault);


                for (int i = 0; i < _buffer.Length; ++i)
                {
                    _rtf.Append(String.Format("{0:X2}", _buffer[i]));
                }

                return _rtf.ToString();
            }
            finally
            {
                if (_graphics != null)
                    _graphics.Dispose();
                if (_metaFile != null)
                    _metaFile.Dispose();
                if (_stream != null)
                    _stream.Close();
            }
        }
    }
}
