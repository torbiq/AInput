public interface IRange<T> {
    T min { get; }
    T max { get; }
    T Get();
}