package com.example.authdemo.controller;

import io.swagger.v3.oas.annotations.Operation;
import io.swagger.v3.oas.annotations.Parameter;
import io.swagger.v3.oas.annotations.tags.Tag;
import org.springframework.web.bind.annotation.*;

import java.util.*;

/**
 * 用户管理控制器 - 需要登录才能访问
 */
@Tag(name = "用户管理", description = "用户 CRUD 操作（需要登录）")
@RestController
@RequestMapping("/api/users")
public class UserController {

    // 模拟数据库
    private static final Map<Long, Map<String, Object>> USER_DB = new HashMap<>();
    private static long idCounter = 1;

    static {
        // 初始化一些测试数据
        Map<String, Object> user1 = new HashMap<>();
        user1.put("id", 1L);
        user1.put("name", "张三");
        user1.put("email", "zhangsan@example.com");
        user1.put("age", 28);
        USER_DB.put(1L, user1);

        Map<String, Object> user2 = new HashMap<>();
        user2.put("id", 2L);
        user2.put("name", "李四");
        user2.put("email", "lisi@example.com");
        user2.put("age", 32);
        USER_DB.put(2L, user2);

        idCounter = 3;
    }

    @Operation(summary = "获取用户列表")
    @GetMapping
    public Map<String, Object> list(
            @Parameter(description = "页码") @RequestParam(defaultValue = "1") int page,
            @Parameter(description = "每页数量") @RequestParam(defaultValue = "10") int size) {

        Map<String, Object> result = new HashMap<>();
        result.put("code", 200);
        result.put("data", new ArrayList<>(USER_DB.values()));
        result.put("total", USER_DB.size());
        result.put("page", page);
        result.put("size", size);
        return result;
    }

    @Operation(summary = "获取单个用户")
    @GetMapping("/{id}")
    public Map<String, Object> get(@Parameter(description = "用户ID") @PathVariable Long id) {
        Map<String, Object> result = new HashMap<>();

        if (USER_DB.containsKey(id)) {
            result.put("code", 200);
            result.put("data", USER_DB.get(id));
        } else {
            result.put("code", 404);
            result.put("message", "用户不存在");
        }
        return result;
    }

    @Operation(summary = "创建用户")
    @PostMapping
    public Map<String, Object> create(
            @Parameter(description = "用户名") @RequestParam String name,
            @Parameter(description = "邮箱") @RequestParam String email,
            @Parameter(description = "年龄") @RequestParam(required = false) Integer age) {

        Map<String, Object> user = new HashMap<>();
        user.put("id", idCounter);
        user.put("name", name);
        user.put("email", email);
        user.put("age", age != null ? age : 0);

        USER_DB.put(idCounter, user);
        idCounter++;

        Map<String, Object> result = new HashMap<>();
        result.put("code", 200);
        result.put("message", "创建成功");
        result.put("data", user);
        return result;
    }

    @Operation(summary = "更新用户")
    @PutMapping("/{id}")
    public Map<String, Object> update(
            @Parameter(description = "用户ID") @PathVariable Long id,
            @Parameter(description = "用户名") @RequestParam(required = false) String name,
            @Parameter(description = "邮箱") @RequestParam(required = false) String email,
            @Parameter(description = "年龄") @RequestParam(required = false) Integer age) {

        Map<String, Object> result = new HashMap<>();

        if (!USER_DB.containsKey(id)) {
            result.put("code", 404);
            result.put("message", "用户不存在");
            return result;
        }

        Map<String, Object> user = USER_DB.get(id);
        if (name != null)
            user.put("name", name);
        if (email != null)
            user.put("email", email);
        if (age != null)
            user.put("age", age);

        result.put("code", 200);
        result.put("message", "更新成功");
        result.put("data", user);
        return result;
    }

    @Operation(summary = "删除用户")
    @DeleteMapping("/{id}")
    public Map<String, Object> delete(@Parameter(description = "用户ID") @PathVariable Long id) {
        Map<String, Object> result = new HashMap<>();

        if (USER_DB.containsKey(id)) {
            USER_DB.remove(id);
            result.put("code", 200);
            result.put("message", "删除成功");
        } else {
            result.put("code", 404);
            result.put("message", "用户不存在");
        }
        return result;
    }
}
