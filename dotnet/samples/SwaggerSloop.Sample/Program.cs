using System.Reflection;
using Microsoft.OpenApi.Models;
using SwaggerSloop;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "SwaggerSloop Demo API",
        Version = "v1",
        Description = "这是一个演示 SwaggerSloop 功能的示例 API，包含用户管理、商品管理、订单管理等模块。",
        Contact = new OpenApiContact
        {
            Name = "SwaggerSloop",
            Url = new Uri("https://github.com/rain7788/swagger-sloop")
        }
    });

    // 加载 XML 注释文件
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
    {
        c.IncludeXmlComments(xmlPath);
    }
});

var app = builder.Build();

// Enable Swagger
app.UseSwagger();

// Use SwaggerSloop UI
app.UseSwaggerSloop(options =>
{
    options.DocumentTitle = "SwaggerSloop Demo";
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Demo API V1");
    options.DefaultTheme = SwaggerSloopTheme.Auto;
    options.PrimaryColor = "#5D87FF";
});

// ============================================
// 用户管理 API
// ============================================

app.MapGet("/api/users", (int? page, int? pageSize) =>
{
    var users = new[]
    {
        new User(1, "张三", "zhangsan@example.com", "13800138001", DateTime.Now.AddDays(-30), true),
        new User(2, "李四", "lisi@example.com", "13800138002", DateTime.Now.AddDays(-20), true),
        new User(3, "王五", "wangwu@example.com", "13800138003", DateTime.Now.AddDays(-10), false),
    };
    return Results.Ok(new ApiResponse<User[]>(true, "获取成功", users));
})
.WithName("GetUsers")
.WithSummary("获取用户列表")
.WithDescription("获取系统中所有用户的列表，支持分页")
.WithTags("用户管理");

app.MapGet("/api/users/{id}", (int id) =>
{
    var user = new User(id, "张三", "zhangsan@example.com", "13800138001", DateTime.Now.AddDays(-30), true);
    return Results.Ok(new ApiResponse<User>(true, "获取成功", user));
})
.WithName("GetUserById")
.WithSummary("获取用户详情")
.WithDescription("根据用户ID获取用户的详细信息")
.WithTags("用户管理");

app.MapPost("/api/users", (CreateUserRequest request) =>
{
    var user = new User(1, request.Name, request.Email, request.Phone, DateTime.Now, true);
    return Results.Created($"/api/users/{user.Id}", new ApiResponse<User>(true, "创建成功", user));
})
.WithName("CreateUser")
.WithSummary("创建用户")
.WithDescription("创建一个新的用户账户")
.WithTags("用户管理");

app.MapPut("/api/users/{id}", (int id, UpdateUserRequest request) =>
{
    var user = new User(id, request.Name, request.Email, request.Phone, DateTime.Now.AddDays(-30), request.IsActive);
    return Results.Ok(new ApiResponse<User>(true, "更新成功", user));
})
.WithName("UpdateUser")
.WithSummary("更新用户")
.WithDescription("更新指定用户的信息")
.WithTags("用户管理");

app.MapDelete("/api/users/{id}", (int id) =>
{
    return Results.Ok(new ApiResponse<object>(true, "删除成功", null));
})
.WithName("DeleteUser")
.WithSummary("删除用户")
.WithDescription("删除指定的用户账户")
.WithTags("用户管理");

// ============================================
// 商品管理 API
// ============================================

app.MapGet("/api/products", (string? keyword, int? categoryId, decimal? minPrice, decimal? maxPrice) =>
{
    var products = new[]
    {
        new Product(1, "iPhone 15 Pro", "Apple 最新旗舰手机", 7999.00m, 100, 1, "https://example.com/iphone.jpg"),
        new Product(2, "MacBook Pro 14", "M3 Pro 芯片，性能强劲", 14999.00m, 50, 1, "https://example.com/macbook.jpg"),
        new Product(3, "AirPods Pro 2", "主动降噪，音质出众", 1899.00m, 200, 2, "https://example.com/airpods.jpg"),
    };
    return Results.Ok(new ApiResponse<Product[]>(true, "获取成功", products));
})
.WithName("GetProducts")
.WithSummary("获取商品列表")
.WithDescription("获取商品列表，支持按关键词、分类、价格区间筛选")
.WithTags("商品管理");

