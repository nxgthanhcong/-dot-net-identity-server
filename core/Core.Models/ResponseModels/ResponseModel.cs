namespace Core.Models.ResponseModels
{
    public class ResponseModel
    {
        public bool IsSucceed { get; set; }
        public object Data { get; set; }
        public string Message { get; set; }

        public static ResponseModel Error =
            new ResponseModel
            {
                Message = "Sever Error",
                Data = new
                {
                    Contact = "nxgthanhcongcommunity@gmail.com"
                }
            };
        public static ResponseModel Succeed(object data)
        {
            return new ResponseModel
            {
                IsSucceed = true,
                Data = data,
            };
        }
        public static ResponseModel Failed(string message, object data = null)
        {
            return new ResponseModel
            {
                IsSucceed = false,
                Data = data,
                Message = message
            };
        }
    }
}
