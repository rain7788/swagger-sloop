package io.github.rain7788.swaggersloop;

import org.springframework.boot.autoconfigure.condition.ConditionalOnProperty;
import org.springframework.boot.autoconfigure.condition.ConditionalOnWebApplication;
import org.springframework.boot.context.properties.EnableConfigurationProperties;
import org.springframework.context.annotation.Bean;
import org.springframework.context.annotation.Configuration;
import org.springframework.web.servlet.config.annotation.ResourceHandlerRegistry;
import org.springframework.web.servlet.config.annotation.ViewControllerRegistry;
import org.springframework.web.servlet.config.annotation.WebMvcConfigurer;

/**
 * Auto configuration for SwaggerSloop UI
 *
 * @author SeaCode
 */
@Configuration
@ConditionalOnWebApplication(type = ConditionalOnWebApplication.Type.SERVLET)
@ConditionalOnProperty(name = "swagger-sloop.enabled", havingValue = "true", matchIfMissing = true)
@EnableConfigurationProperties(SwaggerSloopProperties.class)
public class SwaggerSloopAutoConfiguration implements WebMvcConfigurer {

    private final SwaggerSloopProperties properties;

    public SwaggerSloopAutoConfiguration(SwaggerSloopProperties properties) {
        this.properties = properties;
    }

    @Override
    public void addResourceHandlers(ResourceHandlerRegistry registry) {
        String routePrefix = properties.getRoutePrefix().replaceAll("^/|/$", "");

        // Register static resources
        registry.addResourceHandler("/" + routePrefix + "/**")
                .addResourceLocations("classpath:/static/swagger-sloop/")
                .resourceChain(false);
    }

    @Override
    public void addViewControllers(ViewControllerRegistry registry) {
        String routePrefix = properties.getRoutePrefix().replaceAll("^/|/$", "");

        // Redirect to trailing slash
        registry.addRedirectViewController("/" + routePrefix, "/" + routePrefix + "/");
    }

    @Bean
    public SwaggerSloopIndexController swaggerSloopIndexController() {
        return new SwaggerSloopIndexController(properties);
    }
}
