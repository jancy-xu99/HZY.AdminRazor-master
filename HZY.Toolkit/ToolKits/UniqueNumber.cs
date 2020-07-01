using System;

namespace HZY.Toolkit.ToolKits
{
    public class UniqueNumber

    {

        private static long num = 0;//流水号

        private static object lockObj = new object();//锁


         
        /// <summary>

        /// 生成自增长码

        /// </summary>

        /// <returns></returns>

        private static long GenerateUniqueNumber()

        {

            lock (lockObj)//加锁

            {

                num = num + 1;

                num = (num == 100000 ? 1 : num); //如果大于10W则从零开始，由于一台服务器一秒内不太可能有10W并发，所以yymmddhhmmss+num是唯一号。yymmddhhmmss+num+SystemNo针对多台服务器也是唯一号。

            }



            return num;

        }



        /// <summary>

        /// 获取唯一码

        /// </summary>

        /// <param name="SystemNo">系统号</param>

        /// <returns>唯一码</returns>

        public static long GetUniqueNumber(int SystemNo)

        {

            if (SystemNo > 99 || SystemNo < 1)

            {

                throw new Exception("系统号有误");

            }



            lock (lockObj)// 要使静态变量多并发下同步，需要两次加锁。

            {

                string time = DateTime.Now.ToString("yyMMddHHmmss");//12位;

                return long.Parse(time + GenerateUniqueNumber().ToString().PadLeft(5, '0') + SystemNo.ToString().PadLeft(2, '0'));//19位

            }

        }

    }
}