app.MapGet("/api/products/{id}", (int id) =>
{
    var product = new Product(id, "iPhone 15 Pro", "Apple 最新旗舰手机", 7999.00m, 100, 1, "https://example.com/iphone.jpg");
    return Results.Ok(new ApiResponse<Product>(true, "获取成功", product));
})
.WithName("GetProductById")
.WithSummary("获取商品详情")
.WithDescription("根据商品ID获取商品的详细信息")
.WithTags("商品管理");

app.MapPost("/api/products", (CreateProductRequest request) =>
{
    var product = new Product(1, request.Name, request.Description, request.Price, request.Stock, request.CategoryId, request.ImageUrl);
    return Results.Created($"/api/products/{product.Id}", new ApiResponse<Product>(true, "创建成功", product));
})
.WithName("CreateProduct")
.WithSummary("创建商品")
.WithDescription("创建一个新的商品")
.WithTags("商品管理");

app.MapPut("/api/products/{id}", (int id, UpdateProductRequest request) =>
{
    var product = new Product(id, request.Name, request.Description, request.Price, request.Stock, request.CategoryId, request.ImageUrl);
    return Results.Ok(new ApiResponse<Product>(true, "更新成功", product));
})
.WithName("UpdateProduct")
.WithSummary("更新商品")
.WithDescription("更新指定商品的信息")
.WithTags("商品管理");

app.MapDelete("/api/products/{id}", (int id) =>
{
    return Results.Ok(new ApiResponse<object>(true, "删除成功", null));
})
.WithName("DeleteProduct")
.WithSummary("删除商品")
.WithDescription("删除指定的商品")
.WithTags("商品管理");

// ============================================
// 订单管理 API
// ============================================

app.MapGet("/api/orders", (int? userId, string? status, DateTime? startDate, DateTime? endDate) =>
{
    var orders = new[]
    {
        new Order(1001, 1, new[] { new OrderItem(1, "iPhone 15 Pro", 7999.00m, 1) }, 7999.00m, "已支付", DateTime.Now.AddDays(-5)),
        new Order(1002, 2, new[] { new OrderItem(2, "MacBook Pro 14", 14999.00m, 1), new OrderItem(3, "AirPods Pro 2", 1899.00m, 1) }, 16898.00m, "待发货", DateTime.Now.AddDays(-2)),
    };
    return Results.Ok(new ApiResponse<Order[]>(true, "获取成功", orders));
})
.WithName("GetOrders")
.WithSummary("获取订单列表")
.WithDescription("获取订单列表，支持按用户、状态、时间范围筛选")
.WithTags("订单管理");

app.MapGet("/api/orders/{id}", (int id) =>
{
    var order = new Order(id, 1, new[] { new OrderItem(1, "iPhone 15 Pro", 7999.00m, 1) }, 7999.00m, "已支付", DateTime.Now.AddDays(-5));
    return Results.Ok(new ApiResponse<Order>(true, "获取成功", order));
})
.WithName("GetOrderById")
.WithSummary("获取订单详情")
.WithDescription("根据订单ID获取订单的详细信息")
.WithTags("订单管理");

app.MapPost("/api/orders", (CreateOrderRequest request) =>
{
    var order = new Order(1001, request.UserId, request.Items.Select(i => new OrderItem(i.ProductId, "商品名称", i.Price, i.Quantity)).ToArray(),
        request.Items.Sum(i => i.Price * i.Quantity), "待支付", DateTime.Now);
    return Results.Created($"/api/orders/{order.Id}", new ApiResponse<Order>(true, "创建成功", order));
})
.WithName("CreateOrder")
.WithSummary("创建订单")
.WithDescription("创建一个新的订单")
.WithTags("订单管理");

app.MapPatch("/api/orders/{id}/status", (int id, UpdateOrderStatusRequest request) =>
{
    return Results.Ok(new ApiResponse<object>(true, $"订单状态已更新为: {request.Status}", null));
})
.WithName("UpdateOrderStatus")
.WithSummary("更新订单状态")
.WithDescription("更新指定订单的状态")
.WithTags("订单管理");

