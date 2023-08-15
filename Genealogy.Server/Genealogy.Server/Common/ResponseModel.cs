namespace Genealogy.Common
{
    public class ResponseModel
    {
        public int Code { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }

        public ResponseModel()
        {
            Code = 1;
            Message = string.Empty;
            Data = null;
        }
    }
}

