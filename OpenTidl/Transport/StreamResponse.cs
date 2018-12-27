/*
    Copyright (C) 2015  Jack Fagner

    This file is part of OpenTidl.

    OpenTidl is free software: you can redistribute it and/or modify
    it under the terms of the GNU Affero General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    OpenTidl is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU Affero General Public License for more details.

    You should have received a copy of the GNU Affero General Public License
    along with OpenTidl.  If not, see <http://www.gnu.org/licenses/>.

    --- 

    Modified 2019 J. Westlake
*/

using System;
using System.IO;

namespace OpenTidl.Transport
{
    public class StreamResponse : IDisposable
    {
        public Stream Stream { get; }
        public string ETag { get; }
        public Int32 StatusCode { get; }
        public Exception Exception { get; }
        public bool IsSuccess { get; }

        public StreamResponse(Stream stream, Int32 statusCode, string etag)
        {
            Stream = stream;
            StatusCode = statusCode;
            ETag = etag;
            IsSuccess = true;
        }

        public StreamResponse(Exception ex)
        {
            Exception = ex;
        }

        public void Dispose()
        {
            Stream?.Dispose();
        }
    }
}