app.MapDelete("/api/orders/{id}", (int id) =>
{
    return Results.Ok(new ApiResponse<object>(true, "订单已取消", null));
})
.WithName("CancelOrder")
.WithSummary("取消订单")
.WithDescription("取消指定的订单")
.WithTags("订单管理");

// ============================================
// 分类管理 API
// ============================================

app.MapGet("/api/categories", () =>
{
    var categories = new[]
    {
        new Category(1, "电子产品", "手机、电脑、平板等电子设备", null),
        new Category(2, "配件", "耳机、充电器、保护壳等配件", null),
        new Category(3, "服装", "男装、女装、童装", null),
    };
    return Results.Ok(new ApiResponse<Category[]>(true, "获取成功", categories));
})
.WithName("GetCategories")
.WithSummary("获取分类列表")
.WithDescription("获取所有商品分类")
.WithTags("分类管理");

app.MapGet("/api/categories/{id}", (int id) =>
{
    var category = new Category(id, "电子产品", "手机、电脑、平板等电子设备", null);
    return Results.Ok(new ApiResponse<Category>(true, "获取成功", category));
})
.WithName("GetCategoryById")
.WithSummary("获取分类详情")
.WithDescription("根据分类ID获取分类详情")
.WithTags("分类管理");

app.MapPost("/api/categories", (CreateCategoryRequest request) =>
{
    var category = new Category(1, request.Name, request.Description, request.ParentId);
    return Results.Created($"/api/categories/{category.Id}", new ApiResponse<Category>(true, "创建成功", category));
})
.WithName("CreateCategory")
.WithSummary("创建分类")
.WithDescription("创建一个新的商品分类")
.WithTags("分类管理");

// ============================================
// 统计分析 API
// ============================================

app.MapGet("/api/stats/overview", () =>
{
    var stats = new DashboardStats(1234, 567, 89012.50m, 45);
    return Results.Ok(new ApiResponse<DashboardStats>(true, "获取成功", stats));
})
.WithName("GetOverviewStats")
.WithSummary("获取概览统计")
.WithDescription("获取仪表盘概览统计数据")
.WithTags("统计分析");

app.MapGet("/api/stats/sales", (DateTime? startDate, DateTime? endDate) =>
{
    var sales = new[]
    {
        new SalesData("2024-01", 125000.00m, 156),
        new SalesData("2024-02", 138000.00m, 178),
        new SalesData("2024-03", 156000.00m, 203),
    };
    return Results.Ok(new ApiResponse<SalesData[]>(true, "获取成功", sales));
})
.WithName("GetSalesStats")
.WithSummary("获取销售统计")
.WithDescription("获取指定时间范围内的销售统计数据")
.WithTags("统计分析");

app.MapGet("/api/stats/top-products", (int? limit) =>
{
    var products = new[]
    {
        new TopProduct(1, "iPhone 15 Pro", 523, 4180177.00m),
        new TopProduct(2, "MacBook Pro 14", 128, 1919872.00m),
        new TopProduct(3, "AirPods Pro 2", 892, 1693108.00m),
    };
    return Results.Ok(new ApiResponse<TopProduct[]>(true, "获取成功", products));
})
.WithName("GetTopProducts")
.WithSummary("获取热销商品")
.WithDescription("获取销量最高的商品列表")
.WithTags("统计分析");

// ============================================
// 文件上传 API
// ============================================

app.MapPost("/api/files/upload", async (IFormFile file) =>
{
    if (file == null || file.Length == 0)
    {
        return Results.BadRequest(new ApiResponse<object>(false, "请选择要上传的文件", null));
    }

    var result = new FileUploadResult(
        file.FileName,
        file.ContentType,
        file.Length,
        Guid.NewGuid().ToString("N") + Path.GetExtension(file.FileName)
    );

    return Results.Ok(new ApiResponse<FileUploadResult>(true, "文件上传成功", result));
})
.WithName("UploadFile")
.WithSummary("上传单个文件")
.WithDescription("上传单个文件到服务器，支持任意类型文件")
.WithTags("文件上传")
.DisableAntiforgery();

