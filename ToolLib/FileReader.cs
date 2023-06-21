namespace ToolLib;

public abstract class FileReader<T, TU> where T : class where TU : class, new()
{
    protected virtual byte[] Magic { get; set; } = Array.Empty<byte>();

    protected readonly BinaryReader Reader;
    protected readonly TU Options;

    protected FileReader(BinaryReader reader, TU? options)
    {
        Reader = reader;
        Options = options ?? new TU();

        if (!CheckMagic())
        {
            throw new InvalidDataException("Magic Mismatch");
        }
    }

    public static T Open(Stream stream, TU? options = null)
    {
            
        var reader = new BinaryReader(stream);
        if (Activator.CreateInstance(typeof(T), reader, options) is not T instance)
        {
            throw new InvalidCastException();
        }

        return instance;
    }

    public static T Open(byte[] bytes, TU? options = null)
    {
        if (bytes.Length == 0)
        {
            throw new InvalidDataException("Zero Bytes");
        }

        return Open(new MemoryStream(bytes), options);
    }

    public static T Open(string filename, TU? options =null)
    {
        if (!File.Exists(filename))
        {
            throw new FileNotFoundException();
        }

        return Open(File.ReadAllBytes(filename), options);
    }

    private bool CheckMagic()
    {
        return Reader.BaseStream.Position + Magic.Length < Reader.BaseStream.Length && Reader.ReadBytes(Magic.Length).SequenceEqual(Magic);
    }
}
