package com.example.authdemo.controller;

import cn.dev33.satoken.stp.StpUtil;
import io.swagger.v3.oas.annotations.Operation;
import io.swagger.v3.oas.annotations.Parameter;
import io.swagger.v3.oas.annotations.tags.Tag;
import org.springframework.web.bind.annotation.*;

import java.util.HashMap;
import java.util.Map;

/**
 * 认证控制器
 */
@Tag(name = "认证管理", description = "用户登录、登出相关接口")
@RestController
@RequestMapping("/auth")
public class AuthController {

    @Operation(summary = "用户登录", description = "使用用户名密码登录，返回 token")
    @PostMapping("/login")
    public Map<String, Object> login(
            @Parameter(description = "用户名") @RequestParam String username,
            @Parameter(description = "密码") @RequestParam String password) {

        Map<String, Object> result = new HashMap<>();

        // 简单验证，实际项目中应该查询数据库
        if ("admin".equals(username) && "123456".equals(password)) {
            // Sa-Token 登录，参数为用户 ID
            StpUtil.login(10001);

            result.put("code", 200);
            result.put("message", "登录成功");
            result.put("token", StpUtil.getTokenValue());
        } else {
            result.put("code", 401);
            result.put("message", "用户名或密码错误");
        }

        return result;
    }

    @Operation(summary = "用户登出")
    @PostMapping("/logout")
    public Map<String, Object> logout() {
        StpUtil.logout();

        Map<String, Object> result = new HashMap<>();
        result.put("code", 200);
        result.put("message", "登出成功");
        return result;
    }

    @Operation(summary = "获取验证码", description = "获取图形验证码")
    @GetMapping("/captcha")
    public Map<String, Object> captcha() {
        Map<String, Object> result = new HashMap<>();
        result.put("code", 200);
        result.put("captcha", "ABC123"); // 模拟验证码
        return result;
    }

    @Operation(summary = "检查登录状态")
    @GetMapping("/check")
    public Map<String, Object> checkLogin() {
        Map<String, Object> result = new HashMap<>();
        result.put("code", 200);
        result.put("isLogin", StpUtil.isLogin());
        if (StpUtil.isLogin()) {
            result.put("userId", StpUtil.getLoginId());
            result.put("tokenValue", StpUtil.getTokenValue());
        }
        return result;
    }
}
