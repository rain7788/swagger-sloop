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

/// <summary>
/// 获取所有用户列表
/// </summary>
/// <remarks>
/// 返回系统中所有注册用户的基本信息，支持分页查询。
/// </remarks>
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

/// <summary>
/// 根据ID获取用户详情
/// </summary>
app.MapGet("/api/users/{id}", (int id) =>
{
    var user = new User(id, "张三", "zhangsan@example.com", "13800138001", DateTime.Now.AddDays(-30), true);
    return Results.Ok(new ApiResponse<User>(true, "获取成功", user));
})
.WithName("GetUserById")
.WithSummary("获取用户详情")
.WithDescription("根据用户ID获取用户的详细信息")
.WithTags("用户管理");

/// <summary>
/// 创建新用户
/// </summary>
app.MapPost("/api/users", (CreateUserRequest request) =>
{
    var user = new User(1, request.Name, request.Email, request.Phone, DateTime.Now, true);
    return Results.Created($"/api/users/{user.Id}", new ApiResponse<User>(true, "创建成功", user));
})
.WithName("CreateUser")
.WithSummary("创建用户")
.WithDescription("创建一个新的用户账户")
.WithTags("用户管理");

/// <summary>
/// 更新用户信息
/// </summary>
app.MapPut("/api/users/{id}", (int id, UpdateUserRequest request) =>
{
    var user = new User(id, request.Name, request.Email, request.Phone, DateTime.Now.AddDays(-30), request.IsActive);
    return Results.Ok(new ApiResponse<User>(true, "更新成功", user));
})
.WithName("UpdateUser")
.WithSummary("更新用户")
.WithDescription("更新指定用户的信息")
.WithTags("用户管理");

/// <summary>
/// 删除用户
/// </summary>
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
