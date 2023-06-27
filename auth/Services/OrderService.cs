using auth.Data;
using auth.Interfaces;
using auth.Model;
using auth.Model.DTO;
using auth.Model.Request;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Net.Http.Headers;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Text.Json;

namespace auth.Services
{
    public class OrderService : IOrderService
    {
        private readonly ApplicationDBContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogService _log;
        private readonly IMapper _mapper;
        private readonly IHttpClientFactory _httpClientFactory;

        public OrderService(ApplicationDBContext context, IHttpContextAccessor httpContextAccessor, ILogService log, IMapper mapper, IHttpClientFactory httpClientFactory)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _log = log;
            _mapper = mapper;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<int> CreateOrder(OrderRequest model)
        {
            int orderId = -1;
            var order = new Order
            {
                PaymentMethod = model.PaymentType,
                CustomerName = model.Name,
                Address = model.Address,
                Phone = model.Phone,
                Status = 0,
                UserId = GetUserId(),
            };
            _context.Orders.Add(order);
            _context.SaveChanges();
            orderId = order.Id;
            //Tạo orderProduct
            var orderProducts = GetOrderProducts(model.orderProducts, order.Id);
            order.Total = orderProducts.Sum(p => p.Quantity * p.Price); // Tổng hóa đơn
            _context.OrderProducts.AddRange(orderProducts);
            await _context.SaveChangesAsync();
            return orderId;

        }
        public List<OrderProduct> GetOrderProducts(List<OrderProductRequest> productRequests, int OrderId)
        {
            List<OrderProduct> orderProducts = new List<OrderProduct>();
            foreach (var productRequest in productRequests)
            {
                var product = _context.Products.FirstOrDefault(p => p.Id == productRequest.ProductId);
                if (product == null)
                {
                    throw new Exception("Không tìm thấy sản phẩm: " + productRequest.ProductId);
                }
                if (product.Stock < productRequest.Quanlity)
                {
                    throw new Exception("Sản phẩm: " + product.Code + " tồn kho không đủ");
                }
                var orderProduct = new OrderProduct
                {
                    ProductId = product.Id,
                    OrderId = OrderId,
                    Price = product.Price,
                    Quantity = productRequest.Quanlity
                };
                orderProducts.Add(orderProduct);
            }
            return orderProducts;
        }

        public List<OrderDTO> GetOrders()
        {
            var orders = _context.Orders.Where(o=>o.PaymentMethod == "COD" || (o.PaymentMethod == "NganLuong" && o.PaymentTime != DateTime.MinValue))
                    .Include(o => o.OrderProducts).ThenInclude(p => p.Product)
                    .Include(o => o.User).OrderBy(p => p.CreatedAt)
                    .Select(o => _mapper.Map<OrderDTO>(o)).ToList();
            return orders;
        }

        public List<OrderProductDTO> GetOrderProducts(int orderId)
        {
            var orderProducts = _context.OrderProducts.Where(o => o.OrderId == orderId).Include(o => o.Product).Select(o => _mapper.Map<OrderProductDTO>(o)).ToList();
            return orderProducts;
        }

        public async Task UpdateOrder(int id, OrderDTO model)
        {
            if (id != model.Id)
                throw new Exception("Having a trouble");
            var order = await FindOrder(id);
            if (order == null)
            {
                throw new Exception("Không tìm thấy hóa đơn");
            }
            order.Status = model.Status;
            if (order.Status == 1)
            {
                UpdateProductSales(order.OrderProducts);
            }
            order.UpdatedAt = DateTime.Now;
            _log.SaveLog("Cập nhật đơn hàng: " + id);
            _context.Orders.Update(order);
        }

        public List<OrderDTO> GetOrdersByUserId()
        {
            var orders = _context.Orders.Where(o => o.UserId == GetUserId())
                        .Where(o=>o.PaymentMethod == "COD" || (o.PaymentMethod == "NganLuong" && o.PaymentTime != DateTime.MinValue))
                        .Include(o => o.OrderProducts)
                        .ThenInclude(p => p.Product)
                        .Include(o => o.User)
                        .OrderBy(p => p.CreatedAt).Select(o => _mapper.Map<OrderDTO>(o)).ToList();
            return orders;
        }

        public void DeleteOrder(int id)
        {
            var order = _context.Orders.FirstOrDefault(o => o.Id == id && o.UserId == GetUserId()) ?? throw new Exception("Không tìm thấy hóa đơn");
            if (order.Status != 0)
            {
                throw new Exception("Không thể thực hiện yêu cầu");
            }
            order.Status = -1;
            order.UpdatedAt = DateTime.Now;
            _context.Orders.Update(order);
            _context.SaveChanges();
        }
        private void UpdateProductSales(List<OrderProduct> orderProducts)
        {
            foreach (var item in orderProducts)
            {
                var product = _context.Products.FirstOrDefault(p => p.Id == item.ProductId);
                product.Stock -= item.Quantity;
                product.Sales += item.Quantity;
                _context.Products.Update(product);
            }
            _context.SaveChanges();
        }
        private string GetUserId() => _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

