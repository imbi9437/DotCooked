namespace Interface
{
    public interface IInitializable
    {
        public bool IsCompleteInitialize { get; set; }
        public void Initialize();
    }

    public interface IInitializable<in T>
    {
        public bool IsCompleteInitialize { get; set; }
        public void Initialize(T data);
    }
}
