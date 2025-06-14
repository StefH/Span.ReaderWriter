using AwesomeAssertions;
using Span.ReaderWriter.Ebml;
using Xunit;

// ReSharper disable once CheckNamespace
namespace System.IO;

public class SpanReaderTests
{
    [Fact]
    public void Read()
    {
        // Arrange
        var dateTime = DateTime.UtcNow;
        bool bo = true;
        char c = 'c';
        char cUtf8 = 'ಸ';
        byte b = 45;
        sbyte sb = 67;
        short s = short.MinValue;
        ushort us = ushort.MaxValue;
        int i = int.MinValue;
        uint ui = uint.MaxValue;
        long l = long.MinValue;
        ulong ul = ulong.MaxValue;
        decimal d = decimal.MinValue;
        float f = 533174.1f;
        double db = double.MaxValue;
        string st = "Hello World";
        string stLong = new string('x', 5000);
        string stUtf8 = "ᚠHello Worldಸ";

        using var memoryStream = new MemoryStream();
        using var binaryWriter = new BinaryWriter(memoryStream);
        binaryWriter.Write(dateTime.ToBinary());
        binaryWriter.Write(bo);
        binaryWriter.Write(c);
        binaryWriter.Write(cUtf8);
        binaryWriter.Write(b);
        binaryWriter.Write(sb);
        binaryWriter.Write(s);
        binaryWriter.Write(us);
        binaryWriter.Write(i);
        binaryWriter.Write(ui);
        binaryWriter.Write(l);
        binaryWriter.Write(ul);
        binaryWriter.Write(d);
        binaryWriter.Write(f);
        binaryWriter.Write(db);
        binaryWriter.Write(st);
        binaryWriter.Write(stLong);
        binaryWriter.Write(stUtf8);

        var bytes = memoryStream.ToArray();

        // Act
        var spanReader = new SpanReader(bytes);

        // Assert
        spanReader.ReadDateTime().Should().Be(dateTime);
        spanReader.ReadBoolean().Should().Be(bo);
        spanReader.ReadChar().Should().Be(c);
        spanReader.ReadChar().Should().Be(cUtf8);
        spanReader.ReadByte().Should().Be(b);
        spanReader.ReadSByte().Should().Be(sb);
        spanReader.ReadShort().Should().Be(s);
        spanReader.ReadUShort().Should().Be(us);
        spanReader.ReadInt().Should().Be(i);
        spanReader.ReadUInt().Should().Be(ui);
        spanReader.ReadLong().Should().Be(l);
        spanReader.ReadULong().Should().Be(ul);
        spanReader.ReadDecimal().Should().Be(d);
        spanReader.ReadSingle().Should().Be(f);
        spanReader.ReadDouble().Should().Be(db);
        spanReader.ReadString().Should().Be(st);
        spanReader.ReadString().Should().Be(stLong);
        spanReader.ReadString().Should().Be(stUtf8);
    }

    [Theory]
    [InlineData(new byte[] { 0x80 }, 1, 0x80ul, 0, "VInt, value = 0, length = 1, encoded = 0x80")]
    [InlineData(new byte[] { 0x81 }, 1, 0x81ul, 1, "VInt, value = 1, length = 1, encoded = 0x81")]
    [InlineData(new byte[] { 0xfe }, 1, 0xfeul, 126, "VInt, value = 126, length = 1, encoded = 0xFE")]
    [InlineData(new byte[] { 0x40, 0x7f }, 2, 0x407ful, 127, "VInt, value = 127, length = 2, encoded = 0x407F")]
    [InlineData(new byte[] { 0x40, 0x80 }, 2, 0x4080ul, 128, "VInt, value = 128, length = 2, encoded = 0x4080")]
    [InlineData(new byte[] { 0x10, 0xDE, 0xFF, 0xAD }, 4, 0x10deffad, 0xdeffad, "VInt, value = 14614445, length = 4, encoded = 0x10DEFFAD")]

    public void TestVInt(byte[] bytes, int expectedLength, ulong expectedEncodedValue, ulong expectedValue, string toString)
    {
        var spanReader = new SpanReader(bytes);
        var vint = spanReader.ReadVInt(4);

        vint.Length.Should().Be(expectedLength);
        vint.EncodedValue.Should().Be(expectedEncodedValue);
        vint.Value.Should().Be(expectedValue);
        vint.ToString().Should().Be(toString);

        var writeSpan = new byte[VInt.GetSize(expectedValue)].AsSpan();
        var spanWriter = new SpanWriter(writeSpan);

        var writeLength = spanWriter.Write(vint);
        writeLength.Should().Be(expectedLength);
        spanWriter.ToArray().Should().BeEquivalentTo(bytes);
    }
}