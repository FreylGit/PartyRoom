namespace PartyRoom.Domain.Exceptions
{
    public class PartyRoomNullException : Exception
    {
        public PartyRoomNullException() { }
        public PartyRoomNullException(string name) : base(  $"Поступил пустой объект {name} ") { }
        public PartyRoomNullException(string message, Exception innerException) : base(message, innerException) { }
    }
    public class PartyRoomElementAlreadyExistsException : Exception
    {
        public PartyRoomElementAlreadyExistsException()
        {
            
        }
        public PartyRoomElementAlreadyExistsException(string name):base($"Не удалось сохранить {name} из-за того, что такой элемент уже существует")
        {
                
        }
         public PartyRoomElementAlreadyExistsException(string message, Exception innerException) : base(message, innerException) { }
    }
    public class PartyRoomElementSaveExistsException : Exception
    {
        public PartyRoomElementSaveExistsException() { }
        public PartyRoomElementSaveExistsException(string name):base($"Не удалось сохранить {name}") { }
        public PartyRoomElementSaveExistsException(string message, Exception innerException) : base(message, innerException) { }

    }
}