        public async Task UpdateOrderStatus(List<int> ids, int status)
        {
            foreach (var id in ids)
            {
                var order = await _context.Orders.Include(o => o.OrderProducts).ThenInclude(p => p.Product).FirstOrDefaultAsync(o => o.Id == id);
                if (order == null)
                {
                    throw new Exception("Không tìm thấy hóa đơn: " + id);
                }
                if (status == 1)
                {
                    var temp = await CreateOrderGHN(order);
                    order.Code = temp.Code;
                    order.DeliveryTime = temp.DeliveryTime;
                }
                if (status == 2)
                {
                    UpdateProductSales(order.OrderProducts);
                }
                order.Status = status;
                order.UpdatedAt = DateTime.Now;
                _context.Orders.Update(order);
                _log.SaveLog($"Cập nhật hóa đơn: {id} thành {status}");
                 _context.SaveChanges();
            }
        }
        /*
         * Tạo đơn hàng và gửi qua GHN
         */
        private async Task<Order> CreateOrderGHN(Order order)
        {
            /*
             * Khai báo dữ liệu cần gửi đi
             */
            var address = order.Address.Split(',');
            IEnumerable<object> items = order.OrderProducts.Select(p => new
            {
                name = p.Product.Name,
                code = p.Product.Code,
                quantity = p.Quantity,
                price = p.Price / 1000,
            });
            string item = JsonConvert.SerializeObject(items);
            object data = new
            {
                payment_type_id = 2,
                to_name = order.CustomerName,
                to_phone = order.Phone,
                to_address = address[0],
                to_ward_name = address[address.Length - 3].Trim(),
                to_district_name = address[address.Length - 2].Trim(),
                to_province_name = address[address.Length - 1].Trim(),
                cod_amount = order.PaymentMethod == "COD" ? order.Total / 1000 : 0,
                weight = 200,
                length = 10,
                width = 5,
                height = 10,
                service_type_id = 2,
                service_id = 0,
                required_note = "CHOTHUHANG",
                Items = items,
            };
            string dataSend = JsonConvert.SerializeObject(data);
            /*
             * Khai báo HTTP
             */
            var stringContent = new StringContent(dataSend, Encoding.UTF8, "application/json");
            HttpClient client = _httpClientFactory.CreateClient();
            var uri = new Uri("https://dev-online-gateway.ghn.vn/shiip/public-api/v2/shipping-order/create");
            client.DefaultRequestHeaders.Add("ShopId", "124347");
            client.DefaultRequestHeaders.Add("Token", "c0ad7ca7-fb92-11ed-92f3-0e596e5953f1");
            /*
             * Gửi dữ liệu thông qua HTTP Post
             */
            var response = await client.PostAsync(uri, stringContent);
            if (response.IsSuccessStatusCode)
            {
                /*
                * Lấy dữ liệu trả về và convert
                */
                var jsonData = await response.Content.ReadAsStringAsync();
                dynamic json = JsonConvert.DeserializeObject(jsonData);
                /*
                * Cập nhật thông tin hóa đơn
                */
                order.Code = json.data.order_code;
                order.DeliveryTime = json.data.expected_delivery_time;
            }
            else
            {
                var jsonData = await response.Content.ReadAsStringAsync();
                dynamic json = JsonConvert.DeserializeObject(jsonData);
                string ex = json.code_message_value;
                throw new Exception(ex);
            }
            return order;
        }

        public async Task<Order> FindOrder(int id)
        {
            var order = await _context.Orders.FirstOrDefaultAsync(o => o.Id == id);
            return order;
        }
        public async Task UpdateOrderCheckout(int id, long price)
        {
            var order = await FindOrder(id);
            if (order == null)
            {
                throw new Exception("Không tìm thấy hóa đơn");
            }
            if (order.PaymentTime != DateTime.MinValue)
            {
                throw new Exception("Đơn hàng đã được thanh toán");
            }
            if (price != order.Total)
            {
                throw new Exception("Sai thông tin sản phẩm");
            }
            order.PaymentMethod = "NganLuong";
            order.PaymentTime = DateTime.Now;
            _context.Update(order);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateOrderCheckout(int id)
        {
            var order = await FindOrder(id);
            if (order == null)
            {
                throw new Exception("Không tìm thấy hóa đơn");
            }
            if (order.PaymentTime != DateTime.MinValue)
            {
                throw new Exception("Đơn hàng đã được thanh toán");
            }
            order.PaymentMethod = "NganLuong";
            order.PaymentTime = DateTime.Now;
            _context.Update(order);
            await _context.SaveChangesAsync();
        }
    }
}
