using auth.Interfaces;
using System.Collections;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace auth.Services
{
    public class NganLuongService : INganLuongService
    {
        private const string nganLuongURL = "https://www.nganluong.vn/checkout.php";
        private const string merchantSiteCode = "36680";
        private const string securePass = "matkhauketnoi";
        private string receiver = "demo@nganluong.vn";
        private string returnURL = "http://localhost:3000/checkout";
        private string cancelURL = "http://localhost:3000/checkout/";
        private string notifyUrl = "https://localhost:7182/api/checkout/receiver";
        public string BuildCheckOutURL(string transactionInfo, string orderId, string price)
        {
            //Khởi tạo Secure Code
            string secureCode = "";
            secureCode += merchantSiteCode;
            secureCode += " " + HttpUtility.UrlEncode(returnURL).ToLower();
            secureCode += " " + receiver;
            secureCode += " " + transactionInfo;
            secureCode += " " + orderId;
            secureCode += " " + price;
            secureCode += " " + securePass;
            //Tạo mảng để băm
            Hashtable ht = new Hashtable();
            ht.Add("merchant_site_code", merchantSiteCode);
            ht.Add("return_url", HttpUtility.UrlEncode(returnURL).ToLower());
            ht.Add("receiver", receiver);
            ht.Add("transaction_info", transactionInfo);
            ht.Add("order_code", orderId);
            ht.Add("price", price);
            ht.Add("secure_code", GetMD5Hash(secureCode));
            ht.Add("cancel_url", HttpUtility.UrlEncode(cancelURL).ToLower());
            ht.Add("notify_url", HttpUtility.UrlEncode(notifyUrl).ToLower());
            //Khởi tạo url
            string redirect_url = nganLuongURL;

            if (redirect_url.IndexOf("?") == -1)
            {
                redirect_url += "?";
            }
            else if (redirect_url.Substring(redirect_url.Length - 1, 1) != "?" && redirect_url.IndexOf("&") == -1)
            {
                redirect_url += "&";
            }

            string url = "";
            IDictionaryEnumerator en = ht.GetEnumerator();

            while (en.MoveNext())
            {
                if (url == "")
                    url += en.Key.ToString() + "=" + en.Value.ToString();
                else
                    url += "&" + en.Key.ToString() + "=" + en.Value;
            }

            String rdu = redirect_url + url;

            return rdu;
        }

        public bool VerifyPaymentUrl(string transaction_info, string order_code, string price, string payment_id, string payment_type, string error_text, string secure_code)
        {
            // Tạo mã xác thực từ web 
            string str = "";

            str += " " + transaction_info;

            str += " " + order_code;

            str += " " + price;

            str += " " + payment_id;

            str += " " + payment_type;

            str += " " + error_text;

            str += " " + merchantSiteCode;

            str += " " + securePass;

            // Mã hóa các tham số
            string verify_secure_code = "";

            verify_secure_code = GetMD5Hash(str);

            // Xác thực mã của web với mã trả về từ nganluong.vn 
            if (verify_secure_code == secure_code) return true;

            return false;
        }

        public static string GetMD5Hash(string input)
        {
            // Use input string to calculate MD5 hash
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                // Convert the byte array to hexadecimal string
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }
                return sb.ToString().ToLower();
            }
        }
    }
}