app.MapPost("/api/files/upload-with-info", async (IFormFile file, string? description, string? category) =>
{
    if (file == null || file.Length == 0)
    {
        return Results.BadRequest(new ApiResponse<object>(false, "请选择要上传的文件", null));
    }

    var result = new FileUploadWithInfoResult(
        file.FileName,
        file.ContentType,
        file.Length,
        Guid.NewGuid().ToString("N") + Path.GetExtension(file.FileName),
        description ?? "",
        category ?? "默认分类"
    );

    return Results.Ok(new ApiResponse<FileUploadWithInfoResult>(true, "文件上传成功", result));
})
.WithName("UploadFileWithInfo")
.WithSummary("上传文件并附带信息")
.WithDescription("上传文件的同时可以附带描述和分类信息")
.WithTags("文件上传")
.DisableAntiforgery();

app.MapPost("/api/files/upload-avatar", async (IFormFile avatar, int userId) =>
{
    if (avatar == null || avatar.Length == 0)
    {
        return Results.BadRequest(new ApiResponse<object>(false, "请选择头像文件", null));
    }

    // 检查文件类型
    var allowedTypes = new[] { "image/jpeg", "image/png", "image/gif", "image/webp" };
    if (!allowedTypes.Contains(avatar.ContentType))
    {
        return Results.BadRequest(new ApiResponse<object>(false, "只支持 JPEG、PNG、GIF、WebP 格式的图片", null));
    }

    var result = new AvatarUploadResult(
        userId,
        $"/avatars/{userId}/{Guid.NewGuid():N}{Path.GetExtension(avatar.FileName)}",
        avatar.Length
    );

    return Results.Ok(new ApiResponse<AvatarUploadResult>(true, "头像上传成功", result));
})
.WithName("UploadAvatar")
.WithSummary("上传用户头像")
.WithDescription("上传用户头像图片，支持 JPEG、PNG、GIF、WebP 格式")
.WithTags("文件上传")
.DisableAntiforgery();

// ============================================
// 文件下载 API
// ============================================

app.MapGet("/api/files/download/sample-text", () =>
{
    var content = "这是一个示例文本文件的内容。\n\nSwaggerSloop 文件下载测试\n\n创建时间: " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
    var bytes = System.Text.Encoding.UTF8.GetBytes(content);
    return Results.File(bytes, "text/plain", "sample.txt");
})
.WithName("DownloadSampleText")
.WithSummary("下载示例文本文件")
.WithDescription("下载一个示例的文本文件，用于测试文件下载功能")
.WithTags("文件下载")
.Produces(200, contentType: "text/plain");

app.MapGet("/api/files/download/sample-csv", () =>
{
    var csv = "ID,姓名,邮箱,创建时间\n1,张三,zhangsan@example.com,2024-01-15\n2,李四,lisi@example.com,2024-01-16\n3,王五,wangwu@example.com,2024-01-17";
    var bytes = System.Text.Encoding.UTF8.GetPreamble().Concat(System.Text.Encoding.UTF8.GetBytes(csv)).ToArray();
    return Results.File(bytes, "text/csv", "users-export.csv");
})
.WithName("DownloadSampleCsv")
.WithSummary("下载示例 CSV 文件")
.WithDescription("导出用户数据为 CSV 格式文件")
.WithTags("文件下载")
.Produces(200, contentType: "text/csv");

app.MapGet("/api/files/download/sample-json", () =>
{
    var data = new
    {
        title = "SwaggerSloop 数据导出",
        exportTime = DateTime.Now,
        records = new[]
        {
            new { id = 1, name = "商品A", price = 99.99 },
            new { id = 2, name = "商品B", price = 199.99 },
            new { id = 3, name = "商品C", price = 299.99 }
        }
    };
    var json = System.Text.Json.JsonSerializer.Serialize(data, new System.Text.Json.JsonSerializerOptions { WriteIndented = true });
    var bytes = System.Text.Encoding.UTF8.GetBytes(json);
    return Results.File(bytes, "application/json", "data-export.json");
})
.WithName("DownloadSampleJson")
.WithSummary("下载示例 JSON 文件")
.WithDescription("导出数据为 JSON 格式文件")
.WithTags("文件下载")
.Produces(200, contentType: "application/json");

