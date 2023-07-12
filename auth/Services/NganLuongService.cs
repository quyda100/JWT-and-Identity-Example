using auth.Interfaces;
using System.Collections;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace auth.Services
{
    public class NganLuongService : INganLuongService
    {
        private const string nganLuongURL = "https://sandbox.nganluong.vn/nl35/checkout.php";
        private const string merchantSiteCode = "52274";
        private const string securePass = "571ccf4c67dafe78ba19547eb589bc0a";
        private string receiver = "nmvuong263@gmail.com";
        private string returnURL = "http://localhost:3000/checkout";
        private string cancelURL = "http://localhost:3000/checkout";
        private string notifyUrl = "https://localhost:7182/api/checkout/receiver";
        public string BuildCheckOutURL(string userInfo, string orderId, long price)
        {
            //Khởi tạo Secure Code
            string secureCode = "";
            secureCode += "" + merchantSiteCode;
            secureCode += " " + returnURL;
            secureCode += " " + receiver; // tài khoản ngân lượng
            secureCode += " " + "Thanh toán đơn hàng";
            secureCode += " " + orderId;
            secureCode += " " + price.ToString();
            secureCode += " " + "vnd"; // hỗ trợ 2 loại tiền tệ currency usd,vnd
            secureCode += " " + "1"; // số lượng mặc định 1
            secureCode += " " + "0";
            secureCode += " " + "0";
            secureCode += " " + "0";
            secureCode += " " + "0";
            secureCode += " " + "Thanh toán đơn hàng";
            secureCode += " " + userInfo;
            secureCode += " " + "";
            secureCode += " " + securePass;

            //Tạo mảng để băm
            Hashtable ht = new Hashtable();
            ht.Add("merchant_site_code", merchantSiteCode);
            ht.Add("return_url", returnURL.ToLower());
            ht.Add("receiver", receiver);
            ht.Add("transaction_info", "Thanh toán đơn hàng");
            ht.Add("order_code", orderId);
            ht.Add("price", price);
            ht.Add("currency", "vnd");
            ht.Add("quantity", 1);
            ht.Add("tax", 0);
            ht.Add("discount", 0);
            ht.Add("fee_cal", 0);
            ht.Add("fee_shipping", 0);
            ht.Add("order_description", "Thanh toán đơn hàng");
            ht.Add("buyer_info", userInfo);
            ht.Add("affiliate_code", "");
            ht.Add("secure_code", GetMD5Hash(secureCode));
            ht.Add("cancel_url", cancelURL.ToLower());
            ht.Add("notify_url", notifyUrl.ToLower());
            ht.Add("time_limit", DateTime.UtcNow.AddHours(7).AddMinutes(30).ToString("dd/MM/yyyy,HH:mm"));
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
            //https://sandbox.nganluong.vn/nl35/checkout.php?currency=vnd&secure_code=8fdc52f5743a701a594aef30f1d90bbf&price=1000&return_url=http://localhost:3000/checkout&transaction_info=abcd&buyer_info=&fee_shipping=0&cancel_url=http://localhost:3000/checkout&time_limit=31/05/2023,04:22&notify_url=https://localhost:7182/api/checkout/receiver&tax=0&receiver=nmvuong263@gmail.com&quantity=1&affiliate_code=&order_code=1&merchant_site_code=52274&order_description=&discount=0&fee_cal=0"
            //https://sandbox.nganluong.vn/nl35/checkout.php?merchant_site_code=47806&return_url=http://localhost:3000/callback-payment&currency=vnd&discount=0&fee_cal=0&fee_shipping=0&order_code=laskjflkasjj8o3jflkdsjv&order_description=day%20la%20don%20hang%20test&price=10000&quantity=1&receiver=nhayhoc@gmail.com&tax=0&transaction_info=day%20la%20thong%20tin%20giao%20dich&secure_code=fc6f861d1fbc89599ac0491fbdba2ced
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

        public String GetMD5Hash(String input)
        {

            System.Security.Cryptography.MD5CryptoServiceProvider x = new System.Security.Cryptography.MD5CryptoServiceProvider();

            byte[] bs = System.Text.Encoding.UTF8.GetBytes(input);

            bs = x.ComputeHash(bs);

            System.Text.StringBuilder s = new System.Text.StringBuilder();

            foreach (byte b in bs)
            {
                s.Append(b.ToString("x2").ToLower());
            }
            String md5String = s.ToString();
            return md5String;
        }

        public bool UpdateOrder(string order_code, string payment_id, string payment_type, string secure_code, string transaction_info)
        {
            string str = "";

            str += " " + transaction_info;

            str += " " + order_code;

            str += " " + payment_id;

            str += " " + payment_type;

            str += " " + securePass;

            string verify = GetMD5Hash(str);

            return verify == secure_code;
        }
    }
}
