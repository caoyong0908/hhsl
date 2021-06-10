namespace hhsl_api_server.Response
{
    public class ApiResponse
    {
        /// <summary>
        /// 响应码
        /// </summary>
        public ApiResponseCode Code { get; set; } = ApiResponseCode.Success;

        /// <summary>
        /// 消息
        /// </summary>
        public string Msg { get; set; }

        /// <summary>
        /// 数据
        /// </summary>
        public object Data { get; set; }
    }
}
