using System;
namespace Model
{
    public class MessageModel<T>
    {
        /// <summary>
        /// 状态码
        /// </summary>
        public int Status { get; set; } = 201;

        /// <summary>
        /// 返回信息
        /// </summary>
        public string Msg { get; set; } = "服务器异常";
        /// <summary>
        /// 返回数据集合
        /// </summary>
        public T Response { get; set; }

    }
}