app.MapGet("/api/files/download/report/{format}", (string format) =>
{
    format = format.ToLower();

    return format switch
    {
        "pdf" => Results.File(
            GenerateSamplePdf(),
            "application/pdf",
            $"report-{DateTime.Now:yyyyMMdd}.pdf"
        ),
        "xlsx" => Results.File(
            GenerateSampleExcel(),
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            $"report-{DateTime.Now:yyyyMMdd}.xlsx"
        ),
        _ => Results.BadRequest(new ApiResponse<object>(false, "不支持的格式，请使用 pdf 或 xlsx", null))
    };
})
.WithName("DownloadReport")
.WithSummary("下载报表文件")
.WithDescription("下载指定格式的报表文件，支持 PDF 和 Excel 格式")
.WithTags("文件下载");

app.MapGet("/api/files/download/image", () =>
{
    // 生成一个简单的 1x1 像素 PNG 图片 (最小有效 PNG)
    var pngBytes = new byte[] {
        0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A,  // PNG signature
        0x00, 0x00, 0x00, 0x0D, 0x49, 0x48, 0x44, 0x52,  // IHDR chunk
        0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x01,  // 1x1 size
        0x08, 0x02, 0x00, 0x00, 0x00, 0x90, 0x77, 0x53,
        0xDE, 0x00, 0x00, 0x00, 0x0C, 0x49, 0x44, 0x41,  // IDAT chunk
        0x54, 0x08, 0xD7, 0x63, 0xF8, 0x0F, 0x00, 0x00,
        0x01, 0x01, 0x00, 0x05, 0x18, 0xD8, 0x4D, 0x4D,
        0x00, 0x00, 0x00, 0x00, 0x49, 0x45, 0x4E, 0x44,  // IEND chunk
        0xAE, 0x42, 0x60, 0x82
    };
    return Results.File(pngBytes, "image/png", "sample-image.png");
})
.WithName("DownloadImage")
.WithSummary("下载示例图片")
.WithDescription("下载一个示例图片文件")
.WithTags("文件下载")
.Produces(200, contentType: "image/png");

// Helper functions for generating sample files
static byte[] GenerateSamplePdf()
{
    // 生成一个最小的有效 PDF 文件
    var pdfContent = @"%PDF-1.4
1 0 obj
<< /Type /Catalog /Pages 2 0 R >>
endobj
2 0 obj
<< /Type /Pages /Kids [3 0 R] /Count 1 >>
endobj
3 0 obj
<< /Type /Page /Parent 2 0 R /MediaBox [0 0 612 792] /Contents 4 0 R >>
endobj
4 0 obj
<< /Length 44 >>
stream
BT /F1 12 Tf 100 700 Td (SwaggerSloop Report) Tj ET
endstream
endobj
xref
0 5
0000000000 65535 f 
0000000009 00000 n 
0000000058 00000 n 
0000000115 00000 n 
0000000206 00000 n 
trailer
<< /Size 5 /Root 1 0 R >>
startxref
300
%%EOF";
    return System.Text.Encoding.ASCII.GetBytes(pdfContent);
}

static byte[] GenerateSampleExcel()
{
    // 返回一个简单的占位字节数组（真实场景需要用库生成）
    // 这里返回一个带有标识的简单文件
    var content = "PK\x03\x04 - This is a placeholder for Excel file. Use a proper library like EPPlus or ClosedXML in production.";
    return System.Text.Encoding.UTF8.GetBytes(content);
}

// ============================================
// 表单提交 API
// ============================================

