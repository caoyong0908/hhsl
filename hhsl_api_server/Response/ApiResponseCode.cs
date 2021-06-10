namespace hhsl_api_server.Response
{
    public enum ApiResponseCode
    {
        /// <summary>
        /// 成功
        /// </summary>
        Success = 200,
        /// <summary>
        /// 调用失败
        /// </summary>
        Error = 300,
        /// <summary>
        /// 服务器内部错误
        /// </summary>
        ServerError = 500,
    }
}
