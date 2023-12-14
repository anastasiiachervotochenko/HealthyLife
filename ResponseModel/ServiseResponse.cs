namespace HealthyLife.ResponseModel
{
    public class ServiseResponse<T>
    {
        public T Data { get; set; }
        public bool Completed { get; set; }
        public string Message { get; set; }
    }
}