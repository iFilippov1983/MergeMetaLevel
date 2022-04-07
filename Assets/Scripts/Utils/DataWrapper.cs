namespace Utils
{
    public class DataWrapper<T>
    {
        public bool hasData;

        private T _data;

        public T data
        {
            get => _data;
            set
            {
                _data = value;
                hasData = value != null;
            }
        }

        // public static bool operator ==(DataWrapper<T> self, bool value)
        //     => self.hasData == value;
        //
        // public static bool operator !=(DataWrapper<T> self, bool value)
        //     => self.hasData != value;
        //
        // public static bool operator !(DataWrapper<T> self)
        //     => !self.hasData;
        //
        // public static bool operator true(DataWrapper<T> self)
        //     => self.hasData;
        //
        // public static bool operator false(DataWrapper<T> self)
        //     => self.hasData;
    }
}