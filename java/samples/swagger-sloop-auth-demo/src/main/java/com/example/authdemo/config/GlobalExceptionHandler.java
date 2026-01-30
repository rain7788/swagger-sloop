package com.example.authdemo.config;

import cn.dev33.satoken.exception.NotLoginException;
import cn.dev33.satoken.exception.NotPermissionException;
import cn.dev33.satoken.exception.NotRoleException;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.http.HttpStatus;
import org.springframework.web.bind.annotation.ExceptionHandler;
import org.springframework.web.bind.annotation.ResponseStatus;
import org.springframework.web.bind.annotation.RestControllerAdvice;

/**
 * 全局异常处理器
 * 
 * 处理 Sa-Token 认证异常，返回友好的 JSON 响应而不是 500 错误
 */
@RestControllerAdvice
public class GlobalExceptionHandler {

    private static final Logger log = LoggerFactory.getLogger(GlobalExceptionHandler.class);

    /**
     * 处理未登录异常
     */
    @ExceptionHandler(NotLoginException.class)
    @ResponseStatus(HttpStatus.UNAUTHORIZED)
    public ApiError handleNotLoginException(NotLoginException e) {
        log.debug("未登录访问: {}", e.getMessage());
        return new ApiError(401, "未登录，请先登录", e.getMessage());
    }

    /**
     * 处理无权限异常
     */
    @ExceptionHandler(NotPermissionException.class)
    @ResponseStatus(HttpStatus.FORBIDDEN)
    public ApiError handleNotPermissionException(NotPermissionException e) {
        log.debug("无权限访问: {}", e.getMessage());
        return new ApiError(403, "无权限访问", e.getMessage());
    }

    /**
     * 处理无角色异常
     */
    @ExceptionHandler(NotRoleException.class)
    @ResponseStatus(HttpStatus.FORBIDDEN)
    public ApiError handleNotRoleException(NotRoleException e) {
        log.debug("无角色访问: {}", e.getMessage());
        return new ApiError(403, "无角色权限", e.getMessage());
    }

    /**
     * 处理其他异常
     */
    @ExceptionHandler(Exception.class)
    @ResponseStatus(HttpStatus.INTERNAL_SERVER_ERROR)
    public ApiError handleException(Exception e) {
        log.error("服务器内部错误", e);
        return new ApiError(500, "服务器内部错误", e.getMessage());
    }
}
