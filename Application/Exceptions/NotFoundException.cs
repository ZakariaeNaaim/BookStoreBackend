namespace Application.Exceptions
{
    public class NotFoundException : ApiException
    {
        public NotFoundException() : base() { }

        public NotFoundException(string message) : base(message) { }

        public NotFoundException(string message, params object[] args)
            : base(String.Format(System.Globalization.CultureInfo.CurrentCulture, message, args))
        {
        }

        public NotFoundException(string name, object key)
            : base($"Entity \"{name}\" ({key}) was not found.")
        {
        }
    }
}
