package io.github.rain7788.swaggersloop;

import org.springframework.boot.context.properties.ConfigurationProperties;

import java.util.ArrayList;
import java.util.List;

/**
 * SwaggerSloop configuration properties
 *
 * @author rain7788
 */
@ConfigurationProperties(prefix = "swagger-sloop")
public class SwaggerSloopProperties {

    /**
     * Enable SwaggerSloop UI
     */
    private boolean enabled = true;

    /**
     * The title of the documentation page
     */
    private String documentTitle = "API Documentation";

    /**
     * The route prefix for accessing the swagger UI (e.g., "docs" means /docs)
     */
    private String routePrefix = "swagger";

    /**
     * The primary color for the UI theme (hex color code, e.g., "#5D87FF")
     */
    private String primaryColor = "#5D87FF";

    /**
     * The default theme (light, dark, auto)
     */
    private String defaultTheme = "auto";

    /**
     * Enable global search feature (Ctrl+K)
     */
    private boolean enableSearch = true;

    /**
     * Enable code copy feature
     */
    private boolean enableCodeCopy = true;

    /**
     * List of Swagger JSON endpoints
     */
    private List<SwaggerEndpoint> swaggerEndpoints = new ArrayList<>();

    /**
     * Additional CSS files to inject
     */
    private List<String> additionalStylesheets = new ArrayList<>();

    /**
     * Additional JavaScript files to inject
     */
    private List<String> additionalScripts = new ArrayList<>();

    /**
     * Inline CSS and JS resources into HTML.
     * When enabled, only /swagger/* path pattern is needed for interceptor
     * exclusion.
     * When disabled (default), /swagger/** pattern is required.
     */
    private boolean inlineResources = false;

    /**
     * OAuth2 client ID (for OAuth authentication)
     */
    private String oauthClientId;

    /**
     * OAuth2 client secret (for OAuth authentication)
     */
    private String oauthClientSecret;

    // Getters and Setters

    public boolean isEnabled() {
        return enabled;
    }

    public void setEnabled(boolean enabled) {
        this.enabled = enabled;
    }

    public String getDocumentTitle() {
        return documentTitle;
    }

    public void setDocumentTitle(String documentTitle) {
        this.documentTitle = documentTitle;
    }

    public String getRoutePrefix() {
        return routePrefix;
    }

    public void setRoutePrefix(String routePrefix) {
        this.routePrefix = routePrefix;
    }

    public String getPrimaryColor() {
        return primaryColor;
    }

    public void setPrimaryColor(String primaryColor) {
        this.primaryColor = primaryColor;
    }

    public String getDefaultTheme() {
        return defaultTheme;
    }

    public void setDefaultTheme(String defaultTheme) {
        this.defaultTheme = defaultTheme;
    }

    public boolean isEnableSearch() {
        return enableSearch;
    }

    public void setEnableSearch(boolean enableSearch) {
        this.enableSearch = enableSearch;
    }

    public boolean isEnableCodeCopy() {
        return enableCodeCopy;
    }

    public void setEnableCodeCopy(boolean enableCodeCopy) {
        this.enableCodeCopy = enableCodeCopy;
    }

    public List<SwaggerEndpoint> getSwaggerEndpoints() {
        return swaggerEndpoints;
    }

    public void setSwaggerEndpoints(List<SwaggerEndpoint> swaggerEndpoints) {
        this.swaggerEndpoints = swaggerEndpoints;
    }

    public List<String> getAdditionalStylesheets() {
        return additionalStylesheets;
    }

    public void setAdditionalStylesheets(List<String> additionalStylesheets) {
        this.additionalStylesheets = additionalStylesheets;
    }

    public List<String> getAdditionalScripts() {
        return additionalScripts;
    }

    public void setAdditionalScripts(List<String> additionalScripts) {
        this.additionalScripts = additionalScripts;
    }

    public boolean isInlineResources() {
        return inlineResources;
    }

    public void setInlineResources(boolean inlineResources) {
        this.inlineResources = inlineResources;
    }

    public String getOauthClientId() {
        return oauthClientId;
    }

    public void setOauthClientId(String oauthClientId) {
        this.oauthClientId = oauthClientId;
    }

    public String getOauthClientSecret() {
        return oauthClientSecret;
    }

    public void setOauthClientSecret(String oauthClientSecret) {
        this.oauthClientSecret = oauthClientSecret;
    }

    /**
     * Swagger endpoint configuration
     */
    public static class SwaggerEndpoint {
        private String url;
        private String name;

        public SwaggerEndpoint() {
        }

        public SwaggerEndpoint(String url, String name) {
            this.url = url;
            this.name = name;
        }

        public String getUrl() {
            return url;
        }

        public void setUrl(String url) {
            this.url = url;
        }

        public String getName() {
            return name;
        }

        public void setName(String name) {
            this.name = name;
        }
    }
}
