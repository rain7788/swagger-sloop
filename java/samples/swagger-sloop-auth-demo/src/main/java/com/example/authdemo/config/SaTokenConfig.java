package com.example.authdemo.config;

import cn.dev33.satoken.interceptor.SaInterceptor;
import cn.dev33.satoken.stp.StpUtil;
import org.springframework.context.annotation.Configuration;
import org.springframework.web.servlet.config.annotation.InterceptorRegistry;
import org.springframework.web.servlet.config.annotation.WebMvcConfigurer;

/**
 * Sa-Token 拦截器配置
 * 
 * 【演示】使用 inline-resources=true 模式，只需要 /swagger/* 即可！
 * 
 * 当启用 swagger-sloop.inline-resources=true 时：
 * - CSS 和 JS 会被内联到 HTML 中
 * - 只需要放行 /swagger/* 即可（单层匹配）
 * - 减少 HTTP 请求数量（3个变1个）
 * 
 * 如果未启用 inline-resources，则需要使用 /swagger/** 匹配所有子路径
 */
@Configuration
public class SaTokenConfig implements WebMvcConfigurer {

    @Override
    public void addInterceptors(InterceptorRegistry registry) {
        // 注册 Sa-Token 拦截器，打开注解式鉴权功能
        registry.addInterceptor(new SaInterceptor(handle -> StpUtil.checkLogin()))
                // 拦截所有路径
                .addPathPatterns("/**")

                // ========== 认证相关路径 ==========
                .excludePathPatterns("/auth/login")
                .excludePathPatterns("/auth/captcha")

                // ========== SwaggerSloop UI 路径 ==========
                // 使用 inline-resources=true 时，只需要 /swagger/* 即可！
                // 如果未启用 inline-resources，需要改成 /swagger/**
                .excludePathPatterns("/swagger/*")

                // ========== SpringDoc OpenAPI 路径 ==========
                // API 文档 JSON 端点
                .excludePathPatterns("/v3/api-docs")
                .excludePathPatterns("/v3/api-docs/**")
                // Swagger 配置端点
                .excludePathPatterns("/v3/api-docs/swagger-config")

                // ========== 原生 Swagger UI (如果启用) ==========
                .excludePathPatterns("/swagger-ui/**")
                .excludePathPatterns("/swagger-ui.html")

                // ========== WebJars 静态资源 ==========
                .excludePathPatterns("/webjars/**")

                // ========== 其他静态资源 ==========
                .excludePathPatterns("/favicon.ico")
                .excludePathPatterns("/doc.html");
    }
}
