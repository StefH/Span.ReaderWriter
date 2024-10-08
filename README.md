# Info
SpanReader and SpanWriter which wraps a `Span<byte>` and provide a convenient functionality for reading and writing.

[![NuGet](https://img.shields.io/nuget/v/Span.ReaderWriter)](https://www.nuget.org/packages/Span.ReaderWriter)

### Usage
Read some values from a `Span<byte>`
``` c#
var bytes = new [] { ... }; 
var reader = new SpanReader(bytes);

var @int = reader.ReadInt();
var @long = reader.ReadLong();
```

Write some values to a `Span<byte>` or `byte[]`:
``` c#
var bytes = new byte[16]; // allocate enough space 
var writer = new SpanWriter(bytes);

writer.Write(123);
writer.Write("test");
```

# Credits / References
- [Tedd.SpanUtils](https://github.com/tedd/Tedd.SpanUtils)
- [Microsoft Refence Source](https://referencesource.microsoft.com)