app.MapPost("/api/forms/contact", async (HttpContext context) =>
{
    var form = await context.Request.ReadFormAsync();
    var name = form["name"].ToString();
    var email = form["email"].ToString();
    var message = form["message"].ToString();

    if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(message))
    {
        return Results.BadRequest(new ApiResponse<object>(false, "姓名、邮箱和消息内容都是必填项", null));
    }

    var result = new ContactFormResult(
        Guid.NewGuid().ToString("N"),
        name,
        email,
        message,
        DateTime.Now
    );

    return Results.Ok(new ApiResponse<ContactFormResult>(true, "表单提交成功，我们会尽快回复您", result));
})
.WithName("SubmitContactForm")
.WithSummary("提交联系表单")
.WithDescription("使用 application/x-www-form-urlencoded 格式提交联系表单")
.WithTags("表单提交")
.Accepts<ContactFormRequest>("application/x-www-form-urlencoded")
.DisableAntiforgery();

app.MapPost("/api/forms/feedback", async (HttpContext context) =>
{
    var form = await context.Request.ReadFormAsync();
    var rating = int.TryParse(form["rating"], out var r) ? r : 0;
    var feedback = form["feedback"].ToString();
    var email = form["email"].ToString();

    if (rating < 1 || rating > 5)
    {
        return Results.BadRequest(new ApiResponse<object>(false, "评分必须在 1-5 之间", null));
    }

    var result = new FeedbackFormResult(
        Guid.NewGuid().ToString("N"),
        rating,
        feedback,
        email,
        DateTime.Now
    );

    return Results.Ok(new ApiResponse<FeedbackFormResult>(true, "感谢您的反馈", result));
})
.WithName("SubmitFeedback")
.WithSummary("提交反馈表单")
.WithDescription("使用 application/x-www-form-urlencoded 格式提交用户反馈")
.WithTags("表单提交")
.Accepts<FeedbackFormRequest>("application/x-www-form-urlencoded")
.DisableAntiforgery();

app.Run();

// ============================================
// 数据模型定义
// ============================================

/// <summary>
/// 统一API响应格式
/// </summary>
/// <typeparam name="T">数据类型</typeparam>
/// <param name="Success">是否成功</param>
/// <param name="Message">响应消息</param>
/// <param name="Data">响应数据</param>
record ApiResponse<T>(bool Success, string Message, T? Data);

/// <summary>
/// 用户信息
/// </summary>
/// <param name="Id">用户ID</param>
/// <param name="Name">用户名称</param>
/// <param name="Email">电子邮箱</param>
/// <param name="Phone">手机号码</param>
/// <param name="CreatedAt">创建时间</param>
/// <param name="IsActive">是否激活</param>
record User(int Id, string Name, string Email, string Phone, DateTime CreatedAt, bool IsActive);

/// <summary>
/// 创建用户请求
/// </summary>
/// <param name="Name">用户名称</param>
/// <param name="Email">电子邮箱</param>
/// <param name="Phone">手机号码</param>
/// <param name="Password">登录密码</param>
record CreateUserRequest(string Name, string Email, string Phone, string Password);

/// <summary>
/// 更新用户请求
/// </summary>
record UpdateUserRequest(string Name, string Email, string Phone, bool IsActive);

/// <summary>
/// 商品信息
/// </summary>
/// <param name="Id">商品ID</param>
/// <param name="Name">商品名称</param>
/// <param name="Description">商品描述</param>
/// <param name="Price">商品价格</param>
/// <param name="Stock">库存数量</param>
/// <param name="CategoryId">分类ID</param>
/// <param name="ImageUrl">商品图片</param>
record Product(int Id, string Name, string Description, decimal Price, int Stock, int CategoryId, string ImageUrl);

/// <summary>
/// 创建商品请求
/// </summary>
record CreateProductRequest(string Name, string Description, decimal Price, int Stock, int CategoryId, string ImageUrl);

/// <summary>
/// 更新商品请求
/// </summary>
record UpdateProductRequest(string Name, string Description, decimal Price, int Stock, int CategoryId, string ImageUrl);

/// <summary>
/// 订单信息
/// </summary>
/// <param name="Id">订单ID</param>
/// <param name="UserId">用户ID</param>
/// <param name="Items">订单项</param>
/// <param name="TotalAmount">订单总额</param>
/// <param name="Status">订单状态</param>
/// <param name="CreatedAt">创建时间</param>
record Order(int Id, int UserId, OrderItem[] Items, decimal TotalAmount, string Status, DateTime CreatedAt);

