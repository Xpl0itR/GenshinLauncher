// Copyright © 2021 Xpl0itR
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System.IO;

namespace GenshinLauncher
{
    public class LengthStream : Stream
    {
        private readonly Stream _stream;
        private readonly long?  _length;

        public LengthStream(Stream stream, long? length)
        {
            _stream = stream;
            _length = length;
        }

        public override bool CanRead  => _stream.CanRead;
        public override bool CanSeek  => _stream.CanSeek;
        public override bool CanWrite => _stream.CanWrite;
        public override long Length   => _length ?? _stream.Length;
        public override long Position
        {
            get => _stream.Position;
            set => _stream.Position = value;
        }

        public override void Flush() =>
            _stream.Flush();

        public override int Read(byte[] buffer, int offset, int count) =>
            _stream.Read(buffer, offset, count);

        public override long Seek(long offset, SeekOrigin origin) =>
            _stream.Seek(offset, origin);

        public override void SetLength(long value) =>
            _stream.SetLength(value);

        public override void Write(byte[] buffer, int offset, int count) =>
            _stream.Write(buffer, offset, count);

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _stream.Dispose();
            }
        }
    }
}