package io.github.rain7788.swaggersloop;

import com.fasterxml.jackson.core.JsonProcessingException;
import com.fasterxml.jackson.databind.ObjectMapper;
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
 * @author SeaCode
 */
@Controller
public class SwaggerSloopIndexController {

    private static final String RESOURCE_PATH = "static/swagger-sloop/";
    private static final ObjectMapper objectMapper = new ObjectMapper();

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
}
