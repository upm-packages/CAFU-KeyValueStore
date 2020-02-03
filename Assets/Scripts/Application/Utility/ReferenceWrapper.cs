namespace CAFU.KeyValueStore.Application.Utility
{
    public class ReferenceWrapper<T> where T : struct
    {
        private ReferenceWrapper(T value)
        {
            this.value = value;
        }

        private T value;

        private T Value => value;

        public override string ToString()
        {
            return value.ToString();
        }

        public static implicit operator T(ReferenceWrapper<T> wrapper)
        {
            return wrapper.Value;
        }

        public static implicit operator ReferenceWrapper<T>(T value)
        {
            return new ReferenceWrapper<T>(value);
        }
    }

    public static class StructExtension
    {
        public static ReferenceWrapper<T> Wrap<T>(this T value) where T : struct
        {
            return value;
        }

        public static T Unwrap<T>(this ReferenceWrapper<T> wrappedValue) where T : struct
        {
            return wrappedValue;
        }
    }
}