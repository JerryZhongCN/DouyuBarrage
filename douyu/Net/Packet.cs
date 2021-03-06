﻿namespace Douyu.Net
{
    /// <summary>
    /// 包结构
    /// </summary>
    public class Packet
    {
        /// <summary>
        /// 同LengthB
        /// </summary>
        public int LengthA { get; set; }

        /// <summary>
        /// 包长度（不含本身）
        /// </summary>
        public int LengthB { get; set; }

        /// <summary>
        /// 包类型（请求689，回复690） or  (0x2b1,0x2b2)
        /// </summary>
        public int Type { get; set; }

        /// <summary>
        /// 数据
        /// </summary>
        public string Data { get; set; }
    }
}
