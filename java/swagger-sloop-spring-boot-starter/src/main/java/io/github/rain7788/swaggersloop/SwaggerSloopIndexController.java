package io.github.rain7788.swaggersloop;

import com.fasterxml.jackson.core.JsonProcessingException;
import com.fasterxml.jackson.databind.ObjectMapper;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.core.io.ClassPathResource;
import org.springframework.core.io.Resource;
import org.springframework.http.MediaType;
import org.springframework.http.ResponseEntity;
import org.springframework.stereotype.Controller;
import org.springframework.util.StreamUtils;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.ResponseBody;

import java.io.IOException;
import java.nio.charset.StandardCharsets;
import java.util.*;
import java.util.stream.Collectors;

/**
 * Controller for serving SwaggerSloop index.html with dynamic configuration
 *
 * @author rain7788
 */
@Controller
public class SwaggerSloopIndexController {

    private static final Logger log = LoggerFactory.getLogger(SwaggerSloopIndexController.class);
    private static final String RESOURCE_PATH = "static/swagger-sloop/";
    private static final ObjectMapper objectMapper = new ObjectMapper();

    // Cached inline resources (loaded once on first request when inline mode is
    // enabled)
    private volatile String cachedCss = null;
    private volatile String cachedJs = null;

    private final SwaggerSloopProperties properties;

    public SwaggerSloopIndexController(SwaggerSloopProperties properties) {
        this.properties = properties;
    }

    @GetMapping(value = { "/${swagger-sloop.route-prefix:swagger}/",
            "/${swagger-sloop.route-prefix:swagger}/index.html" }, produces = MediaType.TEXT_HTML_VALUE)
    @ResponseBody
    public ResponseEntity<String> index() throws IOException {
        Resource resource = new ClassPathResource(RESOURCE_PATH + "index.html");
        if (!resource.exists()) {
            return ResponseEntity.notFound().build();
        }

        String html = StreamUtils.copyToString(resource.getInputStream(), StandardCharsets.UTF_8);

        // Build URLs JSON
        List<SwaggerSloopProperties.SwaggerEndpoint> endpoints = properties.getSwaggerEndpoints();
        if (endpoints == null || endpoints.isEmpty()) {
            endpoints = Collections.singletonList(
                    new SwaggerSloopProperties.SwaggerEndpoint("/v3/api-docs", "API V1"));
        }

        String urlsJson;
        try {
            List<Map<String, String>> urlList = endpoints.stream()
                    .map(e -> {
                        Map<String, String> map = new HashMap<>();
                        map.put("url", e.getUrl());
                        map.put("name", e.getName());
                        return map;
                    })
                    .collect(Collectors.toList());
            urlsJson = objectMapper.writeValueAsString(urlList);
        } catch (JsonProcessingException e) {
            urlsJson = "[]";
        }

        // Replace placeholders
        String version = String.valueOf(System.currentTimeMillis());
        html = html
                .replace("%(DocumentTitle)", escapeHtml(properties.getDocumentTitle()))
                .replace("%(PrimaryColor)", escapeHtml(properties.getPrimaryColor()))
                .replace("%(DefaultTheme)", properties.getDefaultTheme().toLowerCase())
                .replace("%(EnableSearch)", String.valueOf(properties.isEnableSearch()))
                .replace("%(EnableCodeCopy)", String.valueOf(properties.isEnableCodeCopy()))
                .replace("%(Version)", version)
                .replace("%(Urls)", urlsJson);

        // Inline resources if enabled (allows /swagger/* instead of /swagger/**)
        if (properties.isInlineResources()) {
            html = inlineResources(html);
        }

        // Inject additional stylesheets
        if (properties.getAdditionalStylesheets() != null && !properties.getAdditionalStylesheets().isEmpty()) {
            StringBuilder stylesheetTags = new StringBuilder();
            for (String stylesheet : properties.getAdditionalStylesheets()) {
                stylesheetTags.append("    <link rel=\"stylesheet\" href=\"")
                        .append(escapeHtml(stylesheet))
                        .append("\">\n");
            }
            html = html.replace("</head>", stylesheetTags + "</head>");
        }

        // Inject additional scripts
        if (properties.getAdditionalScripts() != null && !properties.getAdditionalScripts().isEmpty()) {
            StringBuilder scriptTags = new StringBuilder();
            for (String script : properties.getAdditionalScripts()) {
                scriptTags.append("    <script src=\"")
                        .append(escapeHtml(script))
                        .append("\"></script>\n");
            }
            html = html.replace("</body>", scriptTags + "</body>");
        }

        return ResponseEntity.ok()
                .contentType(MediaType.TEXT_HTML)
                .body(html);
    }

    private String escapeHtml(String value) {
        if (value == null)
            return "";
        return value
                .replace("&", "&amp;")
                .replace("<", "&lt;")
                .replace(">", "&gt;")
                .replace("\"", "&quot;")
                .replace("'", "&#39;");
    }

    /**
     * Inline CSS and JS resources into the HTML.
     * This allows users to use /swagger/* instead of /swagger/** in their
     * interceptor exclusions.
     */
    private String inlineResources(String html) {
        try {
            // Load and cache CSS
            if (cachedCss == null) {
                Resource cssResource = new ClassPathResource(RESOURCE_PATH + "swagger-sloop.css");
                if (cssResource.exists()) {
                    cachedCss = StreamUtils.copyToString(cssResource.getInputStream(), StandardCharsets.UTF_8);
                    log.debug("SwaggerSloop: CSS resource loaded and cached ({} bytes)", cachedCss.length());
                }
            }

            // Load and cache JS
            if (cachedJs == null) {
                Resource jsResource = new ClassPathResource(RESOURCE_PATH + "swagger-sloop.js");
                if (jsResource.exists()) {
                    cachedJs = StreamUtils.copyToString(jsResource.getInputStream(), StandardCharsets.UTF_8);
                    log.debug("SwaggerSloop: JS resource loaded and cached ({} bytes)", cachedJs.length());
                }
            }

            // Replace external CSS link with inline style
            if (cachedCss != null) {
                html = html.replaceFirst(
                        "<link rel=\"stylesheet\" href=\"\\./swagger-sloop\\.css[^\"]*\">",
                        "<style>\n" + cachedCss + "\n</style>");
            }

            // Replace external JS script with inline script
            if (cachedJs != null) {
                html = html.replaceFirst(
                        "<script src=\"\\./swagger-sloop\\.js[^\"]*\"></script>",
                        "<script>\n" + cachedJs + "\n</script>");
            }

            log.debug("SwaggerSloop: Resources inlined successfully");
        } catch (IOException e) {
            log.warn("SwaggerSloop: Failed to inline resources, falling back to external files", e);
        }

        return html;
    }
}
