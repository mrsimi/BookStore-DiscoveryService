namespace BookStore_DiscoveryService.DTO.Responses
{
    public class GenericResponse <T>
    {
        public T Data {get; set;}
        public int StatusCode {get; set;}
        public string ResponseMessage {get; set;}
    }
}