## Info
SpanReader and SpanWriter which wraps a `Span<byte>` and provide a convenient functionality for reading and writing.

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

### Sponsors

[Entity Framework Extensions](https://entityframework-extensions.net/?utm_source=StefH) and [Dapper Plus](https://dapper-plus.net/?utm_source=StefH) are major sponsors and proud to contribute to the development of **Span.ReaderWriter**.

[![Entity Framework Extensions](https://raw.githubusercontent.com/StefH/resources/main/sponsor/entity-framework-extensions-sponsor.png)](https://entityframework-extensions.net/bulk-insert?utm_source=StefH)

[![Dapper Plus](https://raw.githubusercontent.com/StefH/resources/main/sponsor/dapper-plus-sponsor.png)](https://dapper-plus.net/bulk-insert?utm_source=StefH)