package com.example.authdemo;

import org.springframework.boot.SpringApplication;
import org.springframework.boot.autoconfigure.SpringBootApplication;

/**
 * Demo application demonstrating SwaggerSloop with Sa-Token authentication
 * 
 * 这个 demo 模拟用户的使用场景：
 * 1. 使用 Sa-Token 进行认证拦截
 * 2. 需要正确排除 Swagger 相关路径
 */
@SpringBootApplication
public class AuthDemoApplication {

    public static void main(String[] args) {
        SpringApplication.run(AuthDemoApplication.class, args);
    }
}