/// <summary>
/// 订单项
/// </summary>
/// <param name="ProductId">商品ID</param>
/// <param name="ProductName">商品名称</param>
/// <param name="Price">单价</param>
/// <param name="Quantity">数量</param>
record OrderItem(int ProductId, string ProductName, decimal Price, int Quantity);

/// <summary>
/// 创建订单请求
/// </summary>
record CreateOrderRequest(int UserId, CreateOrderItemRequest[] Items);

/// <summary>
/// 创建订单项请求
/// </summary>
record CreateOrderItemRequest(int ProductId, decimal Price, int Quantity);

/// <summary>
/// 更新订单状态请求
/// </summary>
/// <param name="Status">订单状态：待支付、已支付、待发货、已发货、已完成、已取消</param>
record UpdateOrderStatusRequest(string Status);

/// <summary>
/// 商品分类
/// </summary>
/// <param name="Id">分类ID</param>
/// <param name="Name">分类名称</param>
/// <param name="Description">分类描述</param>
/// <param name="ParentId">父分类ID</param>
record Category(int Id, string Name, string Description, int? ParentId);

/// <summary>
/// 创建分类请求
/// </summary>
record CreateCategoryRequest(string Name, string Description, int? ParentId);

/// <summary>
/// 仪表盘统计数据
/// </summary>
/// <param name="TotalUsers">总用户数</param>
/// <param name="TotalOrders">总订单数</param>
/// <param name="TotalSales">总销售额</param>
/// <param name="TodayOrders">今日订单数</param>
record DashboardStats(int TotalUsers, int TotalOrders, decimal TotalSales, int TodayOrders);

/// <summary>
/// 销售数据
/// </summary>
/// <param name="Month">月份</param>
/// <param name="Amount">销售额</param>
/// <param name="OrderCount">订单数</param>
record SalesData(string Month, decimal Amount, int OrderCount);

/// <summary>
/// 热销商品
/// </summary>
/// <param name="ProductId">商品ID</param>
/// <param name="ProductName">商品名称</param>
/// <param name="SalesCount">销量</param>
/// <param name="SalesAmount">销售额</param>
record TopProduct(int ProductId, string ProductName, int SalesCount, decimal SalesAmount);

// ============================================
// 文件上传相关模型
// ============================================

/// <summary>
/// 文件上传结果
/// </summary>
/// <param name="OriginalName">原始文件名</param>
/// <param name="ContentType">文件类型</param>
/// <param name="Size">文件大小（字节）</param>
/// <param name="SavedName">保存后的文件名</param>
record FileUploadResult(string OriginalName, string ContentType, long Size, string SavedName);

/// <summary>
/// 带信息的文件上传结果
/// </summary>
record FileUploadWithInfoResult(string OriginalName, string ContentType, long Size, string SavedName, string Description, string Category);

/// <summary>
/// 头像上传结果
/// </summary>
/// <param name="UserId">用户ID</param>
/// <param name="AvatarUrl">头像URL</param>
/// <param name="Size">文件大小</param>
record AvatarUploadResult(int UserId, string AvatarUrl, long Size);

// ============================================
// 表单提交相关模型
// ============================================

/// <summary>
/// 联系表单请求
/// </summary>
/// <param name="name">姓名</param>
/// <param name="email">邮箱</param>
/// <param name="message">留言内容</param>
record ContactFormRequest(string name, string email, string message);

/// <summary>
/// 联系表单提交结果
/// </summary>
record ContactFormResult(string Id, string Name, string Email, string Message, DateTime SubmittedAt);

/// <summary>
/// 反馈表单请求
/// </summary>
/// <param name="rating">评分 1-5</param>
/// <param name="feedback">反馈内容</param>
/// <param name="email">联系邮箱（可选）</param>
record FeedbackFormRequest(int rating, string feedback, string? email);

/// <summary>
/// 反馈表单提交结果
/// </summary>
record FeedbackFormResult(string Id, int Rating, string Feedback, string Email, DateTime SubmittedAt);
