using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AgriShop.Models;
using AgriShop.DTOs;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using Microsoft.Extensions.Options;

namespace AgriShop.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class QueryController : ControllerBase
    {
        private readonly AgriShopContext _context;
        private readonly EmailConfig _emailConfig;

        public QueryController(AgriShopContext context, IOptions<EmailConfig> emailConfig)
        {
            _context = context;
            _emailConfig = emailConfig.Value;
        }

        [HttpGet("test")]
        public IActionResult Test()
        {
            return Ok(new { message = "Query API is working!", timestamp = DateTime.Now });
        }

        [HttpPost("send")]
        public async Task<IActionResult> SendQuery([FromBody] QueryDTO queryDto)
        {
            try
            {
                // Log the incoming request
                Console.WriteLine($"Query request received: ProductTypeId={queryDto.ProductTypeId}, ProductId={queryDto.ProductId}, VariantId={queryDto.VariantId}");

                // Fetch the required data from database
                var productType = await _context.ProductTypes
                    .FirstOrDefaultAsync(pt => pt.ProductTypeId == queryDto.ProductTypeId);

                var product = await _context.Products
                    .FirstOrDefaultAsync(p => p.ProductId == queryDto.ProductId);

                var variant = await _context.ProductVariants
                    .FirstOrDefaultAsync(v => v.VariantId == queryDto.VariantId);

                if (productType == null || product == null || variant == null)
                {
                    var errorMsg = $"Invalid data: ProductType={productType?.TypeName ?? "null"}, Product={product?.ProductName ?? "null"}, Variant={variant?.Size ?? "null"}";
                    Console.WriteLine(errorMsg);
                    return BadRequest(errorMsg);
                }

                Console.WriteLine($"Found data: ProductType={productType.TypeName}, Product={product.ProductName}, Variant={variant.Size}");

                // Build email body with predefined and dynamic content
                var emailBody = $@"
                    <html>
                    <body style='font-family: Arial, sans-serif; line-height: 1.6; color: #333;'>
                        <div style='max-width: 600px; margin: 0 auto; padding: 20px; border: 1px solid #ddd; border-radius: 8px;'>
                            <h2 style='color: #2c5aa0; border-bottom: 2px solid #2c5aa0; padding-bottom: 10px;'>
                                🌾 AgriShop - Product Purchase Query
                            </h2>
                            
                            <p><strong>Hi,</strong></p>
                            
                            <p>A customer has shown interest in purchasing a product from AgriShop. Here are the details:</p>
                            
                            <div style='background-color: #f8f9fa; padding: 15px; border-radius: 5px; margin: 20px 0;'>
                                <h4 style='color: #2c5aa0; margin-top: 0;'>📦 Product Details:</h4>
                                <ul style='list-style: none; padding-left: 0;'>
                                    <li style='margin-bottom: 8px;'><strong>🏷️ Product Type:</strong> {productType.TypeName}</li>
                                    <li style='margin-bottom: 8px;'><strong>📋 Product Name:</strong> {product.ProductName}</li>
                                    <li style='margin-bottom: 8px;'><strong>📏 Variant:</strong> {variant.Size}</li>
                                    <li style='margin-bottom: 8px;'><strong>💰 Price:</strong> ₹{variant.Price}</li>
                                </ul>
                            </div>
                            
                            <p><strong>Customer Message:</strong></p>
                            <p style='font-style: italic; background-color: #fff3cd; padding: 10px; border-left: 4px solid #ffc107; border-radius: 3px;'>
                                ""I am interested in buying this product. Please contact me regarding availability, pricing, and delivery options.""
                            </p>
                            
                            <div style='margin-top: 30px; padding-top: 20px; border-top: 1px solid #ddd;'>
                                <p style='margin-bottom: 5px;'><strong>Best regards,</strong></p>
                                <p style='margin-bottom: 5px;'>AgriShop Customer Support</p>
                                <p style='font-size: 12px; color: #666; margin-top: 20px;'>
                                    This is an automated query from the AgriShop website. 
                                    Please respond to the customer as soon as possible.
                                </p>
                            </div>
                        </div>
                    </body>
                    </html>";

                // Send email using configuration
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress("AgriShop Support", _emailConfig.Email));
                message.To.Add(new MailboxAddress("You", _emailConfig.Email));
                message.Subject = "Query about Product Purchase";

                message.Body = new TextPart("html")
                {
                    Text = emailBody
                };

                using (var client = new SmtpClient())
                {
                    client.Connect(_emailConfig.SmtpServer, _emailConfig.SmtpPort, SecureSocketOptions.StartTls);
                    client.Authenticate(_emailConfig.Email, _emailConfig.AppPassword);
                    client.Send(message);
                    client.Disconnect(true);
                }

                Console.WriteLine("Email sent successfully");
                return Ok(new { message = "Query sent successfully" });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in SendQuery: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                return StatusCode(500, new { message = $"Error sending query: {ex.Message}" });
            }
        }
    }
}
