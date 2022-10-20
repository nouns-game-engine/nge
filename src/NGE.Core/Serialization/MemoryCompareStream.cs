using System.Diagnostics;

namespace NGE.Core.Serialization;

public class MemoryCompareStream : Stream
{
    private readonly byte[] comparand;
    private long position;

    public MemoryCompareStream(byte[] comparand)
    {
        this.comparand = comparand;
        position = 0;
    }

    public override void Write(byte[] buffer, int offset, int count)
    {
        for (var i = 0; i < count; i++)
        {
            if (buffer[offset + i] == comparand[position + i])
                continue;

            Debug.Assert(false);
            throw new Exception("Data mismatch");
        }

        position += count;
    }

    public override void WriteByte(byte value)
    {
        if (comparand[position] != value)
        {
            Debug.Assert(false);
            throw new Exception("Data mismatch");
        }

        position++;
    }


    public override bool CanRead => false;
    public override bool CanSeek => true;
    public override bool CanWrite => true;
    public override void Flush() { }
    public override long Length => comparand.Length;
    public override long Position { get => position; set => position = value;
    }
    public override int Read(byte[] buffer, int offset, int count) => throw new InvalidOperationException();

    public override long Seek(long offset, SeekOrigin origin)
    {
        switch (origin)
        {
            case SeekOrigin.Begin:
                position = offset;
                break;
            case SeekOrigin.Current:
                position += offset;
                break;
            case SeekOrigin.End:
                position = comparand.Length - offset;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(origin), origin, null);
        }
        return Position;
    }

    public override void SetLength(long value) => throw new InvalidOperationException();
}