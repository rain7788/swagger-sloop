# SwaggerSloop for Java

🎨 A beautiful Swagger UI skin inspired by **Art-Design-Pro** with **knife4j-like** features.

Modern, elegant, and feature-rich API documentation interface for Java Spring Boot applications.

## ✨ Features

- 🎨 **Art-Design-Pro Style** - Modern UI with OKLCH color system, smooth animations
- 🌙 **Dark/Light Theme** - Auto-detect system preference with manual toggle
- 🔍 **Global Search** - Quick search across all APIs (Ctrl+K)
- 📋 **Copy Code** - One-click copy for cURL, JavaScript, Java code snippets
- 📑 **API Groups** - Easy switch between multiple API groups
- 📱 **Responsive** - Works on desktop, tablet, and mobile
- ⚡ **Lightweight** - Pure vanilla JS/CSS, zero dependencies
- 🔧 **Customizable** - Easy to customize colors and layout
- 🚀 **Auto Configuration** - Works out of the box with Spring Boot

## 📦 Installation

### Maven

```xml
<dependency>
    <groupId>io.github.rain7788</groupId>
    <artifactId>swagger-sloop-spring-boot-starter</artifactId>
    <version>1.0.0</version>
</dependency>
```

### Gradle

```groovy
implementation 'io.github.rain7788:swagger-sloop-spring-boot-starter:1.0.0'
```

## 🚀 Quick Start

Just add the dependency and it works! SwaggerSloop auto-configures itself with Spring Boot.

Make sure you have SpringDoc OpenAPI in your project:

```xml
<!-- For Spring Boot 2.x -->
<dependency>
    <groupId>org.springdoc</groupId>
    <artifactId>springdoc-openapi-ui</artifactId>
    <version>1.8.0</version>
</dependency>

<!-- For Spring Boot 3.x -->
<dependency>
    <groupId>org.springdoc</groupId>
    <artifactId>springdoc-openapi-starter-webmvc-ui</artifactId>
    <version>2.3.0</version>
</dependency>
```

Then access your API docs at: `http://localhost:8080/swagger/`

## ⚙️ Configuration

### application.properties

```properties
# Enable/disable SwaggerSloop
swagger-sloop.enabled=true

# Page title
swagger-sloop.document-title=My API Documentation

# Route prefix (default: swagger)
swagger-sloop.route-prefix=swagger

# Theme color
swagger-sloop.primary-color=#5D87FF

# Default theme (auto, light, dark)
swagger-sloop.default-theme=auto

# Features
swagger-sloop.enable-search=true
swagger-sloop.enable-code-copy=true

# Multiple API endpoints
swagger-sloop.swagger-endpoints[0].url=/v3/api-docs
swagger-sloop.swagger-endpoints[0].name=API V1
swagger-sloop.swagger-endpoints[1].url=/v3/api-docs/v2
swagger-sloop.swagger-endpoints[1].name=API V2
```

### application.yml

```yaml
swagger-sloop:
  enabled: true
  document-title: My API Documentation
  route-prefix: swagger
  primary-color: "#5D87FF"
  default-theme: auto
  enable-search: true
  enable-code-copy: true
  swagger-endpoints:
    - url: /v3/api-docs
      name: API V1
    - url: /v3/api-docs/v2
      name: API V2
```

## 🎨 Themes

| Theme   | Description                                |
| ------- | ------------------------------------------ |
| `light` | Clean white background with subtle shadows |
| `dark`  | Dark mode with reduced eye strain          |
| `auto`  | Follows system preference (default)        |

### Custom Colors

```properties
swagger-sloop.primary-color=#5D87FF  # Blue (default)
swagger-sloop.primary-color=#B48DF3  # Purple
swagger-sloop.primary-color=#60C041  # Green
swagger-sloop.primary-color=#F9901F  # Orange
```

## ⌨️ Keyboard Shortcuts

| Shortcut       | Action                  |
| -------------- | ----------------------- |
| `Ctrl/Cmd + K` | Open global search      |
| `Escape`       | Close dialogs           |
| `↑ / ↓`        | Navigate search results |
| `Enter`        | Select search result    |

## 🔧 Advanced Usage

### Custom CSS/JavaScript

```properties
swagger-sloop.additional-stylesheets[0]=/css/custom-swagger.css
swagger-sloop.additional-scripts[0]=/js/custom-swagger.js
```

### Disable Default SpringDoc UI

If you want to use only SwaggerSloop UI:

```properties
springdoc.swagger-ui.enabled=false
```

## 📦 Supported Versions

- Java 8+
- Spring Boot 2.7.x
- Spring Boot 3.x (with Java 17+)

## 🔄 Spring Boot 3 Support

For Spring Boot 3.x, use the maven profile:

```bash
mvn clean install -Pspring-boot-3
```

Or configure manually with Java 17+ and SpringDoc 2.x.

## 📄 License

MIT License - feel free to use in your projects!

## 🙏 Credits

- UI Design inspired by [Art-Design-Pro](https://github.com/art-design-pro)
- Features inspired by [knife4j](https://github.com/xiaoymin/knife4j)
- Built on top of [SpringDoc OpenAPI](https://github.com/springdoc/springdoc-openapi)
